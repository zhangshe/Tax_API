using System;
using System.Collections.Generic;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 工资导入模板1（37列）
    /// </summary>
    public class ImportTaxSalary1
    {
        [ColName("员工编号")]
        public string S_WorkerCode { get; set; }

        [ColName("姓名")]
        public string S_WorkerName { get; set; }

        [ColName("部门")]
        public string S_OrgName { get; set; }

        [ColName("岗位（技）工资")]
        public decimal G_GWJGZ { get; set; }

        [ColName("保留工资")]
        public decimal G_BLGZ { get; set; }

        [ColName("工龄津贴")]
        public decimal G_GLJT { get; set; }

        [ColName("上岗津贴")]
        public decimal G_SGJT { get; set; }

        [ColName("技术(技能)津贴")]
        public decimal G_JSJNJT { get; set; }

        [ColName("住房补贴")]
        public decimal G_ZFBT { get; set; }

        [ColName("保留津贴")]
        public decimal G_BLJT { get; set; }


        [ColName("边远矿贴")]
        public decimal G_BYKT { get; set; }

        [ColName("其它津贴")]
        public decimal G_QTJT { get; set; }

        [ColName("夜班津贴")]
        public decimal G_YBJT { get; set; }

        [ColName("加班、加点工资")]
        public decimal G_JBJDGZ { get; set; }

        [ColName("基础月奖")]
        public decimal G_JCYJ { get; set; }

        [ColName("业绩奖金")]
        public decimal G_YJJJ { get; set; }

        [ColName("独生子女(保健防暑）")]
        public decimal G_DSZNBJFS { get; set; }

        [ColName("误餐补贴(三费)")]
        public decimal G_WCBTSF { get; set; }

        [ColName("补发（扣）")]
        public decimal G_BFK { get; set; }

        [ColName("应发合计")]
        public decimal T_YFHJ { get; set; }

        [ColName("医疗保险")]
        public decimal K_YiLiaoBX { get; set; }

        [ColName("失业保险")]
        public decimal K_SYBX { get; set; }

        [ColName("养老保险")]
        public decimal K_YangLaoBX { get; set; }

        [ColName("住房公积金")]
        public decimal K_ZFGJJ { get; set; }

        [ColName("企业年金")]
        public decimal K_QYNJ { get; set; }

        [ColName("其他扣项")]
        public decimal K_QTKX { get; set; }

        [ColName("应税合计")]
        public decimal T_YSHJ { get; set; }

        [ColName("扣税")]
        public decimal K_KS { get; set; }

        [ColName("实发合计")]
        public decimal T_SFHJ { get; set; }

        [ColName("调增项1")]
        public decimal Adjust1 { get; set; }

        [ColName("调增项2")]
        public decimal Adjust2 { get; set; }

        [ColName("调增项3")]
        public decimal Adjust3 { get; set; }

        [ColName("调增项4")]
        public decimal Adjust4 { get; set; }

        [ColName("调减项1")]
        public decimal Adjust5 { get; set; }

        [ColName("调减项2")]
        public decimal Adjust6 { get; set; }

        [ColName("调减项3")]
        public decimal Adjust7 { get; set; }

        [ColName("调减项4")]
        public decimal Adjust8 { get; set; }
    }
}
