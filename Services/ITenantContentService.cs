using MainBit.MultiTenancy.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Html;
using Orchard.Mvc.Routes;
using Orchard.MultiTenancy.Services;
using MainBit.MultiTenancy.Helpers;
using Orchard.Mvc.Extensions;

namespace MainBit.MultiTenancy.Services
{
    public interface ITenantContentService : IDependency
    {
        TenantContentPart Get(int masterContentItemId);
        string ItemVirtualPath(IContent content, string tenantName);
        string ItemDisplayUrl(IContent content, string tenantName);
    }

    public class TenantContentService : ITenantContentService
    {
        private readonly ITenantWorkContextAccessor _twca;
        private readonly IContentManager _contentManager;
        private readonly ShellSettings _shellSettings;
        private readonly UrlHelper _urlHelper;
        private readonly IMainBitTenantService _mainBitTenantService;
        private readonly ITenantService _tenantService;
        private readonly IWorkContextAccessor _wca;
        private readonly UrlPrefix _urlPrefix;

        public TenantContentService(ITenantWorkContextAccessor twca,
            IContentManager contentManager,
            ShellSettings shellSettings,
            UrlHelper urlHelper,
            IMainBitTenantService mainBitTenantService,
            ITenantService tenantService,
            IWorkContextAccessor wca)
        {
            _twca = twca;
            _contentManager = contentManager;
            _shellSettings = shellSettings;
            _urlHelper = urlHelper;
            _mainBitTenantService = mainBitTenantService;
            _tenantService = tenantService;
            _wca = wca;

            if (!string.IsNullOrEmpty(_shellSettings.RequestUrlPrefix))
                _urlPrefix = new UrlPrefix(_shellSettings.RequestUrlPrefix);
        }

        public TenantContentPart Get(int masterContentItemId)
        {
            return _contentManager
                .Query<TenantContentPart, TenantContentPartRecord>()
                .Where(p => p.MasterContentItemId == masterContentItemId)
                .Slice(1)
                .ToList()
                .FirstOrDefault();
        }

        public string ItemVirtualPath(IContent content, string tenantName)
        {
            var workContext = _wca.GetContext();
            string virtualPath = null;

            if (tenantName == _shellSettings.Name)
            {
                virtualPath = _urlHelper.ItemDisplayUrl(content);
            }
            else if (_shellSettings.Name == ShellSettings.DefaultName)
            {
                // from the default tenant
                virtualPath = _urlHelper.Action("DisplayOnDependent", "TenantContent", new { area = "MainBit.MultiTenancy", masterContentItemId = content.Id });
            }
            else
            {
                // from an dependent tenant
                var tenantContent = content.As<TenantContentPart>();
                if (tenantContent != null)
                {
                    if (tenantName == ShellSettings.DefaultName)
                    {
                        // to the default tenant
                        virtualPath = _urlHelper.Action("DisplayOnDefault", "TenantContent", new { area = "MainBit.MultiTenancy", id = tenantContent.MasterContentItemId });
                    }
                    else
                    {
                        // to an other dependent tenant
                        virtualPath = _urlHelper.Action("DisplayOnDependent", "TenantContent", new { area = "MainBit.MultiTenancy", masterContentItemId = tenantContent.MasterContentItemId });
                    }
                }
                else
                {
                    virtualPath = "";
                }
            }

            virtualPath = virtualPath.Substring(workContext.HttpContext.Request.ApplicationPath.Length).TrimStart('/');

            if (_urlPrefix != null)
                virtualPath = _urlPrefix.RemoveLeadingSegments(virtualPath);

            return virtualPath;
        }

        public string ItemDisplayUrl(IContent content, string tenantName)
        {
            var tenant = _tenantService.GetTenant(tenantName);
            var tenantUrl = _mainBitTenantService.TenantUrl(tenant);
            var virtualPath = ItemVirtualPath(content, tenantName);

            return virtualPath == string.Empty
                ? tenantUrl
                : tenantUrl + "/" + virtualPath;
        }
    }
}