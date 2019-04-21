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
        public string title = "Sharings";

        public Sharings(PersonalViewsMigration.PersonalViewsMigration pvm)
        {
            InitializeComponent();
            Application.EnableVisualStyles();
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
                foreach (var sharing in sharings.OrderBy(x => x.GetAttributeValue<string>("principaltypecode")))
                {
                    var userteam = "";
                    var lvGroup = listViewSharings.Groups[0];
                    if (sharing.GetAttributeValue<string>("principaltypecode") == "systemuser")
                        userteam = sharing.GetAttributeValue<AliasedValue>("systemuser.domainname").Value.ToString();
                    if (sharing.GetAttributeValue<string>("principaltypecode") == "team")
                    {
                        userteam = sharing.GetAttributeValue<AliasedValue>("team.name").Value.ToString();
                        lvGroup = listViewSharings.Groups[1];
                    }

                    var item = new ListViewItem()
                    {
                        Group = lvGroup,
                        Text = userteam,
                        SubItems =
                        {
                            sharing.GetAttributeValue<int>("accessrightsmask").ToString(),
                            sharing.GetAttributeValue<DateTime>("changedon").ToLocalTime().ToString("dd-MMM-yyyy HH:mm")
                        },
                        Tag = (Guid)sharing.GetAttributeValue<Guid>("principalid")
                    };

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

                    revoked++;
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

        private void listViewSharings_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Checked = e.IsSelected;
        }

        private void Sharings_Load(object sender, EventArgs e)
        {
            this.Text = title;
        }
    }
}
