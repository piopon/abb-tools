namespace abbTools
{
    partial class appRemoteABB
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
            this.buttonActionsModify = new System.Windows.Forms.Button();
            this.groupRemoteWatching = new System.Windows.Forms.GroupBox();
            this.listActionsWatch = new System.Windows.Forms.ListView();
            this.watchHeaderSig = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.watchHeaderActor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.watchHeaderResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.watchHeaderApp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonActionsNew = new System.Windows.Forms.Button();
            this.groupRemoteSignals = new System.Windows.Forms.GroupBox();
            this.buttonUpdateSignals = new System.Windows.Forms.Button();
            this.listRobotSignals = new System.Windows.Forms.CheckedListBox();
            this.groupRemoteActions = new System.Windows.Forms.GroupBox();
            this.radioChangeTo1 = new System.Windows.Forms.RadioButton();
            this.labelActionActor = new System.Windows.Forms.Label();
            this.radioChangeTo0 = new System.Windows.Forms.RadioButton();
            this.labelActionResultant = new System.Windows.Forms.Label();
            this.comboResultant = new System.Windows.Forms.ComboBox();
            this.groupRemoteApp = new System.Windows.Forms.GroupBox();
            this.labelAppDir = new System.Windows.Forms.Label();
            this.buttonPCAppSel = new System.Windows.Forms.Button();
            this.labelAppDirTitle = new System.Windows.Forms.Label();
            this.pcAppLocation = new System.Windows.Forms.OpenFileDialog();
            this.buttonActionsRemove = new System.Windows.Forms.Button();
            this.backThread = new System.ComponentModel.BackgroundWorker();
            this.panelLoading = new System.Windows.Forms.Panel();
            this.labelLoadSignals = new System.Windows.Forms.Label();
            this.buttonEditModifier = new System.Windows.Forms.Button();
            this.groupRemoteWatching.SuspendLayout();
            this.groupRemoteSignals.SuspendLayout();
            this.groupRemoteActions.SuspendLayout();
            this.groupRemoteApp.SuspendLayout();
            this.panelLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonActionsModify
            // 
            this.buttonActionsModify.BackColor = System.Drawing.Color.Silver;
            this.buttonActionsModify.Enabled = false;
            this.buttonActionsModify.FlatAppearance.BorderSize = 2;
            this.buttonActionsModify.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonActionsModify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonActionsModify.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonActionsModify.Location = new System.Drawing.Point(681, 189);
            this.buttonActionsModify.Name = "buttonActionsModify";
            this.buttonActionsModify.Size = new System.Drawing.Size(103, 63);
            this.buttonActionsModify.TabIndex = 17;
            this.buttonActionsModify.Text = "MODIFY";
            this.buttonActionsModify.UseVisualStyleBackColor = false;
            this.buttonActionsModify.Click += new System.EventHandler(this.buttonActionsModify_Click);
            // 
            // groupRemoteWatching
            // 
            this.groupRemoteWatching.Controls.Add(this.listActionsWatch);
            this.groupRemoteWatching.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRemoteWatching.Location = new System.Drawing.Point(12, 351);
            this.groupRemoteWatching.Name = "groupRemoteWatching";
            this.groupRemoteWatching.Size = new System.Drawing.Size(772, 191);
            this.groupRemoteWatching.TabIndex = 16;
            this.groupRemoteWatching.TabStop = false;
            this.groupRemoteWatching.Text = " WATCHING";
            // 
            // listActionsWatch
            // 
            this.listActionsWatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listActionsWatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listActionsWatch.CheckBoxes = true;
            this.listActionsWatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.watchHeaderSig,
            this.watchHeaderActor,
            this.watchHeaderResult,
            this.watchHeaderApp});
            this.listActionsWatch.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listActionsWatch.FullRowSelect = true;
            this.listActionsWatch.GridLines = true;
            this.listActionsWatch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listActionsWatch.LabelWrap = false;
            this.listActionsWatch.Location = new System.Drawing.Point(15, 33);
            this.listActionsWatch.MultiSelect = false;
            this.listActionsWatch.Name = "listActionsWatch";
            this.listActionsWatch.ShowGroups = false;
            this.listActionsWatch.Size = new System.Drawing.Size(730, 146);
            this.listActionsWatch.TabIndex = 0;
            this.listActionsWatch.UseCompatibleStateImageBehavior = false;
            this.listActionsWatch.View = System.Windows.Forms.View.Details;
            this.listActionsWatch.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listActionsWatch_ItemChecked);
            // 
            // watchHeaderSig
            // 
            this.watchHeaderSig.Text = "signal";
            this.watchHeaderSig.Width = 115;
            // 
            // watchHeaderActor
            // 
            this.watchHeaderActor.Text = "actor";
            this.watchHeaderActor.Width = 100;
            // 
            // watchHeaderResult
            // 
            this.watchHeaderResult.Text = "result";
            this.watchHeaderResult.Width = 120;
            // 
            // watchHeaderApp
            // 
            this.watchHeaderApp.Text = "application";
            this.watchHeaderApp.Width = 300;
            // 
            // buttonActionsNew
            // 
            this.buttonActionsNew.BackColor = System.Drawing.Color.Silver;
            this.buttonActionsNew.Enabled = false;
            this.buttonActionsNew.FlatAppearance.BorderSize = 2;
            this.buttonActionsNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.YellowGreen;
            this.buttonActionsNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonActionsNew.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonActionsNew.Location = new System.Drawing.Point(681, 29);
            this.buttonActionsNew.Name = "buttonActionsNew";
            this.buttonActionsNew.Size = new System.Drawing.Size(103, 144);
            this.buttonActionsNew.TabIndex = 15;
            this.buttonActionsNew.Text = "NEW";
            this.buttonActionsNew.UseVisualStyleBackColor = false;
            this.buttonActionsNew.Click += new System.EventHandler(this.buttonActionsNew_Click);
            // 
            // groupRemoteSignals
            // 
            this.groupRemoteSignals.Controls.Add(this.panelLoading);
            this.groupRemoteSignals.Controls.Add(this.buttonUpdateSignals);
            this.groupRemoteSignals.Controls.Add(this.listRobotSignals);
            this.groupRemoteSignals.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRemoteSignals.Location = new System.Drawing.Point(12, 18);
            this.groupRemoteSignals.Name = "groupRemoteSignals";
            this.groupRemoteSignals.Size = new System.Drawing.Size(281, 313);
            this.groupRemoteSignals.TabIndex = 14;
            this.groupRemoteSignals.TabStop = false;
            this.groupRemoteSignals.Text = " SIGNALS  ";
            // 
            // buttonUpdateSignals
            // 
            this.buttonUpdateSignals.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonUpdateSignals.Location = new System.Drawing.Point(18, 27);
            this.buttonUpdateSignals.Name = "buttonUpdateSignals";
            this.buttonUpdateSignals.Size = new System.Drawing.Size(239, 25);
            this.buttonUpdateSignals.TabIndex = 6;
            this.buttonUpdateSignals.Text = "update...";
            this.buttonUpdateSignals.UseVisualStyleBackColor = true;
            this.buttonUpdateSignals.Click += new System.EventHandler(this.buttonUpdateSignals_Click);
            // 
            // listRobotSignals
            // 
            this.listRobotSignals.BackColor = System.Drawing.Color.White;
            this.listRobotSignals.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listRobotSignals.FormattingEnabled = true;
            this.listRobotSignals.Location = new System.Drawing.Point(18, 60);
            this.listRobotSignals.Name = "listRobotSignals";
            this.listRobotSignals.Size = new System.Drawing.Size(239, 238);
            this.listRobotSignals.TabIndex = 2;
            this.listRobotSignals.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listRobotSignals_ItemCheck);
            // 
            // groupRemoteActions
            // 
            this.groupRemoteActions.Controls.Add(this.buttonEditModifier);
            this.groupRemoteActions.Controls.Add(this.radioChangeTo1);
            this.groupRemoteActions.Controls.Add(this.labelActionActor);
            this.groupRemoteActions.Controls.Add(this.radioChangeTo0);
            this.groupRemoteActions.Controls.Add(this.labelActionResultant);
            this.groupRemoteActions.Controls.Add(this.comboResultant);
            this.groupRemoteActions.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRemoteActions.Location = new System.Drawing.Point(327, 18);
            this.groupRemoteActions.Name = "groupRemoteActions";
            this.groupRemoteActions.Size = new System.Drawing.Size(324, 155);
            this.groupRemoteActions.TabIndex = 13;
            this.groupRemoteActions.TabStop = false;
            this.groupRemoteActions.Text = " ACTION  ";
            // 
            // radioChangeTo1
            // 
            this.radioChangeTo1.AutoSize = true;
            this.radioChangeTo1.Checked = true;
            this.radioChangeTo1.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioChangeTo1.Location = new System.Drawing.Point(174, 53);
            this.radioChangeTo1.Name = "radioChangeTo1";
            this.radioChangeTo1.Size = new System.Drawing.Size(126, 21);
            this.radioChangeTo1.TabIndex = 7;
            this.radioChangeTo1.TabStop = true;
            this.radioChangeTo1.Text = "sig change to \"1\"";
            this.radioChangeTo1.UseVisualStyleBackColor = true;
            // 
            // labelActionActor
            // 
            this.labelActionActor.AutoSize = true;
            this.labelActionActor.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelActionActor.Location = new System.Drawing.Point(24, 30);
            this.labelActionActor.Name = "labelActionActor";
            this.labelActionActor.Size = new System.Drawing.Size(40, 17);
            this.labelActionActor.TabIndex = 6;
            this.labelActionActor.Text = "actor";
            // 
            // radioChangeTo0
            // 
            this.radioChangeTo0.AutoSize = true;
            this.radioChangeTo0.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioChangeTo0.Location = new System.Drawing.Point(24, 53);
            this.radioChangeTo0.Name = "radioChangeTo0";
            this.radioChangeTo0.Size = new System.Drawing.Size(128, 21);
            this.radioChangeTo0.TabIndex = 5;
            this.radioChangeTo0.Text = "sig change to \"0\"";
            this.radioChangeTo0.UseVisualStyleBackColor = true;
            // 
            // labelActionResultant
            // 
            this.labelActionResultant.AutoSize = true;
            this.labelActionResultant.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelActionResultant.Location = new System.Drawing.Point(24, 85);
            this.labelActionResultant.Name = "labelActionResultant";
            this.labelActionResultant.Size = new System.Drawing.Size(45, 17);
            this.labelActionResultant.TabIndex = 4;
            this.labelActionResultant.Text = "result";
            // 
            // comboResultant
            // 
            this.comboResultant.BackColor = System.Drawing.Color.White;
            this.comboResultant.DisplayMember = "0";
            this.comboResultant.DropDownHeight = 100;
            this.comboResultant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboResultant.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboResultant.FormattingEnabled = true;
            this.comboResultant.IntegralHeight = false;
            this.comboResultant.ItemHeight = 16;
            this.comboResultant.Items.AddRange(new object[] {
            "app execute",
            "app terminate",
            "app restart",
            "app mouse clik",
            "app key press"});
            this.comboResultant.Location = new System.Drawing.Point(24, 107);
            this.comboResultant.Name = "comboResultant";
            this.comboResultant.Size = new System.Drawing.Size(276, 24);
            this.comboResultant.TabIndex = 0;
            this.comboResultant.ValueMember = "0";
            this.comboResultant.SelectedIndexChanged += new System.EventHandler(this.comboResultant_SelectedIndexChanged);
            // 
            // groupRemoteApp
            // 
            this.groupRemoteApp.BackColor = System.Drawing.Color.Transparent;
            this.groupRemoteApp.Controls.Add(this.labelAppDir);
            this.groupRemoteApp.Controls.Add(this.buttonPCAppSel);
            this.groupRemoteApp.Controls.Add(this.labelAppDirTitle);
            this.groupRemoteApp.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupRemoteApp.Location = new System.Drawing.Point(327, 188);
            this.groupRemoteApp.Name = "groupRemoteApp";
            this.groupRemoteApp.Size = new System.Drawing.Size(324, 143);
            this.groupRemoteApp.TabIndex = 12;
            this.groupRemoteApp.TabStop = false;
            this.groupRemoteApp.Text = " PC APPLICATION  ";
            // 
            // labelAppDir
            // 
            this.labelAppDir.AutoSize = true;
            this.labelAppDir.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAppDir.Location = new System.Drawing.Point(24, 101);
            this.labelAppDir.Name = "labelAppDir";
            this.labelAppDir.Size = new System.Drawing.Size(175, 17);
            this.labelAppDir.TabIndex = 6;
            this.labelAppDir.Text = "- application directory -";
            // 
            // buttonPCAppSel
            // 
            this.buttonPCAppSel.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonPCAppSel.Location = new System.Drawing.Point(24, 31);
            this.buttonPCAppSel.Name = "buttonPCAppSel";
            this.buttonPCAppSel.Size = new System.Drawing.Size(276, 33);
            this.buttonPCAppSel.TabIndex = 5;
            this.buttonPCAppSel.Text = "select...";
            this.buttonPCAppSel.UseVisualStyleBackColor = true;
            this.buttonPCAppSel.Click += new System.EventHandler(this.buttonPCAppSel_Click);
            // 
            // labelAppDirTitle
            // 
            this.labelAppDirTitle.AutoSize = true;
            this.labelAppDirTitle.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAppDirTitle.Location = new System.Drawing.Point(21, 80);
            this.labelAppDirTitle.Name = "labelAppDirTitle";
            this.labelAppDirTitle.Size = new System.Drawing.Size(126, 17);
            this.labelAppDirTitle.TabIndex = 4;
            this.labelAppDirTitle.Text = "selected application";
            // 
            // pcAppLocation
            // 
            this.pcAppLocation.DefaultExt = "*.exe";
            this.pcAppLocation.Filter = "exe files|*.exe|all files|*.*";
            this.pcAppLocation.InitialDirectory = "C:\\";
            // 
            // buttonActionsRemove
            // 
            this.buttonActionsRemove.BackColor = System.Drawing.Color.Silver;
            this.buttonActionsRemove.Enabled = false;
            this.buttonActionsRemove.FlatAppearance.BorderSize = 2;
            this.buttonActionsRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonActionsRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonActionsRemove.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonActionsRemove.Location = new System.Drawing.Point(681, 268);
            this.buttonActionsRemove.Name = "buttonActionsRemove";
            this.buttonActionsRemove.Size = new System.Drawing.Size(103, 63);
            this.buttonActionsRemove.TabIndex = 18;
            this.buttonActionsRemove.Text = "DELETE";
            this.buttonActionsRemove.UseVisualStyleBackColor = false;
            this.buttonActionsRemove.Click += new System.EventHandler(this.buttonActionsRemove_Click);
            // 
            // backThread
            // 
            this.backThread.WorkerReportsProgress = true;
            this.backThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backThread_DoWork);
            this.backThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backThread_ProgressChanged);
            this.backThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backThread_RunWorkerCompleted);
            // 
            // panelLoading
            // 
            this.panelLoading.BackColor = System.Drawing.Color.DarkOrange;
            this.panelLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLoading.Controls.Add(this.labelLoadSignals);
            this.panelLoading.Location = new System.Drawing.Point(37, 98);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Size = new System.Drawing.Size(200, 162);
            this.panelLoading.TabIndex = 7;
            this.panelLoading.Visible = false;
            // 
            // labelLoadSignals
            // 
            this.labelLoadSignals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoadSignals.Location = new System.Drawing.Point(0, 0);
            this.labelLoadSignals.Name = "labelLoadSignals";
            this.labelLoadSignals.Size = new System.Drawing.Size(198, 160);
            this.labelLoadSignals.TabIndex = 0;
            this.labelLoadSignals.Text = "reading signals...";
            this.labelLoadSignals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonEditModifier
            // 
            this.buttonEditModifier.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonEditModifier.Location = new System.Drawing.Point(226, 83);
            this.buttonEditModifier.Name = "buttonEditModifier";
            this.buttonEditModifier.Size = new System.Drawing.Size(74, 23);
            this.buttonEditModifier.TabIndex = 8;
            this.buttonEditModifier.Text = "edit...";
            this.buttonEditModifier.UseVisualStyleBackColor = true;
            this.buttonEditModifier.Visible = false;
            // 
            // appRemoteABB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.buttonActionsRemove);
            this.Controls.Add(this.buttonActionsModify);
            this.Controls.Add(this.groupRemoteWatching);
            this.Controls.Add(this.buttonActionsNew);
            this.Controls.Add(this.groupRemoteSignals);
            this.Controls.Add(this.groupRemoteActions);
            this.Controls.Add(this.groupRemoteApp);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "appRemoteABB";
            this.Size = new System.Drawing.Size(807, 565);
            this.groupRemoteWatching.ResumeLayout(false);
            this.groupRemoteSignals.ResumeLayout(false);
            this.groupRemoteActions.ResumeLayout(false);
            this.groupRemoteActions.PerformLayout();
            this.groupRemoteApp.ResumeLayout(false);
            this.groupRemoteApp.PerformLayout();
            this.panelLoading.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonActionsModify;
        private System.Windows.Forms.GroupBox groupRemoteWatching;
        private System.Windows.Forms.ListView listActionsWatch;
        private System.Windows.Forms.ColumnHeader watchHeaderSig;
        private System.Windows.Forms.ColumnHeader watchHeaderActor;
        private System.Windows.Forms.ColumnHeader watchHeaderResult;
        private System.Windows.Forms.ColumnHeader watchHeaderApp;
        private System.Windows.Forms.Button buttonActionsNew;
        private System.Windows.Forms.GroupBox groupRemoteSignals;
        private System.Windows.Forms.CheckedListBox listRobotSignals;
        private System.Windows.Forms.GroupBox groupRemoteActions;
        private System.Windows.Forms.RadioButton radioChangeTo1;
        private System.Windows.Forms.Label labelActionActor;
        private System.Windows.Forms.RadioButton radioChangeTo0;
        private System.Windows.Forms.Label labelActionResultant;
        private System.Windows.Forms.ComboBox comboResultant;
        private System.Windows.Forms.GroupBox groupRemoteApp;
        private System.Windows.Forms.Label labelAppDir;
        private System.Windows.Forms.Button buttonPCAppSel;
        private System.Windows.Forms.Label labelAppDirTitle;
        private System.Windows.Forms.OpenFileDialog pcAppLocation;
        private System.Windows.Forms.Button buttonUpdateSignals;
        private System.Windows.Forms.Button buttonActionsRemove;
        private System.ComponentModel.BackgroundWorker backThread;
        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label labelLoadSignals;
        private System.Windows.Forms.Button buttonEditModifier;
    }
}
