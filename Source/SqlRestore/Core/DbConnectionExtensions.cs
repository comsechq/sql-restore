using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
        /// Generates a restore database from file to specified location SQL statement.
        /// </summary>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="mdfRestorePath">The restore path for MDF files.</param>
        /// <param name="ldfRestorePath">The restore path for LDF (optional, MDF path used when not set).</param>
        /// <returns>A restore database from file SQL statement</returns>
        /// <remarks>This method is public only so that it can be unit tested</remarks>
        public static string GenerateRestoreSql(BackupFile backupFile, string mdfRestorePath, string ldfRestorePath = null)
        {
            // LDF restore path defaults to the MDF location when not set
            ldfRestorePath = ldfRestorePath ?? mdfRestorePath;

            var builder = new StringBuilder()
                .Append("RESTORE DATABASE [").Append(backupFile.DatabaseName).AppendLine("]")
                .Append("FROM DISK = '").Append(backupFile.FileName).AppendLine("' WITH REPLACE,");

            for (var i = 0; i < backupFile.FileList.Count; i++)
            {
                var fileListEntry = backupFile.FileList[i];

                var fileName = Path.GetFileName(fileListEntry.PhysicalName) ?? string.Empty;
                string fullFileName;
                
                var isDataFile = fileListEntry.Type == "D";

                if (isDataFile)
                {
                    fullFileName = Path.Combine(mdfRestorePath, fileName).ToLower();

                    if (!fullFileName.EndsWith(".mdf"))
                    {
                        fullFileName += ".mdf";
                    }
                }
                else
                {
                    fullFileName = Path.Combine(ldfRestorePath, fileName).ToLower();

                    if (!fullFileName.EndsWith(".ldf"))
                    {
                        fullFileName += ".ldf";
                    }
                }

                builder.Append("MOVE '").Append(fileListEntry.LogicalName).Append("' TO '").Append(fullFileName).Append("'");

                if (i != backupFile.FileList.Count -1)
                {
                    builder.AppendLine(",");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Restores the database with the given name.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="dataFilesPath">The data files restore path (for MDF files).</param>
        /// <param name="logFilesPath">The log files restore path (optiona: default to <see cref="dataFilesPath"/> when not set).</param>
        /// <param name="drop">if set to <c>true</c> drop the database if it already exists.</param>
        /// <returns></returns>
        public static IDbConnection RestoreDatabase(this IDbConnection connection, BackupFile backupFile, string dataFilesPath, string logFilesPath = null, bool drop = false)
        {
            if (drop && connection.DatabaseExists(backupFile.DatabaseName))
            {
                connection.DropDatabase(backupFile.DatabaseName);
            }

            if (!connection.DatabaseExists(backupFile.DatabaseName))
            {
                var sql = GenerateRestoreSql(backupFile, dataFilesPath, logFilesPath);
                
                connection.ExecuteNonQuery(sql, null, 0);
            }

            return connection;
        }

    }
}
