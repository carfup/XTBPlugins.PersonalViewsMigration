using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carfup.XTBPlugins.AppCode;

namespace Carfup.XTBPlugins.Forms
{
    public partial class Options : Form
    {
        PersonalViewsMigration.PersonalViewsMigration pvm;
        public Options(PersonalViewsMigration.PersonalViewsMigration pvm)
        {
            InitializeComponent();
            this.pvm = pvm;
            PopulateSettings(this.pvm.settings);
        }

        private void PopulateSettings(PluginSettings settings)
        {
            if (settings == null)
            {
                settings = new PluginSettings();
            }

            checkboxAllowStats.Checked = settings.AllowLogUsage != false;
            checkBoxUserDisplayAll.Checked = settings.UsersDisplayAll;
            checkBoxUserDisplayEnabled.Checked = settings.UsersDisplayEnabled;
            checkBoxUserDisplayDisabled.Checked = settings.UsersDisplayDisabled;
            radioButtonSortingOrderAsc.Checked = (settings.SortOrderPref == SortOrder.Ascending || settings.SortOrderPref == null);
            radioButtoradioButtonSortingOrderDesc.Checked = !radioButtonSortingOrderAsc.Checked;
        }

        internal PluginSettings GetSettings()
        {
            var settings = this.pvm.settings;
            settings.AllowLogUsage = checkboxAllowStats.Checked;
            settings.CurrentVersion = PersonalViewsMigration.PersonalViewsMigration.CurrentVersion;
            settings.UsersDisplayAll = checkBoxUserDisplayAll.Checked;
            settings.UsersDisplayDisabled = checkBoxUserDisplayDisabled.Checked;
            settings.UsersDisplayEnabled = checkBoxUserDisplayEnabled.Checked;
            settings.SortOrderPref = (radioButtonSortingOrderAsc.Checked || settings.SortOrderPref == null) ? SortOrder.Ascending : SortOrder.Descending;

            return settings;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.pvm.settings = GetSettings();
            this.pvm.SaveSettings();
            this.Close();
        }

        private void checkBoxUserDisplayAll_CheckedChanged(object sender, EventArgs e)
        {
            var displayAll = checkBoxUserDisplayAll.Checked;

            if (displayAll)
            {
                checkBoxUserDisplayEnabled.Checked = true;
                checkBoxUserDisplayDisabled.Checked = true;
            }

            checkBoxUserDisplayEnabled.Enabled = !displayAll;
            checkBoxUserDisplayDisabled.Enabled = !displayAll;
        }

        private void checkBoxUserDisplayEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxUserDisplayAll.Checked)
                checkBoxUserDisplayDisabled.Checked = !checkBoxUserDisplayEnabled.Checked;
        }

        private void checkBoxUserDisplayDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxUserDisplayAll.Checked)
                checkBoxUserDisplayEnabled.Checked = !checkBoxUserDisplayDisabled.Checked;
        }
    }
}
