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

        /// <summary>
        /// GET and SET controller object in current instance
        /// </summary>
        public Controller controller { get; set; }

        /// <summary>
        /// GET and SET controller name in current instance
        /// </summary>
        public string controllerName { get; set; }

        /// <summary>
        /// GET and SET logger object in current instance
        /// </summary>
        public loggerABB logger { get; set; }

        /// <summary>
        /// GET or SET max number of days for which backups will not be deleted
        /// </summary>
        public int clearDays { get; set; }

        /// <summary>
        /// GET or SET current instances' output path to store backups
        /// </summary>
        public string outputDir { get; set; }

        /// <summary>
        /// GET or SET current instance watchdog timer settings (ON or OFF)
        /// </summary>
        public bool timer { get; set; }

        //internal data
        private DateTime emptyTime = new DateTime(1, 1, 1);
        private Signal mySigExe;
        private Signal mySigInP;
        private BackupMasterPC backupPC;
        private BackupMasterRobot backupRobot;
        //events
        public delegate void robotBackupState(int newState);
        public delegate void pcBackupState(int newState);
        public event robotBackupState RobotBackupStateChanged;
        public event pcBackupState PCBackupStateChanged;

        /********************************************************
         ***  BACKUP MANAGER - BASE 
         ********************************************************/
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public BackupManager()
        {
            //controller data
            controller = null;
            controllerName = "";
            //logger data
            logger = null;
            //backup masters
            backupPC = new BackupMasterPC();
            backupRobot = new BackupMasterRobot();
            //internal data
            timer = false;
            outputDir = "";
            clearDays = 0;
            mySigExe = null;
            mySigInP = null;
        }

        /// <summary>
        /// Constructor with filling some basic data objects
        /// </summary>
        /// <param name="watchOn">If timer should be started at start</param>
        /// <param name="outPath">Output path</param>
        public BackupManager(bool watchOn, string outPath)
        {
            //controller data
            controller = null;
            controllerName = "";
            //logger data
            logger = null;
            //backup masters
            backupPC = new BackupMasterPC();
            backupRobot = new BackupMasterRobot();
            //internal data
            timer = watchOn;
            outputDir = outPath;
            clearDays = 0;
            mySigExe = null;
            mySigInP = null;
        }

        /// <summary>
        /// Method to clear all object data
        /// </summary>
        public void clearData()
        {
            //clear controller data
            if (controller != null) {
                controller.BackupCompleted -= backupDoneEvent;
                controller = null;
            }
            controllerName = "";
            //clear logger data
            logger = null;
            //clear backup masters data
            backupPC.clearData();
            backupRobot.clearData();
            //reset internal data
            timer = false;
            clearDays = 0;
            outputDir = "";
            //clear signals data
            if (mySigExe != null) {
                mySigExe.Changed -= DoBackup_Changed;
                mySigExe = null;
            }
            if (mySigInP != null) {
                mySigInP.Changed -= DiBackup_Changed;
                mySigInP = null;
            }
        }

        /// <summary>
        /// Save backup masters data
        /// </summary>
        /// <param name="xmlSubnode">Subnode to save data to</param>
        public void saveMaterData(ref System.Xml.XmlWriter xmlSubnode)
        {
            //save robot master data
            backupRobot.saveData(ref xmlSubnode);
            //save pc master data
            backupPC.saveData(ref xmlSubnode);
        }

        /// <summary>
        /// Load backup masters data
        /// </summary>
        /// <param name="xmlSubnode">Subnode to load data from</param>
        public void loadMasterData(ref System.Xml.XmlReader xmlSubnode)
        {
            //save robot master data
            backupRobot.loadData(ref xmlSubnode);
            //save pc master data
            backupPC.loadData(ref xmlSubnode);
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
            if (controller == null) {
                logger?.writeLog(logType.error, "abbTools - can't clear folders! No controller connected...");
                return;
            }
            //check if output path is defined
            if (outputDir == null || outputDir == "") {
                logger?.writeLog(logType.error, "abbTools - can't clear folders! No outputh path defined...");
                return;
            }
            //get all directories in output dir
            if (Directory.Exists(outputDir + "\\")) {
                string[] folders = Directory.GetDirectories(outputDir + "\\");
                //compare every folder creation date with current date
                int foldersDeleted = 0;
                for (int i = 0; i < folders.Length; i++) {
                    //delete only backup files (contains _BACKUP_ string)
                    if (folders[i].Contains("_BACKUP_")) {
                        DateTime createdTime = Directory.GetCreationTime(folders[i]);
                        TimeSpan timeDiff = DateTime.Now - createdTime;
                        if (clearDays > 0 && timeDiff.TotalDays > clearDays) {
                            Directory.Delete(folders[i], true);
                            foldersDeleted++;
                        }
                    }
                }
                //show log if some folders were deleted
                if (foldersDeleted > 0) {
                    logger?.writeLog(logType.warning, $"controller <b>{controller.SystemName}</b> cleaned {foldersDeleted.ToString()} folders!");
                } else {
                    if (guiDemand) {
                        logger?.writeLog(logType.warning, $"controller <b>{controller.SystemName}</b> no folders to clean!");
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
                if (desiredAction == (int)BackupSettings.duplicate.overwrite) {
                    //we will overwrite backup - delete older one
                    Directory.Delete(result, true);
                } else if (desiredAction == (int)BackupSettings.duplicate.increment) {
                    //we will add second folder with increment number - check how many folder there is
                    string[] folders = Directory.GetDirectories(outPath + "\\");
                    int currFolder = 0;
                    //find the newest file (it should be the one with last increment number
                    for (int i = 0; i < folders.Length; i++) {
                        if (folders[i].Contains(abbName)) {
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
                    }
                    //update suffix with next number
                    backupFolder += "_" + (++currFolder).ToString();
                    //update backup path
                    result = outPath + "\\" + backupFolder;
                } else if (desiredAction == (int)BackupSettings.duplicate.additTime) {
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
        /// <param name="selMaster">Select backup master: PC or ROBOT</param>
        /// <param name="selTime">Select time to check: LAST or EXACT</param>
        /// <returns>TRUE if selected time is defined in object</returns>
        public bool timeExists(BackupSettings.master selMaster, BackupSettings.time selTime)
        {
            bool result = false;

            if (selMaster == BackupSettings.master.pc) {
                if (selTime == BackupSettings.time.last) {
                    result = !(pcLastBackupTime == null || pcLastBackupTime == emptyTime);
                } else if (selTime == BackupSettings.time.exact) {
                    result = !(pcDailyTime == null || pcDailyTime == emptyTime);
                }
            } else if (selMaster == BackupSettings.master.robot) {
                result = !(robotLastBackupTime == null || robotLastBackupTime == emptyTime);
            }
            return result;
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
            backupRobot.sigBackupExe = sigNameExe;
            backupRobot.sigBackupInP = sigNameInP;
            //update signal objects
            if (controller != null) {
                //update signal "DO BACKUP" (if defined)
                if (backupRobot.sigBackupExe != "") {
                    mySigExe = controller.IOSystem.GetSignal(backupRobot.sigBackupExe);
                    if (mySigExe != null) mySigExe.Changed += DoBackup_Changed;
                } else {
                    mySigExe = null;
                }
                //update signal "BACKUP IN PROGRESS"
                if (backupRobot.sigBackupInP != "") {
                    mySigInP = controller.IOSystem.GetSignal(backupRobot.sigBackupInP);
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
                    logger?.writeLog(logType.info, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]:  backup in progress - waiting for end...");
                    //robot backup state changed = doing backup - call event method
                    eventRobotBackupState(0);
                } else {
                    logger?.writeLog(logType.error, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]:  backup in progress - no signal defined!");
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
                if (backupRobot.sourceDir != null && backupRobot.sourceDir!= "") {
                    //check if output path is created
                    if (outputDir != null && outputDir != "") {
                        if (timer) {
                            logger?.writeLog(logType.info, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]: backup done - download in queue...");
                        } else {
                            logger?.writeLog(logType.warning, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]: backup done - turn timer ON to queue download!");
                        }
                        //backup created - set flag to get it from robot
                        backupRobot.checkBackup = true;
                        backupRobot.lastBackupTime = DateTime.Now;
                        //robot backup state changed = wait for download - call event method
                        eventRobotBackupState(1);
                    } else {
                        //now output path - error
                        logger?.writeLog(logType.error, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]: backup done - but no output directory selected!");
                    }
                } else {
                    //now output path - error
                    logger?.writeLog(logType.error, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]: backup done - but no source directory selected!");
                }
            }
        }

        /// <summary>
        /// Method used to start downloading backup files from robot (ASYNC)
        /// </summary>
        public void robotGetBackup()
        {
            if (controller != null) {
                //set remote directory to most top
                controller.FileSystem.RemoteDirectory = controller.GetEnvironmentVariable("SYSTEM");
                //check if user-specified directory exist in controller
                if (controller.FileSystem.DirectoryExists(backupRobot.sourceDir)) {
                    //find newest file
                    string newestFile = robotGetNewestFile(controller, backupRobot.sourceDir);
                    if (newestFile == "") {
                        logger?.writeLog(logType.error, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]: backup done - but no controller file found!");
                        return;
                    }
                    string backupSrc = backupRobot.sourceDir + "/" + newestFile;
                    //check if current path is existent
                    string backupOut = createBackupPath(outputDir, controller.SystemName, backupRobot.suffix, backupRobot.duplicateMethod);
                    //leave log for user
                    logger?.writeLog(logType.info, $"controller <b>{controller.SystemName}</b> [ROBOT MASTER]:  ackup done - download copy in progress...");
                    //update robot backup time
                    backupRobot.lastBackupTime = DateTime.Now;
                    //robot backup state changed = download in progress - call event method
                    eventRobotBackupState(2);
                    //copy backup file
                    controller.FileSystem.RemoteDirectory = controller.GetEnvironmentVariable("SYSTEM");
                    IAsyncResult dirCopyRes = controller.FileSystem.BeginGetDirectory(backupSrc, backupOut, true, robotGetBackupCallback, null);
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
                    controller.FileSystem.EndCopyDirectory(res);
                    logger?.writeLog(logType.info, $"controller {controller.SystemName} [ROBOT MASTER]: backup done - download copy OK!");
                    //robot backup state changed = download OK - call event method
                    eventRobotBackupState(3);
                }
            } catch (Exception e) {
                //abb has internal problem in postprocess after copy instructions - check if its that case
                if (e.TargetSite.Name == "PostProcessCmd" && res.IsCompleted) {
                    logger?.writeLog(logType.info, $"controller {controller.SystemName} [ROBOT MASTER]: backup done - download copy OK!");
                    //robot backup state changed = download OK - call event method
                    eventRobotBackupState(3);
                } else {
                    logger?.writeLog(logType.error, $"controller {controller.SystemName} [ROBOT MASTER]: backup done - but exception thrown {e.Message}!");
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
        /// GET robot signal - do backup execute
        /// </summary>
        public string robotSignalExe
        {
            get { return backupRobot.sigBackupExe; }
        }

        /// <summary>
        /// GET robot signal - backup in progress
        /// </summary>
        public string robotSignalInP
        {
            get { return backupRobot.sigBackupInP; }
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
            get { return backupRobot.sourceDir; }
            set { backupRobot.sourceDir = value; }
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
        public void createBackup(BackupSettings.source backupSrc, bool updateTime = true)
        {
            //check if controller is existing in network
            if (controller == null) {
                logger?.writeLog(logType.error, "abbTools - can't create backup! No controller connected...");
                return;
            }
            //check if output path is defined
            if (outputDir == null || outputDir == "") {
                logger?.writeLog(logType.error, "abbTools - can't create backup! No outputh path defined...");
                return;
            }
            //create full backup path
            string backupPath = createBackupPath(outputDir, controller.SystemName, backupPC.getSuffix(backupSrc), backupPC.duplicateMethod);
            //check if backup isnt currently in progress
            try {
                bool wasConnected = controller.CurrentUser != null;
                //check if we are logged to controller
                if (!wasConnected) controller.Logon(UserInfo.DefaultUser);
                //check if backup is already in progress
                if (!controller.BackupInProgress) {
                    //hide info about updating time after backup ok (to check it in event on backup done)
                    if (controller.UICulture.NumberFormat.PositiveSign.IndexOf("_") == -1) {
                        controller.UICulture.NumberFormat.PositiveSign += "_" + updateTime.ToString();
                    }
                    //subscribe to backup completed event
                    controller.BackupCompleted -= backupDoneEvent;
                    controller.BackupCompleted += backupDoneEvent;
                    //everything is ok - do backup
                    controller.Backup(backupPath);
                    //pc backup state changed = backup in progress - call event method
                    eventPCBackupState(0);
                    //show log info
                    logger?.writeLog(logType.info, $"controller <b>{controller.SystemName}</b> [PC MASTER]: doing robot backup ({DateTime.Now.ToShortTimeString()})!");
                } else {
                    //backup in progress - inform user
                    logger?.writeLog(logType.info, $"controller <b>{controller.SystemName}</b> [PC MASER]: backup in progress ({DateTime.Now.ToShortTimeString()})...");
                }
                //at end check if we werent connected - if yes then disconnect
                if (!wasConnected) controller.Logoff();
            } catch (Exception e) {
                logger?.writeLog(logType.error, $"controller <b>{controller.SystemName}</b> [PC MASTER]: backup exception {e.Message}!");
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
                bool updateTime = true;
                int checkPos = temp.UICulture.NumberFormat.PositiveSign.IndexOf("_");
                string update = temp.UICulture.NumberFormat.PositiveSign.Substring(checkPos + 1);
                //check if we want to update time 
                if (Boolean.TryParse(update,out updateTime) && updateTime) {
                    backupPC.lastBackupTime = DateTime.Now;
                    //pc backup state changed = backup done - call event method
                    eventPCBackupState(1);
                }
                //restore controller application variable
                temp.UICulture.NumberFormat.PositiveSign = temp.UICulture.NumberFormat.PositiveSign.Substring(0, checkPos);
                temp.BackupCompleted -= backupDoneEvent;
                //show log info
                logger?.writeLog(logType.info, $"controller <b>{temp.SystemName}</b> [PC MASTER]: backup done ({DateTime.Now.ToShortTimeString()})!");
            } else {
                //show log info
                logger?.writeLog(logType.error, $"controller <b>{temp.SystemName}</b> [PC MASTER]: backup error ({DateTime.Now.ToShortTimeString()})...");
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
        /// <param name="element">Interval element to set (min, hour or day)</param>
        /// <param name="value">Interval element value</param>
        public void pcIntervalSet(BackupSettings.interval element, int value)
        {
            backupPC.intervalSet(element, value);
        }

        /// <summary>
        /// Method used to get interval element time (minutes, hours OR days)
        /// </summary>
        /// <param name="element">Interval element to get (mins, hours or days)</param>
        /// <returns>Interval element time</returns>
        public int pcIntervalGet(BackupSettings.interval element)
        {
            return backupPC.intervalGet(element);
        }

        /// <summary>
        /// GET or SET suffix of backup created at offset time (INTERVAL)
        /// </summary>
        public string pcIntervalSuffix
        {
            get { return backupPC.suffixInterval; }
            set { backupPC.suffixInterval = value; }
        }

        /// <summary>
        /// GET or SET suffix of backup created from user demand (GUI)
        /// </summary>
        public string pcGUISuffix
        {
            get { return backupPC.suffixGUI; }
            set { backupPC.suffixGUI = value; }
        }

        /// <summary>
        /// GET or SET suffix of backup created at exact time (DAILY)
        /// </summary>
        public string pcDailySuffix
        {
            get { return backupPC.suffixTime; }
            set { backupPC.suffixTime = value; }
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

        public int itemWatchedNo()
        {
            int result = 0;
            //scan all collection
            foreach (BackupManager item in this) {
                //check what to search
                if (item.timer) result++;
            }
            return result;
        }

        /// <summary>
        /// Get index of inputted item in whole collection
        /// </summary>
        /// <param name="cItem">Item to check index</param>
        /// <returns>ITEM INDEX in collection (-1 if item non-existent)</returns>
        private int itemIndex(Controller cItem)
        {
            int result = 0;
            //check if inputted item exists
            if (cItem != null) {
                //scan all collection
                foreach (BackupManager item in this) {
                    //check what to search
                    if (item.controllerName == cItem.SystemName) {
                        break;
                    } else {
                        result++;
                    }
                }
                //check if something was found
                if (result >= Count) result = -1;
            } else {
                result = -1;
            }
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
                if (item.controllerName == cItemName) {
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
        public bool itemExists(Controller cItem, string cItemName= "")
        {
            bool result = false;
            if (cItem != null) {
                result = itemIndex(cItem) != -1;
            } else {
                result = itemIndex(cItemName) != -1;
            }
            return result;
        }

        /// <summary>
        /// Add inputted item to collection (internally checking no duplicates)
        /// </summary>
        /// <param name="cItem">Item to add to collection (checking no duplicates)</param>
        public BackupManager itemGet(Controller cItem, string cItemName="")
        {
            BackupManager result = null;
            //check if current element exist in collection
            if (itemExists(cItem, cItemName)) {
                result = (cItem != null) ? this[itemIndex(cItem)] : this[itemIndex(cItemName)];
                result.controller = cItem;
            } else {
                //item not existent - create new one
                result = new BackupManager();
                if (cItem == null) {
                    result.controllerName = cItemName;
                } else {
                    result.controller = cItem;
                    result.controllerName = cItem.SystemName;
                }
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
            //scan all collection
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
                    if (xmlSubnode.IsStartElement() && !xmlSubnode.IsEmptyElement) {
                        //create new object in collection (pass string arg if controller not visible [null])
                        BackupManager loadItem = itemGet(parent,parentName);
                        if (loadItem != null) {
                            //get backup manager item data
                            while (xmlSubnode.Read()) {
                                bool start = xmlSubnode.NodeType == System.Xml.XmlNodeType.Element,
                                     outputData = xmlSubnode.Name.StartsWith("output");
                                //if we are starting to read client data then get it
                                if (start && outputData){
                                    loadItem.outputDir = xmlSubnode.GetAttribute("dir");
                                    loadItem.clearDays = int.Parse(xmlSubnode.GetAttribute("clr"));
                                    loadItem.timer = bool.Parse(xmlSubnode.GetAttribute("watch"));
                                    //break from WHILE loop - now will be masters data
                                    break;
                                }
                            }
                            //check if there is something to read
                            if (!xmlSubnode.EOF) loadItem.loadMasterData(ref xmlSubnode);
                            //update current element signals
                            loadItem.updateRobotSignals(loadItem.robotSignalExe, loadItem.robotSignalInP);
                        }
                    } else {
                        break;
                    }
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
                //controller is on list - save backup manager data
                xmlSubnode.WriteStartElement("output");
                //output directory
                xmlSubnode.WriteAttributeString("dir", this[cIndex].outputDir);
                //clear output days number
                xmlSubnode.WriteAttributeString("clr", this[cIndex].clearDays.ToString());
                //watchdog activated
                xmlSubnode.WriteAttributeString("watch", this[cIndex].timer.ToString());
                //end client node
                xmlSubnode.WriteEndElement();
                //save all messages in collection
                this[cIndex].saveMaterData(ref xmlSubnode);
                
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
            //input data OK - get controller in collection
            try {
                int foundIndex = -1;
                if (newController != null) {
                    foundIndex = itemIndex(newController);
                    //update controller address of selected controller
                    this[foundIndex].controller = newController;
                }
                else {
                    foundIndex = itemIndex(storeControllerName);
                    //update controller address of selected controller
                    this[foundIndex].controller = null;
                }
                //all ok - return true
                return true;
            } catch {
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
