using MainBit.MultiTenancy.Models;
using Orchard.Environment.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.MultiTenancy.ViewModels
{
    public class TenantEditorViewModel
    {
        public TenantPart TenantPart { get; set; }
        public IEnumerable<ShellSettings> TenantSettings { get; set; }
    }
}