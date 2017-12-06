using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Carfup.XTBPlugins.AppCode
{
    public class ControllerManager
    {
        public IOrganizationService service { get; set; } = null;
        public OrganizationServiceProxy proxy { get; set; } = null;
        public ViewManager viewManager { get; set; } = null;
        public UserManager userManager { get; set; } = null;
        public Guid? XTBUser { get; set; } = null;
        public Guid? userFrom { get; set; } = null;
        public Guid? userDestination { get; set; } = null;

        public ControllerManager(IOrganizationService service)
        {
            this.service = service;
            this.proxy = (OrganizationServiceProxy)service;
            this.userManager = new UserManager(this);
            this.viewManager = new ViewManager(this);
            this.XTBUser = ((WhoAmIResponse)this.service.Execute(new WhoAmIRequest())).UserId;
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
