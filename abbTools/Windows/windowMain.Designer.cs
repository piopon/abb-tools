﻿namespace abbTools
{
    partial class windowMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(windowMain));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("network", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("virtual", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("saved", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "ABBtrack",
            "127.0.0.1"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "robot 3",
            "255.255.255.255"}, -1);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconQMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMainMenu = new System.Windows.Forms.Panel();
            this.labelAppTitle = new System.Windows.Forms.Label();
            this.panelStatusBar = new System.Windows.Forms.Panel();
            this.panelConnStatus = new System.Windows.Forms.Panel();
            this.labelConnControllerName = new System.Windows.Forms.Label();
            this.panelLogger = new System.Windows.Forms.Panel();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.pictureLogType = new System.Windows.Forms.PictureBox();
            this.panelApp = new System.Windows.Forms.Panel();
            this.robotListQMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToSavedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagesStatus = new System.Windows.Forms.ImageList(this.components);
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.imagesLogType = new System.Windows.Forms.ImageList(this.components);
            this.appContainer = new System.Windows.Forms.SplitContainer();
            this.btnRobotListCollapse = new System.Windows.Forms.Button();
            this.listViewRobots = new System.Windows.Forms.ListView();
            this.robotGroupColName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.robotGroupColIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabActions = new System.Windows.Forms.TabControl();
            this.actionDashboard = new System.Windows.Forms.TabPage();
            this.actionRemotePC = new System.Windows.Forms.TabPage();
            this.appRemotePC = new abbTools.appRemoteABB();
            this.actionBackupManager = new System.Windows.Forms.TabPage();
            this.appBackupManager = new abbTools.AppBackupManager.appBackupManager();
            this.actionWinIPC = new System.Windows.Forms.TabPage();
            this.appWindowsIPC = new abbTools.AppWindowsIPC.appWindowsIPC();
            this.notifyIconQMenu.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.panelMainMenu.SuspendLayout();
            this.panelStatusBar.SuspendLayout();
            this.panelConnStatus.SuspendLayout();
            this.panelLogger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogType)).BeginInit();
            this.panelApp.SuspendLayout();
            this.robotListQMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appContainer)).BeginInit();
            this.appContainer.Panel1.SuspendLayout();
            this.appContainer.Panel2.SuspendLayout();
            this.appContainer.SuspendLayout();
            this.tabActions.SuspendLayout();
            this.actionRemotePC.SuspendLayout();
            this.actionBackupManager.SuspendLayout();
            this.actionWinIPC.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "stealth mode on!";
            this.notifyIcon.BalloonTipTitle = "abbTools";
            this.notifyIcon.ContextMenuStrip = this.notifyIconQMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "abbTools";
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // notifyIconQMenu
            // 
            this.notifyIconQMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notifyIconQMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.notifyIconQMenu.Name = "notifyIconQMenu";
            this.notifyIconQMenu.Size = new System.Drawing.Size(119, 56);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.testToolStripMenuItem.Text = "show";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.exitToolStripMenuItem.Text = "exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuBar
            // 
            this.menuBar.BackColor = System.Drawing.Color.DarkOrange;
            this.menuBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.menuBar.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.menuBar.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.exitToolStripMenuItem1,
            this.minimizeToolStripMenuItem,
            this.helpToolStripMenuItem1,
            this.toolsToolStripMenuItem1});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(1008, 64);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuBar";
            this.menuBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuBar_MouseDown);
            this.menuBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menuBar_MouseMove);
            this.menuBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.menuBar_MouseUp);
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem2});
            this.fileToolStripMenuItem1.Image = global::abbTools.Properties.Resources.mainMenu_abb;
            this.fileToolStripMenuItem1.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(62, 60);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem1.Image")));
            this.newToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.newToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(280, 28);
            this.newToolStripMenuItem1.Text = "&New";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem1.Image")));
            this.openToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.openToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(280, 28);
            this.openToolStripMenuItem1.Text = "&Open";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
            this.saveToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(280, 28);
            this.saveToolStripMenuItem1.Text = "&Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(280, 28);
            this.saveAsToolStripMenuItem.Text = "Save &as...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(277, 6);
            // 
            // exitToolStripMenuItem2
            // 
            this.exitToolStripMenuItem2.Font = new System.Drawing.Font("GOST Common", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem2.Name = "exitToolStripMenuItem2";
            this.exitToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem2.Size = new System.Drawing.Size(280, 28);
            this.exitToolStripMenuItem2.Text = "E&xit";
            this.exitToolStripMenuItem2.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem;
            this.exitToolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitToolStripMenuItem1.BackColor = System.Drawing.Color.Transparent;
            this.exitToolStripMenuItem1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exitToolStripMenuItem1.ForeColor = System.Drawing.Color.Transparent;
            this.exitToolStripMenuItem1.Image = global::abbTools.Properties.Resources.mainMenu_exit;
            this.exitToolStripMenuItem1.Margin = new System.Windows.Forms.Padding(1, 1, 8, 1);
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(2);
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(58, 58);
            this.exitToolStripMenuItem1.Text = " exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // minimizeToolStripMenuItem
            // 
            this.minimizeToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.minimizeToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.minimizeToolStripMenuItem.Image = global::abbTools.Properties.Resources.mainMenu_minimize;
            this.minimizeToolStripMenuItem.Name = "minimizeToolStripMenuItem";
            this.minimizeToolStripMenuItem.Size = new System.Drawing.Size(62, 60);
            this.minimizeToolStripMenuItem.Text = "minimize";
            this.minimizeToolStripMenuItem.Click += new System.EventHandler(this.minimizeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripMenuItem1.ForeColor = System.Drawing.Color.Transparent;
            this.helpToolStripMenuItem1.Image = global::abbTools.Properties.Resources.mainMenu_info;
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(62, 60);
            this.helpToolStripMenuItem1.Text = "&Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // toolsToolStripMenuItem1
            // 
            this.toolsToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolsToolStripMenuItem1.Image = global::abbTools.Properties.Resources.mainMenu_settings;
            this.toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
            this.toolsToolStripMenuItem1.Size = new System.Drawing.Size(62, 60);
            this.toolsToolStripMenuItem1.Text = "&Tools";
            this.toolsToolStripMenuItem1.Click += new System.EventHandler(this.toolsToolStripMenuItem1_Click);
            // 
            // panelMainMenu
            // 
            this.panelMainMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMainMenu.Controls.Add(this.labelAppTitle);
            this.panelMainMenu.Controls.Add(this.menuBar);
            this.panelMainMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMainMenu.ForeColor = System.Drawing.Color.Black;
            this.panelMainMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMainMenu.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panelMainMenu.Name = "panelMainMenu";
            this.panelMainMenu.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.panelMainMenu.Size = new System.Drawing.Size(1010, 65);
            this.panelMainMenu.TabIndex = 2;
            // 
            // labelAppTitle
            // 
            this.labelAppTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAppTitle.AutoSize = true;
            this.labelAppTitle.BackColor = System.Drawing.Color.DarkOrange;
            this.labelAppTitle.Enabled = false;
            this.labelAppTitle.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAppTitle.ForeColor = System.Drawing.Color.Black;
            this.labelAppTitle.Location = new System.Drawing.Point(466, 22);
            this.labelAppTitle.Name = "labelAppTitle";
            this.labelAppTitle.Size = new System.Drawing.Size(62, 17);
            this.labelAppTitle.TabIndex = 1;
            this.labelAppTitle.Text = "abbTools";
            this.labelAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelStatusBar
            // 
            this.panelStatusBar.BackColor = System.Drawing.Color.Gray;
            this.panelStatusBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatusBar.Controls.Add(this.panelConnStatus);
            this.panelStatusBar.Controls.Add(this.panelLogger);
            this.panelStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatusBar.Location = new System.Drawing.Point(0, 679);
            this.panelStatusBar.Name = "panelStatusBar";
            this.panelStatusBar.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panelStatusBar.Size = new System.Drawing.Size(1010, 30);
            this.panelStatusBar.TabIndex = 3;
            // 
            // panelConnStatus
            // 
            this.panelConnStatus.Controls.Add(this.labelConnControllerName);
            this.panelConnStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelConnStatus.Location = new System.Drawing.Point(820, 0);
            this.panelConnStatus.Name = "panelConnStatus";
            this.panelConnStatus.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.panelConnStatus.Size = new System.Drawing.Size(183, 28);
            this.panelConnStatus.TabIndex = 5;
            // 
            // labelConnControllerName
            // 
            this.labelConnControllerName.BackColor = System.Drawing.Color.Transparent;
            this.labelConnControllerName.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelConnControllerName.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelConnControllerName.ForeColor = System.Drawing.Color.DarkOrange;
            this.labelConnControllerName.Location = new System.Drawing.Point(5, 5);
            this.labelConnControllerName.Name = "labelConnControllerName";
            this.labelConnControllerName.Size = new System.Drawing.Size(205, 18);
            this.labelConnControllerName.TabIndex = 2;
            this.labelConnControllerName.Text = "connected to: ---";
            // 
            // panelLogger
            // 
            this.panelLogger.Controls.Add(this.statusTextBox);
            this.panelLogger.Controls.Add(this.pictureLogType);
            this.panelLogger.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLogger.Location = new System.Drawing.Point(5, 0);
            this.panelLogger.Name = "panelLogger";
            this.panelLogger.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.panelLogger.Size = new System.Drawing.Size(809, 28);
            this.panelLogger.TabIndex = 4;
            // 
            // statusTextBox
            // 
            this.statusTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.statusTextBox.BackColor = System.Drawing.Color.Gray;
            this.statusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.statusTextBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.statusTextBox.Font = new System.Drawing.Font("GOST Common", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.statusTextBox.ForeColor = System.Drawing.Color.White;
            this.statusTextBox.Location = new System.Drawing.Point(29, 5);
            this.statusTextBox.Multiline = false;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.statusTextBox.ShortcutsEnabled = false;
            this.statusTextBox.Size = new System.Drawing.Size(780, 18);
            this.statusTextBox.TabIndex = 1;
            this.statusTextBox.TabStop = false;
            this.statusTextBox.Text = "abbTools: app running  ";
            // 
            // pictureLogType
            // 
            this.pictureLogType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureLogType.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureLogType.ErrorImage = null;
            this.pictureLogType.InitialImage = null;
            this.pictureLogType.Location = new System.Drawing.Point(5, 5);
            this.pictureLogType.Name = "pictureLogType";
            this.pictureLogType.Size = new System.Drawing.Size(18, 18);
            this.pictureLogType.TabIndex = 3;
            this.pictureLogType.TabStop = false;
            // 
            // panelApp
            // 
            this.panelApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelApp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelApp.Controls.Add(this.appContainer);
            this.panelApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelApp.Location = new System.Drawing.Point(0, 65);
            this.panelApp.Margin = new System.Windows.Forms.Padding(0);
            this.panelApp.Name = "panelApp";
            this.panelApp.Size = new System.Drawing.Size(1010, 614);
            this.panelApp.TabIndex = 4;
            // 
            // robotListQMenu
            // 
            this.robotListQMenu.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.robotListQMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.robotListQMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.addToSavedToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.robotListQMenu.Name = "contextMenuStrip1";
            this.robotListQMenu.Size = new System.Drawing.Size(158, 108);
            this.robotListQMenu.Opening += new System.ComponentModel.CancelEventHandler(this.robotListQMenu_Opening);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.connectToolStripMenuItem.Text = "connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.disconnectToolStripMenuItem.Text = "disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // addToSavedToolStripMenuItem
            // 
            this.addToSavedToolStripMenuItem.Name = "addToSavedToolStripMenuItem";
            this.addToSavedToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.addToSavedToolStripMenuItem.Text = "add to saved";
            this.addToSavedToolStripMenuItem.Click += new System.EventHandler(this.addToSavedToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.removeToolStripMenuItem.Text = "remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // imagesStatus
            // 
            this.imagesStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesStatus.ImageStream")));
            this.imagesStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imagesStatus.Images.SetKeyName(0, "status_network.png");
            this.imagesStatus.Images.SetKeyName(1, "status_networkAvail.png");
            this.imagesStatus.Images.SetKeyName(2, "status_networkConn.png");
            this.imagesStatus.Images.SetKeyName(3, "status_networkDisconn.png");
            this.imagesStatus.Images.SetKeyName(4, "status_virtual.png");
            this.imagesStatus.Images.SetKeyName(5, "status_virtualAvail.png");
            this.imagesStatus.Images.SetKeyName(6, "status_virtualConn.png");
            this.imagesStatus.Images.SetKeyName(7, "status_virtualDisconn.png");
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "*.xml";
            this.saveDialog.Filter = "XML file|*.xml|All files|*.*";
            // 
            // openDialog
            // 
            this.openDialog.DefaultExt = "*.xml";
            this.openDialog.FileName = "openDialog";
            this.openDialog.Filter = "XML file|*.xml|All files|*.*";
            // 
            // imagesLogType
            // 
            this.imagesLogType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesLogType.ImageStream")));
            this.imagesLogType.TransparentColor = System.Drawing.Color.Transparent;
            this.imagesLogType.Images.SetKeyName(0, "log_info.png");
            this.imagesLogType.Images.SetKeyName(1, "log_warning.png");
            this.imagesLogType.Images.SetKeyName(2, "log_error.png");
            // 
            // appContainer
            // 
            this.appContainer.BackColor = System.Drawing.Color.Transparent;
            this.appContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appContainer.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.appContainer.ForeColor = System.Drawing.Color.Black;
            this.appContainer.IsSplitterFixed = true;
            this.appContainer.Location = new System.Drawing.Point(0, 0);
            this.appContainer.Name = "appContainer";
            // 
            // appContainer.Panel1
            // 
            this.appContainer.Panel1.Controls.Add(this.btnRobotListCollapse);
            this.appContainer.Panel1.Controls.Add(this.listViewRobots);
            this.appContainer.Panel1.Padding = new System.Windows.Forms.Padding(12, 12, 7, 12);
            this.appContainer.Panel1MinSize = 280;
            // 
            // appContainer.Panel2
            // 
            this.appContainer.Panel2.Controls.Add(this.tabActions);
            this.appContainer.Panel2.Padding = new System.Windows.Forms.Padding(7, 12, 7, 12);
            this.appContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.appContainer.Size = new System.Drawing.Size(1008, 612);
            this.appContainer.SplitterDistance = 280;
            this.appContainer.SplitterWidth = 1;
            this.appContainer.TabIndex = 1;
            // 
            // btnRobotListCollapse
            // 
            this.btnRobotListCollapse.BackColor = System.Drawing.Color.DarkOrange;
            this.btnRobotListCollapse.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRobotListCollapse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.btnRobotListCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRobotListCollapse.Font = new System.Drawing.Font("GOST Common", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRobotListCollapse.Location = new System.Drawing.Point(251, 12);
            this.btnRobotListCollapse.Name = "btnRobotListCollapse";
            this.btnRobotListCollapse.Size = new System.Drawing.Size(22, 588);
            this.btnRobotListCollapse.TabIndex = 1;
            this.btnRobotListCollapse.Text = "<<<";
            this.btnRobotListCollapse.UseVisualStyleBackColor = false;
            this.btnRobotListCollapse.Click += new System.EventHandler(this.btnRobotListCollapse_Click);
            // 
            // listViewRobots
            // 
            this.listViewRobots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRobots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.robotGroupColName,
            this.robotGroupColIP});
            this.listViewRobots.ContextMenuStrip = this.robotListQMenu;
            this.listViewRobots.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            listViewGroup1.Header = "network";
            listViewGroup1.Name = "robotsGroupNet";
            listViewGroup2.Header = "virtual";
            listViewGroup2.Name = "robotsGroupSim";
            listViewGroup3.Header = "saved";
            listViewGroup3.Name = "robotsGroupSaved";
            this.listViewRobots.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listViewRobots.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewRobots.HideSelection = false;
            listViewItem1.Checked = true;
            listViewItem1.Group = listViewGroup3;
            listViewItem1.StateImageIndex = 4;
            listViewItem2.Checked = true;
            listViewItem2.Group = listViewGroup3;
            listViewItem2.StateImageIndex = 4;
            this.listViewRobots.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listViewRobots.Location = new System.Drawing.Point(12, 12);
            this.listViewRobots.Margin = new System.Windows.Forms.Padding(0);
            this.listViewRobots.MultiSelect = false;
            this.listViewRobots.Name = "listViewRobots";
            this.listViewRobots.ShowItemToolTips = true;
            this.listViewRobots.Size = new System.Drawing.Size(234, 588);
            this.listViewRobots.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRobots.StateImageList = this.imagesStatus;
            this.listViewRobots.TabIndex = 0;
            this.listViewRobots.UseCompatibleStateImageBehavior = false;
            this.listViewRobots.View = System.Windows.Forms.View.Details;
            this.listViewRobots.DoubleClick += new System.EventHandler(this.listViewRobots_DoubleClick);
            this.listViewRobots.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewRobots_KeyDown);
            // 
            // robotGroupColName
            // 
            this.robotGroupColName.Text = "name";
            this.robotGroupColName.Width = 120;
            // 
            // robotGroupColIP
            // 
            this.robotGroupColIP.Text = "ip address";
            this.robotGroupColIP.Width = 110;
            // 
            // tabActions
            // 
            this.tabActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabActions.Controls.Add(this.actionDashboard);
            this.tabActions.Controls.Add(this.actionRemotePC);
            this.tabActions.Controls.Add(this.actionBackupManager);
            this.tabActions.Controls.Add(this.actionWinIPC);
            this.tabActions.ItemSize = new System.Drawing.Size(100, 23);
            this.tabActions.Location = new System.Drawing.Point(1, 12);
            this.tabActions.Margin = new System.Windows.Forms.Padding(100);
            this.tabActions.Name = "tabActions";
            this.tabActions.SelectedIndex = 0;
            this.tabActions.Size = new System.Drawing.Size(715, 588);
            this.tabActions.TabIndex = 0;
            // 
            // actionDashboard
            // 
            this.actionDashboard.BackColor = System.Drawing.Color.Transparent;
            this.actionDashboard.BackgroundImage = global::abbTools.Properties.Resources.windowMain_back;
            this.actionDashboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.actionDashboard.Location = new System.Drawing.Point(4, 27);
            this.actionDashboard.Name = "actionDashboard";
            this.actionDashboard.Size = new System.Drawing.Size(719, 557);
            this.actionDashboard.TabIndex = 2;
            this.actionDashboard.Text = "dashboard";
            // 
            // actionRemotePC
            // 
            this.actionRemotePC.AllowDrop = true;
            this.actionRemotePC.BackColor = System.Drawing.Color.White;
            this.actionRemotePC.BackgroundImage = global::abbTools.Properties.Resources.sidebar;
            this.actionRemotePC.Controls.Add(this.appRemotePC);
            this.actionRemotePC.Location = new System.Drawing.Point(4, 27);
            this.actionRemotePC.Margin = new System.Windows.Forms.Padding(0);
            this.actionRemotePC.Name = "actionRemotePC";
            this.actionRemotePC.Size = new System.Drawing.Size(719, 557);
            this.actionRemotePC.TabIndex = 0;
            this.actionRemotePC.Text = "remotePC";
            // 
            // appRemotePC
            // 
            this.appRemotePC.BackColor = System.Drawing.Color.White;
            this.appRemotePC.Location = new System.Drawing.Point(0, 0);
            this.appRemotePC.Margin = new System.Windows.Forms.Padding(0);
            this.appRemotePC.Name = "appRemotePC";
            this.appRemotePC.Size = new System.Drawing.Size(714, 557);
            this.appRemotePC.TabIndex = 0;
            // 
            // actionBackupManager
            // 
            this.actionBackupManager.BackColor = System.Drawing.Color.White;
            this.actionBackupManager.BackgroundImage = global::abbTools.Properties.Resources.sidebar;
            this.actionBackupManager.Controls.Add(this.appBackupManager);
            this.actionBackupManager.Location = new System.Drawing.Point(4, 27);
            this.actionBackupManager.Name = "actionBackupManager";
            this.actionBackupManager.Padding = new System.Windows.Forms.Padding(3);
            this.actionBackupManager.Size = new System.Drawing.Size(719, 557);
            this.actionBackupManager.TabIndex = 1;
            this.actionBackupManager.Text = "backupManager";
            // 
            // appBackupManager
            // 
            this.appBackupManager.BackColor = System.Drawing.Color.White;
            this.appBackupManager.Location = new System.Drawing.Point(0, 0);
            this.appBackupManager.Name = "appBackupManager";
            this.appBackupManager.Size = new System.Drawing.Size(714, 557);
            this.appBackupManager.TabIndex = 0;
            // 
            // actionWinIPC
            // 
            this.actionWinIPC.BackColor = System.Drawing.Color.White;
            this.actionWinIPC.BackgroundImage = global::abbTools.Properties.Resources.sidebar;
            this.actionWinIPC.Controls.Add(this.appWindowsIPC);
            this.actionWinIPC.Location = new System.Drawing.Point(4, 27);
            this.actionWinIPC.Name = "actionWinIPC";
            this.actionWinIPC.Size = new System.Drawing.Size(707, 557);
            this.actionWinIPC.TabIndex = 3;
            this.actionWinIPC.Text = "windowsIPC";
            // 
            // appWindowsIPC
            // 
            this.appWindowsIPC.BackColor = System.Drawing.Color.White;
            this.appWindowsIPC.Location = new System.Drawing.Point(0, 0);
            this.appWindowsIPC.Name = "appWindowsIPC";
            this.appWindowsIPC.Size = new System.Drawing.Size(714, 557);
            this.appWindowsIPC.TabIndex = 0;
            // 
            // windowMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1010, 709);
            this.Controls.Add(this.panelApp);
            this.Controls.Add(this.panelStatusBar);
            this.Controls.Add(this.panelMainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuBar;
            this.Name = "windowMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "abbTools";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.windowMain_FormClosed);
            this.Load += new System.EventHandler(this.windowMain_Load);
            this.Resize += new System.EventHandler(this.mainWindow_Resize);
            this.notifyIconQMenu.ResumeLayout(false);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.panelMainMenu.ResumeLayout(false);
            this.panelMainMenu.PerformLayout();
            this.panelStatusBar.ResumeLayout(false);
            this.panelConnStatus.ResumeLayout(false);
            this.panelLogger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogType)).EndInit();
            this.panelApp.ResumeLayout(false);
            this.robotListQMenu.ResumeLayout(false);
            this.appContainer.Panel1.ResumeLayout(false);
            this.appContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.appContainer)).EndInit();
            this.appContainer.ResumeLayout(false);
            this.tabActions.ResumeLayout(false);
            this.actionRemotePC.ResumeLayout(false);
            this.actionBackupManager.ResumeLayout(false);
            this.actionWinIPC.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconQMenu;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem minimizeToolStripMenuItem;
        private System.Windows.Forms.Panel panelMainMenu;
        private System.Windows.Forms.Panel panelStatusBar;
        private System.Windows.Forms.Panel panelApp;
        private System.Windows.Forms.Label labelAppTitle;
        private System.Windows.Forms.ListView listViewRobots;
        private System.Windows.Forms.ColumnHeader robotGroupColName;
        private System.Windows.Forms.ColumnHeader robotGroupColIP;
        private System.Windows.Forms.ContextMenuStrip robotListQMenu;
        private System.Windows.Forms.ToolStripMenuItem addToSavedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ImageList imagesStatus;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.SplitContainer appContainer;
        private System.Windows.Forms.Button btnRobotListCollapse;
        private System.Windows.Forms.TabControl tabActions;
        private System.Windows.Forms.TabPage actionRemotePC;
        private System.Windows.Forms.TabPage actionBackupManager;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.TabPage actionDashboard;
        private appRemoteABB appRemotePC;
        private System.Windows.Forms.Label labelConnControllerName;
        private System.Windows.Forms.Panel panelLogger;
        private System.Windows.Forms.PictureBox pictureLogType;
        private System.Windows.Forms.Panel panelConnStatus;
        private System.Windows.Forms.ImageList imagesLogType;
        private System.Windows.Forms.TabPage actionWinIPC;
        private AppWindowsIPC.appWindowsIPC appWindowsIPC;
        private AppBackupManager.appBackupManager appBackupManager;
    }
}

