using System;
using Comsec.SqlRestore.Interfaces;
using Comsec.SqlRestore.Services;
using Sugar.Command;

namespace Comsec.SqlRestore.Commands
{
    /// <summary>
    /// Restores a directory to a server
    /// </summary>
    public class RestoreCommand : BoundCommand<RestoreCommand.Options>
    {
        [Flag("server", "src", "dest")]
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
            [Parameter("src")]
            public string SourceDirectory { get; set; }

            /// <summary>
            /// Gets or sets the destination directory.
            /// </summary>
            /// <value>
            /// The db file path.
            /// </value>
            [Parameter("dest")]
            public string DataFilesDestinationDirectory { get; set; }

            /// <summary>
            /// Gets or sets the log files destination directory.
            /// </summary>
            /// <value>
            /// The log files destination directory.
            /// </value>
            [Parameter("log-dest", Required = false)]
            public string LogFilesDestinationDirectory { get; set; }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreCommand" /> class.
        /// </summary>
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
            var files = BackupFileService.ParseDirectory(options.SourceDirectory);

            files = BackupFileService.RemoveDuplicatesByDate(files);
            files = BackupFileService.RemoveDuplicatesBySize(files);
            
            foreach (var file in files)
            {
                Console.WriteLine("Restoring Database: " + file.DatabaseName);

                file.FileList = SqlService.GetLogicalNames(options.Server, file);
                SqlService.Restore(options.Server, file, options.DataFilesDestinationDirectory, options.LogFilesDestinationDirectory);
            }
        }
    }
}
