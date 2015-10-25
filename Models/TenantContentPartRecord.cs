using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.MultiTenancy.Models
{
    public class TenantContentPartRecord : ContentPartRecord
    {
        public virtual int MasterContentItemId { get; set; }
    }
}