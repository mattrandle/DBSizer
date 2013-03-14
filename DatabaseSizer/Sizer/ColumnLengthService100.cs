using Microsoft.SqlServer.Management.Smo;

namespace DatabaseSizer.Sizer
{
    internal class ColumnLengthService100 : IColumnLengthService
    {
        // This looks odd but its right - we cant estimate average length of bin/var columns so have to set to 0

        public long GetAvgBinaryLengthForExternalVariableColumn(Table tbl, Column col)
        {
            return 0;
        }

        public long GetAvgDataLengthForVariantColumn(Table tbl, Column col)
        {
            return 0;
        }

        public long GetAvgLengthForExternalVariableColumn(Table tbl, Column col)
        {
            return 0;
        }

        public long GetAvgLengthForVariableColumn(Table tbl, Column col)
        {
            return col.DataType.MaximumLength;
        }
    }
}