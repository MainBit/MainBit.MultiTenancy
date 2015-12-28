using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace MainBit.MultiTenancy.Migrations
{
    [OrchardFeature("MainBit.MultiTenancy.ContentRelations")]
    public class ContentRelations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("TenantContentPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<int>("MasterContentItemId")
                );

            SchemaBuilder.AlterTable("TenantContentPartRecord", table => table
                .CreateIndex("IDX_TenantContentPartRecord_MasterContentItemId", "MasterContentItemId")
            );

            ContentDefinitionManager.AlterPartDefinition("TenantContentPart", part => part
                .Attachable(true)
                .WithDescription("Allow to set related default tenant content item id"));

            return 1;
        }
    }
}