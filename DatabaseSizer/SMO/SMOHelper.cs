using System.Collections.Generic;
using System.Data;
using System.Linq;
using DatabaseSizer.Helpers;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseSizer.SMO
{
    public class SMOHelper : ISMOHelper
    {
        public void RunScript(ServerConnection connection, string script)
        {
            var sqlServer = new Server(connection);
            sqlServer.ConnectionContext.ExecuteNonQuery(script);
        }

        public IEnumerable<string> GetServerList()
        {
            var dtSqlServers = SmoApplication.EnumAvailableSqlServers();
            return dtSqlServers.AsEnumerable().Select((server, b) =>
                {
                    var serverName = server["Server"].ToString();

                    if (server["Instance"] != null &&
                        server["Instance"].ToString().Length > 0)
                        serverName += @"\" + server["Instance"].ToString();
                    return serverName;
                }).OrderBy(a => a);
        }

        public IEnumerable<string> GetTableNames(SqlConnectionDetails connectionDetails, string databaseName)
        {
            var sqlServer = new Server(connectionDetails.ServerName);
            var db = new Database(sqlServer, databaseName);
            db.Tables.Refresh();
            var linqList = db.Tables.Cast<Table>();
            return linqList.TakeWhile((a, b) => !a.IsSystemObject).
                            Select((a, b) => a.Name).
                            OrderBy(a => a);
        }

        public IEnumerable<string> GetDatabaseNames(SqlConnectionDetails connectionDetails)
        {
            var sqlServer = new Server(connectionDetails.ServerName);
            var linqList = sqlServer.Databases.Cast<Database>();
            return linqList.Select((a, b) => a.Name.Trim('[', ']')).
                            OrderBy(a => a);
        }

        public IEnumerable<Column> GetTableColumns(SqlConnectionDetails connectionDetails, string databaseName,
                                                   string tableName)
        {
            var sqlServer = new Server(connectionDetails.ServerName);
            var db = new Database(sqlServer, databaseName);
            var table = new Table(db, tableName);
            table.Columns.Refresh();
            return table.Columns.Cast<Column>();
        }

        public IEnumerable<string> GetTableColumnNames(ServerConnection connection, string databaseName,
                                                       string tableName)
        {
            var sqlServer = new Server(connection);
            var db = new Database(sqlServer, databaseName);
            var table = new Table(db, tableName);
            table.Columns.Refresh();
            var linqList = table.Columns.Cast<Column>();
            return linqList.Select((a, b) => a.Name.Trim('[', ']')).
                            OrderBy(a => a);
        }
    }
}