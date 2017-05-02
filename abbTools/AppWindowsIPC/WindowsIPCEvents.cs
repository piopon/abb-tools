using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abbTools.AppWindowsIPC
{
    public class WindowsIPCEventArgs : EventArgs
    {
        /********************************************************
         ***  WINDOWS IPC EVENT ARGS - data 
         ********************************************************/

        /// <summary>
        /// class field / property - event message
        /// </summary>
        public string message { get; }

        /// <summary>
        /// class field / property - server name
        /// </summary>
        public string server { get; }

        /// <summary>
        /// class field / property - client connected
        /// </summary>
        public bool connected { get; }

        /// <summary>
        /// class field / property - auto restore communication
        /// </summary>
        public bool restoreComm { get; }

        /********************************************************
         ***  WINDOWS IPC EVENT ARGS - data 
         ********************************************************/

        /// <summary>
        /// Constructor with filling all class fields
        /// </summary>
        /// <param name="msg">Event message string</param>
        /// <param name="serverName">Server name</param>
        /// <param name="clientConn">Is client connected to server</param>
        /// <param name="autoRestart">If communication will be restarted</param>
        public WindowsIPCEventArgs(string msg, string serverName, bool clientConn, bool autoRestart)
        {
            message = msg;
            server = serverName;
            connected = clientConn;
            restoreComm = autoRestart;
        }
    }

    /********************************************************
     ***  GLOBAL - WindowsIPC delegates / events
     ********************************************************/
    public delegate void WindowsIPCWaiting(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCConnect(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCSent(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCReceived(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCDisconnect(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCEnd(object sender, WindowsIPCEventArgs e);
}
