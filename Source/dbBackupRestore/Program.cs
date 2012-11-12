using Sugar.Command;
using dbBackupRestore.Commands;
using dbBackupRestore.Core;
using dbBackupRestore.Domain;

namespace dbBackupRestore
{
    public static class BackupRestore
    {
        static void Main(string[] args)
        {
            var console = new BackupRestoreConsole();
            console.Commands.Add(new RestoreCommand());

            console.Run(args);
        }
    }
}
