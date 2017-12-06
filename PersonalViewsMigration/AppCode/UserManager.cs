using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Carfup.XTBPlugins.AppCode
{
    public class UserManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>

        public ControllerManager connection = null;



        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class UserManager
        /// </summary>
        /// <param name="proxy">Details of the connected user</param>
        public UserManager(ControllerManager connection)
        {
            this.connection = connection;
        }

        #endregion Constructor

        #region Methods

        
        public Boolean manageImpersonification(bool action = false)
        {
            bool isUserModified = action;

            if (isUserModified)
            {
                this.CheckIfUserEnabled(this.connection.userDestination.Value, 0);
                //this.connection.userDestination = this.connection.userFrom;
            }
            else
            {
                isUserModified = this.CheckIfUserEnabled(this.connection.userDestination.Value);
            }

            this.connection.UpdateCallerId(this.connection.userDestination.Value);

            return isUserModified;
        }

        public Boolean CheckIfUserEnabled(Guid userGuid, int accessmode = 4)
        {
            bool ismodified = false;

            // We put back the admin user to be sure that he has permission to perform the following actions
            this.connection.UpdateCallerId(this.connection.XTBUser.Value);

            // By default i set it to the user
            Entity user = this.connection.service.Retrieve("systemuser", userGuid, new Microsoft.Xrm.Sdk.Query.ColumnSet("isdisabled", "accessmode", "fullname"));

            // we check if the user exist in the crm                       
            if (user != null)
            {
                Trace.TraceInformation(String.Format("checking User : {0}, isdisabled : {1}, accessmode : {2}", user["fullname"], user["isdisabled"], ((OptionSetValue)user["accessmode"]).Value));
                // If the user is disabled or is in Non Interactive mode, we update it.
                if (Boolean.Parse(user["isdisabled"].ToString()) || ((OptionSetValue)user["accessmode"]).Value == 4)
                {
                    user["accessmode"] = new OptionSetValue(accessmode);

                    this.connection.service.Update(user);
                    Trace.TraceInformation(String.Format("updated User : {0} to accessmode : {1}", user["fullname"], accessmode));
                    ismodified = true;
                }
            }

            return ismodified;
        }
        
        public List<Entity> getListOfUsers()
        {
            return this.connection.service.RetrieveMultiple(new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("domainname", "systemuserid", "isdisabled"),
                Criteria = new FilterExpression
                {
                    Conditions =
                            {
                                new ConditionExpression("accessmode", ConditionOperator.NotEqual, 3),
                                new ConditionExpression("domainname", ConditionOperator.NotNull),
                                new ConditionExpression("domainname", ConditionOperator.NotEqual, ""),
                            }
                }
            }).Entities.ToList();
        }

        #endregion Methods
    }
}
