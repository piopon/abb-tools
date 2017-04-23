using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace abbTools.AppRemoteABB
{
    class RemoteAction
    {
        //define all available resultants and enums defining them
        private static string[] allActions = { "app execute","app terminate","app restart",
                                               "app mouse clik","app key press" };
        public enum type
        {
            appNull = -1,
            appExe = 0,
            appKill = 1,
            appReset = 2,
            appMouse = 3,
            appKey = 4
        };

        private enum myMouseBtn
        {
            left = 1,
            middle = 2,
            right = 3,
        }

        private type myType;
        private string myModifier;
        private string myProgram;
        private Process myProc;

        //dll methods to operate with simulated mouse and keyboard
        private const int MOUSE_EVENT_LD = 0x02;
        private const int MOUSE_EVENT_LU = 0x04;
        private const int MOUSE_EVENT_RD = 0x08;
        private const int MOUSE_EVENT_RU = 0x10;
        private const int MOUSE_EVENT_MD = 0x20;
        private const int MOUSE_EVENT_MU = 0x40;
        [DllImport("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
                                                                     [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
                                                                     int bufferSize, uint flags);

        public RemoteAction()
        {
            myType = type.appNull;
            myModifier = "";
            myProgram = "";
            myProc = null;
        }

        public RemoteAction(string action, string modifier, string program)
        {
            myType = actionToType(action);
            myModifier = modifier;
            myProgram = program;
            myProc = null;
        }

        public RemoteAction(type action, string modifier, string program)
        {
            myType = action;
            myModifier = modifier;
            myProgram = program;
            myProc = null;
        }

        public RemoteAction(RemoteAction original)
        {
            myType = original.myType;
            myModifier = original.myModifier;
            myProgram = original.myProgram;
        }

        public void modify(string updAct, string updMod, string updApp)
        {
            type newType = actionToType(updAct);
            if (resultant != newType) resultant = newType;
            if (modifier != updMod) modifier = updMod;
            if (appPath != updApp) appPath = updApp;
        }

        public void clear()
        {
            myType = type.appNull;
            myModifier = "";
            myProgram = "";
            myProc = null;
        }

        /// <summary>
        /// Auto implemented property - resultant
        /// </summary>
        public type resultant
        {
            get { return myType; }
            set { myType = value; }
        }

        /// <summary>
        /// Auto implemented property - result modifier 
        /// </summary>
        public string modifier
        {
            get { return myModifier; }
            set { myModifier = value; }
        }

        /// <summary>
        /// Auto implemented property - app path 
        /// </summary>
        public string appPath
        {
            get { return myProgram; }
            set { myProgram = value; }
        }

        /// <summary>
        /// Fill WatchSignal definiton from XML node
        /// </summary>
        /// <param name="xmlNode">node containing info about WatchSignal</param>
        /// <returns>TRUE if load successfull, FALSE otherwise</returns>
        public bool loadFromXML(ref System.Xml.XmlReader xmlNode)
        {
            bool result = false;
            string shortAttr;

            while (result == false && xmlNode.Read()) {
                //only xml element nodes with attributes and "signal_" name are OK
                result = (xmlNode.NodeType == System.Xml.XmlNodeType.Element) && (xmlNode.Name == "action");
                if (result) {
                    if (xmlNode.HasAttributes) {
                        //get result (short attribute and convert it to useful data)
                        shortAttr = xmlNode.GetAttribute("res");
                        myType = (type)short.Parse(shortAttr);
                        //get modifier (short attribute and convert it to useful data)
                        shortAttr = xmlNode.GetAttribute("mod");
                        myModifier = shortAttr == "-" ? "" : shortAttr;
                        //get app dir
                        myProgram = xmlNode.GetAttribute("app");
                    } else {
                        myType = type.appNull;
                        myModifier = "";
                        myProgram = "";
                    }
                }
            }
            return result;
        }

        public void saveToXML(ref System.Xml.XmlWriter xmlNode)
        {
            string saveAttr;
            //write start element
            xmlNode.WriteStartElement("action");
            //signal resultant (action to do)
            saveAttr = ((int)myType).ToString();
            xmlNode.WriteAttributeString("res", saveAttr);
            //resultant modifier
            saveAttr = myModifier.Length == 0 ? "-" : myModifier;
            xmlNode.WriteAttributeString("mod", saveAttr);
            //application path
            xmlNode.WriteAttributeString("app", myProgram);
            //finish element
            xmlNode.WriteEndElement();
        }

        /// <summary>
        /// Method filling selected ListViewItem control
        /// </summary>
        /// <param name="fill">ListViewItem to fill with data</param>
        public void fillWinFormControl(ref ListViewItem fill)
        {
            fill.SubItems.Add(allActions[(int)myType]);
            fill.SubItems.Add(myProgram);
            fill.ToolTipText = myModifier.Length == 0 ? "no actor modifier" : "actor modifier: " + myModifier;
        }

        public bool executeAction()
        {
            bool result = true;
            //new process variable to check running processes
            if (myProc == null) myProc = new Process();
            bool appRun = isAppRunning();

            //check selected action
            switch (myType)
            {
                case type.appExe:
                    //execute program only if app isnt already running
                    if (!appRun) myProc.Start();
                    break;
                case type.appKill:
                    //only running program can be terminated
                    if (appRun) myProc.Kill();
                    break;
                case type.appReset:
                    //if app is running then we have to kill it first
                    if (appRun) myProc.Kill();
                    myProc.StartInfo.FileName = myProgram;
                    myProc.Start();
                    break;
                case RemoteAction.type.appMouse:
                    //check if desired application is running
                    if (!appRun) myProc.Start();
                    //show application in foreground
                    SetForegroundWindow(myProc.MainWindowHandle);
                    //give time to repaint selected application in foreground
                    System.Threading.Thread.Sleep(100);
                    //do simulated mouse actions
                    remoteMouseActions(myModifier);
                    break;
                case RemoteAction.type.appKey:
                    //check if desired application is running
                    if (!appRun) myProc.Start();
                    //show application in foreground
                    SetForegroundWindow(myProc.MainWindowHandle);
                    //give time to repaint selected application in foreground
                    System.Threading.Thread.Sleep(100);
                    //application should be running - simulate mouse
                    remoteKeyboardActions(myModifier);
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        private bool isAppRunning()
        {
            bool result = false;
            //find limit chars
            int procBeg = myProgram.LastIndexOf("//") > 0 ? myProgram.LastIndexOf("//") : myProgram.LastIndexOf("\\"),
                procEnd = myProgram.IndexOf(".exe"),
                pathLen = myProgram.Length;

            //first get only the name of out app (process name)
            if ((procBeg >= 0 && procBeg < pathLen) || (procEnd >= 0 && procEnd < pathLen))
            {
                string procName = myProgram.Substring(procBeg + 1, procEnd - procBeg - 1);
                //check if process is running
                Process[] running = Process.GetProcessesByName(procName);
                if (running.Length != 0)
                {
                    //program is running
                    result = true;
                    //get current app
                    myProc = running[0];
                }
                else
                {
                    //program is not running
                    result = false;
                    //combine process with program
                    myProc.StartInfo.FileName = myProgram;
                }
            }

            return result;
        }

        private void remoteMouseActions(string actions)
        {
            //sim mouse if there are some actions to do
            if (actions.Length > 0)
            {
                int startPos = 0, commaPos = 0;
                string currAction = "";

                //while there are actions to do
                while (commaPos != actions.Length)
                {
                    //find button identifier and comma(s)
                    startPos = actions.IndexOf('[', commaPos);
                    commaPos = actions.IndexOf(',', startPos + 1);
                    //if no start sign was found then end loop
                    if (startPos == -1) break;
                    //if no comma was found then this is the last command
                    if (commaPos == -1) commaPos = actions.Length;
                    //get action string
                    currAction = actions.Substring(startPos, commaPos - startPos);
                    //do current action
                    mouseActionExecute(mouseActionInterpret(currAction));
                }
            }
        }

        private int[] mouseActionInterpret(string action)
        {
            int[] corrdsDiv = { action.IndexOf('['), action.IndexOf(';'), action.IndexOf(']') };
            string curr;
            //get coordinates from inputted action string
            curr = action.Substring(corrdsDiv[0] + 1, corrdsDiv[1] - corrdsDiv[0] - 1);
            int coordX = short.Parse(curr);
            curr = action.Substring(corrdsDiv[1] + 1, corrdsDiv[2] - corrdsDiv[1] - 1);
            int coordY = short.Parse(curr);
            //get button ID from inpputed action string
            curr = action.Substring(corrdsDiv[2] + 1, 1);
            int btn = -1;
            switch (curr)
            {
                case "L":
                    btn = (int)myMouseBtn.left;
                    break;
                case "M":
                    btn = (int)myMouseBtn.middle;
                    break;
                case "R":
                    btn = (int)myMouseBtn.right;
                    break;
                default:
                    break;
            }
            //fill result
            int[] result = { coordX, coordY, btn };

            return result;
        }

        private void mouseActionExecute(int[] details)
        {
            //check input data (only positive integers are ok
            if (details[0] > 0 && details[1] > 0 && details[2] > 0)
            {
                int buttonActionDown = 0x00,
                    buttonActionUp = 0x00;
                //convert int button to dlls accepted format
                switch (details[2])
                {
                    case (int)myMouseBtn.left:
                        buttonActionDown = MOUSE_EVENT_LD;
                        buttonActionUp = MOUSE_EVENT_LU;
                        break;
                    case (int)myMouseBtn.middle:
                        buttonActionDown = MOUSE_EVENT_MD;
                        buttonActionUp = MOUSE_EVENT_MU;
                        break;
                    case (int)myMouseBtn.right:
                        buttonActionDown = MOUSE_EVENT_RD;
                        buttonActionUp = MOUSE_EVENT_RU;
                        break;
                    default:
                        break;
                }
                //move mouse cursor to defined position
                SetCursorPos(details[0], details[1]);
                mouse_event(buttonActionDown, details[0], details[1], 0, 0);
                System.Threading.Thread.Sleep(10);
                mouse_event(buttonActionUp, details[0], details[1], 0, 0);
            }
        }

        private void remoteKeyboardActions(string actions)
        {
            //sim keyboard as log as there are some actions to do
            if (actions.Length > 0)
            {
                int startPos = 0, commaPos = 0;
                string currAction = "";

                //while there are actions to do
                while (commaPos != actions.Length)
                {
                    //find button identifier and comma(s)
                    commaPos = actions.IndexOf(',', startPos);
                    //if no comma was found then this is the last command
                    if (commaPos == -1) commaPos = actions.Length;
                    //get action string
                    currAction = actions.Substring(startPos, commaPos - startPos);
                    //do current action
                    keyboardActionExecute(keyboardActionInterpret(currAction));
                    //start pos is next char after comma
                    if (commaPos != actions.Length) startPos = commaPos + 1;
                }
            }
        }

        private string keyboardActionInterpret(string action)
        {
            string result = "";
            //to use function ToUnicode
            Keys myKeyCode = (Keys)Enum.Parse(typeof(Keys), action, true);
            var kState = new byte[256];
            var buffer = new StringBuilder(256);

            int converted = ToUnicode((uint)myKeyCode, 0, kState, buffer, 256, 0);
            //convert keys using 
            if (converted > 0)
            {
                result = buffer.ToString();
            }
            else
            {
                //functional keys cant be converted to unicode (F1, left arrow, etc)
                result = functionKeysToString(action);
            }

            return result;
        }

        private void keyboardActionExecute(string key)
        {
            SendKeys.Send(key);
        }

        private string functionKeysToString(string functionKey)
        {
            string result = "";
            string[] notUsedList = { "ShiftKey", "ControlKey", "LWin", "RWin", "Menu" };
            bool functionNotUsed = false;

            //check if selected function key is not used
            for (int i = 0; i < notUsedList.Length; i++)
            {
                if (functionKey.Contains(notUsedList[i]))
                {
                    functionNotUsed = true;
                    break;
                }
            }
            //output result
            if (functionNotUsed)
            {
                result = "";
            }
            else
            {
                if (functionKey == "Capital")
                {
                    result = "{CAPSLOCK}";
                }
                else if (functionKey == "PageUp")
                {
                    result = "{PGUP}";
                }
                else if (functionKey == "PageDown" || functionKey == "Next")
                {
                    result = "{PGDN}";
                }
                else
                {
                    result = "{" + functionKey + "}";
                }
            }

            return result;
        }

        private type actionToType(string actionStr)
        {
            type result = type.appNull;
            //check if input is delivered by full name or by index
            if (actionStr.Length > 1) {
                for (short i = 0; i < allActions.Length; i++) {
                    if (allActions[i] == actionStr) {
                        result = (type)i;
                        break;
                    }
                }
            } else {
                result = (type)short.Parse(actionStr);
            }
            return result;
        }

        public static string[] actionsList()
        {
            return allActions;
        }

        public static int actionsGetIndex(string action)
        {
            int result = -1;
            for (int itemNo = 0; itemNo < allActions.Length; itemNo++) {
                if (action == allActions[itemNo]) {
                    result = itemNo;
                    break;
                }
            }
            return result;
        }

        public static int actionsCount()
        {
            return allActions.Length;
        }
    }
}
