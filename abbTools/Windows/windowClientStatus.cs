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
                ListViewItem guiClient = new ListViewItem(testClient.serverName + "  [GUI]");
                guiClient.ImageIndex = testClient.isRunning() ? 1 : 0;
                listViewClients.Items.Add(guiClient);
            }
            //add all items from collection
            foreach (WindowsIPC item in clientCollection) {
                ListViewItem currClient = new ListViewItem(item.client.serverName + "  ["+item.controllerName+"]");
                currClient.ImageIndex = item.client.isRunning() ? 1 : 0;
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
                labelValRunning.Text = testClient.isRunning().ToString().ToUpper();
                labelValStatus.Text = testClient.status.ToUpper();
                labelValAutoRecon.Text = testClient.autoRecon.ToString().ToUpper();
                labelValAutoOpen.Text = testClient.autoStart.ToString().ToUpper();
                labelValMsgRecv.Text = testClient.recvNo.ToString();
                labelValMsgSent.Text = testClient.sentNo.ToString();
                labelValMsgExe.Text = "0";
                labelValEvents.Text = testClient.events.ToString().ToUpper();
                labelValLastMsg.Text = testClient.messageReport;
            } else {
                //collection client data
                labelValRunning.Text = clientCollection[clientNo - 1].client.isRunning().ToString().ToUpper();
                labelValStatus.Text = clientCollection[clientNo - 1].client.status.ToUpper();
                labelValAutoRecon.Text = clientCollection[clientNo - 1].client.autoRecon.ToString().ToUpper();
                labelValAutoOpen.Text = clientCollection[clientNo - 1].client.autoStart.ToString().ToUpper();
                labelValMsgRecv.Text = clientCollection[clientNo - 1].client.recvNo.ToString();
                labelValMsgSent.Text = clientCollection[clientNo - 1].client.sentNo.ToString();
                labelValMsgExe.Text = clientCollection[clientNo - 1].messagesExecuted.ToString();
                labelValEvents.Text = clientCollection[clientNo - 1].client.events.ToString().ToUpper();
                labelValLastMsg.Text = clientCollection[clientNo - 1].client.messageReport;
            }
        }

        private void buttonRefresh_Click(object sender, System.EventArgs e)
        {
            int selectedIndex = listViewClients.SelectedIndices[0];
            if (selectedIndex >= 0) {
                if (selectedIndex == 0 && testClient != null) {
                    listViewClients.Items[selectedIndex].ImageIndex = testClient.isRunning() ? 1 : 0;
                } else {
                    if (testClient == null) {
                        listViewClients.Items[selectedIndex].ImageIndex = clientCollection[selectedIndex].client.isRunning() ? 1 : 0;
                    } else {
                        listViewClients.Items[selectedIndex].ImageIndex = clientCollection[selectedIndex - 1].client.isRunning() ? 1 : 0;
                    }
                }
                getClientDetails(selectedIndex);
            }
            listViewClients.Focus();
        }
    }
}
