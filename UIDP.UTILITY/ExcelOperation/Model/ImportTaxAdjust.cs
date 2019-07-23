using System;
using System.Collections.Generic;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 导入工资调整表
    /// </summary>
    public class ImportTaxAdjust
    {
        [ColName("工号")]
        public string S_WorkerCode { get; set; }

        [ColName("姓名")]
        public string S_WorkerName { get; set; }

        [ColName("部门")]
        public string S_OrgName { get; set; }

        [ColName("部门编号")]
        public string S_OrgCode { get; set; }

        [ColName("调增项5")]
        public decimal Adjust9 { get; set; }

        [ColName("调增项6")]
        public decimal Adjust10 { get; set; }

        [ColName("调增项7")]
        public decimal Adjust11 { get; set; }

        [ColName("调增项8")]
        public decimal Adjust12 { get; set; }

        [ColName("调增项9")]
        public decimal Adjust13 { get; set; }

        [ColName("调增项10")]
        public decimal Adjust14 { get; set; }

        [ColName("调增项11")]
        public decimal Adjust15 { get; set; }


        [ColName("调增项12")]
        public decimal Adjust16 { get; set; }

        [ColName("调增项13")]
        public decimal Adjust17 { get; set; }

        [ColName("调增项14")]
        public decimal Adjust18 { get; set; }

        [ColName("调减项5")]
        public decimal Adjust19 { get; set; }

        [ColName("调减项6")]
        public decimal Adjust20 { get; set; }

        [ColName("调减项7")]
        public decimal Adjust21 { get; set; }

        [ColName("调减项8")]
        public decimal Adjust22 { get; set; }

        [ColName("调减项9")]
        public decimal Adjust23 { get; set; }

        [ColName("调减项10")]
        public decimal Adjust24 { get; set; }

        [ColName("调减项11")]
        public decimal Adjust25 { get; set; }

        [ColName("调减项12")]
        public decimal Adjust26 { get; set; }

        [ColName("调减项13")]
        public decimal Adjust27 { get; set; }

        [ColName("调减项14")]
        public decimal Adjust28 { get; set; }

        [ColName("备注")]
        public string S_Remark { get; set; }

    }
}
