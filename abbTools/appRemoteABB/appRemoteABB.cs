using System;
using System.Drawing;
using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using abbTools.AppRemoteABB;

namespace abbTools
{
    public partial class appRemoteABB : UserControl
    {
        /********************************************************
         ***  APP REMOTE - variables
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
        
        /********************************************************
         ***  APP REMOTE - main operations
         ********************************************************/

        public appRemoteABB()
        {
            InitializeComponent();
            abbController = null;
            abbLogger = null;
            remoteABB = new RemoteABBCollection();
            abbSignals = new SignalCollection();
        }

        public appRemoteABB(loggerABB newLogger)
        {
            InitializeComponent();
            abbController = null;
            abbLogger = newLogger;
            remoteABB = new RemoteABBCollection(abbLogger);
            abbSignals = new SignalCollection();
        }

        public void appRemoteClear()
        {
            //clear all data (robot and reset GUI)
            desyncController();
            desyncLogger();
        }

        public void syncLogger(loggerABB myLogger)
        {
            //update controller logger
            abbLogger = myLogger;
            remoteABB.syncLogger(myLogger);
        }

        public void desyncLogger()
        {
            //clear controller logger
            remoteABB.desyncLogger();
            abbLogger = null;
        }

        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
            //add controller to remote collection
            remoteABB.addController(abbController);
            listActionsWatch.BackColor = Color.White;
            remoteABB.fillWinFormControl(ref listActionsWatch);
        }

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

        /********************************************************
         ***  APP REMOTE - update signals (BACKGROUND TASK)
         ********************************************************/

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

        private void backThread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //get all signals and report to main thread when finished
            abbSignals = abbController.IOSystem.GetSignals(IOFilterTypes.Digital);
            //update list in process changed event
            foreach (Signal sig in abbSignals) {
                if (abbController != null) backThread.ReportProgress(0, sig);
            }
        }

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

        private void buttonPCAppSel_Click(object sender, EventArgs e)
        {
            //show file selection dialog
            pcAppLocation.ShowDialog();
            labelAppDir.Text = pcAppLocation.FileName;
            //update logic condition for enabling buttons
            pcCondition = labelAppDir.Text.Contains(".exe");
            checkEnableButtons();
        }

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

        private void listActionsWatch_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //update logic condition for enabling buttons
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        private void buttonEditModifier_Click(object sender, EventArgs e)
        {
            actorModifiers = showInputWindow(comboResultant.SelectedIndex);
        }

        private void comboResultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update logic condition for enabling buttons
            resultCondition = comboResultant.SelectedIndex != -1;
            checkEnableButtons();
            //show user window to specify details
            if (comboResultant.SelectedIndex >= (int)RemoteAction.type.appMouse) {
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

        private string showInputWindow(int resultantSelected)
        {
            string result = "", header, message;
            bool autoKeyboard = false, autoMouse = false;

            //get window title and message
            switch (resultantSelected) {
                case (int)RemoteAction.type.appMouse:
                    header = "app mouse click resultant";
                    message = "input mouse position and button to be sim-clicked." + Environment.NewLine +
                              "FORMAT: [posX;posY]L/M/R";
                    autoMouse = true;
                    break;
                case (int)RemoteAction.type.appKey:
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

        public void clearAppData()
        {
            //clear collections 
            remoteABB.clear();
        }

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

        public void controllerFound(Controller found)
        {
            if (found != null) {
                remoteABB.addController(found);
            }
        }

        public void controllerLost(Controller lost)
        {
            if (lost != null) {
                remoteABB.clearController(lost);
                //reset GUI
                if(abbController != null && lost.SystemName == abbController.SystemName) resetGUI();
            }
        }
    }
}
