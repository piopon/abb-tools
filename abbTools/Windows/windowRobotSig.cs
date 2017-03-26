using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.IOSystemDomain;
using System.Drawing;

namespace abbTools.Windows
{
    public partial class windowRobotSig : Form
    {
        private Controller currAbbController;
        private SignalCollection currAbbSignals;
        public string selectedSignal;
        private Form overrideParent;

        public windowRobotSig(Controller abb)
        {
            InitializeComponent();
            //update abb parent controller object
            currAbbController = abb;
        }

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
        }

        public void updateSignals(Controller abb)
        {
            currAbbController = abb;
            btnUpdateSignals_Click(this, null);
        }

        public void selectSig(string sig)
        {

        }

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

        private void backThread_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //update list if controller is connected
            if (currAbbController != null) {
                if (e.UserState != null) listRobotSignals.Items.Add((Signal)e.UserState);
            }
        }

        private void backThread_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //check if in mean time disconnection occured
            if (currAbbController != null) {
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

        private void btnCloseMe_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void listRobotSignals_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //uncheck other elements
            for (int i = 0; i < listRobotSignals.Items.Count; ++i)
            {
                if (i != e.Index)
                {
                    listRobotSignals.SetItemChecked(i, false);
                }
            }
            //update logic condition for enabling buttons
            selectedSignal = listRobotSignals.Items[e.Index].ToString();
        }
    }
}
