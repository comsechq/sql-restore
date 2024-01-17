using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;
using System.Linq;
using Comsec.SqlRestore.Services;

namespace Comsec.SqlRestore.Commands
{
    /// <summary>
    /// Restores a directory to a server
    /// </summary>
    public class RestoreCommand : ICommand<RestoreCommand.Input>
    {
        private readonly IBackupFileService backupFileService;
        private readonly ISqlService sqlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreCommand" /> class.
        /// </summary>
        public RestoreCommand(IBackupFileService backupFileService, ISqlService sqlService)
        {
            this.backupFileService = backupFileService;
            this.sqlService = sqlService;
        }

        public class Input
        {
            public Input(string server, DirectoryInfo src, DirectoryInfo mdfPath, DirectoryInfo ldfPath)
            {
                Server = server;
                SourceDirectory = src;
                MdfRestorePath = mdfPath;
                LdfRestorePath = ldfPath;
            }

            public string Server { get; set; }

            public DirectoryInfo SourceDirectory { get; set; }

            public DirectoryInfo MdfRestorePath { get; set; }

            public DirectoryInfo LdfRestorePath { get; set; }
        }

        public class InputBinder : BinderBase<Input>
        {
            private readonly Argument<string> serverArgument;
            private readonly Option<DirectoryInfo> srcOption;
            private readonly Option<DirectoryInfo> mdfOption;
            private readonly Option<DirectoryInfo> ldfOption;

            public InputBinder(Argument<string> serverArgument, Option<DirectoryInfo> srcOption, Option<DirectoryInfo> mdfOption, Option<DirectoryInfo> ldfOption)
            {
                this.serverArgument = serverArgument;
                this.srcOption = srcOption;
                this.mdfOption = mdfOption;
                this.ldfOption = ldfOption;
            }
        
            protected override Input GetBoundValue(BindingContext bindingContext)
            {
                return new Input(
                    bindingContext.ParseResult.GetValueForArgument(this.serverArgument),
                    bindingContext.ParseResult.GetValueForOption(this.srcOption),
                    bindingContext.ParseResult.GetValueForOption(this.mdfOption),
                    bindingContext.ParseResult.GetValueForOption(this.ldfOption)
                );
            }
        }

        /// <summary>
        /// Executes the command and restores the given directory onto the SQL server
        /// </summary>
        /// <param name="input">The options.</param>
        public void Execute(Input input)
        {
            var files = backupFileService.ParseDirectory(input.SourceDirectory);

            files = backupFileService.RemoveDuplicatesByDate(files);
            files = backupFileService.RemoveDuplicatesBySize(files);

            foreach (var file in files)
            {
                Console.WriteLine("Restoring Database: " + file.DatabaseName);

                file.FileList = sqlService.GetLogicalNames(input.Server, file)
                                          .ToList();

                try
                {
                    sqlService.Restore(input.Server, file, input.MdfRestorePath, input.LdfRestorePath);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    // Don't bomb out when a SQL exception is thrown, move on to the next file
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
