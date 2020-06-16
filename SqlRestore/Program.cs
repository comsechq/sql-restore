using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
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

            restoreCommand.AddArgument(
                new Argument<string>(
                    "server",
                    "The host name of the SQL server. Trusted authentication will be used."));
            
            restoreCommand.AddOption(
                new Option<DirectoryInfo>(
                        "--src",
                        "The source directory where the backup files are located.")
                    {Required = true});

            restoreCommand.AddOption(
                new Option<DirectoryInfo>(
                        "--mdfPath",
                        "The destination directory to restore the data (.mdf) files to.")
                    {Required = true});

            restoreCommand.AddOption(
                new Option<DirectoryInfo>(
                        "--ldfPath",
                        "The destination directory to restore the log (.ldf) files to.")
                    {Required = true}
            );

            restoreCommand.Handler =
                CommandHandler.Create<RestoreCommand.Input>(input =>
                {
                    var backupFileService = new BackupFileService();
                    var sqlService = new SqlService();

                    var command = new RestoreCommand(backupFileService, sqlService);

                    command.Execute(input);
                });

            rootCommand.Add(restoreCommand);

            var builder = new CommandLineBuilder(rootCommand);

            builder.UseDefaults()
                   .Build();

            return rootCommand.InvokeAsync(args)
                              .Result;
        }
    }
}
