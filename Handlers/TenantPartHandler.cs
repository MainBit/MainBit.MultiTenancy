using JetBrains.Annotations;
using System.Linq;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using MainBit.MultiTenancy.Models;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;

namespace MainBit.MultiTenancy.Handlers
{
    [UsedImplicitly]
    [OrchardFeature("MainBit.MultiTenancy.Relations")]
    public class TenantPartHandler : ContentHandler {
        
        private readonly IShellSettingsManager _shellSettingsManager;

        public TenantPartHandler(IShellSettingsManager shellSettingsManager, IRepository<TenantPartRecord> repository)
        {
            _shellSettingsManager = shellSettingsManager;

            Filters.Add(StorageFilter.For(repository));

            OnLoading<TenantPart>((context, part) => LazyLoadHandlers(part));
        }

        protected void LazyLoadHandlers(TenantPart part)
        {
            part.ShellSettingsField.Loader(() => string.IsNullOrWhiteSpace(part.Name)
                ? null
                : _shellSettingsManager.LoadSettings().Where(settings => settings.Name == part.Name).FirstOrDefault());
        }
    }
}