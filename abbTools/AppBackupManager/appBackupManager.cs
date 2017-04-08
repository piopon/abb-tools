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
        Controller abbController = null;
        loggerABB abbLogger = null;
        BackupManager myData = null;
        //internal data
        int doBackupIndex;
        int diBackupIndex;
        //additional windows & settings
        public int parentHeight;
        public int parentWidth;
        windowRobotSig signalsWindow;
        windowRobotFiles filesWindow;

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
            abbController = null;
            abbLogger = null;
            myData = new BackupManager();
            myData.PCBackupStateChanged += updatePCBackupState;
            myData.RobotBackupStateChanged += updateRobotBackupState;
            //init internal data
            doBackupIndex = -1;
            diBackupIndex = -1;
            //init internal signal window
            signalsWindow = new windowRobotSig(abbController);
            signalsWindow.StartPosition = FormStartPosition.CenterParent;
            signalsWindow.ShowInTaskbar = false;
            //init internal files window
            filesWindow = new windowRobotFiles(abbController,true,true);
            filesWindow.StartPosition = FormStartPosition.CenterParent;
            filesWindow.ShowInTaskbar = false;
            //clear robot data (no robots yet)
            clearData();
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
            myData.logger = abbLogger;
        }

        /// <summary>
        /// Desynchronize logger from current app (no logging available)
        /// </summary>
        public void desyncLogger()
        {
            //clear controller logger
            abbLogger = null;
            myData.logger = null;
        }

        /// <summary>
        /// Synchronize ABB controller (called on controller connect!)
        /// </summary>
        /// <param name="myController">ABB controller to synchronize with</param>
        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
            myData.controller = abbController;
            //update signals in background
            if (signalsWindow != null) {
                signalsWindow.Height = parentHeight;
                signalsWindow.Width = parentWidth;
                signalsWindow.updateSignals(abbController);
            }
            //update file structure in background
            if (filesWindow != null) {
                filesWindow.Height = parentHeight;
                filesWindow.Width = parentWidth;
                filesWindow.updateFiles(abbController);
            }
            //get data from collection
            getData(abbController);
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Desynchronize current ABB controller from app (no controller actions available)
        /// </summary>
        public void desyncController()
        {
            //reset controller address
            abbController = null;
            myData.controller = abbController;
            //reset GUI
            resetGUI();
        }

        void clearData()
        {
            myData.clearData();
        }

        void getData(Controller cController)
        {

        }

        /********************************************************
         ***  APP BACKUP MANAGER - EVENT METHODS
         ********************************************************/

        private void updateRobotBackupState(int newState)
        {
            labelBackupStatus.ImageIndex = newState;
            if (newState == 2) labelLastTimeROB.Text = myData.robotLastBackupTime.ToString();
        }

        private void updatePCBackupState(int newState)
        {
            if (newState == 1) labelLastTimePC.Text = myData.pcLastBackupTime.ToString();
        }

        /********************************************************
         ***  APP BACKUP MANAGER - GUI
         ********************************************************/

        /// <summary>
        /// Reset GUI components to initial state
        /// </summary>
        public void resetGUI()
        {
            bool enablePC = false, enableROB = false;
            //check if robot is connected
            if(abbController != null) {
                enablePC = checkPCactive.Checked;
                enableROB = checkRobActive.Checked;
                //fill robot data
            } else {
                //hide robot data
            }
            //do enable/disable PC group
            if (enablePC) {
                groupPCmaster.Enabled = true;
                btnBackupExe.BackColor = System.Drawing.Color.Chartreuse;
            } else {
                groupPCmaster.Enabled = false;
                btnBackupExe.BackColor = System.Drawing.Color.Silver;
            }
            //do enable/disable ROBOT group
            if (enableROB) {
                groupRobMaster.Enabled = true;
            } else {
                groupRobMaster.Enabled = false;
            }
            //check if directory was selected
            if (myData.robotDirSrc != null) {
                labelRobBackupDir.ImageIndex = myData.robotDirSrc != "" ? 1 : 0;
            } else {
                labelRobBackupDir.ImageIndex = 0;
            }
            //fill independent data - TextBox suffixes
            textGuiSuffix.Text = myData.pcGUISuffix;
            textIntervalSuffix.Text = myData.pcIntervalSuffix;
            textDailySuffix.Text = myData.pcDailySuffix;
            textRobotSuffix.Text = myData.robotDirSuffix;
            //fill independent data - RadioButton duplicates PC
            radioPCOverwrite.Checked = myData.duplicateMethodPC == (int)sameNameAction.overwrite;
            radioPCIncr.Checked = myData.duplicateMethodPC == (int)sameNameAction.increment;
            radioPCTime.Checked = myData.duplicateMethodPC == (int)sameNameAction.additTime;
            //fill independent data - RadioButton duplicates PC
            radioROBOverwrite.Checked = myData.duplicateMethodRobot == (int)sameNameAction.overwrite;
            radioROBIncr.Checked = myData.duplicateMethodRobot == (int)sameNameAction.increment;
            radioROBTime.Checked = myData.duplicateMethodRobot == (int)sameNameAction.additTime;
            //do enable/disable COMMON group
            if (abbController != null) {
                groupCommonSettings.Enabled = true;
                btnOutSelect.BackColor = System.Drawing.Color.DarkOrange;
                btnOutShow.BackColor = System.Drawing.Color.DarkOrange;
                btnCleanExe.BackColor = System.Drawing.Color.OrangeRed;
                if (myData.watchdog) {
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
                btnOutSelect.BackColor = System.Drawing.Color.Silver;
                btnOutShow.BackColor = System.Drawing.Color.Silver;
                btnCleanExe.BackColor = System.Drawing.Color.Silver;
                btnWatchOn.BackColor = System.Drawing.Color.Silver;
                btnWatchOff.BackColor = System.Drawing.Color.Silver;
            }
        }

        /********************************************************
         ***  APP BACKUP MANAGER - main window data management 
         ********************************************************/

        public void clearAppData()
        {
            clearData();
        }

        public void saveAppData(ref System.Xml.XmlWriter saveXml, Controller parent = null, string parentName = "")
        {
            //save current robot child node to XML document
            string saveName = parent != null ? parent.SystemName : parentName;
            if (saveName.Length > 0) {
                //save backup manager data
                MessageBox.Show("appBackupManager - loadAppData TODO");
                return;
            } else {
                abbLogger.writeLog(logType.error, "cant save controller(s) without specified name...");
            }
        }

        public void loadAppData(ref System.Xml.XmlReader loadXml, Controller parent = null, string parentName = "")
        {
            //reset GUI
            resetGUI();
            MessageBox.Show("appBackupManager - loadAppData TODO");
            return;
            ////read XML untill current app settings node appears
            //while (loadXml.Read())
            //{
            //    bool start = loadXml.NodeType == System.Xml.XmlNodeType.Element,
            //         backupManager = loadXml.Name.StartsWith("backupManager");
            //    //if we are starting to read windowsIPC app setting then break from WHILE loop
            //    if (start && backupManager) {
            //        //if current element is empty then return
            //        if (loadXml.IsEmptyElement) return;
            //        //if element not empty then load its data
            //        break;
            //    }
            //    //if we are at end of current robot then dont read anythig
            //    if (loadXml.Name.StartsWith("robot_") && loadXml.NodeType == System.Xml.XmlNodeType.EndElement) return;
            //}
            ////read next element (this one will be with robot info
            //System.Xml.XmlReader nodeCurrRobot = loadXml.ReadSubtree();
            ////read every child node from XML document (stopped at reading robot)

            ////reset GUI
            //resetGUI();
        }

        public void savedControllerFound(Controller found)
        {
            //update controllers data in collection
            //it was saved but non-visible at start, but it showed up right now!
            if (found != null)
            {

            }
        }

        public void savedControllerLost(Controller lost)
        {
            if (lost != null) {
                if (abbController != null && lost.SystemName == abbController.SystemName) {
                    //clear logger and client
                    abbController = null;
                    //reset GUI
                    resetGUI();
                }
            }
        }

        private void btnOutDir_Click(object sender, EventArgs e)
        {
            //show select folder dialog
            dialogOutDir.Description = "Select output directory for backup folders.";
            dialogOutDir.RootFolder = Environment.SpecialFolder.Desktop;
            if (dialogOutDir.ShowDialog() == DialogResult.OK) {
                //remember current output path
                myData.outputPath = dialogOutDir.SelectedPath;
                //store it to local variable to show in label for user
                string path = myData.outputPath;
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

        private void checkActive_CheckedChanged(object sender, EventArgs e)
        {
            //check if any check box is checked
            myData.activateMaster(checkPCactive.Checked, checkRobActive.Checked);
            //check if any robot is connected
            if (abbController == null) {
                if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - connect to any controller to change data...");
            }
            //update GUI
            resetGUI();
        }

        private void timerCheckBackup_Tick(object sender, EventArgs e)
        {
            //get current time
            DateTime now = DateTime.Now;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //TODO: scan all data collection
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //check if controller is existing in network
            if (abbController == null) {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - watch action error! No controller found...");
                return;
            }
            //check if output path is defined
            if (myData.outputPath == null || myData.outputPath == "") {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - watch action error! No outputh path defined...");
                return;
            }
            //==============================================================================  
            //=== clean output folder 
            if (myData.clearDays != 0) {
                myData.clearOutputDir();
            }
            //==============================================================================  
            //=== create backup from interval settings
            if (myData.pcIntervalCheck()) {
                //check interval (if reference time exist then its OK)
                if (myData.timeExists(backupMaster.pc,timeType.last)) {
                    //check time difference between current time and last time
                    DateTime pcLastBackup = myData.pcLastBackupTime;
                    TimeSpan diff = now - pcLastBackup;
                    //check if difference is bigger than desired interval
                    if (diff.TotalMinutes >= myData.pcIntervalInMins) {
                        myData.createBackup(backupSource.interval);
                    }
                } else {
                    //there is no backup done yet - no reference to count from... create it
                    myData.createBackup(backupSource.interval);
                }
                //update xml file
            }
            //==============================================================================  
            //=== create backup from exact time
            if (myData.timeExists(backupMaster.pc, timeType.exact)) {
                DateTime pcDailyBackup = myData.pcDailyTime;
                //check if its time for backup
                if (now.Hour == pcDailyBackup.Hour && now.Minute == pcDailyBackup.Minute) {
                    myData.createBackup(backupSource.daily, false);
                }
            }
            //==============================================================================  
            //=== get backup from robot
            if (myData.robotWatchBackup && myData.timeExists(backupMaster.robot, timeType.last)) {
                //check time difference between current time and robot time (give robot at leas 1 minute timeout)
                DateTime robLastBackup = myData.robotLastBackupTime;
                TimeSpan diff = now - robLastBackup;
                if (diff.Minutes >= 1) {
                    myData.robotGetBackup();
                    myData.robotWatchBackup = false;
                }
                //update xml file
            }
            //==============================================================================  
        }

        private void labelOutPathVal_MouseEnter(object sender, EventArgs e)
        {
            //show tooltip with full path value (if its exceed label capacity)
            if (myData.outputPath != null && myData.outputPath != "" && labelOutPathVal.Text != myData.outputPath + "\\") {
                myToolTip.SetToolTip(labelRobBackupDir, myData.outputPath);
                myToolTip.Show(myData.outputPath + "\\", labelOutPathVal, 0, 18);
            }
        }

        private void labelOutPathVal_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.RemoveAll();
            myToolTip.Hide(labelOutPathVal);
        }

        private void numClearDays_ValueChanged(object sender, EventArgs e)
        {
            //update clear days internal data
            myData.clearDays = (int)numClearDays.Value;
        }

        private void btnBackupExe_Click(object sender, EventArgs e)
        {
            //create backup on GUI demand
            myData.createBackup(backupSource.gui);
        }

        private void btnCleanExe_Click(object sender, EventArgs e)
        {
            //clear output directory on GUI demand
            myData.clearOutputDir(true);
        }

        private void btnWatchOn_Click(object sender, EventArgs e)
        {
            //turn on monitoring timer (if stopped)
            watchTimer(true, true);
            //refresh GUI
            resetGUI();
        }

        private void btnWatchOff_Click(object sender, EventArgs e)
        {
            //turn off monitoring timer
            watchTimer(false, true);
            //refresh GUI
            resetGUI();
        }

        private void watchTimer(bool on, bool guiDemand = false)
        {
            //check action to do
            if (on) {
                //TURN ON timer
                myData.watchdog = true;
                if (!timerCheckBackup.Enabled) {
                    timerCheckBackup.Start();
                    //if timer on from GUI then inform user that its running
                    if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - watch timer turned ON!");
                } else {
                    //if timer on from GUI then inform user that its already running
                    if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - watch timer was already running...");
                }
            } else {
                //TURN OFF timer
                myData.watchdog = true;
                timerCheckBackup.Stop();
                //if timer on from GUI then inform user that its running
                if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - watch timer turned OFF!");
            }
        }

        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            myData.pcIntervalSet((int)numIntervalMins.Value, (int)numIntervalHours.Value, (int)numIntervalDays.Value);
            //check if there was reference backup (to measure time from)
            if (!myData.timeExists(backupMaster.pc, timeType.last)) {
                //no reference time - inform user to create backup
                if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - create backup to get reference to count from...");
            } else {
                //reference backup time present - check if timer is running
                if (!myData.watchdog) {
                    //timer is not running - inform user to run timer
                    if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - turn timer on to start monitoring...");
                }
            }
        }

        private void textBackupSuffix_TextChanged(object sender, EventArgs e)
        {
            TextBox textParent = (TextBox)sender;
            if (textParent.Name.Contains("Gui")) myData.pcGUISuffix = textGuiSuffix.Text;
            if (textParent.Name.Contains("Interval")) myData.pcIntervalSuffix = textIntervalSuffix.Text;
            if (textParent.Name.Contains("Daily")) myData.pcDailySuffix = textDailySuffix.Text;
            if (textParent.Name.Contains("Robot")) myData.robotDirSuffix = textRobotSuffix.Text;
        }

        private void textEveryTime_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (textEveryTime.MaskCompleted) {
                if (!e.IsValidInput) {
                    if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - wrong time inputted...");
                } else {
                    myData.pcDailyTime = DateTime.Parse(textEveryTime.Text);
                    //check if timer is running
                    if (!myData.watchdog) {
                        //timer is not running - inform user to run timer
                        if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - turn timer on to start monitoring...");
                    } else {
                        //timer is running - inform user that setting is correct
                        if (abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - backup will be done everyday at "+ myData.pcDailyTime.ToShortTimeString()+"!");
                    }
                }
            }
        }

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

        private void radioPCDuplicate_CheckChanges(object sender, EventArgs e)
        {
            RadioButton curr = (RadioButton)sender;
            if (curr.Checked) {
                myData.duplicateMethodPC = curr.TabIndex - 4;
            }
        }

        private void radioROBDuplicate_CheckChanges(object sender, EventArgs e)
        {
            RadioButton curr = (RadioButton)sender;
            if (curr.Checked) {
                myData.duplicateMethodRobot = curr.TabIndex - 4;
            }
        }

        private void btnOutShow_Click(object sender, EventArgs e)
        {
            if (myData.outputPath != null && myData.outputPath != "") {
                //open windows explorer window with current path
                System.Diagnostics.Process.Start("explorer.exe", myData.outputPath);
            } else {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - cant show output path! Its not defined...");
            }
        }

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
            myData.updateRobotSignals(doBackupSigName, diBackupSigName);
        }

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
            myData.updateRobotSignals(doBackupSigName, diBackupSigName);
        }

        private void btnRobBackupDir_Click(object sender, EventArgs e)
        {
            filesWindow.ShowDialog();
            //check if directory was selected
            myData.robotDirSrc = filesWindow.selectedDir;
            labelRobBackupDir.ImageIndex = myData.robotDirSrc != "" ? 1 : 0;
        }

        private void labelRobBackupDir_MouseEnter(object sender, EventArgs e)
        {
            //show tooltip with full robot backu value
            if (myData.robotDirSrc != null && myData.robotDirSrc != "") {
                myToolTip.SetToolTip(labelRobBackupDir, myData.robotDirSrc);
                myToolTip.Show(myData.robotDirSrc, labelRobBackupDir, 0, 18);
            } else {
                myToolTip.Show("no robot backup folder selected...", labelRobBackupDir, 0, 18);
            }
        }
 
        private void labelRobBackupDir_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.RemoveAll();
            myToolTip.Hide(labelOutPathVal);
        }

        private void labelBackupStatus_MouseEnter(object sender, EventArgs e)
        {
            string info = "";
            switch (labelBackupStatus.ImageIndex)
            {
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
            myToolTip.SetToolTip(labelBackupStatus, myData.robotDirSrc);
            myToolTip.Show(info, labelBackupStatus, 0, 18);
        }

        private void labelBackupStatus_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.RemoveAll();
            myToolTip.Hide(labelBackupStatus);
        }

        private void textSigDoBackup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) textSigDoBackup_Click(sender, null);
        }

        private void textSigBackupProg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) textSigBackupProg_Click(sender, null);
        }
    }
}
