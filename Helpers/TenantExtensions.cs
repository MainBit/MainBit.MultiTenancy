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

namespace MainBit.MultiTenancy.Helpers {
    public static class TenantExtensions {

        public static string TenantUrl(this UrlHelper urlHelper, ShellSettings tenantShellSettings)
        {
            var workContext = urlHelper.RequestContext.GetWorkContext();
            var tenantService = workContext.Resolve<IMainBitTenantService>();
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
            var tenantWorkContext = multiTenancyService.GetContext(tenantName);

            return tenantWorkContext;
        }

        public static string TenantContentUrl(this UrlHelper urlHelper, IContent content, string tenantName)
        {
            var workContext = urlHelper.RequestContext.GetWorkContext();
            var _tenantContentService = workContext.Resolve<ITenantContentService>();
            return _tenantContentService.ItemDisplayUrl(content, tenantName);
        }
    }
}
