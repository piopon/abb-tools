using System;
using System.Drawing;
using System.Windows.Forms;

namespace abbTools.AppWindowsIPC
{
    public partial class appWindowsIPC : UserControl
    {
        /********************************************************
         ***  APP IPC - data
         ********************************************************/

        //delegate methods for updating GUI from other threads
        delegate void buttonUpdateUI(Button component, bool enabled, Color clr);
        //data containers
        private loggerABB abbLogger = null;
        private WindowsIPCClient myClient;

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

        private void refreshButton(Button btn, bool enable, Color clr)
        {
            btn.BackColor = clr;
            btn.Enabled = enable;
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
