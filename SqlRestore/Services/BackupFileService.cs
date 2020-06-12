using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Comsec.SqlRestore.Domain;
using Comsec.SqlRestore.Interfaces;

namespace Comsec.SqlRestore.Services
{
    /// <summary>
    /// Service to manipulate SQL server backup files
    /// </summary>
    public class BackupFileService : IBackupFileService
    {
        /// <summary>
        /// Parses the directory and returns all the database backups inside.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public IList<BackupFile> ParseDirectory(string directory)
        {
            var backupFileList = new List<BackupFile>();
            var filePaths = Directory.GetFiles(directory, "*.bak");
           
            foreach (var file in filePaths)
            {
                var fi = new FileInfo(file);
                var l = fi.Name.IndexOf("_backup", StringComparison.Ordinal);
                var dbName = fi.Name.Substring(0, l);

                var item = new BackupFile
                           {
                               Created = fi.CreationTime,
                               DatabaseName = dbName,
                               FileName = fi.FullName,
                               Length = fi.Length
                           };

                backupFileList.Add(item);
            }
            

            return backupFileList;
        }

        /// <summary>
        /// Returns a list of backup files, removing the smallest versions of each database
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public IList<BackupFile> RemoveDuplicatesBySize(IList<BackupFile> files)
        {
            var results = new List<BackupFile>();
            var sorted = files.OrderByDescending(x => x.Length);

            foreach (var file in sorted)
            {
                if (results.Any(x => x.DatabaseName == file.DatabaseName))
                {
                    continue;
                }
                results.Add(file);
            }

            return results;
        }

        /// <summary>
        /// Removes the duplicates by date.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public IList<BackupFile> RemoveDuplicatesByDate(IList<BackupFile> files)
        {
            var results = new List<BackupFile>();
            var sorted = files.OrderByDescending(x => x.Created);

            foreach (var file in sorted)
            {
                if (results.Any(x => x.DatabaseName == file.DatabaseName && x.Created.Date > file.Created.Date))
                {
                    continue;
                }
                results.Add(file);
            }

            return results;
        }
    }
}
