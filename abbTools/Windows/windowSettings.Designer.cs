namespace abbTools
{
    partial class windowSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(windowSettings));
            this.panelFooterButtons = new System.Windows.Forms.Panel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelWindowTitle = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabSettingsGeneral = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupWindowsIPC = new System.Windows.Forms.GroupBox();
            this.textDefaultServerName = new System.Windows.Forms.TextBox();
            this.labelDefaultServerName = new System.Windows.Forms.Label();
            this.checkFillMsg = new System.Windows.Forms.CheckBox();
            this.groupRemotePC = new System.Windows.Forms.GroupBox();
            this.checkRememberAppDir = new System.Windows.Forms.CheckBox();
            this.groupBackupManager = new System.Windows.Forms.GroupBox();
            this.checkUpdBackupTime = new System.Windows.Forms.CheckBox();
            this.checkCreateOutputDir = new System.Windows.Forms.CheckBox();
            this.labelSettingsApps = new System.Windows.Forms.Label();
            this.panelSettingsGeneral = new System.Windows.Forms.Panel();
            this.checkShowProjPath = new System.Windows.Forms.CheckBox();
            this.groupSignalRun = new System.Windows.Forms.GroupBox();
            this.radioSigSet0 = new System.Windows.Forms.RadioButton();
            this.labelSigName = new System.Windows.Forms.Label();
            this.radioSigSet1 = new System.Windows.Forms.RadioButton();
            this.textSigName = new System.Windows.Forms.TextBox();
            this.checkRunSigActive = new System.Windows.Forms.CheckBox();
            this.labelSettingsGeneral = new System.Windows.Forms.Label();
            this.checkLoadLastProject = new System.Windows.Forms.CheckBox();
            this.groupMinimizeMethod = new System.Windows.Forms.GroupBox();
            this.radioMinTray = new System.Windows.Forms.RadioButton();
            this.radioMinNotifyIcon = new System.Windows.Forms.RadioButton();
            this.checkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.tabSettingsMail = new System.Windows.Forms.TabPage();
            this.tabSettingsUser = new System.Windows.Forms.TabPage();
            this.imageTabs = new System.Windows.Forms.ImageList(this.components);
            this.panelTemplate = new System.Windows.Forms.Panel();
            this.textTo = new System.Windows.Forms.TextBox();
            this.textCC = new System.Windows.Forms.TextBox();
            this.textSubject = new System.Windows.Forms.TextBox();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.textUser = new System.Windows.Forms.TextBox();
            this.textPass = new System.Windows.Forms.TextBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textSMTP = new System.Windows.Forms.TextBox();
            this.groupMailSettings = new System.Windows.Forms.GroupBox();
            this.checkSSL = new System.Windows.Forms.CheckBox();
            this.buttonTestSend = new System.Windows.Forms.Button();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelCC = new System.Windows.Forms.Label();
            this.labelSubject = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPass = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelSMTP = new System.Windows.Forms.Label();
            this.checkMailActive = new System.Windows.Forms.CheckBox();
            this.panelFooterButtons.SuspendLayout();
            this.panelBackground.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabSettingsGeneral.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupWindowsIPC.SuspendLayout();
            this.groupRemotePC.SuspendLayout();
            this.groupBackupManager.SuspendLayout();
            this.panelSettingsGeneral.SuspendLayout();
            this.groupSignalRun.SuspendLayout();
            this.groupMinimizeMethod.SuspendLayout();
            this.tabSettingsMail.SuspendLayout();
            this.groupMailSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFooterButtons
            // 
            this.panelFooterButtons.BackColor = System.Drawing.Color.DarkOrange;
            this.panelFooterButtons.Controls.Add(this.buttonApply);
            this.panelFooterButtons.Controls.Add(this.buttonCancel);
            this.panelFooterButtons.Controls.Add(this.buttonOK);
            this.panelFooterButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooterButtons.Location = new System.Drawing.Point(0, 509);
            this.panelFooterButtons.Name = "panelFooterButtons";
            this.panelFooterButtons.Size = new System.Drawing.Size(583, 50);
            this.panelFooterButtons.TabIndex = 0;
            // 
            // buttonApply
            // 
            this.buttonApply.BackColor = System.Drawing.Color.DimGray;
            this.buttonApply.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.buttonApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonApply.Location = new System.Drawing.Point(225, 9);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(133, 34);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "apply";
            this.buttonApply.UseVisualStyleBackColor = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.DimGray;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonCancel.Location = new System.Drawing.Point(76, 9);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(133, 34);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.DimGray;
            this.buttonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.buttonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonOK.Location = new System.Drawing.Point(374, 9);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(133, 34);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "ok";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panelBackground
            // 
            this.panelBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBackground.Controls.Add(this.panelHeader);
            this.panelBackground.Controls.Add(this.tabSettings);
            this.panelBackground.Controls.Add(this.panelFooterButtons);
            this.panelBackground.Location = new System.Drawing.Point(197, 34);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(585, 561);
            this.panelBackground.TabIndex = 1;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DimGray;
            this.panelHeader.Controls.Add(this.labelWindowTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(583, 50);
            this.panelHeader.TabIndex = 2;
            // 
            // labelWindowTitle
            // 
            this.labelWindowTitle.BackColor = System.Drawing.Color.DarkOrange;
            this.labelWindowTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWindowTitle.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelWindowTitle.ForeColor = System.Drawing.Color.Black;
            this.labelWindowTitle.Location = new System.Drawing.Point(0, 0);
            this.labelWindowTitle.Name = "labelWindowTitle";
            this.labelWindowTitle.Size = new System.Drawing.Size(583, 50);
            this.labelWindowTitle.TabIndex = 0;
            this.labelWindowTitle.Text = "settings...";
            this.labelWindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.tabSettingsGeneral);
            this.tabSettings.Controls.Add(this.tabSettingsMail);
            this.tabSettings.Controls.Add(this.tabSettingsUser);
            this.tabSettings.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tabSettings.ImageList = this.imageTabs;
            this.tabSettings.ItemSize = new System.Drawing.Size(150, 50);
            this.tabSettings.Location = new System.Drawing.Point(17, 56);
            this.tabSettings.Multiline = true;
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.ShowToolTips = true;
            this.tabSettings.Size = new System.Drawing.Size(551, 447);
            this.tabSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSettings.TabIndex = 0;
            // 
            // tabSettingsGeneral
            // 
            this.tabSettingsGeneral.BackColor = System.Drawing.Color.White;
            this.tabSettingsGeneral.Controls.Add(this.panel1);
            this.tabSettingsGeneral.Controls.Add(this.panelSettingsGeneral);
            this.tabSettingsGeneral.ImageIndex = 1;
            this.tabSettingsGeneral.Location = new System.Drawing.Point(4, 54);
            this.tabSettingsGeneral.Name = "tabSettingsGeneral";
            this.tabSettingsGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingsGeneral.Size = new System.Drawing.Size(543, 389);
            this.tabSettingsGeneral.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupWindowsIPC);
            this.panel1.Controls.Add(this.groupRemotePC);
            this.panel1.Controls.Add(this.groupBackupManager);
            this.panel1.Controls.Add(this.labelSettingsApps);
            this.panel1.Location = new System.Drawing.Point(290, 22);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(239, 351);
            this.panel1.TabIndex = 7;
            // 
            // groupWindowsIPC
            // 
            this.groupWindowsIPC.Controls.Add(this.textDefaultServerName);
            this.groupWindowsIPC.Controls.Add(this.labelDefaultServerName);
            this.groupWindowsIPC.Controls.Add(this.checkFillMsg);
            this.groupWindowsIPC.Location = new System.Drawing.Point(26, 217);
            this.groupWindowsIPC.Name = "groupWindowsIPC";
            this.groupWindowsIPC.Size = new System.Drawing.Size(179, 108);
            this.groupWindowsIPC.TabIndex = 8;
            this.groupWindowsIPC.TabStop = false;
            this.groupWindowsIPC.Text = "windowsIPC";
            // 
            // textDefaultServerName
            // 
            this.textDefaultServerName.Location = new System.Drawing.Point(15, 72);
            this.textDefaultServerName.Name = "textDefaultServerName";
            this.textDefaultServerName.Size = new System.Drawing.Size(142, 23);
            this.textDefaultServerName.TabIndex = 1;
            // 
            // labelDefaultServerName
            // 
            this.labelDefaultServerName.AutoSize = true;
            this.labelDefaultServerName.Location = new System.Drawing.Point(12, 56);
            this.labelDefaultServerName.Name = "labelDefaultServerName";
            this.labelDefaultServerName.Size = new System.Drawing.Size(128, 17);
            this.labelDefaultServerName.TabIndex = 5;
            this.labelDefaultServerName.Text = "default server name";
            // 
            // checkFillMsg
            // 
            this.checkFillMsg.AutoSize = true;
            this.checkFillMsg.Location = new System.Drawing.Point(15, 26);
            this.checkFillMsg.Name = "checkFillMsg";
            this.checkFillMsg.Size = new System.Drawing.Size(108, 21);
            this.checkFillMsg.TabIndex = 0;
            this.checkFillMsg.Text = "fill messages";
            this.checkFillMsg.UseVisualStyleBackColor = true;
            // 
            // groupRemotePC
            // 
            this.groupRemotePC.Controls.Add(this.checkRememberAppDir);
            this.groupRemotePC.Location = new System.Drawing.Point(26, 134);
            this.groupRemotePC.Name = "groupRemotePC";
            this.groupRemotePC.Size = new System.Drawing.Size(179, 66);
            this.groupRemotePC.TabIndex = 7;
            this.groupRemotePC.TabStop = false;
            this.groupRemotePC.Text = "remotePC";
            // 
            // checkRememberAppDir
            // 
            this.checkRememberAppDir.AutoSize = true;
            this.checkRememberAppDir.Location = new System.Drawing.Point(15, 27);
            this.checkRememberAppDir.Name = "checkRememberAppDir";
            this.checkRememberAppDir.Size = new System.Drawing.Size(139, 21);
            this.checkRememberAppDir.TabIndex = 0;
            this.checkRememberAppDir.Text = "remember last app";
            this.checkRememberAppDir.UseVisualStyleBackColor = true;
            // 
            // groupBackupManager
            // 
            this.groupBackupManager.Controls.Add(this.checkUpdBackupTime);
            this.groupBackupManager.Controls.Add(this.checkCreateOutputDir);
            this.groupBackupManager.Location = new System.Drawing.Point(26, 30);
            this.groupBackupManager.Name = "groupBackupManager";
            this.groupBackupManager.Size = new System.Drawing.Size(179, 89);
            this.groupBackupManager.TabIndex = 6;
            this.groupBackupManager.TabStop = false;
            this.groupBackupManager.Text = "backupManager";
            // 
            // checkUpdBackupTime
            // 
            this.checkUpdBackupTime.AutoSize = true;
            this.checkUpdBackupTime.Location = new System.Drawing.Point(15, 53);
            this.checkUpdBackupTime.Name = "checkUpdBackupTime";
            this.checkUpdBackupTime.Size = new System.Drawing.Size(129, 21);
            this.checkUpdBackupTime.TabIndex = 1;
            this.checkUpdBackupTime.Text = "auto update time";
            this.checkUpdBackupTime.UseVisualStyleBackColor = true;
            // 
            // checkCreateOutputDir
            // 
            this.checkCreateOutputDir.AutoSize = true;
            this.checkCreateOutputDir.Location = new System.Drawing.Point(15, 26);
            this.checkCreateOutputDir.Name = "checkCreateOutputDir";
            this.checkCreateOutputDir.Size = new System.Drawing.Size(142, 21);
            this.checkCreateOutputDir.TabIndex = 0;
            this.checkCreateOutputDir.Text = "create output path";
            this.checkCreateOutputDir.UseVisualStyleBackColor = true;
            // 
            // labelSettingsApps
            // 
            this.labelSettingsApps.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSettingsApps.Font = new System.Drawing.Font("GOST Common", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSettingsApps.Location = new System.Drawing.Point(5, 5);
            this.labelSettingsApps.Name = "labelSettingsApps";
            this.labelSettingsApps.Size = new System.Drawing.Size(227, 22);
            this.labelSettingsApps.TabIndex = 5;
            this.labelSettingsApps.Text = "APPS";
            // 
            // panelSettingsGeneral
            // 
            this.panelSettingsGeneral.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelSettingsGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSettingsGeneral.Controls.Add(this.checkShowProjPath);
            this.panelSettingsGeneral.Controls.Add(this.groupSignalRun);
            this.panelSettingsGeneral.Controls.Add(this.labelSettingsGeneral);
            this.panelSettingsGeneral.Controls.Add(this.checkLoadLastProject);
            this.panelSettingsGeneral.Controls.Add(this.groupMinimizeMethod);
            this.panelSettingsGeneral.Controls.Add(this.checkAlwaysOnTop);
            this.panelSettingsGeneral.Location = new System.Drawing.Point(22, 22);
            this.panelSettingsGeneral.Name = "panelSettingsGeneral";
            this.panelSettingsGeneral.Padding = new System.Windows.Forms.Padding(5);
            this.panelSettingsGeneral.Size = new System.Drawing.Size(246, 351);
            this.panelSettingsGeneral.TabIndex = 3;
            // 
            // checkShowProjPath
            // 
            this.checkShowProjPath.AutoSize = true;
            this.checkShowProjPath.Location = new System.Drawing.Point(23, 65);
            this.checkShowProjPath.Name = "checkShowProjPath";
            this.checkShowProjPath.Size = new System.Drawing.Size(135, 21);
            this.checkShowProjPath.TabIndex = 1;
            this.checkShowProjPath.Text = "show project path";
            this.checkShowProjPath.UseVisualStyleBackColor = true;
            this.checkShowProjPath.CheckedChanged += new System.EventHandler(this.checkShowProjPath_CheckedChanged);
            // 
            // groupSignalRun
            // 
            this.groupSignalRun.Controls.Add(this.radioSigSet0);
            this.groupSignalRun.Controls.Add(this.labelSigName);
            this.groupSignalRun.Controls.Add(this.radioSigSet1);
            this.groupSignalRun.Controls.Add(this.textSigName);
            this.groupSignalRun.Controls.Add(this.checkRunSigActive);
            this.groupSignalRun.Location = new System.Drawing.Point(23, 123);
            this.groupSignalRun.Name = "groupSignalRun";
            this.groupSignalRun.Size = new System.Drawing.Size(200, 135);
            this.groupSignalRun.TabIndex = 6;
            this.groupSignalRun.TabStop = false;
            this.groupSignalRun.Text = "app run signal";
            // 
            // radioSigSet0
            // 
            this.radioSigSet0.AutoSize = true;
            this.radioSigSet0.Location = new System.Drawing.Point(15, 100);
            this.radioSigSet0.Name = "radioSigSet0";
            this.radioSigSet0.Size = new System.Drawing.Size(152, 21);
            this.radioSigSet0.TabIndex = 3;
            this.radioSigSet0.TabStop = true;
            this.radioSigSet0.Text = "\"0\" when app running";
            this.radioSigSet0.UseVisualStyleBackColor = true;
            // 
            // labelSigName
            // 
            this.labelSigName.AutoSize = true;
            this.labelSigName.Location = new System.Drawing.Point(12, 28);
            this.labelSigName.Name = "labelSigName";
            this.labelSigName.Size = new System.Drawing.Size(76, 17);
            this.labelSigName.TabIndex = 3;
            this.labelSigName.Text = "signal name";
            // 
            // radioSigSet1
            // 
            this.radioSigSet1.AutoSize = true;
            this.radioSigSet1.Location = new System.Drawing.Point(15, 78);
            this.radioSigSet1.Name = "radioSigSet1";
            this.radioSigSet1.Size = new System.Drawing.Size(150, 21);
            this.radioSigSet1.TabIndex = 2;
            this.radioSigSet1.TabStop = true;
            this.radioSigSet1.Text = "\"1\" when app running";
            this.radioSigSet1.UseVisualStyleBackColor = true;
            // 
            // textSigName
            // 
            this.textSigName.Location = new System.Drawing.Point(15, 46);
            this.textSigName.Name = "textSigName";
            this.textSigName.Size = new System.Drawing.Size(170, 23);
            this.textSigName.TabIndex = 1;
            // 
            // checkRunSigActive
            // 
            this.checkRunSigActive.AutoSize = true;
            this.checkRunSigActive.Location = new System.Drawing.Point(119, 0);
            this.checkRunSigActive.Name = "checkRunSigActive";
            this.checkRunSigActive.Size = new System.Drawing.Size(66, 21);
            this.checkRunSigActive.TabIndex = 0;
            this.checkRunSigActive.Text = "active";
            this.checkRunSigActive.UseVisualStyleBackColor = true;
            // 
            // labelSettingsGeneral
            // 
            this.labelSettingsGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSettingsGeneral.Font = new System.Drawing.Font("GOST Common", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSettingsGeneral.Location = new System.Drawing.Point(5, 5);
            this.labelSettingsGeneral.Name = "labelSettingsGeneral";
            this.labelSettingsGeneral.Size = new System.Drawing.Size(234, 22);
            this.labelSettingsGeneral.TabIndex = 5;
            this.labelSettingsGeneral.Text = "GENERAL";
            // 
            // checkLoadLastProject
            // 
            this.checkLoadLastProject.AutoSize = true;
            this.checkLoadLastProject.Location = new System.Drawing.Point(23, 38);
            this.checkLoadLastProject.Name = "checkLoadLastProject";
            this.checkLoadLastProject.Size = new System.Drawing.Size(127, 21);
            this.checkLoadLastProject.TabIndex = 0;
            this.checkLoadLastProject.Text = "load last project";
            this.checkLoadLastProject.UseVisualStyleBackColor = true;
            // 
            // groupMinimizeMethod
            // 
            this.groupMinimizeMethod.Controls.Add(this.radioMinTray);
            this.groupMinimizeMethod.Controls.Add(this.radioMinNotifyIcon);
            this.groupMinimizeMethod.Location = new System.Drawing.Point(23, 267);
            this.groupMinimizeMethod.Name = "groupMinimizeMethod";
            this.groupMinimizeMethod.Size = new System.Drawing.Size(200, 58);
            this.groupMinimizeMethod.TabIndex = 1;
            this.groupMinimizeMethod.TabStop = false;
            this.groupMinimizeMethod.Text = "minimize action";
            // 
            // radioMinTray
            // 
            this.radioMinTray.AutoSize = true;
            this.radioMinTray.Location = new System.Drawing.Point(103, 22);
            this.radioMinTray.Name = "radioMinTray";
            this.radioMinTray.Size = new System.Drawing.Size(78, 21);
            this.radioMinTray.TabIndex = 1;
            this.radioMinTray.Text = "tray bar";
            this.radioMinTray.UseVisualStyleBackColor = true;
            // 
            // radioMinNotifyIcon
            // 
            this.radioMinNotifyIcon.AutoSize = true;
            this.radioMinNotifyIcon.Checked = true;
            this.radioMinNotifyIcon.Location = new System.Drawing.Point(8, 22);
            this.radioMinNotifyIcon.Name = "radioMinNotifyIcon";
            this.radioMinNotifyIcon.Size = new System.Drawing.Size(91, 21);
            this.radioMinNotifyIcon.TabIndex = 0;
            this.radioMinNotifyIcon.TabStop = true;
            this.radioMinNotifyIcon.Text = "notify icon";
            this.radioMinNotifyIcon.UseVisualStyleBackColor = true;
            // 
            // checkAlwaysOnTop
            // 
            this.checkAlwaysOnTop.AutoSize = true;
            this.checkAlwaysOnTop.Location = new System.Drawing.Point(23, 92);
            this.checkAlwaysOnTop.Name = "checkAlwaysOnTop";
            this.checkAlwaysOnTop.Size = new System.Drawing.Size(111, 21);
            this.checkAlwaysOnTop.TabIndex = 2;
            this.checkAlwaysOnTop.Text = "always on top";
            this.checkAlwaysOnTop.UseVisualStyleBackColor = true;
            // 
            // tabSettingsMail
            // 
            this.tabSettingsMail.BackColor = System.Drawing.Color.White;
            this.tabSettingsMail.Controls.Add(this.labelMessage);
            this.tabSettingsMail.Controls.Add(this.labelSubject);
            this.tabSettingsMail.Controls.Add(this.labelCC);
            this.tabSettingsMail.Controls.Add(this.labelTo);
            this.tabSettingsMail.Controls.Add(this.groupMailSettings);
            this.tabSettingsMail.Controls.Add(this.textMessage);
            this.tabSettingsMail.Controls.Add(this.textSubject);
            this.tabSettingsMail.Controls.Add(this.textCC);
            this.tabSettingsMail.Controls.Add(this.textTo);
            this.tabSettingsMail.ForeColor = System.Drawing.Color.Black;
            this.tabSettingsMail.ImageIndex = 0;
            this.tabSettingsMail.Location = new System.Drawing.Point(4, 54);
            this.tabSettingsMail.Name = "tabSettingsMail";
            this.tabSettingsMail.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingsMail.Size = new System.Drawing.Size(543, 389);
            this.tabSettingsMail.TabIndex = 0;
            this.tabSettingsMail.ToolTipText = "mail settings";
            // 
            // tabSettingsUser
            // 
            this.tabSettingsUser.BackColor = System.Drawing.Color.White;
            this.tabSettingsUser.ForeColor = System.Drawing.Color.Black;
            this.tabSettingsUser.ImageIndex = 2;
            this.tabSettingsUser.Location = new System.Drawing.Point(4, 54);
            this.tabSettingsUser.Name = "tabSettingsUser";
            this.tabSettingsUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingsUser.Size = new System.Drawing.Size(543, 389);
            this.tabSettingsUser.TabIndex = 2;
            // 
            // imageTabs
            // 
            this.imageTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageTabs.ImageStream")));
            this.imageTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imageTabs.Images.SetKeyName(0, "settings_mail.png");
            this.imageTabs.Images.SetKeyName(1, "settings_app.png");
            this.imageTabs.Images.SetKeyName(2, "settings_users.png");
            // 
            // panelTemplate
            // 
            this.panelTemplate.BackColor = System.Drawing.Color.Black;
            this.panelTemplate.Location = new System.Drawing.Point(197, 34);
            this.panelTemplate.Name = "panelTemplate";
            this.panelTemplate.Size = new System.Drawing.Size(585, 561);
            this.panelTemplate.TabIndex = 2;
            // 
            // textTo
            // 
            this.textTo.Location = new System.Drawing.Point(123, 17);
            this.textTo.Name = "textTo";
            this.textTo.Size = new System.Drawing.Size(363, 23);
            this.textTo.TabIndex = 0;
            // 
            // textCC
            // 
            this.textCC.Location = new System.Drawing.Point(123, 46);
            this.textCC.Name = "textCC";
            this.textCC.Size = new System.Drawing.Size(363, 23);
            this.textCC.TabIndex = 0;
            // 
            // textSubject
            // 
            this.textSubject.Location = new System.Drawing.Point(123, 75);
            this.textSubject.Name = "textSubject";
            this.textSubject.Size = new System.Drawing.Size(363, 23);
            this.textSubject.TabIndex = 0;
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(123, 104);
            this.textMessage.Multiline = true;
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(363, 112);
            this.textMessage.TabIndex = 1;
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(65, 30);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(214, 23);
            this.textUser.TabIndex = 0;
            // 
            // textPass
            // 
            this.textPass.Location = new System.Drawing.Point(65, 61);
            this.textPass.Name = "textPass";
            this.textPass.PasswordChar = '*';
            this.textPass.Size = new System.Drawing.Size(214, 23);
            this.textPass.TabIndex = 0;
            this.textPass.UseSystemPasswordChar = true;
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(66, 94);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(65, 23);
            this.textPort.TabIndex = 0;
            // 
            // textSMTP
            // 
            this.textSMTP.Location = new System.Drawing.Point(183, 94);
            this.textSMTP.Name = "textSMTP";
            this.textSMTP.Size = new System.Drawing.Size(167, 23);
            this.textSMTP.TabIndex = 0;
            this.textSMTP.Text = "smtp.gmail.com";
            // 
            // groupMailSettings
            // 
            this.groupMailSettings.Controls.Add(this.labelSMTP);
            this.groupMailSettings.Controls.Add(this.labelPort);
            this.groupMailSettings.Controls.Add(this.labelPass);
            this.groupMailSettings.Controls.Add(this.labelUser);
            this.groupMailSettings.Controls.Add(this.buttonTestSend);
            this.groupMailSettings.Controls.Add(this.checkMailActive);
            this.groupMailSettings.Controls.Add(this.checkSSL);
            this.groupMailSettings.Controls.Add(this.textUser);
            this.groupMailSettings.Controls.Add(this.textPass);
            this.groupMailSettings.Controls.Add(this.textPort);
            this.groupMailSettings.Controls.Add(this.textSMTP);
            this.groupMailSettings.Location = new System.Drawing.Point(58, 229);
            this.groupMailSettings.Name = "groupMailSettings";
            this.groupMailSettings.Size = new System.Drawing.Size(428, 133);
            this.groupMailSettings.TabIndex = 2;
            this.groupMailSettings.TabStop = false;
            this.groupMailSettings.Text = "settings";
            // 
            // checkSSL
            // 
            this.checkSSL.AutoSize = true;
            this.checkSSL.Location = new System.Drawing.Point(366, 96);
            this.checkSSL.Name = "checkSSL";
            this.checkSSL.Size = new System.Drawing.Size(48, 21);
            this.checkSSL.TabIndex = 1;
            this.checkSSL.Text = "ssl";
            this.checkSSL.UseVisualStyleBackColor = true;
            // 
            // buttonTestSend
            // 
            this.buttonTestSend.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonTestSend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonTestSend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.buttonTestSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTestSend.Location = new System.Drawing.Point(305, 30);
            this.buttonTestSend.Name = "buttonTestSend";
            this.buttonTestSend.Size = new System.Drawing.Size(109, 54);
            this.buttonTestSend.TabIndex = 2;
            this.buttonTestSend.Text = "TEST SEND";
            this.buttonTestSend.UseVisualStyleBackColor = false;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(55, 20);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(24, 17);
            this.labelTo.TabIndex = 3;
            this.labelTo.Text = "to:";
            // 
            // labelCC
            // 
            this.labelCC.AutoSize = true;
            this.labelCC.Location = new System.Drawing.Point(55, 49);
            this.labelCC.Name = "labelCC";
            this.labelCC.Size = new System.Drawing.Size(23, 17);
            this.labelCC.TabIndex = 3;
            this.labelCC.Text = "cc:";
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(55, 78);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(54, 17);
            this.labelSubject.TabIndex = 3;
            this.labelSubject.Text = "subject:";
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(55, 107);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(62, 17);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "message:";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(20, 33);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(38, 17);
            this.labelUser.TabIndex = 3;
            this.labelUser.Text = "user:";
            // 
            // labelPass
            // 
            this.labelPass.AutoSize = true;
            this.labelPass.Location = new System.Drawing.Point(20, 64);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(39, 17);
            this.labelPass.TabIndex = 3;
            this.labelPass.Text = "pass:";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(21, 97);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(37, 17);
            this.labelPort.TabIndex = 3;
            this.labelPort.Text = "port:";
            // 
            // labelSMTP
            // 
            this.labelSMTP.AutoSize = true;
            this.labelSMTP.Location = new System.Drawing.Point(140, 97);
            this.labelSMTP.Name = "labelSMTP";
            this.labelSMTP.Size = new System.Drawing.Size(40, 17);
            this.labelSMTP.TabIndex = 3;
            this.labelSMTP.Text = "smtp:";
            // 
            // checkMailActive
            // 
            this.checkMailActive.AutoSize = true;
            this.checkMailActive.Location = new System.Drawing.Point(348, 0);
            this.checkMailActive.Name = "checkMailActive";
            this.checkMailActive.Size = new System.Drawing.Size(66, 21);
            this.checkMailActive.TabIndex = 1;
            this.checkMailActive.Text = "active";
            this.checkMailActive.UseVisualStyleBackColor = true;
            // 
            // windowSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(978, 629);
            this.Controls.Add(this.panelBackground);
            this.Controls.Add(this.panelTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "windowSettings";
            this.Opacity = 0.75D;
            this.Text = "windowSettins";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.windowSettings_FormClosed);
            this.Load += new System.EventHandler(this.windowSettings_Load);
            this.panelFooterButtons.ResumeLayout(false);
            this.panelBackground.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettingsGeneral.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupWindowsIPC.ResumeLayout(false);
            this.groupWindowsIPC.PerformLayout();
            this.groupRemotePC.ResumeLayout(false);
            this.groupRemotePC.PerformLayout();
            this.groupBackupManager.ResumeLayout(false);
            this.groupBackupManager.PerformLayout();
            this.panelSettingsGeneral.ResumeLayout(false);
            this.panelSettingsGeneral.PerformLayout();
            this.groupSignalRun.ResumeLayout(false);
            this.groupSignalRun.PerformLayout();
            this.groupMinimizeMethod.ResumeLayout(false);
            this.groupMinimizeMethod.PerformLayout();
            this.tabSettingsMail.ResumeLayout(false);
            this.tabSettingsMail.PerformLayout();
            this.groupMailSettings.ResumeLayout(false);
            this.groupMailSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFooterButtons;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tabSettingsMail;
        private System.Windows.Forms.TabPage tabSettingsGeneral;
        private System.Windows.Forms.ImageList imageTabs;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelWindowTitle;
        private System.Windows.Forms.CheckBox checkLoadLastProject;
        private System.Windows.Forms.GroupBox groupMinimizeMethod;
        private System.Windows.Forms.RadioButton radioMinNotifyIcon;
        private System.Windows.Forms.RadioButton radioMinTray;
        private System.Windows.Forms.CheckBox checkAlwaysOnTop;
        private System.Windows.Forms.Panel panelSettingsGeneral;
        private System.Windows.Forms.GroupBox groupSignalRun;
        private System.Windows.Forms.RadioButton radioSigSet0;
        private System.Windows.Forms.Label labelSigName;
        private System.Windows.Forms.RadioButton radioSigSet1;
        private System.Windows.Forms.TextBox textSigName;
        private System.Windows.Forms.CheckBox checkRunSigActive;
        private System.Windows.Forms.Label labelSettingsGeneral;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelSettingsApps;
        private System.Windows.Forms.Panel panelTemplate;
        private System.Windows.Forms.CheckBox checkShowProjPath;
        private System.Windows.Forms.GroupBox groupBackupManager;
        private System.Windows.Forms.CheckBox checkCreateOutputDir;
        private System.Windows.Forms.GroupBox groupWindowsIPC;
        private System.Windows.Forms.GroupBox groupRemotePC;
        private System.Windows.Forms.CheckBox checkUpdBackupTime;
        private System.Windows.Forms.CheckBox checkFillMsg;
        private System.Windows.Forms.TextBox textDefaultServerName;
        private System.Windows.Forms.Label labelDefaultServerName;
        private System.Windows.Forms.CheckBox checkRememberAppDir;
        private System.Windows.Forms.TabPage tabSettingsUser;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.Label labelCC;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.GroupBox groupMailSettings;
        private System.Windows.Forms.Label labelSMTP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Button buttonTestSend;
        private System.Windows.Forms.CheckBox checkSSL;
        private System.Windows.Forms.TextBox textUser;
        private System.Windows.Forms.TextBox textPass;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textSMTP;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.TextBox textSubject;
        private System.Windows.Forms.TextBox textCC;
        private System.Windows.Forms.TextBox textTo;
        private System.Windows.Forms.CheckBox checkMailActive;
    }
}