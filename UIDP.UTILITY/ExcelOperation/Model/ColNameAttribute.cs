using System;

namespace UIDP.UTILITY
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColNameAttribute : Attribute
    {
        public ColNameAttribute(string colName)
        {
            this.ColName = colName;
        }

        public string ColName { get; set; }
    }
}
