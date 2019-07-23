using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDP.UTILITY
{
    public static class FiltersFlyWeight
    {
        private static Hashtable Table = Hashtable.Synchronized(new Hashtable(1024));
        public static List<IFilter> CreateFilters<TTemplate>(ExcelHeaderRow headerRow)
        {
            Type templateType = typeof(TTemplate);
            var key = templateType;
            if (Table[key] != null)
            {
                return (List<IFilter>)Table[key];
            }

            List<IFilter> filters = new List<IFilter>();
            List<BaseFilterAttribute> attrs = new List<BaseFilterAttribute>();
            TypeFilterInfo typeFilterInfo = TypeFilterInfoFlyweight.CreateInstance(typeof(TTemplate), headerRow);

            typeFilterInfo.PropertyFilterInfos.ForEach(a => a.FilterAttrs.ForEach(f => attrs.Add(f)));

            attrs.Distinct(new FilterAttributeComparer()).ToList().ForEach
            (a =>
            {
                var filter = FilterFactory.CreateInstance(a.GetType());
                if (filter != null)
                {
                    filters.Add(filter);
                }
            });

            Table[key] = filters;
            return filters;
        }
    }
}
