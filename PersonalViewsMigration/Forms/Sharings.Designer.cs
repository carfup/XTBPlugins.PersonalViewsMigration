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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Users", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Teams", System.Windows.Forms.HorizontalAlignment.Left);
            this.listViewSharings = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDeleteSharings = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelSelectAll = new System.Windows.Forms.LinkLabel();
            this.linkLabelUnSelect = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelNoSharings = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewSharings
            // 
            this.listViewSharings.AllowColumnReorder = true;
            this.listViewSharings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSharings.CheckBoxes = true;
            this.listViewSharings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader6,
            this.columnHeader7});
            this.tableLayoutPanel1.SetColumnSpan(this.listViewSharings, 3);
            this.listViewSharings.FullRowSelect = true;
            this.listViewSharings.GridLines = true;
            listViewGroup1.Header = "Users";
            listViewGroup1.Name = "listViewGroupUsers";
            listViewGroup2.Header = "Teams";
            listViewGroup2.Name = "listViewGroupTeams";
            this.listViewSharings.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listViewSharings.Location = new System.Drawing.Point(6, 58);
            this.listViewSharings.Margin = new System.Windows.Forms.Padding(6);
            this.listViewSharings.Name = "listViewSharings";
            this.listViewSharings.Size = new System.Drawing.Size(1349, 603);
            this.listViewSharings.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewSharings.TabIndex = 3;
            this.listViewSharings.UseCompatibleStateImageBehavior = false;
            this.listViewSharings.View = System.Windows.Forms.View.Details;
            this.listViewSharings.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewSharings_ItemSelectionChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Shared With";
            this.columnHeader3.Width = 280;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Access";
            this.columnHeader6.Width = 206;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Shared On";
            this.columnHeader7.Width = 240;
            // 
            // btnDeleteSharings
            // 
            this.btnDeleteSharings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSharings.Location = new System.Drawing.Point(927, 3);
            this.btnDeleteSharings.Name = "btnDeleteSharings";
            this.btnDeleteSharings.Size = new System.Drawing.Size(431, 46);
            this.btnDeleteSharings.TabIndex = 13;
            this.btnDeleteSharings.Text = "Delete selected sharings";
            this.btnDeleteSharings.UseVisualStyleBackColor = true;
            this.btnDeleteSharings.Click += new System.EventHandler(this.btnDeleteSharings_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.31654F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.68346F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 436F));
            this.tableLayoutPanel1.Controls.Add(this.listViewSharings, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteSharings, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.946027F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.05397F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1361, 667);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.linkLabelSelectAll);
            this.panel1.Controls.Add(this.linkLabelUnSelect);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 46);
            this.panel1.TabIndex = 15;
            // 
            // linkLabelSelectAll
            // 
            this.linkLabelSelectAll.AutoSize = true;
            this.linkLabelSelectAll.Location = new System.Drawing.Point(20, 11);
            this.linkLabelSelectAll.Name = "linkLabelSelectAll";
            this.linkLabelSelectAll.Size = new System.Drawing.Size(94, 25);
            this.linkLabelSelectAll.TabIndex = 17;
            this.linkLabelSelectAll.TabStop = true;
            this.linkLabelSelectAll.Text = "Select All";
            this.linkLabelSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSelectAll_LinkClicked);
            // 
            // linkLabelUnSelect
            // 
            this.linkLabelUnSelect.AutoSize = true;
            this.linkLabelUnSelect.Location = new System.Drawing.Point(120, 11);
            this.linkLabelUnSelect.Name = "linkLabelUnSelect";
            this.linkLabelUnSelect.Size = new System.Drawing.Size(119, 25);
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
            this.panel2.Controls.Add(this.labelNoSharings);
            this.panel2.Location = new System.Drawing.Point(440, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(481, 46);
            this.panel2.TabIndex = 19;
            // 
            // labelNoSharings
            // 
            this.labelNoSharings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNoSharings.AutoSize = true;
            this.labelNoSharings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNoSharings.ForeColor = System.Drawing.Color.Red;
            this.labelNoSharings.Location = new System.Drawing.Point(3, 11);
            this.labelNoSharings.Name = "labelNoSharings";
            this.labelNoSharings.Size = new System.Drawing.Size(368, 25);
            this.labelNoSharings.TabIndex = 18;
            this.labelNoSharings.Text = "There are no sharings for this record.";
            this.labelNoSharings.Visible = false;
            // 
            // Sharings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 691);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sharings";
            this.ShowIcon = false;
            this.Text = "Sharings";
            this.Load += new System.EventHandler(this.Sharings_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewSharings;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button btnDeleteSharings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelSelectAll;
        private System.Windows.Forms.LinkLabel linkLabelUnSelect;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelNoSharings;
    }
}