using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
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
        private string actorModifiers = "";

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
            //unsubscribe events from all signals
            foreach (Signal sig in watchedSignals) {
                sig.Changed -= currSignalChangedEvent;
                watchedSignals.Remove(sig);
            }
            watchedSignals.Clear();
            watchedSignals = null;
        }

        public void syncController(Controller myController)
        {
            //update controller address
            abbController = myController;
        }

        public void syncLogger(loggerABB myLogger)
        {
            //update controller logger
            abbLogger = myLogger;
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
            if (Int16.Parse(details[0])==(int)newValue.Value) {
                //do action
                if (executeAction(details[1], details[2])) {
                    //log triggerred action
                    abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: signal "
                                       + caller.Name + " triggered ( value = " + newValue.Value.ToString()
                                       + ", time = " + DateTime.Now.ToLongTimeString() + " )!");
                }
            }
        }

        private bool executeAction(string actionToDo, string pcProgram)
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
                case resActions.appKey:
                    break;
                case resActions.appMouse:
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

        private string[] getWatchSignalDetails(string sigName)
        {
            string actuator = "", resultant = "", pcProg = "";

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
                    //selected application path
                    pcProg = item.SubItems[3].Text;
                }
            }
            string[] result = { actuator, resultant, pcProg };
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
            foreach (Signal sig in abbSignals) backThread.ReportProgress(0, sig);
        }

        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //hide progress bar
            listRobotSignals.BackColor = Color.White;
            panelLoading.Visible = false;
            //update info
            abbLogger.writeLog(logType.info, "controller <bu>" + abbController.SystemName + "</bu>: updated signals!");
        }

        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list
            if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
        }

        private void listActionsWatch_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //update logic condition for enabling buttons
            watchCondition = listActionsWatch.CheckedItems.Count != 0;
            checkEnableButtons();
        }

        private void comboResultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update logic condition for enabling buttons
            resultCondition = comboResultant.SelectedIndex != -1;
            checkEnableButtons();
            //show user window to specify details
            if (comboResultant.SelectedIndex>=(int)resActions.appMouse) {
                actorModifiers = e.ToString();
                //show button to edit inputted modifiers
                buttonEditModifier.Visible = true;
            } else {
                //hide button editing modifiers (and clean modifier itself)
                buttonEditModifier.Visible = false;
                actorModifiers = "";
            }
        }

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
                listRobotSignals.BackColor = Color.Silver;
                panelLoading.Visible = true;
            } else {
                abbLogger.writeLog(logType.info, "No controller connected!");
            }
        }

    }
}
