using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using MainBit.MultiTenancy.Models;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;

namespace MainBit.MultiTenancy.Handlers
{
    [OrchardFeature("MainBit.MultiTenancy.ContentRelations")]
    public class TenantContentPartHandler : ContentHandler {
        
        private readonly IShellSettingsManager _shellSettingsManager;

        public TenantContentPartHandler(IRepository<TenantContentPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}