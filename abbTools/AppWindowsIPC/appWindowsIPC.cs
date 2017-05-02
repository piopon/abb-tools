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
        private WindowsIPCClient testClient = null;
        //data containers
        private WindowsIPCCollection ipcData = null;
        //enable buttons logic vars
        private bool sigCondition = false,
                     msgCondition = false,
                     serverCondition = false,
                     watchCondition = false;
        //remember current selected data
        private int syncIndex = -1;
        public int parentHeight = 0;
        public int parentWidth = 0;
        private string currMessage = "";
        private string currSignal = "";
        private bool checkBoxClick;

        /********************************************************
         ***  APP IPC - manage connection data containers
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
            ipcData.ClientControlChange += clientControlButtons;
            //reset current data
            syncIndex = -1;
            currMessage = "";
            currSignal = "";
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
            //update item options
            syncIndex = ipcData.controllerIndex(myController.SystemName);
            ipcData[syncIndex].controllerConnected = true;
            myClient = ipcData[syncIndex].ipcClient;
            if (myClient != null) {
                clientControlButtons(!myClient.running);
            } else {
                myClient = testClient;
            }
            //reset GUI
            resetGUI();
            //update GUI
            ipcData.updateContainerGUI(listMessagesWatch);
            ipcData.updateServerGUI(textServerName, checkAutoReconnect, checkAutoOpen);
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
            myClient = testClient;
            ipcData[syncIndex].controllerConnected = false;
            syncIndex = -1;
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
                if (!myClient.eventsConn) {
                    myClient.OnConnect += clientStatusEvent;
                    myClient.OnDisconnect += clientStatusEvent;
                    myClient.OnWaiting += clientStatusEvent;
                    myClient.OnReceived += clientRecvEvent;
                    myClient.OnSent += clientSentEvent;
                    myClient.OnEnd += clientEndEvent;
                }
                //update event status
                myClient.eventsConn = true;
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
            if (myClient != null) {
                textServerName.Text = myClient.server;
                checkAutoOpen.Checked = myClient.autoOpen;
                checkAutoReconnect.Checked = myClient.autoRecon;
                clientControlButtons(!myClient.running);
            } else {
                textServerName.Text = "";
                checkAutoOpen.Checked = false;
                checkAutoReconnect.Checked = false;
                clientControlButtons(true);
            }
            if (abbController == null) {
                //clear signals list and show info panel
                listRobotSignals.SelectedItems.Clear();
                listRobotSignals.Items.Clear();
                listRobotSignals.BackColor = Color.Silver;
                panelLoading.Visible = true;
                panelLoading.BackColor = Color.Gold;
                labelLoadSignals.Text = "update signals...";
                //clear watch signal table
                listMessagesWatch.SelectedItems.Clear();
                listMessagesWatch.Items.Clear();
                listMessagesWatch.BackColor = Color.Silver;
            }
            //disable watch buttons
            sigCondition = listRobotSignals.SelectedItems.Count > 0;
            serverCondition = textServerName.Text != "";
            msgCondition = listBoxAllMessages.SelectedItems.Count > 0;
            watchCondition = listMessagesWatch.SelectedItems.Count > 0;
            checkWatchButtons();
        }

        /// <summary>
        /// Update client control buttons GUI state
        /// </summary>
        /// <param name="clientOff">Input TRUE if buttons have to represent client off state</param>
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
            //update label identifing client name
            updateClientDescr();
        }

        /// <summary>
        /// Action on click button btnSendMsg
        /// </summary>
        /// <param name="sender">which component triggered event</param>
        /// <param name="e">event arguments</param>
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            //send inputted message from client to 
            myClient?.send(textMsgToSend.Text);
        }

        /// <summary>
        /// Action on click buttonClientON = named-pipe client open communication
        /// </summary>
        /// <param name="sender">Object which triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void buttonClientON_Click(object sender, EventArgs e)
        {
            //check if user defined server name
            if (myClient == testClient) {
                if (textServerName.Text != "") {
                    openClientIPC();
                    clientControlButtons(false);
                } else {
                    abbLogger?.writeLog(logType.error, "Server IPC - no server name defined");
                }
            } else {
                int collectionIndex = ipcData.controllerIndex(abbController.SystemName);
                if (collectionIndex >= 0) {
                    ipcData[collectionIndex].ipcClientOpen();
                }
            }
        }

        /// <summary>
        /// Action on click buttonClientOFF = named-pipe client close communication
        /// </summary>
        /// <param name="sender">Object which triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void buttonClientOFF_Click(object sender, EventArgs e)
        {
            //close communication pipe
            myClient?.close();
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
                    abbLogger?.writeLog(logType.info, "Message <b>"+cMsg+"</b> added to list!");
                    //select last added item
                    listBoxAllMessages.SelectedIndex = listBoxAllMessages.Items.Count - 1;
                } else {
                    abbLogger?.writeLog(logType.warning, "Inputted message <b>"+cMsg+"</b> exists in list [index = "+checkIndex.ToString()+"]...");
                    //select exisiting element number
                    listBoxAllMessages.SelectedIndex = checkIndex;
                }
                //clear message after update
                textManualMessage.Text = "";
            } else {
                abbLogger?.writeLog(logType.error, "No message inputted in box!");
            }
        }

        /// <summary>
        /// Event on popup menu open when messages right clicked
        /// </summary>
        /// <param name="sender">ListBox parent of popup menu</param>
        /// <param name="e">Event arguments</param>
        private void listBoxMsgMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //enable remove single item from list if any element is selected
            removeItemToolStripMenuItem.Enabled = listBoxAllMessages.SelectedIndex >= 0;
        }

        /// <summary>
        /// Event on remove item from messages popup menu
        /// </summary>
        /// <param name="sender">Popup menu item that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //remove selected item from list
            listBoxAllMessages.Items.RemoveAt(listBoxAllMessages.SelectedIndex);
        }

        /// <summary>
        /// Event on clear all item from messages popup menu
        /// </summary>
        /// <param name="sender">Popup menu item that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear all elements from list
            listBoxAllMessages.Items.Clear();
        }

        /// <summary>
        /// Action triggered on server name text change 
        /// </summary>
        /// <param name="sender">TextBox that triggered current event</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Event triggered on index change of messages list
        /// </summary>
        /// <param name="sender">ListBox parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void listBoxAllMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sIndex = listBoxAllMessages.SelectedIndex;
            //update logic condition for enabling buttons
            msgCondition = sIndex >= 0;
            if (sIndex>=0) currMessage = listBoxAllMessages.Items[sIndex].ToString();
            //check watch buttons enabled status
            checkWatchButtons();
        }

        /// <summary>
        /// Event triggered on change of item in robot signals list
        /// </summary>
        /// <param name="sender">CheckedListBox that triggered this event</param>
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
            sigCondition = e.NewValue == CheckState.Checked;
            currSignal = listRobotSignals.Items[e.Index].ToString();
            //check watch buttons enabled status
            checkWatchButtons();
        }

        /// <summary>
        /// Event triggered on check of item in messages list
        /// </summary>
        /// <param name="sender">CheckedListBox that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void listMessagesWatch_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //update logic condition for enabling buttons
            watchCondition = listMessagesWatch.CheckedItems.Count != 0;
            //check watch buttons enabled status
            checkWatchButtons();
        }

        //================================================================

        /// <summary>
        /// Event triggered on new message button click
        /// </summary>
        /// <param name="sender">Button parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void buttonMsgNew_Click(object sender, EventArgs e)
        {
            //add new element to collection
            WindowsIPC newItem = new WindowsIPC(abbController, myClient, abbLogger);
            //fill new item messages
            newItem.messageAdd(currMessage, currSignal, radioSigTo0.Checked ? 0 : 1);
            //add new item to collection (GUI auto-fill)
            ipcData.itemAdd(newItem);
            myClient = ipcData[ipcData.controllerIndex(abbController.SystemName)].ipcClient;
            updateClientDescr();
            //update GUI
            ipcData.updateContainerGUI(listMessagesWatch);
            ipcData.updateServerGUI(textServerName, checkAutoReconnect, checkAutoOpen);
        }

        /// <summary>
        /// Event triggered on modify message button click
        /// </summary>
        /// <param name="sender">Button parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
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
                //update GUI
                ipcData.updateContainerGUI(listMessagesWatch);
                ipcData.updateServerGUI(textServerName, checkAutoReconnect, checkAutoOpen);
            } else {
                abbLogger?.writeLog(logType.warning, "Select only one element to be modified!");
            }
        }

        /// <summary>
        /// Event triggered on remove message button click
        /// </summary>
        /// <param name="sender">Button parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
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
                //update GUI
                ipcData.updateContainerGUI(listMessagesWatch);
                ipcData.updateServerGUI(textServerName, checkAutoReconnect, checkAutoOpen);
            }
            //update GUI
            if (listMessagesWatch.Items.Count==0) listMessagesWatch.BackColor = Color.Silver;
        }

        //================================================================

        /// <summary>
        /// Event triggered on focus leave from servers name text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textServerName_Leave(object sender, EventArgs e)
        {
            //check if client exists and its running
            if (myClient != null && myClient.running) {
                //client is running - dont do anything
                return;
            }
            //get client data
            string serverName = textServerName.Text;
            bool recon = checkAutoReconnect.Checked;
            bool autoStart = checkAutoOpen.Checked;
            //create client (if no client or test client)
            if (myClient == null || myClient == testClient) {
                if (serverName != "") {
                    testClient = new WindowsIPCClient(serverName, recon, autoStart);
                } else {
                    testClient = null;
                }
                myClient = testClient;
            } else {
                //collection data client - check if user changed name
                if (myClient.server != serverName) {
                    //server name changed - update it
                    ipcData[syncIndex].ipcClientUpdate(serverName, myClient.autoRecon, myClient.autoOpen);
                    //update GUI (messages list and server settings)
                    ipcData.updateContainerGUI(listMessagesWatch);
                    ipcData.updateServerGUI(textServerName,checkAutoReconnect,checkAutoOpen);
                    //update myClient address
                    myClient = ipcData[syncIndex].ipcClient;
                    //show log
                    abbLogger?.writeLog(logType.info, "Setting applied to all messages for current robot!");
                }
            }
            //update label identifing client name
            updateClientDescr();
        }

        /// <summary>
        /// Event triggered on focus enter from servers name text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textServerName_Enter(object sender, EventArgs e)
        {
            //check if current client is running
            if (myClient != null && myClient.running) {
                abbLogger?.writeLog(logType.error, "Cant change server name while running! Stop client and change name!");
            } else {
                //clear my client (if no client or test client)
                if (myClient == null || myClient == testClient) myClient = null;
            }
            //update label identifing client name
            updateClientDescr();
        }

        /// <summary>
        /// Event triggered on key press at servers name text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textServerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (myClient != null) {
                //there is client defined
                if (myClient.running) {
                    //its running = cant change name!
                    e.Handled = true;
                    abbLogger?.writeLog(logType.error, "Key pressed ignored! Stop client and change name!");
                } else {
                    //its not running = change name available
                    if (myClient != testClient && syncIndex >= 0 && ipcData[syncIndex].messagesCount() > 0) {
                        //let the user know that changing collection client will affect its messages
                        abbLogger?.writeLog(logType.warning, "New server name will be applied to all messages!");
                    }
                }
            }
        }

        /// <summary>
        /// Event triggered on auto reconnect check changed
        /// </summary>
        /// <param name="sender">CheckBox parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void checkAutoReconnect_CheckedChanged(object sender, EventArgs e)
        {
            if (myClient != null) {
                myClient.autoRecon = checkAutoReconnect.Checked;
                //check if we are operating on client in collection
                if (myClient != testClient && checkBoxClick) {
                    //leave info for user that we have changed
                    abbLogger?.writeLog(logType.info, "Setting applied to all messages for current robot!");
                }
            }
            checkBoxClick = false;
        }

        /// <summary>
        /// Event triggered on auto open communication check changed
        /// </summary>
        /// <param name="sender">CheckBox parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void checkAutoOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (myClient != null) {
                myClient.autoOpen = checkAutoOpen.Checked;
                //check if we are operating on client in collection
                if (myClient != testClient) {
                    //leave info for user that we have changed
                    if (checkBoxClick) abbLogger?.writeLog(logType.info, "Setting applied to all messages for current robot!");
                }
            }
            checkBoxClick = false;
        }

        /// <summary>
        /// Event triggered on key press at message text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textManualMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if user presses Enter (return) then simulate add message click
            if (e.KeyChar == '\r') btnAddManualMessage_Click(sender, new EventArgs());
        }

        /// <summary>
        /// Event triggered on focus enter at send message text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textMsgToSend_Enter(object sender, EventArgs e)
        {
            //cleartext box message title
            textMsgToSend.Text = "";
        }

        /// <summary>
        /// Event triggered on focus leave from send message text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textMsgToSend_Leave(object sender, EventArgs e)
        {
            //show text box message title
            textMsgToSend.Text = "test message";
        }

        /// <summary>
        /// Event triggered on key press at send message text box
        /// </summary>
        /// <param name="sender">TextBox parent that triggered event</param>
        /// <param name="e">Event arguments</param>
        private void textMsgToSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if user presses Enter (return) then simulate add message click
            if (e.KeyChar == '\r') {
                btnSendMsg_Click(sender, new EventArgs());
                textMsgToSend.Text = "";
            }
        }

        /// <summary>
        /// Method called to update label client description
        /// </summary>
        private void updateClientDescr()
        {
            string clientDescr = "";
            if (myClient != null) {
                clientDescr = myClient == testClient ? "TEST > " + myClient.server : "DATA > " + myClient.server;
            } else {
                clientDescr = "null";
            }
            labelClientAddress.Text = "info: " + clientDescr;
        }

        /// <summary>
        /// Event triggered when mouse button is down on auto open checkbox
        /// </summary>
        /// <param name="sender">CheckBox that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void checkAutoOpen_MouseDown(object sender, MouseEventArgs e)
        {
            checkBoxClick = true;
        }

        /// <summary>
        /// Event triggered when mouse button is down on auto reconnect checkbox
        /// </summary>
        /// <param name="sender">CheckBox that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void checkAutoReconnect_MouseDown(object sender, MouseEventArgs e)
        {
            checkBoxClick = true;
        }

        /// <summary>
        /// Event triggered when link label is clicked
        /// </summary>
        /// <param name="sender">LinkLabel that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void linkMoreDescr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            windowClientStatus status = new windowClientStatus(testClient, ipcData);
            status.StartPosition = FormStartPosition.CenterParent;
            status.Height = parentHeight;
            status.Width = parentWidth;
            status.ShowInTaskbar = false;
            status.ShowDialog();
        }

        //================================================================

        /// <summary>
        /// wrapper of checkBox checked state to use with Invoke and normal approach
        /// </summary>
        /// <param name="component"></param>
        /// <param name="newCheckState">true if checkbox is to be checked</param>
        private void updCheckBoxState(CheckBox component, bool newCheckState)
        {
            if (component != null) {
                component.Checked = newCheckState;
            }
        }

        /// <summary>
        /// Method used to update selected buttons apperance
        /// </summary>
        /// <param name="btn">Button parent to refresh apperance</param>
        /// <param name="enable">Should button be enabled</param>
        /// <param name="clr">Color of button</param>
        private void refreshButton(Button btn, bool enable, Color clr)
        {
            if (btn != null) {
                btn.BackColor = clr;
                btn.Enabled = enable;
            }
        }

        /// <summary>
        /// Method called when adding new item to listbox 
        /// </summary>
        /// <param name="list">LisbBox to add item</param>
        /// <param name="item">Item to add</param>
        private void addListBoxItem(ListBox list, string item)
        {
             list?.Items.Add(item);
        }

        /********************************************************
         ***  LOGIC
         ********************************************************/

        /// <summary>
        /// Method used to check message action butoons state and apperance
        /// </summary>
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

        /// <summary>
        /// Event triggered on update signals button click (execute backuground thread)
        /// </summary>
        /// <param name="sender">Button parent that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void buttonUpdateSignals_Click(object sender, EventArgs e)
        {
            if (abbController != null) {
                //check if background worker is running
                if (!backThread.IsBusy){
                    abbLogger?.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updating signals...");
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
                abbLogger?.writeLog(logType.warning, "can't update signals... no controller connected!");
                panelLoading.BackColor = Color.Red;
                labelLoadSignals.Text = "no controller...";
            }
            //if controller connected or not show panel with info
            listRobotSignals.BackColor = Color.Silver;
            panelLoading.Visible = true;
        }

        /// <summary>
        /// Actions executed when running background thread
        /// </summary>
        /// <param name="sender">Parent thread</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Event triggered when background threads action changed its state
        /// </summary>
        /// <param name="sender">Parent thread</param>
        /// <param name="e">Event arguments</param>
        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list if controller is connected
            if (abbController != null) {
                if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
            }
        }

        /// <summary>
        /// Event triggered when background thread completed its action
        /// </summary>
        /// <param name="sender">Parent thread</param>
        /// <param name="e">Event arguments</param>
        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (abbController != null) {
                //hide progress bar
                listRobotSignals.BackColor = Color.White;
                panelLoading.Visible = false;
                //update info
                abbLogger?.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updated signals!");
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
            if (myClient.eventsConn) {
                myClient.OnConnect -= clientStatusEvent;
                myClient.OnDisconnect -= clientStatusEvent;
                myClient.OnWaiting -= clientStatusEvent;
                myClient.OnReceived -= clientRecvEvent;
                myClient.OnSent -= clientSentEvent;
                myClient.OnEnd -= clientEndEvent;
            }
            //update event status
            myClient.eventsConn = false;
            //update GUI buttons 
            clientControlButtons(true);
            //show log info
            abbLogger?.writeLog(logType.info, "[TEST IPC client " + e.server + "] status: " + e.message);
        }

        /// <summary>
        /// event on change communication status
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientStatusEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger?.writeLog(logType.info, "[TEST IPC client " + e.server+"] status: "+e.message);
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
            abbLogger?.writeLog(logType.info, "[TEST IPC client " + e.server + "] received: " + e.message);
        }

        /// <summary>
        /// event on message sent to server
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientSentEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger?.writeLog(logType.info, "[TEST IPC client " + e.server + "] sent: " + e.message);
        }

        /********************************************************
         ***  APP IPC - data management from main window
         ********************************************************/

        /// <summary>
        /// Method called to clear all app data containers
        /// </summary>
        public void clearAppData()
        {
            //clear collections 
            ipcData.clear();
            //disconnect logger 
            ipcData.disconnectLogger();
        }

        /// <summary>
        /// Method called when saving app data to XML file
        /// </summary>
        /// <param name="saveXml">XML file to save data to</param>
        /// <param name="parent">ABB controller parent of current data</param>
        /// <param name="parentName">ABB controllers name (useful when controller was lost in network)</param>
        public void saveAppData(ref System.Xml.XmlWriter saveXml, Controller parent = null, string parentName = "")
        {
            //save current robot child node to XML document
            string saveName = parent != null ? parent.SystemName : parentName;
            if (saveName.Length > 0) {
                ipcData.saveToXml(ref saveXml, saveName);
            } else {
                abbLogger?.writeLog(logType.error, "cant save controller(s) without specified name...");
            }
        }

        /// <summary>
        /// Method called when loading app data from XML file
        /// </summary>
        /// <param name="loadXml">XML file to load data from</param>
        /// <param name="parent">ABB controller parent of current data</param>
        /// <param name="parentName">ABB controllers name (useful when controller isnt visible in network)</param>
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
            //reset GUI
            resetGUI();
        }

        /// <summary>
        /// Method called when ABB controller from saved group was found in network
        /// </summary>
        /// <param name="found">ABB controller that was found in network</param>
        public void savedControllerFound(Controller found)
        {
            //update controllers data in collection
            //it was saved but non-visible at start, but it showed up right now!
            if (found != null) {
                ipcData.controllerUpdate(found);
                //check if client has auto-open flag set to true
                int foundIndex = ipcData.controllerIndex(found.SystemName);
                if (ipcData[foundIndex].ipcClient.autoOpen && !ipcData[foundIndex].ipcClient.running) {
                    //auto-open TRUE = open client
                    ipcData[foundIndex].ipcClientOpen();
                }
            }
        }

        /// <summary>
        /// Method called when ABB controller from saved group was lost in network
        /// </summary>
        /// <param name="lost">ABB controller that was lost in network</param>
        public void savedControllerLost(Controller lost)
        {
            if (lost != null) {
                ipcData.controllerClear(lost);
                //reset GUI
                if (abbController != null && lost.SystemName == abbController.SystemName) {
                    //clear logger and client
                    abbController = null;
                    syncIndex = -1;
                    myClient = testClient;
                    updateClientDescr();
                    //reset GUI
                    resetGUI();
                }
            }
        }
    }
}
