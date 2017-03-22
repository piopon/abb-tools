namespace abbTools.AppWindowsIPC
{
    partial class windowClientStatus
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("client 1", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("client 2", 0);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("client 3", 1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(windowClientStatus));
            this.panelFormTemplate = new System.Windows.Forms.Panel();
            this.panelVisual = new System.Windows.Forms.Panel();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.panelClientContent = new System.Windows.Forms.Panel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.tableClientStatus = new System.Windows.Forms.TableLayoutPanel();
            this.labelValLastMsg = new System.Windows.Forms.Label();
            this.labelValEvents = new System.Windows.Forms.Label();
            this.labelValMsgExe = new System.Windows.Forms.Label();
            this.labelValMsgSent = new System.Windows.Forms.Label();
            this.labelValMsgRecv = new System.Windows.Forms.Label();
            this.labelValAutoOpen = new System.Windows.Forms.Label();
            this.labelValAutoRecon = new System.Windows.Forms.Label();
            this.labelValStatus = new System.Windows.Forms.Label();
            this.labelTitleRunning = new System.Windows.Forms.Label();
            this.labelTitleMsgExe = new System.Windows.Forms.Label();
            this.labelTitleMsgSent = new System.Windows.Forms.Label();
            this.labelTitleMsgRecv = new System.Windows.Forms.Label();
            this.labelTitleAutoOpen = new System.Windows.Forms.Label();
            this.labelTitleAutoRecon = new System.Windows.Forms.Label();
            this.labelTitleStatus = new System.Windows.Forms.Label();
            this.labelTitleEvents = new System.Windows.Forms.Label();
            this.labelTitleLastMsg = new System.Windows.Forms.Label();
            this.labelValRunning = new System.Windows.Forms.Label();
            this.panelSelectItemInfo = new System.Windows.Forms.Panel();
            this.labelSelectListItem = new System.Windows.Forms.Label();
            this.panelDetailTitle = new System.Windows.Forms.Panel();
            this.labelPanelDetails = new System.Windows.Forms.Label();
            this.listViewClients = new System.Windows.Forms.ListView();
            this.columnClientName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imagesClientStatus = new System.Windows.Forms.ImageList(this.components);
            this.buttonCloseMe = new System.Windows.Forms.Button();
            this.panelVisual.SuspendLayout();
            this.panelDetails.SuspendLayout();
            this.panelClientContent.SuspendLayout();
            this.tableClientStatus.SuspendLayout();
            this.panelSelectItemInfo.SuspendLayout();
            this.panelDetailTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFormTemplate
            // 
            this.panelFormTemplate.BackColor = System.Drawing.Color.White;
            this.panelFormTemplate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormTemplate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.panelFormTemplate.Location = new System.Drawing.Point(110, 154);
            this.panelFormTemplate.Name = "panelFormTemplate";
            this.panelFormTemplate.Size = new System.Drawing.Size(657, 347);
            this.panelFormTemplate.TabIndex = 0;
            this.panelFormTemplate.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panelVisual
            // 
            this.panelVisual.BackColor = System.Drawing.Color.LightGray;
            this.panelVisual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVisual.Controls.Add(this.panelDetails);
            this.panelVisual.Controls.Add(this.listViewClients);
            this.panelVisual.Controls.Add(this.buttonCloseMe);
            this.panelVisual.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.panelVisual.Location = new System.Drawing.Point(110, 154);
            this.panelVisual.Name = "panelVisual";
            this.panelVisual.Size = new System.Drawing.Size(657, 347);
            this.panelVisual.TabIndex = 1;
            // 
            // panelDetails
            // 
            this.panelDetails.BackColor = System.Drawing.Color.White;
            this.panelDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDetails.Controls.Add(this.panelClientContent);
            this.panelDetails.Controls.Add(this.panelSelectItemInfo);
            this.panelDetails.Controls.Add(this.panelDetailTitle);
            this.panelDetails.Location = new System.Drawing.Point(215, 10);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(422, 286);
            this.panelDetails.TabIndex = 2;
            // 
            // panelClientContent
            // 
            this.panelClientContent.Controls.Add(this.buttonRefresh);
            this.panelClientContent.Controls.Add(this.tableClientStatus);
            this.panelClientContent.Location = new System.Drawing.Point(3, 26);
            this.panelClientContent.Name = "panelClientContent";
            this.panelClientContent.Padding = new System.Windows.Forms.Padding(10, 7, 10, 40);
            this.panelClientContent.Size = new System.Drawing.Size(414, 255);
            this.panelClientContent.TabIndex = 1;
            this.panelClientContent.Visible = false;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackColor = System.Drawing.Color.LightGray;
            this.buttonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRefresh.Location = new System.Drawing.Point(10, 224);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(394, 25);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "refresh";
            this.buttonRefresh.UseVisualStyleBackColor = false;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // tableClientStatus
            // 
            this.tableClientStatus.BackColor = System.Drawing.Color.White;
            this.tableClientStatus.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableClientStatus.ColumnCount = 2;
            this.tableClientStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.38677F));
            this.tableClientStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.61323F));
            this.tableClientStatus.Controls.Add(this.labelValLastMsg, 1, 8);
            this.tableClientStatus.Controls.Add(this.labelValEvents, 1, 7);
            this.tableClientStatus.Controls.Add(this.labelValMsgExe, 1, 6);
            this.tableClientStatus.Controls.Add(this.labelValMsgSent, 1, 5);
            this.tableClientStatus.Controls.Add(this.labelValMsgRecv, 1, 4);
            this.tableClientStatus.Controls.Add(this.labelValAutoOpen, 1, 3);
            this.tableClientStatus.Controls.Add(this.labelValAutoRecon, 1, 2);
            this.tableClientStatus.Controls.Add(this.labelValStatus, 1, 1);
            this.tableClientStatus.Controls.Add(this.labelTitleRunning, 0, 0);
            this.tableClientStatus.Controls.Add(this.labelTitleMsgExe, 0, 6);
            this.tableClientStatus.Controls.Add(this.labelTitleMsgSent, 0, 5);
            this.tableClientStatus.Controls.Add(this.labelTitleMsgRecv, 0, 4);
            this.tableClientStatus.Controls.Add(this.labelTitleAutoOpen, 0, 3);
            this.tableClientStatus.Controls.Add(this.labelTitleAutoRecon, 0, 2);
            this.tableClientStatus.Controls.Add(this.labelTitleStatus, 0, 1);
            this.tableClientStatus.Controls.Add(this.labelTitleEvents, 0, 7);
            this.tableClientStatus.Controls.Add(this.labelTitleLastMsg, 0, 8);
            this.tableClientStatus.Controls.Add(this.labelValRunning, 1, 0);
            this.tableClientStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableClientStatus.Location = new System.Drawing.Point(10, 7);
            this.tableClientStatus.Name = "tableClientStatus";
            this.tableClientStatus.RowCount = 9;
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableClientStatus.Size = new System.Drawing.Size(394, 208);
            this.tableClientStatus.TabIndex = 2;
            // 
            // labelValLastMsg
            // 
            this.labelValLastMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValLastMsg.Location = new System.Drawing.Point(147, 185);
            this.labelValLastMsg.Name = "labelValLastMsg";
            this.labelValLastMsg.Size = new System.Drawing.Size(243, 22);
            this.labelValLastMsg.TabIndex = 17;
            this.labelValLastMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValEvents
            // 
            this.labelValEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValEvents.Location = new System.Drawing.Point(147, 162);
            this.labelValEvents.Name = "labelValEvents";
            this.labelValEvents.Size = new System.Drawing.Size(243, 22);
            this.labelValEvents.TabIndex = 16;
            this.labelValEvents.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValMsgExe
            // 
            this.labelValMsgExe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValMsgExe.Location = new System.Drawing.Point(147, 139);
            this.labelValMsgExe.Name = "labelValMsgExe";
            this.labelValMsgExe.Size = new System.Drawing.Size(243, 22);
            this.labelValMsgExe.TabIndex = 15;
            this.labelValMsgExe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValMsgSent
            // 
            this.labelValMsgSent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValMsgSent.Location = new System.Drawing.Point(147, 116);
            this.labelValMsgSent.Name = "labelValMsgSent";
            this.labelValMsgSent.Size = new System.Drawing.Size(243, 22);
            this.labelValMsgSent.TabIndex = 14;
            this.labelValMsgSent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValMsgRecv
            // 
            this.labelValMsgRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValMsgRecv.Location = new System.Drawing.Point(147, 93);
            this.labelValMsgRecv.Name = "labelValMsgRecv";
            this.labelValMsgRecv.Size = new System.Drawing.Size(243, 22);
            this.labelValMsgRecv.TabIndex = 13;
            this.labelValMsgRecv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValAutoOpen
            // 
            this.labelValAutoOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValAutoOpen.Location = new System.Drawing.Point(147, 70);
            this.labelValAutoOpen.Name = "labelValAutoOpen";
            this.labelValAutoOpen.Size = new System.Drawing.Size(243, 22);
            this.labelValAutoOpen.TabIndex = 12;
            this.labelValAutoOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValAutoRecon
            // 
            this.labelValAutoRecon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValAutoRecon.Location = new System.Drawing.Point(147, 47);
            this.labelValAutoRecon.Name = "labelValAutoRecon";
            this.labelValAutoRecon.Size = new System.Drawing.Size(243, 22);
            this.labelValAutoRecon.TabIndex = 11;
            this.labelValAutoRecon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValStatus
            // 
            this.labelValStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValStatus.Location = new System.Drawing.Point(147, 24);
            this.labelValStatus.Name = "labelValStatus";
            this.labelValStatus.Size = new System.Drawing.Size(243, 22);
            this.labelValStatus.TabIndex = 10;
            this.labelValStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleRunning
            // 
            this.labelTitleRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleRunning.Location = new System.Drawing.Point(4, 1);
            this.labelTitleRunning.Name = "labelTitleRunning";
            this.labelTitleRunning.Size = new System.Drawing.Size(136, 22);
            this.labelTitleRunning.TabIndex = 6;
            this.labelTitleRunning.Text = "RUNNING";
            this.labelTitleRunning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleMsgExe
            // 
            this.labelTitleMsgExe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleMsgExe.Location = new System.Drawing.Point(4, 139);
            this.labelTitleMsgExe.Name = "labelTitleMsgExe";
            this.labelTitleMsgExe.Size = new System.Drawing.Size(136, 22);
            this.labelTitleMsgExe.TabIndex = 5;
            this.labelTitleMsgExe.Text = "EXECUTED COUNT";
            this.labelTitleMsgExe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleMsgSent
            // 
            this.labelTitleMsgSent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleMsgSent.Location = new System.Drawing.Point(4, 116);
            this.labelTitleMsgSent.Name = "labelTitleMsgSent";
            this.labelTitleMsgSent.Size = new System.Drawing.Size(136, 22);
            this.labelTitleMsgSent.TabIndex = 4;
            this.labelTitleMsgSent.Text = "SENT COUNT";
            this.labelTitleMsgSent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleMsgRecv
            // 
            this.labelTitleMsgRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleMsgRecv.Location = new System.Drawing.Point(4, 93);
            this.labelTitleMsgRecv.Name = "labelTitleMsgRecv";
            this.labelTitleMsgRecv.Size = new System.Drawing.Size(136, 22);
            this.labelTitleMsgRecv.TabIndex = 3;
            this.labelTitleMsgRecv.Text = "RECEIVED COUNT";
            this.labelTitleMsgRecv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleAutoOpen
            // 
            this.labelTitleAutoOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleAutoOpen.Location = new System.Drawing.Point(4, 70);
            this.labelTitleAutoOpen.Name = "labelTitleAutoOpen";
            this.labelTitleAutoOpen.Size = new System.Drawing.Size(136, 22);
            this.labelTitleAutoOpen.TabIndex = 2;
            this.labelTitleAutoOpen.Text = "AUTO OPEN";
            this.labelTitleAutoOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleAutoRecon
            // 
            this.labelTitleAutoRecon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleAutoRecon.Location = new System.Drawing.Point(4, 47);
            this.labelTitleAutoRecon.Name = "labelTitleAutoRecon";
            this.labelTitleAutoRecon.Size = new System.Drawing.Size(136, 22);
            this.labelTitleAutoRecon.TabIndex = 1;
            this.labelTitleAutoRecon.Text = "RECONNECT";
            this.labelTitleAutoRecon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleStatus
            // 
            this.labelTitleStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleStatus.Location = new System.Drawing.Point(4, 24);
            this.labelTitleStatus.Name = "labelTitleStatus";
            this.labelTitleStatus.Size = new System.Drawing.Size(136, 22);
            this.labelTitleStatus.TabIndex = 0;
            this.labelTitleStatus.Text = "STATUS";
            this.labelTitleStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleEvents
            // 
            this.labelTitleEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleEvents.Location = new System.Drawing.Point(4, 162);
            this.labelTitleEvents.Name = "labelTitleEvents";
            this.labelTitleEvents.Size = new System.Drawing.Size(136, 22);
            this.labelTitleEvents.TabIndex = 7;
            this.labelTitleEvents.Text = "EVENTS SUBSCRIBE";
            this.labelTitleEvents.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitleLastMsg
            // 
            this.labelTitleLastMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleLastMsg.Location = new System.Drawing.Point(4, 185);
            this.labelTitleLastMsg.Name = "labelTitleLastMsg";
            this.labelTitleLastMsg.Size = new System.Drawing.Size(136, 22);
            this.labelTitleLastMsg.TabIndex = 8;
            this.labelTitleLastMsg.Text = "LAST MESSAGES";
            this.labelTitleLastMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelValRunning
            // 
            this.labelValRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelValRunning.Location = new System.Drawing.Point(147, 1);
            this.labelValRunning.Name = "labelValRunning";
            this.labelValRunning.Size = new System.Drawing.Size(243, 22);
            this.labelValRunning.TabIndex = 9;
            this.labelValRunning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSelectItemInfo
            // 
            this.panelSelectItemInfo.BackColor = System.Drawing.Color.Gold;
            this.panelSelectItemInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSelectItemInfo.Controls.Add(this.labelSelectListItem);
            this.panelSelectItemInfo.Location = new System.Drawing.Point(110, 92);
            this.panelSelectItemInfo.Name = "panelSelectItemInfo";
            this.panelSelectItemInfo.Padding = new System.Windows.Forms.Padding(10);
            this.panelSelectItemInfo.Size = new System.Drawing.Size(200, 100);
            this.panelSelectItemInfo.TabIndex = 1;
            // 
            // labelSelectListItem
            // 
            this.labelSelectListItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSelectListItem.Location = new System.Drawing.Point(10, 10);
            this.labelSelectListItem.Name = "labelSelectListItem";
            this.labelSelectListItem.Size = new System.Drawing.Size(178, 78);
            this.labelSelectListItem.TabIndex = 0;
            this.labelSelectListItem.Text = "select item from list...";
            this.labelSelectListItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDetailTitle
            // 
            this.panelDetailTitle.BackColor = System.Drawing.Color.DimGray;
            this.panelDetailTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDetailTitle.Controls.Add(this.labelPanelDetails);
            this.panelDetailTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetailTitle.Location = new System.Drawing.Point(0, 0);
            this.panelDetailTitle.Name = "panelDetailTitle";
            this.panelDetailTitle.Padding = new System.Windows.Forms.Padding(50, 3, 50, 0);
            this.panelDetailTitle.Size = new System.Drawing.Size(420, 24);
            this.panelDetailTitle.TabIndex = 0;
            // 
            // labelPanelDetails
            // 
            this.labelPanelDetails.BackColor = System.Drawing.Color.Transparent;
            this.labelPanelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPanelDetails.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPanelDetails.ForeColor = System.Drawing.Color.White;
            this.labelPanelDetails.Location = new System.Drawing.Point(50, 3);
            this.labelPanelDetails.Name = "labelPanelDetails";
            this.labelPanelDetails.Size = new System.Drawing.Size(318, 19);
            this.labelPanelDetails.TabIndex = 0;
            this.labelPanelDetails.Text = "details...";
            this.labelPanelDetails.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listViewClients
            // 
            this.listViewClients.BackColor = System.Drawing.Color.White;
            this.listViewClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnClientName});
            this.listViewClients.FullRowSelect = true;
            this.listViewClients.GridLines = true;
            this.listViewClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.Checked = true;
            listViewItem3.StateImageIndex = 1;
            this.listViewClients.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.listViewClients.Location = new System.Drawing.Point(11, 10);
            this.listViewClients.MultiSelect = false;
            this.listViewClients.Name = "listViewClients";
            this.listViewClients.Size = new System.Drawing.Size(188, 286);
            this.listViewClients.SmallImageList = this.imagesClientStatus;
            this.listViewClients.TabIndex = 1;
            this.listViewClients.UseCompatibleStateImageBehavior = false;
            this.listViewClients.View = System.Windows.Forms.View.Details;
            this.listViewClients.SelectedIndexChanged += new System.EventHandler(this.listViewClients_SelectedIndexChanged);
            // 
            // columnClientName
            // 
            this.columnClientName.Text = "client";
            this.columnClientName.Width = 184;
            // 
            // imagesClientStatus
            // 
            this.imagesClientStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesClientStatus.ImageStream")));
            this.imagesClientStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imagesClientStatus.Images.SetKeyName(0, "client_stop.png");
            this.imagesClientStatus.Images.SetKeyName(1, "client_run.png");
            this.imagesClientStatus.Images.SetKeyName(2, "client_error.png");
            // 
            // buttonCloseMe
            // 
            this.buttonCloseMe.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonCloseMe.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.buttonCloseMe.FlatAppearance.BorderSize = 2;
            this.buttonCloseMe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseMe.Location = new System.Drawing.Point(265, 305);
            this.buttonCloseMe.Name = "buttonCloseMe";
            this.buttonCloseMe.Size = new System.Drawing.Size(125, 32);
            this.buttonCloseMe.TabIndex = 0;
            this.buttonCloseMe.Text = "ok";
            this.buttonCloseMe.UseVisualStyleBackColor = false;
            this.buttonCloseMe.Click += new System.EventHandler(this.buttonCloseMe_Click);
            // 
            // windowClientStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(877, 654);
            this.Controls.Add(this.panelVisual);
            this.Controls.Add(this.panelFormTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "windowClientStatus";
            this.Opacity = 0.75D;
            this.Text = "windowClientStatus";
            this.panelVisual.ResumeLayout(false);
            this.panelDetails.ResumeLayout(false);
            this.panelClientContent.ResumeLayout(false);
            this.tableClientStatus.ResumeLayout(false);
            this.panelSelectItemInfo.ResumeLayout(false);
            this.panelDetailTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFormTemplate;
        private System.Windows.Forms.Panel panelVisual;
        private System.Windows.Forms.Button buttonCloseMe;
        private System.Windows.Forms.ListView listViewClients;
        private System.Windows.Forms.ColumnHeader columnClientName;
        private System.Windows.Forms.ImageList imagesClientStatus;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.Panel panelDetailTitle;
        private System.Windows.Forms.Label labelPanelDetails;
        private System.Windows.Forms.Panel panelSelectItemInfo;
        private System.Windows.Forms.Label labelSelectListItem;
        private System.Windows.Forms.Panel panelClientContent;
        private System.Windows.Forms.TableLayoutPanel tableClientStatus;
        private System.Windows.Forms.Label labelTitleStatus;
        private System.Windows.Forms.Label labelTitleAutoRecon;
        private System.Windows.Forms.Label labelTitleAutoOpen;
        private System.Windows.Forms.Label labelTitleMsgRecv;
        private System.Windows.Forms.Label labelTitleMsgSent;
        private System.Windows.Forms.Label labelTitleMsgExe;
        private System.Windows.Forms.Label labelTitleRunning;
        private System.Windows.Forms.Label labelTitleEvents;
        private System.Windows.Forms.Label labelTitleLastMsg;
        private System.Windows.Forms.Label labelValLastMsg;
        private System.Windows.Forms.Label labelValEvents;
        private System.Windows.Forms.Label labelValMsgExe;
        private System.Windows.Forms.Label labelValMsgSent;
        private System.Windows.Forms.Label labelValMsgRecv;
        private System.Windows.Forms.Label labelValAutoOpen;
        private System.Windows.Forms.Label labelValAutoRecon;
        private System.Windows.Forms.Label labelValStatus;
        private System.Windows.Forms.Label labelValRunning;
        private System.Windows.Forms.Button buttonRefresh;
    }
}