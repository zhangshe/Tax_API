using System;
using System.Collections.Generic;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.UTILITY
{
    /// <summary>
    /// 导入纳税人基本信息
    /// </summary>
    public class ImportTaxPayerInfo
    {
        [ColName("单位")]
        public string S_OrgName { get; set; }
        [ColName("工号")]
        public string WorkerNumber { get; set; }
        [ColName("*姓名")]
        public string WorkerName { get; set; }
        
        //[ColName("单位编号")]
        //public string S_OrgCode { get; set; }
        [ColName("*证照类型")]
        public string IdType { get; set; }
        public string IdTypeCode { get; set; }

        [ColName("*证照号码")]
        public string IdNumber { get; set; }

        [ColName("*国籍(地区)")]
        public string Nationality { get; set; }

        public string NationalityId { get; set; }

        [ColName("*性别")]
        public string Sex { get; set; }

        [ColName("*人员状态")]
        public string WorkerStatus { get; set; }

        [ColName("*出生日期")]
        public string BirthDate { get; set; }

        [ColName("*任职受雇从业类型")]
        public string JobType { get; set; }

        public string JobTypeCode { get; set; }

        [ColName("手机号码")]
        public string Tel { get; set; }

        [ColName("任职受雇从业日期")]
        public string EmployeeDate { get; set; }

        [ColName("离职日期")]
        public string QuitDate { get; set; }

        [ColName("是否残疾")]
        public string IsDisability { get; set; }

        [ColName("是否烈属")]
        public string IsLieShu { get; set; }

        [ColName("是否孤老")]
        public string IsAlone { get; set; }

        [ColName("残疾证号")]
        public string DisabilityNo { get; set; }

        [ColName("烈属证号")]
        public string LiShuZH { get; set; }

        [ColName("个人投资额")]
        public string Investment { get; set; }

        [ColName("个人投资比例(%)")]
        public decimal PerInvestment { get; set; }

        [ColName("备注")]
        public string Remark { get; set; }

        [ColName("是否境外人员")]
        public string IsAbroad { get; set; }

        [ColName("中文名")]
        public string BroadName { get; set; }

        [ColName("出生国家(地区)")]
        public string BirthPlace { get; set; }


        [ColName("首次入境时间")]
        public string FirstEntryTime { get; set; }

        //[ColName("本年入境时间")]//新模板没有用上
        //public string ThisYearEntryTime { get; set; }

        [ColName("预计离境时间")]
        public string EstimatedDepartureTime { get; set; }

        [ColName("其他证照类型")]
        public string OtherIdType { get; set; }
        public string OtherIdTypeCode { get; set; }

        [ColName("其他证照号码")]
        public string OtherIdNumber { get; set; }

        [ColName("户籍所在地（省）")]
        public string Province { get; set; }

        [ColName("户籍所在地（市）")]
        public string City { get; set; }

        [ColName("户籍所在地（区县）")]
        public string County { get; set; }

        [ColName("户籍所在地（详细地址）")]
        public string Domicile { get; set; }

        [ColName("居住地址（省）")]
        public string Adress_Province { get; set; }

        [ColName("居住地址（市）")]
        public string Adress_City { get; set; }

        [ColName("居住地址（区县）")]
        public string Adress_County { get; set; }

        [ColName("居住地址（详细地址）")]
        public string PostalAddress { get; set; }
        [ColName("联系地址（省）")]
        public string L_Province { get; set; }
        [ColName("联系地址（市）")]
        public string L_City { get; set; }
        [ColName("联系地址（区县）")]
        public string L_County { get; set; }
        [ColName("联系地址（详细地址）")]
        public string L_Adress { get; set; }

        [ColName("电子邮箱")]
        public string Email { get; set; }

        [ColName("学历")]
        public string Education { get; set; }
        public string EducationCode { get; set; }

        [ColName("开户银行")]
        public string BankName { get; set; }

        [ColName("银行账号")]
        public string BankNumber { get; set; }

        [ColName("职务")]
        public string WorkPost { get; set; }
        public string WorkPostCode { get; set; }






        #region  以下是老模板剩余列，在新模板中并没有用到。
       

        //[ColName("是否雇员")]
        //public string IsEmployee { get; set; }

        //[ColName("职业")]
        //public string Occupation { get; set; }
        //public string OccupationCode { get; set; }
        //[ColName("是否特定行业")]
        //public string IsSpecialIndustry { get; set; }

        //[ColName("是否在职")]
        //public string IsWorking { get; set; }

        //[ColName("是否股东、投资者")]
        //public string IsShareholder { get; set; }
        //[ColName("境内有无住所")]
        //public string IsLive { get; set; }

        //[ColName("联系地址（省）")]
        //public string S_Province { get; set; }
        //[ColName("联系地址（市）")]
        //public string S_City { get; set; }
        //[ColName("联系地址（区县）")]
        //public string S_County { get; set; }
        //[ColName("联系地址（详细地址）")]
        //public string S_Address { get; set; }
        //[ColName("支付地")]
        //public string PayPlace { get; set; }
        //[ColName("境外支付地")]
        //public string OtherPayPlace { get; set; }
        //[ColName("境内职务")]
        //public string ChinaPost { get; set; }
        //[ColName("境外职务")]
        //public string UnChinaPost { get; set; }
        //[ColName("任职期限")]
        //public string OfficeTime { get; set; }
        //[ColName("境外纳税人识别号")]
        //public string TaxpayersNumber { get; set; }
        #endregion
    }
}
