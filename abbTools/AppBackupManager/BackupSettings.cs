using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abbTools.AppBackupManager
{
    class BackupSettings
    {
        //robot or pc backup master 
        public enum master
        {
            pc = 0,
            robot = 1
        }

        //action type when backup names duplicate
        public enum duplicate
        {
            overwrite = 0,
            increment = 1,
            additTime = 2
        }

        //save time selection
        public enum time
        {
            last = 0,
            exact
        }

        //backup source selection
        public enum source
        {
            gui = 0,
            interval,
            daily,
            robot
        }

        //interval element selection
        public enum interval
        {
            mins = 0,
            hours,
            days
        }
    }
}
