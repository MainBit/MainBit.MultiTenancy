using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.Utility.Extensions;
using Orchard.Mvc.Html;
using MainBit.MultiTenancy.Services;
using Orchard;
using Orchard.Environment.Configuration;
using MainBit.MultiTenancy.Models;
using Orchard.MultiTenancy.Services;
using System.Linq;

namespace MainBit.MultiTenancy.Helpers {
    public static class ITenantServiceExtensions
    {
        public static ShellSettings GetTenant(this ITenantService tenantService, string tenantName) {
            return tenantService.GetTenants().FirstOrDefault(t =>
                string.Equals(t.Name, tenantName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
