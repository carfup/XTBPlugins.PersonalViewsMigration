namespace Carfup.XTBPlugins.PersonalViewsMigration
{
    partial class PersonalViewsMigration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonalViewsMigration));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCloseTool = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoadUsers = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOptions = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxWhatUsersToDisplayDestination = new System.Windows.Forms.ComboBox();
            this.listViewUsersDestination = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonLoadUserViews = new System.Windows.Forms.Button();
            this.listViewUserViewsList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxWhatUsersToDisplay = new System.Windows.Forms.ComboBox();
            this.buttonLoadUsers = new System.Windows.Forms.Button();
            this.listViewUsers = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteSelectedViews = new System.Windows.Forms.Button();
            this.buttonMigrateSelectedViews = new System.Windows.Forms.Button();
            this.buttonCopySelectedViews = new System.Windows.Forms.Button();
            this.labelDisclaimer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCloseTool,
            this.toolStripButtonLoadUsers,
            this.toolStripButtonOptions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1042, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonCloseTool
            // 
            this.toolStripButtonCloseTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCloseTool.Image = global::Carfup.XTBPlugins.Properties.Resources.close;
            this.toolStripButtonCloseTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCloseTool.Name = "toolStripButtonCloseTool";
            this.toolStripButtonCloseTool.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCloseTool.Text = "Close";
            this.toolStripButtonCloseTool.Click += new System.EventHandler(this.toolStripButtonCloseTool_Click);
            // 
            // toolStripButtonLoadUsers
            // 
            this.toolStripButtonLoadUsers.Image = global::Carfup.XTBPlugins.Properties.Resources.load;
            this.toolStripButtonLoadUsers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoadUsers.Name = "toolStripButtonLoadUsers";
            this.toolStripButtonLoadUsers.Size = new System.Drawing.Size(113, 22);
            this.toolStripButtonLoadUsers.Text = "Load CRM Users";
            this.toolStripButtonLoadUsers.Click += new System.EventHandler(this.toolStripButtonLoadUsers_Click);
            // 
            // toolStripButtonOptions
            // 
            this.toolStripButtonOptions.Image = global::Carfup.XTBPlugins.Properties.Resources.gear;
            this.toolStripButtonOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOptions.Name = "toolStripButtonOptions";
            this.toolStripButtonOptions.Size = new System.Drawing.Size(69, 22);
            this.toolStripButtonOptions.Text = "Options";
            this.toolStripButtonOptions.Click += new System.EventHandler(this.toolStripButtonOptions_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 64);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1026, 425);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.comboBoxWhatUsersToDisplayDestination);
            this.groupBox4.Controls.Add(this.listViewUsersDestination);
            this.groupBox4.Location = new System.Drawing.Point(719, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(304, 419);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "User List Destination";
            // 
            // comboBoxWhatUsersToDisplayDestination
            // 
            this.comboBoxWhatUsersToDisplayDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWhatUsersToDisplayDestination.FormattingEnabled = true;
            this.comboBoxWhatUsersToDisplayDestination.Items.AddRange(new object[] {
            "All",
            "Enabled",
            "Disabled"});
            this.comboBoxWhatUsersToDisplayDestination.Location = new System.Drawing.Point(6, 19);
            this.comboBoxWhatUsersToDisplayDestination.Name = "comboBoxWhatUsersToDisplayDestination";
            this.comboBoxWhatUsersToDisplayDestination.Size = new System.Drawing.Size(291, 21);
            this.comboBoxWhatUsersToDisplayDestination.TabIndex = 7;
            this.comboBoxWhatUsersToDisplayDestination.SelectedIndexChanged += new System.EventHandler(this.comboBoxWhatUsersToDisplayDestination_SelectedIndexChanged);
            // 
            // listViewUsersDestination
            // 
            this.listViewUsersDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUsersDestination.CheckBoxes = true;
            this.listViewUsersDestination.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.listViewUsersDestination.Location = new System.Drawing.Point(8, 48);
            this.listViewUsersDestination.Name = "listViewUsersDestination";
            this.listViewUsersDestination.ShowGroups = false;
            this.listViewUsersDestination.Size = new System.Drawing.Size(289, 352);
            this.listViewUsersDestination.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewUsersDestination.TabIndex = 2;
            this.listViewUsersDestination.UseCompatibleStateImageBehavior = false;
            this.listViewUsersDestination.View = System.Windows.Forms.View.Details;
            this.listViewUsersDestination.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewUsersDestination_ColumnClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "User Email";
            this.columnHeader4.Width = 280;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "State";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonLoadUserViews);
            this.groupBox2.Controls.Add(this.listViewUserViewsList);
            this.groupBox2.Location = new System.Drawing.Point(311, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 419);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Views list";
            // 
            // buttonLoadUserViews
            // 
            this.buttonLoadUserViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadUserViews.Location = new System.Drawing.Point(6, 19);
            this.buttonLoadUserViews.Name = "buttonLoadUserViews";
            this.buttonLoadUserViews.Size = new System.Drawing.Size(290, 23);
            this.buttonLoadUserViews.TabIndex = 4;
            this.buttonLoadUserViews.Text = "Load user\'s views";
            this.buttonLoadUserViews.UseVisualStyleBackColor = true;
            this.buttonLoadUserViews.Click += new System.EventHandler(this.buttonLoadUserViews_Click);
            // 
            // listViewUserViewsList
            // 
            this.listViewUserViewsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUserViewsList.CheckBoxes = true;
            this.listViewUserViewsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewUserViewsList.FullRowSelect = true;
            this.listViewUserViewsList.Location = new System.Drawing.Point(6, 48);
            this.listViewUserViewsList.Name = "listViewUserViewsList";
            this.listViewUserViewsList.Size = new System.Drawing.Size(290, 351);
            this.listViewUserViewsList.TabIndex = 1;
            this.listViewUserViewsList.UseCompatibleStateImageBehavior = false;
            this.listViewUserViewsList.View = System.Windows.Forms.View.Details;
            this.listViewUserViewsList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewUserViewsList_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "View Name";
            this.columnHeader1.Width = 190;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Entity";
            this.columnHeader2.Width = 93;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxWhatUsersToDisplay);
            this.groupBox1.Controls.Add(this.buttonLoadUsers);
            this.groupBox1.Controls.Add(this.listViewUsers);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 419);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User List";
            // 
            // comboBoxWhatUsersToDisplay
            // 
            this.comboBoxWhatUsersToDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWhatUsersToDisplay.FormattingEnabled = true;
            this.comboBoxWhatUsersToDisplay.Items.AddRange(new object[] {
            "All",
            "Enabled",
            "Disabled"});
            this.comboBoxWhatUsersToDisplay.Location = new System.Drawing.Point(154, 19);
            this.comboBoxWhatUsersToDisplay.Name = "comboBoxWhatUsersToDisplay";
            this.comboBoxWhatUsersToDisplay.Size = new System.Drawing.Size(142, 21);
            this.comboBoxWhatUsersToDisplay.TabIndex = 6;
            this.comboBoxWhatUsersToDisplay.SelectedIndexChanged += new System.EventHandler(this.comboBoxWhatUsersToDisplay_SelectedIndexChanged);
            // 
            // buttonLoadUsers
            // 
            this.buttonLoadUsers.Location = new System.Drawing.Point(6, 19);
            this.buttonLoadUsers.Name = "buttonLoadUsers";
            this.buttonLoadUsers.Size = new System.Drawing.Size(142, 23);
            this.buttonLoadUsers.TabIndex = 5;
            this.buttonLoadUsers.Text = "Load users";
            this.buttonLoadUsers.UseVisualStyleBackColor = true;
            this.buttonLoadUsers.Click += new System.EventHandler(this.buttonLoadUsers_Click);
            // 
            // listViewUsers
            // 
            this.listViewUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.State});
            this.listViewUsers.Location = new System.Drawing.Point(6, 48);
            this.listViewUsers.MultiSelect = false;
            this.listViewUsers.Name = "listViewUsers";
            this.listViewUsers.ShowGroups = false;
            this.listViewUsers.Size = new System.Drawing.Size(290, 351);
            this.listViewUsers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewUsers.TabIndex = 1;
            this.listViewUsers.UseCompatibleStateImageBehavior = false;
            this.listViewUsers.View = System.Windows.Forms.View.Details;
            this.listViewUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewUsers_ColumnClick);
            this.listViewUsers.SelectedIndexChanged += new System.EventHandler(this.listViewUsers_SelectedIndexChanged);
            this.listViewUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewUsers_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "User Email";
            this.columnHeader3.Width = 280;
            // 
            // State
            // 
            this.State.Text = "State";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.buttonDeleteSelectedViews);
            this.groupBox3.Controls.Add(this.buttonMigrateSelectedViews);
            this.groupBox3.Controls.Add(this.buttonCopySelectedViews);
            this.groupBox3.Location = new System.Drawing.Point(619, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(94, 419);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Actions";
            // 
            // buttonDeleteSelectedViews
            // 
            this.buttonDeleteSelectedViews.Location = new System.Drawing.Point(13, 81);
            this.buttonDeleteSelectedViews.Name = "buttonDeleteSelectedViews";
            this.buttonDeleteSelectedViews.Size = new System.Drawing.Size(71, 23);
            this.buttonDeleteSelectedViews.TabIndex = 4;
            this.buttonDeleteSelectedViews.Text = "Delete";
            this.buttonDeleteSelectedViews.UseVisualStyleBackColor = true;
            this.buttonDeleteSelectedViews.Click += new System.EventHandler(this.buttonDeleteSelectedViews_Click);
            // 
            // buttonMigrateSelectedViews
            // 
            this.buttonMigrateSelectedViews.Location = new System.Drawing.Point(13, 52);
            this.buttonMigrateSelectedViews.Name = "buttonMigrateSelectedViews";
            this.buttonMigrateSelectedViews.Size = new System.Drawing.Size(71, 23);
            this.buttonMigrateSelectedViews.TabIndex = 3;
            this.buttonMigrateSelectedViews.Text = "ReAssign";
            this.buttonMigrateSelectedViews.UseVisualStyleBackColor = true;
            this.buttonMigrateSelectedViews.Click += new System.EventHandler(this.buttonMigrateSelectedViews_Click);
            // 
            // buttonCopySelectedViews
            // 
            this.buttonCopySelectedViews.Location = new System.Drawing.Point(13, 23);
            this.buttonCopySelectedViews.Name = "buttonCopySelectedViews";
            this.buttonCopySelectedViews.Size = new System.Drawing.Size(71, 23);
            this.buttonCopySelectedViews.TabIndex = 2;
            this.buttonCopySelectedViews.Text = "Copy";
            this.buttonCopySelectedViews.UseVisualStyleBackColor = true;
            this.buttonCopySelectedViews.Click += new System.EventHandler(this.buttonCopySelectedViews_Click);
            // 
            // labelDisclaimer
            // 
            this.labelDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDisclaimer.AutoSize = true;
            this.labelDisclaimer.ForeColor = System.Drawing.Color.Black;
            this.labelDisclaimer.Location = new System.Drawing.Point(83, 26);
            this.labelDisclaimer.Name = "labelDisclaimer";
            this.labelDisclaimer.Size = new System.Drawing.Size(904, 26);
            this.labelDisclaimer.TabIndex = 4;
            this.labelDisclaimer.Text = resources.GetString("labelDisclaimer.Text");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Disclaimer : ";
            // 
            // PersonalViewsMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDisclaimer);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PersonalViewsMigration";
            this.Size = new System.Drawing.Size(1042, 492);
            this.Load += new System.EventHandler(this.PersonalViewsMigration_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCloseTool;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoadUsers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonMigrateSelectedViews;
        private System.Windows.Forms.Button buttonCopySelectedViews;
        private System.Windows.Forms.ListView listViewUserViewsList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView listViewUsers;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button buttonLoadUserViews;
        private System.Windows.Forms.ListView listViewUsersDestination;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader State;
        private System.Windows.Forms.ComboBox comboBoxWhatUsersToDisplay;
        private System.Windows.Forms.Button buttonLoadUsers;
        private System.Windows.Forms.ComboBox comboBoxWhatUsersToDisplayDestination;
        private System.Windows.Forms.Label labelDisclaimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOptions;
        private System.Windows.Forms.Button buttonDeleteSelectedViews;
    }
}
