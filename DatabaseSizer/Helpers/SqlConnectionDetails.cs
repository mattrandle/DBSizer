namespace DatabaseSizer.Helpers
{
    /// <summary>
    ///     Stores connection details used to connect to SQL Server instance
    ///     Immutable
    /// </summary>
    public sealed class SqlConnectionDetails
    {
        /// <summary>
        ///     Default database
        /// </summary>
        public readonly string DefaultDatabase;

        /// <summary>
        ///     Password for SQL Server authentication
        /// </summary>
        public readonly string Password;

        /// <summary>
        ///     The SQL Server Instance to connect to
        /// </summary>
        public readonly string ServerName;

        /// <summary>
        ///     Use integrated Windows authentication
        /// </summary>
        public readonly bool? UseSqlAuthentication;

        /// <summary>
        ///     Username for SQL Server authentication
        /// </summary>
        public readonly string Username;

        /// <summary>
        ///     Integrated authentication with default db
        /// </summary>
        public SqlConnectionDetails(string serverName, string databaseName)
            : this(serverName, false, "", "", databaseName)
        {
        }

        /// <summary>
        ///     Integrated authentication without default db
        /// </summary>
        public SqlConnectionDetails(string serverName)
            : this(serverName, false, "", "")
        {
        }

        /// <summary>
        /// </summary>
        public SqlConnectionDetails(string serverName, bool? useSqlAuthentication, string userName, string password)
        {
            this.ServerName = serverName;
            this.UseSqlAuthentication = useSqlAuthentication;
            this.Username = userName;
            this.Password = password;
        }

        public SqlConnectionDetails(string serverName, bool? useSqlAuthentication, string userName, string password,
                                    string defaultDatabase)
        {
            this.ServerName = serverName;
            this.UseSqlAuthentication = useSqlAuthentication;
            this.Username = userName;
            this.Password = password;
            this.DefaultDatabase = defaultDatabase;
        }

        public SqlConnectionDetails(string serverName, string userName, string password, string defaultDb)
        {
            this.ServerName = serverName;
            this.UseSqlAuthentication = false;
            this.Username = userName;
            this.Password = password;
            this.DefaultDatabase = defaultDb;
        }

        public string ConnectionString
        {
            get
            {
                var result =
                    "Data Source=" + this.ServerName + ";" +
                    "Persist Security Info=False;";

                if (this.UseSqlAuthentication.HasValue && this.UseSqlAuthentication.Value)
                    result +=
                        "Integrated Security=false;" +
                        "User ID=" + this.Username + ";" +
                        "Password=" + this.Password;
                else
                    result += "Integrated Security=true";

                if (!string.IsNullOrEmpty(this.DefaultDatabase))
                    result += ";Initial Catalog=" + this.DefaultDatabase;

                return result;
            }
        }
    }
}