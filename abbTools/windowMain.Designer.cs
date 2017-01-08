namespace abbTools
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMainMenu = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStatusBar = new System.Windows.Forms.Panel();
            this.panelApp = new System.Windows.Forms.Panel();
            this.buttonRobotToggle = new System.Windows.Forms.Button();
            this.panelRobots = new System.Windows.Forms.Panel();
            this.listViewRobots = new System.Windows.Forms.ListView();
            this.robotGroupColName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.robotGroupColIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.robotListQMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToSavedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagesStatus = new System.Windows.Forms.ImageList(this.components);
            this.notifyIconQMenu.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.panelMainMenu.SuspendLayout();
            this.panelApp.SuspendLayout();
            this.panelRobots.SuspendLayout();
            this.robotListQMenu.SuspendLayout();
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
            this.menuBar.Font = new System.Drawing.Font("Calibri", 10.5F);
            this.menuBar.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.toolsToolStripMenuItem1,
            this.exitToolStripMenuItem1,
            this.minimizeToolStripMenuItem,
            this.helpToolStripMenuItem1});
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
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(177, 26);
            this.newToolStripMenuItem1.Text = "&New";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem1.Image")));
            this.openToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.openToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(177, 26);
            this.openToolStripMenuItem1.Text = "&Open";
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
            this.saveToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(177, 26);
            this.saveToolStripMenuItem1.Text = "&Save";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(174, 6);
            // 
            // exitToolStripMenuItem2
            // 
            this.exitToolStripMenuItem2.Name = "exitToolStripMenuItem2";
            this.exitToolStripMenuItem2.Size = new System.Drawing.Size(177, 26);
            this.exitToolStripMenuItem2.Text = "E&xit";
            // 
            // toolsToolStripMenuItem1
            // 
            this.toolsToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem1.Image = global::abbTools.Properties.Resources.mainMenu_settings;
            this.toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
            this.toolsToolStripMenuItem1.Size = new System.Drawing.Size(62, 60);
            this.toolsToolStripMenuItem1.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(162, 26);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(162, 26);
            this.optionsToolStripMenuItem.Text = "&Options";
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
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
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
            // panelMainMenu
            // 
            this.panelMainMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMainMenu.Controls.Add(this.label1);
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
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DarkOrange;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(466, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "abbTools";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelStatusBar
            // 
            this.panelStatusBar.BackColor = System.Drawing.Color.Gray;
            this.panelStatusBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatusBar.Location = new System.Drawing.Point(0, 679);
            this.panelStatusBar.Name = "panelStatusBar";
            this.panelStatusBar.Size = new System.Drawing.Size(1010, 30);
            this.panelStatusBar.TabIndex = 3;
            // 
            // panelApp
            // 
            this.panelApp.BackgroundImage = global::abbTools.Properties.Resources.windowMain_back;
            this.panelApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelApp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelApp.Controls.Add(this.buttonRobotToggle);
            this.panelApp.Controls.Add(this.panelRobots);
            this.panelApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelApp.Location = new System.Drawing.Point(0, 65);
            this.panelApp.Margin = new System.Windows.Forms.Padding(0);
            this.panelApp.Name = "panelApp";
            this.panelApp.Size = new System.Drawing.Size(1010, 614);
            this.panelApp.TabIndex = 4;
            // 
            // buttonRobotToggle
            // 
            this.buttonRobotToggle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonRobotToggle.BackgroundImage")));
            this.buttonRobotToggle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRobotToggle.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonRobotToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRobotToggle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonRobotToggle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRobotToggle.Location = new System.Drawing.Point(257, 11);
            this.buttonRobotToggle.Name = "buttonRobotToggle";
            this.buttonRobotToggle.Size = new System.Drawing.Size(68, 65);
            this.buttonRobotToggle.TabIndex = 1;
            this.buttonRobotToggle.UseVisualStyleBackColor = true;
            this.buttonRobotToggle.Click += new System.EventHandler(this.buttonRobotToggle_Click);
            // 
            // panelRobots
            // 
            this.panelRobots.BackColor = System.Drawing.Color.White;
            this.panelRobots.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRobots.Controls.Add(this.listViewRobots);
            this.panelRobots.Location = new System.Drawing.Point(-2, -2);
            this.panelRobots.Margin = new System.Windows.Forms.Padding(0);
            this.panelRobots.Name = "panelRobots";
            this.panelRobots.Padding = new System.Windows.Forms.Padding(12);
            this.panelRobots.Size = new System.Drawing.Size(260, 615);
            this.panelRobots.TabIndex = 0;
            // 
            // listViewRobots
            // 
            this.listViewRobots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.robotGroupColName,
            this.robotGroupColIP});
            this.listViewRobots.ContextMenuStrip = this.robotListQMenu;
            this.listViewRobots.Dock = System.Windows.Forms.DockStyle.Fill;
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
            listViewItem1.StateImageIndex = 1;
            listViewItem2.Checked = true;
            listViewItem2.Group = listViewGroup3;
            listViewItem2.StateImageIndex = 1;
            this.listViewRobots.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listViewRobots.Location = new System.Drawing.Point(12, 12);
            this.listViewRobots.Margin = new System.Windows.Forms.Padding(0);
            this.listViewRobots.MultiSelect = false;
            this.listViewRobots.Name = "listViewRobots";
            this.listViewRobots.ShowItemToolTips = true;
            this.listViewRobots.Size = new System.Drawing.Size(234, 589);
            this.listViewRobots.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewRobots.StateImageList = this.imagesStatus;
            this.listViewRobots.TabIndex = 0;
            this.listViewRobots.UseCompatibleStateImageBehavior = false;
            this.listViewRobots.View = System.Windows.Forms.View.Details;
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
            // robotListQMenu
            // 
            this.robotListQMenu.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.robotListQMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.robotListQMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToSavedToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.robotListQMenu.Name = "contextMenuStrip1";
            this.robotListQMenu.Size = new System.Drawing.Size(158, 56);
            this.robotListQMenu.Opening += new System.ComponentModel.CancelEventHandler(this.robotListQMenu_Opening);
            // 
            // addToSavedToolStripMenuItem
            // 
            this.addToSavedToolStripMenuItem.Name = "addToSavedToolStripMenuItem";
            this.addToSavedToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.addToSavedToolStripMenuItem.Text = "add to saved";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            this.removeToolStripMenuItem.Text = "remove";
            // 
            // imagesStatus
            // 
            this.imagesStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesStatus.ImageStream")));
            this.imagesStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imagesStatus.Images.SetKeyName(0, "status_network.png");
            this.imagesStatus.Images.SetKeyName(1, "status_virtual.png");
            this.imagesStatus.Images.SetKeyName(2, "status_networkConn.png");
            this.imagesStatus.Images.SetKeyName(3, "status_virtualConn.png");
            this.imagesStatus.Images.SetKeyName(4, "status_networkDisconn.png");
            this.imagesStatus.Images.SetKeyName(5, "status_virtualDisconn.png");
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
            this.panelApp.ResumeLayout(false);
            this.panelRobots.ResumeLayout(false);
            this.robotListQMenu.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem minimizeToolStripMenuItem;
        private System.Windows.Forms.Panel panelMainMenu;
        private System.Windows.Forms.Panel panelStatusBar;
        private System.Windows.Forms.Panel panelApp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelRobots;
        private System.Windows.Forms.ListView listViewRobots;
        private System.Windows.Forms.ColumnHeader robotGroupColName;
        private System.Windows.Forms.ColumnHeader robotGroupColIP;
        private System.Windows.Forms.Button buttonRobotToggle;
        private System.Windows.Forms.ContextMenuStrip robotListQMenu;
        private System.Windows.Forms.ToolStripMenuItem addToSavedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ImageList imagesStatus;
    }
}

