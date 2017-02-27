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
            this.checkIPCclientState = new System.Windows.Forms.CheckBox();
            this.textMsgToSend = new System.Windows.Forms.TextBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkIPCclientState
            // 
            this.checkIPCclientState.AutoSize = true;
            this.checkIPCclientState.Location = new System.Drawing.Point(22, 16);
            this.checkIPCclientState.Name = "checkIPCclientState";
            this.checkIPCclientState.Size = new System.Drawing.Size(103, 21);
            this.checkIPCclientState.TabIndex = 0;
            this.checkIPCclientState.Text = "IPC CLIENT";
            this.checkIPCclientState.UseVisualStyleBackColor = true;
            this.checkIPCclientState.CheckedChanged += new System.EventHandler(this.checkIPCclientState_CheckedChanged);
            // 
            // textMsgToSend
            // 
            this.textMsgToSend.Location = new System.Drawing.Point(160, 14);
            this.textMsgToSend.Name = "textMsgToSend";
            this.textMsgToSend.Size = new System.Drawing.Size(137, 22);
            this.textMsgToSend.TabIndex = 1;
            this.textMsgToSend.Text = "message";
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(303, 13);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 2;
            this.btnSendMsg.Text = "send";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // appWindowsIPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.textMsgToSend);
            this.Controls.Add(this.checkIPCclientState);
            this.Name = "appWindowsIPC";
            this.Size = new System.Drawing.Size(504, 347);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkIPCclientState;
        private System.Windows.Forms.TextBox textMsgToSend;
        private System.Windows.Forms.Button btnSendMsg;
    }
}
