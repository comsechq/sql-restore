using System;
using Sugar.Command;

namespace dbBackupRestore
{
    public class BackupRestoreConsole : BaseCommandConsole
    {
        public override void Default()
        {
            Console.WriteLine("No Options!");
        }
    }
}
