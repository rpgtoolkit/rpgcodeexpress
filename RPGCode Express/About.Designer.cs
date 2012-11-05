namespace RpgCodeExpress
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.pnlInformation = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblLicense = new System.Windows.Forms.Label();
            this.lblInformation = new System.Windows.Forms.Label();
            this.lblIconLicense = new System.Windows.Forms.Label();
            this.pnlInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInformation
            // 
            this.pnlInformation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlInformation.Controls.Add(this.lblIconLicense);
            this.pnlInformation.Controls.Add(this.pictureBox1);
            this.pnlInformation.Controls.Add(this.lblLicense);
            this.pnlInformation.Controls.Add(this.lblInformation);
            this.pnlInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInformation.Location = new System.Drawing.Point(0, 0);
            this.pnlInformation.Name = "pnlInformation";
            this.pnlInformation.Size = new System.Drawing.Size(550, 239);
            this.pnlInformation.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RpgCodeExpress.Properties.Resources.RPGCode_Express_Logo__Scaled_;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 127);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Location = new System.Drawing.Point(189, 87);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(350, 104);
            this.lblLicense.TabIndex = 1;
            this.lblLicense.Text = resources.GetString("lblLicense.Text");
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.Location = new System.Drawing.Point(189, 9);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(228, 78);
            this.lblInformation.TabIndex = 0;
            this.lblInformation.Text = "RPGCode Express \r\nVersion 1.0\r\nCopyright © Joshua Michael Daly 2012\r\n\r\nA powerful" +
                " code editor for RPGCode version 3.\r\n\r\n";
            // 
            // lblIconLicense
            // 
            this.lblIconLicense.AutoSize = true;
            this.lblIconLicense.Location = new System.Drawing.Point(189, 202);
            this.lblIconLicense.Name = "lblIconLicense";
            this.lblIconLicense.Size = new System.Drawing.Size(319, 26);
            this.lblIconLicense.TabIndex = 3;
            this.lblIconLicense.Text = "Some icons by Yusuke Kamiyamane. All rights reserved. Licensed \r\nunder a Creative" +
                " Commons Attribution 3.0 License.";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 239);
            this.Controls.Add(this.pnlInformation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About RPGCode Express";
            this.pnlInformation.ResumeLayout(false);
            this.pnlInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlInformation;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Label lblLicense;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblIconLicense;

    }
}