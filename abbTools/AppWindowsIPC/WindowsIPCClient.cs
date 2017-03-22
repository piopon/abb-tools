using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Runtime.InteropServices;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPCClient
    {
        /********************************************************
         ***  WINDOWS IPC CLIENT - data
         ********************************************************/

        //background communication thread
        private Thread commThread;
        //control named pipe
        private NamedPipeClientStream pipeStream;
        private StreamReader readMsg;
        private StreamWriter sendMsg;
        private bool closeComm;
        private bool restartComm;
        private bool autoOpen;
        private bool eventsConn;
        private string myStatus;
        private string myServer;
        private string sendBuffor;
        private string recvBuffor;
        private int recvCounter;
        private int sentCounter;
        private string lastMsgSent;
        private string lastMsgRecv;
        //client events
        public event WindowsIPCWaiting OnWaiting;
        public event WindowsIPCConnect OnConnect;
        public event WindowsIPCDisconnect OnDisconnect;
        public event WindowsIPCSent OnSent;
        public event WindowsIPCReceived OnReceived;
        public event WindowsIPCEnd OnEnd;
        [DllImport("kernel32.dll", SetLastError = true)] static extern bool PeekNamedPipe(SafeHandle handle,byte[] buffer, uint nBufferSize, ref uint bytesRead,ref uint bytesAvail, ref uint BytesLeftThisMessage);

        /********************************************************
         ***  WINDOWS IPC CLIENT - public methods
         ********************************************************/

        /// <summary>
        /// Default class constructor
        /// </summary>
        /// <param name="serverName">specify server name (defined in server app)</param>
        /// <param name="restoreConn">true if communication should restore after disconnection</param>
        /// <param name="autoStart">true if communication should start at program start</param>
        public WindowsIPCClient(string serverName, bool restoreConn, bool autoStart)
        {
            //check if there was pipe client before
            if (pipeStream != null) {
                pipeStream.Dispose();
                pipeStream = null;
            }
            //create named pipe client with specified 
            myServer = serverName;
            pipeStream = new NamedPipeClientStream(".", myServer, PipeDirection.InOut);
            //named pipe operations
            closeComm = false;
            restartComm = restoreConn;
            autoOpen = autoStart;
            eventsConn = false;
            //internal data
            myStatus = "not executed yet";
            recvCounter = 0;
            sentCounter = 0;
            lastMsgSent ="";
            lastMsgRecv = "";
            //clear send and receive buffors
            sendBuffor = "";
            recvBuffor = "";
        }

        /// <summary>
        /// Open client background communication thread
        /// </summary>
        public void open()
        {
            //run communication in background thread (if not running)
            if (commThread == null) {
                commThread = new Thread(commLoop);
                commThread.IsBackground = true;
            } else {
                if (commThread.IsAlive) commThread.Abort();
            }
            //set internal data
            closeComm = false;
            //set thread properties and run it
            myStatus = "opened";
            commThread.Start();
        }

        /// <summary>
        /// Close client background communication thread
        /// </summary>
        public void close()
        {
            //cancel background thread
            myStatus = "closed";
            closeComm = true;
        }

        /// <summary>
        /// Check if client is connected to server
        /// </summary>
        /// <returns>true if client is connected</returns>
        public bool isConnected()
        {
            bool result = isServerAlive();
            if (result) result = pipeStream.IsConnected;

            return result;
        }

        /// <summary>
        /// Check if client is restoring connection on non-GUI close
        /// </summary>
        /// <returns>true if client is auto restoring comm</returns>
        public bool isAutoRestart()
        {
            return !closeComm && restartComm;
        }

        /// <summary>
        /// Check if communication is running
        /// </summary>
        /// <returns>true if communication thread is running</returns>
        public bool isRunning()
        {
            return commThread != null && commThread.IsAlive;
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message">Message text to be send</param>
        public void send(string message)
        {
            sendBuffor = message;
        }

        /// <summary>
        /// Get server name
        /// </summary>
        public string serverName
        {
            get { return myServer; }
        }

        /// <summary>
        /// Get auto start property
        /// </summary>
        public bool autoStart
        {
            get { return autoOpen; }
            set { autoOpen = value; }
        }

        /// <summary>
        /// Get auto start property
        /// </summary>
        public bool autoRecon
        {
            get { return restartComm; }
            set { restartComm = value; }
        }

        /// <summary>
        /// Get and set event connected flag
        /// </summary>
        public bool events
        {
            get { return eventsConn; }
            set { eventsConn = value; }
        }

        /// <summary>
        /// Get current client status
        /// </summary>
        public string status
        {
            get { return myStatus; }
        }

        /// <summary>
        /// Get current sent messages number
        /// </summary>
        public int sentNo
        {
            get { return sentCounter; }
        }

        /// <summary>
        /// Get current received messages number
        /// </summary>
        public int recvNo
        {
            get { return recvCounter; }
        }

        /// <summary>
        /// Get last messages sent and received
        /// </summary>
        public string messageReport
        {
            get {
                string lastSent = lastMsgSent != "" ? lastMsgSent : "---";
                string lastRecv = lastMsgRecv != "" ? lastMsgRecv : "---";
                return "SENT: " + lastSent + " RECV: " + lastRecv;
            }
        }

        /********************************************************
         ***  WINDOWS IPC CLIENT - private methods
         ********************************************************/

        /// <summary>
        /// Communication loop (run in background thread)
        /// </summary>
        private void commLoop()
        {
            //internal flags on conenction status
            bool connTry = false, connOk = false;
            //check if user wanted restore connection on server down... 
            while (commCheck(connTry)) {
                //wait for connect to server
                connOk = connect(250);
                connTry = true;
                //communicate while evethingh is OK
                while (commStatus()) {
                    //try to send message
                    if (send()) {
                        //run event if message was sent
                        eventOnSent(new WindowsIPCEventArgs(sendBuffor, myServer, true, isAutoRestart()));
                        //clear send buffer
                        sendBuffor = "";
                    }
                    //try to receive message
                    if (receive()) {
                        //run event if message was received
                        eventOnReceived(new WindowsIPCEventArgs(recvBuffor, myServer, true, isAutoRestart()));
                        //clear receive buffer
                        recvBuffor = "";
                    }
                    //to avoid processor heavy usage
                    Thread.Sleep(100);
                    //to avoid memory leak
                    GC.Collect();
                    //update current status
                    myStatus = "com loop running";
                }
                //at end close client
                commCleanup();
                //run disconnect event (if client was connected)
                if (connOk) {
                    eventOnDisconnect(new WindowsIPCEventArgs("disconnected from server.", myServer, false, isAutoRestart()));
                    myStatus = "disconnected from server";
                }
            }
            //run comm end event
            eventOnCommEnd(new WindowsIPCEventArgs("communication thread end.", myServer, true, isAutoRestart()));
            myStatus = "com loop stopped";
        }

        /// <summary>
        /// Check if start communication
        /// </summary>
        /// <param name="triedToConn">Input true if connection attempt was made</param>
        /// <returns>true if communication should be started/restored</returns>
        private bool commCheck(bool triedToConn)
        {
            bool result = false;
            //check if user closed communication from GUI
            if (closeComm) {
                //close thread as usert wanted
                result = false;
                myStatus = "closed from GUI";
            } else {
                //thread not closed by user... 
                //check if user wanted to reconnect
                if (triedToConn) {
                    result = restartComm;
                    myStatus = "try reconnect";
                } else {
                    //no attempt to connect - try it
                    result = true;
                    myStatus = "try connect";
                }
            }
            return result;
        }

        /// <summary>
        /// Check communication status (server+connection+GUI)
        /// </summary>
        /// <returns>everything is ok</returns>
        private bool commStatus()
        {
            //check if GUI wants to close client
            if (closeComm) {
                return false;
            } 
            //check if server is alive
            if (!isServerAlive()) {
                return false;
            }
            //check if client is connected
            if (!isConnected()) {
                return false;
            }
            //if we are here then everything is ok
            return true;
        }

        /// <summary>
        /// Clean data after communication loop
        /// </summary>
        private void commCleanup()
        {
            //clear buffers
            recvBuffor = "";
            sendBuffor = "";
            //disconnect from pipe
            pipeStream.Close();
            pipeStream.Dispose();
            pipeStream = null;
            //clear thread (if we are quitting it)
            if (!isAutoRestart()){
                commThread = null;
            }
            //update current status
            myStatus = "com cleanup";
        }

        /// <summary>
        /// Connect to server (specified in constructor)
        /// </summary>
        /// <param name="timeout">timeout for check conn status</param>
        /// <returns>true if connection successful</returns>
        private bool connect(int timeout)
        {
            bool result = false;
            bool msgShown = false;
            //clear buffers
            sendBuffor = "";
            recvBuffor = "";
            //check if pipe is existing and opened
            if (pipeStream == null) {
                pipeStream = new NamedPipeClientStream(".", myServer, PipeDirection.InOut);
            }
            //try to connect to server
            while (!pipeStream.IsConnected) {
                try {
                    pipeStream.Connect(timeout);
                    //passed connect method = connected!
                    result = true;
                } catch {
                    //timeout triggered = not connected
                    result = false;
                    //check communication status
                    if (closeComm) {
                        //client closed from GUI
                        myStatus = "closed from GUI";
                        break;
                    } else if (!pipeStream.IsConnected) {
                        //run event if not connected (timeout) 
                        myStatus = "waiting for server";
                        if (!msgShown) {
                            eventOnWait(new WindowsIPCEventArgs("waiting for server...", myServer, false, isAutoRestart()));
                            msgShown = true;
                        }
                    }
                }
                //to avoid processor heavy usage 
                Thread.Sleep(250);
            }
            //run event if client connected
            if (pipeStream.IsConnected) {
                myStatus = "connected to server";
                eventOnConnect(new WindowsIPCEventArgs("connected to server.", myServer, true, isAutoRestart()));
            }
            return result;
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <returns>true if send was successful</returns>
        private bool send()
        {
            bool result = false;
            //check if we are still connected
            if (isConnected() && isServerAlive()) {
                //check if there is some message to send
                if (sendBuffor != "") {
                    sendMsg = new StreamWriter(pipeStream);
                    sendMsg.Write(sendBuffor);
                    sendMsg.Flush();
                    //all ok
                    result = true;
                    //update internal data
                    sentCounter++;
                    lastMsgSent = sendBuffor;
                    myStatus = "sent command: " + lastMsgSent;
                }
            }
            return result;
        }

        /// <summary>
        /// Receive message from server
        /// </summary>
        /// <returns>true if message received successfully</returns>
        private bool receive()
        {
            bool result = false;
            //check if we are still connected
            if (isConnected() && isServerAlive()) {
                string message = "", temp = "";
                int recvLen = 0;
                //check if there is some message to recv
                if (recvAvailable()) {
                    //create reader
                    readMsg = new StreamReader(pipeStream);
                    while (readMsg.Peek() >= 0) {
                        char curr = (char)readMsg.Read();
                        if (curr > 0) temp += curr;
                    }
                    message = temp.Substring(recvLen);
                    recvLen += message.Length;
                    if (message!="") {
                        recvBuffor = message;
                        result = true;
                        //update internal data
                        recvCounter++;
                        lastMsgRecv = message;
                        myStatus = "recv command: " + lastMsgRecv;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Check if message is available for receiving
        /// </summary>
        /// <returns>true if message is available</returns>
        private bool recvAvailable()
        {
            bool result = false;
            byte[] aPeekBuffer = new byte[1];
            uint bytesRead = 0, bytesAvail = 0, bytesLeft = 0;

            //StreamReader.peek() function BLOCKS thread (damn...)
            //using kernel32 PeekNamedPipe function does the job just fine
            result = PeekNamedPipe(pipeStream.SafePipeHandle, aPeekBuffer, 1, ref bytesRead, ref bytesAvail, ref bytesLeft);

            return result && aPeekBuffer[0] != 0;
        }

        /// <summary>
        /// Check if server is opened
        /// </summary>
        /// <returns>true if server is alive and ready</returns>
        private bool isServerAlive()
        {
            bool result = false;
            uint bytesRead = 0, bytesAvail = 0, bytesLeft = 0;

            //if server is not alive then result equals false
            result = PeekNamedPipe(pipeStream.SafePipeHandle, new byte[1], 1, ref bytesRead, ref bytesAvail, ref bytesLeft);

            return result;
        }

        /********************************************************
         ***  WINDOWS IPC CLIENT - events
         ********************************************************/

        /// <summary>
        /// Event triggered on waiting for server
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnWait(WindowsIPCEventArgs e)
        {
            // SIMPLIFIED { if (OnWaiting != null) OnWaiting(this, e); }
            OnWaiting?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered on successful connect to server
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnConnect(WindowsIPCEventArgs e)
        {
            // SIMPLIFIED { if (OnConnect != null) OnConnect(this, e); }
            OnConnect?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered on successful disconnect to server
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnDisconnect(WindowsIPCEventArgs e)
        {
            // SIMPLIFIED { if (OnDisconnect != null) OnDisconnect(this, e); }
            OnDisconnect?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered on message sent to server
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnSent(WindowsIPCEventArgs e)
        {
            // SIMPLIFIED { if (OnSent != null) OnSent(this, e); }
            OnSent?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered on message received from server
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnReceived(WindowsIPCEventArgs e)
        {
            // SIMPLIFIED { if (OnReceived != null) OnReceived(this, e); }
            OnReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered on communication thread ended
        /// </summary>
        /// <param name="e">event arguments</param>
        protected void eventOnCommEnd(WindowsIPCEventArgs e)
        {
            // SIMPLIFIED { if (OnDisconnect != null) OnDisconnect(this, e); }
            OnEnd?.Invoke(this, e);
        }
    }
}
