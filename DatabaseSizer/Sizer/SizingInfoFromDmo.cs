using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DatabaseSizer.SizingMetaData;
using DatabaseSizer.SqlDataTypes;
using Microsoft.SqlServer.Management.Smo;
using SqlDataType = DatabaseSizer.SqlDataTypes.SqlDataType;

namespace DatabaseSizer.Sizer
{
    public delegate void ProgressDelegate();

    public class SizingInfoFromDmo
    {
        private readonly IColumnLengthService _columnLengthService;

        public SizingInfoFromDmo(IColumnLengthService columnLengthService)
        {
            this._columnLengthService = columnLengthService;
        }

        public SqlDatabase CreateSqlDatabaseFromSMO(Database smoDb, Action progressCallback,
                                                    CancellationToken cancellationToken)
        {
            return new SqlDatabase(smoDb.Name, this.SqlTablesFromSmoDatabase(smoDb, progressCallback, cancellationToken));
        }

        private IList<SqlTable> SqlTablesFromSmoDatabase(Database smoDb, Action progressCallback,
                                                         CancellationToken cancellationToken)
        {
            smoDb.Tables.Refresh();
            return smoDb.Tables.Cast<Table>().Select(tbl =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (progressCallback != null)
                        progressCallback();

                    var fullTableName = string.Format("{0}.{1}", tbl.Schema, tbl.Name);
                    return new SqlTable(fullTableName, this.SqlColumnsFromSmoTable(smoDb, tbl),
                                        this.SqlIndexesFromSmoTable(smoDb, tbl));
                }).ToList();
        }

        private IList<SqlColumn> SqlColumnsFromSmoTable(Database db, Table tbl)
        {
            tbl.Columns.Refresh();
            return
                tbl.Columns.Cast<Column>().Select(col => this.DmoColumnToSqlColumn(db, tbl, col)).ToList();
        }

        private SqlColumn DmoColumnToSqlColumn(Database db, Table tbl, Column col)
        {
            var dataTypeSizingMetaData = GetSqlDataType(db, col);

            var avgSize = 0L;
            var avgBinarySize = 0L;

            switch (dataTypeSizingMetaData.StorageCharacteristics)
            {
                case StorageCharacteristicsEnum.Fixed:
                    avgSize = col.DataType.MaximumLength;
                    break;

                case StorageCharacteristicsEnum.FixedByLength:
                    avgSize = col.DataType.MaximumLength;
                    break;

                case StorageCharacteristicsEnum.Variable:
                    if (dataTypeSizingMetaData.StoredExternal)
                    {
                        avgSize = this._columnLengthService.GetAvgLengthForExternalVariableColumn(tbl, col);
                        avgBinarySize = this._columnLengthService.GetAvgBinaryLengthForExternalVariableColumn(tbl, col);
                    }
                    else
                    {
                        avgSize = this._columnLengthService.GetAvgLengthForVariableColumn(tbl, col);
                        if (avgSize == 0)
                            avgSize = (col.DataType.MaximumLength/2);
                    }
                    break;

                case StorageCharacteristicsEnum.Variant:
                    avgSize = this._columnLengthService.GetAvgDataLengthForVariantColumn(tbl, col);
                    if (avgSize == 0)
                        avgSize = (col.DataType.MaximumLength/2);
                    break;
            }

            return new SqlColumn(col.Name, dataTypeSizingMetaData, col.DataType.MaximumLength, avgSize, avgBinarySize);
        }

        private static SqlDataType GetSqlDataType(Database db, Column col)
        {
            var dataTypeSizingMetaData =
                SqlDataTypeFactory.GetDataTypeByName(col.DataType.SqlDataType.ToString());

            if (dataTypeSizingMetaData == null)
            {
                db.UserDefinedDataTypes.Refresh();
                var udt =
                    db.UserDefinedDataTypes.Cast<UserDefinedDataType>()
                      .SingleOrDefault(dt => dt.Name == col.DataType.Name);
                
                if (udt == null)
                {
                    throw new NullReferenceException(string.Format("Cannot find user defined data type {0}", col.DataType.Name));
                }    
                    
                dataTypeSizingMetaData = SqlDataTypeFactory.GetDataTypeByName(udt.SystemType);
            }

            return dataTypeSizingMetaData;
        }

        private IList<SqlIndex> SqlIndexesFromSmoTable(Database db, Table tbl)
        {
            tbl.Indexes.Refresh();
            return tbl.Indexes.Cast<Index>().Select(idx =>
                {
                    var tableColumns = tbl.Columns.Cast<Column>();
                    idx.IndexedColumns.Refresh();
                    return new SqlIndex(idx.Name, idx.IsClustered,
                                        idx.IndexedColumns.Cast<IndexedColumn>()
                                           .Select(
                                               col =>
                                               this.DmoColumnToSqlColumn(db, tbl,
                                                                         tableColumns.Single(tc => tc.Name == col.Name)))
                                           .ToList());
                }).ToList();
        }
    }
}