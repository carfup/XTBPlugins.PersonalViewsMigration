using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Carfup.XTBPlugins.AppCode
{
    class DataManager
    {
        #region Variables

        /// <summary>
        /// Crm web service
        /// </summary>
        private IOrganizationService service;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class DataManager
        /// </summary>
        /// <param name="service">Details of the connected user</param>
        public DataManager(IOrganizationService service)
        {
            this.service = service;
        }

        #endregion Constructor
    }
}
