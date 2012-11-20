using System;
using System.Collections.Generic;

namespace Comsec.SqlRestore.Domain
{
    /// <summary>
    /// Represents a single database backup file to be restored
    /// </summary>
    public class BackupFile
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="BackupFile" /> class from being created.
        /// </summary>
        public BackupFile()
        {
            FileList = new List<FileListEntry>();
        }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets the logical names.
        /// </summary>
        /// <value>
        /// The logical names.
        /// </value>
        public IList<FileListEntry> FileList { get; set; }
    }
}
