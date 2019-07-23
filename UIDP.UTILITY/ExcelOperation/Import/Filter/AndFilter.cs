using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    public class AndFilter : IFilter
    {
        public List<IFilter> filters { get; set; }

        public Type AttributeType => null;

        public List<ExcelDataRow> Filter(List<ExcelDataRow> excelDataRows, FilterContext context)
        {
            foreach (var filter in filters)
            {
                excelDataRows = filter.Filter(excelDataRows, context);
            }

            return excelDataRows;
        }
    }
}
