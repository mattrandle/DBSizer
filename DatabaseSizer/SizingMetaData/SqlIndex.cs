using System.Collections.Generic;

namespace DatabaseSizer.SizingMetaData
{
    public class SqlIndex : ColumnOwnerBase
    {
        public SqlIndex(string indexName, bool isClustered, IList<SqlColumn> columns)
            : base(indexName, columns)
        {
            this.IsClustered = isClustered;
        }

        public bool IsClustered { get; private set; }
    }
}