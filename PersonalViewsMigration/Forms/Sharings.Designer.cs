namespace Carfup.XTBPlugins.Forms
{
    partial class Sharings
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
            this.btnDeleteSharings = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvSharings = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelSelectAll = new System.Windows.Forms.LinkLabel();
            this.linkLabelUnSelect = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxFilterSharings = new System.Windows.Forms.TextBox();
            this.btnModifySharings = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSharings)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDeleteSharings
            // 
            this.btnDeleteSharings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSharings.Location = new System.Drawing.Point(558, 2);
            this.btnDeleteSharings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeleteSharings.Name = "btnDeleteSharings";
            this.btnDeleteSharings.Size = new System.Drawing.Size(227, 28);
            this.btnDeleteSharings.TabIndex = 13;
            this.btnDeleteSharings.Text = "Delete selected sharings";
            this.btnDeleteSharings.UseVisualStyleBackColor = true;
            this.btnDeleteSharings.Click += new System.EventHandler(this.btnDeleteSharings_Click);
            this.btnDeleteSharings.Leave += new System.EventHandler(this.btnDeleteSharings_Leave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.71682F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.28318F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 193F));
            this.tableLayoutPanel1.Controls.Add(this.dgvSharings, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteSharings, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnModifySharings, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(981, 364);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // dgvSharings
            // 
            this.dgvSharings.AllowUserToAddRows = false;
            this.dgvSharings.AllowUserToDeleteRows = false;
            this.dgvSharings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dgvSharings, 4);
            this.dgvSharings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSharings.Location = new System.Drawing.Point(2, 35);
            this.dgvSharings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvSharings.Name = "dgvSharings";
            this.dgvSharings.RowHeadersWidth = 72;
            this.dgvSharings.RowTemplate.Height = 31;
            this.dgvSharings.Size = new System.Drawing.Size(977, 327);
            this.dgvSharings.TabIndex = 15;
            this.dgvSharings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSharings_CellValueChanged);
            this.dgvSharings.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSharings_ColumnHeaderMouseClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.linkLabelSelectAll);
            this.panel1.Controls.Add(this.linkLabelUnSelect);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(211, 28);
            this.panel1.TabIndex = 15;
            // 
            // linkLabelSelectAll
            // 
            this.linkLabelSelectAll.AutoSize = true;
            this.linkLabelSelectAll.Location = new System.Drawing.Point(11, 6);
            this.linkLabelSelectAll.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelSelectAll.Name = "linkLabelSelectAll";
            this.linkLabelSelectAll.Size = new System.Drawing.Size(51, 13);
            this.linkLabelSelectAll.TabIndex = 17;
            this.linkLabelSelectAll.TabStop = true;
            this.linkLabelSelectAll.Text = "Select All";
            this.linkLabelSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSelectAll_LinkClicked);
            // 
            // linkLabelUnSelect
            // 
            this.linkLabelUnSelect.AutoSize = true;
            this.linkLabelUnSelect.Location = new System.Drawing.Point(65, 6);
            this.linkLabelUnSelect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelUnSelect.Name = "linkLabelUnSelect";
            this.linkLabelUnSelect.Size = new System.Drawing.Size(65, 13);
            this.linkLabelUnSelect.TabIndex = 16;
            this.linkLabelUnSelect.TabStop = true;
            this.linkLabelUnSelect.Text = "UnSelect All";
            this.linkLabelUnSelect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUnSelect_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.textBoxFilterSharings);
            this.panel2.Location = new System.Drawing.Point(217, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(337, 28);
            this.panel2.TabIndex = 19;
            // 
            // textBoxFilterSharings
            // 
            this.textBoxFilterSharings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilterSharings.Location = new System.Drawing.Point(3, 5);
            this.textBoxFilterSharings.Name = "textBoxFilterSharings";
            this.textBoxFilterSharings.Size = new System.Drawing.Size(332, 20);
            this.textBoxFilterSharings.TabIndex = 12;
            this.textBoxFilterSharings.Text = "Search in results ...";
            this.textBoxFilterSharings.Click += new System.EventHandler(this.textBoxFilterSharings_Click);
            this.textBoxFilterSharings.TextChanged += new System.EventHandler(this.textBoxFilterSharings_TextChanged);
            // 
            // btnModifySharings
            // 
            this.btnModifySharings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModifySharings.Location = new System.Drawing.Point(789, 2);
            this.btnModifySharings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnModifySharings.Name = "btnModifySharings";
            this.btnModifySharings.Size = new System.Drawing.Size(190, 28);
            this.btnModifySharings.TabIndex = 20;
            this.btnModifySharings.Text = "Modify Sharings";
            this.btnModifySharings.UseVisualStyleBackColor = true;
            this.btnModifySharings.Click += new System.EventHandler(this.btnModifySharings_Click);
            // 
            // Sharings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 364);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinimizeBox = false;
            this.Name = "Sharings";
            this.ShowIcon = false;
            this.Text = "Sharings";
            this.Load += new System.EventHandler(this.Sharings_Load);
            this.Leave += new System.EventHandler(this.Sharings_Leave);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSharings)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnDeleteSharings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelSelectAll;
        private System.Windows.Forms.LinkLabel linkLabelUnSelect;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxFilterSharings;
        private System.Windows.Forms.Button btnModifySharings;
        private System.Windows.Forms.DataGridView dgvSharings;
    }
}