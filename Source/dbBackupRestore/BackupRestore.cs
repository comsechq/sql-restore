//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.IO;

//namespace dbBackupRestore
//{
//    public static class BackupRestore
//    {
//        static void Main(string[] args)
//        {
//            var filePaths = Directory.GetFiles(@"C:\SQL_Backup\Current\", "*.bak");
//            var backupsToRestore = new Dictionary<string, DateTime>();
//            var backupPaths = new Dictionary<string, string>();
//            long lastSize = 0;

//            foreach(var file in filePaths)
//            {
//                var fi = new FileInfo(file);
//                var l = fi.Name.IndexOf("_backup");
//                var dbName = fi.Name.Substring(0, l);

//                if (backupsToRestore.ContainsKey(dbName) && backupsToRestore[dbName] < fi.CreationTime)
//                {
//                    if (dbName == "watchdog" && fi.Length > lastSize)
//                    {
//                        lastSize = fi.Length;
//                        ReplaceBackupData(backupsToRestore, dbName, backupPaths, fi, file);
//                    }
//                    else if (dbName != "watchdog")
//                    {
//                        ReplaceBackupData(backupsToRestore, dbName, backupPaths, fi, file);
//                    }
//                }
//                else if (!backupsToRestore.ContainsKey(dbName))
//                {
//                    if (dbName == "watchdog")
//                    {
//                        lastSize = fi.Length;
//                    }

//                    backupsToRestore.Add(dbName, fi.CreationTime);
//                    backupPaths.Add(dbName, file);
//                }
//            }
            
//            Restore(backupsToRestore, backupPaths);
//        }

//        private static void ReplaceBackupData(Dictionary<string, DateTime> backupsToRestore, string dbName, Dictionary<string, string> backupPaths, FileInfo fi,
//                                              string file)
//        {
//            backupsToRestore.Remove(dbName);
//            backupPaths.Remove(dbName);
//            backupsToRestore.Add(dbName, fi.CreationTime);
//            backupPaths.Add(dbName, file);
//        }

//        private static void Restore(Dictionary<string, DateTime> backups, Dictionary<string, string> filePaths)
//        {
//            const string connString = @"Data Source=(local);Integrated Security=SSPI;Initial Catalog=master;";

//            using (var connection = new SqlConnection(connString))
//            {
//                connection.Open();
//                foreach(var bkp in backups)
//                {
//                    RestoreDatabase(connection, bkp.Key, filePaths[bkp.Key], true);
//                }
//            }
//        }
        
//        /// <summary>
//        /// Ensures we are only working in SQL connections.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        private static void ValidateConnection(IDbConnection connection)
//        {
//            if (!(connection is SqlConnection))
//            {
//                throw new ArgumentException("Can't clone non-SQL server connections");
//            }
//        }

//        /// <summary>
//        /// Checks if a database with the given name existing on the server.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="name">The name.</param>
//        /// <returns></returns>
//        public static bool DatabaseExists(this IDbConnection connection, string name)
//        {
//            ValidateConnection(connection);

//            const string sql = @"if db_id('{0}') is not null select 1;if not db_id('{0}') is not null select 0";

//            return connection.ExecuteScalar<int>(sql, name) == 1;
//        }

//        /// <summary>
//        /// Drops the database with the given name
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="name">The name.</param>
//        public static void DropDatabase(this IDbConnection connection, string name)
//        {
//            ValidateConnection(connection);

//            const string sql = @"ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{0}];";

//            connection.ExecuteNonQuery(sql, name);
//        }

//        /// <summary>
//        /// Restores the database with the given name.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="name">The name.</param>
//        /// <param name="path">The backup file path </param>
//        /// <param name="drop">if set to <c>true</c> drop the database if it already exists.</param>
//        /// <returns></returns>
//        public static IDbConnection RestoreDatabase(this IDbConnection connection, string name, string path, bool drop = false)
//        {
//            ValidateConnection(connection);

//            if (drop && connection.DatabaseExists(name))
//            {
//                connection.DropDatabase(name);
//            }

//            if (!connection.DatabaseExists(name))
//            {
//                const string sql = @"RESTORE DATABASE [{0}] FROM DISK = '{1}'
//                                   WITH MOVE '{0}' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\{0}.MDF',
//                                   MOVE '{0}_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\{0}.LDF'";

//                connection.ExecuteNonQuery(sql, name, path);
//            }

//            return connection;
//        }

//        /// <summary>
//        /// Executes the specified SQL against the <see cref="IDbConnection"/>.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="sql">The SQL.</param>
//        /// <param name="args">The args.</param>
//        /// <returns></returns>
//        public static T ExecuteScalar<T>(this IDbConnection connection, string sql, params object[] args)
//        {
//            var formatted = string.Format(sql, args);

//            return connection.ExecuteScalar<T>(formatted, new List<DbParameter>(), 0);
//        }

//        /// <summary>
//        /// Executes the specified SQL against the <see cref="IDbConnection"/>.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="sql">The SQL.</param>
//        /// <param name="args">The args.</param>
//        /// <returns></returns>
//        public static IDbConnection ExecuteNonQuery(this IDbConnection connection, string sql, params object[] args)
//        {
//            var formatted = string.Format(sql, args);

//            return connection.ExecuteNonQuery(formatted);
//        }

//        /// <summary>
//        /// Executes the specified SQL against the <see cref="IDbConnection"/>.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="sql">The SQL.</param>
//        /// <param name="parameters">The parameters.</param>
//        /// <param name="timeout">The timeout.</param>
//        /// <returns></returns>
//        public static T ExecuteScalar<T>(this IDbConnection connection, string sql, IList<DbParameter> parameters, int timeout)
//        {
//            T result;

//            using (var command = connection.CreateCommand())
//            {
//                command.CommandText = sql;
//                command.CommandTimeout = timeout;

//                if (parameters != null)
//                {
//                    foreach (var parameter in parameters)
//                    {
//                        command.Parameters.Add(parameter);
//                    }
//                }

//                result = (T)command.ExecuteScalar();
//            }

//            return result;
//        }

//        /// <summary>
//        /// Executes the specified SQL against the <see cref="IDbConnection"/>.
//        /// </summary>
//        /// <param name="connection">The connection.</param>
//        /// <param name="sql">The SQL.</param>
//        /// <param name="parameters">The parameters.</param>
//        /// <param name="timeout">The timeout.</param>
//        public static IDbConnection ExecuteNonQuery(this IDbConnection connection, string sql, IList<DbParameter> parameters = null, int timeout = 0)
//        {
//            using (var command = connection.CreateCommand())
//            {
//                command.CommandText = sql;
//                command.CommandTimeout = timeout;

//                if (parameters != null)
//                {
//                    foreach (var parameter in parameters)
//                    {
//                        command.Parameters.Add(parameter);
//                    }
//                }

//                command.ExecuteNonQuery();
//            }

//            return connection;
//        }
//    }
//}
