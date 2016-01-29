namespace Comsec.SqlRestore
{
    public static class BackupRestore
    {
        static int Main(string[] args)
        {
            var console = new BackupRestoreConsole();
            
            return console.Run(args);
        }
    }
}
