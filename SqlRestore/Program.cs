using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.IO;
using Comsec.SqlRestore.Commands;
using Comsec.SqlRestore.Services;

namespace Comsec.SqlRestore
{
    public static class BackupRestore
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand("SQL Restore Utility");

            var restoreCommand = new Command("restore", "CLI utility to restore a directory of backup files to an instance of Microsoft SQL Server.");

            var serverArgument = new Argument<string>(
                "server",
                "The host name of the SQL server. Trusted authentication will be used.");

            restoreCommand.AddArgument(serverArgument);

            var srcOption = new Option<DirectoryInfo>(
                    "--src",
                    "The source directory where the backup files are located.")
                {IsRequired = true};

            restoreCommand.AddOption(srcOption);

            var mdfOption = new Option<DirectoryInfo>(
                    "--mdfPath",
                    "The destination directory to restore the data (.mdf) files to.")
                {IsRequired = true};

            restoreCommand.AddOption(mdfOption);

            var ldfOption = new Option<DirectoryInfo>(
                    "--ldfPath",
                    "The destination directory to restore the log (.ldf) files to.")
                {IsRequired = true};

            restoreCommand.AddOption(ldfOption);

            restoreCommand.SetHandler((input) =>
                {
                    var backupFileService = new BackupFileService();
                    var sqlService = new SqlService();

                    var command = new RestoreCommand(backupFileService, sqlService);

                    command.Execute(input);
                },
                new RestoreCommand.InputBinder(serverArgument, srcOption, mdfOption, ldfOption));

            rootCommand.Add(restoreCommand);

            var builder = new CommandLineBuilder(rootCommand).UseDefaults()
                                                             .Build();

            return builder.Invoke(args);
        }
    }
}
