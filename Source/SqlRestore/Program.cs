using Comsec.SqlRestore.Commands;

namespace Comsec.SqlRestore
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
