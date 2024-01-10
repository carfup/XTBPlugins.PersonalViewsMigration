using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Reflection;
using System.Web.UI.WebControls;
using System.IO;
using XrmToolBox.Extensibility.Interfaces;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Concurrent;
using Microsoft.Xrm.Sdk.Messages;
using Carfup.XTBPlugins.AppCode;
using Microsoft.Crm.Sdk.Messages;
using System.Diagnostics;
using Carfup.XTBPlugins.Forms;
using McTools.Xrm.Connection;
using XrmToolBox;
using Options = Carfup.XTBPlugins.Forms.Options;

namespace Carfup.XTBPlugins.PersonalViewsMigration
{
    public partial class PersonalViewsMigration : PluginControlBase, IGitHubPlugin, IPayPalPlugin
    {
        #region varibables
        private List<Entity> listOfUsers = null;
        private List<Entity> listOfTeams = null;
        private List<Entity> listOfUserViews = null;
        private List<Entity> listOfUserCharts = null;
        private List<Entity> listOfUserDashboards = null;
        public ControllerManager controllerManager = null;
        internal PluginSettings settings = new PluginSettings();
        public LogUsageManager log = null;
        private int currentColumnOrder;

        public string RepositoryName => "XTBPlugins.PersonalViewsMigration";
        public string UserName => "carfup";
        public string EmailAccount => "clement@carfup.com";
        public string DonationDescription => "Thanks a lot for your support, this really mean something to me, and push me to keep going for sure ! Long life to Personal User view migration ! =)";

        #endregion
        public PersonalViewsMigration()
        {
            InitializeComponent();
        }

        private void PersonalViewsMigration_Load(object sender, EventArgs e)
        {
            comboBoxWhatUsersToDisplay.SelectedIndex = 0;
            comboBoxWhatUsersToDisplayDestination.SelectedIndex = 0;

            

            log = new LogUsageManager(this);
            log.LogData(EventType.Event, LogAction.SettingLoaded);
            LoadSetting();
            ManageDisplayUsingSettings();

            if (ConnectionDetail != null && ConnectionDetail.ServiceClient != null)
            {
                // creating the controller
                controllerManager = new ControllerManager(ConnectionDetail.ServiceClient);

                IsOnlineOrg(ConnectionDetail);
            }

            ManageDisplayOfFormComponents(false);

            if (controllerManager != null && controllerManager.isOnPrem)
                log.LogData(EventType.Event, LogAction.EnvironmentOnPremise);
        }

        private void IsOnlineOrg(ConnectionDetail cd)
        {
            if (cd == null || cd.UseOnline)
                return;

            // changing the disclaimer message if OnPrem instance
            labelDisclaimer.Text =
                "Make sure you have the necessary permissions to perform actions within the plugin.\nThe needed privilege is : \"prvActOnBehalfOfAnotherUser\" included in the Delegate security role.";

            controllerManager.isOnPrem = true;

            // if onprem , we force the list to enabled only
            comboBoxWhatUsersToDisplayDestination.SelectedItem = "Enabled";
            comboBoxWhatUsersToDisplayDestination.Enabled = false;
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            controllerManager = new ControllerManager(detail.ServiceClient);
            IsOnlineOrg(detail);

            base.UpdateConnection(newService, detail, actionName, parameter);
        }

        private void toolStripButtonCloseTool_Click(object sender, System.EventArgs e)
        {
            this.log.LogData(EventType.Event, LogAction.PluginClosed);

            // Saving settings for the next usage of plugin
            SaveSettings();

            // Making sure that all message are sent if stats are enabled
            this.log.Flush();

            CloseTool();
        }

        private void toolStripButtonLoadUsers_Click(object sender, System.EventArgs evt)
        {
            ExecuteMethod(LoadUsersIntoListView);
        }

        private void LoadUsersIntoListView()
        {
            if (!controllerManager.isOnPrem && !controllerManager.userManager.CheckIfNonInteractiveSeatAvailable())
            {
                MessageBox.Show("It seems that all Non Interactive seats are used.\n\nPlease free at least one to enjoy the functionalities of this plugin !", "Warning, Non Interactive seat needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading CRM Users...",
                Work = (bw, e) =>
                {
                    listOfUsers = controllerManager.userManager.GetListOfUsers();
                    listOfTeams = controllerManager.userManager.GetListOfTeams();
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        this.log.LogData(EventType.Exception, LogAction.UsersLoaded, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (listOfUsers != null)
                    {
                        listViewUsers.Items.Clear();
                        listViewUsersDestination.Items.Clear();

                        // We filter the results based on the selection defined
                        var userToKeep = ManageUsersToDisplay();
                        if (userToKeep == null)
                            return;

                        ManageUsersToDisplay("destination");

                        ManageDisplayOfFormComponents(true);

                        this.log.LogData(EventType.Event, LogAction.UsersLoaded);

                        listViewUsersDestination.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listViewUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void ManageDisplayOfFormComponents(bool enable)
        {
            comboBoxWhatUsersToDisplay.Enabled = enable;
            comboBoxWhatUsersToDisplayDestination.Enabled = enable;
            buttonLoadUserViews.Enabled = enable;
            buttonLoadUserDashboards.Enabled = enable;
            buttonLoadUserCharts.Enabled = enable;
            buttonCopySelectedViews.Enabled = enable;
            buttonMigrateSelectedViews.Enabled = enable;
            buttonDeleteSelectedViews.Enabled = enable;
            btnViewSharings.Enabled = enable;
            btnConvertToSystemView.Enabled = enable;
            textBoxFilterUsersDestination.Enabled = enable;
            textBoxFilterUsers.Enabled = enable;
            textBoxFilterCharts.Enabled = enable;
            textBoxFilterDashboards.Enabled = enable;
            textBoxFilterViews.Enabled = enable;

            // if onprem , we force the list to enabled only
            if (controllerManager == null || controllerManager.isOnPrem)
            {
                comboBoxWhatUsersToDisplayDestination.SelectedItem = "Enabled";
                comboBoxWhatUsersToDisplayDestination.Enabled = false;
            }
        }
        
        private void buttonCopySelectedViews_Click(object sender, EventArgs evt)
        {
            string type = null;
            ListView listViewUserData = null;
            string action = null;
            List<Entity> listOfUserData = null;
            string typeLogicalName = null;
            Dictionary<int,List<Entity>> sharingsToCopy = new Dictionary<int, List<Entity>>();

            switch (tabControlUserData.SelectedTab.Name)
            {
                case "tabPageCharts":
                    type = UserDataType.Charts;
                    listViewUserData = listViewUserChartsList;
                    action = LogAction.ChartsCopied;
                    listOfUserData = listOfUserCharts;
                    typeLogicalName = UserDataType.ChartLogicalName;
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.DashboardsCopied;
                    listOfUserData = listOfUserDashboards;
                    typeLogicalName = UserDataType.DashboardLogicalName;
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.ViewsCopied;
                    listOfUserData = listOfUserViews;
                    typeLogicalName = UserDataType.ViewLogicalName;
                    break;
            }


            ListViewItem[] dataGuid = new ListViewItem[listViewUserData.CheckedItems.Count];
            ListViewItem[] usersGuid = new ListViewItem[listViewUsersDestination.CheckedItems.Count];

            if (!usersGuid.Any())
            {
                MessageBox.Show("Please select at least one destination user to perform the Copy action.");
                return;
            }
            if (!dataGuid.Any())
            {
                MessageBox.Show($"Please select at least one {type} to perform a Copy action.");
                return;
            }

            bool success = true;
            bool copySharings = false;

            var copySharingsDialogResult = MessageBox.Show("Do you want to copy the sharings (if any) as well ?",
                "Copy sharings as well ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (copySharingsDialogResult == DialogResult.Yes)
                copySharings = true;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Copying the user {type}(s) ...",
                Work = (bw, e) =>
                {
                    
                    Invoke(new Action(() =>
                    {
                        listViewUserData.CheckedItems.CopyTo(dataGuid,0);
                        listViewUsersDestination.CheckedItems.CopyTo(usersGuid, 0);
                    }));

                    if (dataGuid == null && usersGuid == null)
                        return;


                    var requestWithResults = new ExecuteMultipleRequest()
                    {
                        // Assign settings that define execution behavior: continue on error, return responses. 
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = false,
                            ReturnResponses = true
                        },
                        // Create an empty organization request collection.
                        Requests = new OrganizationRequestCollection()
                    };

                    var isUserFromModified = controllerManager.userManager.ManageImpersonification(false, controllerManager.userFrom);
                    for(int i = 0; i < dataGuid.Length; i++)
                    {
                        ListViewItem itemView = dataGuid[i];
                            Entity itemEntity = listOfUserData.Find(x => x.Id == (Guid) itemView.Tag);

                        if (copySharings)
                        {
                            sharingsToCopy.Add(i,
                                controllerManager.dataManager.retriveRecordSharings((Guid) itemView.Tag,
                                    typeLogicalName));
                        }

                        CreateRequest cr = new CreateRequest();
                        switch (type)
                        {
                            case UserDataType.Charts:
                                cr.Target = this.controllerManager.chartManager.PrepareChartToMigrate(itemEntity);
                                break;
                            case UserDataType.Dashboards:
                                cr.Target = this.controllerManager.dashboardManager.PrepareDashboardToMigrate(itemEntity);
                                break;
                            default:
                                cr.Target = this.controllerManager.viewManager.PrepareViewToMigrate(itemEntity);
                                break;
                        }

                        requestWithResults.Requests.Add(cr);
                    }

                    if(isUserFromModified)
                    {
                        bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                        controllerManager.userManager.ManageImpersonification(isUserFromModified, controllerManager.userFrom);
                    }

                    bw.ReportProgress(0, $"Migrating user {type}s...");
                    foreach (ListViewItem itemUser in usersGuid)
                    {
                        var userId = (Guid) itemUser.Tag;

                        controllerManager.UpdateCallerId(userId);
                        controllerManager.userDestination = controllerManager.userManager.SetUserType(userId, itemUser.Text);

                        // Check if we need to switch to NonInteractive mode
                        bw.ReportProgress(0, "Checking destination user accessibility...");
                        var isUserModified = controllerManager.userManager.ManageImpersonification();

                        // check if user has any roles assigned
                        if (!controllerManager.userManager.UserHasAnyRole(controllerManager.userDestination))
                        {
                            if (usersGuid.Length == 1)
                            {
                                MessageBox.Show("The selected user has no security roles assigned.\nMake sure you assign at least one security role in order to perform any action for this user", "Warning, Security role needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                success = false;
                            }

                            continue;
                        }

                        bw.ReportProgress(0, $"Copying the {type}(s)...");
                        ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)controllerManager.serviceClient.Execute(requestWithResults);

                        if (isUserModified)
                        {
                            bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                            controllerManager.userManager.ManageImpersonification(isUserModified);
                        }

                        for(int j = 0; j< responseWithResults.Responses.Count; j++)
                        {
                            var assigneEntity = itemUser.Text == "team" ? itemUser.Text : "systemuser";
                            var responseItem = responseWithResults.Responses[j];
                            if (responseItem.Fault == null && itemUser.Text == "team")
                            {
                                
                                AssignRequest ar = new AssignRequest()
                                {
                                    Assignee = new EntityReference(assigneEntity, (Guid)itemUser.Tag),
                                    Target = new EntityReference(listOfUserData.FirstOrDefault().LogicalName, (Guid)responseItem.Response.Results["id"])
                                };
                                controllerManager.serviceClient.Execute(ar);
                            }

                            if (responseItem.Fault == null && copySharings)
                            {
                                bw.ReportProgress(0, "Copying the sharings...");
                                foreach (var sharing in sharingsToCopy[j])
                                {
                                    GrantAccessRequest grant = new GrantAccessRequest()
                                    {
                                        Target = new EntityReference(typeLogicalName, (Guid)responseItem.Response.Results["id"]),
                                        PrincipalAccess = new PrincipalAccess()
                                        {
                                            AccessMask = sharing.GetAttributeValue<AccessRights>("accessrightsmask"),
                                            Principal = new EntityReference(sharing.GetAttributeValue<string>("principaltypecode"), sharing.GetAttributeValue<Guid>("principalid"))
                                        }
                                    };

                                    controllerManager.serviceClient.Execute(grant);
                                }
                            }
                            // An error has occurred.
                            if (responseItem.Fault != null)
                                throw new Exception(responseItem.Fault.Message);
                        }
                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        this.log.LogData(EventType.Exception, action, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        if (!success) return;

                        log.LogData(EventType.Event, action);
                        MessageBox.Show($"{dataGuid.Length} {type}{(dataGuid.Length == 1 ? "was" : "(s) were")} copied !", "Successful copy.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void buttonDeleteSelectedViews_Click(object sender, EventArgs evt)
        {
            string type = null;
            ListView listViewUserData = null;
            string action = null;
            string entityDataToDelete = null;

            switch (tabControlUserData.SelectedTab.Name)
            {
                case "tabPageCharts":
                    type = UserDataType.Charts;
                    listViewUserData = listViewUserChartsList;
                    action = LogAction.ChartsDeleted;
                    entityDataToDelete = "userqueryvisualization";
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.DashboardsDeleted;
                    entityDataToDelete = "userform";
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.ViewsDeleted;
                    entityDataToDelete = "userquery";
                    break;
            }

            ListViewItem[] dataGuid = new ListViewItem[listViewUserData.CheckedItems.Count];

            // We make sure that the user really want to delete the view
            var areYouSure = MessageBox.Show($"Do you really want to delete the {type}(s) ? \rYou won't be able to get it back after.", "Warning !", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (areYouSure == DialogResult.No)
                return;

            if (!dataGuid.Any())
            {
                MessageBox.Show($"Please select at least one {type} to perform the Delete action.");
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Deleting the user {type}(s) ...",
                Work = (bw, e) =>
                {

                    Invoke(new Action(() =>
                    {
                        listViewUserData.CheckedItems.CopyTo(dataGuid, 0);
                    }));

                    if (dataGuid == null)
                        return;

                    foreach (ListViewItem itemView in dataGuid)
                    {
                        bw.ReportProgress(0, $"Deleting the user {type}(s)...");

                        controllerManager.UpdateCallerId(controllerManager.userFrom.userId.Value);
                        controllerManager.userDestination = controllerManager.userFrom;

                        // Check if we need to switch to NonInteractive mode
                        bw.ReportProgress(0, "Checking user accessibility...");
                        var isUserModified = controllerManager.userManager.ManageImpersonification();

                        DeleteRequest dr = new DeleteRequest()
                        {
                            Target = new EntityReference(entityDataToDelete, (Guid)itemView.Tag)
                        };

                        controllerManager.serviceClient.Execute(dr);

                        if (isUserModified)
                        {
                            bw.ReportProgress(0, "Setting back the user to Read/Write mode...");
                            controllerManager.userManager.ManageImpersonification(isUserModified);
                        }
                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        log.LogData(EventType.Exception, action, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        foreach (ListViewItem view in dataGuid.ToList())
                            listViewUserData.Items.Remove(view);

                        log.LogData(EventType.Event, action);
                        MessageBox.Show($"{dataGuid.Length} {type}{(dataGuid.Length == 1 ? " was" : "(s) were")} deleted !", "Successful deletion.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void btnConvertToSystemView_Click(object sender, EventArgs evt)
        {
            string type = null;
            ListView listViewUserData = null;
            string action = null;
            List<Entity> listOfUserData = null;

            switch (tabControlUserData.SelectedTab.Name)
            {
                case "tabPageCharts":
                    type = UserDataType.Charts;
                    listViewUserData = listViewUserChartsList;
                    action = LogAction.Personal2SystemChart;
                    listOfUserData = listOfUserCharts;
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.Personal2SystemDashboard;
                    listOfUserData = listOfUserDashboards;
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.Personal2SystemView;
                    listOfUserData = listOfUserViews;
                    break;
            }

            if (listViewUserData.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a record in order to view the related sharings.", "Select a record first.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            ListViewItem[] dataGuid = new ListViewItem[listViewUserData.CheckedItems.Count];

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Converting the personal {type}(s) to system...",
                Work = (bw, e) =>
                {
                    Invoke(new Action(() =>
                    {
                        listViewUserData.CheckedItems.CopyTo(dataGuid, 0);
                    }));

                    foreach (ListViewItem item in dataGuid)
                    {
                        Entity itemEntity = listOfUserData.Find(x => x.Id == (Guid)item.Tag);
                        Entity systemToCreate = null;
                        switch (type)
                        {
                            case UserDataType.Charts:
                                systemToCreate = this.controllerManager.chartManager.ConvertChartToMigrate(itemEntity);
                                break;
                            case UserDataType.Dashboards:
                                systemToCreate = this.controllerManager.dashboardManager.ConvertDashboardToSystem(itemEntity);
                                break;
                            default:
                                systemToCreate = this.controllerManager.viewManager.ConvertViewToSystem(itemEntity);
                                break;
                        }
                        

                        this.Service.Create(systemToCreate);
                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        log.LogData(EventType.Exception, action, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        log.LogData(EventType.Event, action);
                        MessageBox.Show($@"{dataGuid.Length} {type}{(dataGuid.Length == 1 ? " was" : "(s) were")} converted in system !", @"Successful conversion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void btnViewSharings_Click(object sender, EventArgs evt)
        {
            List<Entity> sharings = new List<Entity>()
            {
                new Entity("principalobjectaccess")
                {
                    ["name"] = "crwd",
                    ["principaltypecode"] = "systemuser",
                    ["systemuser.domainname"] = "clement@carfup.com",
                    ["accessrightsmask"] = 65543,
                    ["principalid"] = new Guid()
                },
                new Entity("principalobjectaccess") {
                    ["name"] = "rw",
                    ["principaltypecode"] = "systemuser",
                    ["systemuser.domainname"] = "clement@carfup.com",
                    ["accessrightsmask"] = 6,
                    ["principalid"] = new Guid()
                }
            };

            var diagSharings2 = new Sharings(this);

            if (diagSharings2.sharingList != null)
                diagSharings2.sharingList.Clear();

            //diagSharings2.loadSharings(sharings);
          //  diagSharings.title = $"Sharings for \"{itemToVerify.Text}\"";

           // diagSharings.isUserModified = isUserModified;

          //  diagSharings.Show();

            
            ListView listViewUserData = null;
            string entityDataToMigrate = null;

            switch (tabControlUserData.SelectedTab.Name)
            {
                case "tabPageCharts":
                    listViewUserData = listViewUserChartsList;
                    entityDataToMigrate = UserDataType.ChartLogicalName;
                    break;
                case "tabPageDashboards":
                    listViewUserData = listViewUserDashboardsList;
                    entityDataToMigrate = UserDataType.DashboardLogicalName;
                    break;
                default:
                    listViewUserData = listViewUserViewsList;
                    entityDataToMigrate = UserDataType.ViewLogicalName;
                    break;
            }

            if (listViewUserData.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a record in order to view the related sharings.", "Select a record first.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (listViewUserData.CheckedItems.Count > 1)
            {
                MessageBox.Show("Please select one record at a time.", "One record at a time.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var itemToVerify = listViewUserData.CheckedItems[0];
            var subItemIndexTag = tabControlUserData.SelectedTab.Name == "tabPageDashboards" ? 1 : 2;

            // Managing owner if it"s shared item
            var userForCRMCall = (itemToVerify.Group.Header == "Shared records")
                ? this.controllerManager.userManager.SetUserType(((EntityReference)itemToVerify.SubItems[subItemIndexTag].Tag).Id, ((EntityReference)itemToVerify.SubItems[subItemIndexTag].Tag).LogicalName)
                : controllerManager.userFrom;
            var isUserModified = false;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Retrieving Sharings ...",
                Work = (bw, e) =>
                    {
                        isUserModified = controllerManager.userManager.ManageImpersonification(false, userForCRMCall);
                        e.Result = this.controllerManager.dataManager.retriveRecordSharings((Guid)itemToVerify.Tag, entityDataToMigrate);

                    },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        log.LogData(EventType.Exception, LogAction.SharingsLoaded, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        var result = e.Result as List<Entity>;

                        if (result.Count == 0)
                        {
                            MessageBox.Show("There are no sharings for this record.", "No Sharings",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var diagSharings = new Sharings(this);

                        if (diagSharings.sharingList != null)
                            diagSharings.sharingList.Clear();

                        diagSharings.loadSharings(result);
                        diagSharings.title = $"Sharings for \"{itemToVerify.Text}\"";

                        diagSharings.isUserModified = isUserModified;

                        diagSharings.Show();

                        log.LogData(EventType.Event, LogAction.SharingsLoaded);
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
            
        }

        private void buttonMigrateSelectedViews_Click(object sender, System.EventArgs evt)
        {
            string type = null;
            ListView listViewUserData = null;
            string action = null;
            string entityDataToMigrate = null;

            switch (tabControlUserData.SelectedTab.Name)
            {
                case "tabPageCharts":
                    type = UserDataType.Charts;
                    listViewUserData = listViewUserChartsList;
                    action = LogAction.ChartsReAssigned;
                    entityDataToMigrate = "userqueryvisualization";
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.DashboardsReAssigned;
                    entityDataToMigrate = "userform";
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.ViewsReAssigned;
                    entityDataToMigrate = "userquery";
                    break;
            }

            ListViewItem[] dataGuid = new ListViewItem[listViewUserData.CheckedItems.Count];
            ListViewItem[] usersGuid = new ListViewItem[listViewUsersDestination.CheckedItems.Count];

            if(usersGuid.Count() > 1)
            {
                MessageBox.Show("You can't select more than one user for the ReAssign functionality");
                return;
            }

            if (!usersGuid.Any())
            {
                MessageBox.Show("Please select at least one destination user to perform the ReAssign action.");
                return;
            }

            if (!dataGuid.Any())
            {
                MessageBox.Show($"Please select at least one {type} to perform a ReAssign action.");
                return;
            }


            WorkAsync(new WorkAsyncInfo
            {
                Message = $"ReAssigning the user {type}(s) ...",
                Work = (bw, e) =>
                {

                    Invoke(new Action(() =>
                    {
                        listViewUserData.CheckedItems.CopyTo(dataGuid, 0);
                        listViewUsersDestination.CheckedItems.CopyTo(usersGuid, 0);
                    }));

                    if (dataGuid == null && usersGuid == null)
                        return;

                    foreach (ListViewItem itemView in dataGuid)
                    {
                        bw.ReportProgress(0, $"Changing ownership of the {type}s...");

                        foreach (ListViewItem itemUser in usersGuid)
                        {
                            controllerManager.UpdateCallerId(controllerManager.userFrom.userId.Value);
                            controllerManager.userDestination = controllerManager.userFrom;

                            // Check if we need to switch to NonInteractive mode
                            bw.ReportProgress(0, "Checking destination user accessibility...");
                            bool isUserModified = controllerManager.userManager.ManageImpersonification();


                            //proxy.CallerId = (Guid)itemUser.Tag;
                            bw.ReportProgress(0, $"Changing ownership of the {type}(s)...");

                            var assigneEntity = itemUser.Text == "team" ? itemUser.Text : "systemuser";
                            AssignRequest ar = new AssignRequest
                            {
                                Assignee = new EntityReference(assigneEntity, (Guid)itemUser.Tag),
                                Target = new EntityReference(entityDataToMigrate, (Guid)itemView.Tag)
                            };

                            controllerManager.serviceClient.Execute(ar);

                            if (isUserModified)
                            {
                                bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                                controllerManager.userManager.ManageImpersonification(isUserModified);
                            }
                        } 
                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        log.LogData(EventType.Exception, action, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        foreach(ListViewItem view in dataGuid.ToList())
                            listViewUserData.Items.Remove(view);

                        log.LogData(EventType.Event, action);
                        MessageBox.Show($"{dataGuid.Length} {type}{(dataGuid.Length == 1 ? " was" : "(s) were")} reassigned !", "Successful reassignment", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void buttonLoadUserDashboards_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadUserData, UserDataType.Dashboards);
        }

        private void buttonLoadUserCharts_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadUserData, UserDataType.Charts);
        }

        private void buttonLoadUserViews_Click(object sender, EventArgs evt)
        {
            ExecuteMethod(LoadUserData, UserDataType.Views);
        }

        private void listViewUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Loading views by default on doubleclick
            ExecuteMethod(LoadUserData, UserDataType.Views);

            // Clearing the charts and dashboards
            listViewUserChartsList.Items.Clear();
            listViewUserDashboardsList.Items.Clear();
        }

        private void LoadUserData(string type)
        {
            if (listViewUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one user.");
                return;
            }

            this.controllerManager.userFrom = this.controllerManager.userManager.SetUserType((Guid)listViewUsers.SelectedItems[0].Tag,listViewUsers.SelectedItems[0].Text);
            this.controllerManager.userDestination = this.controllerManager.userManager.SetUserType((Guid)listViewUsers.SelectedItems[0].Tag, listViewUsers.SelectedItems[0].Text);

            var userDestination = this.controllerManager.userDestination.userId.Value;

            if (this.controllerManager.userDestination.userEntity == "systemuser")
                this.controllerManager.UpdateCallerId(userDestination);

            List<Entity> listOfUserData = null;
            string actionToDo = null;
            ListView listViewOfData = null;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Retrieving User {type}(s)...",
                Work = (bw, e) =>
                {
                    bw.ReportProgress(0, "Checking user accessibility...");
                    var isUserModified = controllerManager.userManager.ManageImpersonification();

                    if (!controllerManager.userManager.UserHasAnyRole(this.controllerManager.userDestination))
                    {
                        MessageBox.Show("The selected user has no security roles assigned.\nMake sure you assign at least one security role in order to perform any action for this user", "Warning, Security role needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bw.ReportProgress(0, $"Retrieving user's {type}(s)...");

                    switch (type)
                    {
                        case UserDataType.Charts:
                            listOfUserData = controllerManager.chartManager.ListOfUserCharts(this.controllerManager.userDestination);
                            listOfUserCharts = listOfUserData;
                            actionToDo = LogAction.UserChartsLoaded;
                            listViewOfData = listViewUserChartsList;
                            break;
                        case UserDataType.Dashboards:
                            listOfUserData = controllerManager.dashboardManager.ListOfUserDashboards(this.controllerManager.userDestination);
                            listOfUserDashboards = listOfUserData;
                            actionToDo = LogAction.UserDashboardsLoaded;
                            listViewOfData = listViewUserDashboardsList;
                            break;
                        default:
                            listOfUserData = controllerManager.viewManager.ListOfUserViews(this.controllerManager.userDestination);
                            listOfUserViews = listOfUserData;
                            actionToDo = LogAction.UserViewsLoaded;
                            listViewOfData = listViewUserViewsList;
                            break;
                    }

                    if (isUserModified)
                    {
                        bw.ReportProgress(0, "Setting back the user to Read/Write mode...");
                        controllerManager.userManager.ManageImpersonification(isUserModified);

                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        log.LogData(EventType.Exception, actionToDo, e.Error);
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if(listOfUserData != null)
                    {
                        ManageUserDataToDisplay(listViewOfData, listOfUserData, type);
                    }

                    if (listOfUserData != null && !listOfUserData.Any())
                        MessageBox.Show($"This user has no personal {type}(s) associated to his user account.", $"No {type}(s) available.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    log.LogData(EventType.Event, actionToDo);
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void listViewUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (listViewUsers.SelectedItems.Count > 0)
                {
                    buttonLoadUserViews.Text = $"Load user's views";
                    buttonLoadUserCharts.Text = $"Load user's charts";
                    buttonLoadUserDashboards.Text = $"Load user's dashboards";
                    buttonLoadUserViews.Enabled = true;
                }
                else { 
                    buttonLoadUserViews.Text = "Select an user to load its views.";
                    buttonLoadUserCharts.Text = "Select an user to load its charts.";
                    buttonLoadUserDashboards.Text = "Select an user to load its dashboards.";
                    buttonLoadUserViews.Enabled = false;
                }
            }));
            
        }

        private void buttonLoadUsers_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadUsersIntoListView);
        }

        private void comboBoxWhatUsersToDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManageUsersToDisplay();
            listViewUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void comboBoxWhatUsersToDisplayDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManageUsersToDisplay("destination");
            listViewUsersDestination.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private List<Entity> ManageUsersToDisplay(string type = "source", string filter = null)
        {
            ListView listToAccess = listViewUsers;

            if (type == "destination")
                listToAccess = listViewUsersDestination;

            // cleaning the list first
            listToAccess.Items.Clear();

            // avoid exception on first load
            if (listOfUsers == null)
                return null;

            var bothLists = listOfUsers.Union(listOfTeams).ToList();
            var usersToKeep = bothLists;

            string comboxBoxValue = comboBoxWhatUsersToDisplay.Text;
            if (type == "destination")
            {
                comboxBoxValue = comboBoxWhatUsersToDisplayDestination.Text;
            }

            if(comboxBoxValue == "Enabled")
                usersToKeep = bothLists.Where(x => !x.GetAttributeValue<bool>("isdisabled")).ToList();
            else if (comboxBoxValue == "Disabled")
                usersToKeep = bothLists.Where(x => x.GetAttributeValue<bool>("isdisabled")).ToList();
            else if (comboxBoxValue == "Teams only")
                usersToKeep = bothLists.Where(x => x.LogicalName == "team").ToList();
            else if (comboxBoxValue == "Users only")
                usersToKeep = bothLists.Where(x => x.LogicalName == "systemuser").ToList();

            if (!string.IsNullOrEmpty(filter))
                usersToKeep = usersToKeep.Where(x => (x.LogicalName == "team" && x.GetAttributeValue<string>("name").ToLower().Contains(filter)) ||
                                                     (x.LogicalName == "systemuser" && 
                                                     ((x.GetAttributeValue<string>("domainname") != null && x.GetAttributeValue<string>("domainname").ToLower().Contains(filter)) || 
                                                     (x.GetAttributeValue<string>("lastname") != null && x.GetAttributeValue<string>("lastname").ToLower().Contains(filter)) ||
                                                     (x.GetAttributeValue<string>("firstname") != null && x.GetAttributeValue<string>("firstname").ToLower().Contains(filter))))).ToList();

            ConcurrentBag<ListViewItem> cbItems = new ConcurrentBag<ListViewItem>();

            Parallel.ForEach(usersToKeep, user =>
            {
                var item = user.LogicalName == "systemuser" ? new ListViewItem("user") : new ListViewItem("team");
                if (user.LogicalName == "systemuser")
                {
                    item.SubItems.Add(user.GetAttributeValue<string>("domainname"));
                    item.SubItems.Add(user.GetAttributeValue<string>("firstname"));
                    item.SubItems.Add(user.GetAttributeValue<string>("lastname"));
                    item.SubItems.Add(user.GetAttributeValue<bool>("isdisabled") ? "Disabled" : "Enabled");
                    item.Tag = user.Id;
                }
                else if (user.LogicalName == "team")
                {
                    item.SubItems.Add(user.GetAttributeValue<string>("name"));
                    item.SubItems.Add("");
                    item.SubItems.Add("");
                    item.SubItems.Add("Enabled");
                    item.Tag = user.Id;
                }

                cbItems.Add(item);
            });

            listToAccess.Items.AddRange((ListViewItem[])cbItems.ToArray());

            return usersToKeep;
        }

        private void ManageUserDataToDisplay(ListView listViewOfUserData, List<Entity> listOfUserData, string type, string filter = null)
        {
            listViewOfUserData.Items.Clear();

            if (listOfUserData == null)
                return;

            string entityNameField = null;
            var listToKeep = listOfUserData;

            if (!string.IsNullOrEmpty(filter))
                listToKeep = listOfUserData.Where(x => x.GetAttributeValue<string>("name").ToLower().Contains(filter)).ToList();

            switch (type)
            {
                case UserDataType.Charts:
                    entityNameField = "primaryentitytypecode";
                    break;
                case UserDataType.Dashboards: break;
                default:
                    entityNameField = "returnedtypecode";
                    break;
            }

            var ownItems = new ListViewGroup()
            {
                Header = "Owned records",
                Name = "ownRecordsHeader"
            };
            var sharedItems = new ListViewGroup()
            {
                Header = "Shared records",
                Name = "sharedRecordsHeader"
            };
            listViewOfUserData.Groups.Add(ownItems);
            listViewOfUserData.Groups.Add(sharedItems);

            ConcurrentBag<ListViewItem> cbItems = new ConcurrentBag<ListViewItem>();

            Parallel.ForEach(listToKeep, view =>
            {
                var item = new ListViewItem()
                {
                    Text = view.GetAttributeValue<string>("name"),
                    Group = view.GetAttributeValue<EntityReference>("ownerid").Id ==
                            this.controllerManager.userFrom.userId.Value
                        ? ownItems
                        : sharedItems
                };

                if (entityNameField != null)
                    item.SubItems.Add(view[entityNameField].ToString());
                item.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = view.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("dd-MMM-yyyy HH:mm"),
                    Tag = view.GetAttributeValue<EntityReference>("ownerid")
                });
                item.Tag = view.Id;

                cbItems.Add(item);
            });

            listViewOfUserData.Items.AddRange((ListViewItem[])cbItems.ToArray());

            if (listToKeep.Any())
                listViewOfUserData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public void SaveSettings(bool closeApp = false)
        {
            if(closeApp)
                log.LogData(EventType.Event, LogAction.SettingsSavedWhenClosing);
            else
                log.LogData(EventType.Event, LogAction.SettingsSaved);
            SettingsManager.Instance.Save(typeof(PersonalViewsMigration), settings);

            //reordering columns if necessary
            SortListView(listViewUsers, 0, settings.SortOrderPref);
            SortListView(listViewUserViewsList, 0, settings.SortOrderPref);
            SortListView(listViewUsersDestination, 0, settings.SortOrderPref);
        }

        private void LoadSetting()
        {
            try
            {
                if (SettingsManager.Instance.TryLoad<PluginSettings>(typeof(PersonalViewsMigration), out settings))
                {
                    return;
                }
                else
                    settings = new PluginSettings();
            }
            catch (InvalidOperationException ex)
            {
                log.LogData(EventType.Exception, LogAction.SettingLoaded, ex);
            }

            log.LogData(EventType.Event, LogAction.SettingLoaded);

            if (!settings.AllowLogUsage.HasValue)
            {
                log.PromptToLog();
                SaveSettings();
            }
        }

        public static string CurrentVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fileVersionInfo.ProductVersion;
            }
        }

        private void toolStripButtonOptions_Click(object sender, EventArgs e)
        {
            var allowLogUsage = settings.AllowLogUsage;
            var optionDlg = new Options(this);
            if (optionDlg.ShowDialog(this) == DialogResult.OK)
            {
                settings = optionDlg.GetSettings();
                if (allowLogUsage != settings.AllowLogUsage)
                {
                    if (settings.AllowLogUsage == true)
                    {
                        this.log.UpdateForceLog();
                        this.log.LogData(EventType.Event, LogAction.StatsAccepted);
                    }
                    else if (!settings.AllowLogUsage == true)
                    {
                        this.log.UpdateForceLog();
                        this.log.LogData(EventType.Event, LogAction.StatsDenied);
                    }
                }

                ManageDisplayUsingSettings();
            }
        }

        private void ManageDisplayUsingSettings()
        {
            comboBoxWhatUsersToDisplay.SelectedItem = settings.UsersDisplayAll ? "All" : (settings.UsersDisplayDisabled ? "Disabled" : "Enabled");
            comboBoxWhatUsersToDisplayDestination.SelectedItem = settings.UsersDisplayAll ? "All" : (settings.UsersDisplayDisabled ? "Disabled" : "Enabled");
        }

        #region handling reorder of listview items

        private void SortListView(ListView listView, int columnIndex, SortOrder? sort = null)
        {
            if (sort != null)
            {
                listView.ListViewItemSorter = new ListViewItemComparer(columnIndex, sort.Value);
            }
            else if (columnIndex == currentColumnOrder)
            {
                listView.Sorting = listView.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

                listView.ListViewItemSorter = new ListViewItemComparer(columnIndex, listView.Sorting);
            }
            else
            {
                currentColumnOrder = columnIndex;
                listView.ListViewItemSorter = new ListViewItemComparer(columnIndex, SortOrder.Ascending);
            }
        }

        private void listViewUsers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewUsers, e.Column);
        }

        private void listViewUserViewsList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewUserViewsList, e.Column);
        }

        private void listViewUsersDestination_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewUsersDestination, e.Column);
        }

        private void listViewUserDashboardsList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewUserDashboardsList, e.Column);
        }

        private void listViewUserChartsList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewUserChartsList, e.Column);
        }
        #endregion handling reorder of listview items

        #region handling filters on ListViews
        private void textBoxFilterUsersDestination_TextChanged(object sender, EventArgs e)
        {
            var filter = textBoxFilterUsersDestination.Text;

            if (filter.Length > 1)
                ManageUsersToDisplay("destination", filter.ToLower());
            else if (filter == "")
                ManageUsersToDisplay("destination");
        }

        private void textBoxFilterUsers_TextChanged(object sender, EventArgs e)
        {
            var filter = textBoxFilterUsers.Text;

            if(filter.Length > 1)
                ManageUsersToDisplay("source", filter.ToLower());
            else if (filter == "")
                ManageUsersToDisplay();
        }

        private void textBoxFilterViews_TextChanged(object sender, EventArgs e)
        {
            var filter = textBoxFilterViews.Text;

            if (filter.Length > 1)
                ManageUserDataToDisplay(listViewUserViewsList, listOfUserViews, UserDataType.Views, filter.ToLower());
            else if (filter == "")
                ManageUserDataToDisplay(listViewUserViewsList, listOfUserViews, UserDataType.Views);
        }

        private void textBoxFilterUsers_Click(object sender, EventArgs e)
        {
            if (textBoxFilterUsers.Text == "Search in results ...")
                textBoxFilterUsers.Text = "";
        }

        private void textBoxFilterUsersDestination_Click(object sender, EventArgs e)
        {
            if (textBoxFilterUsersDestination.Text == "Search in results ...")
                textBoxFilterUsersDestination.Text = "";
        }

        private void textBoxFilterViews_Click(object sender, EventArgs e)
        {
            if (textBoxFilterViews.Text == "Search in results ...")
                textBoxFilterViews.Text = "";
        }

        private void textBoxFilterDashboards_TextChanged(object sender, EventArgs e)
        {
            var filter = textBoxFilterDashboards.Text;

            if (filter.Length > 1)
                ManageUserDataToDisplay(listViewUserDashboardsList, listOfUserDashboards, UserDataType.Dashboards, filter.ToLower());
            else if (filter == "")
                ManageUserDataToDisplay(listViewUserDashboardsList, listOfUserDashboards, UserDataType.Dashboards);
        }

        private void textBoxFilterCharts_TextChanged(object sender, EventArgs e)
        {
            var filter = textBoxFilterCharts.Text;

            if (filter.Length > 1)
                ManageUserDataToDisplay(listViewUserChartsList, listOfUserCharts, UserDataType.Charts, filter.ToLower());
            else if (filter == "")
                ManageUserDataToDisplay(listViewUserChartsList, listOfUserCharts, UserDataType.Charts);
        }

        private void textBoxFilterDashboards_Click(object sender, EventArgs e)
        {
            if (textBoxFilterDashboards.Text == "Search in results ...")
                textBoxFilterDashboards.Text = "";
        }

        private void textBoxFilterCharts_Click(object sender, EventArgs e)
        {
            if (textBoxFilterCharts.Text == "Search in results ...")
                textBoxFilterCharts.Text = "";
        }
        #endregion  handling filters on ListViews

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            var helpDlg = new HelpForm(this);
            helpDlg.ShowDialog(this);
        }

        private void listViewUsersDestination_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Checked = e.IsSelected;
        }

        private void listViewUserViewsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Checked = e.IsSelected;
        }

        private void listViewUserDashboardsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Checked = e.IsSelected;
        }

        private void listViewUserChartsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Checked = e.IsSelected;
        }

        private void btnShareWithUser_Click(object sender, EventArgs e)
        {
            try
            {
                ListView currentListView;
                String itemToShareType;
                //select the view and entity type
                switch (tabControlUserData.SelectedTab.Name)
                {
                    case "tabPageCharts":
                        currentListView = listViewUserChartsList;
                        itemToShareType = "userqueryvisualization";
                        break;
                    case "tabPageDashboards":
                        currentListView = listViewUserDashboardsList;
                        itemToShareType = "userform";
                        break;
                    default:
                        currentListView = listViewUserViewsList;
                        itemToShareType = "userquery";
                        break;
                }

                if (currentListView.CheckedItems.Count != 1)
                {
                    MessageBox.Show("Please select only one item to share", "Error", MessageBoxButtons.OK);
                    return;
                }

                //find the selected item, that needs to be shared
                var itemToShare = currentListView.CheckedItems[0];

                //at least one item has to be selected in the destination view
                if (listViewUsersDestination.CheckedItems.Count < 1)
                {
                    MessageBox.Show("Please select at least one user to share with", "Error", MessageBoxButtons.OK);
                    return;
                }

                //exactly one user has to be selected in the "source" view, it will be used as CallerId
                if (listViewUsers.SelectedItems.Count != 1)
                {
                    //TODO needs proper error message
                    return;
                }

                ListViewItem[] destinationUsers = new ListViewItem[listViewUsersDestination.CheckedItems.Count];
                listViewUsersDestination.CheckedItems.CopyTo(destinationUsers, 0);
                
                //iterating through destination list and share the selected item with eash user
                foreach (var selectedDestinationUser in destinationUsers)
                {
                    Guid destinationUserId = (Guid)selectedDestinationUser.Tag;
                    String destinationType = (String)selectedDestinationUser.Text;

                    //Share only with Users and skip Teams
                    if (destinationType != "user")
                        continue;

                    //Populate request with details
                    GrantAccessRequest request = new GrantAccessRequest();
                    request.Target = new EntityReference(itemToShareType, (Guid)itemToShare.Tag);
                    PrincipalAccess access = new PrincipalAccess();
                    access.Principal = new EntityReference("systemuser", destinationUserId);
                    access.AccessMask = AccessRights.ReadAccess;
                    request.PrincipalAccess = access;

                    //set callerid to the selected user, cause only user himself can grant access to own records
                    this.controllerManager.serviceClient.CallerId = (Guid)listViewUsers.SelectedItems[0].Tag;

                    this.controllerManager.serviceClient.Execute(request);

                    //drop CallerId back to null
                    this.controllerManager.serviceClient.CallerId = Guid.Empty;
                }

                MessageBox.Show("Selected item was successfully shared with selected user(s).");
            }
            catch (Exception ex)
            {
                //TODO meaningfull error message? 
                MessageBox.Show(ex.Message);
            }
        }
    }
}
