using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABB.Robotics;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.IOSystemDomain;
using ABB.Robotics.Controllers.RapidDomain;

enum abbFoundType
{
    net = 0,
    netSaveConn = 1,
    netSaveDiscon = 2,
    sim = 3,
    simSaveConn = 4,
    simSaveDisconn = 5
};

namespace abbTools
{
    public partial class windowMain : Form
    {
        //global variables
        public bool isDragged = false;
        private int mouseX, mouseY;
        //abb network scanner
        private NetworkScanner abbScanner = null;
        private NetworkWatcher abbWatcher = null;
        //object representing connected controller
        private Controller abbConn = null;

        public windowMain()
        {
            InitializeComponent();
        }

        private void mainWindow_Resize(object sender, EventArgs e)
        {
            //set the notification icon
            notifyIcon.Icon = this.Icon;
            //check if window is minimized or opened (normal or maximized)
            notifyIcon.Visible = (this.WindowState == FormWindowState.Minimized);
            //show tip and hide app from taskbar if window is minimized
            if (notifyIcon.Visible) this.Hide();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //check which button was pressed
            if (e.Button == MouseButtons.Right) {
                notifyIcon.ContextMenuStrip.Show(Cursor.Position);
            } else {
                //reopen window
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //terminate app
            this.Close();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //reopen window
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //terminate app
            this.Close();
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void menuBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isDragged = false;
        }

        private void menuBar_MouseDown(object sender, MouseEventArgs e)
        {           
            if (e.Button == MouseButtons.Left) {
                isDragged = true;
                mouseX = e.X;
                mouseY = e.Y;
            }
        }

        private void menuBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged) {
                Point curr = new Point();
                curr.X = this.Location.X + (e.X - mouseX);
                curr.Y = this.Location.Y + (e.Y - mouseY);
                //update form position
                this.Location = curr;
            }
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show about window
            windowAbout helpWindow = new windowAbout();
            //no new taskbar element
            helpWindow.ShowInTaskbar = false;
            //position in center of main form
            helpWindow.StartPosition = FormStartPosition.Manual;
            helpWindow.Top = this.Top + (this.Height - helpWindow.Height) / 2;
            helpWindow.Left = this.Left + (this.Width - helpWindow.Width) / 2; ;
            //show form
            helpWindow.ShowDialog();
        }

        private void windowMain_Load(object sender, EventArgs e)
        {
            int foundSavedController = -1;

            //update app run-up time
            statusTextBox.AppendText(DateTime.Now.ToString()+"]");
            //load saved robots to list view
            listViewRobots.Items.Clear();
            loadMyRobots("");
            //scan for abb controllers in network
            this.abbScanner = new NetworkScanner();
            this.abbScanner.Scan();
            ControllerInfoCollection networkControllers = abbScanner.Controllers;
            ListViewItem currItem = null;
            foreach (ControllerInfo controllerInfo in networkControllers) {
                //check if found controler is in saved group
                foundSavedController = 0;
                foreach (ListViewItem item in this.listViewRobots.Items) {
                    if (sameController(item,controllerInfo) && item.Group.Header == "saved") break;
                    foundSavedController++;
                }
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to green
                    currItem = listViewRobots.Items[foundSavedController];
                    currItem.StateImageIndex -= 1;
                } else {
                    //controller doesnt exist in saved group - add it
                    currItem = new ListViewItem(controllerInfo.Name);
                    //check if controller is real or virtual
                    if (controllerInfo.IsVirtual) {
                        currItem.Group = listViewRobots.Groups["robotsGroupSim"];
                        currItem.StateImageIndex = (int)abbFoundType.sim; 
                    } else {
                        currItem.Group = listViewRobots.Groups["robotsGroupNet"];
                        currItem.StateImageIndex = (int)abbFoundType.net;
                    }
                    //add second column value =  IP address 
                    currItem.SubItems.Add(controllerInfo.IPAddress.ToString());
                    //add curent item to list
                    this.listViewRobots.Items.Add(currItem);
                    currItem.Tag = controllerInfo;
                }
            }
            //add network watcher to see if something changes in network
            this.abbWatcher = new NetworkWatcher(abbScanner.Controllers,true);
            this.abbWatcher.Found += abbWatcherFoundEvent;
            this.abbWatcher.Lost += abbWatcherLostEvent;
        }

        private void abbWatcherLostEvent(object sender, NetworkWatcherEventArgs e)
        {
            this.Invoke(new EventHandler<NetworkWatcherEventArgs>(updateRobotList), new Object[] { this, e });
        }

        private void abbWatcherFoundEvent(object sender, NetworkWatcherEventArgs e)
        {
            this.Invoke(new EventHandler<NetworkWatcherEventArgs>(updateRobotList), new Object[] { this, e });
        }

        private void windowMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.abbWatcher.Found -= abbWatcherFoundEvent;
            this.abbWatcher.Lost -= abbWatcherLostEvent;
            //disconnect from controller (if connected)
            if (abbConn != null) {
                abbConn.Logoff();
                abbConn.Dispose();
                abbConn = null;
            }
        }

        private void robotListQMenu_Opening(object sender, CancelEventArgs e)
        {
            if (listViewRobots.SelectedItems.Count > 0){
                /***** CONNECT/DISCONNECT FROM CONTROLLER *****/
                if (abbConn == null) {
                    //no controller connected - disconect menu not visible
                    connectToolStripMenuItem.Visible = true;
                    disconnectToolStripMenuItem.Visible = false;
                } else {
                    //controller which is connected only has disconnect menu
                    string itemName = listViewRobots.FocusedItem.Text;
                    if (itemName == abbConn.SystemName) {
                        connectToolStripMenuItem.Visible = false;
                        disconnectToolStripMenuItem.Visible = true;
                    } else {
                        connectToolStripMenuItem.Visible = true;
                        disconnectToolStripMenuItem.Visible = false;
                    }
                }
                /***** ADD/REMOVE FROM SAVED GROUP *****/
                string itemGroup = listViewRobots.FocusedItem.Group.Header;
                //check which element is selected
                if (itemGroup == "saved") {
                    addToSavedToolStripMenuItem.Visible = false;
                    removeToolStripMenuItem.Visible = true;
                } else if (itemGroup == "network" || itemGroup == "virtual") {
                    addToSavedToolStripMenuItem.Visible = true;
                    removeToolStripMenuItem.Visible = false;
                }
            } else {
                addToSavedToolStripMenuItem.Visible = false;
                removeToolStripMenuItem.Visible = false;
            }
        }

        private void updateRobotList(object sender, NetworkWatcherEventArgs e)
        {
            ControllerInfo eventController = e.Controller;
            //check if event controler is in saved group
            int foundSavedController = 0;
            foreach (ListViewItem item in this.listViewRobots.Items) {
                //if current event controller was saved then break loop
                if (sameController(item,eventController) && item.Group.Header == "saved") break;
                foundSavedController++;
            }
            //check if controller was found or lost
            if (e.Reason == ChangeReasons.New) {
                //found controller - check if its saved
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to green
                    ListViewItem updItem = listViewRobots.Items[foundSavedController];
                    updItem.StateImageIndex -= 1;
                    updItem.Tag = eventController;
                } else {
                    //controller is not saved - add to list                    
                    ListViewItem updItem = new ListViewItem(eventController.Name);
                    //check if controller is real or virtual
                    if (eventController.IsVirtual) {
                        updItem.Group = listViewRobots.Groups["robotsGroupSim"];
                        updItem.StateImageIndex = (int)abbFoundType.sim;
                    } else {
                        updItem.Group = listViewRobots.Groups["robotsGroupNet"];
                        updItem.StateImageIndex = (int)abbFoundType.net;
                    }
                    updItem.SubItems.Add(eventController.IPAddress.ToString());
                    updItem.Tag = eventController;
                    this.listViewRobots.Items.Add(updItem);
                }
            } else {
                //lost controller - check if it is saved
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller lost in saved group - update its icon to red
                    ListViewItem updItem = listViewRobots.Items[foundSavedController];
                    updItem.StateImageIndex += 1;
                    updItem.Tag = null;
                } else {
                    //remove from list (wasnt in saved group)
                    foreach (ListViewItem item in this.listViewRobots.Items) {
                        if ((ControllerInfo)item.Tag == eventController) {
                            this.listViewRobots.Items.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem currItem = listViewRobots.SelectedItems[0];
            //if removed controller exists in network then change it group
            if (currItem.StateImageIndex == (int)abbFoundType.netSaveConn) {
                currItem.Group = listViewRobots.Groups[0];
                currItem.StateImageIndex = (int)abbFoundType.net;
            } else if (currItem.StateImageIndex == (int)abbFoundType.simSaveConn) {
                currItem.Group = listViewRobots.Groups[1];
                currItem.StateImageIndex = (int)abbFoundType.sim;
            } else {
                //robot not found - remove it from list
                listViewRobots.Items.Remove(currItem);
            }
            //sort updated elements
            listViewRobots.Sort();
        }

        private void addToSavedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //get selected item with quick menu
            ListViewItem toSaveItem = listViewRobots.SelectedItems[0];
            //update GUI (move to saved group and change icon to connected!)
            toSaveItem.Group = listViewRobots.Groups[2];
            toSaveItem.StateImageIndex += 1;
            //sort updated elements
            listViewRobots.Sort();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show save dialog
            saveDialog.InitialDirectory = Application.StartupPath;
            saveDialog.ShowDialog();
            //save elements to selected file
            saveMyRobots(saveDialog.FileName);
        }

        private void saveMyRobots(string filePath)
        {
            //check if path is correct
            if (filePath != "") {
                //xml settings to create indents in file
                System.Xml.XmlWriterSettings xmlSettings = new System.Xml.XmlWriterSettings();
                xmlSettings.Indent = true;
                //instance of xmlFile
                System.Xml.XmlWriter xmlFile = System.Xml.XmlWriter.Create(filePath, xmlSettings);
                xmlFile.WriteStartElement("myRobots");
                //save all robots from list (only from saved group)
                for (int i = 0; i < listViewRobots.Items.Count; i++) {
                    if (listViewRobots.Items[i].Group == listViewRobots.Groups[2]) {
                        xmlFile.WriteStartElement("robot_" + i.ToString());
                        xmlFile.WriteAttributeString("name", listViewRobots.Items[i].SubItems[0].Text);
                        xmlFile.WriteAttributeString("IP", listViewRobots.Items[i].SubItems[1].Text);
                        xmlFile.WriteAttributeString("type", listViewRobots.Items[i].StateImageIndex.ToString());
                        xmlFile.WriteEndElement();
                    }
                }
                //close up file
                xmlFile.WriteEndElement();
                xmlFile.Close();
            }
        }

        private void loadMyRobots(string filePath)
        {
            //check if path is correct
            if (filePath != "") {
                //move/delete robots from saved group
                foreach (ListViewItem item in this.listViewRobots.Items) {
                    if (item.Group.Header == "saved") {
                        int currState = item.StateImageIndex;
                        if (currState == (int)abbFoundType.netSaveConn) {
                            //visible robots move to network groups
                            item.Group = listViewRobots.Groups["robotsGroupNet"];
                            item.StateImageIndex = (int)abbFoundType.net;
                        } else if (currState == (int)abbFoundType.simSaveConn) {
                            //visible robots move to virtual groups
                            item.Group = listViewRobots.Groups["robotsGroupSim"];
                            item.StateImageIndex = (int)abbFoundType.sim;
                        } else {
                            //delete non-visible robots
                            this.listViewRobots.Items.Remove(item);
                        }
                    }
                }
                //create instance of xml to read
                System.Xml.XmlReader xmlRead = System.Xml.XmlReader.Create(filePath);
                while (xmlRead.Read()) {
                    //read every node from XML document
                    if ((xmlRead.NodeType == System.Xml.XmlNodeType.Element) && (xmlRead.Name.StartsWith("robot_"))) {
                        if (xmlRead.HasAttributes) {
                            ListViewItem listItem = new ListViewItem(xmlRead.GetAttribute("name"));
                            listItem.Group = listViewRobots.Groups["robotsGroupSaved"];
                            //check if controller is real or virtual
                            bool simController = Int16.Parse(xmlRead.GetAttribute("type")) >= (int)abbFoundType.sim;
                            //check if controller is visible and adjust icon
                            int listController = findController(listItem.Text, simController);
                            if (listController != -1) {
                                //controller visible - delete it from network group and set grenn icon
                                listItem.Tag = listViewRobots.Items[listController].Tag;
                                listViewRobots.Items.RemoveAt(listController);
                                listItem.StateImageIndex = 1 + Convert.ToInt16(simController) * 3;
                            } else {
                                //real controller - set icon
                                listItem.StateImageIndex = 2 + Convert.ToInt16(simController) * 3;
                            }                           
                            //add second column value =  IP address 
                            listItem.SubItems.Add(xmlRead.GetAttribute("IP"));
                            //add curent item to list
                            this.listViewRobots.Items.Add(listItem);
                        }
                    }
                }
                //close up file
                xmlRead.Close();
            }
        }

        private bool sameController(ListViewItem listedRobot, ControllerInfo abbRobot)
        {
            bool sameName = listedRobot.Text == abbRobot.Name;
            bool sameType = abbRobot.IsVirtual && listedRobot.StateImageIndex>=(int)abbFoundType.sim;
            
            return sameName && sameType;
        }

        private bool sameController(ListViewItem listedRobot, string robotName, bool robotSim)
        {
            bool sameName = listedRobot.Text == robotName;
            bool sameType = robotSim && listedRobot.StateImageIndex >= (int)abbFoundType.sim;

            return sameName && sameType;
        }

        private int findController(string robotName, bool robotSim)
        {
            int result = -1, index = 0;

            foreach (ListViewItem item in this.listViewRobots.Items) {
                //match controller names
                if (item.Text == robotName) {
                    bool simCondition = robotSim == true && item.StateImageIndex >= (int)abbFoundType.sim;
                    bool netCondition = robotSim == false && item.StateImageIndex < (int)abbFoundType.sim;
                    if (simCondition || netCondition) {
                        //if robot was found then is no need to search further
                        result = index;
                        break;
                    }
                }
                index++;
            }
            return result;
        }

        private int connectController(ListViewItem listController)
        {
            int result = -1;
            string cName = listController.Text;

            if (listController != null && listController.Tag != null) {
                ControllerInfo controller = (ControllerInfo)listController.Tag;
                if (controller.Availability == Availability.Available) {
                    //first disconnect from previous controller (if connected)
                    if (abbConn != null) {
                        abbConn.Logoff();
                        abbConn.Dispose();
                        abbConn = null;
                    }
                    //show status conn status and process messages 
                    statusTextBox.Text = "controller " + cName + ": connecting...";
                    statusTextBox.Select(11, cName.Length);
                    statusTextBox.SelectionFont = new Font(statusTextBox.Font, FontStyle.Bold);
                    Application.DoEvents();
                    //create controller and log in
                    abbConn = ControllerFactory.CreateFrom(controller);
                    abbConn.Logon(UserInfo.DefaultUser);
                    if (abbConn.Connected) {
                        //output
                        result = 1;
                        //update status
                        statusTextBox.Text = "controller " + cName + ": connected!";
                        statusTextBox.Select(11, cName.Length);
                        statusTextBox.SelectionFont = new Font(statusTextBox.Font, FontStyle.Bold);
                    } 
                } else {
                    //output
                    result = 0;
                    //update status
                    statusTextBox.Text = "controller " + cName + ": not available!";
                    statusTextBox.Select(11, cName.Length);
                    statusTextBox.SelectionFont = new Font(statusTextBox.Font, FontStyle.Bold);
                }
            } else {
                //output
                result = -1;
                //update status
                statusTextBox.Text = "controller " + cName  + ": not visible in network!";
                statusTextBox.Select(11, cName.Length);
                statusTextBox.SelectionFont = new Font(statusTextBox.Font, FontStyle.Bold);
            }

            return result;
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show open dialog
            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.ShowDialog();
            //save elements to selected file
            loadMyRobots(openDialog.FileName);
        }

        private int disconnectController(ListViewItem listController)
        {
            int result = -1;
            string cName = listController.Text;

            if (listController != null && listController.Tag != null) {
                ControllerInfo controller = (ControllerInfo)listController.Tag;
                if (controller.Availability == Availability.Available) {
                    //disconnect from selected controller
                    if (abbConn != null) {
                        abbConn.Logoff();
                        abbConn.Dispose();
                        abbConn = null;
                    }
                    //show status conn status and process messages 
                    statusTextBox.Text = "controller " + cName + ": disconnected!";
                    statusTextBox.Select(11, cName.Length);
                    statusTextBox.SelectionFont = new Font(statusTextBox.Font, FontStyle.Bold);
                }
            }
            return result;
        }


        private void btnRobotListCollapse_Click(object sender, EventArgs e)
        {
            //toggle panel collapse
            appContainer.Panel1Collapsed = !appContainer.Panel1Collapsed;
            //
            if (appContainer.Panel1Collapsed) {
                btnRobotListCollapse.Parent = appContainer.Panel2;
                btnRobotListCollapse.Dock = DockStyle.Left;
                btnRobotListCollapse.Text = ">>>";
                tabActions.Location = new Point(42, 12);
                tabActions.Width -= 40;
            } else {
                btnRobotListCollapse.Parent = appContainer.Panel1;
                btnRobotListCollapse.Dock = DockStyle.Right;
                btnRobotListCollapse.Text = "<<<";
                tabActions.Location = new Point(2, 12);
                tabActions.Width += 40;
            }
        }

        private void listViewRobots_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete) {
                //delete robot only from saved group
                ListViewItem item = listViewRobots.FocusedItem;
                if (item != null && item.Group.Header == "saved") {
                    int currState = item.StateImageIndex;
                    if (currState == (int)abbFoundType.netSaveConn) {
                        //visible robots move to network groups
                        item.Group = listViewRobots.Groups["robotsGroupNet"];
                        item.StateImageIndex = (int)abbFoundType.net;
                    } else if (currState == (int)abbFoundType.simSaveConn) {
                        //visible robots move to virtual groups
                        item.Group = listViewRobots.Groups["robotsGroupSim"];
                        item.StateImageIndex = (int)abbFoundType.sim;
                    } else {
                        //delete non-visible robots
                        this.listViewRobots.Items.Remove(item);
                    }
                }
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //connect to selected controller
            int connStatus = connectController(listViewRobots.FocusedItem);
            if (connStatus == 1) {
                //show connected icon
                SignalCollection signals = abbConn.IOSystem.GetSignals(IOFilterTypes.All);
            }

        }

        private void listViewRobots_DoubleClick(object sender, EventArgs e)
        {
            //connect to selected controller
            int connStatus = connectController(listViewRobots.FocusedItem);
            if (connStatus == 1) {
                //show connected icon
                SignalCollection signals = abbConn.IOSystem.GetSignals(IOFilterTypes.All);
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //disconnect from selected controller
            int disconnStatus = disconnectController(listViewRobots.FocusedItem);
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
