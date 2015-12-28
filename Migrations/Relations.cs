using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace MainBit.MultiTenancy.Migrations
{
    [OrchardFeature("MainBit.MultiTenancy.Relations")]
    public class Relations : DataMigrationImpl
    {
        public int Create() {
            SchemaBuilder.CreateTable("TenantPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Name", c => c.WithLength(2048))
                );

            SchemaBuilder.AlterTable("TenantPartRecord", table => table
                .CreateIndex("IDX_TenantPartRecord_Name", "Name")
            );

            ContentDefinitionManager.AlterPartDefinition("TenantPart", part => part
                .Attachable(true)
                .WithDescription("Adds tenant picker."));

            return 1;
        }
    }
}