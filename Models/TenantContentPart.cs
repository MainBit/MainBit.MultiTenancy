using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.MultiTenancy.Models
{
    public class TenantContentPart : ContentPart<TenantContentPartRecord>
    {
        public int MasterContentItemId
        {
            get { return Retrieve(v => v.MasterContentItemId); }
            set { Store(x => x.MasterContentItemId, value); }
        }
    }
}