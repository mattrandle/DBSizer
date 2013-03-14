using Microsoft.SqlServer.Management.Smo;

namespace DatabaseSizer.Sizer
{
    public interface IColumnLengthService
    {
        long GetAvgBinaryLengthForExternalVariableColumn(Table tbl, Column col);
        long GetAvgDataLengthForVariantColumn(Table tbl, Column col);
        long GetAvgLengthForExternalVariableColumn(Table tbl, Column col);
        long GetAvgLengthForVariableColumn(Table tbl, Column col);
    }
}