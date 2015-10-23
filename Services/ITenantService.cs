using MainBit.MultiTenancy.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainBit.MultiTenancy.Services
{
    public interface ITenantService : IDependency
    {
        string TenantUrl(ShellSettings tenantShellSettings);
    }

    public class TenantService : ITenantService
    {
        private readonly ITenantWorkContextAccessor _twca;
        private readonly UrlHelper _urlHelper;
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _wca;
        private readonly Orchard.Localization.Services.ICultureManager _cultureManager;

        public TenantService(ITenantWorkContextAccessor twca,
            UrlHelper urlHelper,
            IContentManager contentManager,
            IWorkContextAccessor wca)
        {
            _twca = twca;
            _urlHelper = urlHelper;
            _contentManager = contentManager;
            _wca = wca;
        }

        public string TenantUrl(ShellSettings tenantShellSettings)
        {
            var defaultWorkContext = _twca.GetWorkContext(ShellSettings.DefaultName);
            var defaultBaseUrl = defaultWorkContext.CurrentSite.BaseUrl;

            string tenantHost = null;
            if (!string.IsNullOrEmpty(tenantShellSettings.RequestUrlHost))
            {
                var tenantHosts = tenantShellSettings.RequestUrlHost.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (tenantHosts.Length > 0)
                {
                    // need to pick suitable host
                    //tenantHost = string.Format("{0}://{1}", _urlHelper.RequestContext.HttpContext.Request.Url.Scheme, tenantHosts[0]);
                    tenantHost = string.Format("//{0}", tenantHosts[0]);
                }
            }

            var result = tenantHost ?? defaultBaseUrl;

            var applicationPath = _urlHelper.RequestContext.HttpContext.Request.ApplicationPath;
            if (!string.IsNullOrEmpty(applicationPath) && !string.Equals(applicationPath, "/"))
                result += applicationPath;

            if (!string.IsNullOrEmpty(tenantShellSettings.RequestUrlPrefix))
                result += "/" + tenantShellSettings.RequestUrlPrefix;

            return result;
        }

        public void Test()
        {
            //var workContext = _wca.GetContext();

            //string currentCulture = _wca.GetContext().CurrentCulture;
            //var currentCultureId = _cultureManager.GetCultureByName(currentCulture).Id;

            //var branches = _contentManager.HqlQuery()
            //    .ForPart<TenantPart>()
            //    .Where(
            //        x => x.ContentPartRecord<Orchard.Localization.Models.LocalizationPartRecord>(),
            //        x => x.Eq("CultureId", currentCultureId))
            //    .Where(
            //        x => x.ContentPartRecord<Orchard.Projections.Models.FieldIndexPartRecord>().Property("IntegerFieldIndexRecords", "Branch.SortOrder."),
            //        x => x.Eq("PropertyName", "Branch.SortOrder.")
            //    )
            //    .OrderBy(
            //        x => x.Named("Branch.SortOrder."),
            //        x => x.Asc("Value")
            //    )
            //    .OrderBy(
            //        x => x.ContentPartRecord<Orchard.Core.Title.Models.TitlePartRecord>(),
            //        x => x.Asc("Title")
            //    )
            //    .List()
            //    .ToList();

            

            //var query = (IHqlQuery)context.Query;
            //context.Query = query.Where(x => x.ContentPartRecord<LocalizationPartRecord>(), x => x.Eq("CultureId", currentCultureId));
        }
    }
}