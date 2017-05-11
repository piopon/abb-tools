using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using System.Drawing;

namespace abbTools.Windows
{
    public partial class windowRobotSig : Form
    {
        /********************************************************
         ***  WINDOW ROBOT SIGNALS - fields
         ********************************************************/
        
        /// <summary>
        /// GET or SET selected ABB controller signal name
        /// </summary>
        public string selectedSignal { get; set; }

        /// <summary>
        /// GET info about if user has done some changes
        /// </summary>
        public bool changesMade { get; private set; }

        //private fields
        private Controller currAbbController;
        private SignalCollection currAbbSignals;
        private Form overrideParent;
        private string preSelectedSig;

        /********************************************************
         ***  WINDOW ROBOT SIGNALS - constructors
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public windowRobotSig()
        {
            InitializeComponent();
            changesMade = false;
        }

        /// <summary>
        /// Constructor with ABB controller object init
        /// </summary>
        /// <param name="abb">New ABB controller parent to read signals from</param>
        public windowRobotSig(Controller abb)
        {
            InitializeComponent();
            //update abb parent controller object
            currAbbController = abb;
        }

        /********************************************************
         ***  WINDOW ROBOT SIGNALS - form and list events
         ********************************************************/

        /// <summary>
        /// Method called on template panel draw (used to make semi-transparent background and opaque content!)
        /// </summary>
        /// <param name="sender">Panel object that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void panelTemplate_Paint(object sender, PaintEventArgs e)
        {
            //set the panel position
            panelTemplate.Left = (Width - panelTemplate.Width) / 2;
            panelTemplate.Top = (Height - panelTemplate.Height) / 2;
            //we want semi-transparent background and opaque controls - we override it with new form
            overrideParent = new Form();
            overrideParent.ShowInTaskbar = false;
            overrideParent.FormBorderStyle = FormBorderStyle.None;
            overrideParent.Opacity = 100;
            overrideParent.Size = panelTemplate.Size;
            overrideParent.StartPosition = FormStartPosition.Manual;
            overrideParent.Location = panelTemplate.PointToScreen(new Point(0, 0));
            overrideParent.Show(this);
            //new form contains user GUI panel
            panelContents.Parent = overrideParent;
            panelContents.Dock = DockStyle.Fill;
            //set focus on list (to use keyboard)
            listRobotSignals.Focus();
            changesMade = false;
        }

        /// <summary>
        /// Method triggered by ListBox item check event (uncheck all elements except last selected!)
        /// </summary>
        /// <param name="sender">CheckedListBox object which triggered curr event</param>>
        /// <param name="e">Event arguments</param>
        private void listRobotSignals_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //uncheck other elements
            for (int i = 0; i < listRobotSignals.Items.Count; ++i) {
                if (i != e.Index) {
                    listRobotSignals.SetItemChecked(i, false);
                }
            }
            //update logic condition for enabling buttons
            if (e.NewValue == CheckState.Checked) {
                selectedSignal = listRobotSignals.Items[e.Index].ToString();
            } else {
                selectedSignal = "";
            }
        }

        /// <summary>
        /// Method triggered by ListBox key down event (keyboard key actions!)
        /// </summary>
        /// <param name="sender">CheckedListBox object which triggered curr event</param>
        /// <param name="e">Event arguments</param>
        private void listRobotSignals_KeyDown(object sender, KeyEventArgs e)
        {
            //watch keys only if list is filled (and loading panel not visible)
            if (panelLoading.Visible == false)
            {
                //watch only for ESC and ENTER buttons (keys and alpha chars working internally)
                if (e.KeyCode == Keys.Escape) {
                    //user cancelled
                    btnCancel_Click(sender, e);
                } else if (e.KeyCode == Keys.Return) {
                    //user accepted
                    btnOK_Click(sender, e);
                }
            }
        }

        /// <summary>
        /// Event triggered on robot signals form closed
        /// </summary>
        /// <param name="sender">Form parent object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void windowRobotSig_FormClosed(object sender, FormClosedEventArgs e)
        {
            overrideParent.Controls.Clear();
            overrideParent.Close();
            overrideParent = null;
        }

        /********************************************************
         ***  WINDOW ROBOT SIGNALS - buttons events
         ********************************************************/

        /// <summary>
        /// Method called on button CLEAR click event (clear selection and ucheck signal)
        /// </summary>
        /// <param name="sender">Button object that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnClear_Click(object sender, System.EventArgs e)
        {
            listRobotSignals.ClearSelected();
            if (listRobotSignals.CheckedItems.Count > 0) {
                for (int i = 0; i < listRobotSignals.Items.Count; i++) {
                    listRobotSignals.SetItemChecked(i, false);
                }
                selectedSignal = "";
            }
        }

        /// <summary>
        /// Method called on button OK click event (accept new changes)
        /// </summary>
        /// <param name="sender">Button object that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            //only when list if non empty then some changes could be made
            if (listRobotSignals.Items.Count > 0) {
                object checkedItem = listRobotSignals.CheckedItems.Count > 0 ? listRobotSignals.CheckedItems[0] : null;
                int cIndex = checkedItem != null ? listRobotSignals.Items.IndexOf(checkedItem) : -1;
                selectedSignal = (cIndex >= 0) ? listRobotSignals.Items[cIndex].ToString() : "";
                changesMade = selectedSignal != preSelectedSig;
            }
            Close();
        }

        /// <summary>
        /// Method called on button CANCEL click event (decline all changes)
        /// </summary>
        /// <param name="sender">Button object that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            selectedSignal = preSelectedSig;
            changesMade = false;
            Close();
        }

        /// <summary>
        /// Method called on button UPDATE click event (refresh robot signals)
        /// </summary>
        /// <param name="sender">Button object that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void btnUpdateSignals_Click(object sender, System.EventArgs e)
        {
            if (currAbbController != null) {
                //check if background worker is running
                if (!backThread.IsBusy) {
                    //clear list items
                    listRobotSignals.Items.Clear();
                    //run background thread
                    backThread.RunWorkerAsync(listRobotSignals);
                    //show loading info panel
                    panelLoading.BackColor = Color.DarkOrange;
                    labelLoadSignals.Text = "reading signals...";
                    panelLoading.Visible = true;
                }
            } else {
                panelLoading.BackColor = Color.Red;
                labelLoadSignals.Text = "no controller...";
            }
            //if controller connected or not show panel with info
            listRobotSignals.BackColor = Color.Silver;
            panelLoading.Visible = true;
        }

        /********************************************************
         ***  WINDOW ROBOT SIGNALS - functions
         ********************************************************/

        /// <summary>
        /// Mrthod called to update selected ABB controller signals
        /// </summary>
        /// <param name="abb">ABB controller which signals we want to get</param>
        public void updateSignals(Controller abb)
        {
            currAbbController = abb;
            btnUpdateSignals_Click(this, null);
        }

        /// <summary>
        /// Method used to select and check robot signal represented by list index
        /// </summary>
        /// <param name="index">List index of signal to select and check</param>
        public void selectSig(int index)
        {
            if (index >= 0 && index < listRobotSignals.Items.Count) {
                listRobotSignals.SetItemChecked(index, true);
                listRobotSignals.SelectedIndex = index;
            } else {
                //check if list is ready
                if (listRobotSignals.Items.Count > 0) {
                    //uncheck all elements
                    listRobotSignals.SetItemChecked(0, true);
                    listRobotSignals.SetItemChecked(0, false);
                }
                selectedSignal = "";
            }
        }

        /// <summary>
        /// Method used to select and check robot signal represented by its name
        /// </summary>
        /// <param name="name">Name of signal to select and check in list</param>
        public void selectSig(string name)
        {
            listRobotSignals.ClearSelected();
            preSelectedSig = name;
            if (name != null && name != "") {
                Signal curr = currAbbController.IOSystem.GetSignal(name);
                if (listRobotSignals != null) selectSig(listRobotSignals.Items.IndexOf(curr));
            } else {
                //uncheck all elements
                listRobotSignals.SetItemChecked(0, true);
                listRobotSignals.SetItemChecked(0, false);
            }
        }

        /********************************************************
         ***  WINDOW ROBOT SIGNALS - background thread
         ********************************************************/

        /// <summary>
        /// Method triggerd on backgound thread work start 
        /// </summary>
        /// <param name="sender">Thread which triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void backThread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //get all signals and report to main thread when finished
            currAbbSignals = currAbbController.IOSystem.GetSignals(IOFilterTypes.Digital);
            //update list in process changed event
            foreach (Signal sig in currAbbSignals) {
                if (sig.Type == SignalType.DigitalOutput) {
                    if (currAbbController != null) backThread.ReportProgress(0, sig);
                }
            }
        }

        /// <summary>
        /// Method triggerd on backgound thread work progress changed
        /// </summary>
        /// <param name="sender">Thread which triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list if controller is connected
            if (currAbbController != null) {
                if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
            }
        }

        /// <summary>
        /// Method triggerd on backgound thread work end
        /// </summary>
        /// <param name="sender">Thread which triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (currAbbController != null) {
                //if there is inputed name then select it
                if (preSelectedSig != null) selectSig(preSelectedSig);
                //hide progress bar
                listRobotSignals.BackColor = Color.White;
                panelLoading.Visible = false;
            } else {
                //robot disconnected - clear all its signals
                currAbbSignals.Clear();
                currAbbSignals = null;
                listRobotSignals.Items.Clear();
            }
        }
    }
}
