using System.Collections.Generic;
using System.Data.SqlClient;
using Comsec.SqlRestore.Core;
using Comsec.SqlRestore.Domain;
using Comsec.SqlRestore.Interfaces;

namespace Comsec.SqlRestore.Services
{
    /// <summary>
    /// Service to manipulate a SQL server
    /// </summary>
    public class SqlService : ISqlService
    {
        /// <summary>
        /// Restores the specified backup file to the given server.
        /// This method uses a trusted SQL connection
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="dbFilePath">The path to restore physical files to</param>
        public void Restore(string server, BackupFile backupFile, string dbFilePath)
        {
            const string connectionStringFormat = @"Data Source={0};Integrated Security=SSPI;Initial Catalog=master;";

            var connectionString = string.Format(connectionStringFormat, server);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.RestoreDatabase(backupFile, dbFilePath, true);
               
            }
        }

        /// <summary>
        /// Gets the logical names from the backup file
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <returns></returns>
        public List<FileListEntry> GetLogicalNames(string server, BackupFile backupFile)
        {
            List<FileListEntry> names;

            const string connectionStringFormat = @"Data Source={0};Integrated Security=SSPI;Initial Catalog=master;";

            var connectionString = string.Format(connectionStringFormat, server);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                names = connection.GetFileListEntries(backupFile.FileName);
            }

            return names;
        }
    }
}
