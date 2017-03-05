namespace abbTools.AppWindowsIPC
{
    partial class appWindowsIPC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textMsgToSend = new System.Windows.Forms.TextBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.groupRemoteSignals = new System.Windows.Forms.GroupBox();
            this.radioSigTo0 = new System.Windows.Forms.RadioButton();
            this.radioSigTo1 = new System.Windows.Forms.RadioButton();
            this.buttonUpdateSignals = new System.Windows.Forms.Button();
            this.panelLoading = new System.Windows.Forms.Panel();
            this.labelLoadSignals = new System.Windows.Forms.Label();
            this.listRobotSignals = new System.Windows.Forms.CheckedListBox();
            this.groupNamedPipeClient = new System.Windows.Forms.GroupBox();
            this.groupClientTestMessages = new System.Windows.Forms.GroupBox();
            this.panelTextMessage = new System.Windows.Forms.Panel();
            this.groupClientControl = new System.Windows.Forms.GroupBox();
            this.buttonClientON = new System.Windows.Forms.Button();
            this.buttonClientOFF = new System.Windows.Forms.Button();
            this.groupClientSettings = new System.Windows.Forms.GroupBox();
            this.checkAutoOpen = new System.Windows.Forms.CheckBox();
            this.labelServerName = new System.Windows.Forms.Label();
            this.checkAutoReconnect = new System.Windows.Forms.CheckBox();
            this.textServerName = new System.Windows.Forms.TextBox();
            this.groupMessages = new System.Windows.Forms.GroupBox();
            this.labelManualMessage = new System.Windows.Forms.Label();
            this.textManualMessage = new System.Windows.Forms.TextBox();
            this.btnAddManualMessage = new System.Windows.Forms.Button();
            this.checkAutoFillMessages = new System.Windows.Forms.CheckBox();
            this.listBoxAllMessages = new System.Windows.Forms.ListBox();
            this.listBoxMsgMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupWatchMsg = new System.Windows.Forms.GroupBox();
            this.listMessagesWatch = new System.Windows.Forms.ListView();
            this.watchHeaderServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.watchHeaderMsg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.watchHeaderSig = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.watchHeaderState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonMsgRemove = new System.Windows.Forms.Button();
            this.buttonMsgModify = new System.Windows.Forms.Button();
            this.buttonMsgNew = new System.Windows.Forms.Button();
            this.backThread = new System.ComponentModel.BackgroundWorker();
            this.groupRemoteSignals.SuspendLayout();
            this.panelLoading.SuspendLayout();
            this.groupNamedPipeClient.SuspendLayout();
            this.groupClientTestMessages.SuspendLayout();
            this.panelTextMessage.SuspendLayout();
            this.groupClientControl.SuspendLayout();
            this.groupClientSettings.SuspendLayout();
            this.groupMessages.SuspendLayout();
            this.listBoxMsgMenu.SuspendLayout();
            this.groupWatchMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // textMsgToSend
            // 
            this.textMsgToSend.BackColor = System.Drawing.Color.PapayaWhip;
            this.textMsgToSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textMsgToSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textMsgToSend.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textMsgToSend.Location = new System.Drawing.Point(10, 9);
            this.textMsgToSend.Name = "textMsgToSend";
            this.textMsgToSend.Size = new System.Drawing.Size(184, 16);
            this.textMsgToSend.TabIndex = 1;
            this.textMsgToSend.Text = "test message";
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.BackColor = System.Drawing.Color.Silver;
            this.btnSendMsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnSendMsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnSendMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMsg.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnSendMsg.ForeColor = System.Drawing.Color.Black;
            this.btnSendMsg.Location = new System.Drawing.Point(16, 68);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(206, 27);
            this.btnSendMsg.TabIndex = 2;
            this.btnSendMsg.Text = "send";
            this.btnSendMsg.UseVisualStyleBackColor = false;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // groupRemoteSignals
            // 
            this.groupRemoteSignals.Controls.Add(this.radioSigTo0);
            this.groupRemoteSignals.Controls.Add(this.radioSigTo1);
            this.groupRemoteSignals.Controls.Add(this.buttonUpdateSignals);
            this.groupRemoteSignals.Controls.Add(this.panelLoading);
            this.groupRemoteSignals.Controls.Add(this.listRobotSignals);
            this.groupRemoteSignals.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRemoteSignals.Location = new System.Drawing.Point(534, 7);
            this.groupRemoteSignals.Name = "groupRemoteSignals";
            this.groupRemoteSignals.Size = new System.Drawing.Size(268, 318);
            this.groupRemoteSignals.TabIndex = 15;
            this.groupRemoteSignals.TabStop = false;
            this.groupRemoteSignals.Text = " DIGITAL OUTPUTS     ";
            // 
            // radioSigTo0
            // 
            this.radioSigTo0.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioSigTo0.Location = new System.Drawing.Point(136, 277);
            this.radioSigTo0.Name = "radioSigTo0";
            this.radioSigTo0.Size = new System.Drawing.Size(103, 22);
            this.radioSigTo0.TabIndex = 9;
            this.radioSigTo0.TabStop = true;
            this.radioSigTo0.Text = "set to \"0\"";
            this.radioSigTo0.UseVisualStyleBackColor = true;
            // 
            // radioSigTo1
            // 
            this.radioSigTo1.Checked = true;
            this.radioSigTo1.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioSigTo1.Location = new System.Drawing.Point(30, 278);
            this.radioSigTo1.Name = "radioSigTo1";
            this.radioSigTo1.Size = new System.Drawing.Size(110, 21);
            this.radioSigTo1.TabIndex = 8;
            this.radioSigTo1.TabStop = true;
            this.radioSigTo1.Text = "set to \"1\"";
            this.radioSigTo1.UseVisualStyleBackColor = true;
            // 
            // buttonUpdateSignals
            // 
            this.buttonUpdateSignals.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonUpdateSignals.Location = new System.Drawing.Point(191, 1);
            this.buttonUpdateSignals.Name = "buttonUpdateSignals";
            this.buttonUpdateSignals.Size = new System.Drawing.Size(66, 25);
            this.buttonUpdateSignals.TabIndex = 6;
            this.buttonUpdateSignals.Text = "update";
            this.buttonUpdateSignals.UseVisualStyleBackColor = true;
            this.buttonUpdateSignals.Click += new System.EventHandler(this.buttonUpdateSignals_Click);
            // 
            // panelLoading
            // 
            this.panelLoading.BackColor = System.Drawing.Color.Gold;
            this.panelLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLoading.Controls.Add(this.labelLoadSignals);
            this.panelLoading.Location = new System.Drawing.Point(34, 98);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Padding = new System.Windows.Forms.Padding(5, 25, 5, 25);
            this.panelLoading.Size = new System.Drawing.Size(200, 109);
            this.panelLoading.TabIndex = 7;
            // 
            // labelLoadSignals
            // 
            this.labelLoadSignals.BackColor = System.Drawing.Color.Transparent;
            this.labelLoadSignals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoadSignals.Location = new System.Drawing.Point(5, 25);
            this.labelLoadSignals.Name = "labelLoadSignals";
            this.labelLoadSignals.Size = new System.Drawing.Size(188, 57);
            this.labelLoadSignals.TabIndex = 0;
            this.labelLoadSignals.Text = "update signals...";
            this.labelLoadSignals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listRobotSignals
            // 
            this.listRobotSignals.BackColor = System.Drawing.Color.Silver;
            this.listRobotSignals.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listRobotSignals.FormattingEnabled = true;
            this.listRobotSignals.Location = new System.Drawing.Point(16, 32);
            this.listRobotSignals.Name = "listRobotSignals";
            this.listRobotSignals.Size = new System.Drawing.Size(239, 238);
            this.listRobotSignals.TabIndex = 2;
            this.listRobotSignals.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listRobotSignals_ItemCheck);
            // 
            // groupNamedPipeClient
            // 
            this.groupNamedPipeClient.Controls.Add(this.groupClientTestMessages);
            this.groupNamedPipeClient.Controls.Add(this.groupClientControl);
            this.groupNamedPipeClient.Controls.Add(this.groupClientSettings);
            this.groupNamedPipeClient.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupNamedPipeClient.Location = new System.Drawing.Point(8, 7);
            this.groupNamedPipeClient.Name = "groupNamedPipeClient";
            this.groupNamedPipeClient.Size = new System.Drawing.Size(264, 318);
            this.groupNamedPipeClient.TabIndex = 16;
            this.groupNamedPipeClient.TabStop = false;
            this.groupNamedPipeClient.Text = "NAMED PIPE CLIENT";
            // 
            // groupClientTestMessages
            // 
            this.groupClientTestMessages.Controls.Add(this.panelTextMessage);
            this.groupClientTestMessages.Controls.Add(this.btnSendMsg);
            this.groupClientTestMessages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupClientTestMessages.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupClientTestMessages.Location = new System.Drawing.Point(13, 199);
            this.groupClientTestMessages.Name = "groupClientTestMessages";
            this.groupClientTestMessages.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupClientTestMessages.Size = new System.Drawing.Size(239, 108);
            this.groupClientTestMessages.TabIndex = 19;
            this.groupClientTestMessages.TabStop = false;
            this.groupClientTestMessages.Text = "test messages";
            // 
            // panelTextMessage
            // 
            this.panelTextMessage.BackColor = System.Drawing.Color.PapayaWhip;
            this.panelTextMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTextMessage.Controls.Add(this.textMsgToSend);
            this.panelTextMessage.Location = new System.Drawing.Point(16, 25);
            this.panelTextMessage.Name = "panelTextMessage";
            this.panelTextMessage.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.panelTextMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelTextMessage.Size = new System.Drawing.Size(206, 37);
            this.panelTextMessage.TabIndex = 3;
            // 
            // groupClientControl
            // 
            this.groupClientControl.Controls.Add(this.buttonClientON);
            this.groupClientControl.Controls.Add(this.buttonClientOFF);
            this.groupClientControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupClientControl.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupClientControl.Location = new System.Drawing.Point(13, 128);
            this.groupClientControl.Name = "groupClientControl";
            this.groupClientControl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupClientControl.Size = new System.Drawing.Size(239, 67);
            this.groupClientControl.TabIndex = 18;
            this.groupClientControl.TabStop = false;
            this.groupClientControl.Text = "client control";
            // 
            // buttonClientON
            // 
            this.buttonClientON.BackColor = System.Drawing.Color.Chartreuse;
            this.buttonClientON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClientON.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonClientON.Location = new System.Drawing.Point(16, 24);
            this.buttonClientON.Name = "buttonClientON";
            this.buttonClientON.Size = new System.Drawing.Size(100, 29);
            this.buttonClientON.TabIndex = 5;
            this.buttonClientON.Text = "on";
            this.buttonClientON.UseVisualStyleBackColor = false;
            this.buttonClientON.Click += new System.EventHandler(this.buttonClientON_Click);
            // 
            // buttonClientOFF
            // 
            this.buttonClientOFF.BackColor = System.Drawing.Color.Silver;
            this.buttonClientOFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClientOFF.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonClientOFF.Location = new System.Drawing.Point(122, 24);
            this.buttonClientOFF.Name = "buttonClientOFF";
            this.buttonClientOFF.Size = new System.Drawing.Size(100, 29);
            this.buttonClientOFF.TabIndex = 6;
            this.buttonClientOFF.Text = "off";
            this.buttonClientOFF.UseVisualStyleBackColor = false;
            this.buttonClientOFF.Click += new System.EventHandler(this.buttonClientOFF_Click);
            // 
            // groupClientSettings
            // 
            this.groupClientSettings.BackColor = System.Drawing.Color.White;
            this.groupClientSettings.Controls.Add(this.checkAutoOpen);
            this.groupClientSettings.Controls.Add(this.labelServerName);
            this.groupClientSettings.Controls.Add(this.checkAutoReconnect);
            this.groupClientSettings.Controls.Add(this.textServerName);
            this.groupClientSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupClientSettings.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupClientSettings.ForeColor = System.Drawing.Color.Black;
            this.groupClientSettings.Location = new System.Drawing.Point(13, 25);
            this.groupClientSettings.Name = "groupClientSettings";
            this.groupClientSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupClientSettings.Size = new System.Drawing.Size(239, 100);
            this.groupClientSettings.TabIndex = 17;
            this.groupClientSettings.TabStop = false;
            this.groupClientSettings.Text = "settings";
            // 
            // checkAutoOpen
            // 
            this.checkAutoOpen.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkAutoOpen.Location = new System.Drawing.Point(126, 67);
            this.checkAutoOpen.Name = "checkAutoOpen";
            this.checkAutoOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkAutoOpen.Size = new System.Drawing.Size(101, 22);
            this.checkAutoOpen.TabIndex = 7;
            this.checkAutoOpen.Text = "auto open";
            this.checkAutoOpen.UseVisualStyleBackColor = true;
            // 
            // labelServerName
            // 
            this.labelServerName.AutoSize = true;
            this.labelServerName.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelServerName.Location = new System.Drawing.Point(16, 18);
            this.labelServerName.Name = "labelServerName";
            this.labelServerName.Size = new System.Drawing.Size(81, 17);
            this.labelServerName.TabIndex = 6;
            this.labelServerName.Text = "server name";
            // 
            // checkAutoReconnect
            // 
            this.checkAutoReconnect.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkAutoReconnect.Location = new System.Drawing.Point(16, 67);
            this.checkAutoReconnect.Name = "checkAutoReconnect";
            this.checkAutoReconnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkAutoReconnect.Size = new System.Drawing.Size(106, 22);
            this.checkAutoReconnect.TabIndex = 4;
            this.checkAutoReconnect.Text = "auto recon";
            this.checkAutoReconnect.UseVisualStyleBackColor = true;
            // 
            // textServerName
            // 
            this.textServerName.BackColor = System.Drawing.Color.White;
            this.textServerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textServerName.Font = new System.Drawing.Font("GOST Common", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textServerName.Location = new System.Drawing.Point(16, 35);
            this.textServerName.Name = "textServerName";
            this.textServerName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textServerName.Size = new System.Drawing.Size(206, 28);
            this.textServerName.TabIndex = 5;
            this.textServerName.TextChanged += new System.EventHandler(this.textServerName_TextChanged);
            // 
            // groupMessages
            // 
            this.groupMessages.Controls.Add(this.labelManualMessage);
            this.groupMessages.Controls.Add(this.textManualMessage);
            this.groupMessages.Controls.Add(this.btnAddManualMessage);
            this.groupMessages.Controls.Add(this.checkAutoFillMessages);
            this.groupMessages.Controls.Add(this.listBoxAllMessages);
            this.groupMessages.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupMessages.Location = new System.Drawing.Point(283, 7);
            this.groupMessages.Name = "groupMessages";
            this.groupMessages.Size = new System.Drawing.Size(240, 318);
            this.groupMessages.TabIndex = 17;
            this.groupMessages.TabStop = false;
            this.groupMessages.Text = "MESSAGES";
            // 
            // labelManualMessage
            // 
            this.labelManualMessage.AutoSize = true;
            this.labelManualMessage.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelManualMessage.Location = new System.Drawing.Point(13, 268);
            this.labelManualMessage.Name = "labelManualMessage";
            this.labelManualMessage.Size = new System.Drawing.Size(83, 17);
            this.labelManualMessage.TabIndex = 4;
            this.labelManualMessage.Text = "add message";
            // 
            // textManualMessage
            // 
            this.textManualMessage.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textManualMessage.Location = new System.Drawing.Point(16, 284);
            this.textManualMessage.Name = "textManualMessage";
            this.textManualMessage.Size = new System.Drawing.Size(125, 23);
            this.textManualMessage.TabIndex = 3;
            // 
            // btnAddManualMessage
            // 
            this.btnAddManualMessage.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnAddManualMessage.Location = new System.Drawing.Point(147, 284);
            this.btnAddManualMessage.Name = "btnAddManualMessage";
            this.btnAddManualMessage.Size = new System.Drawing.Size(75, 23);
            this.btnAddManualMessage.TabIndex = 2;
            this.btnAddManualMessage.Text = "add";
            this.btnAddManualMessage.UseVisualStyleBackColor = true;
            this.btnAddManualMessage.Click += new System.EventHandler(this.btnAddManualMessage_Click);
            // 
            // checkAutoFillMessages
            // 
            this.checkAutoFillMessages.AutoSize = true;
            this.checkAutoFillMessages.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkAutoFillMessages.Location = new System.Drawing.Point(16, 32);
            this.checkAutoFillMessages.Name = "checkAutoFillMessages";
            this.checkAutoFillMessages.Size = new System.Drawing.Size(189, 21);
            this.checkAutoFillMessages.TabIndex = 1;
            this.checkAutoFillMessages.Text = "fill with incoming commands";
            this.checkAutoFillMessages.UseVisualStyleBackColor = true;
            // 
            // listBoxAllMessages
            // 
            this.listBoxAllMessages.ContextMenuStrip = this.listBoxMsgMenu;
            this.listBoxAllMessages.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxAllMessages.FormattingEnabled = true;
            this.listBoxAllMessages.ItemHeight = 16;
            this.listBoxAllMessages.Location = new System.Drawing.Point(16, 56);
            this.listBoxAllMessages.Name = "listBoxAllMessages";
            this.listBoxAllMessages.Size = new System.Drawing.Size(206, 212);
            this.listBoxAllMessages.TabIndex = 0;
            this.listBoxAllMessages.SelectedIndexChanged += new System.EventHandler(this.listBoxAllMessages_SelectedIndexChanged);
            // 
            // listBoxMsgMenu
            // 
            this.listBoxMsgMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.listBoxMsgMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeItemToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.listBoxMsgMenu.Name = "listBoxMsgMenu";
            this.listBoxMsgMenu.Size = new System.Drawing.Size(163, 52);
            this.listBoxMsgMenu.Opening += new System.ComponentModel.CancelEventHandler(this.listBoxMsgMenu_Opening);
            // 
            // removeItemToolStripMenuItem
            // 
            this.removeItemToolStripMenuItem.Name = "removeItemToolStripMenuItem";
            this.removeItemToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
            this.removeItemToolStripMenuItem.Text = "remove item";
            this.removeItemToolStripMenuItem.Click += new System.EventHandler(this.removeItemToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
            this.clearAllToolStripMenuItem.Text = "clear all";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // groupWatchMsg
            // 
            this.groupWatchMsg.Controls.Add(this.listMessagesWatch);
            this.groupWatchMsg.Controls.Add(this.buttonMsgRemove);
            this.groupWatchMsg.Controls.Add(this.buttonMsgModify);
            this.groupWatchMsg.Controls.Add(this.buttonMsgNew);
            this.groupWatchMsg.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupWatchMsg.Location = new System.Drawing.Point(8, 334);
            this.groupWatchMsg.Name = "groupWatchMsg";
            this.groupWatchMsg.Size = new System.Drawing.Size(794, 212);
            this.groupWatchMsg.TabIndex = 18;
            this.groupWatchMsg.TabStop = false;
            this.groupWatchMsg.Text = "WATCHING";
            // 
            // listMessagesWatch
            // 
            this.listMessagesWatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMessagesWatch.BackColor = System.Drawing.Color.Silver;
            this.listMessagesWatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listMessagesWatch.CheckBoxes = true;
            this.listMessagesWatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.watchHeaderServer,
            this.watchHeaderMsg,
            this.watchHeaderSig,
            this.watchHeaderState});
            this.listMessagesWatch.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listMessagesWatch.FullRowSelect = true;
            this.listMessagesWatch.GridLines = true;
            this.listMessagesWatch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listMessagesWatch.LabelWrap = false;
            this.listMessagesWatch.Location = new System.Drawing.Point(13, 31);
            this.listMessagesWatch.MultiSelect = false;
            this.listMessagesWatch.Name = "listMessagesWatch";
            this.listMessagesWatch.ShowGroups = false;
            this.listMessagesWatch.ShowItemToolTips = true;
            this.listMessagesWatch.Size = new System.Drawing.Size(640, 169);
            this.listMessagesWatch.TabIndex = 22;
            this.listMessagesWatch.UseCompatibleStateImageBehavior = false;
            this.listMessagesWatch.View = System.Windows.Forms.View.Details;
            this.listMessagesWatch.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listMessagesWatch_ItemChecked);
            // 
            // watchHeaderServer
            // 
            this.watchHeaderServer.Text = "server";
            this.watchHeaderServer.Width = 127;
            // 
            // watchHeaderMsg
            // 
            this.watchHeaderMsg.Text = "message";
            this.watchHeaderMsg.Width = 130;
            // 
            // watchHeaderSig
            // 
            this.watchHeaderSig.Text = "signal";
            this.watchHeaderSig.Width = 250;
            // 
            // watchHeaderState
            // 
            this.watchHeaderState.Text = "state";
            this.watchHeaderState.Width = 50;
            // 
            // buttonMsgRemove
            // 
            this.buttonMsgRemove.BackColor = System.Drawing.Color.Silver;
            this.buttonMsgRemove.Enabled = false;
            this.buttonMsgRemove.FlatAppearance.BorderSize = 2;
            this.buttonMsgRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonMsgRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMsgRemove.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonMsgRemove.Location = new System.Drawing.Point(678, 153);
            this.buttonMsgRemove.Name = "buttonMsgRemove";
            this.buttonMsgRemove.Size = new System.Drawing.Size(103, 47);
            this.buttonMsgRemove.TabIndex = 21;
            this.buttonMsgRemove.Text = "DELETE";
            this.buttonMsgRemove.UseVisualStyleBackColor = false;
            // 
            // buttonMsgModify
            // 
            this.buttonMsgModify.BackColor = System.Drawing.Color.Silver;
            this.buttonMsgModify.Enabled = false;
            this.buttonMsgModify.FlatAppearance.BorderSize = 2;
            this.buttonMsgModify.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonMsgModify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMsgModify.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonMsgModify.Location = new System.Drawing.Point(678, 92);
            this.buttonMsgModify.Name = "buttonMsgModify";
            this.buttonMsgModify.Size = new System.Drawing.Size(103, 47);
            this.buttonMsgModify.TabIndex = 20;
            this.buttonMsgModify.Text = "MODIFY";
            this.buttonMsgModify.UseVisualStyleBackColor = false;
            // 
            // buttonMsgNew
            // 
            this.buttonMsgNew.BackColor = System.Drawing.Color.Silver;
            this.buttonMsgNew.Enabled = false;
            this.buttonMsgNew.FlatAppearance.BorderSize = 2;
            this.buttonMsgNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.YellowGreen;
            this.buttonMsgNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMsgNew.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonMsgNew.Location = new System.Drawing.Point(678, 31);
            this.buttonMsgNew.Name = "buttonMsgNew";
            this.buttonMsgNew.Size = new System.Drawing.Size(103, 47);
            this.buttonMsgNew.TabIndex = 19;
            this.buttonMsgNew.Text = "NEW";
            this.buttonMsgNew.UseVisualStyleBackColor = false;
            this.buttonMsgNew.Click += new System.EventHandler(this.buttonMsgNew_Click);
            // 
            // backThread
            // 
            this.backThread.WorkerReportsProgress = true;
            this.backThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backThread_DoWork);
            this.backThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backThread_ProgressChanged);
            this.backThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backThread_RunWorkerCompleted);
            // 
            // appWindowsIPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupWatchMsg);
            this.Controls.Add(this.groupMessages);
            this.Controls.Add(this.groupNamedPipeClient);
            this.Controls.Add(this.groupRemoteSignals);
            this.Name = "appWindowsIPC";
            this.Size = new System.Drawing.Size(807, 565);
            this.groupRemoteSignals.ResumeLayout(false);
            this.panelLoading.ResumeLayout(false);
            this.groupNamedPipeClient.ResumeLayout(false);
            this.groupClientTestMessages.ResumeLayout(false);
            this.panelTextMessage.ResumeLayout(false);
            this.panelTextMessage.PerformLayout();
            this.groupClientControl.ResumeLayout(false);
            this.groupClientSettings.ResumeLayout(false);
            this.groupClientSettings.PerformLayout();
            this.groupMessages.ResumeLayout(false);
            this.groupMessages.PerformLayout();
            this.listBoxMsgMenu.ResumeLayout(false);
            this.groupWatchMsg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textMsgToSend;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.GroupBox groupRemoteSignals;
        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label labelLoadSignals;
        private System.Windows.Forms.Button buttonUpdateSignals;
        private System.Windows.Forms.CheckedListBox listRobotSignals;
        private System.Windows.Forms.GroupBox groupNamedPipeClient;
        private System.Windows.Forms.Panel panelTextMessage;
        private System.Windows.Forms.TextBox textServerName;
        private System.Windows.Forms.CheckBox checkAutoReconnect;
        private System.Windows.Forms.Label labelServerName;
        private System.Windows.Forms.Button buttonClientOFF;
        private System.Windows.Forms.Button buttonClientON;
        private System.Windows.Forms.GroupBox groupClientSettings;
        private System.Windows.Forms.GroupBox groupClientTestMessages;
        private System.Windows.Forms.GroupBox groupClientControl;
        private System.Windows.Forms.CheckBox checkAutoOpen;
        private System.Windows.Forms.GroupBox groupMessages;
        private System.Windows.Forms.TextBox textManualMessage;
        private System.Windows.Forms.Button btnAddManualMessage;
        private System.Windows.Forms.CheckBox checkAutoFillMessages;
        private System.Windows.Forms.ListBox listBoxAllMessages;
        private System.Windows.Forms.RadioButton radioSigTo0;
        private System.Windows.Forms.RadioButton radioSigTo1;
        private System.Windows.Forms.GroupBox groupWatchMsg;
        private System.Windows.Forms.Button buttonMsgRemove;
        private System.Windows.Forms.Button buttonMsgModify;
        private System.Windows.Forms.Button buttonMsgNew;
        private System.Windows.Forms.ListView listMessagesWatch;
        private System.Windows.Forms.ColumnHeader watchHeaderServer;
        private System.Windows.Forms.ColumnHeader watchHeaderMsg;
        private System.Windows.Forms.ColumnHeader watchHeaderSig;
        private System.Windows.Forms.ColumnHeader watchHeaderState;
        private System.Windows.Forms.ContextMenuStrip listBoxMsgMenu;
        private System.Windows.Forms.ToolStripMenuItem removeItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.Label labelManualMessage;
        private System.ComponentModel.BackgroundWorker backThread;
    }
}
