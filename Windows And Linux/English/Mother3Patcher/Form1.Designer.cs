namespace Mother3Patcher
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblRomPrompt = new System.Windows.Forms.Label();
            this.txtRomPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkMakeBackup = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(4, 4);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(400, 100);
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lblRomPrompt
            // 
            this.lblRomPrompt.AutoSize = true;
            this.lblRomPrompt.Location = new System.Drawing.Point(7, 111);
            this.lblRomPrompt.Name = "lblRomPrompt";
            this.lblRomPrompt.Size = new System.Drawing.Size(94, 13);
            this.lblRomPrompt.TabIndex = 1;
            this.lblRomPrompt.Text = "MOTHER 3 ROM:";
            // 
            // txtRomPath
            // 
            this.txtRomPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRomPath.Location = new System.Drawing.Point(10, 128);
            this.txtRomPath.Name = "txtRomPath";
            this.txtRomPath.ReadOnly = true;
            this.txtRomPath.Size = new System.Drawing.Size(313, 20);
            this.txtRomPath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(329, 126);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkMakeBackup
            // 
            this.chkMakeBackup.AutoSize = true;
            this.chkMakeBackup.Checked = true;
            this.chkMakeBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMakeBackup.Location = new System.Drawing.Point(16, 154);
            this.chkMakeBackup.Name = "chkMakeBackup";
            this.chkMakeBackup.Size = new System.Drawing.Size(127, 17);
            this.chkMakeBackup.TabIndex = 4;
            this.chkMakeBackup.Text = "Make a backup copy";
            this.chkMakeBackup.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(247, 155);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(157, 44);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Apply patch!";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 206);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.chkMakeBackup);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtRomPath);
            this.Controls.Add(this.lblRomPrompt);
            this.Controls.Add(this.picLogo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MOTHER 3 Fan Translation Patcher";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblRomPrompt;
        private System.Windows.Forms.TextBox txtRomPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkMakeBackup;
        private System.Windows.Forms.Button btnApply;
    }
}

