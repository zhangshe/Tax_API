using System;
using System.Collections.Generic;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 导入一次性奖金
    /// </summary>
    public class ImportTaxBonus
    {
        [ColName("工号")]
        public string S_WorkerCode { get; set; }

        [ColName("姓名")]
        public string S_WorkerName { get; set; }

        [ColName("部门")]
        public string S_OrgName { get; set; }

        [ColName("部门编号")]
        public string S_OrgCode { get; set; }
        
        [ColName("奖金")]
        public decimal OneTimeBonus { get; set; }

        [ColName("预扣税")]
        public decimal DeductibleTax { get; set; }

    }
}
