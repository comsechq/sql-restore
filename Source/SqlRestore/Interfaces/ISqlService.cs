using System.Collections.Generic;
using Comsec.SqlRestore.Domain;

namespace Comsec.SqlRestore.Interfaces
{
    /// <summary>
    /// Service to interact with an SQL server.
    /// </summary>
    public interface ISqlService
    {
        /// <summary>
        /// Restores the specified backup file to the given server.
        /// This method uses a trusted SQL connection
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="backupFile">The backup file.</param>
        /// <param name="dbFilePath">The path to restore physical files to </param>
        void Restore(string server, BackupFile backupFile, string dbFilePath);

        /// <summary>
        /// Gets the logical names from the backup file
        /// </summary>
        /// <param name="server">The server. </param>
        /// <param name="backupFile">The backup file.</param>
        /// <returns></returns>
        List<FileListEntry> GetLogicalNames(string server, BackupFile backupFile);
    }
}
