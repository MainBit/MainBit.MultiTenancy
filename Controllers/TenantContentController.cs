using MainBit.MultiTenancy.Services;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Html;
using Orchard.MultiTenancy.Services;
using Orchard.Mvc.Extensions;
using Orchard.ContentManagement;

namespace MainBit.MultiTenancy.Controllers
{
    [OrchardFeature("MainBit.MultiTenancy.ContentRelations")]
    public class TenantContentController : Controller
    {
        private readonly ITenantContentService _tenantContentManager;
        private readonly IContentManager _contentManager;
        private readonly UrlHelper _urlHelper;
        private readonly IMainBitTenantService _mainBitTenantService;
        private readonly ITenantService _tenantService;

        public TenantContentController(ITenantContentService tenantContentManager,
            IContentManager contentManager,
            UrlHelper urlHelper,
            IMainBitTenantService mainBitTenantService,
            ITenantService tenantService)
        {
            _tenantContentManager = tenantContentManager;
            _contentManager = contentManager;
            _urlHelper = urlHelper;
            _mainBitTenantService = mainBitTenantService;
            _tenantService = tenantService;
        }

        // for displaying content item on the default tenant (uses from dependent tenant)
        public ActionResult DisplayOnDefault(int id)
        {
            var contentItem = _contentManager.Get(id);
            if (contentItem != null)
            {
                return RedirectPermanent(_urlHelper.ItemDisplayUrl(contentItem));
            }
            else
            {
                return Redirect("~/");
            }
        }

        // for displaying content item on a dependent tenant  (uses from a dependent tenant of the default tenant)
        public ActionResult DisplayOnDependent(int masterContentItemId)
        {
            var tenantContentPart = _tenantContentManager.Get(masterContentItemId);
            if (tenantContentPart != null)
            {
                return RedirectPermanent(_urlHelper.ItemDisplayUrl(tenantContentPart));
            }
            else
            {
                return Redirect("~/");
            }
        }
    }
}