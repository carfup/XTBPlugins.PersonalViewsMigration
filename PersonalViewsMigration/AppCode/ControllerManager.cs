using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace Carfup.XTBPlugins.AppCode
{
    public class ControllerManager
    {
        public IOrganizationService service { get; private set; } = null;
        public CrmServiceClient serviceClient { get; private set; } = null;
        public OrganizationServiceProxy proxy { get; private set; } = null;
        public ViewManager viewManager { get; private set; } = null;
        public UserManager userManager { get; private set; } = null;
        public ChartManager chartManager { get; private set; } = null;
        public DashboardManager dashboardManager { get; private set; } = null;
        public DataManager dataManager { get; private set; } = null;
        public Guid? XTBUser { get; private set; } = null;
        public UserInfo userFrom { get; set; } = null;
        public UserInfo userDestination { get; set; } = null;
        public bool isOnPrem { get; set; } = false;
        public ControllerManager(CrmServiceClient service)
        {
            this.serviceClient = service;
            this.service = (IOrganizationService)service.OrganizationServiceProxy;
            this.proxy = service.OrganizationServiceProxy;
            this.userManager = new UserManager(this);
            this.viewManager = new ViewManager(this);
            this.chartManager = new ChartManager(this);
            this.dashboardManager = new DashboardManager(this);
            this.dataManager = new DataManager(this);
            this.XTBUser = ((WhoAmIResponse)this.proxy.Execute(new WhoAmIRequest())).UserId;
        }

        public void UpdateCallerId(Guid guid)
        {
            // We force the change of the caller id 
            this.proxy.Authenticate();
            this.proxy.CallerId = guid;
            this.service = (IOrganizationService)this.proxy;
        }
    }
}
