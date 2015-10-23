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

namespace MainBit.MultiTenancy.Helpers {
    public static class TenantExtensions {

        public static string TenantUrl(this UrlHelper urlHelper, ShellSettings tenantShellSettings)
        {
            var workContext = urlHelper.RequestContext.GetWorkContext();
            var tenantService = workContext.Resolve<ITenantService>();
            return tenantService.TenantUrl(tenantShellSettings);
        }


        public static HtmlHelper TenantHtmlHelper(this HtmlHelper html, string tenantName)
        {
            var tenantWorkContext = html.GetTenantWorkContext(tenantName);
            if (tenantWorkContext == null) { return null; }

            return new HtmlHelper(
                    new ViewContext { HttpContext = tenantWorkContext.HttpContext },
                    new ViewPage(),
                    tenantWorkContext.Resolve<RouteCollection>());
        }

        public static WorkContext GetTenantWorkContext(this HtmlHelper html, string tenantName)
        {
            var currentWorkContext = html.GetWorkContext();
            var multiTenancyService = currentWorkContext.Resolve<ITenantWorkContextAccessor>();
            var tenantWorkContext = multiTenancyService.GetWorkContext(tenantName);

            return tenantWorkContext;
        }
    }
}
