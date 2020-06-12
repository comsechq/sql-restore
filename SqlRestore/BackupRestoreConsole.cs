using System;
using System.Diagnostics;
using System.Reflection;
using Comsec.SqlRestore.Commands;
using Sugar.Command;
using Sugar.Command.Binder;

namespace Comsec.SqlRestore
{
    /// <summary>
    /// Main Command Console
    /// </summary>
    public class BackupRestoreConsole : BaseConsole
    {
        /// <summary>
        /// Entry point for the program logic
        /// </summary>
        protected override int Main()
        {
            var exitCode = Arguments.Count > 0 ? Run(typeof(RestoreCommand), Arguments) : Default();

            return exitCode;
        }

        /// <summary>
        /// Runs the specified parameters.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int Run(Type commandType, Parameters parameters)
        {
            var command = (ICommand) Activator.CreateInstance(commandType);

            command.BindParameters(parameters);

            return command.Execute();
        }

        /// <summary>
        /// Displays the help message
        /// </summary>
        public int Default()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = fvi.FileVersion;

            Console.WriteLine("SQL Restore Utility - v" + version);
            Console.WriteLine("Comsec Solutions Ltd - http://comsechq.com");
            Console.WriteLine("");
            Console.WriteLine("A simple utility to restore a directoy of backup files to a MS SQL Server.");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("    sqlrest.exe -server [server] -src [source] -dest [destination] [-log-dest [log file destination]]");
            Console.WriteLine("");
            Console.WriteLine("  server:   The host name of the SQL server. Uses trusted authentication.");
            Console.WriteLine("     src:   The source directory where the backup files are located");
            Console.WriteLine("    dest:   The destination directory to restore the backup files to");
            Console.WriteLine("    dest:   The destination directory to restore the data (.mdf) files to");
            Console.WriteLine("log-dest:   The destination directory to restore the log (.ldf) files to (optional: defaults to dest)");

            return (int) ExitCode.NoCommand;
        }
    }
}
