using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dbBackupRestore.Domain;
using dbBackupRestore.Interfaces;

namespace dbBackupRestore.Services
{
    public class BackupFileService : IBackupFileService
    {
        public IList<BackupFile> ParseDirectory(string directory)
        {
            var backupFileList = new List<BackupFile>();
            var filePaths = Directory.GetFiles(directory, "*.bak");
           
            foreach (var file in filePaths)
            {
                var fi = new FileInfo(file);
                var l = fi.Name.IndexOf("_backup");
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

        private static void ReplaceBackupData(Dictionary<string, DateTime> backupsToRestore, string dbName, Dictionary<string, string> backupPaths, 
                                                FileInfo fi, string file)
        {
            backupsToRestore.Remove(dbName);
            backupPaths.Remove(dbName);
            backupsToRestore.Add(dbName, fi.CreationTime);
            backupPaths.Add(dbName, file);
        }
    }
}
