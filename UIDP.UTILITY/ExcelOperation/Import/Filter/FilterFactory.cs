using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UIDP.UTILITY
{
    internal static class FilterFactory
    {
        public static IFilter CreateInstance(Type attrType)
        {
            IFilter filter = null;

            Type filterType = Assembly.GetAssembly(attrType).GetTypes().ToList()?.
                 Where(t => typeof(IFilter).IsAssignableFrom(t))?.
                 FirstOrDefault(t => t.IsDefined(typeof(FilterBindAttribute))
                 && t.GetCustomAttribute<FilterBindAttribute>()?.FilterAttributeType == attrType);

            if (filterType != null)
            {
                filter = (IFilter)Activator.CreateInstance(filterType);
            }

            return filter;
        }
    }
}
