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
            this.groupRemotePC = new System.Windows.Forms.GroupBox();
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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panelTemplate = new System.Windows.Forms.Panel();
            this.checkFillMsg = new System.Windows.Forms.CheckBox();
            this.checkRememberAppDir = new System.Windows.Forms.CheckBox();
            this.labelDefaultServerName = new System.Windows.Forms.Label();
            this.textDefaultServerName = new System.Windows.Forms.TextBox();
            this.tabSettingsUser = new System.Windows.Forms.TabPage();
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
            this.tabSettings.ImageList = this.imageList1;
            this.tabSettings.ItemSize = new System.Drawing.Size(150, 50);
            this.tabSettings.Location = new System.Drawing.Point(17, 56);
            this.tabSettings.Multiline = true;
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.ShowToolTips = true;
            this.tabSettings.Size = new System.Drawing.Size(551, 447);
            this.tabSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSettings.TabIndex = 1;
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
            this.checkUpdBackupTime.TabIndex = 2;
            this.checkUpdBackupTime.Text = "auto update time";
            this.checkUpdBackupTime.UseVisualStyleBackColor = true;
            // 
            // checkCreateOutputDir
            // 
            this.checkCreateOutputDir.AutoSize = true;
            this.checkCreateOutputDir.Location = new System.Drawing.Point(15, 26);
            this.checkCreateOutputDir.Name = "checkCreateOutputDir";
            this.checkCreateOutputDir.Size = new System.Drawing.Size(142, 21);
            this.checkCreateOutputDir.TabIndex = 1;
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
            this.checkShowProjPath.TabIndex = 7;
            this.checkShowProjPath.Text = "show project path";
            this.checkShowProjPath.UseVisualStyleBackColor = true;
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
            this.radioSigSet0.TabIndex = 4;
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
            this.tabSettingsMail.ForeColor = System.Drawing.Color.Black;
            this.tabSettingsMail.ImageIndex = 0;
            this.tabSettingsMail.Location = new System.Drawing.Point(4, 54);
            this.tabSettingsMail.Name = "tabSettingsMail";
            this.tabSettingsMail.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingsMail.Size = new System.Drawing.Size(543, 389);
            this.tabSettingsMail.TabIndex = 0;
            this.tabSettingsMail.ToolTipText = "mail settings";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "settings_mail.png");
            this.imageList1.Images.SetKeyName(1, "settings_app.png");
            // 
            // panelTemplate
            // 
            this.panelTemplate.BackColor = System.Drawing.Color.Black;
            this.panelTemplate.Location = new System.Drawing.Point(197, 34);
            this.panelTemplate.Name = "panelTemplate";
            this.panelTemplate.Size = new System.Drawing.Size(585, 561);
            this.panelTemplate.TabIndex = 2;
            // 
            // checkFillMsg
            // 
            this.checkFillMsg.AutoSize = true;
            this.checkFillMsg.Location = new System.Drawing.Point(15, 26);
            this.checkFillMsg.Name = "checkFillMsg";
            this.checkFillMsg.Size = new System.Drawing.Size(108, 21);
            this.checkFillMsg.TabIndex = 4;
            this.checkFillMsg.Text = "fill messages";
            this.checkFillMsg.UseVisualStyleBackColor = true;
            // 
            // checkRememberAppDir
            // 
            this.checkRememberAppDir.AutoSize = true;
            this.checkRememberAppDir.Location = new System.Drawing.Point(15, 27);
            this.checkRememberAppDir.Name = "checkRememberAppDir";
            this.checkRememberAppDir.Size = new System.Drawing.Size(139, 21);
            this.checkRememberAppDir.TabIndex = 5;
            this.checkRememberAppDir.Text = "remember last app";
            this.checkRememberAppDir.UseVisualStyleBackColor = true;
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
            // textDefaultServerName
            // 
            this.textDefaultServerName.Location = new System.Drawing.Point(15, 72);
            this.textDefaultServerName.Name = "textDefaultServerName";
            this.textDefaultServerName.Size = new System.Drawing.Size(142, 23);
            this.textDefaultServerName.TabIndex = 6;
            // 
            // tabSettingsUser
            // 
            this.tabSettingsUser.BackColor = System.Drawing.Color.White;
            this.tabSettingsUser.ForeColor = System.Drawing.Color.Black;
            this.tabSettingsUser.Location = new System.Drawing.Point(4, 54);
            this.tabSettingsUser.Name = "tabSettingsUser";
            this.tabSettingsUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingsUser.Size = new System.Drawing.Size(543, 389);
            this.tabSettingsUser.TabIndex = 2;
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
        private System.Windows.Forms.ImageList imageList1;
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
    }
}