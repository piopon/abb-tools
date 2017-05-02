using System;
using System.Threading;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPCClient
    {
        /********************************************************
         ***  WINDOWS IPC CLIENT - data
         ********************************************************/

        /// <summary>
        /// GET or SET auto open communication property 
        /// </summary>
        public bool autoOpen { get; set; }

        /// <summary>
        /// GET or SET event connected flag
        /// </summary>
        public bool eventsConn { get; set; }

        /// <summary>
        /// GET or SET auto reconnect communication property 
        /// </summary>
        public bool autoRecon
        {
            get { return myCommunictaion.autoRecon; }
            set { myCommunictaion.autoRecon = value; }
        }

        /// <summary>
        /// GET server name property
        /// </summary>
        public string server
        {
            get { return myCommunictaion.server; }
        }

        /// <summary>
        /// GET communiaction running status
        /// </summary>
        public bool running
        {
            get { return myCommunictaion.isRunning(); }
        }

        /// <summary>
        /// GET statistics data property
        /// </summary>
        public WindowsIPCStats stats
        {
            get { return myCommunictaion.stats; }
        }

        //background communication thread
        private WindowsIPCComm myCommunictaion;

        //client events
        public event WindowsIPCWaiting OnWaiting;
        public event WindowsIPCConnect OnConnect;
        public event WindowsIPCDisconnect OnDisconnect;
        public event WindowsIPCSent OnSent;
        public event WindowsIPCReceived OnReceived;
        public event WindowsIPCEnd OnEnd;

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
            myCommunictaion = new WindowsIPCComm(serverName, restoreConn);
            //named pipe operations
            autoOpen = autoStart;
            eventsConn = false;
        }

        /// <summary>
        /// Open client background communication thread
        /// </summary>
        public void open()
        {
            //run communication in background thread (if not running)
            myCommunictaion.runBackgroundComm(commLoop);
        }

        /// <summary>
        /// Close client background communication thread
        /// </summary>
        public void close()
        {
            //cancel background thread
            myCommunictaion.stats.status = "closed";
            myCommunictaion.closeComm = true;
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message">Message text to be send</param>
        public void send(string message)
        {
            myCommunictaion.sendQueue = message;
        }

        /********************************************************
         ***  WINDOWS IPC CLIENT - private methods
         ********************************************************/

        /// <summary>
        /// Communication loop (to run in background thread)
        /// </summary>
        private void commLoop()
        {
            //internal flags on conenction status
            bool connTry = false, connOk = false;
            //check if user wanted restore connection on server down... 
            while (check(connTry)) {
                //wait for connect to server
                connOk = connect(250);
                connTry = true;
                //communicate while evethingh is OK
                while (status()) {
                    //try to send message
                    send();
                    //try to receive message
                    receive();
                    //clean loop data
                    clean();
                }
                //at end close client
                cleanup(connOk);
            }
            //end communication
            end();
        }

        /// <summary>
        /// Check if start communication
        /// </summary>
        /// <param name="triedToConn">Input true if connection attempt was made</param>
        /// <returns>TRUE if communication should be started/restored</returns>
        private bool check(bool triedToConn)
        {
            bool result = false;
            //check if user closed communication from GUI
            if (myCommunictaion.closeComm) {
                //close thread as usert wanted
                result = false;
                myCommunictaion.stats.status = "closed from GUI";
            } else {
                //thread not closed by user... 
                //check if user wanted to reconnect
                if (triedToConn) {
                    result = autoRecon;
                    myCommunictaion.stats.status = "try reconnect";
                } else {
                    //no attempt to connect - try it
                    result = true;
                    myCommunictaion.stats.status = "try connect";
                }
            }
            return result;
        }

        /// <summary>
        /// Check communication status (server+connection+GUI)
        /// </summary>
        /// <returns>TRUE if communication is ok, FALSE otherwise</returns>
        private bool status()
        {
            //check if GUI wants to close client
            if (myCommunictaion.closeComm) return false;
            //check if server is alive
            if (!myCommunictaion.isServerAlive()) return false;
            //check if client is connected
            if (!myCommunictaion.isConnected()) return false;
            //if we are here then everything is ok
            return true;
        }

        /// <summary>
        /// Connect to server (specified in constructor)
        /// </summary>
        /// <param name="timeout">timeout for check conn status</param>
        /// <returns>TRUE if connection was successful, FALSE otherwise</returns>
        private bool connect(int timeout)
        {
            bool result = false;
            bool msgShown = false;

            //initialize data
            myCommunictaion.init();
            //try to connect to server
            while (!myCommunictaion.isConnected()) {
                try {
                    myCommunictaion.pipeStream.Connect(timeout);
                    //passed connect method = connected!
                    result = true;
                } catch {
                    //timeout triggered = not connected
                    result = false;
                    //check communication status
                    if (myCommunictaion.closeComm) {
                        //client closed from GUI
                        myCommunictaion.stats.status = "closed from GUI";
                        break;
                    } else if (!myCommunictaion.isConnected()) {
                        //run event if not connected (timeout) 
                        myCommunictaion.stats.status = "waiting for server";
                        if (!msgShown) {
                            eventOnWait(new WindowsIPCEventArgs("waiting for server...", server, false, myCommunictaion.isAutoRestart()));
                            msgShown = true;
                        }
                    }
                }
                //to avoid processor heavy usage 
                Thread.Sleep(250);
            }
            //run event if client connected
            if (myCommunictaion.isConnected()) {
                myCommunictaion.stats.status = "connected to server";
                eventOnConnect(new WindowsIPCEventArgs("connected to server.", server, true, myCommunictaion.isAutoRestart()));
            }
            return result;
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        private void send()
        {
            //check if we are still connected
            if (myCommunictaion.isConnected() && myCommunictaion.isServerAlive()) {
                //check if there is some message to send
                if (myCommunictaion.sendQueue != "") {
                    myCommunictaion.sendMessage();
                    //trigger event
                    eventOnSent(new WindowsIPCEventArgs(myCommunictaion.sendQueue, server, true, myCommunictaion.isAutoRestart()));
                    //clear send buffer
                    myCommunictaion.sendQueue = "";
                }
            }
        }

        /// <summary>
        /// Receive message from server
        /// </summary>
        private void receive()
        {
            //check if we are still connected
            if (myCommunictaion.isConnected() && myCommunictaion.isServerAlive()) {
                //check if there is some message to recv
                if (myCommunictaion.recvAvailable()) {
                    //create reader
                    if (myCommunictaion.recvMessage()) {
                        //run event if message was received
                        eventOnReceived(new WindowsIPCEventArgs(myCommunictaion.recvQueue, server, true, myCommunictaion.isAutoRestart()));
                        //clear receive buffer
                        myCommunictaion.recvQueue = "";
                    }
                }
            }
        }

        /// <summary>
        /// Method used to clean every loop data (garbage collector)
        /// </summary>
        private void clean()
        { 
            //to avoid processor heavy usage
            Thread.Sleep(100);
            //to avoid memory leak
            GC.Collect();
            //update current status
            myCommunictaion.stats.status = "com loop running";
        }

        /// <summary>
        /// Method used at server disconnection or lost in network
        /// </summary>
        /// <param name="wasConn">Input if client was connected before cleanup</param>
        private void cleanup(bool wasConn)
        {
            myCommunictaion.commCleanup();
            //run disconnect event (if client was connected)
            if (wasConn) {
                eventOnDisconnect(new WindowsIPCEventArgs("disconnected from server.", server, false, myCommunictaion.isAutoRestart()));
                myCommunictaion.stats.status = "disconnected from server";
            }
        }

        /// <summary>
        /// Method used at end of communication lopp
        /// </summary>
        private void end()
        {
            //run comm end event
            eventOnCommEnd(new WindowsIPCEventArgs("communication thread end.", server, true, myCommunictaion.isAutoRestart()));
            myCommunictaion.stats.status = "com loop stopped";
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
