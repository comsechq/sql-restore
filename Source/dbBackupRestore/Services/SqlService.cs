using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using dbBackupRestore.Core;
using dbBackupRestore.Domain;
using dbBackupRestore.Interfaces;

namespace dbBackupRestore.Services
{
    public class SqlService : ISqlService
    {
        public void Restore(string server, BackupFile backupFile, string dbFilePath)
        {
            const string connectionStringFormat = @"Data Source={0};Integrated Security=SSPI;Initial Catalog=master;";
            Console.WriteLine("Restoring Database " + backupFile.DatabaseName);

            var connectionString = string.Format(connectionStringFormat, server);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.RestoreDatabase(backupFile, dbFilePath, true);
               
            }
        }

        public List<FileListEntry> GetLogicalNames(string server, BackupFile backupFile)
        {
            var names = new List<FileListEntry>();
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
