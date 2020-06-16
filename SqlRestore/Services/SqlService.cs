using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Comsec.SqlRestore.Data;
using Comsec.SqlRestore.Domain;
using Dapper;

namespace Comsec.SqlRestore.Services
{
    /// <summary>
    /// Service to manipulate a SQL server
    /// </summary>
    public class SqlService : ISqlService
    {
        /// <summary>
        /// Generates a restore database from file to specified location SQL statement.
        /// </summary>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="mdfRestorePath">The restore path for MDF files.</param>
        /// <param name="ldfRestorePath">The restore path for LDF (optional, MDF path used when not set).</param>
        /// <returns>A restore database from file SQL statement</returns>
        /// <remarks>This method is public only so that it can be unit tested</remarks>
        public static string GenerateRestoreSql(BackupFile backupFile, DirectoryInfo mdfRestorePath, DirectoryInfo ldfRestorePath = null)
        {
            // LDF restore path defaults to the MDF location when not set
            ldfRestorePath ??= new DirectoryInfo(mdfRestorePath.FullName);

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
                    fullFileName = Path.Combine(mdfRestorePath.FullName, fileName).ToLower();

                    if (!fullFileName.EndsWith(".mdf"))
                    {
                        fullFileName += ".mdf";
                    }
                }
                else
                {
                    fullFileName = Path.Combine(ldfRestorePath.FullName, fileName).ToLower();

                    if (!fullFileName.EndsWith(".ldf"))
                    {
                        fullFileName += ".ldf";
                    }
                }

                builder.Append("MOVE '").Append(fileListEntry.LogicalName).Append("' TO '").Append(fullFileName).Append("'");

                if (i != backupFile.FileList.Count - 1)
                {
                    builder.AppendLine(",");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Restores the specified backup file to the given server.
        /// This method uses a trusted SQL connection
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="mdfPath">The path to restore physical files to</param>
        /// <param name="ldfPath">The LDF file path.</param>
        public void Restore(string server, BackupFile backupFile, DirectoryInfo mdfPath, DirectoryInfo ldfPath = null)
        {
            var connectionString = $"Data Source={server};Integrated Security=SSPI;Initial Catalog=master;";

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.DatabaseExists(backupFile.DatabaseName))
                {
                    connection.DropDatabase(backupFile.DatabaseName);
                }

                if (!connection.DatabaseExists(backupFile.DatabaseName))
                {
                    var sql = GenerateRestoreSql(backupFile, mdfPath, ldfPath);

                    connection.Execute(sql, commandTimeout: 0);
                }
            }
        }

        /// <summary>
        /// Gets the logical names from the backup file
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <returns></returns>
        public IEnumerable<FileListEntry> GetLogicalNames(string server, BackupFile backupFile)
        {
            var connectionString = $@"Data Source={server};Integrated Security=SSPI;Initial Catalog=master;";

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = $@"RESTORE FILELISTONLY FROM DISK = '{backupFile.FileName}'";

                return connection.Query<FileListEntry>(sql);
            }
        }
    }
}
