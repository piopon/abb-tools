namespace abbTools.Windows
{
    partial class windowRobotFiles
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
            this.panelContent = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelFilesExplorer = new System.Windows.Forms.Label();
            this.treeRobotDirs = new System.Windows.Forms.TreeView();
            this.backThread = new System.ComponentModel.BackgroundWorker();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTemplate
            // 
            this.panelTemplate.BackColor = System.Drawing.Color.Black;
            this.panelTemplate.Location = new System.Drawing.Point(297, 98);
            this.panelTemplate.Name = "panelTemplate";
            this.panelTemplate.Size = new System.Drawing.Size(431, 492);
            this.panelTemplate.TabIndex = 0;
            this.panelTemplate.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTemplate_Paint);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.LightGray;
            this.panelContent.Controls.Add(this.button1);
            this.panelContent.Controls.Add(this.btnCancel);
            this.panelContent.Controls.Add(this.btnOK);
            this.panelContent.Controls.Add(this.labelFilesExplorer);
            this.panelContent.Controls.Add(this.treeRobotDirs);
            this.panelContent.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.panelContent.Location = new System.Drawing.Point(297, 98);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(431, 492);
            this.panelContent.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(289, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.OrangeRed;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(231, 408);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 45);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Chartreuse;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(83, 408);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(117, 45);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "ok";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelFilesExplorer
            // 
            this.labelFilesExplorer.AutoSize = true;
            this.labelFilesExplorer.Location = new System.Drawing.Point(80, 39);
            this.labelFilesExplorer.Name = "labelFilesExplorer";
            this.labelFilesExplorer.Size = new System.Drawing.Size(125, 17);
            this.labelFilesExplorer.TabIndex = 1;
            this.labelFilesExplorer.Text = "robot files explorer";
            // 
            // treeRobotDirs
            // 
            this.treeRobotDirs.Location = new System.Drawing.Point(83, 58);
            this.treeRobotDirs.Name = "treeRobotDirs";
            this.treeRobotDirs.Size = new System.Drawing.Size(265, 333);
            this.treeRobotDirs.TabIndex = 0;
            // 
            // backThread
            // 
            this.backThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backThread_DoWork);
            this.backThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backThread_RunWorkerCompleted);
            // 
            // windowRobotFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1024, 689);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "windowRobotFiles";
            this.Opacity = 0.75D;
            this.Text = "windowRobotFiles";
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTemplate;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TreeView treeRobotDirs;
        private System.Windows.Forms.Label labelFilesExplorer;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker backThread;
    }
}