using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class TaxSalaryModule
    {
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion

        #region 数据查询引用
        TaxSalaryDB db = new TaxSalaryDB();//工资表
        TaxOrgDB taxOrgdb = new TaxOrgDB();//组织机构对照表
        TaxPlayerInfoDB taxPlayer = new TaxPlayerInfoDB();//纳税人信息表 
        TaxReportStatusDB taxReportStatus = new TaxReportStatusDB();//个税上报状态
        #endregion

        #region 工资相关操作
        public Dictionary<string, object> ExportPreCalTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                DataTable dt = db.getPreCalculateTax(d);
                if (dt != null)
                {
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    { str = d["S_OrgName"].ToString()+"-"+ d["S_Department"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年"; }
                    string title = str + d["S_WorkDate"].ToString().Substring(5, 2) + "月度税务测算";
                    List<string> col = new List<string>() {
                        "S_WorkerCode","S_WorkerName","S_OrgName","S_Department","S_WorkDate","IdNumber",
                        "T_YSHJ","T_BQSR","K_YangLaoBX","K_YiLiaoBX","K_SYBX",
                        "K_ZFGJJ","K_QYNJ","ChildEdu","HousingLoan","HousingRent",
                        "Support","ContinueEdu","T_DYYSSDE","T_YSSDE","D_FYJCBZ",
                        "D_SL","D_SSKCS","T_LJYJS","WithholdingTax","T_DJJE","G_JJ",
                        "K_JJYKS","G_YCXJJ","K_YCXJJYKS","K_KS","KS_HJ","LJ_KS"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, title, d["S_OrgName"].ToString(), "月度税务测算模板", 4, 33, col);
                    r["code"] = 2000;
                    r["message"] = "";
                    //r["item"] = "\\Files\\export\\" + fileName;
                }
                else
                {
                    r["message"] = "暂无导出数据";
                    r["code"] = -1;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            //r["item"] = filePath;
            return r;
        }




        public Dictionary<string, object> ExportMonthTaxSalaryDetail(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            int total = 0;
            try
            {
                DataTable dt = db.getMonthTaxSalaryDetail(d,out  total);
                if (dt != null)
                {
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    { str = d["S_OrgName"].ToString()+"-" + d["S_Department"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年"; }
                    string title = str + d["S_WorkDate"].ToString().Substring(5, 2) + "月度税务核算报表";
                    List<string> col = new List<string>() {
                        "S_WorkNumber","S_WorkName","S_OrgName","S_Department","IDType","IdNumber",
                        "StartTime","T_YFHJ","Income","Cost","Tax","OlderInsurance","HeathInsurance","JobInsurance","HousingFund","EnterpriseAnnuity",
                        "CommercialHealthinsurance","EndowmentInsurance","Other","Month_Other","Month_AccumulatedSpecialDeduction",
                        "Month_ChildEdu","Month_HousingLoan","Month_HousingRent","Month_Support","Month_ContinueEdu", "Drawback","AccumulatedTax","WithholdingTax",
                        "G_JJ","JJ_KS","OneTimeBonus","DeductibleTax","K_KS","HJ_SE"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, title, d["S_OrgName"].ToString(), "月度税务核算报表模板", 4, 35, col);
                    r["code"] = 2000;
                    r["message"] = "";
                    //r["item"] = "\\Files\\export\\" + fileName;
                }
                else
                {
                    r["message"] = "暂无导出数据";
                    r["code"] = -1;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            //r["item"] = filePath;
            return r;
        }

        public Dictionary<string, object> exportGroupMonthTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {

                DataTable dt = db.getGroupMonthTaxSalary(d);
                if (dt != null)
                {
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年"+d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    {
                        str = d["S_OrgName"].ToString()+"-"+ d["S_Department"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年" + d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    }
                        string title = str + "税务核算报表";
                    List<string> col = new List<string>() {
                        "S_OrgName","StartTime","Tax_Num","T_YFHJ","Income","Cost","Tax",
                        "OlderInsurance","HeathInsurance","JobInsurance","HousingFund","EnterpriseAnnuity","CommercialHealthinsurance","EndowmentInsurance","Other","CumulativeOther",
                        "Month_AccumulatedSpecialDeduction","Month_ChildEdu","Month_Support","Month_ContinueEdu","Month_HousingLoan","Month_HousingRent","Drawback","lj_SE"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, title, d["S_OrgName"].ToString(), "月度税务核算分组报表模板", 4, 24, col);
                    r["code"] = 2000;
                    r["message"] = "";
                    //r["item"] = "\\Files\\export\\" + fileName;
                }
                else
                {
                    r["message"] = "暂无导出数据";
                    r["code"] = -1;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            //r["item"] = filePath;
            return r;
        }

        public Dictionary<string, object> exportGroupSumMonthTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {

                DataTable dt = db.getGroupSumMonthTaxSalary(d);
                if (dt != null)
                {
                    //string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年" + d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    //string title = str + "税务核算报表";
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年累计至" + d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    {
                        str = d["S_OrgName"].ToString() +"-"+ d["S_Department"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年累计至" + d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    }
                   
                    string title = str + "税务核算报表";
                    List<string> col = new List<string>() {
                        "S_OrgName","StartTime","ljYFHJ","AccumulatedIncome","ljOlderInsurance","ljHeathInsurance","ljJobInsurance",
                        "ljHousingFund","ljEnterpriseAnnuity","Donation","Deductions","TaxSavings","AccumulatedSpecialDeduction","CumulativeOther",
                        "ChildEdu","HousingLoan","HousingRent","Support","ContinueEdu","AccumulatedTax","CumulativeWithholding","WithholdingTax","Drawback","K_KS"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, title, d["S_OrgName"].ToString(), "月度累计税务核算分组报表模板", 4, 24, col);
                    r["code"] = 2000;
                    r["message"] = "";
                    //r["item"] = "\\Files\\export\\" + fileName;
                }
                else
                {
                    r["message"] = "暂无导出数据";
                    r["code"] = -1;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            //r["item"] = filePath;
            return r;
        }
        public Dictionary<string, object> ExportMonthTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            int total = 0;
            try
            {
                DataTable dt = db.getMonthTaxSalary(d, out total);
                if (dt != null)
                {
                    //d["S_OrgName"].ToString() 

                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年累计至" + d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    {
                        str = d["S_OrgName"].ToString() + "-" + d["S_Department"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年累计至" + d["S_WorkDate"].ToString().Substring(5, 2) + "月";
                    }
                    //string str =  d["S_WorkDate"].ToString().Substring(0,4)+"年累计至"+ d["S_WorkDate"].ToString().Substring(5,2)+"月";
                    //if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    //{ str = str + d["S_Department"].ToString(); }
                    string title = str + "税务核算报表";
                    List<string> col = new List<string>() {
                        "S_WorkNumber","S_WorkName","S_OrgName","S_Department","IDType","IDNumber",
                        "ljYFHJ","AccumulatedIncome","ljOlderInsurance","ljHeathInsurance","ljJobInsurance","ljHousingFund","ljEnterpriseAnnuity",
                        "Donation","Deductions","TaxSavings","AccumulatedSpecialDeduction","CumulativeOther",
                        "ChildEdu","HousingLoan","HousingRent","Support","ContinueEdu",
                        "Reduction","TaxRate","QuickDeduction","AccumulatedTax","CumulativeWithholding","WithholdingTax","Drawback","K_KS"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, title, d["S_OrgName"].ToString(), "月度累计税务核算报表模板", 4, 31, col);
                    r["code"] = 2000;
                    r["message"] = "";
                    //r["item"] = "\\Files\\export\\" + fileName;
                }
                else
                {
                    r["message"] = "暂无导出数据";
                    r["code"] = -1;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            //r["item"] = filePath;
            return r;
        }
        /// <summary>
        /// 查询工资薪资
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetTaxSalaryList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (d.Keys.Contains("Is_Page") && d["Is_Page"] != null && d["Is_Page"].ToString() != "" && d["Is_Page"].ToString() == "true")
                {
                    //int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                    //int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                    //DataTable dt = db.getTaxSalaryList(d);
                    DataSet ds = db.createSalaryTemp(d);
                    DataTable dt = ds.Tables[0];//真实数据dt
                    DataTable du = ds.Tables[1];//真实条数dt
                    //r["total"] = dt.Rows.Count;
                    //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["items"] = dt;
                    r["total"] = du.Rows[0][0];
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    DataTable dt = db.getTaxSalaryList(d);
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(dt);
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public Dictionary<string, object> GetExportTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getTaxSalaryList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(dt);
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        
        public Dictionary<string, object> getSumPreCalculateTax(Dictionary<string, object> d)
            {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                    DataTable dt = db.getSumPreCalculateTax(d);
                    //r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(dt);
                    r["code"] = 2000;
                    r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                //r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public Dictionary<string, object> getPreCalculateTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (d.Keys.Contains("Is_Page") && d["Is_Page"] != null && d["Is_Page"].ToString() != "" && d["Is_Page"].ToString() == "true")
                {
                    int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                    int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                    DataTable dt = db.getPreCalculateTax(d);
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    DataTable dt = db.getPreCalculateTax(d);
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(dt);
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public Dictionary<string, object> getMonthTaxSalaryDetail(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            int total = 0;
            try
            {
                if (d.Keys.Contains("Is_Page") && d["Is_Page"] != null && d["Is_Page"].ToString() != "" && d["Is_Page"].ToString() == "true")
                {

                    int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                    int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                    DataTable dt = db.getMonthTaxSalaryDetail(d,out  total);
                    //r["total"] = dt.Rows.Count;
                    //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["total"] = total;
                    r["items"] = dt;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else {
                    DataTable dt = db.getMonthTaxSalaryDetail(d, out total);
                    //r["total"] = dt.Rows.Count;
                    //r["items"] = KVTool.TableToListDic(dt);
                    r["total"] = total;
                    r["items"] = dt;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }


        
         public Dictionary<string, object> getGroupMonthTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                    DataTable dt = db.getGroupMonthTaxSalary(d);
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(dt);
                    r["code"] = 2000;
                    r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public Dictionary<string, object> getGroupSumMonthTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getGroupSumMonthTaxSalary(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(dt);
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        
        public Dictionary<string, object> getMonthTaxSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            int total = 0;
            try
            {
                if (d.Keys.Contains("Is_Page") && d["Is_Page"] != null && d["Is_Page"].ToString() != "" && d["Is_Page"].ToString() == "true")
                {
                    int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                    int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                    //    DataTable dt = db.getMonthTaxSalary(d);
                    //    r["total"] = dt.Rows.Count;
                    //    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    //    r["code"] = 2000;
                    //    r["message"] = "查询成功";
                    //}
                    //else
                    //{
                    //    DataTable dt = db.getMonthTaxSalary(d);
                    //    r["total"] = dt.Rows.Count;
                    //    r["items"] = KVTool.TableToListDic(dt);
                    //    r["code"] = 2000;
                    //    r["message"] = "查询成功";
                    //}
                    DataTable dt = db.getMonthTaxSalary(d, out total);
                    //r["total"] = dt.Rows.Count;
                    //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["total"] = total;
                    r["items"] = dt;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    DataTable dt = db.getMonthTaxSalary(d, out total);
                    //r["total"] = dt.Rows.Count;
                    //r["items"] = KVTool.TableToListDic(dt);
                    r["total"] = total;
                    r["items"] = dt;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 验证导入工资
        /// </summary>
        /// <param name="filePath">导入文件路径</param>
        /// <param name="importOrgCode">导入部门编码</param>
        /// <param name="dateMonth">导入月份</param>
        /// <returns></returns>
        public Dictionary<string, object> ValidateTaxSalary(string filePath, string importOrgCode,string importOrgName, DateTime dateMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            string validatePerson = "";//验证人员变动
            string validateOrg = "";//检验导入的部门是否配置了是否算税及合法性
            string importModel = "";//导入模板名称
            string workcodereplace = "";//工号重复
            string hardCodeMsg = "";
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxSalary1> list1 = new List<ImportTaxSalary1>();//模板一数据集合
            List<ImportTaxSalary2> list2 = new List<ImportTaxSalary2>();//模板二数据集合
            //IEnumerable<IGrouping<string, ImportTaxSalary1>> lst1 = null;//针对模板一数据进行分组
            //IEnumerable<IGrouping<string, ImportTaxSalary2>> lst2 = null;//针对模板二数据进行分组
            DataTable taxOrgdt = null;//部门配置表结果集（包含是否算税、机关部门等信息）
            try
            {
                Init(filePath);//初始化数据
                #region 根据导入部门编号获取配置模板
                if (!string.IsNullOrEmpty(importOrgCode))
                {
                    taxOrgdt = taxOrgdb.getTaxOrgInfo(importOrgCode);
                    if (taxOrgdt != null && taxOrgdt.Rows.Count > 0)
                    {
                        importModel = taxOrgdt.Rows[0]["ImportModel"].ToString();//获取导入模板类型
                    }
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = "系统错误，导入部门编号异常！";
                    return r;
                }
                #endregion
                string msg = validateTemp(importModel, HeaderRow);//检验模板是否匹配
                if (msg == "")
                {

                    if (importModel == "样表一")
                    {
                        rows = ExcelConverter.Convert<ImportTaxSalary1>(Sheet, HeaderRow, 1);
                        foreach (var item in rows)
                        {
                            list1.Add(HardCode1(item));//将excel数据转换为List对象
                            hardCodeMsg = "第" + (list1.Count() + 1) + "条数据硬编码错误";
                        }
                        hardCodeMsg = "";
                        //lst1 = list1.GroupBy(x => x.S_OrgName);
                        //员工编号验重
                        var workcode = list1.GroupBy(x => x.S_WorkerCode).Where(x => x.Count() > 1).ToList();
                        foreach (var workitem in workcode)
                        {
                            workcodereplace += "【" + workitem.Key + "】";
                        }
                        if (!string.IsNullOrEmpty(workcodereplace))
                        {
                            workcodereplace = "员工编号：" + workcodereplace + "工资数据重复，请确认导入信息";
                            r["code"] = -1;
                            r["message"] = "导入失败！" + workcodereplace;
                            return r;
                        }
                        //List<string> orglst1 = lst1.Select(x => x.Key).ToList();
                        //List<ImportInfo1> temp1 = lst1.Select(x => new ImportInfo1 { S_OrgName = x.Key, List = x.ToList() }).ToList();

                        //#region 验证是否允许导入
                        //foreach (var org in orglst1)
                        //{
                        //    DataRow[] taxRows = taxOrgdt.Select("ORG_NAME = '" + org + "' and IsComputeTax=0 ");
                        //    if (taxRows == null || taxRows.Length == 0)
                        //    {
                        //        validateOrg += "【" + org + "】";
                        //    }
                        //    else if (taxRows.Length != 1)
                        //    {
                        //        validateOrg = "存在相同名称的组织机构";
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(validateOrg))
                        //{
                        //    validateOrg = "导入失败，导入的数据中" + validateOrg + "，组织机构信息不正确或否算税配置项有误！";
                        //    r["code"] = -1;
                        //    r["message"] = validateOrg;
                        //    return r;
                        //}
                        //#endregion

                        #region 验证人员变动
                        //validatePerson = validatePersonChange(temp1, taxOrgdt, dateMonth, importModel);
                        
                            validatePerson = validatePersonChange(list1, importOrgCode, importOrgName, dateMonth, importModel);
                        if (!string.IsNullOrEmpty(validatePerson))
                        {
                            r["code"] = 2001;
                            r["message"] = validatePerson;
                            r["item"] = filePath;
                            return r;
                        }
                        #endregion
                        if (string.IsNullOrEmpty(validatePerson) && string.IsNullOrEmpty(validateOrg))
                        {
                            r["code"] = 2000;
                            r["message"] = "验证通过";
                            r["item"] = filePath;
                        }

                    }
                    else if (importModel == "样表二")
                    {
                        rows = ExcelConverter.Convert<ImportTaxSalary2>(Sheet, HeaderRow, 1);
                        foreach (var item in rows)
                        {
                            list2.Add(HardCode2(item));
                            hardCodeMsg = "第" + list1.Count() + 1 + "条数据硬编码错误";
                        }
                        hardCodeMsg = "";
                        //员工编号验重
                        var workcode = list2.GroupBy(x => x.S_WorkerCode).Where(x => x.Count() > 1).ToList();
                        foreach (var workitem in workcode)
                        {
                            workcodereplace += "【" + workitem.Key + "】";
                        }
                        if (!string.IsNullOrEmpty(workcodereplace))
                        {
                            workcodereplace = "员工编号：" + workcodereplace + "工资数据重复，请确认导入信息";
                            r["code"] = -1;
                            r["message"] = "导入失败！" + workcodereplace;
                            return r;
                        }
                        //lst2 = list2.GroupBy(x => x.S_OrgName);
                        //List<string> orglst2 = lst2.Select(x => x.Key).ToList();
                        //List<ImportInfo2> temp2 = lst2.Select(x => new ImportInfo2 { S_OrgName = x.Key, List = x.ToList() }).ToList();
                        //#region 验证是否允许导入
                        //foreach (var org in orglst2)
                        //{
                        //    DataRow[] taxRows = taxOrgdt.Select("ORG_NAME = '" + org + "' and IsComputeTax=0 ");
                        //    if (taxRows == null || taxRows.Length == 0)
                        //    {
                        //        validateOrg += "【" + org + "】";
                        //    }
                        //    else if (taxRows.Length != 1)
                        //    {
                        //        validateOrg = "存在相同名称的组织机构";
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(validateOrg))
                        //{
                        //    validateOrg = "导入失败，导入的数据中" + validateOrg + "，组织机构信息不正确或否算税配置项有误！";
                        //    r["code"] = -1;
                        //    r["message"] = validateOrg;
                        //    return r;
                        //}
                        //#endregion

                        #region 验证人员变动
                        //validatePerson = validatePersonChange(temp2, taxOrgdt, dateMonth, importModel);
                        validatePerson = validatePersonChange(list2, importOrgCode, importOrgName, dateMonth, importModel);
                        if (!string.IsNullOrEmpty(validatePerson))
                        {
                            r["code"] = 2001;
                            r["message"] = validatePerson;
                            r["item"] = filePath;
                            return r;
                        }
                        #endregion
                        if (string.IsNullOrEmpty(validatePerson) && string.IsNullOrEmpty(validateOrg))
                        {
                            r["code"] = 2000;
                            r["message"] = "验证通过";
                            r["item"] = filePath;
                        }
                    }
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = msg;
                    return r;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message + hardCodeMsg;
            }
            r["item"] = filePath;
            return r;
        }

        public Dictionary<string, object> ImportTaxSalary(string filePath, string importOrgCode,string importOrgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            string importModel = "";//导入模板名称
            DataTable taxOrgdt = null;//部门配置表结果集（包含是否算税、机关部门等信息）
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxSalary1> list1 = new List<ImportTaxSalary1>();//模板一数据集合
            List<ImportTaxSalary2> list2 = new List<ImportTaxSalary2>();//模板二数据集合
            //IEnumerable<IGrouping<string, ImportTaxSalary1>> lst1 = null;//针对模板一数据进行分组
            //IEnumerable<IGrouping<string, ImportTaxSalary2>> lst2 = null;//针对模板二数据进行分组
            try
            {
                Init(filePath);//初始化数据
                if (!string.IsNullOrEmpty(importOrgCode))
                {
                    taxOrgdt = taxOrgdb.getTaxOrgInfo(importOrgCode);
                    if (taxOrgdt != null && taxOrgdt.Rows.Count > 0)
                    {
                        importModel = taxOrgdt.Rows[0]["ImportModel"].ToString();//获取导入模板类型
                    }
                }

                if (importModel == "样表一")
                {
                    rows = ExcelConverter.Convert<ImportTaxSalary1>(Sheet, HeaderRow, 1);
                    foreach (var item in rows)
                    {
                        list1.Add(HardCode1(item));//将excel数据转换为List对象                  
                    }

                    //lst1 = list1.GroupBy(x => x.S_OrgName);
                    //List<ImportInfo1> temp1 = lst1.Select(x => new ImportInfo1 { S_OrgName = x.Key, List = x.ToList() }).ToList();
                    //foreach (var item in temp1)
                    //{
                    //    DataRow[] taxRows = taxOrgdt.Select("ORG_NAME = '" + item.S_OrgName + "' and IsComputeTax=1 ");
                    //    if (taxRows.Length == 1)
                    //    {
                    //        item.S_OrgCode = taxRows[0]["ORG_CODE"].ToString();
                    //    }
                    //}
                        StringBuilder sb = new StringBuilder();//验证返回结果
                        string addPersonsql = "";//新增人员 
                        string insertPersonsql = "";//调入人员 
                        string reducePersonsql = "";//减少人员
                        string personChangesql = "";//人员变动情况sql
                        string inserttaxpayer = "";//新增人员信息
                        string updatetaxpayer = "";//修改人员信息
                        DataTable dt = db.getTaxSalaryPersonByOrgMonth(importOrgCode, dateMonth, importModel);
                        if (dt != null)
                        {
                            List<ImportTaxSalary1> uplst = KVTool.TableToList<ImportTaxSalary1>(dt);
                            List<string> impstrlst = list1.Select(o => o.S_WorkerCode).ToList();
                            List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();

                        ////工号不一致补丁----------------------------------------------------------------------------
                        //List<int> listInt = impstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                        //impstrlst = listInt.ConvertAll<string>(x => x.ToString());
                        ////--------------------------------------------------------------------------------------------
                        //List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                        ////工号不一致补丁----------------------------------------------------------------------------
                        //List<int> uplistInt = upstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                        //upstrlst = uplistInt.ConvertAll<string>(x => x.ToString());
                        //--------------------------------------------------------------------------------------------



                        List<string> listadd = new List<string>();
                            List<string> listreduce = new List<string>();
                            if (uplst != null && uplst.Count > 0)
                            {
                                listadd = impstrlst.Except(upstrlst).ToList();//增加
                                int addNum = listadd.Count;
                                if (addNum > 0)
                                {
                                    foreach (var additem in listadd)
                                    {
                                        DataTable dtTaxPlayer = taxPlayer.getTaxPlayerInfoByWorkerNumber(additem.Trim());
                                        if (dtTaxPlayer != null && dtTaxPlayer.Rows.Count > 0)
                                        {
                                        addPersonsql = @"insert into tax_taxpayerrecord (S_Id,S_OrgCode,S_CreateDate,S_CreateBy,WorkerName,WorkerCode,ImportMonth,WorkerStatus) values ("
                                        + "'" + Guid.NewGuid().ToString() + "',"
                                        + "'" + importOrgCode + "',"
                                        + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                         + "'" + userId + "',"
                                         + "'" + dtTaxPlayer.Rows[0]["WorkerName"].ToString() + "',"
                                          + "'" + additem + "',"
                                          + "'" + dateMonth.ToString("yyyy-MM-dd") + "',1)";
                                        sb.Append(addPersonsql);
                                        updatetaxpayer = @"update tax_taxpayerinfo with (rowlock,UpdLock) set S_OrgName='" + importOrgName+ "',S_OrgCode='"+ importOrgCode+ "',S_UpdateDate='"+dateMonth+ "',S_CreateBy='" + userId + "' where WorkerNumber='" + additem + "'";
                                        sb.Append(updatetaxpayer);


                                    }
                                        else
                                        {
                                            ImportTaxSalary1 objWorker = list1.Where(p => p.S_WorkerCode == additem).FirstOrDefault();
                                        insertPersonsql = @"insert into tax_taxpayerrecord (S_Id,S_OrgCode,S_CreateDate,S_CreateBy,WorkerName,WorkerCode,ImportMonth,WorkerStatus) values ("
                                    + "'" + Guid.NewGuid().ToString() + "',"
                                    + "'" + importOrgCode + "',"
                                    + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                     + "'" + userId + "',"
                                     + "'" + objWorker.S_WorkerName.ToString() + "',"
                                      + "'" + additem + "',"
                                      + "'" + dateMonth.ToString("yyyy-MM-dd") + "',2)";
                                        sb.Append(insertPersonsql);
                                        inserttaxpayer = "insert into tax_taxpayerinfo(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,WorkerNumber,WorkerName) values("
                                      + "'" + Guid.NewGuid().ToString() + "',"
                                      + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                       + "'" + userId + "',"
                                       + "'" + importOrgName + "',"
                                        + "'" + importOrgCode + "',"
                                        + "'" + objWorker.S_WorkerCode.ToString() + "',"
                                        + "'" + objWorker.S_WorkerName.ToString() + "')";
                                          sb.Append(inserttaxpayer);
                                    }
                                    }
                                }
                                listreduce = upstrlst.Except(impstrlst).ToList();
                                int reduceNum = listreduce.Count;
                                if (reduceNum > 0)
                                {
                                    foreach (var item in listreduce)
                                    {
                                        ImportTaxSalary1 objreduceWorker = uplst.Where(p => p.S_WorkerCode == item).FirstOrDefault();
                                    reducePersonsql = @"insert into tax_taxpayerrecord (S_Id,S_OrgCode,S_CreateDate,S_CreateBy,WorkerName,WorkerCode,ImportMonth,WorkerStatus) values ("
                                 + "'" + Guid.NewGuid().ToString() + "',"
                                 + "'" + importOrgCode + "',"
                                 + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                  + "'" + userId + "',"
                                  + "'" + objreduceWorker.S_WorkerName.ToString() + "',"
                                   + "'" + item + "',"
                                   + "'" + dateMonth.ToString("yyyy-MM-dd") + "',0)";
                                    sb.Append(reducePersonsql);
                                    //updatetaxpayer = @"update tax_taxpayerinfo set S_OrgName=null,S_OrgCode=null  where WorkerNumber='" + item + "'";
                                    //sb.Append(updatetaxpayer);
                                }
                                }
                            }
                        }
                    personChangesql=sb.ToString();
                    r["message"] = db.createTaxSalary1(list1, dateMonth, userId, importOrgCode, importOrgName, personChangesql);
                    if (!string.IsNullOrEmpty(r["message"].ToString()))
                    {
                        r["code"] = -1;
                    }
                    else
                    {
                        r["code"] = 2000;
                    }
                    //return r;
                }
                else if (importModel == "样表二")
                {
                    rows = ExcelConverter.Convert<ImportTaxSalary2>(Sheet, HeaderRow, 1);
                    foreach (var item in rows)
                    {
                        list2.Add(HardCode2(item));//将excel数据转换为List对象
                    }
                    //lst2 = list2.GroupBy(x => x.S_OrgName);
                    //List<ImportInfo2> temp2 = lst2.Select(x => new ImportInfo2 { S_OrgName = x.Key, List = x.ToList() }).ToList();
                    //foreach (var item in temp2)
                    //{
                    //    DataRow[] taxRows = taxOrgdt.Select("ORG_NAME like '" + item.S_OrgName + "%' and IsComputeTax=1 ");
                    //    if (taxRows.Length == 1)
                    //    {
                    //        item.S_OrgCode = taxRows[0]["ORG_CODE"].ToString();
                    //    }
                    //}


                    StringBuilder sb = new StringBuilder();//验证返回结果
                    string addPersonsql = "";//新增人员 
                    string insertPersonsql = "";//调入人员 
                    string reducePersonsql = "";//减少人员
                    string personChangesql = "";//人员变动情况说明
                    string inserttaxpayer = "";//新增人员信息
                    string updatetaxpayer = "";//修改人员信息
                    DataTable dt = db.getTaxSalaryPersonByOrgMonth(importOrgCode, dateMonth, importModel);
                    if (dt != null)
                    {
                        List<ImportTaxSalary2> uplst = KVTool.TableToList<ImportTaxSalary2>(dt);
                        List<string> impstrlst = list2.Select(o => o.S_WorkerCode).ToList();
                        List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                        ////工号不一致补丁----------------------------------------------------------------------------
                        //List<int> listInt = impstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                        //impstrlst = listInt.ConvertAll<string>(x => x.ToString());
                        ////--------------------------------------------------------------------------------------------
                        //List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                        ////工号不一致补丁----------------------------------------------------------------------------
                        //List<int> uplistInt = upstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                        //upstrlst = uplistInt.ConvertAll<string>(x => x.ToString());
                        ////--------------------------------------------------------------------------------------------
                        List<string> listadd = new List<string>();
                        List<string> listreduce = new List<string>();
                        if (uplst != null && uplst.Count > 0)
                        {
                            listadd = impstrlst.Except(upstrlst).ToList();//增加
                            int addNum = listadd.Count;
                            if (addNum > 0)
                            {
                                foreach (var additem in listadd)
                                {
                                    DataTable dtTaxPlayer = taxPlayer.getTaxPlayerInfoByWorkerNumber(additem.Trim());
                                    if (dtTaxPlayer != null && dtTaxPlayer.Rows.Count > 0)
                                    {
                                        addPersonsql = @"insert into tax_taxpayerrecord (S_Id,S_OrgCode,S_CreateDate,S_CreateBy,WorkerName,WorkerCode,ImportMonth,WorkerStatus) values ("
                                                                              + "'" + Guid.NewGuid().ToString() + "',"
                                                                              + "'" + importOrgCode + "',"
                                                                              + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                                                               + "'" + userId + "',"
                                                                               + "'" + dtTaxPlayer.Rows[0]["WorkerName"].ToString() + "',"
                                                                                + "'" + additem + "',"
                                                                                + "'" + dateMonth.ToString("yyyy-MM-dd") + "',1)";
                                        sb.Append(addPersonsql);
                                        updatetaxpayer = @"update tax_taxpayerinfo with (rowlock,UpdLock) set S_OrgName='" + importOrgName + "',S_OrgCode='" + importOrgCode + "',S_UpdateDate='" + dateMonth + "',S_CreateBy='" + userId + "' where WorkerNumber='" + additem + "'";
                                        sb.Append(updatetaxpayer);
                                    }
                                    else
                                    {
                                        ImportTaxSalary2 objWorker = list2.Where(p => p.S_WorkerCode == additem).FirstOrDefault();
                                        insertPersonsql = @"insert into tax_taxpayerrecord (S_Id,S_OrgCode,S_CreateDate,S_CreateBy,WorkerName,WorkerCode,ImportMonth,WorkerStatus) values ("
                                    + "'" + Guid.NewGuid().ToString() + "',"
                                    + "'" + importOrgCode + "',"
                                    + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                     + "'" + userId + "',"
                                     + "'" + objWorker.S_WorkerName.ToString() + "',"
                                      + "'" + additem + "',"
                                      + "'" + dateMonth.ToString("yyyy-MM-dd") + "',2)";
                                        sb.Append(insertPersonsql);
                                        inserttaxpayer = "insert into tax_taxpayerinfo(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,WorkerNumber,WorkerName) values("
+ "'" + Guid.NewGuid().ToString() + "',"
+ "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
+ "'" + userId + "',"
+ "'" + importOrgName + "',"
+ "'" + importOrgCode + "',"
+ "'" + objWorker.S_WorkerCode.ToString() + "',"
+ "'" + objWorker.S_WorkerName.ToString() + "')";
                                        sb.Append(inserttaxpayer);
                                    }
                                }
                            }
                            listreduce = upstrlst.Except(impstrlst).ToList();
                            int reduceNum = listreduce.Count;
                            if (reduceNum > 0)
                            {
                                foreach (var item in listreduce)
                                {
                                    ImportTaxSalary2 objreduceWorker = uplst.Where(p => p.S_WorkerCode == item).FirstOrDefault();
                                    reducePersonsql = @"insert into tax_taxpayerrecord (S_Id,S_OrgCode,S_CreateDate,S_CreateBy,WorkerName,WorkerCode,ImportMonth,WorkerStatus) values ("
                                 + "'" + Guid.NewGuid().ToString() + "',"
                                 + "'" + importOrgCode + "',"
                                 + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',"
                                  + "'" + userId + "',"
                                  + "'" + objreduceWorker.S_WorkerName.ToString() + "',"
                                   + "'" + item + "',"
                                   + "'" + dateMonth.ToString("yyyy-MM-dd") + "',0)";
                                    sb.Append(reducePersonsql);
                                    //updatetaxpayer = @"update tax_taxpayerinfo set S_OrgName=null,S_OrgCode=null  where WorkerNumber='" + item + "'";
                                    //sb.Append(updatetaxpayer);
                                }
                            }
                        }

                    }

                    personChangesql = sb.ToString();

                    r["message"] = db.createTaxSalary2(list2, dateMonth, userId,  importOrgCode, importOrgName, personChangesql);
                    if (!string.IsNullOrEmpty(r["message"].ToString()))
                    {
                        r["code"] = -1;
                        r["message"] = "导入失败！";
                    }
                    else
                    {
                        r["code"] = 2000;
                        r["message"] = "导入成功！";
                    }
                    //return r;
                }

            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
                return r;
            }
            return r;
        }

        /// <summary>
        /// 删除工资信息
        /// </summary>
        /// <returns></returns>
        public string deleteTaxSalary(DateTime dateMonth, string orgCode)
        {
            return db.delTaxSalary(dateMonth, orgCode);
        }
        #endregion

        #region 硬编码方式转换实体对象
        /// <summary>
        /// 针对模板一硬编码
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static ImportTaxSalary1 HardCode1(ExcelDataRow row)
        {
            var t = new ImportTaxSalary1();
            //Type type = t.GetType();
            //PropertyInfo[] propertyInfos = type.GetProperties();
            //foreach (PropertyInfo prop in propertyInfos)
            //{
            //    if (prop.PropertyType==typeof(string))
            //    {
            //        prop.SetValue()
            //    }
            //}
            t.S_WorkerCode = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerCode").ColValue.Replace(" ", "");
            t.S_WorkerName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerName").ColValue.Replace(" ", "");
            t.S_OrgName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_OrgName").ColValue.Replace(" ", "");
            t.G_GWJGZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_GWJGZ").ColValue));
            t.G_BLGZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BLGZ").ColValue));
            t.G_GLJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_GLJT").ColValue));
            t.G_SGJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_SGJT").ColValue));
            t.G_JSJNJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JSJNJT").ColValue));
            t.G_ZFBT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_ZFBT").ColValue));
            t.G_BLJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BLJT").ColValue));
            t.G_BYKT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BYKT").ColValue));
            t.G_QTJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_QTJT").ColValue));
            t.G_YBJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_YBJT").ColValue));
            t.G_JBJDGZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JBJDGZ").ColValue));
            t.G_JCYJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JCYJ").ColValue));
            t.G_YJJJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_YJJJ").ColValue));
            t.G_DSZNBJFS = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_DSZNBJFS").ColValue));
            t.G_WCBTSF = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_WCBTSF").ColValue));
            t.G_BFK = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BFK").ColValue));
            t.T_YFHJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_YFHJ").ColValue));
            t.K_YiLiaoBX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_YiLiaoBX").ColValue));
            t.K_SYBX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_SYBX").ColValue));
            t.K_YangLaoBX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_YangLaoBX").ColValue));
            t.K_ZFGJJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_ZFGJJ").ColValue));
            t.K_QYNJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_QYNJ").ColValue));
            t.K_QTKX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_QTKX").ColValue));
            t.T_YSHJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_YSHJ").ColValue));
            t.K_KS = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_KS").ColValue));
            t.T_SFHJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_SFHJ").ColValue));
            t.Adjust1 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust1")?.ColValue));
            t.Adjust2 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust2")?.ColValue));
            t.Adjust3 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust3")?.ColValue));
            t.Adjust4 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust4")?.ColValue));
            t.Adjust5 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust5")?.ColValue));
            t.Adjust6 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust6")?.ColValue));
            t.Adjust7 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust7")?.ColValue));
            t.Adjust8 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust8")?.ColValue));
            return t;


        }

        public static string isnull(string value)
        {
            if (value == "")
            {
                return "0";
            }
            return value.Replace(" ", "");
        }
        /// <summary>
        /// 针对模板二硬编码
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static ImportTaxSalary2 HardCode2(ExcelDataRow row)
        {
            var t = new ImportTaxSalary2();
            t.S_WorkerCode = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerCode").ColValue.Replace(" ", "");
            t.S_WorkerName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerName").ColValue.Replace(" ", "");
            t.S_OrgName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_OrgName").ColValue.Replace(" ", "");
            t.G_GWJGZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_GWJGZ").ColValue));
            t.G_GWJGZB = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_GWJGZB").ColValue));
            t.G_BLGZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BLGZ").ColValue));
            t.G_GLJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_GLJT").ColValue));
            t.G_SGJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_SGJT").ColValue));
            t.G_JSJNJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JSJNJT").ColValue));
            t.G_ZFBT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_ZFBT").ColValue));
            t.G_BLJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BLJT").ColValue));
            t.G_BYKT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BYKT").ColValue));
            t.G_ZFJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_ZFJT").ColValue));
            t.G_HMJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_HMJT").ColValue));
            t.G_JSJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JSJT").ColValue));
            t.G_XFGZJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_XFGZJT").ColValue));
            t.G_FLGDZJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_FLGDZJT").ColValue));
            t.G_BZRJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BZRJT").ColValue));
            t.G_SYYLJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_SYYLJT").ColValue));
            t.G_YBJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_YBJT").ColValue));
            t.G_XQJB = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_XQJB").ColValue));
            t.G_PSJB = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_PSJB").ColValue));
            t.G_JRJB = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JRJB").ColValue));
            t.G_JCYJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JCYJ").ColValue));
            t.G_YJJJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_YJJJ").ColValue));
            t.G_ZENBFBK = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_ZENBFBK").ColValue));
            t.G_ZXJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_ZXJ").ColValue));
            t.G_C01 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_C01").ColValue));
            t.G_C02 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_C02").ColValue));
            t.G_C03 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_C03").ColValue));
            t.G_C04 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_C04").ColValue));
            t.T_XJ1 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_XJ1").ColValue));
            t.G_DSZNJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_DSZNJT").ColValue));
            t.G_BJSPJT = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_BJSPJT").ColValue));
            t.G_FSJWF = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_FSJWF").ColValue));
            t.G_WCBZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_WCBZ").ColValue));
            t.G_TXBZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_TXBZ").ColValue));
            t.G_JTBZ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_JTBZ").ColValue));
            t.G_HLHJYJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_HLHJYJ").ColValue));
            t.G_C05 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_C05").ColValue));
            t.G_ZEWBFBK = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_ZEWBFBK").ColValue));
            t.G_LYF = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "G_LYF").ColValue));
            t.T_XJ2 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_XJ2").ColValue));
            t.T_YFHJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_YFHJ").ColValue));
            t.K_YiLiaoBX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_YiLiaoBX").ColValue));
            t.K_SYBX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_SYBX").ColValue));
            t.K_YangLaoBX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_YangLaoBX").ColValue));
            t.K_ZFGJJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_ZFGJJ").ColValue));
            t.K_QYNJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_QYNJ").ColValue));
            t.K_QTKX = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_QTKX").ColValue));
            t.T_YSHJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_YSHJ").ColValue));
            t.K_KS = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "K_KS").ColValue));
            t.T_SFHJ = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "T_SFHJ").ColValue));
            t.Adjust1 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust1").ColValue));
            t.Adjust2 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust2").ColValue));
            t.Adjust3 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust3").ColValue));
            t.Adjust4 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust4").ColValue));
            t.Adjust5 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust5").ColValue));
            t.Adjust6 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust6").ColValue));
            t.Adjust7 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust7").ColValue));
            t.Adjust8 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust8").ColValue));
            return t;
        }
        #endregion

        #region Excel导入操作

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="fileUrl"></param>
        private static void Init(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                throw new ArgumentNullException("fileUrl");
            }

            if (!File.Exists(fileUrl))
            {
                throw new FileNotFoundException();
            }

            string ext = Path.GetExtension(fileUrl).ToLower().Trim();
            if (ext != ".xls" && ext != ".xlsx")
            {
                throw new NotSupportedException("非法文件");
            }

            try
            {
                //文件流：读取
                using (FileStream fs = File.Open(fileUrl, FileMode.Open))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    if (ext == ".xls")
                    {
                        Workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        Workbook = WorkbookFactory.Create(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Sheet = Workbook.GetSheetAt(0);

            SetDictHeader();
            //Workbook.Close();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 设置表头字典，列索引：列名
        /// </summary>
        /// <returns></returns>
        private static void SetDictHeader()
        {
            HeaderRow = new ExcelHeaderRow();
            IRow row = Sheet.GetRow(0);
            ICell cell;
            for (int i = 0; i < row.PhysicalNumberOfCells; i++)
            {
                cell = row.GetCell(i);
                HeaderRow.Cells.Add(
                    new ExcelCol()
                    {
                        ColIndex = i,
                        ColName = cell.GetStringValue()
                    });
            }
        }
        #endregion

        #region 验证导入模板是否正确
        /// <summary>
        /// 验证导入模板匹配
        /// </summary>
        /// <param name="tempType">模板类型</param>
        /// <param name="headerRow">导入的列头</param>
        /// <returns></returns>
        public string validateTemp(string tempType, ExcelHeaderRow headerRow)
        {
            string msg = "";
            if (tempType == "样表一")
            {
                foreach (var headItem in headerRow.Cells)
                {
                    switch (headItem.ColName.Trim())
                    {
                        case "员工编号":
                        case "姓名":
                        case "部门":
                        case "岗位（技）工资":
                        case "保留工资":
                        case "工龄津贴":
                        case "上岗津贴":
                        case "技术(技能)津贴":
                        case "住房补贴":
                        case "保留津贴":
                        case "边远矿贴":
                        case "其它津贴":
                        case "夜班津贴":
                        case "加班、加点工资":
                        case "基础月奖":
                        case "业绩奖金":
                        case "独生子女(保健防暑）":
                        case "误餐补贴(三费)":
                        case "补发（扣）":
                        case "应发合计":
                        case "养老保险":
                        case "失业保险":
                        case "医疗保险":
                        case "住房公积金":
                        case "企业年金":
                        case "其他扣项":
                        case "应税合计":
                        case "扣税":
                        case "实发合计":
                        case "调增项1":
                        case "调增项2":
                        case "调增项3":
                        case "调增项4":
                        case "调减项1":
                        case "调减项2":
                        case "调减项3":
                        case "调减项4":
                            break;
                        default:
                            msg += "【" + headItem.ColName + "】";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = "导入失败！" + msg + "列名不正确，请确认导入模板样式！";
                }
            }
            else if (tempType == "样表二")
            {
                foreach (var headItem in headerRow.Cells)
                {
                    switch (headItem.ColName.Trim())
                    {
                        case "员工编号":
                        case "姓名":
                        case "部门":
                        case "岗位（技）工资":
                        case "岗位（技）工资（补）":
                        case "保留工资":
                        case "工龄津贴":
                        case "上岗津贴":
                        case "技术(技能)津贴":
                        case "住房补贴":
                        case "保留津贴":
                        case "边远矿贴":
                        case "政府津贴":
                        case "回民津贴":
                        case "驾驶津贴":
                        case "信访工作津贴":
                        case "法轮功斗争津贴":
                        case "班主任津贴":
                        case "医疗卫生津贴":
                        case "夜班津贴":
                        case "星期加班":
                        case "平时加班":
                        case "节日加班":
                        case "基础月奖":
                        case "业绩奖金":
                        case "总额内补发（补扣）":
                        case "专项奖":
                        case "C01专项奖":
                        case "C02专项奖":
                        case "C03专项奖":
                        case "C04专项奖":
                        case "小计1":
                        case "独生子女补贴":
                        case "保健食品津贴":
                        case "防暑降温费":
                        case "误餐补助":
                        case "通讯补助":
                        case "交通津贴":
                        case "合理化建议奖":
                        case "C05专项奖":
                        case "总额外补发（补扣）":
                        case "疗养费":
                        case "小计2":
                        case "小计3":
                        case "养老保险":
                        case "失业保险":
                        case "医疗保险":
                        case "住房公积金":
                        case "企业年金":
                        case "其他扣项":
                        case "应税合计":
                        case "扣税":
                        case "实发合计":
                        case "调增项1":
                        case "调增项2":
                        case "调增项3":
                        case "调增项4":
                        case "调减项1":
                        case "调减项2":
                        case "调减项3":
                        case "调减项4":
                            break;
                        default:
                            msg += "【" + headItem.ColName + "】";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = "导入失败！" + msg + "列名不正确，请确认导入模板样式！";
                }
            }
            else
            {
                msg = "导入失败！模板类型配置有误，请联系系统管理员！";
            }
            return msg;
        }
        #endregion

        #region 验证人员变动

        #region 验证模板一中人员变动情况

        //public string validatePersonChange(List<ImportInfo1> temp1, DataTable taxOrgdt, DateTime dateMonth, string importModel)
        //{

        //    StringBuilder sb = new StringBuilder();//验证返回结果
        //    foreach (var item1 in temp1)
        //    {
        //        string addPerson = "";//新增人员 
        //        string insertPerson = "";//调入人员 
        //        string reducePerson = "";//减少人员
        //        string validatePerson = "";//人员变动情况说明
        //        int addnum = 0;//新增人数
        //        int insertnum = 0;//调入人数
        //        DataRow[] importRows = taxOrgdt.Select("ORG_NAME ='" + item1.S_OrgName + "' and IsComputeTax=1 ");
        //        if (importRows.Length == 1)
        //        {
        //            string orgCode = importRows[0]["ORG_CODE"].ToString();
        //            DataTable dt = db.getTaxSalaryPersonByOrgMonth(orgCode, dateMonth, importModel);
        //            if (dt != null)
        //            {
        //                List<ImportTaxSalary1> uplst = KVTool.TableToList<ImportTaxSalary1>(dt);
        //                List<ImportTaxSalary1> implst = item1.List;
        //                List<string> impstrlst = item1.List.Select(o => o.S_WorkerCode).ToList();
        //                List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
        //                List<string> listadd= new List<string>();
        //                List<string> listreduce = new List<string>();
        //                //List<ImportTaxSalary1> listadd = new List<ImportTaxSalary1>();
        //                //List<ImportTaxSalary1> listreduce = new List<ImportTaxSalary1>();
        //                if (uplst != null && uplst.Count > 0)
        //                {
        //                    listadd = impstrlst.Except(upstrlst).ToList();//增加
        //                    //xxx2 = ccc1.Except(rrr1).ToList();//减少
        //                    //listadd = implst.Except(uplst).ToList();
        //                    int addNum = listadd.Count;
        //                    if (addNum > 0)
        //                    {
        //                        foreach (var additem in listadd)
        //                        {
        //                            DataTable dtTaxPlayer = taxPlayer.getTaxPlayerInfoByWorkerNumber(additem);
        //                            if (dtTaxPlayer != null && dtTaxPlayer.Rows.Count > 0)
        //                            {
        //                                addnum++;
        //                                addPerson += "【" + additem + "：" + dtTaxPlayer.Rows[0]["S_WorkerName"].ToString() + "】";
        //                            }
        //                            else
        //                            {
        //                                insertnum++;
        //                                //ImportTaxSalary1 objWorker= (ImportTaxSalary1)implst.Select(p => p.S_WorkerCode == additem);
        //                                ImportTaxSalary1 objWorker = implst.Where(p => p.S_WorkerCode == additem).FirstOrDefault();
        //                                insertPerson += "【" + additem +"："+ objWorker.S_WorkerName + "】";
        //                            }
        //                        }
        //                    }
        //                    //listreduce = uplst.Except(implst).ToList();
        //                    listreduce = upstrlst.Except(impstrlst).ToList();
        //                    int reduceNum = listreduce.Count;
        //                    if (reduceNum > 0)
        //                    {
        //                        foreach (var item in listreduce)
        //                        {

        //                            ImportTaxSalary1 objreduceWorker = uplst.Where(p => p.S_WorkerCode == item).FirstOrDefault();
        //                            //ImportTaxSalary1 objWorker = (ImportTaxSalary1)implst.Select(p => p.S_WorkerCode == item);
        //                            reducePerson += "【" + item + "：" + objreduceWorker.S_WorkerName + "】";
        //                        }
        //                        reducePerson = " 调出" + reduceNum + "人：" + reducePerson;
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            validatePerson = item1.S_OrgName + "导入数据中组织机构数据异常！";
        //            return validatePerson;
        //        }
        //        if (!string.IsNullOrEmpty(addPerson))
        //        {
        //            addPerson = " 新增" + addnum + "人：" + addPerson;
        //            validatePerson += addPerson;
        //        }
        //        if (!string.IsNullOrEmpty(insertPerson))
        //        {
        //            insertPerson = " 调入" + insertnum + "人：" + insertPerson;
        //            validatePerson += insertPerson;
        //        }
        //        if (!string.IsNullOrEmpty(reducePerson))
        //        {
        //            validatePerson += reducePerson;
        //        }
        //        if (!string.IsNullOrEmpty(validatePerson))
        //        {
        //            validatePerson = "\n"+ item1.S_OrgName + dateMonth.ToString("yyyy-MM") + validatePerson + "\n";
        //            sb.Append(validatePerson);
        //        }
        //    }
        //    return sb.ToString();
        //}
        public string validatePersonChange(List<ImportTaxSalary1> implst, string orgCode, string orgName, DateTime dateMonth, string importModel)
        {
            StringBuilder sb = new StringBuilder();//验证返回结果
            string addPerson = "";//新增人员 
            string insertPerson = "";//调入人员 
            string reducePerson = "";//减少人员
            string validatePerson = "";//人员变动情况说明
            int addnum = 0;//新增人数
            int insertnum = 0;//调入人数
            DataTable dt = db.getTaxSalaryPersonByOrgMonth(orgCode, dateMonth, importModel);
            if (dt != null)
            {
                List<ImportTaxSalary1> uplst = KVTool.TableToList<ImportTaxSalary1>(dt);
                List<string> impstrlst = implst.Select(o => o.S_WorkerCode).ToList();
                List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                ////工号不一致补丁----------------------------------------------------------------------------
                //List<int> listInt = impstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                //impstrlst = listInt.ConvertAll<string>(x => x.ToString());
                ////--------------------------------------------------------------------------------------------
                //List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                ////工号不一致补丁----------------------------------------------------------------------------
                //List<int> uplistInt = upstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                //upstrlst = uplistInt.ConvertAll<string>(x => x.ToString());
                ////--------------------------------------------------------------------------------------------
                List<string> listadd = new List<string>();
                List<string> listreduce = new List<string>();
                if (uplst != null && uplst.Count > 0)
                {
                    listadd = impstrlst.Except(upstrlst).ToList();//增加
                    int addNum = listadd.Count;
                    if (addNum > 0)
                    {
                        foreach (var additem in listadd)
                        {
                            DataTable dtTaxPlayer = taxPlayer.getTaxPlayerInfoByWorkerNumber(additem);
                            if (dtTaxPlayer != null && dtTaxPlayer.Rows.Count > 0)
                            {
                                addnum++;
                                addPerson += "【" + additem + "：" + (dtTaxPlayer.Rows[0]["WorkerName"]==null?"": dtTaxPlayer.Rows[0]["WorkerName"].ToString()) + "】";
                            }
                            else
                            {
                                insertnum++;
                                ImportTaxSalary1 objWorker = implst.Where(p => p.S_WorkerCode == additem).FirstOrDefault();
                                insertPerson += "【" + additem + "：" + objWorker.S_WorkerName + "】";
                            }
                        }
                    }
                    listreduce = upstrlst.Except(impstrlst).ToList();
                    int reduceNum = listreduce.Count;
                    if (reduceNum > 0)
                    {
                        foreach (var item in listreduce)
                        {

                            ImportTaxSalary1 objreduceWorker = uplst.Where(p => p.S_WorkerCode == item).FirstOrDefault();
                            if (objreduceWorker != null)
                            {
                                reducePerson += "【" + item + "：" + objreduceWorker.S_WorkerName + "】";
                            }
                            else {
                                throw new Exception("工号：【"+ item + "】上月工资信息不存在！");
                            }
                        }
                        reducePerson = " 调出" + reduceNum + "人：" + reducePerson;
                    }
                }

            }
            if (!string.IsNullOrEmpty(addPerson))
            {
                addPerson = " 调入" + addnum + "人：" + addPerson;
                validatePerson += addPerson;
            }
            if (!string.IsNullOrEmpty(insertPerson))
            {
                insertPerson = " 新增" + insertnum + "人：" + insertPerson;
                validatePerson += insertPerson;
            }
            if (!string.IsNullOrEmpty(reducePerson))
            {
                validatePerson += reducePerson;
            }
            if (!string.IsNullOrEmpty(validatePerson))
            {
                validatePerson = "\n" + orgName + dateMonth.ToString("yyyy-MM") + validatePerson + "\n";
                sb.Append(validatePerson);
            }
            return sb.ToString();
        }

        #endregion
        #region 验证模板二中人员变动情况

        //public string validatePersonChange(List<ImportInfo2> temp1, DataTable taxOrgdt, DateTime dateMonth, string importModel)
        //{
        //    string addPerson = "";//新增人员 
        //    string insertPerson = "";//调入人员 
        //    string reducePerson = "";//减少人员
        //    StringBuilder sb = new StringBuilder();//验证返回结果
        //    foreach (var item1 in temp1)
        //    {
        //        string validatePerson = "";//人员变动情况说明
        //        int addnum = 0;//新增人数
        //        int insertnum = 0;//调入人数
        //        DataRow[] importRows = taxOrgdt.Select("ORG_NAME ='" + item1.S_OrgName + "' and IsComputeTax=1 ");
        //        if (importRows.Length == 1)
        //        {
        //            DataTable dt = db.getTaxSalaryPersonByOrgMonth(importRows[0]["ORG_CODE"].ToString(), dateMonth, importModel);
        //            if (dt != null)
        //            {
        //                List<ImportTaxSalary2> uplst = KVTool.TableToList<ImportTaxSalary2>(dt);
        //                List<ImportTaxSalary2> implst = item1.List;
        //                List<ImportTaxSalary2> listadd = new List<ImportTaxSalary2>();
        //                List<ImportTaxSalary2> listreduce = new List<ImportTaxSalary2>();
        //                if (uplst != null && uplst.Count > 0)
        //                {
        //                    listadd = implst.Except(uplst).ToList();
        //                    int addNum = listadd.Count;
        //                    if (addNum > 0)
        //                    {

        //                        foreach (var additem in listadd)
        //                        {
        //                            DataTable dtTaxPlayer = taxPlayer.getTaxPlayerInfoByWorkerNumber(additem.S_WorkerCode);
        //                            if (dtTaxPlayer != null && dtTaxPlayer.Rows.Count > 0)
        //                            {
        //                                addnum++;
        //                                addPerson += "【" + additem.S_WorkerName + "】";
        //                            }
        //                            else
        //                            {
        //                                insertnum++;
        //                                insertPerson += "【" + additem.S_WorkerName + "】";
        //                            }
        //                        }
        //                    }
        //                    listreduce = uplst.Except(implst).ToList();
        //                    int reduceNum = listreduce.Count;
        //                    if (reduceNum > 0)
        //                    {
        //                        foreach (var item in listadd)
        //                        {
        //                            reducePerson += "【" + item.S_WorkerName + "】";
        //                        }
        //                        reducePerson = " 调出" + reduceNum + "人：" + reducePerson;
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            validatePerson = item1.S_OrgName + "导入数据中组织机构数据异常！";
        //            return validatePerson;
        //        }
        //        if (!string.IsNullOrEmpty(addPerson))
        //        {
        //            addPerson = " 新增" + addnum + "人：" + addPerson;
        //            validatePerson += addPerson;
        //        }
        //        else if (!string.IsNullOrEmpty(insertPerson))
        //        {
        //            insertPerson = " 调入" + insertnum + "人：" + insertPerson;
        //            validatePerson += insertPerson;
        //        }
        //        else if (!string.IsNullOrEmpty(reducePerson))
        //        {
        //            validatePerson += reducePerson;
        //        }
        //        if (!string.IsNullOrEmpty(validatePerson))
        //        {
        //            validatePerson = item1.S_OrgName + dateMonth.ToString("yyyy-MM") + validatePerson + "/n";
        //            sb.Append(validatePerson);
        //        }
        //    }
        //    return sb.ToString();
        //}

        public string validatePersonChange(List<ImportTaxSalary2> implst, string orgCode, string orgName, DateTime dateMonth, string importModel)
        {
            StringBuilder sb = new StringBuilder();//验证返回结果
            string addPerson = "";//新增人员 
            string insertPerson = "";//调入人员 
            string reducePerson = "";//减少人员
            string validatePerson = "";//人员变动情况说明
            int addnum = 0;//新增人数
            int insertnum = 0;//调入人数
            DataTable dt = db.getTaxSalaryPersonByOrgMonth(orgCode, dateMonth, importModel);
            if (dt != null)
            {
                List<ImportTaxSalary2> uplst = KVTool.TableToList<ImportTaxSalary2>(dt);
                List<string> impstrlst = implst.Select(o => o.S_WorkerCode).ToList();
                List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                ////工号不一致补丁----------------------------------------------------------------------------
                //List<int> listInt =impstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                //impstrlst = listInt.ConvertAll<string>(x => x.ToString());
                ////--------------------------------------------------------------------------------------------
                //List<string> upstrlst = uplst.Select(o => o.S_WorkerCode).ToList();
                ////工号不一致补丁----------------------------------------------------------------------------
                //List<int> uplistInt = upstrlst.Select<string, int>(x => Convert.ToInt32(x)).ToList();
                //upstrlst = uplistInt.ConvertAll<string>(x => x.ToString());
                ////--------------------------------------------------------------------------------------------
                List<string> listadd = new List<string>();
                List<string> listreduce = new List<string>();
                if (uplst != null && uplst.Count > 0)
                {
                    listadd = impstrlst.Except(upstrlst).ToList();//增加
                    int addNum = listadd.Count;
                    if (addNum > 0)
                    {
                        foreach (var additem in listadd)
                        {
                            DataTable dtTaxPlayer = taxPlayer.getTaxPlayerInfoByWorkerNumber(additem);
                            if (dtTaxPlayer != null && dtTaxPlayer.Rows.Count > 0)
                            {
                                addnum++;
                                addPerson += "【" + additem + "：" + (dtTaxPlayer.Rows[0]["WorkerName"] == null ? "" : dtTaxPlayer.Rows[0]["WorkerName"].ToString()) + "】";
                            }
                            else
                            {
                                insertnum++;
                                ImportTaxSalary2 objWorker = implst.Where(p => p.S_WorkerCode == additem).FirstOrDefault();
                                insertPerson += "【" + additem + "：" + objWorker.S_WorkerName + "】";
                            }
                        }
                    }
                    listreduce = upstrlst.Except(impstrlst).ToList();
                    int reduceNum = listreduce.Count;
                    if (reduceNum > 0)
                    {
                        foreach (var item in listreduce)
                        {

                            ImportTaxSalary2 objreduceWorker = uplst.Where(p => p.S_WorkerCode == item).FirstOrDefault();
                            //reducePerson += "【" + item + "：" + objreduceWorker.S_WorkerName + "】";
                            if (objreduceWorker != null)
                            {
                                reducePerson += "【" + item + "：" + objreduceWorker.S_WorkerName + "】";
                            }
                            else
                            {
                                throw new Exception("工号：【" + item + "】上月工资信息不存在！");
                            }
                        }
                        reducePerson = " 调出" + reduceNum + "人：" + reducePerson;
                    }
                }

            }
            if (!string.IsNullOrEmpty(addPerson))
            {
                addPerson = " 调入" + addnum + "人：" + addPerson;
                validatePerson += addPerson;
            }
            if (!string.IsNullOrEmpty(insertPerson))
            {
                insertPerson = " 新增" + insertnum + "人：" + insertPerson;
                validatePerson += insertPerson;
            }
            if (!string.IsNullOrEmpty(reducePerson))
            {
                validatePerson += reducePerson;
            }
            if (!string.IsNullOrEmpty(validatePerson))
            {
                validatePerson = "\n" + orgName + dateMonth.ToString("yyyy-MM") + validatePerson + "\n";
                sb.Append(validatePerson);
            }
            return sb.ToString();
        }

        #endregion
        #endregion

        #region 验证上报状态
        public class ValidateCondition
        {
            public string S_OrgCode { get; set; }
            public int ReportStatus { get; set; }
            public int IsComputeTax { get; set; }
        }
        public Dictionary<string, object> validateReportStatus(string orgCode, DateTime dateMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = taxReportStatus.getReportStatus(orgCode, dateMonth);
            r["items"] = KVTool.TableToList<ValidateCondition>(dt);
            return r;
        }
        #endregion
    }
}
