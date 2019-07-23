using System;
using System.Collections.Generic;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 工资导入模板2（61列）
    /// </summary>
    public class ImportTaxSalary2
    {
        [ColName("员工编号")]
        public string S_WorkerCode { get; set; }

        [ColName("姓名")]
        public string S_WorkerName { get; set; }

        [ColName("部门")]
        public string S_OrgName { get; set; }

        [ColName("岗位（技）工资")]
        public decimal G_GWJGZ { get; set; }

        [ColName("岗位（技）工资（补）")]
        public decimal G_GWJGZB { get; set; }

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

        [ColName("政府津贴")]
        public decimal G_ZFJT { get; set; }

        [ColName("回民津贴")]
        public decimal G_HMJT { get; set; }

        [ColName("驾驶津贴")]
        public decimal G_JSJT { get; set; }

        [ColName("信访工作津贴")]
        public decimal G_XFGZJT { get; set; }

        [ColName("法轮功斗争津贴")]
        public decimal G_FLGDZJT { get; set; }

        [ColName("班主任津贴")]
        public decimal G_BZRJT { get; set; }

        [ColName("医疗卫生津贴")]
        public decimal G_SYYLJT { get; set; }

        [ColName("夜班津贴")]
        public decimal G_YBJT { get; set; }

        [ColName("星期加班")]
        public decimal G_XQJB { get; set; }

        [ColName("平时加班")]
        public decimal G_PSJB { get; set; }

        [ColName("节日加班")]
        public decimal G_JRJB { get; set; }

        [ColName("基础月奖")]
        public decimal G_JCYJ { get; set; }

        [ColName("业绩奖金")]
        public decimal G_YJJJ { get; set; }

        [ColName("总额内补发（补扣）")]
        public decimal G_ZENBFBK { get; set; }

        [ColName("专项奖")]
        public decimal G_ZXJ { get; set; }

        [ColName("C01专项奖")]
        public decimal G_C01 { get; set; }

        [ColName("C02专项奖")]
        public decimal G_C02 { get; set; }

        [ColName("C03专项奖")]
        public decimal G_C03 { get; set; }

        [ColName("C04专项奖")]
        public decimal G_C04 { get; set; }

        [ColName("小计1")]
        public decimal T_XJ1 { get; set; }

        [ColName("独生子女补贴")]
        public decimal G_DSZNJT { get; set; }

        [ColName("保健食品津贴")]
        public decimal G_BJSPJT { get; set; }

        [ColName("防暑降温费")]
        public decimal G_FSJWF { get; set; }

        [ColName("误餐补助")]
        public decimal G_WCBZ { get; set; }

        [ColName("通讯补助")]
        public decimal G_TXBZ { get; set; }

        [ColName("交通津贴")]
        public decimal G_JTBZ { get; set; }

        [ColName("合理化建议奖")]
        public decimal G_HLHJYJ { get; set; }

        [ColName("C05专项奖")]
        public decimal G_C05 { get; set; }

        [ColName("总额外补发（补扣）")]
        public decimal G_ZEWBFBK { get; set; }

        [ColName("疗养费")]
        public decimal G_LYF { get; set; }

        [ColName("小计2")]
        public decimal T_XJ2 { get; set; }

        [ColName("小计3")]
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
