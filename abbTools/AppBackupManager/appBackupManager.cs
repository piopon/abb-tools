using System;
using System.IO;
using System.Windows.Forms;
using abbTools.Windows;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppBackupManager
{
    public partial class appBackupManager : UserControl
    {
        /********************************************************
         ***  APP BACKUP MANAGER - data
         ********************************************************/

        //current connection data containers
        Controller abbController = null;
        loggerABB abbLogger = null;
        //current robot data - COMMON
        bool usePC;
        bool useROB;
        bool monitor;
        int clearDays;
        string backupFolder;
        string outputPath;
        //current robot data - PC MASTER
        int backupMin;
        int backupHour;
        int backupDay;
        int duplicateMethodPC;
        string backupSuffixGUI;
        string backupSuffixInterval;
        string backupSuffixTime;      
        DateTime exactPCbackupTime;
        DateTime lastPCbackupTime;
        //current robot data - ROBOT MASTER
        int duplicateMethodRob;
        int doBackupIndex;
        int diBackupIndex;
        string doBackupSigName;
        string diBackupSigName;
        string backupSuffixRobot;
        DateTime lastROBbackupTime;
        Signal sigBackupExe;
        Signal sigBackupInP;
        //internal data
        public int parentHeight;
        public int parentWidth;
        DateTime emptyTime;
        windowRobotSig signalsWindow;
        windowRobotFiles filesWindow;
        enum sameNameMethod
        {
            overwrite = 0,
            increment = 1,
            additTime = 2
        }

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
            //init internal data
            emptyTime = new DateTime(1, 1, 1);
            doBackupSigName = "";
            diBackupSigName = "";
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
        }

        /// <summary>
        /// Desynchronize logger from current app (no logging available)
        /// </summary>
        public void desyncLogger()
        {
            //clear controller logger
            abbLogger = null;
        }

        /// <summary>
        /// Synchronize ABB controller (called on controller connect!)
        /// </summary>
        /// <param name="myController">ABB controller to synchronize with</param>
        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
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
            //reset GUI
            resetGUI();
        }

        void clearData()
        {
            //bool
            usePC = false;
            useROB = false;
            monitor = false;
            //int
            clearDays = 0;
            backupMin = 0;
            backupHour = 0;
            backupDay = 0;
            duplicateMethodPC = (int)sameNameMethod.overwrite;
            duplicateMethodRob = (int)sameNameMethod.overwrite;
            //string
            backupFolder = "";
            backupSuffixGUI = "";
            backupSuffixInterval = "";
            backupSuffixTime = "";
            backupSuffixRobot = "";
            outputPath = "";
            //time
            exactPCbackupTime = emptyTime;
            lastPCbackupTime = emptyTime;
        }

        void getData(Controller cController)
        {

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
            //fill independent data - TextBox suffixes
            textGuiSuffix.Text = backupSuffixGUI;
            textIntervalSuffix.Text = backupSuffixInterval;
            textDailySuffix.Text = backupSuffixTime;
            textRobotSuffix.Text = backupSuffixRobot;
            //fill independent data - RadioButton duplicates PC
            radioPCOverwrite.Checked = duplicateMethodPC == (int)sameNameMethod.overwrite;
            radioPCIncr.Checked = duplicateMethodPC == (int)sameNameMethod.increment;
            radioPCTime.Checked = duplicateMethodPC == (int)sameNameMethod.additTime;
            //fill independent data - RadioButton duplicates PC
            radioROBOverwrite.Checked = duplicateMethodRob == (int)sameNameMethod.overwrite;
            radioROBIncr.Checked = duplicateMethodRob == (int)sameNameMethod.increment;
            radioROBTime.Checked = duplicateMethodRob == (int)sameNameMethod.additTime;
            //do enable/disable COMMON group
            if (abbController != null) {
                groupCommonSettings.Enabled = true;
                btnOutSelect.BackColor = System.Drawing.Color.DarkOrange;
                btnOutShow.BackColor = System.Drawing.Color.DarkOrange;
                btnCleanExe.BackColor = System.Drawing.Color.OrangeRed;
                if (monitor) {
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
                outputPath = dialogOutDir.SelectedPath;
                //store it to local variable to show in label for user
                string path = outputPath;
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
            usePC = checkPCactive.Checked;
            useROB = checkRobActive.Checked;
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
            if (outputPath == null || outputPath == "") {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - watch action error! No outputh path defined...");
                return;
            }
            //==============================================================================  
            //=== clean output folder 
            if (clearDays != 0) {
                clearOutputDir(abbController, clearDays);
            }
            //==============================================================================  
            //=== create backup from interval settings
            if (backupMin != 0 || backupHour != 0 || backupDay != 0) {
                //check interval (if reference time exist then its OK)
                if (lastPCbackupTime != emptyTime) {
                    //check time difference between current time and ref + interval
                    TimeSpan diff = now - lastPCbackupTime;
                    if (diff.Minutes >= backupMin && diff.Hours >= backupHour && diff.Days >= backupDay) { 
                        createBackup(abbController, outputPath, backupSuffixInterval);
                    }
                } else {
                    //there is no backup done yet - no reference to count from... create it
                    createBackup(abbController, outputPath, backupSuffixInterval);
                }
                //update xml file
            }
            //==============================================================================  
            //=== create backup from exact time
            if (exactPCbackupTime != emptyTime) {
                //check if its time for backup
                if (now.Hour == exactPCbackupTime.Hour && now.Minute == exactPCbackupTime.Minute) {
                    createBackup(abbController, outputPath, backupSuffixTime, false);
                }
            }
            //==============================================================================  
        }

        private void clearOutputDir(Controller cController, int daysOld, bool guiDemand=false)
        {
            //check if controller is existing in network
            if (cController == null) {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - can't clear folders! No controller connected...");
                return;
            }
            //check if output path is defined
            if (outputPath == null || outputPath == "") {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - can't clear folders! No outputh path defined...");
                return;
            }
            //get all directories in output dir
            string[] folders = Directory.GetDirectories(outputPath + "\\");
            //compare every folder creation date with current date
            int foldersDeleted = 0;
            for (int i = 0; i < folders.Length; i++) {
                //delete only backup files (contains _BACKUP_ string)
                if (folders[i].Contains("_BACKUP_")) {
                    DateTime createdTime = Directory.GetCreationTime(folders[i]);
                    TimeSpan timeDiff = DateTime.Now - createdTime;
                    if (numClearDays.Value > 0 && timeDiff.TotalDays > (double)numClearDays.Value) {
                        Directory.Delete(folders[i], true);
                        foldersDeleted++;
                    }
                }
            }
            //show log if some folders were deleted
            if (abbLogger != null) {
                if (foldersDeleted > 0) {
                    abbLogger.writeLog(logType.warning, "controller <b>"+cController.SystemName+"</b> cleaned " + foldersDeleted.ToString() + " folders!");
                } else {
                    if(guiDemand) {
                        abbLogger.writeLog(logType.warning, "controller <b>" + cController.SystemName + "</b> no folders to clean!");
                    }
                }
            }
        }

        private void labelOutPathVal_MouseEnter(object sender, EventArgs e)
        {
            //show tooltip with full path value (if its exceed label capacity)
            if (outputPath != null && outputPath != "" && labelOutPathVal.Text != outputPath + "\\") {
                myToolTip.Show(outputPath + "\\", labelOutPathVal, 0, 18);
            }
        }

        private void labelOutPathVal_MouseLeave(object sender, EventArgs e)
        {
            //hide tooltip with full path value
            myToolTip.Hide(labelOutPathVal);
        }

        private void createBackup(Controller cController, string outPath, string fileSuffix, bool updateTime=true)
        {
            //check if controller is existing in network
            if (cController == null) {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - can't create backup! No controller connected...");
                return;
            }
            //check if output path is defined
            if (outPath == null || outPath == "") {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - can't create backup! No outputh path defined...");
                return;
            }
            //create backup folder name and path
            backupFolder = abbController.SystemName + "_BACKUP_" + DateTime.Now.ToShortDateString() + fileSuffix;
            string backupPath = outPath + "\\" + backupFolder;
            //check if current path is non-existent
            if (Directory.Exists(backupPath)) {
                //path exists - check what we want to do
                if (duplicateMethodPC == (int)sameNameMethod.overwrite) {
                    //we will overwrite backup - delete older one
                    Directory.Delete(backupPath,true);
                } else if (duplicateMethodPC == (int)sameNameMethod.increment) {
                    //we will add second folder with increment number - check how many folder there is
                    string[] folders = Directory.GetDirectories(outPath + "\\");
                    int currFolder = 0;
                    //find the newest file (it should be the one with last increment number
                    for (int i = 0; i < folders.Length; i++) {
                        if (folders[i].Contains(DateTime.Now.ToShortDateString())) {
                            //we found the same folder 
                            int incrPos = folders[i].LastIndexOf("_");
                            string folderNo = folders[i].Substring(incrPos+1);
                            //check if we have other incremented folder
                            if (folderNo.Length <= 3) {
                                //another incremented folder - check if its bigger then current
                                int currNumber = int.Parse(folderNo);
                                currFolder = currNumber>currFolder ? currNumber : currFolder;
                            }
                        }
                    }
                    //update suffix with next number
                    backupFolder += "_"+ (++currFolder).ToString();
                    //update backup path
                    backupPath = outPath + "\\" + backupFolder;
                } else if (duplicateMethodPC == (int)sameNameMethod.additTime) {
                    //we will add second folder with current time
                    backupFolder += "_" + DateTime.Now.ToLongTimeString();
                    //replace time colon with dash
                    backupFolder = backupFolder.Replace(":", ";");
                    //update backup path
                    backupPath = outPath + "\\" + backupFolder;
                }
            }
            //check if backup isnt currently in progress
            if (!cController.BackupInProgress) {
                //hide info about updating time after backup ok (to check it in event on backup done)
                cController.UICulture.NumberFormat.PositiveSign += "_" + updateTime.ToString();
                //subscribe to backup completed event
                cController.BackupCompleted -= abbControllerBackupDoneEvent;
                cController.BackupCompleted += abbControllerBackupDoneEvent;
                //everything is ok - do backup
                cController.Backup(backupPath);
                //show log info
                if (abbLogger != null) abbLogger.writeLog(logType.info, "controller <b>" + cController.SystemName + "</b>: "+
                                                            "doing robot backup [" + DateTime.Now.ToShortTimeString() + "]!");
            } else {
                //backup in progress - inform user
                if (abbLogger != null) abbLogger.writeLog(logType.info, "controller <b>" + cController.SystemName + "</b>: "+
                                                            "backup in progress! Wait for end and retry [" + DateTime.Now.ToShortTimeString() + "]...");
            }
        }

        private void abbControllerBackupDoneEvent(object sender, BackupEventArgs e)
        {
            //get controller data
            Controller temp = (Controller)sender;
            //check if there is recent backup time (if not then update it)
            if (e.Succeeded) {
                //get info about update time (hidden in controller data)
                int checkPos = temp.UICulture.NumberFormat.PositiveSign.IndexOf("_");
                string update = temp.UICulture.NumberFormat.PositiveSign.Substring(checkPos+1);
                //check if we want to update time 
                if (Boolean.Parse(update)) {
                    lastPCbackupTime = DateTime.Now;
                    labelLastTimePC.Text = lastPCbackupTime.ToString();
                }
                //restore controller application variable
                temp.UICulture.NumberFormat.PositiveSign = temp.UICulture.NumberFormat.PositiveSign.Substring(0, checkPos);
                //show log info
                if (abbLogger != null) abbLogger.writeLog(logType.info, "controller <b>"+temp.SystemName+"</b>: "+
                                                            "auto backup done ["+ DateTime.Now.ToShortTimeString()+ "]!");
            } else {
                //show log info
                if (abbLogger != null) abbLogger.writeLog(logType.error, "controller <b>" + temp.SystemName + "</b>: "+
                                                            "backup error [" + DateTime.Now.ToShortTimeString() + "]...");
            }
        }

        private void numClearDays_ValueChanged(object sender, EventArgs e)
        {
            //update clear days internal data
            clearDays = (int)numClearDays.Value;
        }

        private void btnBackupExe_Click(object sender, EventArgs e)
        {
            //create backup on GUI demand
            createBackup(abbController, outputPath, backupSuffixGUI);
        }

        private void btnCleanExe_Click(object sender, EventArgs e)
        {
            //clear output directory on GUI demand
            clearOutputDir(abbController,clearDays,true);
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
                monitor = true;
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
                monitor = false;
                timerCheckBackup.Stop();
                //if timer on from GUI then inform user that its running
                if (guiDemand && abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - watch timer turned OFF!");
            }
        }

        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            backupMin = (int)numIntervalMins.Value;
            backupHour = (int)numIntervalHours.Value;
            backupDay = (int)numIntervalDays.Value;
            //check if there was reference backup (to measure time from)
            if (lastPCbackupTime == null || lastPCbackupTime == emptyTime) {
                //no reference time - inform user to create backup
                if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - create backup to get reference to count from...");
            } else {
                //reference backup time present - check if timer is running
                if (!monitor) {
                    //timer is not running - inform user to run timer
                    if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - turn timer on to start monitoring...");
                }
            }
        }

        private void textBackupSuffix_TextChanged(object sender, EventArgs e)
        {
            TextBox textParent = (TextBox)sender;
            if (textParent.Name.Contains("Gui")) backupSuffixGUI = textGuiSuffix.Text;
            if (textParent.Name.Contains("Interval")) backupSuffixInterval = textIntervalSuffix.Text;
            if (textParent.Name.Contains("Daily")) backupSuffixTime = textDailySuffix.Text;
            if (textParent.Name.Contains("Robot")) backupSuffixRobot = textRobotSuffix.Text;
        }

        private void textEveryTime_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (textEveryTime.MaskCompleted) {
                if (!e.IsValidInput) {
                    if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - wrong time inputted...");
                } else {
                    exactPCbackupTime = DateTime.Now;
                    exactPCbackupTime = DateTime.Parse(textEveryTime.Text);
                    //check if timer is running
                    if (!monitor) {
                        //timer is not running - inform user to run timer
                        if (abbLogger != null) abbLogger.writeLog(logType.warning, "abbTools - turn timer on to start monitoring...");
                    } else {
                        //timer is running - inform user that setting is correct
                        if (abbLogger != null) abbLogger.writeLog(logType.info, "abbTools - backup will be done everyday at "+ exactPCbackupTime.ToShortTimeString()+"!");
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
                duplicateMethodPC = curr.TabIndex - 4;
            }
        }

        private void radioROBDuplicate_CheckChanges(object sender, EventArgs e)
        {
            RadioButton curr = (RadioButton)sender;
            if (curr.Checked) {
                duplicateMethodRob = curr.TabIndex - 4;
            }
        }

        private void btnOutShow_Click(object sender, EventArgs e)
        {
            if (outputPath != null && outputPath != "") {
                //open windows explorer window with current path
                System.Diagnostics.Process.Start("explorer.exe", outputPath);
            } else {
                if (abbLogger != null) abbLogger.writeLog(logType.error, "abbTools - cant show output path! Its not defined...");
            }
        }

        private void DoBackup_Changed(object sender, SignalChangedEventArgs e)
        {
            if (e.NewSignalState.Value == 1) {
                string abbName = getControllerName((DigitalSignal)sender);
                if (sigBackupInP != null) {
                    sigBackupInP.Changed += DiBackup_Changed;
                    abbLogger.writeLog(logType.info, "controller <b>"+ abbName + "</b>"+
                                                     " [BACKUP MASTER]:  backup in progress - waiting for end...");
                } else {
                    abbLogger.writeLog(logType.error, "controller <b>" + abbName + "</b>"+
                                                     " [BACKUP MASTER]:  backup in progress - no signal defined!");
                }
            }
        }

        private void DiBackup_Changed(object sender, SignalChangedEventArgs e)
        {
            if (sigBackupExe.Value == 0 && e.NewSignalState.Value == 0) {
                string abbName = getControllerName((DigitalSignal)sender);
                sigBackupInP.Changed -= DiBackup_Changed;
                //check if output path is created
                if (outputPath != null && outputPath != "") {
                    abbLogger.writeLog(logType.info, "controller <b>" + abbName + "</b>" +
                                                     " [BACKUP MASTER]:  backup done - download start...");
                    //backup created - get it from robot
                    string abbDir = "docs";
                    if (abbController.FileSystem.DirectoryExists(abbDir)) {
                        backupFolder = abbController.SystemName + "_BACKUP_" + DateTime.Now.ToShortDateString() + backupSuffixRobot;
                        string backupPath = outputPath + "\\" + backupFolder;
                        //copy backup file
                        IAsyncResult dirCopyRes = abbController.FileSystem.BeginGetDirectory(abbDir, backupPath, true, copyCallback, new object());
                        //update robot backup time
                        lastROBbackupTime = DateTime.Now;
                        labelLastTimeROB.Text = lastROBbackupTime.ToShortTimeString();
                    }
                } else {
                    //now output path - error
                    abbLogger.writeLog(logType.error, "controller <b>" + abbName + "</b>" +
                                                      " [BACKUP MASTER]:  backup done - no output directory!");
                }

            }
        }

        void copyCallback(IAsyncResult res)
        {
            if (res.IsCompleted) {
                abbController.FileSystem.EndCopyDirectory(res);
            }
            
        }

        private string getControllerName(DigitalSignal sig)
        {
            return abbController.SystemName;
        }

        private void textSigDoBackup_Click(object sender, EventArgs e)
        {
            //if signal exists then we subscribe to it - unsubscribe because signal may change
            if (sigBackupExe != null) sigBackupExe.Changed -= DoBackup_Changed;
            //select current signal and show it in signals window
            signalsWindow.selectSig(doBackupIndex);
            signalsWindow.ShowDialog();
            //something might changed - update signal
            doBackupIndex = signalsWindow.selectedIndex;
            doBackupSigName = signalsWindow.selectedSignal;
            //update GUI and manage signal object
            if (doBackupSigName != "") {
                textSigDoBackup.Text = doBackupSigName;
                //create DO signal object 
                sigBackupExe = abbController.IOSystem.GetSignal(doBackupSigName);
                if (sigBackupExe != null) sigBackupExe.Changed += DoBackup_Changed;
            } else {
                textSigDoBackup.Text = "- select signal -";
                //unsubscribe from DO signal object
                sigBackupExe = null;
            }
        }

        private void textSigBackupProg_Click(object sender, EventArgs e)
        {
            //if signal exists then we subscribe to it - unsubscribe because signal may change
            if (sigBackupInP != null) sigBackupInP.Changed -= DiBackup_Changed;
            //select current signal and show it in signals window
            signalsWindow.selectSig(diBackupIndex);
            signalsWindow.ShowDialog();
            //something might changed - update signal
            diBackupIndex = signalsWindow.selectedIndex;
            diBackupSigName = signalsWindow.selectedSignal;
            //update GUI and manage signal object
            if (diBackupSigName != "") {
                textSigBackupProg.Text = diBackupSigName;
                //create DI signal object 
                sigBackupInP = abbController.IOSystem.GetSignal(diBackupSigName);
            }
            else {
                textSigBackupProg.Text = "- select signal -";
                //unsubscribe from DO signal object
                sigBackupInP = null;
            }
        }

        private void btnRobBackupDir_Click(object sender, EventArgs e)
        {
            filesWindow.ShowDialog();
        }
    }
}
