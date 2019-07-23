using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    public class TypeDecoratorInfo
    {
        public List<BaseDecorateAttribute> TypeDecoratorAttrs { get; set; }
        public List<PropertyDecoratorInfo> PropertyDecoratorInfos { get; set; }
    }
}
