using System;
using System.Drawing;
using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using abbTools.AppRemoteABB;
using abbTools.Shared;

namespace abbTools
{
    public partial class appRemoteABB : UserControl, IAbbApplication
    {
        /********************************************************
         ***  APP REMOTE - data 
         ********************************************************/

        //private class members - controller address and enable buttons logic vars
        private Controller abbController = null;
        private loggerABB abbLogger = null;
        private bool listCondition = false,
                     actorCondition = false,
                     resultCondition = false,
                     pcCondition = false,
                     watchCondition = false;
        
        //ABB remote and current signals collection
        RemoteABBCollection remoteABB = null;
        SignalCollection abbSignals = null;
        
        //remember checked elements (signals, watch)
        private int selectedSig = -1;
        private int selectedRes = -1;
        private string actorModifiers = "";

        //interface implementation
        public string appName { get; }
        public int appIndex { get; set; }
        public string appIcon { get; set; }
        public string appDescr { get; set; }
        public int appHeight { get; set; }
        public int appWidth { get; set; }

        /********************************************************
         ***  APP REMOTE - constructors
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public appRemoteABB()
        {
            appName = "appRemotePC";
            appDescr = "Control PC remotely from ABB signals";
            InitializeComponent();
            abbController = null;
            abbLogger = null;
            remoteABB = new RemoteABBCollection();
            abbSignals = new SignalCollection();
        }

        /// <summary>
        /// Constructor with logger object update
        /// </summary>
        /// <param name="newLogger">New logger object</param>
        public appRemoteABB(loggerABB newLogger)
        {
            appName = "appRemotePC";
            appDescr = "Control PC remotely from ABB signals";
            InitializeComponent();
            abbController = null;
            abbLogger = newLogger;
            remoteABB = new RemoteABBCollection(abbLogger);
            abbSignals = new SignalCollection();
        }

        /********************************************************
         ***  APP REMOTE - internal elements sync and desync
         ********************************************************/

        /// <summary>
        /// Method used to synchronize logger with RemoteABB app
        /// </summary>
        /// <param name="myLogger">Logger object to sync with</param>
        public void syncLogger(loggerABB myLogger)
        {
            //update controller logger
            abbLogger = myLogger;
            remoteABB.logger = myLogger;
        }

        /// <summary>
        /// Method used to desynchronize current logger with RemoteABB app
        /// </summary>
        public void desyncLogger()
        {
            //clear controller logger
            remoteABB.logger = null;
            abbLogger = null;
        }

        /// <summary>
        /// Method used to synchronize ABB controller with RemoteABB app
        /// </summary>
        /// <param name="myLogger">ABB controller object to sync with</param>
        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
            //add controller to remote collection
            remoteABB.controllerAdd(abbController);
            listActionsWatch.BackColor = Color.White;
            remoteABB.fillWinFormControl(ref listActionsWatch);
        }

        /// <summary>
        /// Method used to desynchronize ABB controller with RemoteABB app
        /// </summary>
        public void desyncController()
        {
            //reset controller address
            abbController = null;
            //clear all robot signals
            if (abbSignals != null) {
                abbSignals.Clear();
                abbSignals = null;
            }
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Method used to clear (desync) controller and logger data in app
        /// </summary>
        public void appRemoteClear()
        {
            //clear all data (robot and reset GUI)
            desyncController();
            desyncLogger();
        }

        /********************************************************
         ***  APP REMOTE - update signals (BACKGROUND TASK)
         ********************************************************/

        /// <summary>
        /// Method used to get all signals from ABB controller using Background Thread
        /// </summary>
        /// <param name="sender">Button that triggered current method</param>
        /// <param name="e">Event arguments</param>
        private void buttonUpdateSignals_Click(object sender, EventArgs e)
        {
            if (abbController != null) {
                //check if background worker is running
                if (!backThread.IsBusy) {
                    abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updating signals...");
                    //clear list items
                    listRobotSignals.Items.Clear();
                    //run background thread
                    backThread.RunWorkerAsync(listRobotSignals);
                    //show loading info panel
                    panelLoading.BackColor = Color.DarkOrange;
                    labelLoadSignals.Text = "reading signals...";
                    panelLoading.Visible = true;
                }
            } else {
                abbLogger.writeLog(logType.warning, "can't update signals... no controller connected!");
                panelLoading.BackColor = Color.Red;
                labelLoadSignals.Text = "no controller...";
            }
            //if controller connected or not show panel with info
            listRobotSignals.BackColor = Color.Silver;
            panelLoading.Visible = true;
        }

        /// <summary>
        /// Method triggered when running background thread
        /// </summary>
        /// <param name="sender">Which thread triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void backThread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //get all signals and report to main thread when finished
            abbSignals = abbController.IOSystem.GetSignals(IOFilterTypes.Digital);
            //update list in process changed event
            foreach (Signal sig in abbSignals) {
                if (abbController != null) backThread.ReportProgress(0, sig);
            }
        }

        /// <summary>
        /// Method triggered when background thread finished its task
        /// </summary>
        /// <param name="sender">Which thread triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (abbController != null) {
                //hide progress bar
                listRobotSignals.BackColor = Color.White;
                panelLoading.Visible = false;
                //update info
                abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updated signals!");
            } else {
                //robot disconnected - clear all its signals
                abbSignals.Clear();
                abbSignals = null;
                listRobotSignals.Items.Clear();
            }
        }

        /// <summary>
        /// Method triggered when background thread work progress changed
        /// </summary>
        /// <param name="sender">Which thread triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list if controller is connected
            if (abbController != null) {
                if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
            }
        }

        /********************************************************
         ***  APP REMOTE - gui
         ********************************************************/

        /// <summary>
        /// Method used to select PC application directory
        /// </summary>
        /// <param name="sender">Button that triggered current method</param>
        /// <param name="e">Event arguments</param>
        private void buttonPCAppSel_Click(object sender, EventArgs e)
        {
            //show file selection dialog
            pcAppLocation.ShowDialog();
            labelAppDir.Text = pcAppLocation.FileName;
            //update logic condition for enabling buttons
            pcCondition = labelAppDir.Text.Contains(".exe");
            checkEnableButtons();
        }

        /// <summary>
        /// Method used to change selected item of ABB signals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event arguments</param>
        private void listRobotSignals_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //uncheck other elements
            for (int i = 0; i < listRobotSignals.Items.Count; ++i) {
                if (i != e.Index) {
                    listRobotSignals.SetItemChecked(i, false);
                }
            }
            //update logic condition for enabling buttons
            listCondition = e.NewValue == CheckState.Checked;
            selectedSig = e.Index;
            checkEnableButtons();
        }

        /// <summary>
        /// Method used to change selection of item in watched remote signals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event arguments</param>
        private void listActionsWatch_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //update logic condition for enabling buttons
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        /// <summary>
        /// Method used to change action modifier
        /// </summary>
        /// <param name="sender">Button that triggered current method</param>
        /// <param name="e">Event arguments</param>
        private void buttonEditModifier_Click(object sender, EventArgs e)
        {
            actorModifiers = showInputWindow(comboResultant.SelectedIndex);
        }

        /// <summary>
        /// Method used to change selected resultant in combo item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event arguments</param>
        private void comboResultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update logic condition for enabling buttons
            resultCondition = comboResultant.SelectedIndex != -1;
            checkEnableButtons();
            //show user window to specify details
            if (comboResultant.SelectedIndex >= (int)RemoteResultant.type.appMouse) {
                //show window only if index was changed
                if (comboResultant.SelectedIndex != selectedRes) {
                    actorModifiers = "";
                    actorModifiers = showInputWindow(comboResultant.SelectedIndex);
                }
                //show button to edit inputted modifiers
                buttonEditModifier.Visible = true;
            } else {
                //hide button editing modifiers (and clean modifier itself)
                buttonEditModifier.Visible = false;
                actorModifiers = "";
            }
            //remember this selection
            selectedRes = comboResultant.SelectedIndex;
        }

        /// <summary>
        /// Check if enable buttons conditions are met (all components have selected items/indexes)
        /// </summary>
        private void checkEnableButtons()
        {
            //one of radio buttons will be always checked but lets check it anyway
            actorCondition = radioChangeTo0.Checked || radioChangeTo1.Checked;
            //modify button condition
            bool modifyCondition = listCondition && actorCondition && resultCondition && pcCondition && watchCondition;
            buttonActionsModify.Enabled = modifyCondition;
            buttonActionsModify.BackColor = modifyCondition ? Color.Yellow : Color.Silver;
            //new button condition
            bool newCondition = listCondition && actorCondition && resultCondition && pcCondition;
            buttonActionsNew.Enabled = newCondition;
            buttonActionsNew.BackColor = newCondition ? Color.Chartreuse : Color.Silver;
            //delete button condition
            bool deleteCondition = watchCondition;
            buttonActionsRemove.Enabled = deleteCondition;
            buttonActionsRemove.BackColor = deleteCondition ? Color.Red : Color.Silver;
        }

        /// <summary>
        /// Function used to show mouse or keybard action input screen
        /// </summary>
        /// <param name="resultantSelected">Which action resultant is showing this window</param>
        /// <returns>Keyboard or mouse action modifier as string</returns>
        private string showInputWindow(int resultantSelected)
        {
            string result = "", header, message;
            bool autoKeyboard = false, autoMouse = false;

            //get window title and message
            switch (resultantSelected) {
                case (int)RemoteResultant.type.appMouse:
                    header = "app mouse click resultant";
                    message = "input mouse position and button to be sim-clicked." + Environment.NewLine +
                              "FORMAT: [posX;posY]L/M/R";
                    autoMouse = true;
                    break;
                case (int)RemoteResultant.type.appKey:
                    header = "app keyboard click resultant";
                    message = "input keyboard button to be sim-clicked." + Environment.NewLine +
                              "FORMAT: keyName";
                    autoKeyboard = true;
                    break;
                default:
                    header = "UNKNOWN RESULT ACTION";
                    message = "no message - resultant action is not supported!";
                    break;
            }
            //create new input window object
            windowInput input = new windowInput(header, message, windowInput.type.txt, actorModifiers, ParentForm);
            //no new taskbar element
            input.ShowInTaskbar = false;
            //position in center of main form
            input.StartPosition = FormStartPosition.Manual;
            input.Top = this.ParentForm.Top + (this.ParentForm.Height - input.Height) / 2;
            input.Left = this.ParentForm.Left + (this.ParentForm.Width - input.Width) / 2;
            //check if we want to get quick input (auto-printing data)
            input.autoFillInput(autoKeyboard, autoMouse);
            //show dialog and get result
            if (input.ShowDialog() == DialogResult.OK) {
                result = input.getUserInput();
            }
            //dispose input at end
            input.Dispose();

            return result;
        }

        /// <summary>
        /// Method used to reset GUI to its initial state (all empty and blanks)
        /// </summary>
        private void resetGUI()
        {
            //clear signals list and show info panel
            listRobotSignals.Items.Clear();
            listRobotSignals.BackColor = Color.Silver;
            panelLoading.Visible = true;
            panelLoading.BackColor = Color.Gold;
            labelLoadSignals.Text = "update signals...";
            //clear watch signal table
            listActionsWatch.Items.Clear();
            listActionsWatch.BackColor = Color.Silver;
            //clear app label
            labelAppDir.Text = "- application directory -";
            //clear resultant selection
            comboResultant.SelectedIndex = -1;
        }

        /********************************************************
         ***  APP REMOTE - watch signal management
         ********************************************************/

        /// <summary>
        /// Method used to add new action to collection
        /// </summary>
        /// <param name="sender">Button that triggered current method</param>
        /// <param name="e">Event arguments</param>
        private void buttonActionsNew_Click(object sender, EventArgs e)
        {
            //add signal to data object
            string selRadioText = radioChangeTo1.Checked ? "change to \"1\"" : "change to \"0\"";
            Signal currSignal = abbController.IOSystem.GetSignal(listRobotSignals.Text);
            remoteABB.addSignal(abbController, currSignal,
                                selRadioText, comboResultant.Text, actorModifiers, labelAppDir.Text);
            //update GUI
            remoteABB.fillWinFormControl(ref listActionsWatch);
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        /// <summary>
        /// Method used to modify selected action in collection
        /// </summary>
        /// <param name="sender">Button that triggered current method</param>
        /// <param name="e">Event arguments</param>
        private void buttonActionsModify_Click(object sender, EventArgs e)
        {
            //only one element can be checked to modification
            if (listActionsWatch.CheckedItems.Count == 1) {
                //modify selected signal in data object into new values
                string selRadioText = radioChangeTo1.Checked ? "change to \"1\"" : "change to \"0\"";
                Signal currSignal = abbController.IOSystem.GetSignal(listRobotSignals.Text);
                remoteABB.modifySignal(listActionsWatch.CheckedItems[0].Index, abbController, currSignal,
                                        selRadioText, comboResultant.Text, actorModifiers, labelAppDir.Text);
                //update GUI
                remoteABB.fillWinFormControl(ref listActionsWatch);
                watchCondition = listActionsWatch.CheckedItems.Count != 0;
                checkEnableButtons();
            } else {
                abbLogger.writeLog(logType.warning, "select only one element to modify!");
            }
        }

        /// <summary>
        /// Method used to remove selected action in collection
        /// </summary>
        /// <param name="sender">Button that triggered current method</param>
        /// <param name="e">Event arguments</param>
        private void buttonActionsRemove_Click(object sender, EventArgs e)
        {
            //remove all checked elements
            int itemIndex = 0;
            foreach (ListViewItem cItem in listActionsWatch.CheckedItems) {
                Signal currSignal = abbController.IOSystem.GetSignal(listActionsWatch.CheckedItems[itemIndex].Text);
                remoteABB.removeSignal(abbController, currSignal);
                itemIndex++;
            }
            //update GUI
            remoteABB.fillWinFormControl(ref listActionsWatch);
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        /********************************************************
         ***  APP REMOTE - data management from main window
         ********************************************************/

        /// <summary>
        /// Method used to clear all application data elements
        /// </summary>
        public void clearAppData()
        {
            //clear collections 
            remoteABB.clear();
        }

        /// <summary>
        /// Method used to save all RemoteABB application data in XML file
        /// </summary>
        /// <param name="saveXml">XML file to save data to</param>
        /// <param name="parent">ABB controller parent of current data</param>
        /// <param name="parentName">ABB name to use if controller is not visible in network</param>
        public void saveAppData(ref System.Xml.XmlWriter saveXml, Controller parent = null, string parentName = "")
        {
            //save current robot child node to XML document
            string saveName = parent != null ? parent.SystemName : parentName;
            if (saveName.Length > 0) {
                remoteABB.saveToXml(ref saveXml, saveName);
            } else {
                abbLogger.writeLog(logType.error, "cant save controller(s) without specified name...");
            }
        }

        /// <summary>
        /// Method used to load RemoteABB application data from XML file
        /// </summary>
        /// <param name="loadXml">XML file to load data from</param>
        /// <param name="parent">ABB controller parent of loaded data</param>
        /// <param name="parentName">ABB name to use when controller isnt visible in network</param>
        public void loadAppData(ref System.Xml.XmlReader loadXml, Controller parent = null, string parentName = "")
        {
            //reset GUI
            resetGUI();
            //read XML untill current app settings node appears
            while (loadXml.Read()) {
                bool start = loadXml.NodeType == System.Xml.XmlNodeType.Element,
                     remotePC = loadXml.Name.StartsWith("remotePC");
                //if we are starting to read remotePC app setting then break from WHILE loop
                if (start && remotePC) {
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
            remoteABB.loadFromXml(ref nodeCurrRobot, parent, parentName);
        }

        /// <summary>
        /// Method triggered when saved ABB controller was found (showed up in network)
        /// </summary>
        /// <param name="found">ABB controller that showed up</param>
        public void controllerFound(Controller found)
        {
            if (found != null) {
                remoteABB.controllerAdd(found);
            }
        }

        /// <summary>
        /// Method triggered when saved ABB controller was lost (dissapeared in network)
        /// </summary>
        /// <param name="lost">ABB controller that was lost</param>
        public void controllerLost(Controller lost)
        {
            if (lost != null) {
                remoteABB.controllerLost(lost);
                //reset GUI
                if(abbController != null && lost.SystemName == abbController.SystemName) resetGUI();
            }
        }
    }
}
