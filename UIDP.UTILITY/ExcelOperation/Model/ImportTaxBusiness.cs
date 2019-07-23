using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.UTILITY.ExcelOperation.Model
{
    public class ImportTaxBusiness
    {
        [ColName("工号")]
        public string WorkNumber { get; set; }
        [ColName("姓名")]
        public string Name { get; set; }
        [ColName("证照类型")]
        public string IDType { get; set; }
        [ColName("证照号码")]
        public string IDNumber { get; set; }
        [ColName("税款所属期起")]//改字段说明
        public DateTime StartTime { get; set; }
        [ColName("税款所属期止")]//改字段说明
        public DateTime EndTime { get; set; }
        [ColName("所得项目")]//新加字段
        public string IncomeItem { get; set; }
        [ColName("本期收入")]
        public decimal Income { get; set; }
        [ColName("本期费用")]//新加字段
        public decimal Cost { get; set; }
        
        [ColName("本期免税收入")]
        public decimal Tax { get; set; }
        [ColName("本期基本养老保险费")]//修改字段说明
        public decimal OlderInsurance { get; set; }
        [ColName("本期基本医疗保险费")]//修改字段说明
        public decimal HeathInsurance { get; set; }
        [ColName("本期失业保险费")]//修改字段说明
        public decimal JobInsurance { get; set; }
        [ColName("本期住房公积金")]//修改字段说明
        public decimal HousingFund { get; set; }

        [ColName("本期企业(职业)年金")]//修改字段说明
        public decimal EnterpriseAnnuity { get; set; }

        [ColName("本期商业健康保险费")]//修改字段说明
        public decimal CommercialHealthinsurance { get; set; }

        [ColName("本期税延养老保险费")]//修改字段说明
        public decimal EndowmentInsurance { get; set; }

        [ColName("本期其他扣除(其他)")]//修改字段说明
        public decimal Other { get; set; }
        [ColName("累计收入额")]//添加字段
        public decimal AccumulatedIncome { get; set; }
        [ColName("累计减除费用")]//修改字段名
        public decimal Reduction { get; set; }

        [ColName("累计专项扣除")]//添加字段
        public decimal AccumulatedSpecialDeduction { get; set; }


        [ColName("累计子女教育支出扣除")]//修改字段名
        public decimal ChildEdu { get; set; }
        [ColName("累计住房贷款利息支出扣除")]//修改字段名
        public decimal HousingLoan { get; set; }
        [ColName("累计住房租金支出扣除")]//修改字段名
        public decimal HousingRent { get; set; }
        [ColName("累计赡养老人支出扣除")]//修改字段说明
        public decimal Support { get; set; }
        [ColName("累计继续教育支出扣除")]//修改字段说明
        public decimal ContinueEdu { get; set; }
        [ColName("累计其他扣除")]//新增字段
        public decimal CumulativeOther { get; set; }




        [ColName("累计准予扣除的捐赠")]//修改字段说明
        public decimal Donation { get; set; }
        [ColName("累计应纳税所得额")]//修改字段说明
        public decimal Deductions { get; set; }
        [ColName("税率")]//新加字段
        public decimal TaxRate { get; set; }
        [ColName("速算扣除数")]//新加字段
        public decimal QuickDeduction { get; set; }

        [ColName("累计应纳税额")]//新加字段
        public decimal AccumulatedTax { get; set; }

        [ColName("累计减免税额")]//修改字段说明
        public decimal TaxSavings { get; set; }


        [ColName("累计应扣缴税额")]//新加字段
        public decimal CumulativeWithholding { get; set; }

        [ColName("累计已预缴税额")]//修改字段说明
        public decimal WithholdingTax { get; set; }
        [ColName("累计应补(退)税额")]//新加字段
        public decimal Drawback { get; set; }
        [ColName("备注")]
        public string Remark { get; set; }
    }
}
