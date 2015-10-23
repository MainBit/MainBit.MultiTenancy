using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.MultiTenancy.Models
{
    public class TenantPartRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
    }
}