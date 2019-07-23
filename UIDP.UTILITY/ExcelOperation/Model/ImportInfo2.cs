using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 样表二数据模型
    /// </summary>
    public class ImportInfo2
    {
        public string S_OrgName { get; set; }
        public string S_OrgCode { get; set; }
        public List<ImportTaxSalary2> List { get; set; }

    }
}
