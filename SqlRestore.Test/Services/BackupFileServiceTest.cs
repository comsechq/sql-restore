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

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Length, Is.EqualTo(3));
        }

        [Test]
        public void TestRemoveSmallDatabasesWhenDifferentNames()
        {
            var files = CreateBackupFiles();
            files[1].DatabaseName = "Different";

            var results = service.RemoveDuplicatesBySize(files);

            Assert.That(results.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestRemoveDatabasesByDate()
        {
            var files = CreateBackupFiles();
            files[0].Created = new DateTime(2001, 1, 1);
            files[1].Created = new DateTime(2001, 1, 2);

            var results = service.RemoveDuplicatesByDate(files);

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Created, Is.EqualTo(new DateTime(2001, 1, 2)));
        }

        [Test]
        public void TestRemoveDatabasesByDateWithDifferentNames()
        {
            var files = CreateBackupFiles();
            files[0].Created = new DateTime(2001, 1, 1);
            files[1].Created = new DateTime(2001, 1, 1);
            files[1].DatabaseName = "New DB";

            var results = service.RemoveDuplicatesByDate(files);

            Assert.That(results.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void TestRemoveDatabasesByDateWithDifferentTimes()
        {
            var files = CreateBackupFiles();
            files[0].Created = new DateTime(2001, 1, 1, 1, 30, 0);
            files[1].Created = new DateTime(2001, 1, 1, 2, 30, 0);
         
            var results = service.RemoveDuplicatesByDate(files);

            Assert.That(results.Count, Is.EqualTo(2));
        }
    }
}
