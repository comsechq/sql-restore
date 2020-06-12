using Sugar.Command.Binder;

namespace Comsec.SqlRestore
{
    public static class BackupRestore
    {
        static int Main(string[] args)
        {
            var console = new BackupRestoreConsole();
            
            var parameters = new Parameters(System.Environment.CommandLine);

            return console.Run(parameters);
        }
    }
}
