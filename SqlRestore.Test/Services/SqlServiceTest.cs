using System;
using System.Collections.Generic;
using System.IO;
using Comsec.SqlRestore.Domain;
using NUnit.Framework;

namespace Comsec.SqlRestore.Services
{
    [TestFixture]
    public class SqlServiceTest
    {
        [Test]
        public void TestGenerateRestoreSqlWithoutLdfPath()
        {
            var file = new BackupFile
                       {
                           Created = new DateTime(2014, 6, 19, 10, 19, 0),
                           DatabaseName = "comsec",
                           FileList = new List<FileListEntry>
                                      {
                                          new FileListEntry
                                          {
                                              LogicalName = "comsec",
                                              PhysicalName = @"D:\SQL\comsec.mdf",
                                              Type = "D"
                                          },
                                          new FileListEntry
                                          {
                                              LogicalName = "comsec_log",
                                              PhysicalName = @"L:\SQL\comsec_log.LDF",
                                              Type = "L"
                                          }
                                      },
                           FileName = "comsec_backup_2014_06_19_010004_7953268",
                           Length = 100000
                       };

            var query = SqlService.GenerateRestoreSql(
                file,
                new DirectoryInfo(@"D:\SQL\"));

            Assert.AreEqual(@"RESTORE DATABASE [comsec]
FROM DISK = 'comsec_backup_2014_06_19_010004_7953268' WITH REPLACE,
MOVE 'comsec' TO 'd:\sql\comsec.mdf',
MOVE 'comsec_log' TO 'd:\sql\comsec_log.ldf'", query);
        }

        [Test]
        public void TestGenerateRestoreSqlWithLdfPath()
        {
            var file = new BackupFile
                       {
                           Created = new DateTime(2014, 6, 19, 10, 19, 0),
                           DatabaseName = "comsec",
                           FileList = new List<FileListEntry>
                                      {
                                          new FileListEntry
                                          {
                                              LogicalName = "comsec",
                                              PhysicalName = @"D:\SQL\comsec.mdf",
                                              Type = "D"
                                          },
                                          new FileListEntry
                                          {
                                              LogicalName = "comsec_log",
                                              PhysicalName = @"L:\SQL\comsec_log.LDF",
                                              Type = "L"
                                          }
                                      },
                           FileName = "comsec_backup_2014_06_19_010004_7953268",
                           Length = 100000
                       };

            var query = SqlService.GenerateRestoreSql(
                file,
                new DirectoryInfo(@"D:\SQL\"),
                new DirectoryInfo(@"L:\SQL\"));

            Assert.AreEqual(@"RESTORE DATABASE [comsec]
FROM DISK = 'comsec_backup_2014_06_19_010004_7953268' WITH REPLACE,
MOVE 'comsec' TO 'd:\sql\comsec.mdf',
MOVE 'comsec_log' TO 'l:\sql\comsec_log.ldf'", query);
        }
    }
}
