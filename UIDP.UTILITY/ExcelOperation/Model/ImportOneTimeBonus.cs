using System;
using System.Collections.Generic;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 导入一次性奖金
    /// </summary>
    public class ImportOneTimeBonus
    {
        [ColName("工号")]
        public string S_WorkerCode { get; set; }

        [ColName("姓名")]
        public string S_WorkerName { get; set; }

        public string S_OrgName { get; set; }

        public string S_OrgCode { get; set; }
        public string IdType { get; set; }
        [ColName("*证照类型")]
        public string IdTypeName { get; set; }

        [ColName("*证照号码")]
        public string IdNumber { get; set; }

        [ColName("*全年一次性奖金额")]
        public decimal OneTimeBonus { get; set; }
        [ColName("免税收入")]
        public decimal FreeIncome { get; set; }
        [ColName("其他")]
        public decimal Other { get; set; }

        [ColName("准予扣除的捐赠额")]
        public decimal AllowDeduction { get; set; }
        [ColName("减免税额")]
        public decimal TaxSaving { get; set; }
        [ColName("已扣缴税额")]
        public decimal DeductibleTax { get; set; }

        [ColName("备注")]
        public string Remark { get; set; }

    }
}
