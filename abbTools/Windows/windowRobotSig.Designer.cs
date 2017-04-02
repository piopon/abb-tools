namespace abbTools.Windows
{
    partial class windowRobotSig
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
            this.panelTemplate = new System.Windows.Forms.Panel();
            this.panelContents = new System.Windows.Forms.Panel();
            this.panelLoading = new System.Windows.Forms.Panel();
            this.labelLoadSignals = new System.Windows.Forms.Label();
            this.listRobotSignals = new System.Windows.Forms.CheckedListBox();
            this.btnUpdateSignals = new System.Windows.Forms.Button();
            this.btnCloseMe = new System.Windows.Forms.Button();
            this.backThread = new System.ComponentModel.BackgroundWorker();
            this.panelContents.SuspendLayout();
            this.panelLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTemplate
            // 
            this.panelTemplate.BackColor = System.Drawing.Color.Black;
            this.panelTemplate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.panelTemplate.Location = new System.Drawing.Point(204, 128);
            this.panelTemplate.Name = "panelTemplate";
            this.panelTemplate.Size = new System.Drawing.Size(450, 350);
            this.panelTemplate.TabIndex = 0;
            this.panelTemplate.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTemplate_Paint);
            // 
            // panelContents
            // 
            this.panelContents.BackColor = System.Drawing.Color.Gainsboro;
            this.panelContents.Controls.Add(this.panelLoading);
            this.panelContents.Controls.Add(this.listRobotSignals);
            this.panelContents.Controls.Add(this.btnUpdateSignals);
            this.panelContents.Controls.Add(this.btnCloseMe);
            this.panelContents.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.panelContents.Location = new System.Drawing.Point(204, 128);
            this.panelContents.Name = "panelContents";
            this.panelContents.Size = new System.Drawing.Size(450, 350);
            this.panelContents.TabIndex = 1;
            // 
            // panelLoading
            // 
            this.panelLoading.BackColor = System.Drawing.Color.Gold;
            this.panelLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLoading.Controls.Add(this.labelLoadSignals);
            this.panelLoading.Location = new System.Drawing.Point(124, 122);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Padding = new System.Windows.Forms.Padding(5, 25, 5, 25);
            this.panelLoading.Size = new System.Drawing.Size(200, 109);
            this.panelLoading.TabIndex = 9;
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
            this.listRobotSignals.Location = new System.Drawing.Point(106, 56);
            this.listRobotSignals.Name = "listRobotSignals";
            this.listRobotSignals.Size = new System.Drawing.Size(239, 238);
            this.listRobotSignals.TabIndex = 8;
            this.listRobotSignals.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listRobotSignals_ItemCheck);
            this.listRobotSignals.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listRobotSignals_KeyDown);
            // 
            // btnUpdateSignals
            // 
            this.btnUpdateSignals.BackColor = System.Drawing.Color.DarkGray;
            this.btnUpdateSignals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateSignals.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnUpdateSignals.Location = new System.Drawing.Point(106, 15);
            this.btnUpdateSignals.Name = "btnUpdateSignals";
            this.btnUpdateSignals.Size = new System.Drawing.Size(239, 27);
            this.btnUpdateSignals.TabIndex = 1;
            this.btnUpdateSignals.Text = "update signals...";
            this.btnUpdateSignals.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpdateSignals.UseVisualStyleBackColor = false;
            this.btnUpdateSignals.Click += new System.EventHandler(this.btnUpdateSignals_Click);
            // 
            // btnCloseMe
            // 
            this.btnCloseMe.BackColor = System.Drawing.Color.DarkOrange;
            this.btnCloseMe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseMe.Location = new System.Drawing.Point(106, 307);
            this.btnCloseMe.Name = "btnCloseMe";
            this.btnCloseMe.Size = new System.Drawing.Size(239, 27);
            this.btnCloseMe.TabIndex = 0;
            this.btnCloseMe.Text = "OK";
            this.btnCloseMe.UseVisualStyleBackColor = false;
            this.btnCloseMe.Click += new System.EventHandler(this.btnCloseMe_Click);
            // 
            // backThread
            // 
            this.backThread.WorkerReportsProgress = true;
            this.backThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backThread_DoWork);
            this.backThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backThread_ProgressChanged);
            this.backThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backThread_RunWorkerCompleted);
            // 
            // windowRobotSig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(859, 607);
            this.Controls.Add(this.panelContents);
            this.Controls.Add(this.panelTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "windowRobotSig";
            this.Opacity = 0.75D;
            this.Text = "windowRobotSig";
            this.panelContents.ResumeLayout(false);
            this.panelLoading.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTemplate;
        private System.Windows.Forms.Panel panelContents;
        private System.Windows.Forms.Button btnUpdateSignals;
        private System.Windows.Forms.Button btnCloseMe;
        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label labelLoadSignals;
        private System.Windows.Forms.CheckedListBox listRobotSignals;
        private System.ComponentModel.BackgroundWorker backThread;
    }
}