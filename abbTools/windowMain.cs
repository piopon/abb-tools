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
            if (isDragged)
            {
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
            loadMyRobots();
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
                    if (item.Text == controllerInfo.Name) break;
                    foundSavedController++;
                }
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to green
                    currItem = listViewRobots.Items[foundSavedController];
                    currItem.StateImageIndex = 3;
                } else {
                    //controller doesnt exist in saved group - add it
                    currItem = new ListViewItem(controllerInfo.Name);
                    //check if controller is real or virtual
                    if (controllerInfo.IsVirtual) {
                        currItem.Group = listViewRobots.Groups["robotsGroupSim"];
                        currItem.StateImageIndex = 1;
                    } else {
                        currItem.Group = listViewRobots.Groups["robotsGroupNet"];
                        currItem.StateImageIndex = 0;
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
            string itemGroup = listViewRobots.FocusedItem.Group.Header;
            //check which element is selected
            if (itemGroup == "saved") {
                addToSavedToolStripMenuItem.Visible = false;
                removeToolStripMenuItem.Visible = true;
            } else if (itemGroup == "network" || itemGroup == "virtual") {
                addToSavedToolStripMenuItem.Visible = true;
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
                if (eventController.Name == item.Text && item.Group.Header == "saved") break;
                foundSavedController++;
            }
            //check if controller was found or lost
            if (e.Reason == ChangeReasons.New) {
                //found controller - check if its saved
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to green
                    ListViewItem updItem = listViewRobots.Items[foundSavedController];
                    updItem.StateImageIndex = 3;
                } else {
                    //controller is not saved - add to list                    
                    ListViewItem updItem = new ListViewItem(eventController.Name);
                    //check if controller is real or virtual
                    if (eventController.IsVirtual) {
                        updItem.Group = listViewRobots.Groups["robotsGroupSim"];
                        updItem.StateImageIndex = 1;
                    } else {
                        updItem.Group = listViewRobots.Groups["robotsGroupNet"];
                        updItem.StateImageIndex = 0;
                    }
                    updItem.SubItems.Add(eventController.IPAddress.ToString());
                    updItem.Tag = eventController;
                    this.listViewRobots.Items.Add(updItem);
                }
            } else {
                //lost controller - check if its saved
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to green
                    ListViewItem updItem = listViewRobots.Items[foundSavedController];
                    updItem.StateImageIndex = 5;
                } else {
                    //remove from list
                    foreach (ListViewItem item in this.listViewRobots.Items) {
                        if ((ControllerInfo)item.Tag == eventController) {
                            this.listViewRobots.Items.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        private void loadMyRobots()
        {
            //robots loaded before scan = all are disconnected
            foreach (ListViewItem item in this.listViewRobots.Items)
            {
                item.StateImageIndex = 5;
            }
        }
    }
}
