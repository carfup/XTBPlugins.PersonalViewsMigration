using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System.Text.RegularExpressions;

namespace Carfup.XTBPlugins.AppCode
{
    public class DashboardManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>
        private readonly ControllerManager controller = null;

        private static RetrieveEntityResponse metadata = null;
        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class Controller manager
        /// </summary>
        /// <param name="connection">Controller manager</param>
        public DashboardManager(ControllerManager controller)
        {
            this.controller = controller;
        }

        #endregion Constructor

        #region Methods

        private void RetrieveMetadataOfDashboard()
        {
            RetrieveEntityRequest retrieveEntityAttributesRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = "userform"
            };
            metadata = (RetrieveEntityResponse)controller.serviceClient.Execute(retrieveEntityAttributesRequest);
        }

        public List<Entity> ListOfUserDashboards(UserInfo userInfo)
        {
            var sharings = controller.dataManager.retrieveSharingsOfUser(userInfo, "userform");

            var filter = new FilterExpression(LogicalOperator.Or)
            {
                Conditions =
                {
                    new ConditionExpression("userformid", ConditionOperator.In, sharings)
                }
            };
            if (sharings.Length == 0)
                filter = null;

            return controller.serviceClient.RetrieveMultiple(new QueryExpression("userform")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    FilterOperator = LogicalOperator.Or,
                    Filters =
                    {
                        new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression("ownerid", ConditionOperator.Equal, userInfo.userId),
                            }
                        },
                        filter
                    }
                }
            }).Entities.ToList();
        }

        public Entity PrepareDashboardToMigrate(Entity getDashboardDetails)
        {
            List<string> attributesList = new List<string> { "formjson", "formxml", "type", "objecttypecode", "name", "description" };

            if (metadata == null)
                RetrieveMetadataOfDashboard();

            Entity dashboardToMigrate = new Entity("userform");

            foreach (var att in attributesList)
            {
                if (metadata.EntityMetadata.Attributes.Any(x => x.LogicalName == att))
                {
                    dashboardToMigrate[att] = (getDashboardDetails.Contains(att)) ? getDashboardDetails[att] : null;
                }
            }

            return dashboardToMigrate;
        }

        public Entity ConvertDashboardToSystem(Entity getDashboardDetails)
        {
            List<string> attributesList = new List<string> { "formjson", "formxml", "type", "objecttypecode", "name", "description" };

            Entity dashboardToMigrate = new Entity("systemform");

            foreach (var mapping in attributesList)
            {
                if (getDashboardDetails.Contains(mapping))
                    dashboardToMigrate[mapping] = getDashboardDetails[mapping];
            }

            return GenerateUniqueIDsForSystem(dashboardToMigrate);
        }

        public Entity GenerateUniqueIDsForSystem(Entity dashboard)
        {
            string outxml = (string)dashboard["formxml"];
            string outjson = (string)dashboard["formjson"];
            string idPattern = @"\{([a-fA-F0-9]{8}-([a-fA-F0-9]{4}-){3}[a-fA-F0-9]{12})\}";
            string attrPattern = @"( id=""| uniqueid="").*?""";
            string tagPattern = @"[\<](tab |section |cell |control ).*?[\>]";
            foreach (Match match in Regex.Matches((string)dashboard["formxml"], tagPattern))
            {
                string swap = Guid.NewGuid().ToString("B");
                foreach (Match attr in Regex.Matches(match.Value, attrPattern))
                {
                    Match orig = Regex.Match(attr.Value, idPattern);
                    if (orig.Value != String.Empty)
                    {
                        outxml = outxml.Replace(orig.Value, swap);
                        outjson = outjson.Replace(orig.Value, swap);
                    }
                }
            }
            dashboard["formxml"] = outxml;
            dashboard["formjson"] = outjson;
            return dashboard;
        }
        #endregion Methods
    }
}
