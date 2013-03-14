using System;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseSizer.Sizer
{
    internal class ColumnLengthServiceActual : IColumnLengthService
    {
        private readonly string _connectionString;

        public ColumnLengthServiceActual(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public long GetAvgDataLengthForVariantColumn(Table tbl, Column col)
        {
            var sql = string.Format(@"
              SELECT 
                CAST(Avg(DataLength([{0}]) AS BIGINT)
              FROM 
                [{1}].[{2}]", col.Name, tbl.Schema, tbl.Name);

            return this.ExecuteSqlAndGetLongResult(sql);
        }

        public long GetAvgLengthForVariableColumn(Table tbl, Column col)
        {
            var sql = string.Format(@"
                SELECT 
                    CAST(Avg(Len([{0}])) AS BIGINT)
                FROM
                    [{1}].[{2}]", col.Name, tbl.Schema, tbl.Name);

            return this.ExecuteSqlAndGetLongResult(sql);
        }

        public long GetAvgLengthForExternalVariableColumn(Table tbl, Column col)
        {
            var sql = string.Format(@"
                SELECT 
                    CAST(COUNT(1) AS BIGINT)
                FROM 
                    [{0}].[{1}]
                WHERE 
                    [{2}] IS NOT NULL", tbl.Schema, tbl.Name, col.Name);

            var nonNullCount = this.ExecuteSqlAndGetLongResult(sql);

            sql = string.Format(@"
                SELECT 
                    CAST(COUNT(1) AS BIGINT)
                FROM 
                    [{0}].[{1}]", tbl.Schema, tbl.Name);

            var totalCount = this.ExecuteSqlAndGetLongResult(sql);

            var result = 0L;
            if (totalCount > 0)
                result = (nonNullCount*col.DataType.MaximumLength)/totalCount;

            return result;
        }

        public long GetAvgBinaryLengthForExternalVariableColumn(Table tbl, Column col)
        {
            var sql = string.Format(@"
                SELECT 
                    CAST(Avg(DataLength([{0}])) AS BIGINT)
                FROM 
                    [{1}].[{2}]", col.Name, tbl.Schema, tbl.Name);

            return this.ExecuteSqlAndGetLongResult(sql);
        }

        private long ExecuteSqlAndGetLongResult(string sql)
        {
            var actualResult = 0L;
            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                var sqlResult = command.ExecuteScalar();
                if (!(sqlResult is DBNull))
                    actualResult = (long) sqlResult;
            }
            return actualResult;
        }
    }
}