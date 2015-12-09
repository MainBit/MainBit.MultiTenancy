using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Configuration;

namespace MainBit.MultiTenancy.Models
{
    public class TenantPart : ContentPart<TenantPartRecord>
    {

        public string Name
        {
            get { return Retrieve<string>(v => v.Name); }
            set { Store(x => x.Name, value); }
        }

        internal LazyField<ShellSettings> ShellSettingsField = new LazyField<ShellSettings>();
        public ShellSettings ShellSettings
        {
            get { return ShellSettingsField.Value; }
            //set { ShellSettingsField.Value = value; }
        }
    }
}