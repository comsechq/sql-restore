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
        /// <param name="dataFilesPath">The path to restore data files to</param>
        /// <param name="logFilesPath">The path to restore log files to (optional: defaults to <see cref="dataFilesPath"/> when not set).</param>
        void Restore(string server, BackupFile backupFile, string dataFilesPath, string logFilesPath = null);

        /// <summary>
        /// Gets the logical names from the backup file
        /// </summary>
        /// <param name="server">The server. </param>
        /// <param name="backupFile">The backup file.</param>
        /// <returns></returns>
        List<FileListEntry> GetLogicalNames(string server, BackupFile backupFile);
    }
}
