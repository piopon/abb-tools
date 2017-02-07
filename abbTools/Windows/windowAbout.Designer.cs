namespace abbTools
{
    partial class windowAbout
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
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.labelInfoYear = new System.Windows.Forms.Label();
            this.labelInfoAuthor = new System.Windows.Forms.Label();
            this.labelInfoTop = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.labelButtonOK = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelWindowTitle = new System.Windows.Forms.Label();
            this.panelBackground.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.LightGray;
            this.panelBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBackground.Controls.Add(this.panelContent);
            this.panelBackground.Controls.Add(this.panelBottom);
            this.panelBackground.Controls.Add(this.panelTop);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.ForeColor = System.Drawing.Color.Black;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(645, 322);
            this.panelBackground.TabIndex = 0;
            // 
            // panelContent
            // 
            this.panelContent.BackgroundImage = global::abbTools.Properties.Resources.windowAbout_back;
            this.panelContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelContent.Controls.Add(this.labelInfoYear);
            this.panelContent.Controls.Add(this.labelInfoAuthor);
            this.panelContent.Controls.Add(this.labelInfoTop);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 50);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(643, 220);
            this.panelContent.TabIndex = 3;
            // 
            // labelInfoYear
            // 
            this.labelInfoYear.AutoSize = true;
            this.labelInfoYear.BackColor = System.Drawing.Color.Transparent;
            this.labelInfoYear.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInfoYear.Location = new System.Drawing.Point(351, 89);
            this.labelInfoYear.Name = "labelInfoYear";
            this.labelInfoYear.Size = new System.Drawing.Size(118, 17);
            this.labelInfoYear.TabIndex = 2;
            this.labelInfoYear.Text = "license: MIT © 2017";
            // 
            // labelInfoAuthor
            // 
            this.labelInfoAuthor.AutoSize = true;
            this.labelInfoAuthor.BackColor = System.Drawing.Color.Transparent;
            this.labelInfoAuthor.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInfoAuthor.Location = new System.Drawing.Point(351, 72);
            this.labelInfoAuthor.Name = "labelInfoAuthor";
            this.labelInfoAuthor.Size = new System.Drawing.Size(198, 17);
            this.labelInfoAuthor.TabIndex = 1;
            this.labelInfoAuthor.Text = "author: piopon.github@gmail.com";
            // 
            // labelInfoTop
            // 
            this.labelInfoTop.AutoSize = true;
            this.labelInfoTop.BackColor = System.Drawing.Color.Transparent;
            this.labelInfoTop.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInfoTop.Location = new System.Drawing.Point(350, 48);
            this.labelInfoTop.Name = "labelInfoTop";
            this.labelInfoTop.Size = new System.Drawing.Size(179, 24);
            this.labelInfoTop.TabIndex = 0;
            this.labelInfoTop.Text = "abbTools - ver. 1.0.0";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.DimGray;
            this.panelBottom.Controls.Add(this.labelButtonOK);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 270);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(643, 50);
            this.panelBottom.TabIndex = 2;
            // 
            // labelButtonOK
            // 
            this.labelButtonOK.BackColor = System.Drawing.Color.DarkOrange;
            this.labelButtonOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelButtonOK.Font = new System.Drawing.Font("GOST Common", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelButtonOK.ForeColor = System.Drawing.Color.Black;
            this.labelButtonOK.Location = new System.Drawing.Point(147, 8);
            this.labelButtonOK.Margin = new System.Windows.Forms.Padding(0);
            this.labelButtonOK.Name = "labelButtonOK";
            this.labelButtonOK.Size = new System.Drawing.Size(357, 34);
            this.labelButtonOK.TabIndex = 0;
            this.labelButtonOK.Text = "OK";
            this.labelButtonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelButtonOK.Click += new System.EventHandler(this.labelButtonOK_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.DimGray;
            this.panelTop.Controls.Add(this.labelWindowTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(643, 50);
            this.panelTop.TabIndex = 1;
            // 
            // labelWindowTitle
            // 
            this.labelWindowTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWindowTitle.Font = new System.Drawing.Font("GOST Common", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelWindowTitle.ForeColor = System.Drawing.Color.White;
            this.labelWindowTitle.Location = new System.Drawing.Point(0, 0);
            this.labelWindowTitle.Name = "labelWindowTitle";
            this.labelWindowTitle.Size = new System.Drawing.Size(643, 50);
            this.labelWindowTitle.TabIndex = 0;
            this.labelWindowTitle.Text = "about ...";
            this.labelWindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // windowAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(645, 322);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "windowAbout";
            this.Text = "about abbTools...";
            this.panelBackground.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelWindowTitle;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelButtonOK;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label labelInfoAuthor;
        private System.Windows.Forms.Label labelInfoTop;
        private System.Windows.Forms.Label labelInfoYear;
    }
}