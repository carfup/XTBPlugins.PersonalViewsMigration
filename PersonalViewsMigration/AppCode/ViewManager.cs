using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Carfup.XTBPlugins.AppCode
{
    public class ViewManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>
        public ControllerManager connection = null;
        public static RetrieveEntityResponse metadata = null;
        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class UserManager
        /// </summary>
        /// <param name="proxy">Details of the connected user</param>
        public ViewManager(ControllerManager connection)
        {
            this.connection = connection;
        }

        #endregion Constructor

        #region Methods
        public void retrieveMetadataOfView()
        {
            RetrieveEntityRequest retrieveBankAccountEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = "userquery"
            };
            metadata = (RetrieveEntityResponse)this.connection.proxy.Execute(retrieveBankAccountEntityRequest);
        }

        public List<Entity> listOfUserViews(Guid userGuid)
        {
            return this.connection.proxy.RetrieveMultiple(new QueryExpression("userquery")
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

        public Entity prepareViewToMigrate(Entity getViewDetails)
        {
            List<string> attributesList = new List<string> { "fetchxml", "returnedtypecode", "layoutxml", "querytype", "name", "advancedgroupby", "columnsetxml", "description", "offlinesqlquery" };

            if (metadata == null)
                retrieveMetadataOfView();
            
            Entity viewToMigrate = new Entity("userquery");

            foreach(var att in attributesList)
            {
                if (metadata.EntityMetadata.Attributes.Any(x => x.LogicalName == att))
                {
                    viewToMigrate[att] = (getViewDetails.Contains(att)) ? getViewDetails[att] : null;
                }
            }

            return viewToMigrate;
        }
        #endregion Methods
    }
}
