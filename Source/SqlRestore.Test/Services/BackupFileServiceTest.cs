using System;
using System.Collections.Generic;
using Comsec.SqlRestore.Domain;
using Comsec.SqlRestore.Services;
using NUnit.Framework;

namespace dbBackupRestore.Test.Services
{
    [TestFixture]
    public class BackupFileServiceTest
    {
        private BackupFileService service;

        [SetUp]
        public void SetUp()
        {
            service = new BackupFileService();
        }

        private static List<BackupFile> CreateBackupFiles()
        {
            var files = new List<BackupFile>();
            var file1 = new BackupFile { DatabaseName = "Database", Length = 2 };
            var file2 = new BackupFile { DatabaseName = "Database", Length = 3 };
            files.Add(file1);
            files.Add(file2);
            return files;
        }

        [Test]
        public void TestRemoveSmallDatabases()
        {
            var files = CreateBackupFiles();

            var results = service.RemoveDuplicatesBySize(files);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(3, results[0].Length);
        }

        [Test]
        public void TestRemoveSmallDatabasesWhenDifferentNames()
        {
            var files = CreateBackupFiles();
            files[1].DatabaseName = "Different";

            var results = service.RemoveDuplicatesBySize(files);

            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public void TestRemoveDatabasesByDate()
        {
            var files = CreateBackupFiles();
            files[0].Created = new DateTime(2001, 1, 1);
            files[1].Created = new DateTime(2001, 1, 2);

            var results = service.RemoveDuplicatesByDate(files);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(new DateTime(2001, 1, 2), results[0].Created);
        }

        [Test]
        public void TestRemoveDatabasesByDateWithDifferentNames()
        {
            var files = CreateBackupFiles();
            files[0].Created = new DateTime(2001, 1, 1);
            files[1].Created = new DateTime(2001, 1, 1);
            files[1].DatabaseName = "New DB";

            var results = service.RemoveDuplicatesByDate(files);

            Assert.AreEqual(2, results.Count);
        }
        
        [Test]
        public void TestRemoveDatabasesByDateWithDifferentTimes()
        {
            var files = CreateBackupFiles();
            files[0].Created = new DateTime(2001, 1, 1, 1, 30, 0);
            files[1].Created = new DateTime(2001, 1, 1, 2, 30, 0);
         
            var results = service.RemoveDuplicatesByDate(files);

            Assert.AreEqual(2, results.Count);
        }
    }
}
