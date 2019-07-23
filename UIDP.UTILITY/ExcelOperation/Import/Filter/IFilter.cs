using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    public interface IFilter
    {
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="rowWrappers"></param>
        /// <returns></returns>
        List<ExcelDataRow> Filter(List<ExcelDataRow> excelDataRows, FilterContext context);
    }

}
