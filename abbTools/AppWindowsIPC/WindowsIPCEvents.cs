using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abbTools.AppWindowsIPC
{
    public class WindowsIPCEventArgs : EventArgs
    {
        private string eventMessage;
        private string serverName;
        private bool clientConnected;
        private bool clientRestoreComm;

        public WindowsIPCEventArgs(string msg, string server, bool connected, bool autoRestart)
        {
            eventMessage = msg;
            serverName = server;
            clientConnected = connected;
            clientRestoreComm = autoRestart;
        }

        public string message
        {
            get { return eventMessage; }
        }

        public string server
        {
            get { return serverName; }
        }

        public bool connected
        {
            get { return clientConnected; }
        }

        public bool restore
        {
            get { return clientRestoreComm; }
        }
    }

    public delegate void WindowsIPCWaiting(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCConnect(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCSent(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCReceived(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCDisconnect(object sender, WindowsIPCEventArgs e);
    public delegate void WindowsIPCEnd(object sender, WindowsIPCEventArgs e);
}
