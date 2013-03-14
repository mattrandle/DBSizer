using System.Collections.Generic;

namespace DatabaseSizer.SizingMetaData
{
    public class SqlTable : ColumnOwnerBase
    {
        public SqlTable(string name, IList<SqlColumn> columns, IList<SqlIndex> indexes) : base(name, columns)
        {
            this.Indexes = indexes;
        }

        public IList<SqlIndex> Indexes { get; private set; }
    }
}