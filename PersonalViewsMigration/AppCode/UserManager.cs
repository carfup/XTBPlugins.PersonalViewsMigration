using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Carfup.XTBPlugins.AppCode
{
    public class UserManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>

        public ControllerManager controller = null;



        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class Controller manager
        /// </summary>
        /// <param name="connection">Controller manager</param>
        public UserManager(ControllerManager controller)
        {
            this.controller = controller;
        }

        #endregion Constructor

        #region Methods

        
        public Boolean ManageImpersonification(bool action = false, UserInfo userInfo = null)
        {
            if (controller.userDestination.userEntity == "team" || userInfo?.userEntity == "team")
                return false;

            if (userInfo == null)
                userInfo = controller.userDestination;

            bool isUserModified = action;

            if (isUserModified)
            {
                CheckIfUserEnabled(userInfo.userId.Value, 0);
            }
            else
            {
                isUserModified = CheckIfUserEnabled(userInfo.userId.Value);
            }

            controller.UpdateCallerId(userInfo.userId.Value);

            return isUserModified;
        }

        private Boolean CheckIfUserEnabled(Guid userGuid, int accessmode = 4)
        {
            bool ismodified = false;

            // We put back the admin user to be sure that he has permission to perform the following actions
            controller.UpdateCallerId(controller.XTBUser.Value);

            // By default i set it to the user
            Entity user = this.controller.serviceClient.Retrieve("systemuser", userGuid, new ColumnSet("isdisabled", "accessmode", "fullname"));

            //if user is null or is onprem, no need to manage the non interactive mode                    
            if (user == null || controller.isOnPrem)
                return ismodified;

            // we check if the user exist in the crm  
            Trace.TraceInformation($"checking User : {user.GetAttributeValue<string>("fullname")}, isdisabled : {user.GetAttributeValue<bool>("isdisabled")}, accessmode : {user.GetAttributeValue<OptionSetValue>("accessmode")?.Value}");
            // If the user is disabled or is in Non Interactive mode, we update it.
            if (user.GetAttributeValue<bool>("isdisabled") || user.GetAttributeValue<OptionSetValue>("accessmode").Value == 4)
            {
                user["accessmode"] = new OptionSetValue(accessmode);

                controller.serviceClient.Update(user);
                Trace.TraceInformation($"updated User : {user.GetAttributeValue<string>("fullname")} to accessmode : {accessmode}");
                ismodified = true;
            }

            return ismodified;
        }
        
        public List<Entity> GetListOfUsers()
        {
            List<Entity> userList = new List<Entity>();
            int queryCount = 5000;
            int pageNumber = 1;


            var userQuery = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("domainname", "firstname", "lastname", "systemuserid", "isdisabled"),

                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("accessmode", ConditionOperator.NotEqual, 3),
                        new ConditionExpression("domainname", ConditionOperator.NotNull),
                        new ConditionExpression("domainname", ConditionOperator.NotEqual, ""),
                        new ConditionExpression("domainname", ConditionOperator.NotIn, new string[] {"bap_sa@microsoft.com", "crmoln2@microsoft.com"}),
                    },
                    FilterOperator = LogicalOperator.And
                }
            };

            userQuery.PageInfo = new PagingInfo();
            userQuery.PageInfo.Count = queryCount;
            userQuery.PageInfo.PageNumber = pageNumber;
            userQuery.PageInfo.PagingCookie = null;


            while (true)
            {
                EntityCollection results = controller.serviceClient.RetrieveMultiple(userQuery);
                if (results.Entities != null)
                {
                    foreach (Entity user in results.Entities)
                    {
                        userList.Add(user);
                    }
                }

                // Check for more records, if it returns true.
                if (results.MoreRecords)
                {
                    userQuery.PageInfo.PageNumber++;
                    userQuery.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                {
                    // If no more records are in the result nodes, exit the loop.
                    break;
                }
            }

            return userList;
        }

        public List<Entity> GetListOfTeams()
        {
            RetrieveEntityRequest retrieveEntityAttributesRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = "team"
            };
            var metadata = (RetrieveEntityResponse)controller.serviceClient.Execute(retrieveEntityAttributesRequest);

            var condition = new ConditionExpression("teamtype", ConditionOperator.Equal, 0);
            if (!metadata.EntityMetadata.Attributes.Any(x => x.LogicalName == "teamtype"))
                condition = null;


            List<Entity> teamList = new List<Entity>();

            int queryCount = 5000;
            int pageNumber = 1;
            int recordCount = 0;

            var teamQuery = new QueryExpression("team")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria =
                {
                    Conditions =
                    {
                        condition
                    }
                }
            };

            teamQuery.PageInfo = new PagingInfo();
            teamQuery.PageInfo.Count = queryCount;
            teamQuery.PageInfo.PageNumber = pageNumber;
            teamQuery.PageInfo.PagingCookie = null;

            while (true)
            {
                EntityCollection results = controller.serviceClient.RetrieveMultiple(teamQuery);
                if (results.Entities != null)
                {
                    foreach (Entity team in results.Entities)
                    {
                        teamList.Add(team);
                    }
                }

                // Check for more records, if it returns true.
                if (results.MoreRecords)
                {
                    teamQuery.PageInfo.PageNumber++;
                    teamQuery.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                {
                    // If no more records are in the result nodes, exit the loop.
                    break;
                }
            }

            return teamList;
        }

        public bool UserHasAnyRole(UserInfo userInfo)
        {
            if (userInfo.userEntity == "team")
                return true;

            var retrieveRoles = controller.serviceClient.RetrieveMultiple(new QueryExpression("systemuserroles")
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("systemuserid", ConditionOperator.Equal, userInfo.userId.Value)
                    }
                }
            }).Entities.ToList();

            return retrieveRoles.Any();
        }

        public bool CheckIfNonInteractiveSeatAvailable()
        {
            var nonInteractiveCount = controller.serviceClient.RetrieveMultiple(new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("accessmode", ConditionOperator.Equal, 4),
                        new ConditionExpression("firstname", ConditionOperator.NotEqual, "#")
                    }
                }
            }).Entities.Count;

            return nonInteractiveCount < 7;
        }

        public UserInfo SetUserType(Guid userId, string userType)
        {
            return new UserInfo()
            {
                userId = userId,
                userEntity = (userType == "user" || userType == "systemuser") ? "systemuser": "team"
            };
        }
        #endregion Methods
    }
}
