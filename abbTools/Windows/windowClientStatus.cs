using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace abbTools.AppWindowsIPC
{
    internal partial class windowClientStatus : Form
    {
        private WindowsIPCClient testClient;
        private WindowsIPCCollection clientCollection;
        private Form overrideParent;
        //logic variables
        private bool clientsAvailable = false;

        public windowClientStatus(WindowsIPCClient cTestClient, WindowsIPCCollection cCollection)
        {
            testClient = cTestClient;
            clientCollection = cCollection;
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //set the panel position
            panelFormTemplate.Left = (Width - panelFormTemplate.Width) / 2;
            panelFormTemplate.Top = (Height - panelFormTemplate.Height) / 2;
            //we want semi-transparent background and opaque controls - we override it with new form
            overrideParent = new Form();
            overrideParent.ShowInTaskbar = false;
            overrideParent.FormBorderStyle = FormBorderStyle.None;
            overrideParent.Opacity = 100;
            overrideParent.Size = panelFormTemplate.Size;
            overrideParent.StartPosition = FormStartPosition.Manual;
            overrideParent.Location = panelFormTemplate.PointToScreen(new Point(0, 0));
            overrideParent.Show(this);
            //new form contains user GUI panel
            panelVisual.Parent = overrideParent;
            panelVisual.Dock = DockStyle.Fill;
            //fill list with clients
            fillClientList();
        }

        private void fillClientList()
        {
            //first clear debug GUI elements
            listViewClients.Items.Clear();
            //add test client 
            if (testClient != null) {
                ListViewItem guiClient = new ListViewItem(testClient.server + "  [GUI]");
                guiClient.ImageIndex = testClient.running ? 1 : 0;
                listViewClients.Items.Add(guiClient);
            }
            //add all items from collection
            foreach (WindowsIPC item in clientCollection) {
                ListViewItem currClient = new ListViewItem(item.ipcClient.server + "  ["+item.controllerStoredName+"]");
                currClient.ImageIndex = item.ipcClient.running ? 1 : 0;
                listViewClients.Items.Add(currClient);
            }
            //check if list contains any data...
            clientsAvailable = listViewClients.Items.Count != 0;
            if (!clientsAvailable) {
                ListViewItem noClient = new ListViewItem("no clients....");
                noClient.ImageIndex = 2;
                listViewClients.Items.Add(noClient);
            }
        }

        private void buttonCloseMe_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void listViewClients_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listViewClients.SelectedItems.Count > 0 && clientsAvailable) {
                getClientDetails(listViewClients.SelectedIndices[0]);
                buttonRefresh.Enabled = true;
                panelSelectItemInfo.Visible = false;
                panelClientContent.Visible = true;
            } else {
                buttonRefresh.Enabled = false;
                panelClientContent.Visible = false;
                panelSelectItemInfo.Visible = true;
            }
        }

        private void getClientDetails(int clientNo)
        {
            //check if test client (GUI) is defined
            if (testClient == null) clientNo++;
            //view selected client
            if (clientNo==0) {
                //test client data
                labelValRunning.Text = testClient.running.ToString().ToUpper();
                labelValStatus.Text = testClient.stats.status.ToUpper();
                labelValAutoRecon.Text = testClient.autoRecon.ToString().ToUpper();
                labelValAutoOpen.Text = testClient.autoOpen.ToString().ToUpper();
                labelValMsgRecv.Text = testClient.stats.recvCounter.ToString();
                labelValMsgSent.Text = testClient.stats.sentCounter.ToString();
                labelValMsgExe.Text = "0";
                labelValEvents.Text = testClient.eventsConn.ToString().ToUpper();
                labelValLastMsg.Text = testClient.stats.messageReport;
            } else {
                //collection client data
                labelValRunning.Text = clientCollection[clientNo - 1].ipcClient.running.ToString().ToUpper();
                labelValStatus.Text = clientCollection[clientNo - 1].ipcClient.stats.status.ToUpper();
                labelValAutoRecon.Text = clientCollection[clientNo - 1].ipcClient.autoRecon.ToString().ToUpper();
                labelValAutoOpen.Text = clientCollection[clientNo - 1].ipcClient.autoOpen.ToString().ToUpper();
                labelValMsgRecv.Text = clientCollection[clientNo - 1].ipcClient.stats.recvCounter.ToString();
                labelValMsgSent.Text = clientCollection[clientNo - 1].ipcClient.stats.sentCounter.ToString();
                labelValMsgExe.Text = clientCollection[clientNo - 1].ipcClient.stats.messagesExecuted.ToString();
                labelValEvents.Text = clientCollection[clientNo - 1].ipcClient.eventsConn.ToString().ToUpper();
                labelValLastMsg.Text = clientCollection[clientNo - 1].ipcClient.stats.messageReport;
            }
        }

        private void buttonRefresh_Click(object sender, System.EventArgs e)
        {
            int selectedIndex = listViewClients.SelectedIndices[0];
            if (selectedIndex >= 0) {
                if (selectedIndex == 0 && testClient != null) {
                    listViewClients.Items[selectedIndex].ImageIndex = testClient.running ? 1 : 0;
                } else {
                    if (testClient == null) {
                        listViewClients.Items[selectedIndex].ImageIndex = clientCollection[selectedIndex].ipcClient.running ? 1 : 0;
                    } else {
                        listViewClients.Items[selectedIndex].ImageIndex = clientCollection[selectedIndex - 1].ipcClient.running ? 1 : 0;
                    }
                }
                getClientDetails(selectedIndex);
            }
            listViewClients.Focus();
        }
    }
}
