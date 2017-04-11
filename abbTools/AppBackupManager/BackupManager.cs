using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
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
        private string controllerName;
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
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupManager()
        {
            timerStart = false;
            outputDir = "";
            controllerName = "";
            myController = null;
            myLogger = null;
            backupPC = new BackupMasterPC();
            backupRobot = new BackupMasterRobot();
            emptyTime = new DateTime(1, 1, 1);
        }

        /// <summary>
        /// Constructor with filling some basic data objects
        /// </summary>
        /// <param name="watchOn">If timer should be started at start</param>
        /// <param name="outPath">Output path</param>
        public BackupManager(bool watchOn, string outPath)
        {
            timerStart = watchOn;
            outputDir = outPath;
            controllerName = "";
            myController = null;
            myLogger = null;
            backupPC = new BackupMasterPC();
        }

        /// <summary>
        /// Method to clear all object data
        /// </summary>
        public void clearData()
        {
            timerStart = false;
            daysClear = 0;
            outputDir = "";
            backupPC.clearData();
            backupRobot.clearData();
        }

        /// <summary>
        /// GET and SET controller object in current instance
        /// </summary>
        public Controller controller
        {
            get {
                return myController;
            }
            set {
                myController = value;
                if (myController!=null) controllerName = myController.SystemName;
            }
        }

        public string abbName
        {
            get { return controllerName; }
        }

        /// <summary>
        /// GET and SET logger object in current instance
        /// </summary>
        public loggerABB logger
        {
            get { return myLogger; }
            set { myLogger = value; }
        }

        /********************************************************
         ***  BACKUP MANAGER - EVENTS
         ********************************************************/

        /// <summary>
        /// Event triggered at change of backup state taken by robot
        /// </summary>
        /// <param name="newState">New backup state</param>
        protected void eventRobotBackupState(int newState)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            RobotBackupStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// Event triggered at change of backup state taken by PC
        /// </summary>
        /// <param name="newState"></param>
        protected void eventPCBackupState(int newState)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            PCBackupStateChanged?.Invoke(newState);
        }

        /********************************************************
         ***  BACKUP MANAGER - COMMON
         ********************************************************/

        /// <summary>
        /// Method used to clear output dir from folders older than setted number of days
        /// </summary>
        /// <param name="guiDemand">If this method was called by GUI (can be called by timer event also)</param>
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

        /// <summary>
        /// Function used to create FULL backup path to save output files
        /// </summary>
        /// <param name="outPath">User defined output folder to store all backups</param>
        /// <param name="abbName">Name of parent abb controller to do/take backup from</param>
        /// <param name="backupSuffix">Suffix of backup file</param>
        /// <param name="desiredAction">Action to do when backup name duplicates</param>
        /// <returns>Full output path in string</returns>
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

        /// <summary>
        /// Function used to check if selected time is defined in current instance
        /// </summary>
        /// <param name="master">Select backup master: PC or ROBOT</param>
        /// <param name="time">Select time to check: LAST or EXACT</param>
        /// <returns>TRUE if selected time is defined in object</returns>
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

        /// <summary>
        /// GET or SET current instance watchdog timer settings (ON or OFF)
        /// </summary>
        public bool watchdog
        {
            get { return timerStart; }
            set { timerStart = value; }
        }

        /// <summary>
        /// Method used to activate or deactivate PC and ROBOT backup master
        /// </summary>
        /// <param name="pc">Set PC master active (TRUE) or inactive (FALSE)</param>
        /// <param name="robot">Set ROBOT master active (TRUE) or inactive (FALSE)</param>
        public void activateMaster(bool pc, bool robot)
        {
            backupPC.activated = pc;
            backupRobot.activated = robot;
        }

        /// <summary>
        /// GET activation status of PC BACKUP MASTER
        /// </summary>
        public bool pcMasterActive
        {
            get { return backupPC.activated; }
        }

        /// <summary>
        /// GET activation status of ROBOT BACKUP MASTER
        /// </summary>
        public bool robotMasterActive
        {
            get { return backupRobot.activated; }
        }

        /// <summary>
        /// GET or SET max number of days for which backups will not be deleted
        /// </summary>
        public int clearDays
        {
            get { return daysClear; }
            set { daysClear = value; }
        }

        /// <summary>
        /// GET or SET current instances' output path to store backups
        /// </summary>
        public string outputPath
        {
            get { return outputDir; }
            set { outputDir = value; }
        }

        /********************************************************
         ***  BACKUP MANAGER - ROBOT
         ********************************************************/

        /// <summary>
        /// Method to update Signal object from inputted strings
        /// </summary>
        /// <param name="sigNameExe">Name of signal responsible for BACKUP EXE ACTION</param>
        /// <param name="sigNameInP">Name of signal responsible for informing about BACKUP IN PROGRESS</param>
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

        /// <summary>
        /// Event triggered when EXE BACKUP signal state changes
        /// </summary>
        /// <param name="sender">Which signal triggered this event</param>
        /// <param name="e">Event arguments (new state, etc.)</param>
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

        /// <summary>
        /// Event triggered when BACKUP IN PROGRESS signal state changes
        /// </summary>
        /// <param name="sender">Which signal triggered this event</param>
        /// <param name="e">Event arguments (new state, etc.)</param>
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

        /// <summary>
        /// Method used to start downloading backup files from robot (ASYNC)
        /// </summary>
        public void robotGetBackup()
        {
            if (myController != null) {
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
        }

        /// <summary>
        /// Callback from downloading backup function (carries info about ASYNC download result!)
        /// </summary>
        /// <param name="res">Result of backup download from robot</param>
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

        /// <summary>
        /// Function used to get newest file in current directory
        /// </summary>
        /// <param name="abb">ABB controller object to get newest file from</param>
        /// <param name="robotDir">Directory to search newest file/dir</param>
        /// <returns>Name of newest directory (empty when no directory exists)</returns>
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

        /// <summary>
        /// GET last robot backup time
        /// </summary>
        public DateTime robotLastBackupTime
        {
            get { return backupRobot.lastBackupTime; }
        }

        /// <summary>
        /// GET or SET robot backup source directory to scan for new backups
        /// </summary>
        public string robotDirSrc
        {
            get { return backupRobot.srcDir; }
            set { backupRobot.srcDir = value; }
        }

        /// <summary>
        /// GET or SET robot backup file suffix for current instance
        /// </summary>
        public string robotDirSuffix
        {
            get { return backupRobot.suffix; }
            set { backupRobot.suffix = value; }
        }

        /// <summary>
        /// GET or SET info about watching robot backup status (backup done - wait for end and donwload)
        /// </summary>
        public bool robotWatchBackup
        {
            get { return backupRobot.checkBackup; }
            set { backupRobot.checkBackup = value; }
        }

        /// <summary>
        /// GET or SET method to carry with ROBOT backups with duplicate name
        /// </summary>
        public int duplicateMethodRobot
        {
            get { return backupRobot.duplicateMethod; }
            set { backupRobot.duplicateMethod = value; }
        }

        /********************************************************
         ***  BACKUP MANAGER - PC
         ********************************************************/

        /// <summary>
        /// Method used to create backup from robot controller directly to PC master
        /// </summary>
        /// <param name="backupSrc">Source of backup creation demand (GUI, DAILY or INTERVAL)</param>
        /// <param name="updateTime">Check if update time in GUI after backup is finished</param>
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

        /// <summary>
        /// Event triggered when backup creation is finished
        /// </summary>
        /// <param name="sender">Controller which backup was created</param>
        /// <param name="e">Event arguments (success, etc.)</param>
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
                                                            "backup done [" + DateTime.Now.ToShortTimeString() + "]!");
            } else {
                //show log info
                if (myLogger != null) myLogger.writeLog(logType.error, "controller <b>" + temp.SystemName + "</b>: " +
                                                            "backup error [" + DateTime.Now.ToShortTimeString() + "]...");
            }
        }

        /// <summary>
        /// GET last PC backup time
        /// </summary>
        public DateTime pcLastBackupTime
        {
            get { return backupPC.lastBackupTime; }
        }

        /// <summary>
        /// GET or SET exact daily backup time
        /// </summary>
        public DateTime pcDailyTime
        {
            get { return backupPC.exactTime; }
            set { backupPC.exactTime = value; }
        }

        /// <summary>
        /// GET or SET suffix of backup created at exact time (DAILY)
        /// </summary>
        public string pcDailySuffix
        {
            get { return backupPC.timeSuffix; }
            set { backupPC.timeSuffix = value; }
        }

        /// <summary>
        /// GET total interval time in minutes
        /// </summary>
        public int pcIntervalInMins
        {
            get { return backupPC.intervalTimeInMins(); }
        }

        /// <summary>
        /// Function used to check if any interval (!=0) is set
        /// </summary>
        /// <returns>TRUE when interval is set, FALSE otherwise</returns>
        public bool pcIntervalCheck()
        {
            return backupPC.intervalCheck();
        }

        /// <summary>
        /// Method used to set new interval time (time offset to do next backup)
        /// </summary>
        /// <param name="mins">Interval minutes</param>
        /// <param name="hours">Interval hours</param>
        /// <param name="days">Interval days</param>
        public void pcIntervalSet(int mins, int hours, int days)
        {
            backupPC.intervalSet(mins, hours, days);
        }

        /// <summary>
        /// GET or SET suffix of backup created at offset time (INTERVAL)
        /// </summary>
        public string pcIntervalSuffix
        {
            get { return backupPC.intervalSuffix; }
            set { backupPC.intervalSuffix = value; }
        }

        /// <summary>
        /// GET or SET suffix of backup created from user demand (GUI)
        /// </summary>
        public string pcGUISuffix
        {
            get { return backupPC.guiSuffix; }
            set { backupPC.guiSuffix = value; }
        }

        /// <summary>
        /// GET or SET method to carry with PC backups with duplicate name
        /// </summary>
        public int duplicateMethodPC
        {
            get { return backupPC.duplicateMethod; }
            set { backupPC.duplicateMethod = value; }
        }
    }

    //================================================================================================================================
    //================================================================================================================================
    //================================================================================================================================

    class BackupManagerCollection : List<BackupManager>
    {
        /********************************************************
         ***  BACKUP MANAGER COLLECTION - data
         ********************************************************/

        //default logger for all objects in collection
        private loggerABB defaultLogger;
        //events
        public event robotBackupState RobotBackupStateChanged;
        public event pcBackupState PCBackupStateChanged;
        public delegate void robotBackupState(int newState);
        public delegate void pcBackupState(int newState);

        /********************************************************
         ***  BACKUP MANAGER COLLECTION - object constructors
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupManagerCollection()
        {
            //initialize current data element
            defaultLogger = null;
            //clear all elements
            Clear();
        }

        /// <summary>
        /// Constructor with fill logger data
        /// </summary>
        /// <param name="newLogger">New logger data</param>
        public BackupManagerCollection(loggerABB newLogger)
        {
            //initialize current data element
            defaultLogger = newLogger;
            //clear all elements
            Clear();
        }

        /********************************************************
         ***  BACKUP MANAGER COLLECTION - parent management
         ********************************************************/

        /// <summary>
        /// Connect collection data with logger to output statuses
        /// </summary>
        /// <param name="logger">loggerABB for showing status</param>
        public void connectLogger(loggerABB logger)
        {
            defaultLogger = logger;
            foreach (BackupManager item in this) {
                item.logger = defaultLogger;
            }
        }

        /// <summary>
        /// Disconnect logger from collection data (no logging available)
        /// </summary>
        public void disconnectLogger()
        {
            defaultLogger = null;
            foreach (BackupManager item in this) {
                item.logger = defaultLogger;
            }
        }

        /********************************************************
         ***  BACKUP MANAGER COLLECTION - collection management
         ********************************************************/

        /// <summary>
        /// Get index of inputted item in whole collection
        /// </summary>
        /// <param name="cItem">Item to check index</param>
        /// <returns>ITEM INDEX in collection (-1 if item non-existent)</returns>
        private int itemIndex(Controller cItem)
        {
            int result = 0;
            //scan all collection
            foreach (BackupManager item in this) {
                //check what to search
                if (item.abbName == cItem.SystemName) {
                    break;
                } else {
                    result++;
                }
            }
            //check if something was found
            if (result >= Count) result = -1;
            //exit 
            return result;
        }

        /// <summary>
        /// Get index of inputted item name in whole collection
        /// </summary>
        /// <param name="cItemName">Item (controller) name to check index</param>
        /// <returns>ITEM INDEX in collection (-1 if item non-existent)</returns>
        private int itemIndex(string cItemName)
        {
            int result = 0;
            //scan all collection
            foreach (BackupManager item in this) {
                //check what to search
                if (item.abbName == cItemName) {
                    break;
                } else {
                    result++;
                }
            }
            //check if something was found
            if (result >= Count) result = -1;
            //exit 
            return result;
        }

        /// <summary>
        /// Check if inputted item exists in collection
        /// </summary>
        /// <param name="cItem">Item to check</param>
        /// <returns>TRUE if item exists (FALSE otherwise)</returns>
        public bool itemExists(Controller cItem)
        {
            return itemIndex(cItem) != -1;
        }

        /// <summary>
        /// Add inputted item to collection (internally checking no duplicates)
        /// </summary>
        /// <param name="cItem">Item to add to collection (checking no duplicates)</param>
        public BackupManager itemGet(Controller cItem)
        {
            BackupManager result = null;
            //check if current element exist in collection
            if (itemExists(cItem)) {
                result = this[itemIndex(cItem)];
                result.controller = cItem;
            } else {
                //item not existent - create new one
                result = new BackupManager();
                result.controller = cItem;
                result.logger = defaultLogger;
                //subscribe to its events
                result.PCBackupStateChanged += itemPCBackupStateChanged;
                result.RobotBackupStateChanged += itemRobotBackupStateChanged;
                //add item to collection
                this.Add(result);
            }

            return result;
        }

        /********************************************************
         ***  BACKUP MANAGER COLLECTION - data management
         ********************************************************/

        /// <summary>
        /// Clear all collection from data
        /// </summary>
        public void clear()
        {
            foreach (BackupManager cData in this) {
                //clear all messages
                cData.clearData();
                //unsubscribe events
                cData.PCBackupStateChanged -= itemPCBackupStateChanged;
                cData.RobotBackupStateChanged -= itemRobotBackupStateChanged;
            }
            //clear backup manager data collection
            Clear();
        }

        /// <summary>
        /// Method used to load collection data from XML subnode
        /// </summary>
        /// <param name="xmlSubnode">XML subnode containing data for collection</param>
        /// <param name="parent">ABB Controller parent of data (NULL if not visible in network)</param>
        /// <param name="parentName">ABB parent name useful to get data if parent not visible</param>
        public void loadFromXml(ref System.Xml.XmlReader xmlSubnode, Controller parent = null, string parentName = "")
        {
            //we should get xml subtree with robot name as parent node
            while (xmlSubnode.Read()) {
                if (xmlSubnode.Name.StartsWith("backupManager")) {

                }
            }
        }

        /// <summary>
        /// Method used to save collection data to XML subnode
        /// </summary>
        /// <param name="xmlSubnode">XML subnode to save collection data</param>
        /// <param name="nodeParentRobName">Subnode parent controller name</param>
        public void saveToXml(ref System.Xml.XmlWriter xmlSubnode, string nodeParentRobName)
        {
            xmlSubnode.WriteStartElement("backupManager");
            //check if current controller is on our list
            int cIndex = itemIndex(nodeParentRobName);
            if (cIndex >= 0) {
                //controller is on list - save common information
                xmlSubnode.WriteStartElement("common");
                //end client node
                xmlSubnode.WriteEndElement();
            }
            xmlSubnode.WriteEndElement();
        }

        /// <summary>
        /// Function used to add or update controller data in collection (for example: after load not visible - showed up!)
        /// </summary>
        /// <param name="newController">controller object to add/update in collection</param>
        /// <param name="storeControllerName">controller object to ADD (only) to collection</param>
        /// <returns>TRUE if adding or updating OK (FALSE otherwise)</returns>
        public bool controllerUpdate(Controller newController, string storeControllerName = "")
        {
            //check input data
            if (newController == null && storeControllerName == "") return false;
            //input data OK - add controller to collection
            try {
                
                //all ok - return true
                return true;
            }
            catch
            {
                //exception occured - return false
                return false;
            }
        }

        /// <summary>
        /// Method userd to zero controller data from collection (lost from network)
        /// </summary>
        /// <param name="toClear">Which controller was lost in network</param>
        public void controllerClear(Controller toClear)
        {
            //check input data
            if (toClear != null) {
                int cController = itemIndex(toClear);
                if (cController != -1) {
                    //invalidate existing controller
                    this[cController].controller = null;
                }
            } else {
                defaultLogger.writeLog(logType.error, "Cant clear collection controller null reference!");
            }
        }

        /********************************************************
         ***  IPC DATA COLLECTION - my events
         *********************************************************/

        /// <summary>
        /// method used only to pass robot backup state change to main GUI thread from current object
        /// </summary>
        /// <param name="newState">New robot backup state</param>
        protected void itemRobotBackupStateChanged(int newState)
        {
            //call this event (received from GUI)
            eventOnRobotStatusChange(newState);
        }

        /// <summary>
        /// Event triggered on change of robot backup
        /// </summary>
        /// <param name="updState">Updated backup state</param>
        protected void eventOnRobotStatusChange(int updState)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            RobotBackupStateChanged?.Invoke(updState);
        }

        /// <summary>
        /// method used only to pass PC backup state change to main GUI thread from current object
        /// </summary>
        /// <param name="newState">New PC backup state</param>
        protected void itemPCBackupStateChanged(int newState)
        {
            //call this event (received from GUI)
            eventOnPCStatusChange(newState);
        }

        /// <summary>
        /// Event triggered on change of PC backup
        /// </summary>
        /// <param name="updState">Updated backup state</param>
        protected void eventOnPCStatusChange(int updState)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            PCBackupStateChanged?.Invoke(updState);
        }
    }
}
