using System.Collections.Generic;
using DatabaseSizer.Helpers;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseSizer.SMO
{
    public interface ISMOHelper
    {
        IEnumerable<string> GetDatabaseNames(SqlConnectionDetails connectionDetails);
        IEnumerable<string> GetServerList();
        IEnumerable<string> GetTableColumnNames(ServerConnection connection, string databaseName, string tableName);

        IEnumerable<Column> GetTableColumns(SqlConnectionDetails connectionDetails, string databaseName,
                                            string tableName);

        IEnumerable<string> GetTableNames(SqlConnectionDetails connectionDetails, string databaseName);
        void RunScript(ServerConnection connection, string script);
    }
}