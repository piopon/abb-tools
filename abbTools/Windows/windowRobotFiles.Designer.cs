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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("ABB", 1, 1);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("KAWASAKI", 2, 2);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("parent", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(windowRobotFiles));
            this.panelTemplate = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelFilesExplorer = new System.Windows.Forms.Label();
            this.treeRobotDirs = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
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
            this.panelContent.Controls.Add(this.btnClear);
            this.panelContent.Controls.Add(this.btnUpdate);
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
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Gray;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(294, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(104, 30);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Gray;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Location = new System.Drawing.Point(33, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(241, 30);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "update...";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.OrangeRed;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(232, 422);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 45);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Chartreuse;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(33, 422);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(166, 45);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "ok";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelFilesExplorer
            // 
            this.labelFilesExplorer.AutoSize = true;
            this.labelFilesExplorer.Location = new System.Drawing.Point(30, 51);
            this.labelFilesExplorer.Name = "labelFilesExplorer";
            this.labelFilesExplorer.Size = new System.Drawing.Size(125, 17);
            this.labelFilesExplorer.TabIndex = 1;
            this.labelFilesExplorer.Text = "robot files explorer";
            // 
            // treeRobotDirs
            // 
            this.treeRobotDirs.FullRowSelect = true;
            this.treeRobotDirs.HideSelection = false;
            this.treeRobotDirs.ImageIndex = 0;
            this.treeRobotDirs.ImageList = this.imageList;
            this.treeRobotDirs.Location = new System.Drawing.Point(33, 71);
            this.treeRobotDirs.Name = "treeRobotDirs";
            treeNode1.ImageIndex = 1;
            treeNode1.Name = "nodeChildABB";
            treeNode1.SelectedImageIndex = 1;
            treeNode1.Text = "ABB";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "nodeChildKAW";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "KAWASAKI";
            treeNode3.Name = "nodeParent";
            treeNode3.Text = "parent";
            this.treeRobotDirs.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.treeRobotDirs.SelectedImageIndex = 0;
            this.treeRobotDirs.Size = new System.Drawing.Size(365, 333);
            this.treeRobotDirs.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "filesHome.png");
            this.imageList.Images.SetKeyName(1, "filesFolder.png");
            this.imageList.Images.SetKeyName(2, "filesDoc.png");
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
            this.Shown += new System.EventHandler(this.windowRobotFiles_Shown);
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
        private System.Windows.Forms.Button btnUpdate;
        private System.ComponentModel.BackgroundWorker backThread;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button btnClear;
    }
}