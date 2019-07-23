using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    public class BaseFilterAttribute : Attribute
    {
        public string ErrorMsg { get; set; } = "非法";
    }
}
