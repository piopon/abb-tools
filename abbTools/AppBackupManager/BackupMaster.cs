using System;

namespace abbTools.AppBackupManager
{
    //robot or pc backup master 
    enum backupMaster
    {
        pc = 0,
        robot = 1
    }
    //action type when backup names duplicate
    enum sameNameAction
    {
        overwrite = 0,
        increment = 1,
        additTime = 2
    }
    //save time selection
    enum timeType
    {
        last = 0,
        exact
    }
    //backup source selection
    enum backupSource
    {
        gui = 0,
        interval,
        daily,
        robot
    }
    //interval element selection
    enum intervalElement
    {
        mins = 0,
        hours,
        days
    }

    abstract class BackupMaster
    {
        public DateTime lastBackupTime;
        public int duplicateMethod;
        public bool activated;

        abstract public void clearData();
        abstract public string getSuffix(backupSource src);
    }

    class BackupMasterPC : BackupMaster
    {
        private int offsMin;
        private int offsHour;
        private int offsDay;
        private string suffixGUI;
        private string suffixInterval;
        private string suffixTime;
        public DateTime exactTime;

        public BackupMasterPC()
        {
            clearData();
        }

        override public void clearData()
        {
            activated = false;
            duplicateMethod = (int)sameNameAction.overwrite;
            offsMin = 0;
            offsHour = 0;
            offsDay = 0;
            suffixGUI = "";
            suffixInterval = "";
            suffixTime = "";
            exactTime = new DateTime(1, 1, 1);
            lastBackupTime = new DateTime(1, 1, 1);
        }

        override public string getSuffix(backupSource src)
        {
            string result = "";

            switch (src) {
                case backupSource.gui:
                    result = suffixGUI;
                    break;
                case backupSource.interval:
                    result = suffixInterval;
                    break;
                case backupSource.daily:
                    result = suffixTime;
                    break;
                default:
                    result = "ERROR!";
                    break;
            }

            return result;
        }

        public void intervalSet(int mins, int hours, int days)
        {
            offsMin = mins;
            offsHour = hours;
            offsDay = days;
        }

        public int intervalGet(intervalElement element)
        {
            int result = -1;

            switch (element)
            {
                case intervalElement.mins:
                    result = offsMin;
                    break;
                case intervalElement.hours:
                    result = offsHour;
                    break;
                case intervalElement.days:
                    result = offsDay;
                    break;
                default:
                    break;
            }

            return result;
        }

        public bool intervalCheck()
        {
            return (offsMin != 0 || offsHour != 0 || offsDay != 0);
        }

        public int intervalTimeInMins()
        {
            return offsMin + offsHour * 60 + offsDay * 24 * 60;
        }

        public string intervalSuffix
        {
            get { return suffixInterval; }
            set { suffixInterval = value; }
        }

        public string guiSuffix
        {
            get { return suffixGUI; }
            set { suffixGUI = value; }
        }

        public string timeSuffix
        {
            get { return suffixTime; }
            set { suffixTime = value; }
        }
    }

    class BackupMasterRobot : BackupMaster
    {
        public bool checkBackup;
        private string suffixRobot;
        private string sourceDir;
        private string sigBackupExe;
        private string sigBackupInP;

        public BackupMasterRobot()
        {
            clearData();
        }

        override public void clearData()
        {
            activated = false;
            duplicateMethod = (int)sameNameAction.overwrite;
            checkBackup = false;
            suffixRobot = "";
            sourceDir = "";
            sigBackupExe = "";
            sigBackupInP = "";
            lastBackupTime = new DateTime(1, 1, 1);
        }

        override public string getSuffix(backupSource src)
        {
            string result = "";

            switch (src) {
                case backupSource.robot:
                    result = suffixRobot;
                    break;
                default:
                    result = "ERROR!";
                    break;
            }

            return result;
        }

        public string sigExecute
        {
            get { return sigBackupExe; }
            set { sigBackupExe = value; }
        }

        public string sigInProgress
        {
            get { return sigBackupInP; }
            set { sigBackupInP = value; }
        }

        public string suffix
        {
            get { return suffixRobot; }
            set { suffixRobot = value; }
        }

        public string srcDir
        {
            get { return sourceDir; }
            set { sourceDir = value; }
        }
    }
}
