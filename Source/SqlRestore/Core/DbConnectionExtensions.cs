using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Comsec.SqlRestore.Domain;
using Sugar.Data;

namespace Comsec.SqlRestore.Core
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Gets the file list entries.
        /// </summary>
        public static List<FileListEntry> GetFileListEntries(this IDbConnection connection, string file)
        {
            var sql = string.Format(@"RESTORE FILELISTONLY FROM DISK = '{0}'", file);

            return connection.Query<FileListEntry>(sql).ToList();
        }

        /// <summary>
        /// Restores the database with the given name.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="restorePath">The restore path.</param>
        /// <param name="drop">if set to <c>true</c> drop the database if it already exists.</param>
        /// <returns></returns>
        public static IDbConnection RestoreDatabase(this IDbConnection connection, BackupFile backupFile, string restorePath, bool drop = false)
        {
            if (drop && connection.DatabaseExists(backupFile.DatabaseName))
            {
                connection.DropDatabase(backupFile.DatabaseName);
            }

            if (!connection.DatabaseExists(backupFile.DatabaseName))
            {
                string sql = @"RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE";
                const string moveFormat = @", MOVE '{0}' TO '{1}'";

                foreach (var fileListEntry in backupFile.FileList)
                {
                    var fileName = Path.GetFileName(fileListEntry.PhysicalName) ?? string.Empty;
                    var fullFileName = Path.Combine(restorePath, fileName).ToLower();

                    // Add extension if required
                    if (!fullFileName.EndsWith(".mdf") &&
                        !fullFileName.EndsWith(".ldf"))
                    {
                        fullFileName += ".mdf";
                    }

                    sql += string.Format(moveFormat, fileListEntry.LogicalName, fullFileName);
                }
                
                sql = string.Format(sql, backupFile.DatabaseName, backupFile.FileName);
                
                connection.ExecuteNonQuery(sql, null, 0);
            }

            return connection;
        }

    }
}
