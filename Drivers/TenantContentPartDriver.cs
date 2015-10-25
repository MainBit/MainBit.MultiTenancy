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
using System;

namespace MainBit.MultiTenancy.Drivers {
    [UsedImplicitly]
    [OrchardFeature("MainBit.MultiTenancy.ContentRelations")]
    public class TenantContentPartDriver : ContentPartDriver<TenantContentPart> {

        private readonly IShellSettingsManager _shellSettingsManager;

        public TenantContentPartDriver(IOrchardServices services, IShellSettingsManager shellSettingsManager)
        {
            _shellSettingsManager = shellSettingsManager;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        protected override string Prefix {
            get { return "TenantContent"; }
        }

        protected override DriverResult Display(TenantContentPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape("Parts_TenantContent",
                             () => {
                                 return shapeHelper.Parts_TenantContent();
                             }),
                ContentShape("Parts_TenantContent_Summary",
                             () => {
                                 return shapeHelper.Parts_TenantContent_Summary();
                             })
                );
        }

        protected override DriverResult Editor(TenantContentPart part, dynamic shapeHelper)
        {
            var model = BuildEditorViewModel(part);
            return ContentShape("Parts_TenantContent_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/TenantContent", Model: model, Prefix: Prefix));
        }

        protected override DriverResult Editor(TenantContentPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = BuildEditorViewModel(part);
            updater.TryUpdateModel(model, Prefix, null, null);
            return ContentShape("Parts_TenantContent_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/TenantContent", Model: model, Prefix: Prefix));
        }

        protected override void Importing(TenantContentPart part, Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            var importedText = context.Attribute(part.PartDefinition.Name, "ContentId");
            if (importedText != null) {
                part.MasterContentItemId = string.IsNullOrEmpty(importedText) ? 0 : Convert.ToInt32(importedText);
            }
        }

        protected override void Exporting(TenantContentPart part, Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("ContentId", part.MasterContentItemId);
        }

        private TenantContentEditorViewModel BuildEditorViewModel(TenantContentPart part)
        {
            return new TenantContentEditorViewModel
            {
                TenantContentPart = part
            };
        }
    }
}