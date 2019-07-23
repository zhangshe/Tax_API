using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY.ExcelOperation.Model
{
    public class importService
    {
        [ColName("工号")]
        public string WorkerCode { get; set; }
        [ColName("姓名")]
        public string WorkerName { get; set; }
        [ColName("*证照类型")]
        public string IDtype { get; set; }
        [ColName("*证照号码")]
        public string IDNumber { get; set; }
        [ColName("*所得项目")]
        public string IncomeItem { get; set; }
        [ColName("*本期收入")]
        public string Income { get; set; }
        [ColName("本期免税收入")]
        public decimal Tax { get; set; }
        [ColName("商业健康保险")]
        public decimal CommercialHealthinsurance { get; set; }
        [ColName("税延养老保险")]
        public decimal EndowmentInsurance { get; set; }
        [ColName("准予扣除的捐赠额")]
        public decimal Donation { get; set; }
        [ColName("其他")]
        public decimal other { get; set; }
        [ColName("减免税额")]
        public decimal TaxSavings { get; set; }
        [ColName("备注")]
        public string Remark { get; set; }
    }
}
