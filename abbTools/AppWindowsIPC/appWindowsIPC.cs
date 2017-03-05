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
        //data containers
        private Controller abbController = null;
        private SignalCollection abbSignals = null;
        private loggerABB abbLogger = null;
        private WindowsIPCClient myClient;
        //enable buttons logic vars
        private bool sigCondition = false,
                     msgCondition = false,
                     serverCondition = false,
                     watchCondition = false;

        /********************************************************
         ***  APP IPC - named pipe client event
         ********************************************************/

        /// <summary>
        /// Default class constructor
        /// </summary>
        public appWindowsIPC()
        {
            //init all form components
            InitializeComponent();
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

        public void desyncLogger()
        {
            //clear controller logger
            abbLogger = null;
        }

        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
        }

        public void desyncController()
        {
            //reset controller address
            abbController = null;
        }

        /********************************************************
         ***  APP IPC - GUI
         ********************************************************/

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

        private void buttonClientON_Click(object sender, EventArgs e)
        {
            //check if user defined server name
            if (textServerName.Text!="") {
                //create client
                myClient = new WindowsIPCClient(textServerName.Text, checkAutoReconnect.Checked);
                //subscribe events
                myClient.OnConnect += clientStatusEvent;
                myClient.OnDisconnect += clientStatusEvent;
                myClient.OnWaiting += clientStatusEvent;
                myClient.OnReceived += clientRecvEvent;
                myClient.OnSent += clientSentEvent;
                myClient.OnEnd += clientEndEvent;
                //open communication pipe
                myClient.open();
                //update GUI buttons 
                clientControlButtons(false);
            } else {
                abbLogger.writeLog(logType.error, "Server IPC - no server name defined");
            }
        }

        private void buttonClientOFF_Click(object sender, EventArgs e)
        {
            //close communication pipe
            myClient.close();
        }

        private void btnAddManualMessage_Click(object sender, EventArgs e)
        {
            //check if user inputted message
            string cMsg = textManualMessage.Text;
            if (cMsg != "") {
                //check if inputted element doesnt exist in list
                if (checkMsgIndex(cMsg) == -1) {
                    updateListBox(listBoxAllMessages, cMsg);
                } else {
                    abbLogger.writeLog(logType.warning, "Inputted message exists in list...");
                }
                //clear message after update
                textManualMessage.Text = "";
            } else {
                abbLogger.writeLog(logType.error, "Input message to be added!");
            }
        }

        private void listBoxMsgMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            removeItemToolStripMenuItem.Enabled = listBoxAllMessages.SelectedIndex >= 0;
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxAllMessages.Items.RemoveAt(listBoxAllMessages.SelectedIndex);
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxAllMessages.Items.Clear();
        }

        private void clientControlButtons(bool clientOff)
        {
            if (clientOff) {
                updateButton(buttonClientON, true, Color.Chartreuse);
                updateButton(buttonClientOFF, false, Color.Silver);
                updateButton(btnSendMsg, false, Color.Silver);
            } else {
                updateButton(buttonClientON, false, Color.Silver);
                updateButton(buttonClientOFF, true, Color.OrangeRed);
                updateButton(btnSendMsg, true, Color.DarkOrange);
            }
        }

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

        private void checkEnableButtons()
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

        private int checkMsgIndex(string msg)
        {
            int result = -1;

            for (int i=0; i<listBoxAllMessages.Items.Count; i++) {
                if(listBoxAllMessages.Items[i].ToString()==msg) {
                    result = i;
                    break;
                }
            }

            return result;
        }

        /********************************************************
         ***  APP IPC - update signals (BACKGROUND TASK)
         ********************************************************/

        private void buttonUpdateSignals_Click(object sender, EventArgs e)
        {
            if (abbController != null) {
                abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updating signals...");
                //clear list items
                listRobotSignals.Items.Clear();
                //run background thread
                backThread.RunWorkerAsync(listRobotSignals);
                //show loading info panel
                panelLoading.BackColor = Color.DarkOrange;
                labelLoadSignals.Text = "reading signals...";
                panelLoading.Visible = true;
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
            abbSignals = abbController.IOSystem.GetSignals(IOFilterTypes.Output);
            //update list in process changed event
            foreach (Signal sig in abbSignals) {
                if (abbController != null) backThread.ReportProgress(0, sig);
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
         ***  INVOKE GUI FROM BACKGORUND THREAD
         ********************************************************/

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
            //update GUI buttons 
            clientControlButtons(true);
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server + "] status: " + e.message);
        }

        /// <summary>
        /// event on change communication status
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientStatusEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server+"] status: "+e.message);
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
                if (checkMsgIndex(e.message)==-1) {
                    updateListBox(listBoxAllMessages, e.message);
                }
            }
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server + "] received: " + e.message);

        }

        /// <summary>
        /// event on message sent to server
        /// </summary>
        /// <param name="sender">object which triggered the event</param>
        /// <param name="e">window IPC event args</param>
        private void clientSentEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server + "] sent: " + e.message);
        }
    }
}
