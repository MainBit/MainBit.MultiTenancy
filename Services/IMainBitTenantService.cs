using MainBit.MultiTenancy.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using Orchard.MultiTenancy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainBit.MultiTenancy.Services
{
    public interface IMainBitTenantService : IDependency
    {
        string TenantUrl(ShellSettings tenantShellSettings);
        ShellSettings GetTenantByUrl(string url);
    }

    public class MainBitTenantService : IMainBitTenantService
    {
        private readonly ITenantWorkContextAccessor _twca;
        private readonly UrlHelper _urlHelper;
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _wca;
        private readonly ITenantService _tenantService;

        public MainBitTenantService(ITenantWorkContextAccessor twca,
            UrlHelper urlHelper,
            IContentManager contentManager,
            IWorkContextAccessor wca,
            ITenantService tenantService)
        {
            _twca = twca;
            _urlHelper = urlHelper;
            _contentManager = contentManager;
            _wca = wca;
            _tenantService = tenantService;
        }

        public string TenantUrl(ShellSettings tenantShellSettings)
        {
            string result = null;
            if (!string.IsNullOrEmpty(tenantShellSettings.RequestUrlHost))
            {
                var tenantHosts = tenantShellSettings.RequestUrlHost.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (tenantHosts.Length > 0)
                {
                    // need to pick suitable host
                    //result = string.Format("//{0}", tenantHosts[0]);
                    result = string.Format("{0}://{1}", _urlHelper.RequestContext.HttpContext.Request.Url.Scheme, tenantHosts[0]);
                }
            }
            
            if(result == null)
            {
                var defaultWorkContext = _twca.GetContext(ShellSettings.DefaultName);
                result = defaultWorkContext.CurrentSite.BaseUrl;
            }

            //application path alwayes in result ???
            //var applicationPath = _urlHelper.RequestContext.HttpContext.Request.ApplicationPath;
            //if (!string.IsNullOrEmpty(applicationPath) && !string.Equals(applicationPath, "/"))
            //    result += applicationPath;

            if (!string.IsNullOrEmpty(tenantShellSettings.RequestUrlPrefix))
                result += "/" + tenantShellSettings.RequestUrlPrefix;

            return result;
        }

        public ShellSettings GetTenantByUrl(string url)
        {
            string url2;
            if (url.StartsWith("//"))
            {
                url2 = url.Substring(2);
            }
            else if (url.StartsWith("http://"))
            {
                url2 = url.Substring(7);
            }
            else if (url.StartsWith("https://"))
            {
                url2 = url.Substring(8);
            }
            else
            {
                url2 = url;
            }

            var tenantHosts = _tenantService.GetTenants().ToDictionary(t => t, t => t.RequestUrlHost.Split(','));
            return tenantHosts
                .Where(t => t.Value.Any(tenantHost => url2.StartsWith(tenantHost)))
                .Select(t => t.Key)
                .FirstOrDefault();
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