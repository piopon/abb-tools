namespace abbTools.AppBackupManager
{
    partial class appBackupManager
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
            this.groupPCmaster = new System.Windows.Forms.GroupBox();
            this.groupBackupTime = new System.Windows.Forms.GroupBox();
            this.labelEveryTime = new System.Windows.Forms.Label();
            this.textEveryTime = new System.Windows.Forms.MaskedTextBox();
            this.labelDailySuffix = new System.Windows.Forms.Label();
            this.textDailySuffix = new System.Windows.Forms.TextBox();
            this.groupPCcontrol = new System.Windows.Forms.GroupBox();
            this.labelGUISuffix = new System.Windows.Forms.Label();
            this.btnBackupExe = new System.Windows.Forms.Button();
            this.textGuiSuffix = new System.Windows.Forms.TextBox();
            this.labelRecentBackupPC = new System.Windows.Forms.Label();
            this.labelLastTimePC = new System.Windows.Forms.Label();
            this.groupInterval = new System.Windows.Forms.GroupBox();
            this.labelIntervalSuffix = new System.Windows.Forms.Label();
            this.labelIntervalDay = new System.Windows.Forms.Label();
            this.textIntervalSuffix = new System.Windows.Forms.TextBox();
            this.labelIntervalHour = new System.Windows.Forms.Label();
            this.labelIntervalMin = new System.Windows.Forms.Label();
            this.numIntervalDays = new System.Windows.Forms.NumericUpDown();
            this.numIntervalHours = new System.Windows.Forms.NumericUpDown();
            this.numIntervalMins = new System.Windows.Forms.NumericUpDown();
            this.groupPCduplicate = new System.Windows.Forms.GroupBox();
            this.radioPCTime = new System.Windows.Forms.RadioButton();
            this.labelDirExists = new System.Windows.Forms.Label();
            this.radioPCOverwrite = new System.Windows.Forms.RadioButton();
            this.radioPCIncr = new System.Windows.Forms.RadioButton();
            this.checkPCactive = new System.Windows.Forms.CheckBox();
            this.groupDirClean = new System.Windows.Forms.GroupBox();
            this.btnCleanExe = new System.Windows.Forms.Button();
            this.numClearDays = new System.Windows.Forms.NumericUpDown();
            this.labelClearDays = new System.Windows.Forms.Label();
            this.groupOutPath = new System.Windows.Forms.GroupBox();
            this.btnOutShow = new System.Windows.Forms.Button();
            this.btnOutSelect = new System.Windows.Forms.Button();
            this.labelOutPathVal = new System.Windows.Forms.Label();
            this.labelOutPathTitle = new System.Windows.Forms.Label();
            this.groupRobMaster = new System.Windows.Forms.GroupBox();
            this.groupDetails = new System.Windows.Forms.GroupBox();
            this.labelRecentBackupROB = new System.Windows.Forms.Label();
            this.labelLastTimeROB = new System.Windows.Forms.Label();
            this.labelRobotSuffix = new System.Windows.Forms.Label();
            this.textRobotSuffix = new System.Windows.Forms.TextBox();
            this.groupRobotDuplicate = new System.Windows.Forms.GroupBox();
            this.radioROBTime = new System.Windows.Forms.RadioButton();
            this.radioROBOverwrite = new System.Windows.Forms.RadioButton();
            this.radioROBIncr = new System.Windows.Forms.RadioButton();
            this.groupRobotSettings = new System.Windows.Forms.GroupBox();
            this.labelRobBackupDir = new System.Windows.Forms.Label();
            this.btnRobBackupDir = new System.Windows.Forms.Button();
            this.labelSigDoBackup = new System.Windows.Forms.Label();
            this.labelSigBackupInProg = new System.Windows.Forms.Label();
            this.textSigBackupProg = new System.Windows.Forms.TextBox();
            this.textSigDoBackup = new System.Windows.Forms.TextBox();
            this.checkRobActive = new System.Windows.Forms.CheckBox();
            this.timerCheckBackup = new System.Windows.Forms.Timer(this.components);
            this.dialogOutDir = new System.Windows.Forms.FolderBrowserDialog();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupCommonSettings = new System.Windows.Forms.GroupBox();
            this.groupWatchStatus = new System.Windows.Forms.GroupBox();
            this.btnWatchOff = new System.Windows.Forms.Button();
            this.btnWatchOn = new System.Windows.Forms.Button();
            this.groupPCmaster.SuspendLayout();
            this.groupBackupTime.SuspendLayout();
            this.groupPCcontrol.SuspendLayout();
            this.groupInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalMins)).BeginInit();
            this.groupPCduplicate.SuspendLayout();
            this.groupDirClean.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClearDays)).BeginInit();
            this.groupOutPath.SuspendLayout();
            this.groupRobMaster.SuspendLayout();
            this.groupDetails.SuspendLayout();
            this.groupRobotDuplicate.SuspendLayout();
            this.groupRobotSettings.SuspendLayout();
            this.groupCommonSettings.SuspendLayout();
            this.groupWatchStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPCmaster
            // 
            this.groupPCmaster.Controls.Add(this.groupBackupTime);
            this.groupPCmaster.Controls.Add(this.groupPCcontrol);
            this.groupPCmaster.Controls.Add(this.groupInterval);
            this.groupPCmaster.Controls.Add(this.groupPCduplicate);
            this.groupPCmaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupPCmaster.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupPCmaster.ForeColor = System.Drawing.Color.Black;
            this.groupPCmaster.Location = new System.Drawing.Point(15, 3);
            this.groupPCmaster.Name = "groupPCmaster";
            this.groupPCmaster.Size = new System.Drawing.Size(773, 217);
            this.groupPCmaster.TabIndex = 0;
            this.groupPCmaster.TabStop = false;
            this.groupPCmaster.Text = "  PC BACKUP MASTER            ";
            // 
            // groupBackupTime
            // 
            this.groupBackupTime.Controls.Add(this.labelEveryTime);
            this.groupBackupTime.Controls.Add(this.textEveryTime);
            this.groupBackupTime.Controls.Add(this.labelDailySuffix);
            this.groupBackupTime.Controls.Add(this.textDailySuffix);
            this.groupBackupTime.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBackupTime.Location = new System.Drawing.Point(391, 31);
            this.groupBackupTime.Name = "groupBackupTime";
            this.groupBackupTime.Size = new System.Drawing.Size(177, 164);
            this.groupBackupTime.TabIndex = 13;
            this.groupBackupTime.TabStop = false;
            this.groupBackupTime.Text = "backup time";
            // 
            // labelEveryTime
            // 
            this.labelEveryTime.AutoSize = true;
            this.labelEveryTime.Location = new System.Drawing.Point(22, 40);
            this.labelEveryTime.Name = "labelEveryTime";
            this.labelEveryTime.Size = new System.Drawing.Size(72, 17);
            this.labelEveryTime.TabIndex = 11;
            this.labelEveryTime.Text = "exact time:";
            // 
            // textEveryTime
            // 
            this.textEveryTime.Location = new System.Drawing.Point(25, 59);
            this.textEveryTime.Mask = "00:00";
            this.textEveryTime.Name = "textEveryTime";
            this.textEveryTime.Size = new System.Drawing.Size(134, 23);
            this.textEveryTime.TabIndex = 7;
            this.textEveryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textEveryTime.ValidatingType = typeof(System.DateTime);
            this.textEveryTime.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.textEveryTime_TypeValidationCompleted);
            this.textEveryTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEveryTime_KeyDown);
            // 
            // labelDailySuffix
            // 
            this.labelDailySuffix.AutoSize = true;
            this.labelDailySuffix.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDailySuffix.Location = new System.Drawing.Point(22, 113);
            this.labelDailySuffix.Name = "labelDailySuffix";
            this.labelDailySuffix.Size = new System.Drawing.Size(81, 15);
            this.labelDailySuffix.TabIndex = 3;
            this.labelDailySuffix.Text = "dir time suffix";
            // 
            // textDailySuffix
            // 
            this.textDailySuffix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textDailySuffix.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textDailySuffix.Location = new System.Drawing.Point(25, 129);
            this.textDailySuffix.Name = "textDailySuffix";
            this.textDailySuffix.Size = new System.Drawing.Size(134, 23);
            this.textDailySuffix.TabIndex = 2;
            this.textDailySuffix.Text = "_dailyBackup";
            this.textDailySuffix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textDailySuffix.TextChanged += new System.EventHandler(this.textBackupSuffix_TextChanged);
            // 
            // groupPCcontrol
            // 
            this.groupPCcontrol.Controls.Add(this.labelGUISuffix);
            this.groupPCcontrol.Controls.Add(this.btnBackupExe);
            this.groupPCcontrol.Controls.Add(this.textGuiSuffix);
            this.groupPCcontrol.Controls.Add(this.labelRecentBackupPC);
            this.groupPCcontrol.Controls.Add(this.labelLastTimePC);
            this.groupPCcontrol.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupPCcontrol.Location = new System.Drawing.Point(13, 31);
            this.groupPCcontrol.Name = "groupPCcontrol";
            this.groupPCcontrol.Size = new System.Drawing.Size(177, 164);
            this.groupPCcontrol.TabIndex = 13;
            this.groupPCcontrol.TabStop = false;
            this.groupPCcontrol.Text = "control";
            // 
            // labelGUISuffix
            // 
            this.labelGUISuffix.AutoSize = true;
            this.labelGUISuffix.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelGUISuffix.Location = new System.Drawing.Point(18, 113);
            this.labelGUISuffix.Name = "labelGUISuffix";
            this.labelGUISuffix.Size = new System.Drawing.Size(73, 15);
            this.labelGUISuffix.TabIndex = 15;
            this.labelGUISuffix.Text = "dir gui suffix";
            // 
            // btnBackupExe
            // 
            this.btnBackupExe.BackColor = System.Drawing.Color.Chartreuse;
            this.btnBackupExe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupExe.Location = new System.Drawing.Point(21, 72);
            this.btnBackupExe.Name = "btnBackupExe";
            this.btnBackupExe.Size = new System.Drawing.Size(134, 29);
            this.btnBackupExe.TabIndex = 6;
            this.btnBackupExe.Text = "do backup";
            this.btnBackupExe.UseVisualStyleBackColor = false;
            this.btnBackupExe.Click += new System.EventHandler(this.btnBackupExe_Click);
            // 
            // textGuiSuffix
            // 
            this.textGuiSuffix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textGuiSuffix.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textGuiSuffix.Location = new System.Drawing.Point(21, 129);
            this.textGuiSuffix.Name = "textGuiSuffix";
            this.textGuiSuffix.Size = new System.Drawing.Size(134, 23);
            this.textGuiSuffix.TabIndex = 14;
            this.textGuiSuffix.Text = "_GUI";
            this.textGuiSuffix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textGuiSuffix.TextChanged += new System.EventHandler(this.textBackupSuffix_TextChanged);
            // 
            // labelRecentBackupPC
            // 
            this.labelRecentBackupPC.AutoSize = true;
            this.labelRecentBackupPC.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelRecentBackupPC.Location = new System.Drawing.Point(18, 24);
            this.labelRecentBackupPC.Name = "labelRecentBackupPC";
            this.labelRecentBackupPC.Size = new System.Drawing.Size(81, 15);
            this.labelRecentBackupPC.TabIndex = 2;
            this.labelRecentBackupPC.Text = "recent backup:";
            // 
            // labelLastTimePC
            // 
            this.labelLastTimePC.AutoSize = true;
            this.labelLastTimePC.Location = new System.Drawing.Point(19, 41);
            this.labelLastTimePC.Name = "labelLastTimePC";
            this.labelLastTimePC.Size = new System.Drawing.Size(15, 17);
            this.labelLastTimePC.TabIndex = 3;
            this.labelLastTimePC.Text = "-";
            // 
            // groupInterval
            // 
            this.groupInterval.Controls.Add(this.labelIntervalSuffix);
            this.groupInterval.Controls.Add(this.labelIntervalDay);
            this.groupInterval.Controls.Add(this.textIntervalSuffix);
            this.groupInterval.Controls.Add(this.labelIntervalHour);
            this.groupInterval.Controls.Add(this.labelIntervalMin);
            this.groupInterval.Controls.Add(this.numIntervalDays);
            this.groupInterval.Controls.Add(this.numIntervalHours);
            this.groupInterval.Controls.Add(this.numIntervalMins);
            this.groupInterval.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupInterval.Location = new System.Drawing.Point(205, 31);
            this.groupInterval.Name = "groupInterval";
            this.groupInterval.Size = new System.Drawing.Size(171, 164);
            this.groupInterval.TabIndex = 12;
            this.groupInterval.TabStop = false;
            this.groupInterval.Text = "backup interval";
            // 
            // labelIntervalSuffix
            // 
            this.labelIntervalSuffix.AutoSize = true;
            this.labelIntervalSuffix.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelIntervalSuffix.Location = new System.Drawing.Point(15, 113);
            this.labelIntervalSuffix.Name = "labelIntervalSuffix";
            this.labelIntervalSuffix.Size = new System.Drawing.Size(98, 15);
            this.labelIntervalSuffix.TabIndex = 13;
            this.labelIntervalSuffix.Text = "dir interval suffix";
            // 
            // labelIntervalDay
            // 
            this.labelIntervalDay.AutoSize = true;
            this.labelIntervalDay.Location = new System.Drawing.Point(15, 77);
            this.labelIntervalDay.Name = "labelIntervalDay";
            this.labelIntervalDay.Size = new System.Drawing.Size(69, 17);
            this.labelIntervalDay.TabIndex = 10;
            this.labelIntervalDay.Text = "every day:";
            // 
            // textIntervalSuffix
            // 
            this.textIntervalSuffix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textIntervalSuffix.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textIntervalSuffix.Location = new System.Drawing.Point(18, 129);
            this.textIntervalSuffix.Name = "textIntervalSuffix";
            this.textIntervalSuffix.Size = new System.Drawing.Size(134, 23);
            this.textIntervalSuffix.TabIndex = 12;
            this.textIntervalSuffix.Text = "_autoBackup";
            this.textIntervalSuffix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textIntervalSuffix.TextChanged += new System.EventHandler(this.textBackupSuffix_TextChanged);
            // 
            // labelIntervalHour
            // 
            this.labelIntervalHour.AutoSize = true;
            this.labelIntervalHour.Location = new System.Drawing.Point(15, 52);
            this.labelIntervalHour.Name = "labelIntervalHour";
            this.labelIntervalHour.Size = new System.Drawing.Size(75, 17);
            this.labelIntervalHour.TabIndex = 9;
            this.labelIntervalHour.Text = "every hour:";
            // 
            // labelIntervalMin
            // 
            this.labelIntervalMin.AutoSize = true;
            this.labelIntervalMin.Location = new System.Drawing.Point(16, 27);
            this.labelIntervalMin.Name = "labelIntervalMin";
            this.labelIntervalMin.Size = new System.Drawing.Size(67, 17);
            this.labelIntervalMin.TabIndex = 8;
            this.labelIntervalMin.Text = "every min:";
            // 
            // numIntervalDays
            // 
            this.numIntervalDays.Location = new System.Drawing.Point(103, 75);
            this.numIntervalDays.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numIntervalDays.Name = "numIntervalDays";
            this.numIntervalDays.Size = new System.Drawing.Size(49, 23);
            this.numIntervalDays.TabIndex = 6;
            this.numIntervalDays.ValueChanged += new System.EventHandler(this.numInterval_ValueChanged);
            // 
            // numIntervalHours
            // 
            this.numIntervalHours.Location = new System.Drawing.Point(103, 50);
            this.numIntervalHours.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numIntervalHours.Name = "numIntervalHours";
            this.numIntervalHours.Size = new System.Drawing.Size(49, 23);
            this.numIntervalHours.TabIndex = 5;
            this.numIntervalHours.ValueChanged += new System.EventHandler(this.numInterval_ValueChanged);
            // 
            // numIntervalMins
            // 
            this.numIntervalMins.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numIntervalMins.Location = new System.Drawing.Point(103, 25);
            this.numIntervalMins.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numIntervalMins.Name = "numIntervalMins";
            this.numIntervalMins.Size = new System.Drawing.Size(49, 23);
            this.numIntervalMins.TabIndex = 4;
            this.numIntervalMins.ValueChanged += new System.EventHandler(this.numInterval_ValueChanged);
            // 
            // groupPCduplicate
            // 
            this.groupPCduplicate.Controls.Add(this.radioPCTime);
            this.groupPCduplicate.Controls.Add(this.labelDirExists);
            this.groupPCduplicate.Controls.Add(this.radioPCOverwrite);
            this.groupPCduplicate.Controls.Add(this.radioPCIncr);
            this.groupPCduplicate.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupPCduplicate.Location = new System.Drawing.Point(583, 31);
            this.groupPCduplicate.Name = "groupPCduplicate";
            this.groupPCduplicate.Size = new System.Drawing.Size(174, 164);
            this.groupPCduplicate.TabIndex = 11;
            this.groupPCduplicate.TabStop = false;
            this.groupPCduplicate.Text = "output dir";
            // 
            // radioPCTime
            // 
            this.radioPCTime.AutoSize = true;
            this.radioPCTime.Checked = true;
            this.radioPCTime.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioPCTime.Location = new System.Drawing.Point(44, 107);
            this.radioPCTime.Name = "radioPCTime";
            this.radioPCTime.Size = new System.Drawing.Size(78, 21);
            this.radioPCTime.TabIndex = 6;
            this.radioPCTime.TabStop = true;
            this.radioPCTime.Text = "add time";
            this.radioPCTime.UseVisualStyleBackColor = true;
            this.radioPCTime.CheckedChanged += new System.EventHandler(this.radioPCDuplicate_CheckChanges);
            // 
            // labelDirExists
            // 
            this.labelDirExists.AutoSize = true;
            this.labelDirExists.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDirExists.Location = new System.Drawing.Point(27, 37);
            this.labelDirExists.Name = "labelDirExists";
            this.labelDirExists.Size = new System.Drawing.Size(115, 17);
            this.labelDirExists.TabIndex = 7;
            this.labelDirExists.Text = "action if dir exists";
            // 
            // radioPCOverwrite
            // 
            this.radioPCOverwrite.AutoSize = true;
            this.radioPCOverwrite.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioPCOverwrite.Location = new System.Drawing.Point(44, 59);
            this.radioPCOverwrite.Name = "radioPCOverwrite";
            this.radioPCOverwrite.Size = new System.Drawing.Size(87, 21);
            this.radioPCOverwrite.TabIndex = 4;
            this.radioPCOverwrite.Text = "overwrite";
            this.radioPCOverwrite.UseVisualStyleBackColor = true;
            this.radioPCOverwrite.CheckedChanged += new System.EventHandler(this.radioPCDuplicate_CheckChanges);
            // 
            // radioPCIncr
            // 
            this.radioPCIncr.AutoSize = true;
            this.radioPCIncr.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioPCIncr.Location = new System.Drawing.Point(44, 83);
            this.radioPCIncr.Name = "radioPCIncr";
            this.radioPCIncr.Size = new System.Drawing.Size(87, 21);
            this.radioPCIncr.TabIndex = 5;
            this.radioPCIncr.Text = "increment";
            this.radioPCIncr.UseVisualStyleBackColor = true;
            this.radioPCIncr.CheckedChanged += new System.EventHandler(this.radioPCDuplicate_CheckChanges);
            // 
            // checkPCactive
            // 
            this.checkPCactive.AutoSize = true;
            this.checkPCactive.BackColor = System.Drawing.Color.White;
            this.checkPCactive.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkPCactive.Location = new System.Drawing.Point(285, 5);
            this.checkPCactive.Name = "checkPCactive";
            this.checkPCactive.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.checkPCactive.Size = new System.Drawing.Size(72, 23);
            this.checkPCactive.TabIndex = 0;
            this.checkPCactive.Text = "active";
            this.checkPCactive.UseVisualStyleBackColor = false;
            this.checkPCactive.CheckedChanged += new System.EventHandler(this.checkActive_CheckedChanged);
            // 
            // groupDirClean
            // 
            this.groupDirClean.Controls.Add(this.btnCleanExe);
            this.groupDirClean.Controls.Add(this.numClearDays);
            this.groupDirClean.Controls.Add(this.labelClearDays);
            this.groupDirClean.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupDirClean.Location = new System.Drawing.Point(13, 32);
            this.groupDirClean.Name = "groupDirClean";
            this.groupDirClean.Size = new System.Drawing.Size(231, 106);
            this.groupDirClean.TabIndex = 14;
            this.groupDirClean.TabStop = false;
            this.groupDirClean.Text = "output cleaner";
            // 
            // btnCleanExe
            // 
            this.btnCleanExe.BackColor = System.Drawing.Color.OrangeRed;
            this.btnCleanExe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleanExe.Location = new System.Drawing.Point(24, 65);
            this.btnCleanExe.Name = "btnCleanExe";
            this.btnCleanExe.Size = new System.Drawing.Size(182, 29);
            this.btnCleanExe.TabIndex = 7;
            this.btnCleanExe.Text = "do clean";
            this.btnCleanExe.UseVisualStyleBackColor = false;
            this.btnCleanExe.Click += new System.EventHandler(this.btnCleanExe_Click);
            // 
            // numClearDays
            // 
            this.numClearDays.Location = new System.Drawing.Point(124, 29);
            this.numClearDays.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numClearDays.Name = "numClearDays";
            this.numClearDays.Size = new System.Drawing.Size(53, 23);
            this.numClearDays.TabIndex = 1;
            this.numClearDays.ValueChanged += new System.EventHandler(this.numClearDays_ValueChanged);
            // 
            // labelClearDays
            // 
            this.labelClearDays.AutoSize = true;
            this.labelClearDays.Location = new System.Drawing.Point(34, 32);
            this.labelClearDays.Name = "labelClearDays";
            this.labelClearDays.Size = new System.Drawing.Size(72, 17);
            this.labelClearDays.TabIndex = 0;
            this.labelClearDays.Text = "clear days:";
            // 
            // groupOutPath
            // 
            this.groupOutPath.Controls.Add(this.btnOutShow);
            this.groupOutPath.Controls.Add(this.btnOutSelect);
            this.groupOutPath.Controls.Add(this.labelOutPathVal);
            this.groupOutPath.Controls.Add(this.labelOutPathTitle);
            this.groupOutPath.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupOutPath.Location = new System.Drawing.Point(265, 32);
            this.groupOutPath.Name = "groupOutPath";
            this.groupOutPath.Size = new System.Drawing.Size(303, 106);
            this.groupOutPath.TabIndex = 10;
            this.groupOutPath.TabStop = false;
            this.groupOutPath.Text = "output path";
            // 
            // btnOutShow
            // 
            this.btnOutShow.BackColor = System.Drawing.Color.DarkOrange;
            this.btnOutShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutShow.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOutShow.Location = new System.Drawing.Point(155, 65);
            this.btnOutShow.Name = "btnOutShow";
            this.btnOutShow.Size = new System.Drawing.Size(130, 29);
            this.btnOutShow.TabIndex = 10;
            this.btnOutShow.Text = "show";
            this.btnOutShow.UseVisualStyleBackColor = false;
            this.btnOutShow.Click += new System.EventHandler(this.btnOutShow_Click);
            // 
            // btnOutSelect
            // 
            this.btnOutSelect.BackColor = System.Drawing.Color.DarkOrange;
            this.btnOutSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutSelect.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOutSelect.Location = new System.Drawing.Point(15, 65);
            this.btnOutSelect.Name = "btnOutSelect";
            this.btnOutSelect.Size = new System.Drawing.Size(130, 29);
            this.btnOutSelect.TabIndex = 1;
            this.btnOutSelect.Text = "select";
            this.btnOutSelect.UseVisualStyleBackColor = false;
            this.btnOutSelect.Click += new System.EventHandler(this.btnOutDir_Click);
            // 
            // labelOutPathVal
            // 
            this.labelOutPathVal.AutoSize = true;
            this.labelOutPathVal.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelOutPathVal.Location = new System.Drawing.Point(12, 40);
            this.labelOutPathVal.Name = "labelOutPathVal";
            this.labelOutPathVal.Size = new System.Drawing.Size(15, 17);
            this.labelOutPathVal.TabIndex = 9;
            this.labelOutPathVal.Text = "-";
            this.labelOutPathVal.MouseEnter += new System.EventHandler(this.labelOutPathVal_MouseEnter);
            this.labelOutPathVal.MouseLeave += new System.EventHandler(this.labelOutPathVal_MouseLeave);
            // 
            // labelOutPathTitle
            // 
            this.labelOutPathTitle.AutoSize = true;
            this.labelOutPathTitle.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelOutPathTitle.Location = new System.Drawing.Point(12, 23);
            this.labelOutPathTitle.Name = "labelOutPathTitle";
            this.labelOutPathTitle.Size = new System.Drawing.Size(79, 15);
            this.labelOutPathTitle.TabIndex = 8;
            this.labelOutPathTitle.Text = "selected path:";
            this.labelOutPathTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupRobMaster
            // 
            this.groupRobMaster.Controls.Add(this.groupDetails);
            this.groupRobMaster.Controls.Add(this.groupRobotDuplicate);
            this.groupRobMaster.Controls.Add(this.groupRobotSettings);
            this.groupRobMaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupRobMaster.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRobMaster.ForeColor = System.Drawing.Color.Black;
            this.groupRobMaster.Location = new System.Drawing.Point(15, 225);
            this.groupRobMaster.Name = "groupRobMaster";
            this.groupRobMaster.Size = new System.Drawing.Size(773, 167);
            this.groupRobMaster.TabIndex = 1;
            this.groupRobMaster.TabStop = false;
            this.groupRobMaster.Text = "  ROBOT BACKUP MASTER  ";
            // 
            // groupDetails
            // 
            this.groupDetails.Controls.Add(this.labelRecentBackupROB);
            this.groupDetails.Controls.Add(this.labelLastTimeROB);
            this.groupDetails.Controls.Add(this.labelRobotSuffix);
            this.groupDetails.Controls.Add(this.textRobotSuffix);
            this.groupDetails.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupDetails.Location = new System.Drawing.Point(391, 31);
            this.groupDetails.Name = "groupDetails";
            this.groupDetails.Size = new System.Drawing.Size(177, 119);
            this.groupDetails.TabIndex = 14;
            this.groupDetails.TabStop = false;
            this.groupDetails.Text = "details";
            // 
            // labelRecentBackupROB
            // 
            this.labelRecentBackupROB.AutoSize = true;
            this.labelRecentBackupROB.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelRecentBackupROB.Location = new System.Drawing.Point(22, 22);
            this.labelRecentBackupROB.Name = "labelRecentBackupROB";
            this.labelRecentBackupROB.Size = new System.Drawing.Size(81, 15);
            this.labelRecentBackupROB.TabIndex = 4;
            this.labelRecentBackupROB.Text = "recent backup:";
            // 
            // labelLastTimeROB
            // 
            this.labelLastTimeROB.AutoSize = true;
            this.labelLastTimeROB.Location = new System.Drawing.Point(23, 39);
            this.labelLastTimeROB.Name = "labelLastTimeROB";
            this.labelLastTimeROB.Size = new System.Drawing.Size(15, 17);
            this.labelLastTimeROB.TabIndex = 5;
            this.labelLastTimeROB.Text = "-";
            // 
            // labelRobotSuffix
            // 
            this.labelRobotSuffix.AutoSize = true;
            this.labelRobotSuffix.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelRobotSuffix.Location = new System.Drawing.Point(22, 65);
            this.labelRobotSuffix.Name = "labelRobotSuffix";
            this.labelRobotSuffix.Size = new System.Drawing.Size(86, 15);
            this.labelRobotSuffix.TabIndex = 3;
            this.labelRobotSuffix.Text = "dir robot suffix";
            // 
            // textRobotSuffix
            // 
            this.textRobotSuffix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textRobotSuffix.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textRobotSuffix.Location = new System.Drawing.Point(25, 82);
            this.textRobotSuffix.Name = "textRobotSuffix";
            this.textRobotSuffix.Size = new System.Drawing.Size(134, 23);
            this.textRobotSuffix.TabIndex = 2;
            this.textRobotSuffix.Text = "_dailyBackup";
            this.textRobotSuffix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textRobotSuffix.TextChanged += new System.EventHandler(this.textBackupSuffix_TextChanged);
            // 
            // groupRobotDuplicate
            // 
            this.groupRobotDuplicate.Controls.Add(this.radioROBTime);
            this.groupRobotDuplicate.Controls.Add(this.radioROBOverwrite);
            this.groupRobotDuplicate.Controls.Add(this.radioROBIncr);
            this.groupRobotDuplicate.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRobotDuplicate.Location = new System.Drawing.Point(588, 31);
            this.groupRobotDuplicate.Name = "groupRobotDuplicate";
            this.groupRobotDuplicate.Size = new System.Drawing.Size(169, 119);
            this.groupRobotDuplicate.TabIndex = 12;
            this.groupRobotDuplicate.TabStop = false;
            this.groupRobotDuplicate.Text = "duplicates";
            // 
            // radioROBTime
            // 
            this.radioROBTime.AutoSize = true;
            this.radioROBTime.Checked = true;
            this.radioROBTime.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioROBTime.Location = new System.Drawing.Point(41, 81);
            this.radioROBTime.Name = "radioROBTime";
            this.radioROBTime.Size = new System.Drawing.Size(78, 21);
            this.radioROBTime.TabIndex = 6;
            this.radioROBTime.TabStop = true;
            this.radioROBTime.Text = "add time";
            this.radioROBTime.UseVisualStyleBackColor = true;
            // 
            // radioROBOverwrite
            // 
            this.radioROBOverwrite.AutoSize = true;
            this.radioROBOverwrite.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioROBOverwrite.Location = new System.Drawing.Point(41, 27);
            this.radioROBOverwrite.Name = "radioROBOverwrite";
            this.radioROBOverwrite.Size = new System.Drawing.Size(87, 21);
            this.radioROBOverwrite.TabIndex = 4;
            this.radioROBOverwrite.Text = "overwrite";
            this.radioROBOverwrite.UseVisualStyleBackColor = true;
            this.radioROBOverwrite.CheckedChanged += new System.EventHandler(this.radioROBDuplicate_CheckChanges);
            // 
            // radioROBIncr
            // 
            this.radioROBIncr.AutoSize = true;
            this.radioROBIncr.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioROBIncr.Location = new System.Drawing.Point(41, 54);
            this.radioROBIncr.Name = "radioROBIncr";
            this.radioROBIncr.Size = new System.Drawing.Size(87, 21);
            this.radioROBIncr.TabIndex = 5;
            this.radioROBIncr.Text = "increment";
            this.radioROBIncr.UseVisualStyleBackColor = true;
            // 
            // groupRobotSettings
            // 
            this.groupRobotSettings.Controls.Add(this.labelRobBackupDir);
            this.groupRobotSettings.Controls.Add(this.btnRobBackupDir);
            this.groupRobotSettings.Controls.Add(this.labelSigDoBackup);
            this.groupRobotSettings.Controls.Add(this.labelSigBackupInProg);
            this.groupRobotSettings.Controls.Add(this.textSigBackupProg);
            this.groupRobotSettings.Controls.Add(this.textSigDoBackup);
            this.groupRobotSettings.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRobotSettings.Location = new System.Drawing.Point(13, 31);
            this.groupRobotSettings.Name = "groupRobotSettings";
            this.groupRobotSettings.Size = new System.Drawing.Size(363, 119);
            this.groupRobotSettings.TabIndex = 0;
            this.groupRobotSettings.TabStop = false;
            this.groupRobotSettings.Text = "robot settings";
            // 
            // labelRobBackupDir
            // 
            this.labelRobBackupDir.AutoSize = true;
            this.labelRobBackupDir.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelRobBackupDir.Location = new System.Drawing.Point(243, 22);
            this.labelRobBackupDir.Name = "labelRobBackupDir";
            this.labelRobBackupDir.Size = new System.Drawing.Size(62, 15);
            this.labelRobBackupDir.TabIndex = 7;
            this.labelRobBackupDir.Text = "backup dir:";
            // 
            // btnRobBackupDir
            // 
            this.btnRobBackupDir.BackColor = System.Drawing.Color.Gainsboro;
            this.btnRobBackupDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRobBackupDir.Location = new System.Drawing.Point(243, 38);
            this.btnRobBackupDir.Name = "btnRobBackupDir";
            this.btnRobBackupDir.Size = new System.Drawing.Size(101, 71);
            this.btnRobBackupDir.TabIndex = 6;
            this.btnRobBackupDir.Text = "select";
            this.btnRobBackupDir.UseVisualStyleBackColor = false;
            this.btnRobBackupDir.Click += new System.EventHandler(this.btnRobBackupDir_Click);
            // 
            // labelSigDoBackup
            // 
            this.labelSigDoBackup.AutoSize = true;
            this.labelSigDoBackup.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSigDoBackup.Location = new System.Drawing.Point(18, 22);
            this.labelSigDoBackup.Name = "labelSigDoBackup";
            this.labelSigDoBackup.Size = new System.Drawing.Size(115, 15);
            this.labelSigDoBackup.TabIndex = 5;
            this.labelSigDoBackup.Text = "system input: backup";
            // 
            // labelSigBackupInProg
            // 
            this.labelSigBackupInProg.AutoSize = true;
            this.labelSigBackupInProg.Font = new System.Drawing.Font("GOST Common", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSigBackupInProg.Location = new System.Drawing.Point(19, 70);
            this.labelSigBackupInProg.Name = "labelSigBackupInProg";
            this.labelSigBackupInProg.Size = new System.Drawing.Size(184, 15);
            this.labelSigBackupInProg.TabIndex = 4;
            this.labelSigBackupInProg.Text = "system output: backup in progress";
            // 
            // textSigBackupProg
            // 
            this.textSigBackupProg.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textSigBackupProg.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textSigBackupProg.Location = new System.Drawing.Point(21, 86);
            this.textSigBackupProg.Name = "textSigBackupProg";
            this.textSigBackupProg.ReadOnly = true;
            this.textSigBackupProg.Size = new System.Drawing.Size(193, 23);
            this.textSigBackupProg.TabIndex = 3;
            this.textSigBackupProg.Text = "- select signal -";
            this.textSigBackupProg.Click += new System.EventHandler(this.textSigBackupProg_Click);
            // 
            // textSigDoBackup
            // 
            this.textSigDoBackup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textSigDoBackup.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textSigDoBackup.Location = new System.Drawing.Point(21, 38);
            this.textSigDoBackup.Name = "textSigDoBackup";
            this.textSigDoBackup.ReadOnly = true;
            this.textSigDoBackup.Size = new System.Drawing.Size(193, 23);
            this.textSigDoBackup.TabIndex = 2;
            this.textSigDoBackup.Text = "- select signal -";
            this.textSigDoBackup.Click += new System.EventHandler(this.textSigDoBackup_Click);
            // 
            // checkRobActive
            // 
            this.checkRobActive.BackColor = System.Drawing.Color.White;
            this.checkRobActive.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkRobActive.Location = new System.Drawing.Point(285, 225);
            this.checkRobActive.Name = "checkRobActive";
            this.checkRobActive.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.checkRobActive.Size = new System.Drawing.Size(83, 28);
            this.checkRobActive.TabIndex = 0;
            this.checkRobActive.Text = "active";
            this.checkRobActive.UseVisualStyleBackColor = false;
            this.checkRobActive.CheckedChanged += new System.EventHandler(this.checkActive_CheckedChanged);
            // 
            // timerCheckBackup
            // 
            this.timerCheckBackup.Interval = 60000;
            this.timerCheckBackup.Tick += new System.EventHandler(this.timerCheckBackup_Tick);
            // 
            // groupCommonSettings
            // 
            this.groupCommonSettings.Controls.Add(this.groupWatchStatus);
            this.groupCommonSettings.Controls.Add(this.groupDirClean);
            this.groupCommonSettings.Controls.Add(this.groupOutPath);
            this.groupCommonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupCommonSettings.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupCommonSettings.ForeColor = System.Drawing.Color.Black;
            this.groupCommonSettings.Location = new System.Drawing.Point(15, 399);
            this.groupCommonSettings.Name = "groupCommonSettings";
            this.groupCommonSettings.Size = new System.Drawing.Size(773, 150);
            this.groupCommonSettings.TabIndex = 2;
            this.groupCommonSettings.TabStop = false;
            this.groupCommonSettings.Text = "  COMMON SETTINGS  ";
            // 
            // groupWatchStatus
            // 
            this.groupWatchStatus.Controls.Add(this.btnWatchOff);
            this.groupWatchStatus.Controls.Add(this.btnWatchOn);
            this.groupWatchStatus.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupWatchStatus.Location = new System.Drawing.Point(583, 31);
            this.groupWatchStatus.Name = "groupWatchStatus";
            this.groupWatchStatus.Size = new System.Drawing.Size(174, 106);
            this.groupWatchStatus.TabIndex = 15;
            this.groupWatchStatus.TabStop = false;
            this.groupWatchStatus.Text = "watch status";
            // 
            // btnWatchOff
            // 
            this.btnWatchOff.BackColor = System.Drawing.Color.Red;
            this.btnWatchOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchOff.Location = new System.Drawing.Point(44, 66);
            this.btnWatchOff.Name = "btnWatchOff";
            this.btnWatchOff.Size = new System.Drawing.Size(87, 28);
            this.btnWatchOff.TabIndex = 1;
            this.btnWatchOff.Text = "OFF";
            this.btnWatchOff.UseVisualStyleBackColor = false;
            this.btnWatchOff.Click += new System.EventHandler(this.btnWatchOff_Click);
            // 
            // btnWatchOn
            // 
            this.btnWatchOn.BackColor = System.Drawing.Color.LimeGreen;
            this.btnWatchOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchOn.Location = new System.Drawing.Point(44, 29);
            this.btnWatchOn.Name = "btnWatchOn";
            this.btnWatchOn.Size = new System.Drawing.Size(87, 28);
            this.btnWatchOn.TabIndex = 0;
            this.btnWatchOn.Text = "ON";
            this.btnWatchOn.UseVisualStyleBackColor = false;
            this.btnWatchOn.Click += new System.EventHandler(this.btnWatchOn_Click);
            // 
            // appBackupManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.checkRobActive);
            this.Controls.Add(this.checkPCactive);
            this.Controls.Add(this.groupCommonSettings);
            this.Controls.Add(this.groupRobMaster);
            this.Controls.Add(this.groupPCmaster);
            this.Name = "appBackupManager";
            this.Size = new System.Drawing.Size(807, 565);
            this.groupPCmaster.ResumeLayout(false);
            this.groupBackupTime.ResumeLayout(false);
            this.groupBackupTime.PerformLayout();
            this.groupPCcontrol.ResumeLayout(false);
            this.groupPCcontrol.PerformLayout();
            this.groupInterval.ResumeLayout(false);
            this.groupInterval.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalMins)).EndInit();
            this.groupPCduplicate.ResumeLayout(false);
            this.groupPCduplicate.PerformLayout();
            this.groupDirClean.ResumeLayout(false);
            this.groupDirClean.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClearDays)).EndInit();
            this.groupOutPath.ResumeLayout(false);
            this.groupOutPath.PerformLayout();
            this.groupRobMaster.ResumeLayout(false);
            this.groupDetails.ResumeLayout(false);
            this.groupDetails.PerformLayout();
            this.groupRobotDuplicate.ResumeLayout(false);
            this.groupRobotDuplicate.PerformLayout();
            this.groupRobotSettings.ResumeLayout(false);
            this.groupRobotSettings.PerformLayout();
            this.groupCommonSettings.ResumeLayout(false);
            this.groupWatchStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupPCmaster;
        private System.Windows.Forms.CheckBox checkPCactive;
        private System.Windows.Forms.GroupBox groupRobMaster;
        private System.Windows.Forms.CheckBox checkRobActive;
        private System.Windows.Forms.Button btnOutSelect;
        private System.Windows.Forms.Button btnBackupExe;
        private System.Windows.Forms.RadioButton radioPCIncr;
        private System.Windows.Forms.RadioButton radioPCOverwrite;
        private System.Windows.Forms.Label labelDailySuffix;
        private System.Windows.Forms.TextBox textDailySuffix;
        private System.Windows.Forms.GroupBox groupInterval;
        private System.Windows.Forms.Label labelLastTimePC;
        private System.Windows.Forms.Label labelRecentBackupPC;
        private System.Windows.Forms.GroupBox groupPCduplicate;
        private System.Windows.Forms.GroupBox groupOutPath;
        private System.Windows.Forms.Label labelOutPathVal;
        private System.Windows.Forms.Label labelOutPathTitle;
        private System.Windows.Forms.Label labelIntervalDay;
        private System.Windows.Forms.Label labelIntervalHour;
        private System.Windows.Forms.Label labelIntervalMin;
        private System.Windows.Forms.Label labelDirExists;
        private System.Windows.Forms.NumericUpDown numIntervalDays;
        private System.Windows.Forms.NumericUpDown numIntervalHours;
        private System.Windows.Forms.NumericUpDown numIntervalMins;
        private System.Windows.Forms.Timer timerCheckBackup;
        private System.Windows.Forms.GroupBox groupDirClean;
        private System.Windows.Forms.NumericUpDown numClearDays;
        private System.Windows.Forms.Label labelClearDays;
        private System.Windows.Forms.FolderBrowserDialog dialogOutDir;
        private System.Windows.Forms.ToolTip myToolTip;
        private System.Windows.Forms.Button btnCleanExe;
        private System.Windows.Forms.GroupBox groupBackupTime;
        private System.Windows.Forms.Label labelEveryTime;
        private System.Windows.Forms.MaskedTextBox textEveryTime;
        private System.Windows.Forms.GroupBox groupPCcontrol;
        private System.Windows.Forms.GroupBox groupCommonSettings;
        private System.Windows.Forms.GroupBox groupWatchStatus;
        private System.Windows.Forms.Button btnWatchOff;
        private System.Windows.Forms.Button btnWatchOn;
        private System.Windows.Forms.RadioButton radioPCTime;
        private System.Windows.Forms.Label labelIntervalSuffix;
        private System.Windows.Forms.TextBox textIntervalSuffix;
        private System.Windows.Forms.Label labelGUISuffix;
        private System.Windows.Forms.TextBox textGuiSuffix;
        private System.Windows.Forms.Button btnOutShow;
        private System.Windows.Forms.GroupBox groupRobotSettings;
        private System.Windows.Forms.TextBox textSigBackupProg;
        private System.Windows.Forms.TextBox textSigDoBackup;
        private System.Windows.Forms.GroupBox groupDetails;
        private System.Windows.Forms.Label labelRobotSuffix;
        private System.Windows.Forms.TextBox textRobotSuffix;
        private System.Windows.Forms.GroupBox groupRobotDuplicate;
        private System.Windows.Forms.RadioButton radioROBTime;
        private System.Windows.Forms.RadioButton radioROBOverwrite;
        private System.Windows.Forms.RadioButton radioROBIncr;
        private System.Windows.Forms.Label labelSigDoBackup;
        private System.Windows.Forms.Label labelSigBackupInProg;
        private System.Windows.Forms.Label labelRecentBackupROB;
        private System.Windows.Forms.Label labelLastTimeROB;
        private System.Windows.Forms.Label labelRobBackupDir;
        private System.Windows.Forms.Button btnRobBackupDir;
    }
}
