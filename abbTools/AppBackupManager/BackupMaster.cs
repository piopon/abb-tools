using System;

namespace abbTools.AppBackupManager
{
    /********************************************************
     ***  BACKUP MANAGER - global data
     ********************************************************/

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

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    abstract class BackupMaster
    {
        /********************************************************
         ***  BACKUP MASTER - data
         ********************************************************/

        public DateTime lastBackupTime;
        public int duplicateMethod;
        public bool activated;

        /********************************************************
         ***  BACKUP MASTER - common methods
         ********************************************************/

        abstract public void clearData();
        abstract public string getSuffix(backupSource src);
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class BackupMasterPC : BackupMaster
    {
        ///internal data
        private int offsMin;
        private int offsHour;
        private int offsDay;
        private string suffixGUI;
        private string suffixInterval;
        private string suffixTime;
        public DateTime exactTime;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupMasterPC()
        {
            clearData();
        }

        /// <summary>
        /// Method used to zero all instance data
        /// </summary>
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

        /// <summary>
        /// Function used to get suffix string from desired backup source
        /// </summary>
        /// <param name="src">Backup source to get suffix from (GUI, INTERVAL or DAILY)</param>
        /// <returns>Suffix string</returns>
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

        /// <summary>
        /// Method used to set interval time element in current instance
        /// </summary>
        /// <param name="el">Interval element to set</param>
        /// <param name="val">Interval element value</param>
        public void intervalSet(intervalElement el, int val)
        {
            switch (el)
            {
                case intervalElement.mins:
                    offsMin = val;
                    break;
                case intervalElement.hours:
                    offsHour = val;
                    break;
                case intervalElement.days:
                    offsDay = val;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Function used to get interval component
        /// </summary>
        /// <param name="element">Select interval time element (MINS, HOURS or DAYS)</param>
        /// <returns>Selected interval element</returns>
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

        /// <summary>
        /// Function used to check if any interval is set (!=0)
        /// </summary>
        /// <returns>TRUE if !=0 interval is set, FALSE otherwise</returns>
        public bool intervalCheck()
        {
            return (offsMin != 0 || offsHour != 0 || offsDay != 0);
        }

        /// <summary>
        /// Function used to get total interval time in minutes
        /// </summary>
        /// <returns>Total interval time in minutes</returns>
        public int intervalTimeInMins()
        {
            return offsMin + offsHour * 60 + offsDay * 24 * 60;
        }

        /// <summary>
        /// GET and SET suffix INTERVAL string
        /// </summary>
        public string intervalSuffix
        {
            get { return suffixInterval; }
            set { suffixInterval = value; }
        }

        /// <summary>
        /// GET and SET suffix GUI string
        /// </summary>
        public string guiSuffix
        {
            get { return suffixGUI; }
            set { suffixGUI = value; }
        }

        /// <summary>
        /// GET and SET suffix EXACT TIME (DAILY) string
        /// </summary>
        public string timeSuffix
        {
            get { return suffixTime; }
            set { suffixTime = value; }
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class BackupMasterRobot : BackupMaster
    {
        ///internal data
        public bool checkBackup;
        private string suffixRobot;
        private string sourceDir;
        private string sigBackupExe;
        private string sigBackupInP;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupMasterRobot()
        {
            clearData();
        }

        /// <summary>
        /// Method used to zero all instance data
        /// </summary>
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

        /// <summary>
        /// Function used to get suffix string from desired backup source
        /// </summary>
        /// <param name="src">Backup source to get suffix from (ROBOT)</param>
        /// <returns>Suffix string</returns>
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

        /// <summary>
        /// GET and SET signal name backup execute 
        /// </summary>
        public string sigExecute
        {
            get { return sigBackupExe; }
            set { sigBackupExe = value; }
        }

        /// <summary>
        /// GET and SET signal name backup in progress
        /// </summary>
        public string sigInProgress
        {
            get { return sigBackupInP; }
            set { sigBackupInP = value; }
        }

        /// <summary>
        /// GET and SET backup suffix
        /// </summary>
        public string suffix
        {
            get { return suffixRobot; }
            set { suffixRobot = value; }
        }

        /// <summary>
        /// GET and SET backup source directory (on robot)
        /// </summary>
        public string srcDir
        {
            get { return sourceDir; }
            set { sourceDir = value; }
        }
    }
}
