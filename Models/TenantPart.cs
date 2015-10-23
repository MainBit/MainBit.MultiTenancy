using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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