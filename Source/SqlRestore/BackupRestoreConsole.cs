using System;
using Sugar.Command;

namespace Comsec.SqlRestore
{
    /// <summary>
    /// Main Command Console
    /// </summary>
    public class BackupRestoreConsole : BaseCommandConsole
    {
        /// <summary>
        /// Displays the help message
        /// </summary>
        public override void Default()
        {
            Console.WriteLine("SQL Restore Utility");
            Console.WriteLine("Comsec Solutions - comsechq.com");
            Console.WriteLine("");
            Console.WriteLine("A simple utility to restore a directoy of backup files to a MS SQL Server.");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("    sqlrest.exe -server [server] -src [source] -dest [destination]");
            Console.WriteLine("");
            Console.WriteLine("  server:   The host name of the SQL server. Uses trusted authentication.");
            Console.WriteLine("     src:   The source directory where the backup files are located");
            Console.WriteLine("    dest:   The destination directory to restore the backup files to");
        }
    }
}
