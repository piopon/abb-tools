namespace abbTools
{
    partial class windowInput
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.groupInputText = new System.Windows.Forms.GroupBox();
            this.checkAutoMouse = new System.Windows.Forms.CheckBox();
            this.checkAutoKeyboard = new System.Windows.Forms.CheckBox();
            this.buttonClearInput = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.RichTextBox();
            this.labelNoteInputText = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelWindowTitle = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelContent.SuspendLayout();
            this.groupInputText.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Orange;
            this.buttonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonOK.Location = new System.Drawing.Point(11, 16);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(123, 34);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.inputButton_Click);
            // 
            // panelContent
            // 
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContent.AutoSize = true;
            this.panelContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelContent.Controls.Add(this.labelMessage);
            this.panelContent.Controls.Add(this.groupInputText);
            this.panelContent.Location = new System.Drawing.Point(6, 56);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(5);
            this.panelContent.Size = new System.Drawing.Size(520, 250);
            this.panelContent.TabIndex = 1;
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMessage.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelMessage.ForeColor = System.Drawing.Color.DarkOrange;
            this.labelMessage.Location = new System.Drawing.Point(5, 5);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMessage.Size = new System.Drawing.Size(510, 88);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "my message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupInputText
            // 
            this.groupInputText.Controls.Add(this.checkAutoMouse);
            this.groupInputText.Controls.Add(this.checkAutoKeyboard);
            this.groupInputText.Controls.Add(this.buttonClearInput);
            this.groupInputText.Controls.Add(this.textBoxInput);
            this.groupInputText.Controls.Add(this.labelNoteInputText);
            this.groupInputText.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupInputText.Location = new System.Drawing.Point(5, 96);
            this.groupInputText.Name = "groupInputText";
            this.groupInputText.Size = new System.Drawing.Size(507, 146);
            this.groupInputText.TabIndex = 0;
            this.groupInputText.TabStop = false;
            this.groupInputText.Text = " input text ";
            // 
            // checkAutoMouse
            // 
            this.checkAutoMouse.AutoSize = true;
            this.checkAutoMouse.Location = new System.Drawing.Point(23, 90);
            this.checkAutoMouse.Name = "checkAutoMouse";
            this.checkAutoMouse.Size = new System.Drawing.Size(77, 21);
            this.checkAutoMouse.TabIndex = 4;
            this.checkAutoMouse.Text = "auto fill";
            this.checkAutoMouse.UseVisualStyleBackColor = true;
            this.checkAutoMouse.CheckedChanged += new System.EventHandler(this.checkAuto_CheckedChanged);
            // 
            // checkAutoKeyboard
            // 
            this.checkAutoKeyboard.AutoSize = true;
            this.checkAutoKeyboard.Location = new System.Drawing.Point(213, 90);
            this.checkAutoKeyboard.Name = "checkAutoKeyboard";
            this.checkAutoKeyboard.Size = new System.Drawing.Size(77, 21);
            this.checkAutoKeyboard.TabIndex = 3;
            this.checkAutoKeyboard.Text = "auto fill";
            this.checkAutoKeyboard.UseVisualStyleBackColor = true;
            this.checkAutoKeyboard.CheckedChanged += new System.EventHandler(this.checkAuto_CheckedChanged);
            // 
            // buttonClearInput
            // 
            this.buttonClearInput.Location = new System.Drawing.Point(408, 85);
            this.buttonClearInput.Name = "buttonClearInput";
            this.buttonClearInput.Size = new System.Drawing.Size(75, 28);
            this.buttonClearInput.TabIndex = 2;
            this.buttonClearInput.Text = "clear";
            this.buttonClearInput.UseVisualStyleBackColor = true;
            this.buttonClearInput.Click += new System.EventHandler(this.buttonClearInput_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.AcceptsTab = true;
            this.textBoxInput.BackColor = System.Drawing.Color.White;
            this.textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxInput.DetectUrls = false;
            this.textBoxInput.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxInput.Location = new System.Drawing.Point(23, 40);
            this.textBoxInput.Multiline = false;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.textBoxInput.Size = new System.Drawing.Size(460, 35);
            this.textBoxInput.TabIndex = 1;
            this.textBoxInput.Text = "myInput";
            this.textBoxInput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxInput_MouseClick);
            this.textBoxInput.Enter += new System.EventHandler(this.textBoxInput_Enter);
            this.textBoxInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxInput_KeyPress);
            this.textBoxInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxInput_KeyUp);
            this.textBoxInput.Leave += new System.EventHandler(this.textBoxInput_Leave);
            // 
            // labelNoteInputText
            // 
            this.labelNoteInputText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelNoteInputText.Location = new System.Drawing.Point(3, 127);
            this.labelNoteInputText.Name = "labelNoteInputText";
            this.labelNoteInputText.Size = new System.Drawing.Size(501, 16);
            this.labelNoteInputText.TabIndex = 0;
            this.labelNoteInputText.Text = "NOTE: use commas to separate several inputs (in action order)";
            this.labelNoteInputText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Gray;
            this.panelTop.Controls.Add(this.labelWindowTitle);
            this.panelTop.Controls.Add(this.labelHeader);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(1);
            this.panelTop.Size = new System.Drawing.Size(531, 54);
            this.panelTop.TabIndex = 2;
            // 
            // labelWindowTitle
            // 
            this.labelWindowTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelWindowTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelWindowTitle.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelWindowTitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.labelWindowTitle.Location = new System.Drawing.Point(1, 1);
            this.labelWindowTitle.Name = "labelWindowTitle";
            this.labelWindowTitle.Size = new System.Drawing.Size(529, 16);
            this.labelWindowTitle.TabIndex = 0;
            this.labelWindowTitle.Text = "abbTools - input window";
            this.labelWindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelHeader.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelHeader.ForeColor = System.Drawing.Color.White;
            this.labelHeader.Location = new System.Drawing.Point(1, 17);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(529, 36);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "my header";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.Gray;
            this.panelBottom.Controls.Add(this.buttonYes);
            this.panelBottom.Controls.Add(this.buttonNo);
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Controls.Add(this.buttonOK);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 312);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(531, 61);
            this.panelBottom.TabIndex = 3;
            // 
            // buttonYes
            // 
            this.buttonYes.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Orange;
            this.buttonYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonYes.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonYes.Location = new System.Drawing.Point(398, 16);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(123, 34);
            this.buttonYes.TabIndex = 3;
            this.buttonYes.Text = "YES";
            this.buttonYes.UseVisualStyleBackColor = false;
            // 
            // buttonNo
            // 
            this.buttonNo.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.buttonNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Orange;
            this.buttonNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNo.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonNo.Location = new System.Drawing.Point(269, 16);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(123, 34);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "NO";
            this.buttonNo.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Orange;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonCancel.Location = new System.Drawing.Point(140, 16);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(123, 34);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // panelBackground
            // 
            this.panelBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBackground.Controls.Add(this.panelTop);
            this.panelBackground.Controls.Add(this.panelBottom);
            this.panelBackground.Controls.Add(this.panelContent);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(533, 375);
            this.panelBackground.TabIndex = 4;
            // 
            // windowInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(533, 375);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "windowInput";
            this.Text = "windowInput";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.windowInput_FormClosing);
            this.panelContent.ResumeLayout(false);
            this.groupInputText.ResumeLayout(false);
            this.groupInputText.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.GroupBox groupInputText;
        private System.Windows.Forms.Label labelNoteInputText;
        private System.Windows.Forms.Label labelWindowTitle;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.RichTextBox textBoxInput;
        private System.Windows.Forms.Button buttonClearInput;
        private System.Windows.Forms.CheckBox checkAutoMouse;
        private System.Windows.Forms.CheckBox checkAutoKeyboard;
    }
}