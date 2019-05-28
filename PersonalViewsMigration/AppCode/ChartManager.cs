using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Carfup.XTBPlugins.AppCode
{
    public class ChartManager
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
        public ChartManager(ControllerManager controller)
        {
            this.controller = controller;
        }

        #endregion Constructor

        #region Methods

        private void RetrieveMetadataOfChart()
        {
            RetrieveEntityRequest retrieveEntityAttributesRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = "userqueryvisualization"
            };
            metadata = (RetrieveEntityResponse)controller.proxy.Execute(retrieveEntityAttributesRequest);
        }

        public List<Entity> ListOfUserCharts(UserInfo userInfo)
        {
            var sharings = controller.dataManager.retrieveSharingsOfUser(userInfo, "userqueryvisualization");

            var filter = new FilterExpression(LogicalOperator.Or)
            {
                Conditions =
                {
                    new ConditionExpression("userqueryvisualizationid", ConditionOperator.In, sharings)
                }
            };
            if (sharings.Length == 0)
                filter = null;

            return controller.proxy.RetrieveMultiple(new QueryExpression()
            {
                EntityName = "userqueryvisualization",
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

        public Entity PrepareChartToMigrate(Entity getChartwDetails)
        {
            List<string> attributesList = new List<string> { "primaryentitytypecode", "charttype", "presentationdescription", "isdefault", "datadescription", "name", "description" };

            if (metadata == null)
                RetrieveMetadataOfChart();
            
            Entity chartToMigrate = new Entity("userqueryvisualization");

            foreach(var att in attributesList)
            {
                if (metadata.EntityMetadata.Attributes.Any(x => x.LogicalName == att))
                {
                    chartToMigrate[att] = (getChartwDetails.Contains(att)) ? getChartwDetails[att] : null;
                }
            }

            return chartToMigrate;
        }
        #endregion Methods
    }
}
