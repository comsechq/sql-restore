using Sugar.Command;
using dbBackupRestore.Interfaces;
using dbBackupRestore.Services;

namespace dbBackupRestore.Commands
{
    /// <summary>
    /// Restores a directory to a server
    /// </summary>
    public class RestoreCommand : BoundCommand<RestoreCommand.Options>
    {
        [Flag("server", "dir")]
        public class Options
        {
            /// <summary>
            /// Gets or sets the server.
            /// </summary>
            /// <value>
            /// The server.
            /// </value>
            [Parameter("server")]
            public string Server { get; set; }

            /// <summary>
            /// Gets or sets the directory.
            /// </summary>
            /// <value>
            /// The directory.
            /// </value>
            [Parameter("dir")]
            public string Directory { get; set; }

            /// <summary>
            /// Gets or sets the db file path.
            /// </summary>
            /// <value>
            /// The db file path.
            /// </value>
            [Parameter("dbpath")]
            public string DbFilePath { get; set; }
        }
        
        #region Dependencies

        /// <summary>
        /// Gets or sets the backup file service.
        /// </summary>
        /// <value>
        /// The backup file service.
        /// </value>
        public IBackupFileService BackupFileService { get; set; }

        /// <summary>
        /// Gets or sets the SQL service.
        /// </summary>
        /// <value>
        /// The SQL service.
        /// </value>
        public ISqlService SqlService { get; set; }

        #endregion

        public RestoreCommand()
        {
            BackupFileService = new BackupFileService();
            SqlService = new SqlService();
        }

        /// <summary>
        /// Executes the command and restores the given directory onto the SQL server
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var files = BackupFileService.ParseDirectory(options.Directory);
            files = BackupFileService.RemoveDuplicatesByDate(files);
            files = BackupFileService.RemoveDuplicatesBySize(files);
            foreach (var file in files)
            {
                file.FileList = SqlService.GetLogicalNames(options.Server, file);
                SqlService.Restore(options.Server, file, options.DbFilePath);
            }
        }
    }
}
