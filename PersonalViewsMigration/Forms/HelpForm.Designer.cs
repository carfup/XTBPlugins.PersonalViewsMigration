namespace Carfup.XTBPlugins.Forms
{
    partial class HelpForm
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
            this.buttonCloseHelp = new System.Windows.Forms.Button();
            this.pictureBoxHelp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCloseHelp
            // 
            this.buttonCloseHelp.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCloseHelp.Location = new System.Drawing.Point(917, 506);
            this.buttonCloseHelp.Name = "buttonCloseHelp";
            this.buttonCloseHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonCloseHelp.TabIndex = 2;
            this.buttonCloseHelp.Text = "Close";
            this.buttonCloseHelp.UseVisualStyleBackColor = true;
            this.buttonCloseHelp.Click += new System.EventHandler(this.buttonCloseHelp_Click);
            // 
            // pictureBoxHelp
            // 
            this.pictureBoxHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxHelp.Image = global::Carfup.XTBPlugins.Properties.Resources.helpscreenshot;
            this.pictureBoxHelp.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxHelp.Name = "pictureBoxHelp";
            this.pictureBoxHelp.Size = new System.Drawing.Size(980, 488);
            this.pictureBoxHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxHelp.TabIndex = 4;
            this.pictureBoxHelp.TabStop = false;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 541);
            this.Controls.Add(this.pictureBoxHelp);
            this.Controls.Add(this.buttonCloseHelp);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpForm";
            this.ShowIcon = false;
            this.Text = "HelpForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHelp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCloseHelp;
        private System.Windows.Forms.PictureBox pictureBoxHelp;
    }
}