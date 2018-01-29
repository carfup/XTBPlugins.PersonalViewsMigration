namespace Carfup.XTBPlugins.Forms
{
    partial class Options
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.bgStats = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkboxAllowStats = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxUserDisplayDisabled = new System.Windows.Forms.CheckBox();
            this.checkBoxUserDisplayEnabled = new System.Windows.Forms.CheckBox();
            this.checkBoxUserDisplayAll = new System.Windows.Forms.CheckBox();
            this.bgStats.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(450, 219);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(369, 219);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 11;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // bgStats
            // 
            this.bgStats.Controls.Add(this.label1);
            this.bgStats.Controls.Add(this.checkboxAllowStats);
            this.bgStats.Location = new System.Drawing.Point(12, 120);
            this.bgStats.Name = "bgStats";
            this.bgStats.Size = new System.Drawing.Size(513, 93);
            this.bgStats.TabIndex = 10;
            this.bgStats.TabStop = false;
            this.bgStats.Text = "Statistics";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 45);
            this.label1.TabIndex = 6;
            this.label1.Text = "This plugin collects ONLY anonymous usage statistics. No information related your" +
    " CRM / Organization will be retrieve. This will help us to improve the most used" +
    " features !";
            // 
            // checkboxAllowStats
            // 
            this.checkboxAllowStats.AutoSize = true;
            this.checkboxAllowStats.Checked = true;
            this.checkboxAllowStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxAllowStats.Location = new System.Drawing.Point(10, 68);
            this.checkboxAllowStats.Name = "checkboxAllowStats";
            this.checkboxAllowStats.Size = new System.Drawing.Size(94, 17);
            this.checkboxAllowStats.TabIndex = 5;
            this.checkboxAllowStats.Text = "Allow statistics";
            this.checkboxAllowStats.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxUserDisplayDisabled);
            this.groupBox1.Controls.Add(this.checkBoxUserDisplayEnabled);
            this.groupBox1.Controls.Add(this.checkBoxUserDisplayAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Users display";
            // 
            // checkBoxUserDisplayDisabled
            // 
            this.checkBoxUserDisplayDisabled.AutoSize = true;
            this.checkBoxUserDisplayDisabled.Checked = true;
            this.checkBoxUserDisplayDisabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUserDisplayDisabled.Location = new System.Drawing.Point(10, 69);
            this.checkBoxUserDisplayDisabled.Name = "checkBoxUserDisplayDisabled";
            this.checkBoxUserDisplayDisabled.Size = new System.Drawing.Size(67, 17);
            this.checkBoxUserDisplayDisabled.TabIndex = 2;
            this.checkBoxUserDisplayDisabled.Text = "Disabled";
            this.checkBoxUserDisplayDisabled.UseVisualStyleBackColor = true;
            this.checkBoxUserDisplayDisabled.CheckedChanged += new System.EventHandler(this.checkBoxUserDisplayDisabled_CheckedChanged);
            // 
            // checkBoxUserDisplayEnabled
            // 
            this.checkBoxUserDisplayEnabled.AutoSize = true;
            this.checkBoxUserDisplayEnabled.Checked = true;
            this.checkBoxUserDisplayEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUserDisplayEnabled.Location = new System.Drawing.Point(10, 47);
            this.checkBoxUserDisplayEnabled.Name = "checkBoxUserDisplayEnabled";
            this.checkBoxUserDisplayEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxUserDisplayEnabled.TabIndex = 1;
            this.checkBoxUserDisplayEnabled.Text = "Enabled";
            this.checkBoxUserDisplayEnabled.UseVisualStyleBackColor = true;
            this.checkBoxUserDisplayEnabled.CheckedChanged += new System.EventHandler(this.checkBoxUserDisplayEnabled_CheckedChanged);
            // 
            // checkBoxUserDisplayAll
            // 
            this.checkBoxUserDisplayAll.AutoSize = true;
            this.checkBoxUserDisplayAll.Checked = true;
            this.checkBoxUserDisplayAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUserDisplayAll.Location = new System.Drawing.Point(10, 24);
            this.checkBoxUserDisplayAll.Name = "checkBoxUserDisplayAll";
            this.checkBoxUserDisplayAll.Size = new System.Drawing.Size(37, 17);
            this.checkBoxUserDisplayAll.TabIndex = 0;
            this.checkBoxUserDisplayAll.Text = "All";
            this.checkBoxUserDisplayAll.UseVisualStyleBackColor = true;
            this.checkBoxUserDisplayAll.CheckedChanged += new System.EventHandler(this.checkBoxUserDisplayAll_CheckedChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 298);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.bgStats);
            this.Name = "Options";
            this.ShowIcon = false;
            this.Text = "Options";
            this.bgStats.ResumeLayout(false);
            this.bgStats.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox bgStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkboxAllowStats;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxUserDisplayDisabled;
        private System.Windows.Forms.CheckBox checkBoxUserDisplayEnabled;
        private System.Windows.Forms.CheckBox checkBoxUserDisplayAll;
    }
}