﻿using System.Collections.Generic;
using System.IO;
using Comsec.SqlRestore.Domain;

namespace Comsec.SqlRestore.Services
{
    /// <summary>
    /// Service for manipulating <see cref="BackupFile"/> objects.
    /// </summary>
    public interface IBackupFileService
    {
        /// <summary>
        /// Parses the directory and returns all the database backups inside.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        IList<BackupFile> ParseDirectory(DirectoryInfo directory);

        /// <summary>
        /// Returns a list of backup files, removing the smallest versions of each database
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        IList<BackupFile> RemoveDuplicatesBySize(IList<BackupFile> files);

        /// <summary>
        /// Removes the duplicates by date.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        IList<BackupFile> RemoveDuplicatesByDate(IList<BackupFile> files);
    }
}
