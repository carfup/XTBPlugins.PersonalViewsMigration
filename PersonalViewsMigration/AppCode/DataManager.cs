using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Carfup.XTBPlugins.AppCode
{
    public class DataManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>
        private readonly ControllerManager connection = null;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class DataManager
        /// </summary>
        /// <param name="service">Details of the connected user</param>
        public DataManager(ControllerManager connection)
        {
            this.connection = connection;
        }

        public List<Entity> retriveRecordSharings(Guid guid, string entityType)
        {
            return this.connection.service.RetrieveMultiple(new QueryExpression()
            {
                EntityName = "principalobjectaccess",
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("objectid", ConditionOperator.Equal, guid),
                        new ConditionExpression("objecttypecode", ConditionOperator.Equal, entityType),
                        new ConditionExpression("accessrightsmask", ConditionOperator.GreaterThan, 0)
                    }
                },
                LinkEntities =
                {
                    new LinkEntity() {
                        LinkFromEntityName = "principalobjectaccess",
                        LinkFromAttributeName = "principalid",
                        LinkToEntityName = "systemuser",
                        LinkToAttributeName = "systemuserid",
                        EntityAlias = "systemuser",
                        JoinOperator = JoinOperator.LeftOuter,
                        Columns = new ColumnSet("domainname"),
                    },
                    new LinkEntity() {
                        LinkFromEntityName = "principalobjectaccess",
                        LinkFromAttributeName = "principalid",
                        LinkToEntityName = "team",
                        LinkToAttributeName = "teamid",
                        EntityAlias = "team",
                        JoinOperator = JoinOperator.LeftOuter,
                        Columns = new ColumnSet("name"),
                    }
                }
            }).Entities.ToList();
        }

        public Guid[] retrieveSharingsOfUser(Guid userGuid, string entityType)
        {
            return this.connection.service.RetrieveMultiple(new QueryExpression()
            {
                EntityName = "principalobjectaccess",
                ColumnSet = new ColumnSet("objectid"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("principalid", ConditionOperator.Equal, userGuid),
                        new ConditionExpression("principaltypecode", ConditionOperator.Equal, "systemuser"),
                        new ConditionExpression("objecttypecode", ConditionOperator.Equal, entityType),

                    }
                }
            }).Entities.Select(x => x.GetAttributeValue<Guid>("objectid")).ToArray();
        }

        #endregion Constructor
    }
}
