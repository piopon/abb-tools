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

        private void buttonRobotToggle_Click(object sender, EventArgs e)
        {

        }

        private void windowMain_Load(object sender, EventArgs e)
        {
            int foundSavedController = -1;

            //load saved robots to list view
            loadMyRobots("");
            //hide robot panel 

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
        }

        private void robotListQMenu_Opening(object sender, CancelEventArgs e)
        {
            if (listViewRobots.SelectedItems.Count > 0){
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
            //czysc liste elementow
            this.listViewRobots.Items.Clear();
            //check if path is correct
            if (filePath != "") {
                //create instance of xml to read
                System.Xml.XmlReader xmlRead = System.Xml.XmlReader.Create(filePath);
                while (xmlRead.Read()) {
                    //read every node from XML document
                    if ((xmlRead.NodeType == System.Xml.XmlNodeType.Element) && (xmlRead.Name.StartsWith("robot_"))) {
                        if (xmlRead.HasAttributes) {
                            ListViewItem listItem = new ListViewItem(xmlRead.GetAttribute("name"));
                            //check if controller is real or virtual
                            listItem.Group = listViewRobots.Groups["robotsGroupSaved"];
                            int typeIcon = Int16.Parse(xmlRead.GetAttribute("type"));
                            listItem.StateImageIndex = typeIcon >= 3 ? (int)abbFoundType.simSaveDisconn : (int)abbFoundType.simSaveDisconn;
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

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show open dialog
            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.ShowDialog();
            //save elements to selected file
            loadMyRobots(openDialog.FileName);
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
