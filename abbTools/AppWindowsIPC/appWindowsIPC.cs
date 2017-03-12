using System;
using System.Drawing;
using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools.AppWindowsIPC
{
    public partial class appWindowsIPC : UserControl
    {
        /********************************************************
         ***  APP IPC - data
         ********************************************************/

        //delegate methods for updating GUI from other threads
        delegate void buttonUpdateUI(Button component, bool enabled, Color clr);
        delegate void listBoxUpdateUI(ListBox list, string newItem);
        //current connection data containers
        private Controller abbController = null;
        private SignalCollection abbSignals = null;
        private loggerABB abbLogger = null;
        private WindowsIPCClient myClient = null;
        //data containers
        private WindowsIPCCollection ipcData = null;
        //enable buttons logic vars
        private bool sigCondition = false,
                     msgCondition = false,
                     serverCondition = false,
                     watchCondition = false;
        //remember current selected data
        private string currMessage = "";
        private string currSignal = "";

        /********************************************************
         ***  APP IPC - named pipe client event
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public appWindowsIPC()
        {
            //init all form components
            InitializeComponent();
            //init data containers
            abbController = null;
            abbSignals = null;
            abbLogger = null;
            myClient = null;
            ipcData = new WindowsIPCCollection();
            //connect collection data with GUI container
            ipcData.connectContainersGUI(listMessagesWatch,textServerName,checkAutoReconnect,checkAutoOpen);
            //reset current data
            currMessage = "";
            currSignal = "";
    }

        /// <summary>
        /// Synchronize logging component address with current app
        /// </summary>
        /// <param name="myLogger">logging component address</param>
        public void syncLogger(loggerABB myLogger)
        {
            //update logging components address
            abbLogger = myLogger;
            //enable main collection to log data
            ipcData.connectLogger(myLogger);
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
            //add controller to ipc collection
            ipcData.controllerUpdate(abbController);
        }

        /// <summary>
        /// Desynchronize current ABB controller from app (no controller actions available)
        /// </summary>
        public void desyncController()
        {
            //clear all robot digital outputs
            if (abbSignals != null) {
                abbSignals.Clear();
                abbSignals = null;
            }
            //reset controller address
            abbController = null;
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Procedure to open named-pipe client with subscribing all events
        /// </summary>
        public void openClientIPC()
        {
            //check if client exists
            if (myClient != null) {
                //subscribe events
                myClient.OnConnect += clientStatusEvent;
                myClient.OnDisconnect += clientStatusEvent;
                myClient.OnWaiting += clientStatusEvent;
                myClient.OnReceived += clientRecvEvent;
                myClient.OnSent += clientSentEvent;
                myClient.OnEnd += clientEndEvent;
                //update event status
                myClient.events = true;
                //open communication pipe
                myClient.open();
            }
        }

        /********************************************************
         ***  APP IPC - GUI
         ********************************************************/

        /// <summary>
        /// Reset GUI components to initial state
        /// </summary>
        public void resetGUI()
        {
            //clear server name and checkboxes
            textServerName.Text = "";
            checkAutoOpen.Checked = false;
            checkAutoReconnect.Checked = false;
            //clear signals list and show info panel
            listRobotSignals.Items.Clear();
            listRobotSignals.BackColor = Color.Silver;
            panelLoading.Visible = true;
            panelLoading.BackColor = Color.Gold;
            labelLoadSignals.Text = "update signals...";
            //clear watch signal table
            listMessagesWatch.Items.Clear();
            listMessagesWatch.BackColor = Color.Silver;
        }

        /// <summary>
        /// Update client control buttons GUI state
        /// </summary>
        /// <param name="clientOff"></param>
        private void clientControlButtons(bool clientOff)
        {
            //check clients new state
            if (clientOff) {
                //update buttons state (works from main and background threads)
                updateButton(buttonClientON, true, Color.Chartreuse);
                updateButton(buttonClientOFF, false, Color.Silver);
            } else {
                //update buttons state (works from main and background threads)
                updateButton(buttonClientON, false, Color.Silver);
                updateButton(buttonClientOFF, true, Color.OrangeRed);
            }
        }

        /// <summary>
        /// Action on click button btnSendMsg
        /// </summary>
        /// <param name="sender">which component triggered event</param>
        /// <param name="e">event arguments</param>
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            //send inputted message from client to 
            if (myClient != null) {
                myClient.send(textMsgToSend.Text);
            }
        }

        /// <summary>
        /// Action on click buttonClientON = named-pipe client open communication
        /// </summary>
        /// <param name="sender">Object which triggered current event</param>
        /// <param name="e">Click event arguments</param>
        private void buttonClientON_Click(object sender, EventArgs e)
        {
            //check if user defined server name
            if (textServerName.Text!="") {
                openClientIPC();
                //update GUI buttons 
                clientControlButtons(false);
            } else {
                abbLogger.writeLog(logType.error, "Server IPC - no server name defined");
            }
        }

        /// <summary>
        /// Action on click buttonClientOFF = named-pipe client close communication
        /// </summary>
        /// <param name="sender">Object which triggered current event</param>
        /// <param name="e">Click event arguments</param>
        private void buttonClientOFF_Click(object sender, EventArgs e)
        {
            //close communication pipe
            myClient.close();
        }

        /// <summary>
        /// Action on click btnAddManualMessage = add user's message to list
        /// </summary>
        /// <param name="sender">Object which triggered current event</param>
        /// <param name="e">Click event arguments</param>
        private void btnAddManualMessage_Click(object sender, EventArgs e)
        {
            //check if user inputted message
            string cMsg = textManualMessage.Text;
            if (cMsg != "") {
                //check if inputted element doesnt exist in list
                int checkIndex = checkMsgIndex(listBoxAllMessages,cMsg);
                if (checkIndex == -1) {
                    updateListBox(listBoxAllMessages, cMsg);
                    abbLogger.writeLog(logType.info, "Message <b>"+cMsg+"</b> added to list!");
                    //select last added item
                    listBoxAllMessages.SelectedIndex = listBoxAllMessages.Items.Count - 1;
                } else {
                    abbLogger.writeLog(logType.warning, "Inputted message <b>"+cMsg+"</b> exists in list [index = "+checkIndex.ToString()+"]...");
                    //select exisiting element number
                    listBoxAllMessages.SelectedIndex = checkIndex;
                }
                //clear message after update
                textManualMessage.Text = "";
            } else {
                abbLogger.writeLog(logType.error, "No message inputted in box!");
            }
        }

        private void listBoxMsgMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //enable remove single item from list if any element is selected
            removeItemToolStripMenuItem.Enabled = listBoxAllMessages.SelectedIndex >= 0;
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //remove selected item from list
            listBoxAllMessages.Items.RemoveAt(listBoxAllMessages.SelectedIndex);
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear all elements from list
            listBoxAllMessages.Items.Clear();
        }

        private void textServerName_TextChanged(object sender, EventArgs e)
        {
            //update logic condition for enabling buttons
            serverCondition = textServerName.Text != "";
            if (serverCondition) {
                //server name present - enable message test components
                panelTextMessage.BackColor = Color.PapayaWhip;
                panelTextMessage.Enabled = true;
                textMsgToSend.BackColor = Color.PapayaWhip;
                textMsgToSend.Enabled = true;
                btnSendMsg.BackColor = Color.DarkOrange;
                btnSendMsg.Enabled = true;
                //server name present - enable message group components
                listBoxAllMessages.BackColor = Color.White;
                listBoxAllMessages.Enabled = true;
                textManualMessage.BackColor = Color.PapayaWhip;
                textManualMessage.Enabled = true;
                btnAddManualMessage.BackColor = Color.DarkOrange;
                btnAddManualMessage.Enabled = true;
            } else {
                //server name missing - disable message test components
                panelTextMessage.BackColor = Color.Silver;
                panelTextMessage.Enabled = false;
                textMsgToSend.BackColor = Color.Silver;
                textMsgToSend.Enabled = false;
                btnSendMsg.BackColor = Color.Silver;
                btnSendMsg.Enabled = false;
                //server name missing - disable message group components
                listBoxAllMessages.BackColor = Color.Silver;
                listBoxAllMessages.Enabled = false;
                textManualMessage.BackColor = Color.Silver;
                textManualMessage.Enabled = false;
                textManualMessage.Text = "";
                btnAddManualMessage.BackColor = Color.Silver;
                btnAddManualMessage.Enabled = false;
            }
            //check watch buttons enabled status
            checkWatchButtons();
        }

        private void listBoxAllMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sIndex = listBoxAllMessages.SelectedIndex;
            //update logic condition for enabling buttons
            msgCondition = sIndex >= 0;
            if (sIndex>=0) currMessage = listBoxAllMessages.Items[sIndex].ToString();
            //check watch buttons enabled status
            checkWatchButtons();
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
            sigCondition = e.NewValue == CheckState.Checked;
            currSignal = listRobotSignals.Items[e.Index].ToString();
            //check watch buttons enabled status
            checkWatchButtons();
        }

        private void listMessagesWatch_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //update logic condition for enabling buttons
            watchCondition = listMessagesWatch.CheckedItems.Count != 0;
            //check watch buttons enabled status
            checkWatchButtons();
        }

        //================================================================

        private void buttonMsgNew_Click(object sender, EventArgs e)
        {
            //add new element to collection
            WindowsIPC newItem = new WindowsIPC(abbController, myClient, abbLogger);
            //fill new item messages
            newItem.messageAdd(currMessage, currSignal, radioSigTo0.Checked ? 0 : 1);
            //add new item to collection (GUI auto-fill)
            ipcData.itemAdd(newItem);
            //update GUI
            listMessagesWatch.BackColor = Color.White;
        }

        private void buttonMsgModify_Click(object sender, EventArgs e)
        {
            //only one element can be modified
            if (listMessagesWatch.CheckedItems.Count == 1) {
                //uncheck current element
                ListViewItem item = listMessagesWatch.CheckedItems[0];
                item.Checked = false;
                //fill old item 
                WindowsIPC oldItem = new WindowsIPC(abbController, myClient, abbLogger);
                oldItem.messageAdd(item.SubItems[1].Text, item.SubItems[2].Text, int.Parse(item.SubItems[3].Text));
                //fill new item messages
                WindowsIPC newItem = new WindowsIPC(abbController, myClient, abbLogger);
                newItem.messageAdd(currMessage, currSignal, radioSigTo0.Checked ? 0 : 1);
                //update element
                ipcData.itemModify(oldItem, newItem);
            } else {
                abbLogger.writeLog(logType.warning, "Select only one element to be modified!");
            }
        }

        private void buttonMsgRemove_Click(object sender, EventArgs e)
        {
            //remove every selected item
            foreach (ListViewItem item in listMessagesWatch.CheckedItems) {
                //uncheck current element
                item.Checked = false;
                //fill remove item messages
                WindowsIPC removeItem = new WindowsIPC(abbController, myClient, abbLogger);
                removeItem.messageAdd(item.SubItems[1].Text, item.SubItems[2].Text, int.Parse(item.SubItems[3].Text));
                //remove item from collection (GUI auto-fill)
                ipcData.itemRemove(removeItem);
            }
            //update GUI
            if (listMessagesWatch.Items.Count==0) listMessagesWatch.BackColor = Color.Silver;
        }

        //================================================================

        private void textServerName_Leave(object sender, EventArgs e)
        {
            //check if client exists and its running
            if (myClient != null && myClient.isRunning()) {
                //client is running - dont do anything
                return;
            }
            //get client data
            string serverName = textServerName.Text;
            bool recon = checkAutoReconnect.Checked;
            bool autoStart = checkAutoOpen.Checked;
            //create client 
            if (serverName != "") {
                myClient = new WindowsIPCClient(serverName, recon, autoStart);
            } else {
                myClient = null;
            }
        }

        private void textServerName_Enter(object sender, EventArgs e)
        {
            //check if current client is running
            if (myClient != null && myClient.isRunning()) {
                abbLogger.writeLog(logType.error, "Cant change server name while running! Stop client and change name!");
            } else {
                //clear my client
                myClient = null;
            }
        }

        private void textServerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (myClient != null && myClient.isRunning()) {
                e.Handled = true;
                abbLogger.writeLog(logType.error, "Key pressed ignored! Stop client and change name!");
            }
        }

        private void checkAutoReconnect_CheckedChanged(object sender, EventArgs e)
        {
            if (myClient != null) myClient.autoRecon = checkAutoReconnect.Checked;
        }

        private void checkAutoOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (myClient != null) myClient.autoStart = checkAutoOpen.Checked;
        }

        private void textManualMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if user presses Enter (return) then simulate add message click
            if (e.KeyChar == '\r') {
                btnAddManualMessage_Click(sender, new EventArgs());
            }
        }

        private void textMsgToSend_Enter(object sender, EventArgs e)
        {
            //cleartext box message title
            textMsgToSend.Text = "";
        }

        private void textMsgToSend_Leave(object sender, EventArgs e)
        {
            //show text box message title
            textMsgToSend.Text = "test message";
        }

        private void textMsgToSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if user presses Enter (return) then simulate add message click
            if (e.KeyChar == '\r') {
                btnSendMsg_Click(sender, new EventArgs());
                textMsgToSend.Text = "";
            }
        }

        //================================================================

        /// <summary>
        /// wrapper of checkBox checked state to use with Invoke and normal approach
        /// </summary>
        /// <param name="newCheckState">true if checkbox is to be checked</param>
        private void updCheckBoxState(CheckBox component, bool newCheckState)
        {
            if (component != null) {
                component.Checked = newCheckState;
            }
        }

        private void refreshButton(Button btn, bool enable, Color clr)
        {
            if (btn != null) {
                btn.BackColor = clr;
                btn.Enabled = enable;
            }
        }

        private void addListBoxItem(ListBox list, string item)
        {
            if (list != null) {
                list.Items.Add(item);
            }
        }

        /********************************************************
         ***  LOGIC
         ********************************************************/

        private void checkWatchButtons()
        {
            //modify button condition
            bool modifyCondition = sigCondition && serverCondition && msgCondition && watchCondition;
            buttonMsgModify.Enabled = modifyCondition;
            buttonMsgModify.BackColor = modifyCondition ? Color.Yellow : Color.Silver;
            //new button condition
            bool newCondition = sigCondition && serverCondition && msgCondition;
            buttonMsgNew.Enabled = newCondition;
            buttonMsgNew.BackColor = newCondition ? Color.Chartreuse : Color.Silver;
            //delete button condition
            bool deleteCondition = watchCondition;
            buttonMsgRemove.Enabled = deleteCondition;
            buttonMsgRemove.BackColor = deleteCondition ? Color.Red : Color.Silver;
        }

        /// <summary>
        /// Method used to check index of message in selected ListBox parent
        /// </summary>
        /// <param name="parent">ListBox to check for message</param>
        /// <param name="msg">Message which index we want to find</param>
        /// <returns>Index of inputted message (-1 if message not exists)</returns>
        private int checkMsgIndex(ListBox parent, string msg)
        {
            int result = -1;

            for (int i=0; i< parent.Items.Count; i++) {
                if(parent.Items[i].ToString()==msg) {
                    result = i;
                    break;
                }
            }

            return result;
        }

        /********************************************************/
        /***  APP IPC - update signals (BACKGROUND TASK)         /
        /********************************************************/

        private void buttonUpdateSignals_Click(object sender, EventArgs e)
        {
            if (abbController != null) {
                //check if background worker is running
                if (!backThread.IsBusy){
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
                if (sig.Type == SignalType.DigitalOutput) {
                    if (abbController != null) backThread.ReportProgress(0, sig);
                }
            }
        }

        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list if controller is connected
            if (abbController != null) {
                if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
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

        /********************************************************
         ***  INVOKE GUI FROM BACKGORUND OR MAIN THREAD
         ********************************************************/

        /// <summary>
        /// Method to update Button color and enable state (auto-check if update from background thread or main)
        /// </summary>
        /// <param name="btn">Button to update</param>
        /// <param name="enable">new Button enable state</param>
        /// <param name="clr">new Button color</param>
        private void updateButton(Button btn, bool enable, Color clr)
        {
            if (btn.InvokeRequired) {
                //funcion running from other thread - use Invoke
                buttonUpdateUI test = new buttonUpdateUI(refreshButton);
                Invoke(test, new object[] { btn, enable, clr });
            } else {
                //function running from main thread - normal approach
                refreshButton(btn, enable, clr);
            }
        }

        /// <summary>
        /// Method to update ListBox item (auto-check if update from background thread or main)
        /// </summary>
        /// <param name="list">ListBox item to update</param>
        /// <param name="txt">New item to add to ListBox</param>
        private void updateListBox(ListBox list, string txt)
        {
            if (list.InvokeRequired) {
                //funcion running from other thread - use Invoke
                listBoxUpdateUI test = new listBoxUpdateUI(addListBoxItem);
                Invoke(test, new object[] { list, txt });
            } else {
                //function running from main thread - normal approach
                addListBoxItem(list, txt);
            }
        }

        /********************************************************
         ***  APP IPC - named pipe client events
         ********************************************************/

        /// <summary>
        /// event on background communication thread end
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientEndEvent(object sender, WindowsIPCEventArgs e)
        {
            //unsubscribe all client events
            myClient.OnConnect -= clientStatusEvent;
            myClient.OnDisconnect -= clientStatusEvent;
            myClient.OnWaiting -= clientStatusEvent;
            myClient.OnReceived -= clientRecvEvent;
            myClient.OnSent -= clientSentEvent;
            myClient.OnEnd -= clientEndEvent;
            //update event status
            myClient.events = false;
            //update GUI buttons 
            clientControlButtons(true);
            //show log info
            abbLogger.writeLog(logType.info, "[TEST IPC client " + e.server + "] status: " + e.message);
        }

        /// <summary>
        /// event on change communication status
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientStatusEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[TEST IPC client " + e.server+"] status: "+e.message);
        }

        /// <summary>
        /// event on message received from server
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientRecvEvent(object sender, WindowsIPCEventArgs e)
        {
            //check if we want to input incomming messages to list
            if (checkAutoFillMessages.Checked) {
                //check if curr element doesnt exist in list
                if (checkMsgIndex(listBoxAllMessages,e.message)==-1) {
                    updateListBox(listBoxAllMessages, e.message);
                }
            }
            //show log info
            abbLogger.writeLog(logType.info, "[TEST IPC client " + e.server + "] received: " + e.message);
        }

        /// <summary>
        /// event on message sent to server
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientSentEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[TEST IPC client " + e.server + "] sent: " + e.message);
        }

        /********************************************************
         ***  APP IPC - data management from main window
         ********************************************************/

        public void clearAppData()
        {
            //clear collections 
            ipcData.clear();
            //disconnect logger 
            ipcData.disconnectLogger();
        }

        public void saveAppData(ref System.Xml.XmlWriter saveXml, Controller parent = null, string parentName = "")
        {
            //save current robot child node to XML document
            string saveName = parent != null ? parent.SystemName : parentName;
            if (saveName.Length > 0) {
                ipcData.saveToXml(ref saveXml, saveName);
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
                     windowsIPC = loadXml.Name.StartsWith("windowsIPC");
                //if we are starting to read windowsIPC app setting then break from WHILE loop
                if (start && windowsIPC) {
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
            ipcData.loadFromXml(ref nodeCurrRobot, parent, parentName);
        }

        public void savedControllerFound(Controller found)
        {
            //update controllers data in collection
            //it was saved but non-visible at start, but it showed up right now!
            if (found != null) {
                ipcData.controllerUpdate(found);
            }
        }

        public void savedControllerLost(Controller lost)
        {
            if (lost != null) {
                ipcData.controllerClear(lost);
                //reset GUI
                if (abbController != null && lost.SystemName == abbController.SystemName) resetGUI();
            }
        }

    }
}
