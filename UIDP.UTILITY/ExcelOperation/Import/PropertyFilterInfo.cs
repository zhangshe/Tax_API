using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    public class PropertyFilterInfo
    {
        public int ColIndex { get; set; }
        public List<BaseFilterAttribute> FilterAttrs { get; set; }
    }
}
