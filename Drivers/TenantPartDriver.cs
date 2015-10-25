using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using Orchard.Mvc.Html;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Core.Common.Settings;
using Orchard.Core.Common.ViewModels;
using Orchard.Services;
using System.Web.Mvc;
using System.Web.Routing;
using MainBit.MultiTenancy.Models;
using MainBit.MultiTenancy.ViewModels;
using Orchard.MultiTenancy.Services;
using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;

namespace MainBit.MultiTenancy.Drivers {
    [UsedImplicitly]
    [OrchardFeature("MainBit.MultiTenancy.Relations")]
    public class TenantPartDriver : ContentPartDriver<TenantPart> {

        private readonly IShellSettingsManager _shellSettingsManager;

        public TenantPartDriver(IOrchardServices services, IShellSettingsManager shellSettingsManager)
        {
            _shellSettingsManager = shellSettingsManager;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        protected override string Prefix {
            get { return "Tenant"; }
        }

        protected override DriverResult Display(TenantPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape("Parts_Tenant",
                             () => {
                                 return shapeHelper.Parts_Tenant();
                             }),
                ContentShape("Parts_Tenant_Summary",
                             () => {
                                 return shapeHelper.Parts_Tenant_Summary();
                             })
                );
        }

        protected override DriverResult Editor(TenantPart part, dynamic shapeHelper)
        {
            var model = BuildEditorViewModel(part);
            return ContentShape("Parts_Tenant_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Tenant", Model: model, Prefix: Prefix));
        }

        protected override DriverResult Editor(TenantPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = BuildEditorViewModel(part);
            updater.TryUpdateModel(model, Prefix, null, null);
            //return Editor(part, shapeHelper);
            return ContentShape("Parts_Tenant_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Tenant", Model: model, Prefix: Prefix));
        }

        protected override void Importing(TenantPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var importedText = context.Attribute(part.PartDefinition.Name, "Name");
            if (importedText != null) {
                part.Name = importedText;
            }
        }

        protected override void Exporting(TenantPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Name", part.Name);
        }

        private TenantEditorViewModel BuildEditorViewModel(TenantPart part)
        {
            return new TenantEditorViewModel
            {
                TenantPart = part,
                TenantSettings = _shellSettingsManager.LoadSettings()
            };
        }
    }
}