using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using abbTools.Shared;

namespace abbTools
{
    public partial class windowMain : Form
    {
        /********************************************************
         ***  MAIN WINDOW - properties + fields
         ********************************************************/
        
        /// <summary>
        /// GET window settings data
        /// </summary>
        public windowSettings appSettings { get; private set; }

        /// <summary>
        /// GET object representing currently connected controller
        /// </summary>
        //public Controller abbConnected { get; private set; }

        /// <summary>
        /// GET current logger object
        /// </summary>
        public loggerABB status { get; private set; }

        //applications management object
        private AppsManager myApps;
        //abb network scanner
        private Controller abbConnected = null;
        private NetworkScanner abbScanner = null;
        private NetworkWatcher abbWatcher = null;
        //global variables
        private bool isDragged = false;
        private int mouseX, mouseY;

        /********************************************************
         ***  MAIN WINDOW - constructor
         ********************************************************/

        /// <summary>
        /// Default constructor
        /// </summary>
        public windowMain()
        {
            InitializeComponent();
            //connect events when unhandled exceptions triggers
            connectUnhandledExceptions();
            //load application settings
            appSettings = new windowSettings(Height,Width);
            appSettings.loadData();
            //try to send welcome mail
            sendMail(abbStatus.mail.openApp);
            //register applications and update dashboard
            appsRegister();
            appsUpdateGUI();
            //connect application events
            appsConnectEvents();
        }

        /********************************************************
         ***  MAIN WINDOW - uhandled errors
         ********************************************************/

        /// <summary>
        /// Method used to subscribe to unhandled exceptions events (for both: AppDomain and Application)
        /// </summary>
        private void connectUnhandledExceptions()
        {
            //to catch unhandled exceptions triggered by in the app domain
            AppDomain.CurrentDomain.UnhandledException += abbToolsUnhandledException;
            //to catch unhandled exceptions on background threads
            Application.ThreadException += abbToolsThreadException;
        }

        /// <summary>
        /// Method used to unsubscribe from unhandled exceptions events (for both: AppDomain and Application)
        /// </summary>
        private void disconnectUnhandledExceptions()
        {
            //to catch unhandled exceptions triggered by in the app domain
            AppDomain.CurrentDomain.UnhandledException -= abbToolsUnhandledException;
            //to catch unhandled exceptions on background threads
            Application.ThreadException -= abbToolsThreadException;
        }

        /// <summary>
        /// Event connected to catch unhandled exceptions triggered by app domain
        /// </summary>
        /// <param name="sender">AppDomain object that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void abbToolsThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //send e-mail that app is closed
            sendMail(abbStatus.mail.exception);
            Close();
        }

        /// <summary>
        /// Event connected to catch unhandled exception by any app thread
        /// </summary>
        /// <param name="sender">Application element that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void abbToolsUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //send e-mail that app is closed
            sendMail(abbStatus.mail.exception);
            Close();
        }

        /********************************************************
         ***  MAIN WINDOW - application manager
         ********************************************************/

        /// <summary>
        /// Method used to register all abbTools applications
        /// </summary>
        private void appsRegister()
        {
            myApps = new AppsManager(tabActions);
            myApps.registerApp(appBackupManager, "appBackup", Height, Width);
            myApps.registerApp(appRemotePC, "appPC", Height, Width);
            myApps.registerApp(appWindowsIPC, "appIPC", Height, Width);
        }

        /// <summary>
        /// Method used to update abbTools related GUI 
        /// </summary>
        private void appsUpdateGUI()
        {
            //update dashboard
            Panel dash = myApps.generateDashboard();
            dash.Parent = actionDashboard;
            //reorder tab pages
            myApps.reorderPages();
        }

        /// <summary>
        /// Event triggered at change of page in application tabs control
        /// </summary>
        /// <param name="sender">TabPages that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void tabActions_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = tabActions.CausesValidation;
            if (e.TabPageIndex == 0) tabActions.CausesValidation = true;
        }

        /********************************************************
         ***  MAIN WINDOW - main window events
         ********************************************************/

        /// <summary>
        /// Event triggered on applications main window load (create)
        /// </summary>
        /// <param name="sender">Main app window object</param>
        /// <param name="e">Event arguments</param>
        private void windowMain_Load(object sender, EventArgs e)
        {
            int foundSavedController = -1;

            //update app run-up time
            status = new loggerABB(imagesLogType, pictureLogType, statusTextBox, true);
            status.appendLog("<u>[ start " + DateTime.Now.ToString() + " ]</u>");
            //abbTools opened - do apps open actions
            appsLoadProgramActions();
            //load saved robots to list view
            listViewRobots.Items.Clear();
            if(appSettings.loadLastProject) loadMyRobots(appSettings.currProject);
            //scan for abb controllers in network
            abbScanner = new NetworkScanner();
            abbScanner.Scan();
            ControllerInfoCollection networkControllers = abbScanner.Controllers;
            ListViewItem currItem = null;
            foreach (ControllerInfo controllerInfo in networkControllers) {
                //check if found controler is in saved group
                foundSavedController = 0;
                foreach (ListViewItem item in listViewRobots.Items) {
                    if (sameController(item, controllerInfo) && item.Group.Header == "saved") break;
                    foundSavedController++;
                }
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to blue
                    currItem = listViewRobots.Items[foundSavedController];
                    updateIcon(listViewRobots.Items[foundSavedController], abbStatus.conn.available);
                } else {
                    //controller doesnt exist in saved group - add it
                    currItem = new ListViewItem(controllerInfo.Name);
                    //check if controller is real or virtual
                    if (controllerInfo.IsVirtual) {
                        currItem.Group = listViewRobots.Groups["robotsGroupSim"];
                        currItem.StateImageIndex = (int)abbStatus.found.sim;
                    } else {
                        currItem.Group = listViewRobots.Groups["robotsGroupNet"];
                        currItem.StateImageIndex = (int)abbStatus.found.net;
                    }
                    updateIcon(currItem, abbStatus.conn.available);
                    //add second column value =  IP address 
                    currItem.SubItems.Add(controllerInfo.IPAddress.ToString());
                    //add curent item to list
                    listViewRobots.Items.Add(currItem);
                    currItem.Tag = controllerInfo;
                }
            }
            //add network watcher to see if something changes in network
            abbWatcher = new NetworkWatcher(abbScanner.Controllers, true);
            abbWatcher.Found += abbWatcherFoundEvent;
            abbWatcher.Lost += abbWatcherLostEvent;
            //at end manage save project buttons
            manageSaveButtons();
        }

        /// <summary>
        /// Event triggered on close main app window
        /// </summary>
        /// <param name="sender">Main app window object to be closed</param>
        /// <param name="e">Event arguments</param>
        private void windowMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            abbWatcher.Found -= abbWatcherFoundEvent;
            abbWatcher.Lost -= abbWatcherLostEvent;
            //disconnect from controller (if connected)
            if (abbConnected != null) {
                abbConnected.Logoff();
                abbConnected.Dispose();
                abbConnected = null;
            }
        }

        /// <summary>
        /// Event connected to ABB network watched and triggered when controller was lost during program executrion
        /// </summary>
        /// <param name="sender">ABB Waatcher object</param>
        /// <param name="e">Event arguments</param>
        private void abbWatcherLostEvent(object sender, NetworkWatcherEventArgs e)
        {
            Invoke(new EventHandler<NetworkWatcherEventArgs>(updateRobotList), new Object[] { this, e });
        }

        /// <summary>
        /// Event connected to ABB network watched and triggered when controller was found during program executrion
        /// </summary>
        /// <param name="sender">ABB Waatcher object</param>
        /// <param name="e">Event arguments</param>
        private void abbWatcherFoundEvent(object sender, NetworkWatcherEventArgs e)
        {
            Invoke(new EventHandler<NetworkWatcherEventArgs>(updateRobotList), new Object[] { this, e });
        }

        /// <summary>
        /// Event triggered when main app window was resized (minimalized, etc)
        /// </summary>
        /// <param name="sender">Main app window object</param>
        /// <param name="e">Event arguments</param>
        private void mainWindow_Resize(object sender, EventArgs e)
        {
            //set the notification icon
            notifyIcon.Icon = this.Icon;
            //check if window is minimized or opened (normal or maximized)
            notifyIcon.Visible = (this.WindowState == FormWindowState.Minimized);
            //show tip and hide app from taskbar if window is minimized
            if (notifyIcon.Visible) this.Hide();
        }

        /********************************************************
         ***  MAIN WINDOW - main menu and bar actions
         ********************************************************/

        /// <summary>
        /// Event triggered on mouse key up on menu bar (MenuStrip object)
        /// </summary>
        /// <param name="sender">MenuStrip object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void menuBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isDragged = false;
        }

        /// <summary>
        /// Event triggered on mouse key down on menu bar (MenuStrip object)
        /// </summary>
        /// <param name="sender">MenuStrip object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void menuBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                isDragged = true;
                mouseX = e.X;
                mouseY = e.Y;
            }
        }

        /// <summary>
        /// Event triggered on mouse move on menu bar (MenuStrip object)
        /// </summary>
        /// <param name="sender">MenuStrip object that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void menuBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged) {
                Point curr = new Point();
                curr.X = Location.X + (e.X - mouseX);
                curr.Y = Location.Y + (e.Y - mouseY);
                //update form position
                Location = curr;
            }
        }

        /// <summary>
        /// Event triggered on NEW menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //clear application data
            appsClearData(true);
            //clear saved group robots
            cleanSavedGroup();
            //clear project
            appSettings.currProject = "";
            //update enable state of saved buttons
            manageSaveButtons();
        }

        /// <summary>
        /// Event triggered on OPEN menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show open dialog
            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.ShowDialog();
            //save elements to selected file
            loadMyRobots(openDialog.FileName);
            //manage save buttons
            manageSaveButtons();
        }

        /// <summary>
        /// Event triggered on SAVE menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (appSettings.currProject == "") {
                //show save dialog
                saveDialog.InitialDirectory = Application.StartupPath;
                saveDialog.ShowDialog();
                //save elements to selected file
                saveMyRobots(saveDialog.FileName);
            } else {
                //save elements to curr project file
                saveMyRobots(appSettings.currProject);
            }
            //manage save buttons
            manageSaveButtons();
        }

        /// <summary>
        /// Event triggered on SAVE AS menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show save dialog
            saveDialog.InitialDirectory = Application.StartupPath;
            saveDialog.ShowDialog();
            //save elements to selected file
            saveMyRobots(saveDialog.FileName);
            //manage save buttons
            manageSaveButtons();
        }

        /// <summary>
        /// Event triggered on SETTINGS (TOOLS) menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void toolsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //no new taskbar element
            appSettings.ShowInTaskbar = false;
            //position in center of main form
            appSettings.StartPosition = FormStartPosition.Manual;
            appSettings.Top = Top + (Height - appSettings.Height) / 2;
            appSettings.Left = Left + (Width - appSettings.Width) / 2;
            //show form (owner useful for reading run signal property)
            if (appSettings.ShowDialog(this) != DialogResult.Cancel) {
                //when form close then apply data
                appSettingsApply();
            }
        
        }

        /// <summary>
        /// Event triggered on HELP menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show about window
            windowAbout helpWindow = new windowAbout();
            //no new taskbar element
            helpWindow.ShowInTaskbar = false;
            //position in center of main form
            helpWindow.StartPosition = FormStartPosition.Manual;
            helpWindow.Top = Top + (Height - helpWindow.Height) / 2;
            helpWindow.Left = Left + (Width - helpWindow.Width) / 2;
            //show form
            helpWindow.ShowDialog();
        }

        /// <summary>
        /// Event triggered on MINIMIZE menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Event triggered on NOTIFICATION ICON button click
        /// </summary>
        /// <param name="sender">Parent that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //check which button was pressed
            if (e.Button == MouseButtons.Right) {
                notifyIcon.ContextMenuStrip.Show(Cursor.Position);
            } else {
                //reopen window
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Event triggered on SHOW (REOPEN) menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //reopen window
            Show();
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Event triggered on EXIT menu button 
        /// </summary>
        /// <param name="sender">Menu button that triggered current action</param>
        /// <param name="e">Event arguments</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save settings
            appSettings.saveData();
            //send e-mail that app is closed
            sendMail(abbStatus.mail.closeApp);
            //terminate app
            Close();
        }

        /// <summary>
        /// Method used to manage save and save as... buttons
        /// </summary>
        private void manageSaveButtons()
        {
            //save is only enabled if there is curr project opened
            if(appSettings.currProject != "") {
                saveToolStripMenuItem1.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            } else {
                saveToolStripMenuItem1.Enabled = false;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// Method used to apply settings to main window and all applications
        /// </summary>
        private void appSettingsApply()
        {
            labelCurrProjectPath.Visible = appSettings.showCurrProject;
        }

        /********************************************************
         ***  MAIN WINDOW - network visible robots list
         ********************************************************/

        /// <summary>
        /// Method used to update robot list (triggered from network watcher object)
        /// </summary>
        /// <param name="sender">Parent that triggered current event (controller found or lost event)</param>
        /// <param name="e">Event arguments</param>
        private void updateRobotList(object sender, NetworkWatcherEventArgs e)
        {
            ControllerInfo eventController = e.Controller;
            //check if event controler is in saved group
            int foundSavedController = 0;
            foreach (ListViewItem item in listViewRobots.Items) {
                //if current event controller was saved then break loop
                if (sameController(item, eventController) && item.Group.Header == "saved") break;
                foundSavedController++;
            }
            //check if controller was found or lost
            if (e.Reason == ChangeReasons.New) {
                //log that controller was found
                status.writeLog(logType.info, "controller <bu>" + eventController.Name + "</bu> found...");
                //found controller - check if its saved
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller exists in saved group - update its icon to available
                    ListViewItem updItem = listViewRobots.Items[foundSavedController];
                    updateIcon(updItem, abbStatus.conn.available);
                    updItem.Tag = eventController;
                    //update apps data
                    appsControllerFound(ControllerFactory.CreateFrom(eventController));
                } else {
                    //controller is not saved - add to list                    
                    ListViewItem updItem = new ListViewItem(eventController.Name);
                    //check if controller is real or virtual
                    if (eventController.IsVirtual) {
                        updItem.Group = listViewRobots.Groups["robotsGroupSim"];
                    } else {
                        updItem.Group = listViewRobots.Groups["robotsGroupNet"];
                    }
                    updateIcon(updItem, abbStatus.conn.available);
                    updItem.SubItems.Add(eventController.IPAddress.ToString());
                    updItem.Tag = eventController;
                    listViewRobots.Items.Add(updItem);
                }
            } else {
                //lost controller - check if it was connected
                if (abbConnected != null && eventController.Name == abbConnected.SystemName) {
                    if (disconnectController(ref abbConnected) == (int)abbStatus.conn.disconnOK) {
                        //clear controller address in my programs
                        appRemotePC.desyncController();
                        //log that current controller was disconnected because its lost
                        status.writeLog(logType.warning, "controller <bu>" + eventController.Name + "</bu> lost and disconnected...");
                    } else {
                        //log that current controller not disconnected
                        status.writeLog(logType.error, "controller <bu>" + eventController.Name + "</bu> can't disconnect...");
                    }
                } else {
                    //log that controller was found
                    status.writeLog(logType.info, "controller <bu>" + eventController.Name + "</bu> lost...");
                }
                //check if controller was in saved group
                if (foundSavedController < listViewRobots.Items.Count) {
                    //controller lost in saved group - update its icon to red
                    ListViewItem updItem = listViewRobots.Items[foundSavedController];
                    updateIcon(updItem, abbStatus.conn.notAvailable);
                    updItem.Tag = null;
                    //update apps data
                    appsControllerLost(ControllerFactory.CreateFrom(eventController));
                }
                else {
                    //remove from list (wasnt in saved group)
                    foreach (ListViewItem item in listViewRobots.Items) {
                        if ((ControllerInfo)item.Tag == eventController) {
                            listViewRobots.Items.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method triggered before showing robot list popup menu 
        /// </summary>
        /// <param name="sender">Parent popup menu that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void robotListQMenu_Opening(object sender, CancelEventArgs e)
        {
            if (listViewRobots.SelectedItems.Count > 0) {
                /***** CONNECT/DISCONNECT FROM CONTROLLER *****/
                if (abbConnected == null) {
                    //no controller connected - disconect menu not visible
                    connectToolStripMenuItem.Visible = true;
                    disconnectToolStripMenuItem.Visible = false;
                } else {
                    //controller which is connected only has disconnect menu
                    string itemName = listViewRobots.FocusedItem.Text;
                    if (itemName == abbConnected.SystemName) {
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

        /// <summary>
        /// Method triggered after click popup menu item REMOVE FROM SAVED GROUP
        /// </summary>
        /// <param name="sender">Popup menu button parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //only saved controllers can be removed from list
            ListViewItem currItem = listViewRobots.SelectedItems[0];
            string controllerName = currItem.Text;
            //check if current element is virtual or real
            bool simController = currItem.StateImageIndex >= (int)abbStatus.found.sim;
            //if removed controller exists in network then change it group
            if (simController) {
                if (currItem.StateImageIndex == (int)abbStatus.found.simSaveAvail) {
                    currItem.Group = listViewRobots.Groups[1];
                    updateIcon(currItem, abbStatus.conn.available);
                } else if (currItem.StateImageIndex == (int)abbStatus.found.simSaveConn) {
                    currItem.Group = listViewRobots.Groups[1];
                    updateIcon(currItem, abbStatus.conn.connOK);
                } else {
                    //robot not found - remove it from list
                    listViewRobots.Items.Remove(currItem);
                }
            } else {
                if (currItem.StateImageIndex == (int)abbStatus.found.netSaveAvail) {
                    currItem.Group = listViewRobots.Groups[0];
                    updateIcon(currItem, abbStatus.conn.available);
                } else if (currItem.StateImageIndex == (int)abbStatus.found.netSaveConn) {
                    currItem.Group = listViewRobots.Groups[0];
                    updateIcon(currItem, abbStatus.conn.connOK);
                } else {
                    //robot not found - remove it from list
                    listViewRobots.Items.Remove(currItem);
                }
            }
            //sort updated elements
            listViewRobots.Sort();
            status.writeLog(logType.warning, "controller <bu>" + controllerName + "</bu> removed from saved group!");
        }

        /// <summary>
        /// Method triggered after click popup menu item ADD TO SAVED GROUP
        /// </summary>
        /// <param name="sender">Popup menu button parent that triggered this event</param>
        /// <param name="e">Event arguments</param>
        private void addToSavedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //get selected item with quick menu
            ListViewItem toSaveItem = listViewRobots.SelectedItems[0];
            string controllerName = toSaveItem.Text;
            //update GUI (move to saved group and change icon to connected!)
            toSaveItem.Group = listViewRobots.Groups[2];
            if (abbConnected != null && abbConnected.SystemName == toSaveItem.Text) {
                updateIcon(toSaveItem, abbStatus.conn.connOK);
            } else {
                updateIcon(toSaveItem, abbStatus.conn.available);
            }
            //sort updated elements
            listViewRobots.Sort();
            status.writeLog(logType.info, "controller <bu>" + controllerName + "</bu> added to saved group!");
        }

        /// <summary>
        /// Method used to update robot list items icon (connected, visible, not visible, saved, etc)
        /// </summary>
        /// <param name="cItem">Robot list item to update icon</param>
        /// <param name="newStatus">New status of items icon</param>
        /// <param name="overrideSim">Flag used to override check of real or virtual controller type</param>
        private void updateIcon(ListViewItem cItem, abbStatus.conn newStatus, int overrideSim = -1)
        {
            int offset = -1;
            //check if user specified if current item is sim or real
            if (overrideSim >= 0) {
                offset = overrideSim == 0 ? 0 : 4;
            } else {
                //check if selected item is virtual or real
                if (cItem.StateImageIndex >= 0) {
                    offset = Convert.ToInt16(cItem.StateImageIndex >= (int)abbStatus.found.sim) * 4;
                } else {
                    if (cItem.Group == listViewRobots.Groups["robotsGroupNet"]) {
                        offset = 0;
                    } else if (cItem.Group == listViewRobots.Groups["robotsGroupSim"]) {
                        offset = 4;
                    }
                }
            }
            //check if input is ok
            if (offset >= 0) {
                //check new status
                if (newStatus == abbStatus.conn.available) {
                    if (cItem.Group.Header == "saved") {
                        cItem.StateImageIndex = (int)abbStatus.found.netSaveAvail + offset;
                    } else {
                        cItem.StateImageIndex = (int)abbStatus.found.net + offset;
                    }
                } else if (newStatus == abbStatus.conn.connOK) {
                    cItem.StateImageIndex = (int)abbStatus.found.netSaveConn + offset;
                } else if (newStatus == abbStatus.conn.disconnOK) {
                    if (cItem.Group.Header == "saved") {
                        cItem.StateImageIndex = (int)abbStatus.found.netSaveAvail + offset;
                    } else {
                        cItem.StateImageIndex = (int)abbStatus.found.net + offset;
                    }
                } else if (newStatus == abbStatus.conn.notAvailable) {
                    //only saved items can be visible on list and not available!
                    cItem.StateImageIndex = (int)abbStatus.found.netSaveDiscon + offset;
                }
            }
        }

        /// <summary>
        /// Event triggered after SplitContainer collapse button clisk
        /// </summary>
        /// <param name="sender">SplitContainer parent that triggers current event</param>
        /// <param name="e">Event arguments</param>
        private void btnRobotListCollapse_Click(object sender, EventArgs e)
        {
            //toggle panel collapse
            appContainer.Panel1Collapsed = !appContainer.Panel1Collapsed;
            //check if panel is collapsed (then adjust GUI)
            if (appContainer.Panel1Collapsed) {
                btnRobotListCollapse.Parent = appContainer.Panel2;
                btnRobotListCollapse.Dock = DockStyle.Left;
                btnRobotListCollapse.Text = ">>>";
                tabActions.Location = new Point(42, 12);
                tabActions.Width -= 40;
                //if container is collapsed then center all views in tabs
                foreach (TabPage page in tabActions.TabPages) {
                    if (page.Controls.Count > 0) {
                        Control child = page.Controls[0];
                        child.Dock = DockStyle.None;
                        child.Left = (tabActions.Width - child.Width) / 2;
                    }
                }
            } else {
                btnRobotListCollapse.Parent = appContainer.Panel1;
                btnRobotListCollapse.Dock = DockStyle.Right;
                btnRobotListCollapse.Text = "<<<";
                tabActions.Location = new Point(2, 12);
                tabActions.Width += 40;
                //if container is not collapsed then dock all views in tabs
                foreach (TabPage page in tabActions.TabPages) {
                    if (page.Controls.Count > 0) {
                        Control child = page.Controls[0];
                        child.Dock = DockStyle.Fill;
                    }
                }
            }
        }

        /// <summary>
        /// Event triggered at keyboard key down on item from robot list
        /// </summary>
        /// <param name="sender">Robot list that triggers current element</param>
        /// <param name="e">Event arguments</param>
        private void listViewRobots_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete) {
                //delete robot only from saved group
                ListViewItem item = listViewRobots.FocusedItem;
                if (item != null && item.Group.Header == "saved") {
                    int currState = item.StateImageIndex;
                    if (currState == (int)abbStatus.found.netSaveConn) {
                        //visible robots move to network groups
                        item.Group = listViewRobots.Groups["robotsGroupNet"];
                        item.StateImageIndex = (int)abbStatus.found.net;
                    } else if (currState == (int)abbStatus.found.simSaveConn) {
                        //visible robots move to virtual groups
                        item.Group = listViewRobots.Groups["robotsGroupSim"];
                        item.StateImageIndex = (int)abbStatus.found.sim;
                    } else {
                        //delete non-visible robots
                        this.listViewRobots.Items.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Event triggered on robot list item popup menu CONNECT click
        /// </summary>
        /// <param name="sender">Popup menu that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //connect to selected controller
            int connStatus = connectController(listViewRobots.FocusedItem);
            //send controller address to my apps
            if (connStatus == (int)abbStatus.conn.connOK) appsControllerSync(abbConnected);
        }

        /// <summary>
        /// Event triggered when double click on robot list item
        /// </summary>
        /// <param name="sender">Popup menu item that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void listViewRobots_DoubleClick(object sender, EventArgs e)
        {
            //connect to selected controller (simulate connect tool strip click event)
            connectToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Event triggered on robot list item popup menu DISCONNECT click
        /// </summary>
        /// <param name="sender">Popup menu that triggered current event</param>
        /// <param name="e">Event arguments</param>
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //disconnect from selected controller
            int disconnStatus = disconnectController(listViewRobots.FocusedItem);
            //clear controller address in my programs
            if (disconnStatus == (int)abbStatus.conn.disconnOK) appsControllerDesync();
        }

        /// <summary>
        /// Method used to clean 
        /// </summary>
        private void cleanSavedGroup()
        {
            //disconnect from current controller
            disconnectController(ref abbConnected);
            //move/delete robots from saved group
            foreach (ListViewItem item in this.listViewRobots.Items) {
                if (item.Group.Header == "saved") {
                    int currState = item.StateImageIndex;
                    if (currState == (int)abbStatus.found.netSaveConn || currState == (int)abbStatus.found.netSaveAvail) {
                        //visible robots move to network groups
                        item.Group = listViewRobots.Groups["robotsGroupNet"];
                        updateIcon(item, abbStatus.conn.available);
                    } else if (currState == (int)abbStatus.found.simSaveConn || currState == (int)abbStatus.found.simSaveAvail) {
                        //visible robots move to virtual groups
                        item.Group = listViewRobots.Groups["robotsGroupSim"];
                        updateIcon(item, abbStatus.conn.available);
                    } else {
                        //delete non-visible robots
                        listViewRobots.Items.Remove(item);
                    }
                }
            }
        }

        /********************************************************
         ***  MAIN WINDOW - abb robots management
         ********************************************************/

        /// <summary>
        /// Method used to save all listed robots that are in SAVED GROUP
        /// </summary>
        /// <param name="filePath">XML file destination path</param>
        private void saveMyRobots(string filePath)
        {
            //check if path is correct
            if (filePath != "") {
                try {
                    //xml settings to create indents in file
                    XmlWriterSettings xmlSettings = new XmlWriterSettings();
                    xmlSettings.Indent = true;
                    //instance of xmlFile
                    XmlWriter xmlFile = XmlWriter.Create(filePath, xmlSettings);
                    xmlFile.WriteStartElement("myRobots");
                    //save all robots from list (only from saved group)
                    for (int i = 0; i < listViewRobots.Items.Count; i++) {
                        if (listViewRobots.Items[i].Group == listViewRobots.Groups[2]) {
                            string robotName = listViewRobots.Items[i].SubItems[0].Text;
                            xmlFile.WriteStartElement("robot_" + i.ToString());
                            xmlFile.WriteAttributeString("name", robotName);
                            xmlFile.WriteAttributeString("IP", listViewRobots.Items[i].SubItems[1].Text);
                            xmlFile.WriteAttributeString("type", listViewRobots.Items[i].StateImageIndex.ToString());
                            //save user applications data (needs Controller object)
                            appsSaveData(ref xmlFile, itemToController(listViewRobots.Items[i]), robotName);
                            //end element
                            xmlFile.WriteEndElement();
                        }
                    }
                    //close up file
                    xmlFile.WriteEndElement();
                    xmlFile.Close();
                    status.writeLog(logType.info, "File saved (only robots from saved group)!");
                    //update curr proj path
                    appSettings.currProject = filePath;
                } catch (XmlException) {
                    status.writeLog(logType.error, "Error while saving to XML...");
                }
            }
        }

        /// <summary>
        /// Method used to load all SAVED GROUP robots and place them in robot list
        /// </summary>
        /// <param name="filePath">XML file path to load robots from</param>
        private void loadMyRobots(string filePath)
        {
            //check if path is correct
            if (filePath != "" && File.Exists(filePath)) {
                try {
                    //clean robots from saved group
                    cleanSavedGroup();
                    //clear application data
                    appsClearData(true);
                    //create instance of xml to read
                    XmlReader xmlRead = XmlReader.Create(filePath);
                    while (xmlRead.Read()) {
                        //read every node from XML document
                        if ((xmlRead.NodeType == XmlNodeType.Element) && (xmlRead.Name.StartsWith("robot_"))) {
                            if (xmlRead.HasAttributes) {
                                string robotName = xmlRead.GetAttribute("name");
                                ListViewItem listItem = new ListViewItem(robotName);
                                listItem.Group = listViewRobots.Groups["robotsGroupSaved"];
                                //check if controller is real or virtual
                                bool simController = short.Parse(xmlRead.GetAttribute("type")) >= (int)abbStatus.found.sim;
                                //check if controller is visible and adjust icon
                                int listController = findController(listItem.Text, simController);
                                if (listController != -1) {
                                    //controller visible - delete it from network group and set blue icon
                                    listItem.Tag = listViewRobots.Items[listController].Tag;
                                    listViewRobots.Items.RemoveAt(listController);
                                    updateIcon(listItem, abbStatus.conn.available, Convert.ToInt16(simController));
                                } else {
                                    //controller not visible - set red icon (real or sim)
                                    updateIcon(listItem, abbStatus.conn.notAvailable, Convert.ToInt16(simController));
                                }
                                //add second column value =  IP address 
                                listItem.SubItems.Add(xmlRead.GetAttribute("IP"));
                                //add curent item to list
                                listViewRobots.Items.Add(listItem);
                                //load apps data
                                appsLoadData(ref xmlRead, itemToController(listItem), robotName);
                            }
                        }
                    }
                    //close up file
                    xmlRead.Close();
                    //update curr proj path
                    appSettings.currProject = filePath;
                } catch (XmlException) {
                    status.writeLog(logType.error, "Loaded file isn't XML or it's damaged...");
                }
            }
        }

        /// <summary>
        /// Function used to convert from ListViewItem to ABB Controller object
        /// </summary>
        /// <param name="item">List item to convert</param>
        /// <returns>ABB Controller object converted from ListViewItem</returns>
        private Controller itemToController(ListViewItem item)
        {
            Controller result = null;
            if (item.Tag != null) result = ControllerFactory.CreateFrom((ControllerInfo)item.Tag);
            return result;
        }

        /// <summary>
        /// Function used to check if current ListViewItem is the same as referenced ABB ControllerInfo object
        /// </summary>
        /// <param name="listedRobot">List item to be checked</param>
        /// <param name="abbRobot">Reference ABB ControllerInfo object</param>
        /// <returns>TRUE if both items represents the same controller, FALSE otherwise</returns>
        private bool sameController(ListViewItem listedRobot, ControllerInfo abbRobot)
        {
            bool sameName = listedRobot.Text == abbRobot.Name;
            bool sameType = abbRobot.IsVirtual && listedRobot.StateImageIndex >= (int)abbStatus.found.sim;

            return sameName && sameType;
        }

        /// <summary>
        /// Function used to check if current ListViewItem is the same as referenced ABB name and type objects
        /// </summary>
        /// <param name="listedRobot">List item to be checked</param>
        /// <param name="robotName">Name of ABB reference controller</param>
        /// <param name="robotSim">Type of ABB reference controller (real = FALSE or virtual = TRUE)</param>
        /// <returns>TRUE if both items represents the same controller, FALSE otherwise</returns>
        private bool sameController(ListViewItem listedRobot, string robotName, bool robotSim)
        {
            bool sameName = listedRobot.Text == robotName;
            bool sameType = robotSim && listedRobot.StateImageIndex >= (int)abbStatus.found.sim;

            return sameName && sameType;
        }

        /// <summary>
        /// Function used to check if inputted controller is present on robot list
        /// </summary>
        /// <param name="robotName">Searched ABB Controller name (SystemName)</param>
        /// <param name="robotSim">Searched ABB Controller type (real = FALSE, virtual = TRUE)</param>
        /// <returns>Index of inputted ABB Controller (-1 if non-existent)</returns>
        private int findController(string robotName, bool robotSim)
        {
            int result = -1, index = 0;

            foreach (ListViewItem item in this.listViewRobots.Items) {
                //match controller names
                if (item.Text == robotName) {
                    bool simCondition = robotSim == true && item.StateImageIndex >= (int)abbStatus.found.sim;
                    bool netCondition = robotSim == false && item.StateImageIndex < (int)abbStatus.found.sim;
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

        /// <summary>
        /// Function used to connect to inputted controller from list
        /// </summary>
        /// <param name="listController">List item from robot list controller to connect to</param>
        /// <returns>Connection status</returns>
        private int connectController(ListViewItem listController)
        {
            int result = (int)abbStatus.conn.notAvailable;
            string cName = listController.Text;

            if (listController != null && listController.Tag != null) {
                ControllerInfo controller = (ControllerInfo)listController.Tag;
                if (controller.Availability == Availability.Available) {
                    try {
                        //first disconnect from previous controller (if connected)
                        disconnectController(ref abbConnected);
                        //show status conn status and process messages 
                        status.writeLog(logType.info, "controller <bu>" + cName + "</bu> connecting...");
                        Application.DoEvents();
                        //create controller and log in
                        abbConnected = ControllerFactory.CreateFrom(controller);
                        result = connectController(ref abbConnected);
                        if (result == (int)abbStatus.conn.connOK) {
                            //update status
                            status.writeLog(logType.info, "controller <bu>" + cName + "</bu> connected!");
                        }
                    } catch (ABB.Robotics.GenericControllerException e) {
                        status.writeLog(logType.error, "controller <bu>" + abbConnected.SystemName + "</bu>" +
                                        e.Message+".. wait a while and try again.");
                    } catch (Exception e) {
                        status.writeLog(logType.error, "controller <bu>" + abbConnected.SystemName + "</bu>" +
                                        e.Message + ".. wait a while and try again.");
                    }
                } else {
                    //output
                    result = (int)abbStatus.conn.notAvailable;
                    //update status
                    status.writeLog(logType.info, "controller <bu>" + cName + "</bu> not available!");
                }
            } else {
                //output
                result = (int)abbStatus.conn.notVisible;
                //update status
                status.writeLog(logType.warning, "controller <bu>" + cName + "</bu> not visible in network!");
            }

            return result;
        }

        /// <summary>
        /// Function used to connect to inputted controller object
        /// </summary>
        /// <param name="selController">ABB Controller object to connect to</param>
        /// <returns>Connection status</returns>
        private int connectController(ref Controller selController)
        {
            selController.Logon(UserInfo.DefaultUser);
            if (selController.Connected) {
                //update icon on list (first find which element)
                int element = findController(selController.SystemName, selController.IsVirtual);
                updateIcon(listViewRobots.Items[element], abbStatus.conn.connOK);
                //update conn label
                labelConnControllerName.Text = "connected to: "+selController.SystemName;
                //output
                return (int)abbStatus.conn.connOK;
            } else {
                //output
                return (int)abbStatus.conn.disconnOK;
            }
        }

        /// <summary>
        /// Function used to connect to main controller object
        /// </summary>
        /// <returns>Connection status</returns>
        private int connectController()
        {
            abbConnected.Logon(UserInfo.DefaultUser);
            if (abbConnected.Connected) {
                //update icon on list (first find which element)
                int element = findController(abbConnected.SystemName, abbConnected.IsVirtual);
                updateIcon(listViewRobots.Items[element], abbStatus.conn.connOK);
                //update conn label
                labelConnControllerName.Text = "connected to: " + abbConnected.SystemName;
                //output
                return (int)abbStatus.conn.connOK;
            } else {
                //output
                return (int)abbStatus.conn.disconnOK;
            }
        }

        /// <summary>
        /// Function used to disconnect from inputted controller form robot list
        /// </summary>
        /// <param name="listController">List item from robot list controller to disconnect from</param>
        /// <returns>Disonnection status</returns>
        private int disconnectController(ListViewItem listController)
        {
            int result = (int)abbStatus.conn.notAvailable;
            string cName = listController.Text;

            if (listController != null && listController.Tag != null) {
                ControllerInfo controller = (ControllerInfo)listController.Tag;
                if (abbConnected.SystemName == listController.Text && controller.Availability == Availability.Available) {
                    //disconnect from selected controller
                    result = disconnectController(ref abbConnected);
                    //show disconn status and process messages 
                    status.writeLog(logType.info, "controller <bu>" + cName + "</bu> disconnected!");
                } else {
                    result = (int)abbStatus.conn.notAvailable;
                    //show disconn status and process messages 
                    status.writeLog(logType.warning, "controller <bu>" + cName + "</bu> not available!");
                }
            }
            return result;
        }

        /// <summary>
        /// Function used to disconnect from inputted controller object
        /// </summary>
        /// <param name="selController">ABB Controller object to disconnect from</param>
        /// <returns>Disonnection status</returns>
        private int disconnectController(ref Controller selController)
        {
            //disconnect from selected controller
            if (selController != null) {
                //update icon on list (first find which element)
                int element = findController(selController.SystemName, selController.IsVirtual);
                updateIcon(listViewRobots.Items[element], abbStatus.conn.disconnOK);
                //log off from controller
                selController.Logoff();
                selController.Dispose();
                selController = null;
            }
            //update conn label
            labelConnControllerName.Text = "connected to: ---";
            //return disconnect status
            return (int)abbStatus.conn.disconnOK;
        }

        /// <summary>
        /// Function used to disconnect from main controller object
        /// </summary>
        /// <returns>Disonnection status</returns>
        private int disconnectController()
        {
            //disconnect from selected controller
            if (abbConnected != null) {
                //update icon on list (first find which element)
                int element = findController(abbConnected.SystemName, abbConnected.IsVirtual);
                updateIcon(listViewRobots.Items[element], abbStatus.conn.disconnOK);
                //log off from controller
                abbConnected.Logoff();
                abbConnected.Dispose();
                abbConnected = null;
            }
            //update conn label
            labelConnControllerName.Text = "connected to: ---";
            //return disconnect status
            return (int)abbStatus.conn.disconnOK;
        }

        /********************************************************
         ***  MAIN WINDOW - email management
         ********************************************************/

        /// <summary>
        /// Method used to trigger mail send event
        /// </summary>
        /// <param name="mailType">Type of mail to send (open, close app, exception triggered)</param>
        private void sendMail(abbStatus.mail mailType)
        {
            //check if sending e-mail is active
            if (appSettings.mailService.getMailStatus(mailType)) {

            }
        }

        /********************************************************
         ***  MAIN WINDOW - abbTools apps operations
         ********************************************************/

        /// <summary>
        /// Method called to execute all apps actions after main window loads
        /// </summary>
        private void appsLoadProgramActions()
        {
            //remotePC
            appRemotePC.syncLogger(status);
            //backupManager
            appBackupManager.syncLogger(status);
            //windows IPC
            appWindowsIPC.syncLogger(status);
        }

        /// <summary>
        /// Method called to execute all apps clear data action
        /// </summary>
        /// <param name="resyncLogger">Flag informing that logger should be resynchronized after clear</param>
        private void appsClearData(bool resyncLogger)
        {
            appRemotePC.clearAppData();
            appBackupManager.clearAppData();
            appWindowsIPC.clearAppData();
            //check if we want to resync logger
            if (resyncLogger && status != null) {
                appRemotePC.syncLogger(status);
                appBackupManager.syncLogger(status);
                appWindowsIPC.syncLogger(status);
            }
        }

        /// <summary>
        /// Method used to connect all my application events
        /// </summary>
        private void appsConnectEvents()
        {
            //app: backupManager
            appBackupManager.UpdateBackupTimeInXML += updateBackupTimeInFile;
        }

        /// <summary>
        /// Method called to execute all apps load data action
        /// </summary>
        /// <param name="myXML">XML file to load data from</param>
        /// <param name="myController">ABB Controller parent of loading data</param>
        /// <param name="cName">ABB Controller parent name of loading data (useful when not visible during load)</param>
        private void appsLoadData(ref XmlReader myXML, Controller myController, string cName = "")
        {
            //check if input XML file has correct starting node
            if (myXML.Name.StartsWith("robot_")) {
                //check current parent name with file data
                if (cName == "") {
                    //update current parent name
                    cName = myXML.GetAttribute("name");
                    status.writeLog(logType.warning, "Inputted controller name empty... Updated for " + cName);
                } else if (cName != myXML.GetAttribute("name")) {
                    status.writeLog(logType.warning, "Cant load settings for " + cName + " - name differs from file.");
                    return;
                }
                //load applications data
                appRemotePC.loadAppData(ref myXML, myController, cName);
                appBackupManager.loadAppData(ref myXML, myController, cName);
                appWindowsIPC.loadAppData(ref myXML, myController, cName);
            } else {
                status.writeLog(logType.error, "Cant load settings for "+cName+" - wrong start node in XML file...");
            }
        }

        /// <summary>
        /// Method called to execute all apps save data action
        /// </summary>
        /// <param name="myXML">XML file to save data to</param>
        /// <param name="myController">ABB Controller parent of saving data</param>
        /// <param name="cName">ABB Controller parent name of saving data (useful when currently not visible in network)</param>
        private void appsSaveData(ref XmlWriter myXML, Controller myController, string cName = "")
        {
            appRemotePC.saveAppData(ref myXML, myController, cName);
            appBackupManager.saveAppData(ref myXML, myController, cName);
            appWindowsIPC.saveAppData(ref myXML, myController, cName);
        }

        /// <summary>
        /// Method called to execute all apps sync controller action 
        /// </summary>
        /// <param name="syncController">ABB Controller that has to be sync with apps (we connected to it)</param>
        private void appsControllerSync(Controller syncController)
        {
            appRemotePC.syncController(syncController);
            appBackupManager.syncController(syncController);
            appWindowsIPC.syncController(syncController);
        }

        /// <summary>
        /// Method called to execute all apps desync controller action 
        /// </summary>
        private void appsControllerDesync()
        {
            appRemotePC.desyncController();
            appBackupManager.desyncController();
            appWindowsIPC.desyncController();
        }

        /// <summary>
        /// Method called to execute all apps found controller from saved list action 
        /// </summary>
        /// <param name="foundController">ABB Controller that has been found in network</param>
        private void appsControllerFound(Controller foundController)
        {
            appRemotePC.controllerFound(foundController);
            appBackupManager.savedControllerFound(foundController);
            appWindowsIPC.savedControllerFound(foundController);
        }

        /// <summary>
        /// Method called to execute all apps lost controller from saved list action 
        /// </summary>
        /// <param name="lostController">ABB Controller that has been lost from network</param>
        private void appsControllerLost(Controller lostController)
        {
            appRemotePC.controllerLost(lostController);
            appBackupManager.savedControllerLost(lostController);
            appWindowsIPC.savedControllerLost(lostController);
        }

        /********************************************************
         ***  MAIN WINDOW - apps events
         ********************************************************/

        /// <summary>
        /// Method triggered on update of last backup time to save it to file
        /// </summary>
        private void updateBackupTimeInFile()
        {
            //only if project is loaded we can update XML file with new backup times
            if (appSettings.currProject != null && appSettings.currProject != "") {
                //save XML file to update last backup times
                saveMyRobots(appSettings.currProject);
            }
        }
    }
}