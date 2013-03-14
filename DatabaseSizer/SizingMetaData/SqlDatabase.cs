using System.Collections.Generic;

namespace DatabaseSizer.SizingMetaData
{
    public class SqlDatabase : SizingItem
    {
        public SqlDatabase(string databaseName, IList<SqlTable> tables) : base(databaseName)
        {
            this.Tables = tables;
        }

        public IList<SqlTable> Tables { get; private set; }
    }
}