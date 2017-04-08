using System;
using System.IO;
using System.Linq;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using System.Windows.Forms;
using ABB.Robotics.Controllers.FileSystemDomain;

namespace abbTools.AppBackupManager
{
    class BackupManager
    {
        /********************************************************
         ***  BACKUP MANAGER - DATA 
         ********************************************************/

        private Controller myController;
        private Signal mySigExe;
        private Signal mySigInP;
        private loggerABB myLogger;
        //gui data
        private bool timerStart;
        private int daysClear;
        private string outputDir;
        private BackupMasterPC backupPC;
        private BackupMasterRobot backupRobot;
        //inner data
        DateTime emptyTime;
        //events
        public event robotBackupState RobotBackupStateChanged;
        public event pcBackupState PCBackupStateChanged;
        public delegate void robotBackupState(int newState);
        public delegate void pcBackupState(int newState);

        /********************************************************
         ***  BACKUP MANAGER - BASE 
         ********************************************************/

        public BackupManager()
        {
            timerStart = false;
            outputDir = "";
            myController = null;
            myLogger = null;
            backupPC = new BackupMasterPC();
            backupRobot = new BackupMasterRobot();
            emptyTime = new DateTime(1, 1, 1);
        }

        public BackupManager(bool watchOn, string outPath)
        {
            timerStart = watchOn;
            outputDir = outPath;
            myController = null;
            myLogger = null;
            backupPC = new BackupMasterPC();
        }

        public void clearData()
        {
            timerStart = false;
            daysClear = 0;
            outputDir = "";
            backupPC.clearData();
            backupRobot.clearData();
        }

        public Controller controller
        {
            get { return myController;  }
            set { myController = value; }
        }

        public loggerABB logger
        {
            get { return myLogger; }
            set { myLogger = value; }
        }

        /********************************************************
         ***  BACKUP MANAGER - EVENTS
         ********************************************************/

        protected void eventRobotBackupState(int newState)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            RobotBackupStateChanged?.Invoke(newState);
        }

        protected void eventPCBackupState(int newState)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            PCBackupStateChanged?.Invoke(newState);
        }

        /********************************************************
         ***  BACKUP MANAGER - COMMON
         ********************************************************/

        public void clearOutputDir(bool guiDemand = false)
        {
            //check if controller is existing in network
            if (myController == null) {
                if (myLogger != null) myLogger.writeLog(logType.error, "abbTools - can't clear folders! No controller connected...");
                return;
            }
            //check if output path is defined
            if (outputDir == null || outputDir == "") {
                if (myLogger != null) myLogger.writeLog(logType.error, "abbTools - can't clear folders! No outputh path defined...");
                return;
            }
            //get all directories in output dir
            string[] folders = Directory.GetDirectories(outputDir + "\\");
            //compare every folder creation date with current date
            int foldersDeleted = 0;
            for (int i = 0; i < folders.Length; i++) {
                //delete only backup files (contains _BACKUP_ string)
                if (folders[i].Contains("_BACKUP_")) {
                    DateTime createdTime = Directory.GetCreationTime(folders[i]);
                    TimeSpan timeDiff = DateTime.Now - createdTime;
                    if (daysClear > 0 && timeDiff.TotalDays > daysClear) {
                        Directory.Delete(folders[i], true);
                        foldersDeleted++;
                    }
                }
            }
            //show log if some folders were deleted
            if (myLogger != null) {
                if (foldersDeleted > 0) {
                    myLogger.writeLog(logType.warning, "controller <b>" + myController.SystemName + "</b> cleaned " + foldersDeleted.ToString() + " folders!");
                } else {
                    if (guiDemand) {
                        myLogger.writeLog(logType.warning, "controller <b>" + myController.SystemName + "</b> no folders to clean!");
                    }
                }
            }
        }

        private string createBackupPath(string outPath, string abbName, string backupSuffix, int desiredAction)
        {
            //create initial backup folder name
            string backupFolder = abbName + "_BACKUP_" + DateTime.Now.ToShortDateString() + backupSuffix;
            string result = outPath + "\\" + backupFolder;
            //check if final directory exists
            if (Directory.Exists(result)) {
                //path exists - check what we want to do
                if (desiredAction == (int)sameNameAction.overwrite) {
                    //we will overwrite backup - delete older one
                    Directory.Delete(result, true);
                } else if (desiredAction == (int)sameNameAction.increment) {
                    //we will add second folder with increment number - check how many folder there is
                    string[] folders = Directory.GetDirectories(outPath + "\\");
                    int currFolder = 0;
                    //find the newest file (it should be the one with last increment number
                    for (int i = 0; i < folders.Length; i++) {
                        if (folders[i].Contains(DateTime.Now.ToShortDateString())) {
                            //we found folder with the same date - find the same suffix
                            if (folders[i].Contains(backupSuffix)) {
                                int incrPos = folders[i].LastIndexOf("_");
                                string folderNo = folders[i].Substring(incrPos + 1);
                                //check if we have other incremented folder
                                if (folderNo.Length <= 3 && folderNo.All(char.IsDigit)) {
                                    //another incremented folder - check if its bigger then current
                                    int currNumber = int.Parse(folderNo);
                                    currFolder = currNumber > currFolder ? currNumber : currFolder;
                                }
                            }
                        }
                    }
                    //update suffix with next number
                    backupFolder += "_" + (++currFolder).ToString();
                    //update backup path
                    result = outPath + "\\" + backupFolder;
                } else if (desiredAction == (int)sameNameAction.additTime) {
                    //we will add second folder with current time
                    backupFolder += "_" + DateTime.Now.ToLongTimeString();
                    //replace time colon with dash
                    backupFolder = backupFolder.Replace(":", ";");
                    //update backup path
                    result = outPath + "\\" + backupFolder;
                }
            }
            return result;
        }

        public bool timeExists(backupMaster master, timeType time)
        {
            bool result = false;

            if (master == backupMaster.pc) {
                if (time == timeType.last) {
                    result = !(pcLastBackupTime == null || pcLastBackupTime == emptyTime);
                } else if (time == timeType.exact) {
                    result = !(pcDailyTime == null || pcDailyTime == emptyTime);
                }
            } else if (master == backupMaster.robot) {
                result = !(robotLastBackupTime == null || robotLastBackupTime == emptyTime);
            }
            return result;
        }

        public bool watchdog
        {
            get { return timerStart; }
            set { timerStart = value; }
        }

        public void activateMaster(bool pc, bool robot)
        {
            backupPC.activated = pc;
            backupRobot.activated = robot;
        }

        public int clearDays
        {
            get { return daysClear; }
            set { daysClear = value; }
        }

        public string outputPath
        {
            get { return outputDir; }
            set { outputDir = value; }
        }

        /********************************************************
         ***  BACKUP MANAGER - ROBOT
         ********************************************************/

        public void updateRobotSignals(string sigNameExe, string sigNameInP)
        {
            //if signals exists then unsubscribe because signal may change
            if (mySigExe != null) mySigExe.Changed -= DoBackup_Changed;
            if (mySigInP != null) mySigInP.Changed -= DiBackup_Changed;
            //update backup robot object with new signal names
            backupRobot.sigExecute = sigNameExe;
            backupRobot.sigInProgress = sigNameInP;
            //update signal objects
            if (myController != null) {
                //update signal "DO BACKUP" (if defined)
                if (backupRobot.sigExecute != "") {
                    mySigExe = myController.IOSystem.GetSignal(backupRobot.sigExecute);
                    if (mySigExe != null) mySigExe.Changed += DoBackup_Changed;
                } else {
                    mySigExe = null;
                }
                //update signal "BACKUP IN PROGRESS"
                if (backupRobot.sigInProgress != "") {
                    mySigInP = myController.IOSystem.GetSignal(backupRobot.sigInProgress);
                } else {
                    mySigInP = null;
                }
            }
        }

        private void DoBackup_Changed(object sender, SignalChangedEventArgs e)
        {
            if (e.NewSignalState.Value == 1) {
                if (mySigInP != null) {
                    mySigInP.Changed += DiBackup_Changed;
                    myLogger.writeLog(logType.info, "controller <b>" + myController.SystemName + "</b>" +
                                                     " [BACKUP MASTER]:  backup in progress - waiting for end...");
                    //robot backup state changed = doing backup - call event method
                    eventRobotBackupState(0);
                } else {
                    myLogger.writeLog(logType.error, "controller <b>" + myController.SystemName + "</b>" +
                                                     " [BACKUP MASTER]:  backup in progress - no signal defined!");
                }
            }
        }

        private void DiBackup_Changed(object sender, SignalChangedEventArgs e)
        {
            if (mySigExe.Value == 0 && e.NewSignalState.Value == 0)
            {
                mySigInP.Changed -= DiBackup_Changed;
                //check if source path is inputted
                if (backupRobot.srcDir != null && backupRobot.srcDir != "") {
                    //check if output path is created
                    if (outputDir != null && outputDir != "") {
                        if (timerStart) {
                            myLogger.writeLog(logType.info, "controller <b>" + myController.SystemName + "</b>" +
                                                             " [BACKUP MASTER]:  backup done - download in queue...");
                        } else {
                            myLogger.writeLog(logType.warning, "controller <b>" + myController.SystemName + "</b>" +
                                                             " [BACKUP MASTER]:  backup done - turn timer ON to queue download!");
                        }
                        //backup created - set flag to get it from robot
                        backupRobot.checkBackup = true;
                        backupRobot.lastBackupTime = DateTime.Now;
                        //robot backup state changed = wait for download - call event method
                        eventRobotBackupState(1);
                    } else {
                        //now output path - error
                        myLogger.writeLog(logType.error, "controller <b>" + myController.SystemName + "</b>" +
                                                          " [BACKUP MASTER]:  backup done - but no output directory selected!");
                    }
                } else {
                    //now output path - error
                    myLogger.writeLog(logType.error, "controller <b>" + myController.SystemName + "</b>" +
                                                      " [BACKUP MASTER]:  backup done - but no source directory selected!");
                }
            }
        }

        public void robotGetBackup()
        {
            //set remote directory to most top
            myController.FileSystem.RemoteDirectory = myController.GetEnvironmentVariable("SYSTEM");
            //check if user-specified directory exist in controller
            if (myController.FileSystem.DirectoryExists(backupRobot.srcDir)) {
                //find newest file
                string newestFile = robotGetNewestFile(myController, backupRobot.srcDir);
                if (newestFile == "") {
                    myLogger.writeLog(logType.error, "controller <b>" + myController.SystemName + "</b>" +
                                                 " [BACKUP MASTER]:  backup done - but no controller file found!");
                    return;
                }
                string backupSrc = backupRobot.srcDir + "/" + newestFile;
                //check if current path is existent
                string backupOut = createBackupPath(outputDir, myController.SystemName, backupRobot.suffix, backupRobot.duplicateMethod);
                //leave log for user
                myLogger.writeLog(logType.info, "controller <b>" + myController.SystemName + "</b>" +
                                                 " [BACKUP MASTER]:  backup done - download copy in progress...");
                //update robot backup time
                backupRobot.lastBackupTime = DateTime.Now;
                //robot backup state changed = download in progress - call event method
                eventRobotBackupState(2);
                //copy backup file
                myController.FileSystem.RemoteDirectory = myController.GetEnvironmentVariable("SYSTEM");
                IAsyncResult dirCopyRes = myController.FileSystem.BeginGetDirectory(backupSrc, backupOut, true, robotGetBackupCallback, null);
            }
        }

        private void robotGetBackupCallback(IAsyncResult res)
        {
            try {
                if (res.IsCompleted) {
                    myController.FileSystem.EndCopyDirectory(res);
                    myLogger.writeLog(logType.info, "controller " + myController.SystemName + "" +
                                                     " [BACKUP MASTER]:  backup done - download copy OK!");
                    //robot backup state changed = download OK - call event method
                    eventRobotBackupState(3);
                }
            } catch (Exception e) {
                //abb has internal problem in postprocess after copy instructions - check if its that case
                if (e.TargetSite.Name == "PostProcessCmd" && res.IsCompleted) {
                    myLogger.writeLog(logType.info, "controller " + myController.SystemName + "" +
                                                     " [BACKUP MASTER]:  backup done - download copy OK!");
                    //robot backup state changed = download OK - call event method
                    eventRobotBackupState(3);
                } else {
                    myLogger.writeLog(logType.error, "controller " + myController.SystemName + "" +
                                                         " [BACKUP MASTER]:  backup done - but exception thrown! " + e.Message);
                    //robot backup state changed = download ERROR - call event method
                    eventRobotBackupState(4);
                }
            }
        }

        private string robotGetNewestFile(Controller abb, string robotDir)
        {
            string result = "";
            DateTime youngest = new DateTime(1, 1, 1);

            abb.FileSystem.RemoteDirectory = abb.GetEnvironmentVariable("SYSTEM") + "/" + robotDir;
            ControllerFileSystemInfo[] dirs = abb.FileSystem.GetFilesAndDirectories("*");
            for (int i = 0; i < dirs.Length; i++) {
                if (dirs[i].GetType() == typeof(ControllerDirectoryInfo)) {
                    DateTime curr = abb.FileSystem.GetDirectoryCreationTime(dirs[i].Name);
                    if (curr >= youngest) {
                        result = dirs[i].Name;
                        youngest = curr;
                    }
                }
            }
            return result;
        }

        public DateTime robotLastBackupTime
        {
            get { return backupRobot.lastBackupTime; }
        }

        public string robotDirSrc
        {
            get { return backupRobot.srcDir; }
            set { backupRobot.srcDir = value; }
        }

        public string robotDirSuffix
        {
            get { return backupRobot.suffix; }
            set { backupRobot.suffix = value; }
        }

        public bool robotWatchBackup
        {
            get { return backupRobot.checkBackup; }
            set { backupRobot.checkBackup = value; }
        }

        public int duplicateMethodRobot
        {
            get { return backupRobot.duplicateMethod; }
            set { backupRobot.duplicateMethod = value; }
        }

        /********************************************************
         ***  BACKUP MANAGER - PC
         ********************************************************/

        public void createBackup(backupSource backupSrc, bool updateTime = true)
        {
            //check if controller is existing in network
            if (myController == null) {
                if (myLogger != null) myLogger.writeLog(logType.error, "abbTools - can't create backup! No controller connected...");
                return;
            }
            //check if output path is defined
            if (outputDir == null || outputDir == "") {
                if (myLogger != null) myLogger.writeLog(logType.error, "abbTools - can't create backup! No outputh path defined...");
                return;
            }
            //create full backup path
            string backupPath = createBackupPath(outputDir, myController.SystemName, backupPC.getSuffix(backupSrc), backupPC.duplicateMethod);
            //check if backup isnt currently in progress
            if (!myController.BackupInProgress) {
                //hide info about updating time after backup ok (to check it in event on backup done)
                myController.UICulture.NumberFormat.PositiveSign += "_" + updateTime.ToString();
                //subscribe to backup completed event
                myController.BackupCompleted -= backupDoneEvent;
                myController.BackupCompleted += backupDoneEvent;
                //everything is ok - do backup
                myController.Backup(backupPath);
                //pc backup state changed = backup in progress - call event method
                eventPCBackupState(0);
                //show log info
                if (myLogger != null) myLogger.writeLog(logType.info, "controller <b>" + myController.SystemName + "</b>: " +
                                                            "doing robot backup [" + DateTime.Now.ToShortTimeString() + "]!");
            } else {
                //backup in progress - inform user
                if (myLogger != null) myLogger.writeLog(logType.info, "controller <b>" + myController.SystemName + "</b>: " +
                                                            "backup in progress! Wait for end and retry [" + DateTime.Now.ToShortTimeString() + "]...");
            }
        }

        private void backupDoneEvent(object sender, BackupEventArgs e)
        {
            //get controller data
            Controller temp = (Controller)sender;
            //check if there is recent backup time (if not then update it)
            if (e.Succeeded) {
                //get info about update time (hidden in controller data)
                int checkPos = temp.UICulture.NumberFormat.PositiveSign.IndexOf("_");
                string update = temp.UICulture.NumberFormat.PositiveSign.Substring(checkPos + 1);
                //check if we want to update time 
                if (Boolean.Parse(update)) {
                    backupPC.lastBackupTime = DateTime.Now;
                    //pc backup state changed = backup done - call event method
                    eventPCBackupState(1);
                }
                //restore controller application variable
                temp.UICulture.NumberFormat.PositiveSign = temp.UICulture.NumberFormat.PositiveSign.Substring(0, checkPos);
                //show log info
                if (myLogger != null) myLogger.writeLog(logType.info, "controller <b>" + temp.SystemName + "</b>: " +
                                                            "auto backup done [" + DateTime.Now.ToShortTimeString() + "]!");
            } else {
                //show log info
                if (myLogger != null) myLogger.writeLog(logType.error, "controller <b>" + temp.SystemName + "</b>: " +
                                                            "backup error [" + DateTime.Now.ToShortTimeString() + "]...");
            }
        }

        public DateTime pcLastBackupTime
        {
            get { return backupPC.lastBackupTime; }
        }

        public DateTime pcDailyTime
        {
            get { return backupPC.exactTime; }
            set { backupPC.exactTime = value; }
        }

        public string pcDailySuffix
        {
            get { return backupPC.timeSuffix; }
            set { backupPC.timeSuffix = value; }
        }

        public int pcIntervalInMins
        {
            get { return backupPC.intervalTimeInMins(); }
        }

        public bool pcIntervalCheck()
        {
            return backupPC.intervalCheck();
        }

        public void pcIntervalSet(int mins, int hours, int days)
        {
            backupPC.intervalSet(mins, hours, days);
        }

        public string pcIntervalSuffix
        {
            get { return backupPC.intervalSuffix; }
            set { backupPC.intervalSuffix = value; }
        }

        public string pcGUISuffix
        {
            get { return backupPC.guiSuffix; }
            set { backupPC.guiSuffix = value; }
        }

        public int duplicateMethodPC
        {
            get { return backupPC.duplicateMethod; }
            set { backupPC.duplicateMethod = value; }
        }
    }
}
