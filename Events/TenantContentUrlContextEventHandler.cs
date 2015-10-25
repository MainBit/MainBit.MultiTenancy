using MainBit.Alias.Events;
using MainBit.MultiTenancy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.MultiTenancy.Events
{
    public class TenantContentUrlContextEventHandler : IUrlContextEventHandler
    {
        private readonly ITenantContentService _tenantContentService;

        public TenantContentUrlContextEventHandler(ITenantContentService tenantContentService)
        {
            _tenantContentService = tenantContentService;
        }

        public void Changing(ChangingUrlContext context)
        {
            if (context.Content == null) return;

            foreach (var segment in context.ChangingSegments)
            {
                if (segment.UrlSegmentDescriptor.Name == "tenant")
                {
                    context.NewDisplayVirtualPath = _tenantContentService.ItemVirtualPath(
                        context.Content,
                        segment.NewValue.Name
                    );
                }
            }
        }
    }
}