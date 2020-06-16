using System.Collections.Generic;
using System.IO;
using Comsec.SqlRestore.Domain;

namespace Comsec.SqlRestore.Services
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
        /// <param name="mdfPath">The path to restore data files to</param>
        /// <param name="ldfPath">The path to restore log files to (optional: defaults to <see cref="mdfPath"/> when not set).</param>
        void Restore(string server, BackupFile backupFile, DirectoryInfo mdfPath, DirectoryInfo ldfPath = null);

        /// <summary>
        /// Gets the logical names from the backup file
        /// </summary>
        /// <param name="server">The server. </param>
        /// <param name="backupFile">The backup file.</param>
        /// <returns></returns>
        IEnumerable<FileListEntry> GetLogicalNames(string server, BackupFile backupFile);
    }
}
