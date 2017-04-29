using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace abbTools.AppRemoteABB
{
    class RemoteProcess
    {
        /********************************************************
         ***  REMOTE PROCESS - data & fields
         ********************************************************/

        //internal class data
        private Process myProc;
        private RemoteMouse myMouse = null;
        private RemoteKeyboard myKeyboard = null;

        [DllImport("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr handle);

        /********************************************************
         ***  REMOTE PROCESS - constructor
         ********************************************************/
        
        /// <summary>
        /// Default constructor of RemoteProcess class
        /// </summary>
        public RemoteProcess()
        {
            myProc = null;
            myMouse = new RemoteMouse();
            myKeyboard = new RemoteKeyboard();
        }

        /********************************************************
         ***  REMOTE PROCESS - main actions
         ********************************************************/
        
        /// <summary>
        /// Function used to execute action of type defined by arguments
        /// </summary>
        /// <param name="resAction">What action type to perform (appExe, appKill, mouseClick, etc.)</param>
        /// <param name="appPath">On what application to perform selected action</param>
        /// <param name="mod">Modifier which specify mouse or keyboard movements and clicks</param>
        /// <returns>TRUE if all succedded (otherwise FALSE)</returns>
        public bool executeAction(RemoteResultant.type resAction, string appPath, string mod)
        {
            bool result = true;
            //new process variable to check running processes
            if (myProc == null) myProc = new Process();
            bool appRun = isAppRunning(appPath);

            //check selected action
            switch (resAction) {
                case RemoteResultant.type.appExe:
                    //execute program only if app isnt already running
                    if (!appRun) myProc.Start();
                    break;
                case RemoteResultant.type.appKill:
                    //only running program can be terminated
                    if (appRun) myProc.Kill();
                    break;
                case RemoteResultant.type.appReset:
                    //if app is running then we have to kill it first
                    if (appRun) myProc.Kill();
                    myProc.StartInfo.FileName = appPath;
                    myProc.Start();
                    break;
                case RemoteResultant.type.appMouse:
                    //check if desired application is running
                    if (!appRun) myProc.Start();
                    //show application in foreground
                    SetForegroundWindow(myProc.MainWindowHandle);
                    //give time to repaint selected application in foreground
                    System.Threading.Thread.Sleep(100);
                    //do simulated mouse actions
                    myMouse.doActions(mod);
                    break;
                case RemoteResultant.type.appKey:
                    //check if desired application is running
                    if (!appRun) myProc.Start();
                    //show application in foreground
                    SetForegroundWindow(myProc.MainWindowHandle);
                    //give time to repaint selected application in foreground
                    System.Threading.Thread.Sleep(100);
                    //application should be running - simulate mouse
                    myKeyboard.doActions(mod);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Function used to check if selected application is running
        /// </summary>
        /// <param name="appDir">Application to check</param>
        /// <returns>TRUE if app is running, FALSE otherwise</returns>
        private bool isAppRunning(string appDir)
        {
            bool result = false;
            //find limit chars
            int procBeg = appDir.LastIndexOf("//") > 0 ? appDir.LastIndexOf("//") : appDir.LastIndexOf("\\"),
                procEnd = appDir.IndexOf(".exe"),
                pathLen = appDir.Length;

            //first get only the name of out app (process name)
            if ((procBeg >= 0 && procBeg < pathLen) || (procEnd >= 0 && procEnd < pathLen)) {
                string procName = appDir.Substring(procBeg + 1, procEnd - procBeg - 1);
                //check if process is running
                Process[] running = Process.GetProcessesByName(procName);
                if (running.Length != 0) {
                    //program is running
                    result = true;
                    //get current app
                    myProc = running[0];
                } else {
                    //program is not running
                    result = false;
                    //combine process with program
                    myProc.StartInfo.FileName = appDir;
                }
            }
            return result;
        }
    }
}
