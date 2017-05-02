namespace abbTools.AppWindowsIPC
{
    class WindowsIPCStats
    {
        /********************************************************
         ***  WINDOWS IPC STATUS - data 
         ********************************************************/

        /// <summary>
        /// Get current client status
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Get current received messages number
        /// </summary>
        public int recvCounter { get; set; }

        /// <summary>
        /// Get current sent messages number
        /// </summary>
        public int sentCounter { get; set; }

        /// <summary>
        /// Get last sent message
        /// </summary>
        public string lastMsgSent { get; set; }

        /// <summary>
        /// Get last received message
        /// </summary>
        public string lastMsgRecv { get; set; }

        /// <summary>
        /// Get current executed messages counter
        /// </summary>
        public int messagesExecuted { get; set; }

        /********************************************************
         ***  WINDOWS IPC STATUS - constructor 
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowsIPCStats()
        {
            status = "not executed yet";
            recvCounter = 0;
            sentCounter = 0;
            messagesExecuted = 0;
            lastMsgSent = "---";
            lastMsgRecv = "---";
        }

        /********************************************************
         ***  WINDOWS IPC STATUS - functions 
         ********************************************************/

        /// <summary>
        /// Get last messages report
        /// </summary>
        public string messageReport
        {
            get { return $"SENT: { lastMsgSent } RECV: { lastMsgRecv }"; }
        }
    }
}
