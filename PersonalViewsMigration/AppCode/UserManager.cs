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
        /// Initializes a new instance of the class Controller manager
        /// </summary>
        /// <param name="connection">Controller manager</param>
        public UserManager(ControllerManager connection)
        {
            this.connection = connection;
        }

        #endregion Constructor

        #region Methods

        
        public Boolean ManageImpersonification(bool action = false)
        {
            bool isUserModified = action;

            if (isUserModified)
            {
                CheckIfUserEnabled(connection.userDestination.Value, 0);
            }
            else
            {
                isUserModified = CheckIfUserEnabled(connection.userDestination.Value);
            }

            connection.UpdateCallerId(connection.userDestination.Value);

            return isUserModified;
        }

        private Boolean CheckIfUserEnabled(Guid userGuid, int accessmode = 4)
        {
            bool ismodified = false;

            // We put back the admin user to be sure that he has permission to perform the following actions
            this.connection.UpdateCallerId(this.connection.XTBUser.Value);

            // By default i set it to the user
            Entity user = this.connection.service.Retrieve("systemuser", userGuid, new ColumnSet("isdisabled", "accessmode", "fullname"));

            //if user is null or is onprem, no need to manage the non interactive mode                    
            if (user == null || connection.isOnPrem)
                return ismodified;

            // we check if the user exist in the crm  
            Trace.TraceInformation($"checking User : {user["fullname"]}, isdisabled : {user["isdisabled"]}, accessmode : {((OptionSetValue) user["accessmode"]).Value}");
            // If the user is disabled or is in Non Interactive mode, we update it.
            if (Boolean.Parse(user["isdisabled"].ToString()) || ((OptionSetValue)user["accessmode"]).Value == 4)
            {
                user["accessmode"] = new OptionSetValue(accessmode);

                this.connection.service.Update(user);
                Trace.TraceInformation($"updated User : {user["fullname"]} to accessmode : {accessmode}");
                ismodified = true;
            }

            return ismodified;
        }
        
        public List<Entity> GetListOfUsers()
        {
            return connection.service.RetrieveMultiple(new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("domainname", "firstname", "lastname", "systemuserid", "isdisabled"),
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
