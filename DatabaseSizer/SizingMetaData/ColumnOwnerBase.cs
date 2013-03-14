using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseSizer.SizingMetaData
{
    /// <summary>
    ///     Parent class for tables and indexes (both collections of columns)
    /// </summary>
    public class ColumnOwnerBase : SizingItem
    {
        private const int StaticPerRow = 4;
        private const int PageSize = 8096;

        public ColumnOwnerBase(string name, IList<SqlColumn> columns) : base(name)
        {
            this.Columns = columns;
        }

        public IList<SqlColumn> Columns { get; private set; }

        public long RowSize
        {
            get { return this.FixedColStorageSize + this.VarColStorageSize + this.NullBitmap + StaticPerRow; }
        }

        public int NullBitmap
        {
            get { return Convert.ToInt32(2 + ((this.Columns.Count() + 7)/8)); }
        }

        public long FixedColStorageSize
        {
            get { return this.Columns.Where(a => a.DataTypeInfo.IsFixedStorageSize).Sum(a => a.StorageSize); }
        }

        public int VarColCount
        {
            get { return this.Columns.Count(a => !(a.DataTypeInfo.IsFixedStorageSize)); }
        }

        public long VarColStorageSize
        {
            get
            {
                var varCols = this.Columns.Where(a => !(a.DataTypeInfo.IsFixedStorageSize));
                var storageSum = varCols.Sum(a => a.StorageSize);
                return storageSum;
            }
        }

        public long RowsPerPage
        {
            get { return (PageSize/(this.RowSize + 2)); }
        }

        public long FreeRowsPerPage
        {
            get { return PageSize*((100 - this.IndexFillFactor)/100)/(this.RowSize + 2); }
        }

        public int IndexFillFactor
        {
            get
            {
                // Todo - calculate if we have a clustered index on this table
                return 100;
            }
        }

        public long AvgBinaryPerRow
        {
            get { return this.Columns.Sum(a => a.AvgBinarySize); }
        }
    }
}