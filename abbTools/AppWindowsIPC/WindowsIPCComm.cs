using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;

namespace abbTools.AppWindowsIPC
{
    class WindowsIPCComm
    {
        /********************************************************
         ***  WINDOWS IPC DATA - class data and fields
         ********************************************************/

        /// <summary>
        /// GET or SET close comm from GUI property
        /// </summary>
        public bool closeComm { get; set; }

        /// <summary>
        /// GET or SET message to send
        /// </summary>
        public string sendQueue { get; set; }

        /// <summary>
        /// GET or SET message received
        /// </summary>
        public string recvQueue { get; set; }

        /// <summary>
        /// GET IPC naped pipe server name
        /// </summary>
        public string server { get; }

        /// <summary>
        /// GET or SET auto reconnect property
        /// </summary>
        public bool autoRecon { get; set; }
        
        /// <summary>
        /// GET or SET named pipe statistics property
        /// </summary>
        public WindowsIPCStats stats { get; set; }

        //named pipe strem writer and reader
        public NamedPipeClientStream pipeStream;
        private StreamWriter sendMsg;
        private StreamReader readMsg;
        //background communication thread
        private Thread commThread;

        //dll method to see named pipe status
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool PeekNamedPipe(SafeHandle handle, byte[] buffer, uint nBufferSize, ref uint bytesRead, ref uint bytesAvail, ref uint BytesLeftThisMessage);

        /********************************************************
         ***  WINDOWS IPC DATA - constructor
         ********************************************************/

        /// <summary>
        /// Constructor with filling input data
        /// </summary>
        /// <param name="serverName">IPC communication server name</param>
        /// <param name="restoreConn">IPC communication restore connection after server lost</param>
        public WindowsIPCComm(string serverName, bool restoreConn)
        {
            //remember incoming data
            server = serverName;
            autoRecon = restoreConn;
            //reset flags, clear queues
            closeComm = false;
            sendQueue = "";
            recvQueue = "";
            //create named pipe client stream
            if (pipeStream != null) {
                pipeStream.Dispose();
                pipeStream = null;
            }
            pipeStream = new NamedPipeClientStream(".", server, PipeDirection.InOut);
            //create statistics object
            stats = new WindowsIPCStats();
        }

        /********************************************************
         ***  WINDOWS IPC COMM - communication methods
         ********************************************************/

        /// <summary>
        /// Method used to startup inputted comunication loop method in background
        /// </summary>
        /// <param name="startProc">Communication loop method to run in background</param>
        public void runBackgroundComm(ThreadStart startProc)
        {
            if (commThread == null) {
                commThread = new Thread(startProc);
                commThread.IsBackground = true;
            } else {
                if (commThread.IsAlive) commThread.Abort();
            }
            //set internal data
            closeComm = false;
            //set thread properties and run it
            stats.status = "opened";
            commThread.Start();
        }

        /// <summary>
        /// Initialize communnication data
        /// </summary>
        public void init()
        {
            //clear buffers
            sendQueue = "";
            recvQueue = "";
            //check if pipe is existing and opened
            if (pipeStream == null) {
                pipeStream = new NamedPipeClientStream(".", server, PipeDirection.InOut);
            }
        }

        /// <summary>
        /// Check if named pipe communication is opened
        /// </summary>
        /// <returns>true if server is alive and ready</returns>
        public bool isServerAlive()
        {
            bool result = false;
            uint bytesRead = 0, bytesAvail = 0, bytesLeft = 0;

            //if server is not alive then result equals false
            try { 
                result = PeekNamedPipe(pipeStream.SafePipeHandle, new byte[1], 1, ref bytesRead, ref bytesAvail, ref bytesLeft);
            } catch (Exception) {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Clean data after communication loop
        /// </summary>
        public void commCleanup()
        {
            //clear buffers
            recvQueue = "";
            sendQueue = "";
            //disconnect from pipe
            pipeStream.Close();
            pipeStream.Dispose();
            pipeStream = null;
            //clear thread (if we are quitting it)
            if (!isAutoRestart())
            {
                commThread = null;
            }
            //update current status
            stats.status = "com cleanup";
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
            return !closeComm && autoRecon;
        }

        /// <summary>
        /// Check if communication is running
        /// </summary>
        /// <returns>true if communication thread is running</returns>
        public bool isRunning()
        {
            return commThread != null && commThread.IsAlive;
        }

        /********************************************************
         ***  WINDOWS IPC DATA - messaging
         ********************************************************/

        /// <summary>
        /// Method used to send message (from sendQueue)
        /// </summary>
        public void sendMessage()
        {
            sendMsg = new StreamWriter(pipeStream);
            sendMsg.Write(sendQueue);
            sendMsg.Flush();
            //update internal data
            stats.sentCounter++;
            stats.lastMsgSent = sendQueue;
            stats.status = $"sent command: {stats.lastMsgSent}";
        }

        /// <summary>
        /// Function used to receive message (and store it into recvQueue)
        /// </summary>
        /// <returns>TRUE if message was received, FALSE otherwise</returns>
        public bool recvMessage()
        {
            bool result = false;

            string temp = "";
            readMsg = new StreamReader(pipeStream);
            while (readMsg.Peek() >= 0) {
                char curr = (char)readMsg.Read();
                if (curr > 0) temp += curr;
            }
            if (temp.Length > 0) {
                //update output
                recvQueue = temp.Substring(0);
                //update internal data
                stats.recvCounter++;
                stats.lastMsgRecv = recvQueue;
                stats.status = $"recv command: {stats.lastMsgRecv}";
                //all OK
                result = true;
            } else {
                //no message
                recvQueue = "";
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Function used to check if message is available for receiving
        /// </summary>
        /// <returns>TRUE if message is available, FALSE otherwise</returns>
        public bool recvAvailable()
        {
            bool result = false;
            byte[] aPeekBuffer = new byte[1];
            uint bytesRead = 0, bytesAvail = 0, bytesLeft = 0;

            //StreamReader.peek() function BLOCKS thread (damn...)
            //using kernel32 PeekNamedPipe function does the job just fine
            result = PeekNamedPipe(pipeStream.SafePipeHandle, aPeekBuffer, 1, ref bytesRead, ref bytesAvail, ref bytesLeft);

            return result && aPeekBuffer[0] != 0;
        }
    }
}
