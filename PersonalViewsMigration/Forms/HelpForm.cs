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
    public partial class Help : Form
    {
        private PersonalViewsMigration.PersonalViewsMigration pvm;
        public Help(PersonalViewsMigration.PersonalViewsMigration pvm)
        {
            InitializeComponent();
            this.pvm = pvm;
            this.pvm.log.LogData(EventType.Event, LogAction.ShowHelpScreen);
        }

        private void buttonCloseHelp_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
