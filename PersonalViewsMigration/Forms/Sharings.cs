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
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using XrmToolBox.Extensibility;

namespace Carfup.XTBPlugins.Forms
{
    public partial class Sharings : Form
    {
        private PersonalViewsMigration.PersonalViewsMigration pvm;
        public List<Entity> sharingList = null;

        public Sharings(PersonalViewsMigration.PersonalViewsMigration pvm)
        {
            InitializeComponent();
            this.pvm = pvm;
            this.pvm.log.LogData(EventType.Event, LogAction.ShowHelpScreen);
        }

        public void loadSharings(List<Entity> sharings)
        {
            labelNoSharings.Visible = sharings.Count == 0;

            if (sharings.Count == 0)
                return;

            if(sharingList != null)
                sharingList.Clear();
            sharingList = sharings;

            int revoked = 0;

            try
            {
                foreach (var sharing in sharings)
                {
                    var userteam = "";
                    if (sharing.GetAttributeValue<string>("principaltypecode") == "systemuser")
                        userteam = sharing.GetAttributeValue<AliasedValue>("systemuser.fullname").Value.ToString();
                    if (sharing.GetAttributeValue<string>("principaltypecode") == "team")
                        userteam = sharing.GetAttributeValue<AliasedValue>("team.name").Value.ToString();

                    var item = new ListViewItem(userteam);
                    item.SubItems.Add(sharing.GetAttributeValue<int>("accessrightsmask").ToString());
                    item.SubItems.Add(sharing.GetAttributeValue<DateTime>("changedon").ToLocalTime().ToString("dd-MMM-yyyy HH:mm"));
                    item.Tag = (Guid)sharing.GetAttributeValue<Guid>("principalid");

                    listViewSharings.Items.Add(item);

                    revoked++;
                }

                listViewSharings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception e)
            {
                this.pvm.log.LogData(EventType.Exception, LogAction.SharingsToListViewLoaded, e);
                throw;
            }

            this.pvm.log.LogData(EventType.Event, LogAction.SharingsToListViewLoaded);
        }

        private void btnDeleteSharings_Click(object sender, EventArgs e)
        {
            int revoked = 0;

            try
            {
                foreach (ListViewItem sharing in listViewSharings.CheckedItems)
                {
                    var sharingDetails = sharingList.FirstOrDefault(x => x.GetAttributeValue<Guid>("principalid") == (Guid)sharing.Tag);

                    var revokeAccessRequest = new RevokeAccessRequest
                    {
                        Revokee = new EntityReference(sharingDetails.GetAttributeValue<string>("principaltypecode"), (Guid)sharing.Tag),
                        Target = new EntityReference(sharingDetails.GetAttributeValue<string>("objecttypecode"), sharingDetails.GetAttributeValue<Guid>("objectid")),
                    };

                    this.pvm.controllerManager.service.Execute(revokeAccessRequest);
                    listViewSharings.Items.Remove(sharing);
                }

            }
            catch (Exception exception)
            {
                this.pvm.log.LogData(EventType.Exception, LogAction.SharingsRevoked, exception);
                throw;
            }
            

            this.pvm.log.LogData(EventType.Event, LogAction.SharingsRevoked);

            MessageBox.Show( $"You successfully revoked {revoked} sharings.{Environment.NewLine}You may close the this window now.", "Sharings revoked !", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void linkLabelUnSelect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in listViewSharings.Items)
                item.Checked = false;
        }

        private void linkLabelSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in listViewSharings.Items)
                item.Checked = true;
        }
    }
}
