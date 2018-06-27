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

namespace Carfup.XTBPlugins.AppCode
{
    public class ControllerManager
    {
        public IOrganizationService service { get; private set; } = null;
        public OrganizationServiceProxy proxy { get; private set; } = null;
        public ViewManager viewManager { get; private set; } = null;
        public UserManager userManager { get; private set; } = null;
        public Guid? XTBUser { get; private set; } = null;
        public Guid? userFrom { get; set; } = null;
        public Guid? userDestination { get; set; } = null;
        public bool isOnPrem { get; set; } = false;
        public ControllerManager(IOrganizationService service)
        {
            this.service = service;
            this.proxy = (OrganizationServiceProxy)service;
            this.userManager = new UserManager(this);
            this.viewManager = new ViewManager(this);
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
