using System;
using System.IO;
using System.IO.Pipes;
using System.Windows.Forms;

namespace abbTools.AppWindowsIPC
{
    public partial class appWindowsIPC : UserControl
    {
        /********************************************************
         ***  APP IPC - data
         ********************************************************/

        //delegate methods for updating GUI from other threads
        delegate void CheckBoxCallback(CheckBox component, bool checkState);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkIPCclientState_CheckedChanged(object sender, EventArgs e)
        {
            if (checkIPCclientState.Checked) {
                //create client
                myClient = new WindowsIPCClient("abc",false);
                //subscribe events
                myClient.OnConnect += clientStatusEvent;
                myClient.OnDisconnect += clientStatusEvent;
                myClient.OnWaiting += clientStatusEvent;
                myClient.OnReceived += clientRecvEvent;
                myClient.OnSent += clientSentEvent;
                myClient.OnEnd += clientEndEvent;
                //open communication pipe
                myClient.open();
            } else {
                //close communication pipe
                myClient.close();
            }
        }

        /********************************************************
         ***  APP IPC - named pipe client events
         ********************************************************/

        private void clientEndEvent(object sender, WindowsIPCEventArgs e)
        {
            //unsubscribe all client events
            myClient.OnConnect -= clientStatusEvent;
            myClient.OnDisconnect -= clientStatusEvent;
            myClient.OnWaiting -= clientStatusEvent;
            myClient.OnReceived -= clientRecvEvent;
            myClient.OnSent -= clientSentEvent;
            myClient.OnEnd -= clientEndEvent;
            //uncheck box - check if running in other thread
            if (checkIPCclientState.InvokeRequired) {
                //funcion running from other thread - use Invoke
                CheckBoxCallback test = new CheckBoxCallback(updCheckBoxState);
                Invoke(test, new object[] { checkIPCclientState, e.restore });
            } else {
                //function running from main thread - normal approach
                updCheckBoxState(checkIPCclientState, e.restore);
            }
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server + "] status: " + e.message);
        }

        private void clientStatusEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server+"] status: "+e.message);
        }

        private void clientRecvEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server + "] received: " + e.message);
        }

        private void clientSentEvent(object sender, WindowsIPCEventArgs e)
        {
            //show log info
            abbLogger.writeLog(logType.info, "[IPC client " + e.server + "] sent: " + e.message);
        }
    }
}
