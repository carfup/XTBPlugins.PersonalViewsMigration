using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Carfup.XTBPlugins.AppCode
{
    public class DashboardManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>
        private readonly ControllerManager connection = null;

        private static RetrieveEntityResponse metadata = null;
        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class Controller manager
        /// </summary>
        /// <param name="connection">Controller manager</param>
        public DashboardManager(ControllerManager connection)
        {
            this.connection = connection;
        }

        #endregion Constructor

        #region Methods

        private void RetrieveMetadataOfView()
        {
            RetrieveEntityRequest retrieveEntityAttributesRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = "userform"
            };
            metadata = (RetrieveEntityResponse)connection.proxy.Execute(retrieveEntityAttributesRequest);
        }

        public List<Entity> ListOfUserDashboards(Guid userGuid)
        {
            return connection.proxy.RetrieveMultiple(new QueryExpression("userform")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("ownerid", ConditionOperator.Equal, userGuid),
                    }
                }
            }).Entities.ToList();
        }

        public Entity PrepareDashboardToMigrate(Entity getDashboardDetails)
        {
            List<string> attributesList = new List<string> { "formjson", "formxml", "type", "objecttypecode", "name", "description" };

            if (metadata == null)
                RetrieveMetadataOfView();
            
            Entity dashboardToMigrate = new Entity("userform");

            foreach(var att in attributesList)
            {
                if (metadata.EntityMetadata.Attributes.Any(x => x.LogicalName == att))
                {
                    dashboardToMigrate[att] = (getDashboardDetails.Contains(att)) ? getDashboardDetails[att] : null;
                }
            }

            return dashboardToMigrate;
        }
        #endregion Methods
    }
}
