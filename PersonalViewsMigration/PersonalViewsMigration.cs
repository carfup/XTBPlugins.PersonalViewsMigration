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

namespace Carfup.XTBPlugins.PersonalViewsMigration
{
    public partial class PersonalViewsMigration : PluginControlBase, IGitHubPlugin
    {
        #region varibables
        private List<Entity> listOfUsers = null;
        private List<Entity> listOfUserViews = null;
        public ControllerManager connectionManager = null;

        public string RepositoryName
        {
            get
            {
                return "XTBPlugins.PersonalViewsMigration";
            }
        }

        public string UserName
        {
            get
            {
                return "carfup";
            }
        }

        #endregion
        public PersonalViewsMigration()
        {
            InitializeComponent();
            comboBoxWhatUsersToDisplay.SelectedIndex = 0;
            comboBoxWhatUsersToDisplay.Enabled = false;
            comboBoxWhatUsersToDisplayDestination.SelectedIndex = 0;
            comboBoxWhatUsersToDisplayDestination.Enabled = false;
            buttonLoadUserViews.Enabled = false;
            buttonCopySelectedViews.Enabled = false;
            buttonMigrateSelectedViews.Enabled = false;
            labelDisclaimer.Text = $"To handle disabled user this plugin perform a change regarding the Access Mode of the systemuser and switch it to Non Interactive during the actions and then change it back to Read/Write.{Environment.NewLine}Make sure you have one Non Interactive access mode free to perform any actions (limited to 5 by default)";
        }

        private void toolStripButtonCloseTool_Click(object sender, System.EventArgs e)
        {
            CloseTool();
        }

        private void toolStripButtonLoadUsers_Click(object sender, System.EventArgs evt)
        {
            ExecuteMethod(loadUsersIntoListView);
        }

        public void loadUsersIntoListView()
        {
            connectionManager = new ControllerManager(Service);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading CRM Users...",
                Work = (bw, e) =>
                {
                    listOfUsers = connectionManager.userManager.getListOfUsers();
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (listOfUsers != null)
                    {
                        listViewUsers.Items.Clear();
                        listViewUsersDestination.Items.Clear();

                        foreach (Entity user in listOfUsers)
                        {
                            var item = new ListViewItem(user["domainname"].ToString());
                            item.SubItems.Add(((bool)user["isdisabled"]) ? "Disabled" : "Enabled");
                            item.Tag = user.Id;

                            listViewUsers.Items.Add(item);
                            listViewUsersDestination.Items.Add((ListViewItem)item.Clone());
                        }

                        comboBoxWhatUsersToDisplay.Enabled = true;
                        comboBoxWhatUsersToDisplayDestination.Enabled = true;
                        buttonLoadUserViews.Enabled = true;
                        buttonCopySelectedViews.Enabled = true;
                        buttonMigrateSelectedViews.Enabled = true;
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }
        private void buttonCopySelectedViews_Click(object sender, System.EventArgs evt)
        {
            ListViewItem[] viewsGuid = new ListViewItem[listViewUserViewsList.CheckedItems.Count];
            ListViewItem[] usersGuid = new ListViewItem[listViewUsersDestination.CheckedItems.Count];

            if (usersGuid.Count() == 0)
            {
                MessageBox.Show($"Please select at least one destination user to perform the Copy action.");
                return;
            }
            if (viewsGuid.Count() == 0)
            {
                MessageBox.Show($"Please select at least one view to perform a Copy action.");
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Copying the user view(s) ...",
                Work = (bw, e) =>
                {
                    
                    Invoke(new Action(() =>
                    {
                        listViewUserViewsList.CheckedItems.CopyTo(viewsGuid,0);
                        listViewUsersDestination.CheckedItems.CopyTo(usersGuid, 0);
                    }));

                    if (viewsGuid == null && usersGuid == null)
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

                    foreach (ListViewItem itemView in viewsGuid)
                    {
                        CreateRequest cr = new CreateRequest
                        {
                            Target = this.connectionManager.viewManager.prepareViewToMigrate(listOfUserViews.Find(x => x.Id == (Guid)itemView.Tag))
                        };

                        requestWithResults.Requests.Add(cr);
                    }

                    bw.ReportProgress(0, "Migrating user views...");
                    foreach (ListViewItem itemUser in usersGuid)
                    {
                        bool isUserModified = false;
                        this.connectionManager.UpdateCallerId((Guid)itemUser.Tag);
                        this.connectionManager.userDestination = (Guid)itemUser.Tag;
                        
                        // Check if we need to switch to NonInteractive mode
                        bw.ReportProgress(0, "Checking destination user accessibility...");
                        isUserModified = this.connectionManager.userManager.manageImpersonification();
                        

                        //proxy.CallerId = (Guid)itemUser.Tag;
                        bw.ReportProgress(0, "Copying the view(s)...");
                        ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)this.connectionManager.proxy.Execute(requestWithResults);

                        if (isUserModified)
                        {
                            bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                            this.connectionManager.userManager.manageImpersonification(isUserModified);
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
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("View(s) are Copied !");
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void buttonMigrateSelectedViews_Click(object sender, System.EventArgs evt)
        {
            ListViewItem[] viewsGuid = new ListViewItem[listViewUserViewsList.CheckedItems.Count];
            ListViewItem[] usersGuid = new ListViewItem[listViewUsersDestination.CheckedItems.Count];

            if(usersGuid.Count() > 1)
            {
                MessageBox.Show($"You can't select more than one user for the ReAssign functionality");
                return;
            }
            if (viewsGuid.Count() == 0)
            {
                MessageBox.Show($"Please select at least one view to perform a ReAssign action.");
                return;
            }


            WorkAsync(new WorkAsyncInfo
            {
                Message = "ReAssigning the user view(s) ...",
                Work = (bw, e) =>
                {

                    Invoke(new Action(() =>
                    {
                        listViewUserViewsList.CheckedItems.CopyTo(viewsGuid, 0);
                    }));

                    Invoke(new Action(() =>
                    {
                        listViewUsersDestination.CheckedItems.CopyTo(usersGuid, 0);
                    }));


                    if (viewsGuid == null && usersGuid == null)
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

                    foreach (ListViewItem itemView in viewsGuid)
                    {
                        bw.ReportProgress(0, "Changing ownership of the views...");

                        foreach (ListViewItem itemUser in usersGuid)
                        {

                            bool isUserModified = false;
                            this.connectionManager.UpdateCallerId(this.connectionManager.userFrom.Value);
                            this.connectionManager.userDestination = this.connectionManager.userFrom.Value;

                            // Check if we need to switch to NonInteractive mode
                            bw.ReportProgress(0, "Checking destination user accessibility...");
                            isUserModified = this.connectionManager.userManager.manageImpersonification();


                            //proxy.CallerId = (Guid)itemUser.Tag;
                            bw.ReportProgress(0, "Changing ownership of the view(s)...");
                            AssignRequest ar = new AssignRequest
                            {
                                Assignee = new EntityReference("systemuser", (Guid)itemUser.Tag),
                                Target = new EntityReference("userquery", (Guid)itemView.Tag)
                            };

                            this.connectionManager.proxy.Execute(ar);

                            if (isUserModified)
                            {
                                bw.ReportProgress(0, "Setting back the destination user to Read/Write mode...");
                                this.connectionManager.userManager.manageImpersonification(isUserModified);
                            }
                        } 
                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        foreach(ListViewItem view in viewsGuid.ToList())
                        {
                            listViewUserViewsList.Items.Remove(view);
                        }

                        MessageBox.Show("View(s) are reassigned !");
                    }
                },
                ProgressChanged = e => { SetWorkingMessage(e.UserState.ToString()); }
            });
        }

        private void buttonLoadUserViews_Click(object sender, EventArgs evt)
        {
            ExecuteMethod(loadUserViews);
        }

        private void listViewUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExecuteMethod(loadUserViews);
        }

        public void loadUserViews()
        {
            if(listViewUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show($"Please select at least one user.");
                return;
            }

            this.connectionManager.userFrom = (Guid)listViewUsers.SelectedItems[0].Tag;
            this.connectionManager.userDestination = (Guid)listViewUsers.SelectedItems[0].Tag;
            this.connectionManager.UpdateCallerId(this.connectionManager.userDestination.Value);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving User view(s)...",
                Work = (bw, e) =>
                {
                    bool isUserModified = false;

                    bw.ReportProgress(0, "Checking user accessibility...");
                    isUserModified = this.connectionManager.userManager.manageImpersonification();
                    

                    bw.ReportProgress(0, "Retrieving user's view(s)...");
                    listOfUserViews = this.connectionManager.viewManager.listOfUserViews(this.connectionManager.userDestination.Value);

                    if (isUserModified)
                    {
                        bw.ReportProgress(0, "Setting back the user to Read/Write mode...");
                        this.connectionManager.userManager.manageImpersonification(isUserModified);

                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (listOfUserViews != null)
                    {
                        listViewUserViewsList.Items.Clear();

                        foreach (Entity view in listOfUserViews)
                        {
                            var item = new ListViewItem(view["name"].ToString());
                            item.SubItems.Add(view["returnedtypecode"].ToString());
                            item.Tag = view.Id;

                            listViewUserViewsList.Items.Add(item);
                        }
                    }
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
                    buttonLoadUserViews.Text = $"Load {listViewUsers.SelectedItems[listViewUsers.SelectedItems.Count - 1].Text}'s views";
                    buttonLoadUserViews.Enabled = true;
                }

                else { 
                    buttonLoadUserViews.Text = $"Select an user to load its views.";
                    buttonLoadUserViews.Enabled = false;
                }
            }));
            
        }

        private void buttonLoadUsers_Click(object sender, EventArgs e)
        {
            ExecuteMethod(loadUsersIntoListView);
        }

        private void comboBoxWhatUsersToDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            // avoid exception on first load
            if (listOfUsers == null)
                return;

            listViewUsers.Items.Clear();
            var usersToKeep = listOfUsers;

            if (comboBoxWhatUsersToDisplay.Text == "Enabled")
                usersToKeep = listOfUsers.Where(x => (bool)x.Attributes["isdisabled"] == false).ToList();
            else if (comboBoxWhatUsersToDisplay.Text == "Disabled")
                usersToKeep = listOfUsers.Where(x => (bool)x.Attributes["isdisabled"] == true).ToList();

            foreach (Entity user in usersToKeep)
            {
                var item = new ListViewItem(user["domainname"].ToString());
                item.SubItems.Add(((bool)user["isdisabled"]) ? "Disabled" : "Active");
                item.Tag = user.Id;

                listViewUsers.Items.Add(item);
            }
        }

        private void comboBoxWhatUsersToDisplayDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            // avoid exception on first load
            if (listOfUsers == null)
                return;

            listViewUsersDestination.Items.Clear();
            var usersToKeep = listOfUsers;

            if (comboBoxWhatUsersToDisplayDestination.Text == "Enabled")
                usersToKeep = listOfUsers.Where(x => (bool)x.Attributes["isdisabled"] == false).ToList();
            else if (comboBoxWhatUsersToDisplayDestination.Text == "Disabled")
                usersToKeep = listOfUsers.Where(x => (bool)x.Attributes["isdisabled"] == true).ToList();

            foreach (Entity user in usersToKeep)
            {
                var item = new ListViewItem(user["domainname"].ToString());
                item.SubItems.Add(((bool)user["isdisabled"]) ? "Disabled" : "Active");
                item.Tag = user.Id;

                listViewUsersDestination.Items.Add(item);
            }
        }
    }
}
