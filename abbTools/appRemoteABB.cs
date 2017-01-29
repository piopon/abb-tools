using System;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace abbTools
{
    public partial class appRemoteABB : UserControl
    {
        enum resActions
        {
            appExe=0,
            appKill=1,
            appReset=2,
            appMouse=3,
            appKey=4
        };
        enum myMouseBtn
        {
            left = 1,
            middle = 2,
            right = 3,
        }
        //private class members - controller address and enable buttons logic vars
        private Controller abbController = null;
        private loggerABB abbLogger = null;
        private bool listCondition = false,
                     actorCondition = false,
                     resultCondition = false,
                     pcCondition = false,
                     watchCondition = false;
        //watch signal collection
        SignalCollection watchedSignals = null;
        SignalCollection abbSignals = null;
        //remember checked elements (signals, watch)
        private int selectedSig = -1;
        private int selectedRes = -1;
        private string actorModifiers = "";
        //dll methods to operate with simulated mouse and keyboard
        private const int MOUSE_EVENT_LD = 0x02;
        private const int MOUSE_EVENT_LU = 0x04;
        private const int MOUSE_EVENT_RD = 0x08;
        private const int MOUSE_EVENT_RU = 0x10;
        private const int MOUSE_EVENT_MD = 0x20;
        private const int MOUSE_EVENT_MU = 0x40;
        [DllImport("User32.dll")] static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")] static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        public appRemoteABB()
        {
            InitializeComponent();
            abbController = null;
            abbLogger = null;
            watchedSignals = new SignalCollection();
            abbSignals = new SignalCollection();
        }

        public void appRemoteClear()
        {
            //clear all data (robot and reset GUI)
            desyncController();    
        }

        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
        }

        public void desyncController()
        {
            //reset controller address
            abbController = null;
            //clear all robot signals
            abbSignals.Clear();
            abbSignals = null;
            //unsubscribe events from all signals
            foreach (Signal sig in watchedSignals) {
                sig.Changed -= currSignalChangedEvent;
                watchedSignals.Remove(sig);
            }
            watchedSignals.Clear();
            watchedSignals = null;
            //clear signals list and show info panel
            listRobotSignals.Items.Clear();
            panelLoading.Visible = true;
            panelLoading.BackColor = Color.Red;
            labelLoadSignals.Text = "no controller...";
        }

        public void syncLogger(loggerABB myLogger)
        {
            //update controller logger
            abbLogger = myLogger;
        }

        public void saveAppData(System.Xml.XmlWriter saveXml)
        {
            string attribute;
            //write base app node
            saveXml.WriteStartElement("remotePC");
            //save all robots from list (only from saved group)
            for (int i = 0; i < listActionsWatch.Items.Count; i++)
            {
                //write start element
                saveXml.WriteStartElement("signal_"+i.ToString());
                //signal name
                saveXml.WriteAttributeString("name", listActionsWatch.Items[i].SubItems[0].Text);
                //application path
                attribute = listActionsWatch.Items[i].SubItems[3].Text;
                saveXml.WriteAttributeString("app", attribute);
                //signal trigger (value 0 or 1)
                attribute = listActionsWatch.Items[i].SubItems[1].Text;
                attribute = attribute.Substring(attribute.IndexOf('"')+1, 1);
                saveXml.WriteAttributeString("act", attribute);
                //signal resultant (action to do)
                attribute = listActionsWatch.Items[i].SubItems[2].Text;
                int itemNo = 0;
                foreach (object comboObj in comboResultant.Items){
                    if (comboObj.ToString() == attribute) {
                        attribute = itemNo.ToString();
                        break;
                    }
                    itemNo++;
                }
                saveXml.WriteAttributeString("res", attribute);
                //resultant modifier
                attribute = listActionsWatch.Items[i].ToolTipText;
                attribute = attribute.StartsWith("no") ? "-" : attribute.Substring(16);
                saveXml.WriteAttributeString("mod", attribute);
                //finish element
                saveXml.WriteEndElement();
            }
            saveXml.WriteEndElement();
        }

        public void loadAppData(System.Xml.XmlReader loadXml)
        {
            //clear watch robot signal list

        }

        private void buttonPCAppSel_Click(object sender, EventArgs e)
        {
            //show file selection dialog
            pcAppLocation.ShowDialog();
            labelAppDir.Text = pcAppLocation.FileName;
            //update logic condition for enabling buttons
            pcCondition = labelAppDir.Text.Contains(".exe");
            checkEnableButtons();
        }

        private void listRobotSignals_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //uncheck other elements
            for (int ix = 0; ix < listRobotSignals.Items.Count; ++ix) {
                if (ix != e.Index) {
                    listRobotSignals.SetItemChecked(ix, false);
                } 
            }
            //update logic condition for enabling buttons
            listCondition = e.NewValue == CheckState.Checked;
            selectedSig = e.Index;
            checkEnableButtons();
        }

        private void buttonActionsNew_Click(object sender, EventArgs e)
        {
            //add new item to watch list
            string selRadioText = radioChangeTo1.Checked ? "change to \"1\"" : "change to \"0\"";
            string[] itemTxt = { listRobotSignals.Text, selRadioText, comboResultant.Text, labelAppDir.Text };
            ListViewItem newItem = new ListViewItem(itemTxt);
            newItem.Checked = false;
            newItem.Tag = actorModifiers;
            newItem.ToolTipText = actorModifiers != "" ? "actor modifier: " + actorModifiers : "no actor modifier";
            listActionsWatch.Items.Add(newItem);
            //update GUI
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
            //get signal and set trigger event to it
            Signal currSignal = abbController.IOSystem.GetSignal(listRobotSignals.Text);
            currSignal.Changed += currSignalChangedEvent;
            //add watched signal to collection (need for unsubscribing events)
            watchedSignals.Add(currSignal);
            //log new watch event
            abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: signal "
                                             + currSignal.Name + " subscibtion ON!");
        }

        private void currSignalChangedEvent(object sender, SignalChangedEventArgs e)
        {
            this.Invoke(new EventHandler<SignalChangedEventArgs>(signalWatcher), new Object[] { sender, e });
        }

        private void signalWatcher(object sender, SignalChangedEventArgs e)
        {
            //get triggered signal name nad new value
            Signal caller = (Signal)sender;
            SignalState newValue = e.NewSignalState;
            //get all signal settings
            string[] details = getWatchSignalDetails(caller.Name);
            //check actuator
            if (short.Parse(details[0])==(int)newValue.Value) {
                //do action
                if (executeAction(details[1], details[2], details[3])) {
                    //log triggerred action
                    abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: signal "
                                       + caller.Name + " triggered ( value = " + newValue.Value.ToString()
                                       + ", time = " + DateTime.Now.ToLongTimeString() + " )!");
                }
            }
        }

        private bool executeAction(string actionToDo, string modifier, string pcProgram)
        {
            bool result = true;
            //new process variable to check running processes
            Process proc = new Process();
            bool appRun = isAppRunning(pcProgram,ref proc);

            //check selected action
            resActions action = (resActions)short.Parse(actionToDo);
            switch (action) {
                case resActions.appExe:
                    //execute program only if app isnt already running
                    if (!appRun) proc.Start();
                    break;
                case resActions.appKill:
                    //only running program can be terminated
                    if (appRun) proc.Kill();
                    break;
                case resActions.appReset:
                    //if app is running then we have to kill it first
                    if (appRun) proc.Kill();
                    proc.StartInfo.FileName = pcProgram;
                    proc.Start();
                    break;
                case resActions.appMouse:
                    //check if desired application is running
                    if (!appRun) proc.Start();
                    //show application in foreground
                    SetForegroundWindow(proc.MainWindowHandle);
                    //give time to repaint selected application in foreground
                    System.Threading.Thread.Sleep(100);
                    //do simulated mouse actions
                    remoteMouseActions(modifier);
                    break;
                case resActions.appKey:
                    //check if desired application is running
                    if (!appRun) proc.Start();
                    //show application in foreground
                    SetForegroundWindow(proc.MainWindowHandle);
                    //give time to repaint selected application in foreground
                    System.Threading.Thread.Sleep(100);
                    //application should be running - simulate mouse
                    remoteKeyboardActions(modifier);
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        private bool isAppRunning(string myApp, ref Process myProc)
        {
            bool result = false;
            //find limit chars
            int procBeg = myApp.LastIndexOf("//") > 0 ? myApp.LastIndexOf("//") : myApp.LastIndexOf("\\"),
                procEnd = myApp.IndexOf(".exe"),
                pathLen = myApp.Length;

            //first get only the name of out app (process name)
            if ((procBeg >= 0 && procBeg < pathLen) || (procEnd >= 0 && procEnd < pathLen)) {
                string procName = myApp.Substring(procBeg+1, procEnd - procBeg - 1);
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
                    myProc.StartInfo.FileName = myApp;
                }
            }

            return result;
        }

        private void remoteMouseActions(string actions)
        {
            //sim mouse if there are some actions to do
            if (actions.Length > 0) {
                int startPos = 0, commaPos = 0;
                string currAction = "";

                //while there are actions to do
                while (commaPos != actions.Length) {
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
            curr = action.Substring(corrdsDiv[0]+1, corrdsDiv[1] - corrdsDiv[0] - 1);
            int coordX = short.Parse(curr);
            curr = action.Substring(corrdsDiv[1]+1, corrdsDiv[2] - corrdsDiv[1] - 1);
            int coordY = short.Parse(curr);
            //get button ID from inpputed action string
            curr = action.Substring(corrdsDiv[2]+1, 1);
            int btn = -1;
            switch (curr) {
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
            if (details[0] > 0 && details[1] > 0 && details[2] > 0) {
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
            if (actions.Length > 0) {
                int startPos = 0, commaPos = 0;
                string currAction = "";

                //while there are actions to do
                while (commaPos != actions.Length) {
                    //find button identifier and comma(s)
                    commaPos = actions.IndexOf(',', startPos);
                    //if no comma was found then this is the last command
                    if (commaPos == -1) commaPos = actions.Length;
                    //get action string
                    currAction = actions.Substring(startPos,commaPos-startPos);
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
            if (converted > 0) {
                result = buffer.ToString();
            } else {
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
            string[] notUsedList = {"ShiftKey","ControlKey","LWin","RWin","Menu"};
            bool functionNotUsed = false;

            //check if selected function key is not used
            for (int i = 0; i < notUsedList.Length; i++) {
                if (functionKey.Contains(notUsedList[i])) {
                    functionNotUsed = true;
                    break;
                }
            }
            //output result
            if (functionNotUsed) {
                result = "";
            } else {
                if (functionKey == "Capital") {
                    result = "{CAPSLOCK}";
                } else if (functionKey == "PageUp") {
                        result = "{PGUP}";
                } else if (functionKey == "PageDown" || functionKey == "Next") {
                    result = "{PGDN}";
                } else {
                    result = "{" + functionKey + "}";
                }
            }

            return result;
        }


        private string[] getWatchSignalDetails(string sigName)
        {
            string actuator = "", resultant = "", modifier = "", pcProg = "";

            //scan all elements in watched signals list
            foreach (ListViewItem item in listActionsWatch.Items) {
                //if we found searched element then get its details
                if (item.Text == sigName) {
                    //get actuator details
                    actuator = item.SubItems[1].Text;
                    actuator = actuator.Substring(actuator.IndexOf("\"")+1, 1);
                    //resultant action (get item index)
                    resultant = item.SubItems[2].Text;
                    for(int i=0; i < comboResultant.Items.Count; i++) { 
                        if (comboResultant.Items[i].ToString() == resultant) {
                            resultant = i.ToString();
                            break;
                        }
                    }
                    //resultant modifier
                    modifier = (string)item.Tag;
                    //selected application path
                    pcProg = item.SubItems[3].Text;
                }
            }
            string[] result = { actuator, resultant, modifier, pcProg };
            return result;
        }

        private void buttonActionsRemove_Click(object sender, EventArgs e)
        {
            //remember signal name to unsubscribe event
            string sigName = listActionsWatch.CheckedItems[0].Text;
            //get selected item and remove it
            ListViewItem selectedWatch = listActionsWatch.CheckedItems[0];
            listActionsWatch.Items.Remove(selectedWatch);
            //update GUI
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
            //get signal and delete trigger event to it
            foreach (Signal sig in watchedSignals) {
                if (sig.Name == sigName) {
                    sig.Changed -= currSignalChangedEvent;
                    watchedSignals.Remove(sig);
                    //log new watch event
                    abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName 
                                                     + "</bu>: signal " + sig.Name + " subscibtion OFF!");
                    break;
                }
            }
        }

        private void buttonActionsModify_Click(object sender, EventArgs e)
        {
            //get data for logging purposes
            int watchedIndex = listActionsWatch.CheckedItems[0].Index;
            string oldSigName = listActionsWatch.Items[watchedIndex].Text;
            string newSigName = listRobotSignals.Text;
            //get selected item and update its content
            ListViewItem selectedWatch = listActionsWatch.CheckedItems[0];
            //update selected item
            selectedWatch.SubItems[0].Text = listRobotSignals.Text;
            selectedWatch.SubItems[1].Text = radioChangeTo1.Checked ? "change to \"1\"" : "change to \"0\"";
            selectedWatch.SubItems[2].Text = comboResultant.Text;
            selectedWatch.SubItems[3].Text = labelAppDir.Text;
            //add info about modifier
            selectedWatch.Tag = actorModifiers;
            selectedWatch.ToolTipText = actorModifiers != "" ? "actor modifier: " + actorModifiers : "no actor modifier";
            //we have to change signal in collection (rest will be fetched in event) - remove old one
            Signal modifySig = watchedSignals[watchedIndex];
            modifySig.Changed -= currSignalChangedEvent;           
            watchedSignals.Remove(modifySig);
            //insert new one
            modifySig = abbController.IOSystem.GetSignal(listRobotSignals.Text);
            modifySig.Changed += currSignalChangedEvent;
            watchedSignals.Add(modifySig);
            //log new watch event
            abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName
                                             + "</bu>: subscibtion CHANGED ( " + oldSigName +" >>> "+ newSigName + ")!");
            //update GUI
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        private void backThread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //get all signals and report to main thread when finished
            abbSignals = abbController.IOSystem.GetSignals(IOFilterTypes.Digital);
            //update list in process changed event
            foreach (Signal sig in abbSignals) {
                if (abbController != null) backThread.ReportProgress(0, sig);
            }
        }

        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (abbController != null) {
                //hide progress bar
                listRobotSignals.BackColor = Color.White;
                panelLoading.Visible = false;
                //update info
                abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updated signals!");
            } else {
                //robot disconnected - clear all its signals
                abbSignals.Clear();
                abbSignals = null;
                listRobotSignals.Items.Clear();
            }
        }

        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list if controller is connected
            if (abbController != null) {
                if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
            }
        }

        private void listActionsWatch_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //update logic condition for enabling buttons
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        private void buttonEditModifier_Click(object sender, EventArgs e)
        {
            actorModifiers = showInputWindow(comboResultant.SelectedIndex);
        }

        private void comboResultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update logic condition for enabling buttons
            resultCondition = comboResultant.SelectedIndex != -1;
            checkEnableButtons();
            //show user window to specify details
            if (comboResultant.SelectedIndex>=(int)resActions.appMouse) {
                //show window only if index was changed
                if (comboResultant.SelectedIndex != selectedRes) {
                    actorModifiers = "";
                    actorModifiers = showInputWindow(comboResultant.SelectedIndex);
                }
                //show button to edit inputted modifiers
                buttonEditModifier.Visible = true;
            } else {
                //hide button editing modifiers (and clean modifier itself)
                buttonEditModifier.Visible = false;
                actorModifiers = "";
            }
            //remember this selection
            selectedRes = comboResultant.SelectedIndex;
        }

        /// <summary>
        /// Check if enable buttons conditions are met (all components have selected items/indexes)
        /// </summary>
        private void checkEnableButtons()
        {
            //one of radio buttons will be always checked but lets check it anyway
            actorCondition = radioChangeTo0.Checked || radioChangeTo1.Checked;
            //modify button condition
            bool modifyCondition = listCondition && actorCondition && resultCondition && pcCondition && watchCondition;
            buttonActionsModify.Enabled = modifyCondition;
            buttonActionsModify.BackColor = modifyCondition ? Color.Yellow : Color.Silver;
            //new button condition
            bool newCondition = listCondition && actorCondition && resultCondition && pcCondition;
            buttonActionsNew.Enabled = newCondition;
            buttonActionsNew.BackColor = newCondition ? Color.Chartreuse : Color.Silver;
            //delete button condition
            bool deleteCondition = watchCondition;
            buttonActionsRemove.Enabled = deleteCondition;
            buttonActionsRemove.BackColor = deleteCondition ? Color.Red : Color.Silver;
        }

        private void buttonUpdateSignals_Click(object sender, EventArgs e)
        {
            if (abbController != null) {
                abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updating signals...");
                //clear list items
                listRobotSignals.Items.Clear();
                //run background thread
                backThread.RunWorkerAsync(listRobotSignals);
                //show loading info panel
                panelLoading.BackColor = Color.DarkOrange;
                labelLoadSignals.Text = "reading signals...";
                panelLoading.Visible = true;
            } else {
                abbLogger.writeLog(logType.error, "No controller connected!");
                panelLoading.BackColor = Color.Red;
                labelLoadSignals.Text = "no controller...";
            }
            //if controller connected or not show panel with info
            listRobotSignals.BackColor = Color.Silver;
            panelLoading.Visible = true;
        }


        private string showInputWindow(int resultantSelected)
        {
            string result = "", header, message;
            bool autoKeyboard = false, autoMouse = false;

            //get window title and message
            switch (resultantSelected) {
                case (int)resActions.appMouse:
                    header = "app mouse click resultant";
                    message = "input mouse position and button to be sim-clicked." + Environment.NewLine +
                              "FORMAT: [posX;posY]L/M/R";
                    autoMouse = true;
                    break;
                case (int)resActions.appKey:
                    header = "app keyboard click resultant";
                    message = "input keyboard button to be sim-clicked." + Environment.NewLine +
                              "FORMAT: keyName";
                    autoKeyboard = true;
                    break;
                default:
                    header = "UNKNOWN RESULT ACTION";
                    message = "no message - resultant action is not supported!";
                    break;
            }
            //create new input window object
            windowInput input = new windowInput(header,message,windowInput.type.txt, actorModifiers, ParentForm);
            //no new taskbar element
            input.ShowInTaskbar = false;
            //position in center of main form
            input.StartPosition = FormStartPosition.Manual;
            input.Top = this.ParentForm.Top + (this.ParentForm.Height - input.Height) / 2;
            input.Left = this.ParentForm.Left + (this.ParentForm.Width - input.Width) / 2;
            //check if we want to get quick input (auto-printing data)
            input.autoFillInput(autoKeyboard,autoMouse);
            //show dialog and get result
            if (input.ShowDialog()==DialogResult.OK) {
                result = input.getUserInput();
            }
            //dispose input at end
            input.Dispose();

            return result;
        }
    }
}
