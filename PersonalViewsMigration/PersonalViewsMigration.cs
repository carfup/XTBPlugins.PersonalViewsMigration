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
using Microsoft.Xrm.Sdk.Messages;
using Carfup.XTBPlugins.AppCode;
using Microsoft.Crm.Sdk.Messages;
using System.Diagnostics;
using Carfup.XTBPlugins.Forms;
using McTools.Xrm.Connection;

namespace Carfup.XTBPlugins.PersonalViewsMigration
{
    public partial class PersonalViewsMigration : PluginControlBase, IGitHubPlugin
    {
        #region varibables
        private List<Entity> listOfUsers = null;
        private List<Entity> listOfUserViews = null;
        private List<Entity> listOfUserCharts = null;
        private List<Entity> listOfUserDashboards = null;
        public ControllerManager connectionManager = null;
        internal PluginSettings settings = new PluginSettings();
        public LogUsageManager log = null;
        private int currentColumnOrder;

        public string RepositoryName => "XTBPlugins.PersonalViewsMigration";
        public string UserName => "carfup";

        #endregion
        public PersonalViewsMigration()
        {
            InitializeComponent();
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            connectionManager = new ControllerManager(newService);
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
            if (!connectionManager.isOnPrem && !connectionManager.userManager.CheckIfNonInteractiveSeatAvailable())
            {
                MessageBox.Show("It seems that all Non Interactive seats are used.\n\nPlease free at least one to enjoy the functionalities of this plugin !", "Warning, Non Interactive seat needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading CRM Users...",
                Work = (bw, e) =>
                {
                    listOfUsers = connectionManager.userManager.GetListOfUsers();
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
            textBoxFilterUsersDestination.Enabled = enable;
            textBoxFilterUsers.Enabled = enable;
            textBoxFilterCharts.Enabled = enable;
            textBoxFilterDashboards.Enabled = enable;
            textBoxFilterViews.Enabled = enable;

            // if onprem , we force the list to enabled only
            if (connectionManager.isOnPrem)
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

            switch (tabControlUserData.SelectedTab.Name)
            {
                case "tabPageCharts":
                    type = UserDataType.Charts;
                    listViewUserData = listViewUserChartsList;
                    action = LogAction.ChartsCopied;
                    listOfUserData = listOfUserCharts;
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.DashboardsCopied;
                    listOfUserData = listOfUserDashboards;
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.ViewsCopied;
                    listOfUserData = listOfUserViews;
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

                    foreach (ListViewItem itemView in dataGuid)
                    {
                        Entity itemEntity = listOfUserData.Find(x => x.Id == (Guid) itemView.Tag);

                        CreateRequest cr = new CreateRequest();

                        switch (type)
                        {
                            case UserDataType.Charts:
                                cr.Target = this.connectionManager.chartManager.PrepareChartToMigrate(itemEntity);
                                break;
                            case UserDataType.Dashboards:
                                cr.Target = this.connectionManager.dashboardManager.PrepareDashboardToMigrate(itemEntity);
                                break;
                            default:
                                cr.Target = this.connectionManager.viewManager.PrepareViewToMigrate(itemEntity);
                                break;
                        }

                        requestWithResults.Requests.Add(cr);
                    }

                    bw.ReportProgress(0, $"Migrating user {type}s...");
                    foreach (ListViewItem itemUser in usersGuid)
                    {
                        var userId = (Guid) itemUser.Tag;

                        connectionManager.UpdateCallerId(userId);
                        connectionManager.userDestination = userId;

                        // Check if we need to switch to NonInteractive mode
                        bw.ReportProgress(0, "Checking destination user accessibility...");
                        var isUserModified = connectionManager.userManager.ManageImpersonification();

                        // check if user has any roles assigned
                        if (!connectionManager.userManager.UserHasAnyRole(userId))
                        {
                            if (usersGuid.Length == 1)
                            {
                                MessageBox.Show("The selected user has no security roles assigned.\nMake sure you assign at least one security role in order to perform any action for this user", "Warning, Security role needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                success = false;
                            }

                            continue;
                        }

                        bw.ReportProgress(0, $"Copying the {type}(s)...");
                        ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)connectionManager.service.Execute(requestWithResults);

                        if (isUserModified)
                        {
                            bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                            connectionManager.userManager.ManageImpersonification(isUserModified);
                        }

                        foreach (var responseItem in responseWithResults.Responses)
                        {
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
                        MessageBox.Show($"{type}(s) are Copied !");

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
                    action = LogAction.ChartsCopied;
                    entityDataToDelete = "userqueryvisualization";
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.DashboardsCopied;
                    entityDataToDelete = "userform";
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.ViewsCopied;
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

                        connectionManager.UpdateCallerId(connectionManager.userFrom.Value);
                        connectionManager.userDestination = connectionManager.userFrom.Value;

                        // Check if we need to switch to NonInteractive mode
                        bw.ReportProgress(0, "Checking user accessibility...");
                        var isUserModified = connectionManager.userManager.ManageImpersonification();

                        DeleteRequest dr = new DeleteRequest()
                        {
                            Target = new EntityReference(entityDataToDelete, (Guid)itemView.Tag)
                        };

                        connectionManager.proxy.Execute(dr);

                        if (isUserModified)
                        {
                            bw.ReportProgress(0, "Setting back the user to Read/Write mode...");
                            connectionManager.userManager.ManageImpersonification(isUserModified);
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
                            listViewUserViewsList.Items.Remove(view);

                        log.LogData(EventType.Event, action);
                        MessageBox.Show($"{type}(s) are now deleted !");
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
                    action = LogAction.ChartsCopied;
                    entityDataToMigrate = "userqueryvisualization";
                    break;
                case "tabPageDashboards":
                    type = UserDataType.Dashboards;
                    listViewUserData = listViewUserDashboardsList;
                    action = LogAction.DashboardsCopied;
                    entityDataToMigrate = "userform";
                    break;
                default:
                    type = UserDataType.Views;
                    listViewUserData = listViewUserViewsList;
                    action = LogAction.ViewsCopied;
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
                            connectionManager.UpdateCallerId(connectionManager.userFrom.Value);
                            connectionManager.userDestination = connectionManager.userFrom.Value;

                            // Check if we need to switch to NonInteractive mode
                            bw.ReportProgress(0, "Checking destination user accessibility...");
                            bool isUserModified = connectionManager.userManager.ManageImpersonification();


                            //proxy.CallerId = (Guid)itemUser.Tag;
                            bw.ReportProgress(0, $"Changing ownership of the {type}(s)...");
                            AssignRequest ar = new AssignRequest
                            {
                                Assignee = new EntityReference("systemuser", (Guid)itemUser.Tag),
                                Target = new EntityReference(entityDataToMigrate, (Guid)itemView.Tag)
                            };

                            connectionManager.proxy.Execute(ar);

                            if (isUserModified)
                            {
                                bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                                connectionManager.userManager.ManageImpersonification(isUserModified);
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
                        {
                            listViewUserData.Items.Remove(view);
                        }

                        log.LogData(EventType.Event, action);
                        MessageBox.Show($"{type}(s) are reassigned !");
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

            this.connectionManager.userFrom = (Guid)listViewUsers.SelectedItems[0].Tag;
            this.connectionManager.userDestination = (Guid)listViewUsers.SelectedItems[0].Tag;
            Guid userDestination = this.connectionManager.userDestination.Value;
            this.connectionManager.UpdateCallerId(userDestination);

            List<Entity> listOfUserData = null;
            string actionToDo = null;
            ListView listViewOfData = null;
            string fieldToEntity = null;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Retrieving User {type}(s)...",
                Work = (bw, e) =>
                {
                    bw.ReportProgress(0, "Checking user accessibility...");
                    var isUserModified = connectionManager.userManager.ManageImpersonification();

                    if (!connectionManager.userManager.UserHasAnyRole(userDestination))
                        return;


                    bw.ReportProgress(0, $"Retrieving user's {type}(s)...");

                    switch (type)
                    {
                        case UserDataType.Charts:
                            listOfUserData = connectionManager.chartManager.ListOfUserCharts(userDestination);
                            listOfUserCharts = listOfUserData;
                            actionToDo = LogAction.UserChartsLoaded;
                            listViewOfData = listViewUserChartsList;
                            break;
                        case UserDataType.Dashboards:
                            listOfUserData = connectionManager.dashboardManager.ListOfUserDashboards(userDestination);
                            listOfUserDashboards = listOfUserData;
                            actionToDo = LogAction.UserDashboardsLoaded;
                            listViewOfData = listViewUserDashboardsList;
                            break;
                        default:
                            listOfUserData = connectionManager.viewManager.ListOfUserViews(userDestination);
                            listOfUserViews = listOfUserData;
                            actionToDo = LogAction.UserViewsLoaded;
                            listViewOfData = listViewUserViewsList;
                            break;
                    }

                    if (isUserModified)
                    {
                        bw.ReportProgress(0, "Setting back the user to Read/Write mode...");
                        connectionManager.userManager.ManageImpersonification(isUserModified);

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

                    // if user has no role, message and skip next check
                    if (!connectionManager.userManager.UserHasAnyRole(userDestination))
                    {
                        MessageBox.Show("The selected user has no security roles assigned.\nMake sure you assign at least one security role in order to perform any action for this user", "Warning, Security role needed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (listOfUserData != null && !listOfUserData.Any())
                        MessageBox.Show($"This user has no personal {type}(s) associated to his account.", $"No {type}(s) available.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
            // cleaning the list first
            if(type == "source")
                listViewUsers.Items.Clear();
            else 
                listViewUsersDestination.Items.Clear();

            // avoid exception on first load
            if (listOfUsers == null)
                return null;

            var usersToKeep = listOfUsers;

            string comboxBoxValue = comboBoxWhatUsersToDisplay.Text;
            if (type == "destination")
            {
                comboxBoxValue = comboBoxWhatUsersToDisplayDestination.Text;
            }

            if(comboxBoxValue == "Enabled")
                usersToKeep = listOfUsers.Where(x => (bool)x.Attributes["isdisabled"] == false).ToList();
            else if (comboxBoxValue == "Disabled")
                usersToKeep = listOfUsers.Where(x => (bool)x.Attributes["isdisabled"] == true).ToList();

            if(!string.IsNullOrEmpty(filter))
                usersToKeep = usersToKeep.Where(x => x.Attributes["domainname"].ToString().ToLower().Contains(filter) || x.Attributes["lastname"].ToString().ToLower().Contains(filter) || x.Attributes["firstname"].ToString().ToLower().Contains(filter)).ToList();

            foreach (Entity user in usersToKeep)
            {
                var item = new ListViewItem(user["domainname"].ToString());
                item.SubItems.Add(user["firstname"].ToString());
                item.SubItems.Add(user["lastname"].ToString());
                item.SubItems.Add((bool)user["isdisabled"] ? "Disabled" : "Enabled");
                item.Tag = user.Id;

                if(type == "source")
                    listViewUsers.Items.Add(item);
                else
                    listViewUsersDestination.Items.Add(item);
            }

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
                listToKeep = listOfUserData.Where(x => x.Attributes["name"].ToString().ToLower().Contains(filter)).ToList();

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


            foreach (Entity view in listToKeep)
            {
                var item = new ListViewItem(view["name"].ToString());

                if(entityNameField != null)
                    item.SubItems.Add(view[entityNameField].ToString());
                item.SubItems.Add(((DateTime)view["createdon"]).ToLocalTime().ToString("dd-MMM-yyyy HH:mm"));
                item.Tag = view.Id;

                listViewOfUserData.Items.Add(item);
            }

            if (listToKeep.Any())
                listViewOfUserData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void PersonalViewsMigration_Load(object sender, EventArgs e)
        {
            comboBoxWhatUsersToDisplay.SelectedIndex = 0;
            comboBoxWhatUsersToDisplayDestination.SelectedIndex = 0;

            ManageDisplayOfFormComponents(false);

            log = new LogUsageManager(this);
            log.LogData(EventType.Event, LogAction.SettingLoaded);
            LoadSetting();
            ManageDisplayUsingSettings();

            if (Service != null)
            {
                // creating the controller
                connectionManager = new ControllerManager(Service);

                IsOnlineOrg(ConnectionDetail);
            }

            if(connectionManager.isOnPrem)
                log.LogData(EventType.Event, LogAction.EnvironmentOnPremise);
        }

        private void IsOnlineOrg(ConnectionDetail cd)
        {
            if (cd == null || cd.UseOnline)
                return;

            // changing the disclaimer message if OnPrem instance
            labelDisclaimer.Text =
                "Make sure you have the necessary permissions to perform actions within the plugin.\nThe needed privilege is : \"prvActOnBehalfOfAnotherUser\" included in the Delegate security role.";

            connectionManager.isOnPrem = true;

            // if onprem , we force the list to enabled only
            comboBoxWhatUsersToDisplayDestination.SelectedItem = "Enabled";
            comboBoxWhatUsersToDisplayDestination.Enabled = false;
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

        
    }
}
