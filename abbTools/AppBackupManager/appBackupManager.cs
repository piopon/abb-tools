using abbTools.Windows;
using System;
using System.Windows.Forms;
using ABB.Robotics.Controllers;

namespace abbTools.AppBackupManager
{
    public partial class appBackupManager : UserControl
    {
        /********************************************************
         ***  APP BACKUP MANAGER - data
         ********************************************************/

        //data containers
        BackupManagerCollection myCollection = null;
        BackupManager currData = null;
        loggerABB abbLogger = null;
        //additional windows & settings
        public int parentHeight;
        public int parentWidth;
        int doBackupIndex;
        int diBackupIndex;
        windowRobotSig signalsWindow;
        windowRobotFiles filesWindow;
        //event
        public delegate void updateXMLFile();
        public event updateXMLFile UpdateBackupTimeInXML;

        /********************************************************
         ***  APP IPC - manage connection data containers
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public appBackupManager()
        {
            //init all form components
            InitializeComponent();
            //init data containers
            abbLogger = null;
            myCollection = new BackupManagerCollection();
            myCollection.PCBackupStateChanged += updatePCBackupState;
            myCollection.RobotBackupStateChanged += updateRobotBackupState;
            //init internal data
            doBackupIndex = -1;
            diBackupIndex = -1;
            //clear robot data (no robots yet)
            clearData();
            //stop timer
            timerCheckBackup.Stop();
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Synchronize logging component address with current app
        /// </summary>
        /// <param name="myLogger">logging component address</param>
        public void syncLogger(loggerABB myLogger)
        {
            //update logging components address
            abbLogger = myLogger;
            myCollection.connectLogger(abbLogger);
        }

        /// <summary>
        /// Desynchronize logger from current app (no logging available)
        /// </summary>
        public void desyncLogger()
        {
            //clear controller logger
            abbLogger = null;
            myCollection.disconnectLogger();
        }

        /// <summary>
        /// Synchronize ABB controller (called on controller connect!)
        /// </summary>
        /// <param name="myController">ABB controller to synchronize with</param>
        public void syncController(Controller myController)
        {
            //update controller address
            currData = myCollection.itemGet(myController);
            //get data from collection
            updateData(currData.controller);
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Desynchronize current ABB controller from app (no controller actions available)
        /// </summary>
        public void desyncController()
        {
            currData = null;
            //disconnect all info about current controller
            signalsWindow = null;
            filesWindow = null;
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Clear of ALL applciation data (ALL COLLECTION!!!)
        /// </summary>
        void clearData()
        {
            myCollection.clear();
        }

        /// <summary>
        /// Function used to update GUI data from collection (or add new one) when ABB controller changes
        /// </summary>
        /// <param name="cController">New ABB controller object - parent of data to view</param>
        void updateData(Controller cController)
        {
            //update new controllers signals in background
            if (signalsWindow == null) {
                //init internal signal window
                signalsWindow = new windowRobotSig();
                signalsWindow.StartPosition = FormStartPosition.CenterParent;
                signalsWindow.ShowInTaskbar = false;
            }
            signalsWindow.Height = parentHeight;
            signalsWindow.Width = parentWidth;
            signalsWindow.updateSignals(currData.controller);
            //update controllers file structure in background
            if (filesWindow == null) {
                //init internal files window
                filesWindow = new windowRobotFiles();
                filesWindow.StartPosition = FormStartPosition.CenterParent;
                filesWindow.ShowInTaskbar = false;
            }
            filesWindow.Height = parentHeight;
            filesWindow.Width = parentWidth;
            filesWindow.updateFiles(currData.controller);
        }

        /********************************************************
         ***  APP BACKUP MANAGER - EVENT METHODS
         ********************************************************/

        /// <summary>
        /// Function used when BackupManager collection event triggered (change status of ROBOT BACKUP)
        /// </summary>
        /// <param name="newState">New state on doing backup when ROBOT is master</param>
        private void updateRobotBackupState(int newState)
        {
            labelBackupStatus.ImageIndex = newState;
            if (newState == 2) labelLastTimeROB.Text = currData.robotLastBackupTime.ToString();
        }

        /// <summary>
        /// Function used when BackupManager collection event triggered (change status of PC BACKUP)
        /// </summary>
        /// <param name="newState">New state on doing backup when PC is master</param>
        private void updatePCBackupState(int newState)
        {
            if (currData != null && newState == 1) labelLastTimePC.Text = currData.pcLastBackupTime.ToString();
        }

        /// <summary>
        /// Event triggered at update last backup time in xml file
        /// </summary>
        protected void updateBackupTime()
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            UpdateBackupTimeInXML?.Invoke();
        }

        /********************************************************
         ***  APP BACKUP MANAGER - main window data management 
         ********************************************************/

        /// <summary>
        /// Reset appBackupManager state (clear all stored data)
        /// </summary>
        public void clearAppData()
        {
            clearData();
        }

        /// <summary>
        /// Save data to XML file
        /// </summary>
        /// <param name="saveXml">XML file (node) to save data to</param>
        /// <param name="parent">ABB controller who is parent of current saving data</param>
        /// <param name="parentName">Parent name (useful when controller was lost and some data is stored)</param>
        public void saveAppData(ref System.Xml.XmlWriter saveXml, Controller parent = null, string parentName = "")
        {
            //save current robot child node to XML document
            string saveName = parent != null ? parent.SystemName : parentName;
            if (saveName.Length > 0) {
                //save backup manager data
                myCollection.saveToXml(ref saveXml, saveName);
            } else {
                abbLogger.writeLog(logType.error, "cant save controller(s) without specified name...");
            }
        }

        /// <summary>
        /// Load application data from XML file
        /// </summary>
        /// <param name="loadXml">File (node) to load data from</param>
        /// <param name="parent">ABB controller parent who is parent of current data</param>
        /// <param name="parentName">Parent name (important if its not visible during load)</param>
        public void loadAppData(ref System.Xml.XmlReader loadXml, Controller parent = null, string parentName = "")
        {
            //read XML untill current app settings node appears
            while (loadXml.Read()) {
                bool start = loadXml.NodeType == System.Xml.XmlNodeType.Element,
                     backupManager = loadXml.Name.StartsWith("backupManager");
                //if we are starting to read windowsIPC app setting then break from WHILE loop
                if (start && backupManager) {
                    //if current element is empty then return
                    if (loadXml.IsEmptyElement) return;
                    //if element not empty then load its data
                    break;
                }
                //if we are at end of current robot then dont read anythig
                if (loadXml.Name.StartsWith("robot_") && loadXml.NodeType == System.Xml.XmlNodeType.EndElement) return;
            }
            //read next element (this one will be with robot info
            System.Xml.XmlReader nodeCurrRobot = loadXml.ReadSubtree();
            //read every child node from XML document (stopped at reading robot)
            myCollection.loadFromXml(ref nodeCurrRobot, parent, parentName);
            //start timer - if at least one controller has to be watched
            if (myCollection.itemWatchedNo() > 0) timerCheckBackup.Start();
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Action executed when some controller from saved group was found (showed-up in network) 
        /// </summary>
        /// <param name="found"></param>
        public void savedControllerFound(Controller found)
        {
            //update controllers data in collection
            if (found != null) {
                myCollection.controllerUpdate(found, found.SystemName);
            }
        }

        /// <summary>
        /// Action executed when some controller was lost (non-visible in network) during program execution
        /// </summary>
        /// <param name="lost">ABB controller object that was lost</param>
        public void savedControllerLost(Controller lost)
        {
            if (lost != null) {
                if (currData.controller != null && lost.SystemName == currData.controller.SystemName) {
                    myCollection.controllerClear(lost);
                    //reset GUI
                    resetGUI();
                }
            }
        }

        /********************************************************
         ***  APP BACKUP MANAGER - GUI COMMON
         ********************************************************/

        /// <summary>
        /// Reset GUI components to current state (initial or updated robot)
        /// </summary>
        public void resetGUI()
        {
            bool enablePC = false, enableROB = false;
            //check if robot is connected
            if (currData != null && currData.controller != null) {
                enablePC = currData.pcMasterActive;
                enableROB = currData.robotMasterActive;
            } else {
                //hide robot data
                enablePC = false;
                enableROB = false;
            }
            //do enable/disable PC group
            if (enablePC) {
                checkPCactive.Checked = true;
                groupPCmaster.Enabled = true;
                if (currData.timeExists(backupMaster.pc, timeType.last)) {
                    labelLastTimePC.Text = currData.pcLastBackupTime.ToString();
                } else {
                    labelLastTimePC.Text = "-";
                }
                btnBackupExe.BackColor = System.Drawing.Color.Chartreuse;
                if (currData.timeExists(backupMaster.pc, timeType.exact)) {
                    textEveryTime.Text = currData.pcDailyTime.ToShortTimeString();
                } else {
                    textEveryTime.Text = "";
                }
                numIntervalDays.Value = currData.pcIntervalGet(intervalElement.days);
                numIntervalHours.Value = currData.pcIntervalGet(intervalElement.hours);
                numIntervalMins.Value = currData.pcIntervalGet(intervalElement.mins);
            } else {
                checkPCactive.Checked = false;
                groupPCmaster.Enabled = false;
                labelLastTimePC.Text = "-";
                btnBackupExe.BackColor = System.Drawing.Color.Silver;
                textEveryTime.Text = "";
                numIntervalDays.Value = 0;
                numIntervalHours.Value = 0;
                numIntervalMins.Value = 0;
            }
            //do enable/disable ROBOT group
            if (enableROB) {
                checkRobActive.Checked = true;
                groupRobMaster.Enabled = true;
                if (currData.timeExists(backupMaster.robot, timeType.last)) {
                    labelLastTimeROB.Text = currData.robotLastBackupTime.ToString();
                } else {
                    labelLastTimeROB.Text = "-";
                }
                //dir source
                if (currData.robotDirSrc != "") {
                    labelRobBackupDir.ImageIndex = 1;
                } else {
                    labelRobBackupDir.ImageIndex = 0;
                }
                //signals
                if (currData.robotSignalExe != "") {
                    textSigDoBackup.Text = currData.robotSignalExe;
                } else {
                    textSigDoBackup.Text = "- select signal -";
                }
                if (currData.robotSignalInP != "") {
                    textSigBackupProg.Text = currData.robotSignalInP;
                } else {
                    textSigBackupProg.Text = "- select signal -";
                }
            } else {
                checkRobActive.Checked = false;
                groupRobMaster.Enabled = false;
                labelBackupStatus.ImageIndex = -1;
                labelLastTimeROB.Text = "-";
                labelRobBackupDir.ImageIndex = 0;
                textSigDoBackup.Text = "- select signal -";
                textSigBackupProg.Text = "- select signal -";
            }
            //check if directory was selected
            if (currData != null && currData.robotDirSrc != null) {
                labelRobBackupDir.ImageIndex = currData.robotDirSrc != "" ? 1 : 0;
            } else {
                labelRobBackupDir.ImageIndex = 0;
            }
            //fill independent data 
            if (currData != null && currData.controller != null) {
                //TextBox suffixes
                textGuiSuffix.Text = currData.pcGUISuffix;
                textIntervalSuffix.Text = currData.pcIntervalSuffix;
                textDailySuffix.Text = currData.pcDailySuffix;
                textRobotSuffix.Text = currData.robotDirSuffix;
                //RadioButton duplicates PC
                radioPCOverwrite.Checked = currData.duplicateMethodPC == (int)sameNameAction.overwrite;
                radioPCIncr.Checked = currData.duplicateMethodPC == (int)sameNameAction.increment;
                radioPCTime.Checked = currData.duplicateMethodPC == (int)sameNameAction.additTime;
                //RadioButton duplicates ROBOT
                radioROBOverwrite.Checked = currData.duplicateMethodRobot == (int)sameNameAction.overwrite;
                radioROBIncr.Checked = currData.duplicateMethodRobot == (int)sameNameAction.increment;
                radioROBTime.Checked = currData.duplicateMethodRobot == (int)sameNameAction.additTime;
            } else {
                //TextBox suffixes
                textGuiSuffix.Text = "";
                textIntervalSuffix.Text = "";
                textDailySuffix.Text = "";
                textRobotSuffix.Text = "";
                //RadioButton duplicates PC
                radioPCOverwrite.Checked = true;
                radioPCIncr.Checked = false;
                radioPCTime.Checked = false;
                //RadioButton duplicates ROBOT
                radioROBOverwrite.Checked = true;
                radioROBIncr.Checked = false;
                radioROBTime.Checked = false;
            }
            //do enable/disable COMMON group
            if (currData != null && currData.controller != null) {
                groupCommonSettings.Enabled = true;
                if (currData.outputPath != "") {
                    labelOutPathVal.Text = currData.outputPath;
                } else {
                    labelOutPathVal.Text = "-";
                }
                numClearDays.Value = currData.clearDays;
                btnOutSelect.BackColor = System.Drawing.Color.DarkOrange;
                btnOutShow.BackColor = System.Drawing.Color.DarkOrange;
                btnCleanExe.BackColor = System.Drawing.Color.OrangeRed;
                if (currData.watchdog) {
                    btnWatchOn.Enabled = false;
                    btnWatchOn.BackColor = System.Drawing.Color.Silver;
                    btnWatchOff.Enabled = true;
                    btnWatchOff.BackColor = System.Drawing.Color.Red;
                } else {
                    btnWatchOn.Enabled = true;
                    btnWatchOn.BackColor = System.Drawing.Color.LimeGreen;
                    btnWatchOff.Enabled = false;
                    btnWatchOff.BackColor = System.Drawing.Color.Silver;
                }
            } else {
                groupCommonSettings.Enabled = false;
                labelOutPathVal.Text = "-";
                numClearDays.Value = 0;
                btnOutSelect.BackColor = System.Drawing.Color.Silver;
                btnOutShow.BackColor = System.Drawing.Color.Silver;
                btnCleanExe.BackColor = System.Drawing.Color.Silver;
                btnWatchOn.BackColor = System.Drawing.Color.Silver;
                btnWatchOff.BackColor = System.Drawing.Color.Silver;
            }
        }

        /// <summary>
        /// Event executed on every interval of timer (every 60s)
        /// </summary>
        /// <param name="sender">Timer that triggered event (timerCheckBackup)</param>
        /// <param name="e">Event arguments</param>
        private void timerCheckBackup_Tick(object sender, EventArgs e)
        {
            //get current time
            DateTime now = DateTime.Now;
            foreach (BackupManager item in myCollection) {           
                //check if controller is existing in network
                if (item.controller == null) {
                    if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - watch action error! No controller found...");
                    return;
                }
                //check if output path is defined
                if (item.outputPath == null || item.outputPath == "") {
                    if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - watch action error! No outputh path defined...");
                    return;
                }
                //check if item is watched
                if (item.watchdog) {
                    //-------------------------
                    //--- clean output folder 
                    if (item.clearDays != 0) {
                        item.clearOutputDir();
                    }
                    //-------------------------
                    //--- create backup from interval settings
                    if (item.pcIntervalCheck()) {
                        //check interval (if reference time exist then its OK)
                        if (item.timeExists(backupMaster.pc, timeType.last)) {
                            //check time difference between current time and last time
                            DateTime pcLastBackup = item.pcLastBackupTime;
                            TimeSpan diff = now - pcLastBackup;
                            //check if difference is bigger than desired interval
                            if (diff.TotalMinutes >= item.pcIntervalInMins) {
                                item.createBackup(backupSource.interval);
                            }
                        } else {
                            //there is no backup done yet - no reference to count from... create it
                            item.createBackup(backupSource.interval);
                        }
                        //call event to update xml file (new last backup time)
                        updateBackupTime();
                    }
                    //-------------------------
                    //--- create backup from exact time
                    if (item.timeExists(backupMaster.pc, timeType.exact)) {
                        DateTime pcDailyBackup = item.pcDailyTime;
                        //check if its time for backup
                        if (now.Hour == pcDailyBackup.Hour && now.Minute == pcDailyBackup.Minute) {
                            item.createBackup(backupSource.daily, false);
                        }
                    }
                    //-------------------------
                    //--- get backup from robot
                    if (item.robotWatchBackup && item.timeExists(backupMaster.robot, timeType.last)) {
                        //check time difference between current time and robot time (give robot at leas 1 minute timeout)
                        DateTime robLastBackup = item.robotLastBackupTime;
                        TimeSpan diff = now - robLastBackup;
                        if (diff.Minutes >= 1) {
                            item.robotGetBackup();
                            item.robotWatchBackup = false;
                        }
                        //call event to update xml file (new last backup time)
                        updateBackupTime();
                    }
                    //-------------------------
                }  
            }
        }

        //=============================================================================================== ACTIVATE MASTER

        /// <summary>
        /// Update activate state of selected master in collection
        /// </summary>
        /// <param name="sender">CheckBox that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void checkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (currData != null && currData.controller != null) {
                //check if any check box is checked
                currData.activateMaster(checkPCactive.Checked, checkRobActive.Checked);
                //check if any robot is connected
                if (currData.controller == null) {
                    if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - connect to any controller to change data...");
                }
            } else {
                if (((CheckBox)sender).Checked) {
                    if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - connect to any controller to change data...");
                }
            }
            //update GUI
            resetGUI();
        }

        //=============================================================================================== OUTPUT PATH

        /// <summary>
        /// Show select output folder dialog after btnOutDir click
        /// </summary>
        /// <param name="sender">Button that triggered this event (btnOutDir)</param>
        /// <param name="e">Event arguments</param>
        private void btnOutDir_Click(object sender, EventArgs e)
        {
            //show select folder dialog
            dialogOutDir.Description = "Select output directory for backup folders.";
            dialogOutDir.RootFolder = Environment.SpecialFolder.Desktop;
            if (dialogOutDir.ShowDialog() == DialogResult.OK) {
                //remember current output path
                currData.outputPath = dialogOutDir.SelectedPath;
                //store it to local variable to show in label for user
                string path = currData.outputPath;
                if (dialogOutDir.SelectedPath.Length > 35) {
                    //directory is too long - get beginning and end of it
                    int slashF = path.IndexOf("\\"),
                        slashE = path.LastIndexOf("\\");
                    string firstDir = path.Substring(0, path.IndexOf("\\", slashF + 1));
                    path = firstDir + "\\..." + path.Substring(slashE, path.Length - slashE);
                }
                labelOutPathVal.Text = path + "\\";
            }
        }

        /// <summary>
        /// Show output folder in Windows Explorer after btnOutShow click
        /// </summary>
        /// <param name="sender">Button that triggered this event (btnOutShow)</param>
        /// <param name="e">Event arguments</param>
        private void btnOutShow_Click(object sender, EventArgs e)
        {
            if (currData.outputPath != null && currData.outputPath != "") {
                //open windows explorer window with current path
                System.Diagnostics.Process.Start("explorer.exe", currData.outputPath);
            } else {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - cant show output path! Its not defined...");
            }
        }

        /// <summary>
        /// Show tool tip with full output path (if exceeds label capacity) after mouse enter
        /// </summary>
        /// <param name="sender">Label that triggered event (labelOutPathVal)</param>
        /// <param name="e">Event arguments</param>
        private void labelOutPathVal_MouseEnter(object sender, EventArgs e)
        {
            //show tooltip with full path value (if its exceed label capacity)
            if (currData.outputPath != null && currData.outputPath != "" && labelOutPathVal.Text != currData.outputPath + "\\") {
                myToolTip.SetToolTip(labelRobBackupDir, currData.outputPath);
                myToolTip.Show(currData.outputPath + "\\", labelOutPathVal, 0, 18);
            }
        }

        /// <summary>
        /// Hide tool tip after mouse leave
        /// </summary>
        /// <param name="sender">Label that triggered event (labelOutPathVal)</param>
        /// <param name="e">Event arguments</param>
        private void labelOutPathVal_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.RemoveAll();
            myToolTip.Hide(labelOutPathVal);
        }

        //=============================================================================================== CLEAR DIRECTORY

        /// <summary>
        /// Update number of old days that should be cleared in collection
        /// </summary>
        /// <param name="sender">Num edit that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void numClearDays_ValueChanged(object sender, EventArgs e)
        {
            //update clear days internal data
            if (currData != null && currData.controller != null) currData.clearDays = (int)numClearDays.Value;
        }

        /// <summary>
        /// Execute clean output folder after btnCleanExe click
        /// </summary>
        /// <param name="sender">Button that triggered this event (btnCleanExe)</param>
        /// <param name="e">Event arguments</param>
        private void btnCleanExe_Click(object sender, EventArgs e)
        {
            //clear output directory on GUI demand
            currData.clearOutputDir(true);
        }

        //=============================================================================================== WATCH TIMER

        /// <summary>
        /// Execute turn timer on after btnWatchOn click
        /// </summary>
        /// <param name="sender">Button that triggered this event (btnWatchOn)</param>
        /// <param name="e">Event arguments</param>        
        private void btnWatchOn_Click(object sender, EventArgs e)
        {
            //turn on monitoring timer (if stopped)
            watchTimer(true, true);
            //refresh GUI
            resetGUI();
        }

        /// <summary>
        /// Execute turn timer off after btnWatchOff click
        /// </summary>
        /// <param name="sender">Button that triggered this event (btnWatchOff)</param>
        /// <param name="e">Event arguments</param>
        private void btnWatchOff_Click(object sender, EventArgs e)
        {
            //turn off monitoring timer
            watchTimer(false, true);
            //refresh GUI
            resetGUI();
        }

        /// <summary>
        /// Function used to turn on or off watch timer (to do or download backups)
        /// </summary>
        /// <param name="on">TRUE if turn timer on, FALSE otherwise</param>
        /// <param name="guiDemand">Is timer on/off is controlled via GUI</param>
        private void watchTimer(bool on, bool guiDemand = false)
        {
            //check action to do
            if (on) {
                //TURN ON timer
                currData.watchdog = true;
                if (!timerCheckBackup.Enabled) {
                    //timer start
                    timerCheckBackup.Start();
                    //if timer on from GUI then inform user that its running
                    if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - controller " + currData.abbName + " added to watch list! Watch timer turned ON!");
                } else {
                    //inform user that current controller was added to watch list
                    if (myCollection.itemWatchedNo() > 0) {
                        if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - controller "+currData.abbName+" added to watch list!");
                    } else {
                        //if timer on from GUI then inform user that its already running
                        if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - watch timer was already running...");
                    }
                }
            } else {
                //disable current item from backup watch
                currData.watchdog = false;
                //TURN OFF timer only when no items in collection arent watched
                if (myCollection.itemWatchedNo() == 0) {
                    //timer stop
                    timerCheckBackup.Stop();
                    //if timer on from GUI then inform user that its running
                    if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - controller " + currData.abbName + " removed from watch list! Watch timer turned OFF!");
                } else  {
                    if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - controller " + currData.abbName + " removed from watch list!");
                }
            }
        }

        //======================================================================================================

        /********************************************************
         ***  APP BACKUP MANAGER - PC MASTER
         ********************************************************/

        //=============================================================================================== GUI

        /// <summary>
        /// Do backup - GUI demand
        /// </summary>
        /// <param name="sender">Button that triggered this event (btnBackupExe)</param>
        /// <param name="e">Event arguments</param>
        private void btnBackupExe_Click(object sender, EventArgs e)
        {
            //create backup on GUI demand
            currData.createBackup(backupSource.gui);
        }

        //=============================================================================================== INTERVAL

        /// <summary>
        /// Update interval time in BackupManager container
        /// </summary>
        /// <param name="sender">Num edit that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            if (currData != null && currData.controller != null) {
                NumericUpDown numParent = (NumericUpDown)sender;
                if (numParent.Name.Contains("Mins")) currData.pcIntervalSet(intervalElement.mins, (int)numParent.Value);
                if (numParent.Name.Contains("Hour")) currData.pcIntervalSet(intervalElement.hours, (int)numParent.Value);
                if (numParent.Name.Contains("Days")) currData.pcIntervalSet(intervalElement.days, (int)numParent.Value);
                //check if there was reference backup (to measure time from)
                if (!currData.timeExists(backupMaster.pc, timeType.last)) {
                    //no reference time - inform user to create backup
                    if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - create backup to get reference to count from...");
                } else {
                    //reference backup time present - check if timer is running
                    if (!currData.watchdog) {
                        //timer is not running - inform user to run timer
                        if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - turn timer on to start monitoring...");
                    }
                }
            }
        }

        //=============================================================================================== DAILY TIME

        /// <summary>
        /// Validate inputted time in daily backup
        /// </summary>
        /// <param name="sender">Text edit that triggered event (textEveryTime)</param>
        /// <param name="e">Event arguments</param>
        private void textEveryTime_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (textEveryTime.MaskCompleted) {
                if (!e.IsValidInput) {
                    if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - wrong time inputted...");
                } else {
                    currData.pcDailyTime = DateTime.Parse(textEveryTime.Text);
                    //check if timer is running
                    if (!currData.watchdog) {
                        //timer is not running - inform user to run timer
                        if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - turn timer on to start monitoring...");
                    } else {
                        //timer is running - inform user that setting is correct
                        if (abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - backup will be done everyday at " + currData.pcDailyTime.ToShortTimeString() + "!");
                    }
                }
            }
        }

        /// <summary>
        /// Action during inputting daily backup time
        /// </summary>
        /// <param name="sender">Text edit that triggered event (textEveryTime)</param>
        /// <param name="e">Event arguments</param>
        private void textEveryTime_KeyDown(object sender, KeyEventArgs e)
        {
            Keys currKey = e.KeyCode;
            //if user presses any digit then everything is OK
            if ((currKey >= Keys.D0 && currKey <= Keys.D9) || (currKey >= Keys.NumPad0 && currKey <= Keys.NumPad9)) return;
            //if user presses arrows then its OK
            if (currKey == Keys.Left || currKey == Keys.Right) return;
            //we are here - user was pressing invalid key
            if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - only digits allowed...");
            e.Handled = true;
        }

        //=============================================================================================== DUPLICATES

        /// <summary>
        /// Change PC duplicate action radio button
        /// </summary>
        /// <param name="sender">Which button triggered event</param>
        /// <param name="e">Event arguments</param>
        private void radioPCDuplicate_CheckChanges(object sender, EventArgs e)
        {
            if (currData != null && currData.controller != null) {
                RadioButton curr = (RadioButton)sender;
                if (curr.Checked) {
                    currData.duplicateMethodPC = curr.TabIndex - 4;
                }
            }
        }

        //=============================================================================================== SUFFIXES

        /// <summary>
        /// Update suffix data in BackupManager container
        /// </summary>
        /// <param name="sender">Text edit that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void textBackupSuffix_TextChanged(object sender, EventArgs e)
        {
            TextBox textParent = (TextBox)sender;
            if (currData != null && currData.controller != null) {
                if (textParent.Name.Contains("Gui")) currData.pcGUISuffix = textGuiSuffix.Text;
                if (textParent.Name.Contains("Interval")) currData.pcIntervalSuffix = textIntervalSuffix.Text;
                if (textParent.Name.Contains("Daily")) currData.pcDailySuffix = textDailySuffix.Text;
                if (textParent.Name.Contains("Robot")) currData.robotDirSuffix = textRobotSuffix.Text;
            }
        }

        //======================================================================================================

        /********************************************************
         ***  APP BACKUP MANAGER - ROBOT MASTER
         ********************************************************/

        //=============================================================================================== SIGNALS

        /// <summary>
        /// Trigger robot signal window on ENTER press
        /// </summary>
        /// <param name="sender">Text edit that triggerted event (textSigDoBackup)</param>
        /// <param name="e">Event arguments</param>
        private void textSigDoBackup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) textSigDoBackup_Click(sender, null);
        }

        /// <summary>
        /// Trigger robot signal window on ENTER press
        /// </summary>
        /// <param name="sender">Text edit that triggerted event (textSigBackupProg)</param>
        /// <param name="e">Event arguments</param>
        private void textSigBackupProg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) textSigBackupProg_Click(sender, null);
        }

        /// <summary>
        /// Show window with robot signals to select execute backup signal
        /// </summary>
        /// <param name="sender">Text edit that triggerted event (textSigDoBackup)</param>
        /// <param name="e">Event arguments</param>
        private void textSigDoBackup_Click(object sender, EventArgs e)
        {
            //select current signal and show it in signals window
            signalsWindow.selectSig(doBackupIndex);
            signalsWindow.ShowDialog();
            //update signal names and current index
            doBackupIndex = signalsWindow.selectedIndex;
            string doBackupSigName = signalsWindow.selectedSignal;
            string diBackupSigName = textSigBackupProg.Text == "- select signal -" ? "" : textSigBackupProg.Text;
            //update GUI 
            textSigDoBackup.Text = doBackupSigName != "" ? doBackupSigName : "- select signal -";
            //update signals
            currData.updateRobotSignals(doBackupSigName, diBackupSigName);
        }

        /// <summary>
        /// Show window with robot signals to select backup in progress signal
        /// </summary>
        /// <param name="sender">Text edit that triggerted event (textSigBackupProg)</param>
        /// <param name="e">Event arguments</param>
        private void textSigBackupProg_Click(object sender, EventArgs e)
        {
            //select current signal and show it in signals window
            signalsWindow.selectSig(diBackupIndex);
            signalsWindow.ShowDialog();
            //update signal names and current index
            diBackupIndex = signalsWindow.selectedIndex;
            string doBackupSigName = textSigDoBackup.Text == "- select signal -" ? "" : textSigDoBackup.Text;
            string diBackupSigName = signalsWindow.selectedSignal;
            //update GUI
            textSigBackupProg.Text = diBackupSigName != "" ? diBackupSigName : "- select signal -";
            //update signals
            currData.updateRobotSignals(doBackupSigName, diBackupSigName);
        }

        //=============================================================================================== SRC DIR

        /// <summary>
        /// Show window with robot file structure to select source dir
        /// </summary>
        /// <param name="sender">Button that triggered event (btnRobBackupDir)</param>
        /// <param name="e">Event arguments</param>
        private void btnRobBackupDir_Click(object sender, EventArgs e)
        {
            filesWindow.ShowDialog();
            //check if directory was selected
            currData.robotDirSrc = filesWindow.selectedDir;
            labelRobBackupDir.ImageIndex = currData.robotDirSrc != "" ? 1 : 0;
        }

        /// <summary>
        /// Show tool tip with full robot src dir after mouse enter
        /// </summary>
        /// <param name="sender">Label that triggered event (labelRobBackupDir)</param>
        /// <param name="e">Event arguments</param>
        private void labelRobBackupDir_MouseEnter(object sender, EventArgs e)
        {
            //show tooltip with full robot backu value
            if (currData.robotDirSrc != null && currData.robotDirSrc != "") {
                myToolTip.SetToolTip(labelRobBackupDir, currData.robotDirSrc);
                myToolTip.Show(currData.robotDirSrc, labelRobBackupDir, 0, 18);
            } else {
                myToolTip.Show("no robot backup folder selected...", labelRobBackupDir, 0, 18);
            }
        }

        /// <summary>
        /// Hide tool tip with full robot src dir after mouse leave
        /// </summary>
        /// <param name="sender">Label that triggered event (labelRobBackupDir)</param>
        /// <param name="e">Event arguments</param>
        private void labelRobBackupDir_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.RemoveAll();
            myToolTip.Hide(labelOutPathVal);
        }

        //=============================================================================================== STATUS

        /// <summary>
        /// Show tool tip with status description after mouse enter
        /// </summary>
        /// <param name="sender">Label that triggered event (labelBackupStatus)</param>
        /// <param name="e">Event arguments</param>
        private void labelBackupStatus_MouseEnter(object sender, EventArgs e)
        {
            string info = "";
            switch (labelBackupStatus.ImageIndex) {
                case 0:
                    info = "robot backup in progress...";
                    break;
                case 1:
                    info = "backup done - download in queue...";
                    break;
                case 2:
                    info = "backup download in progress...";
                    break;
                case 3:
                    info = "last backup saved OK!";
                    break;
                case 4:
                    info = "last backup saved NOK...";
                    break;
                default:
                    info = "unknown state...";
                    return;
            }
            //show tooltip with full robot backu value
            myToolTip.SetToolTip(labelBackupStatus, currData.robotDirSrc);
            myToolTip.Show(info, labelBackupStatus, 0, 18);
        }

        /// <summary>
        /// Hide tool tip with status description after mouse leave
        /// </summary>
        /// <param name="sender">Label that triggered event (labelBackupStatus)</param>
        /// <param name="e">Event arguments</param>
        private void labelBackupStatus_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.RemoveAll();
            myToolTip.Hide(labelBackupStatus);
        }

        //=============================================================================================== DUPLICATES

        /// <summary>
        /// Change robot duplicate action radio button
        /// </summary>
        /// <param name="sender">Which button triggered event</param>
        /// <param name="e">Event arguments</param>
        private void radioROBDuplicate_CheckChanges(object sender, EventArgs e)
        {
            if (currData != null && currData.controller != null) {
                RadioButton curr = (RadioButton)sender;
                if (curr.Checked) currData.duplicateMethodRobot = curr.TabIndex - 4;
            }
        }
        //=========================================================================================================
    }
}
