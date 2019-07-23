using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UIDP.ODS;
using UIDP.UTILITY;
namespace UIDP.BIZModule
{
    public class TaxPlayerInfoModule
    {
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion
        TaxPlayerInfoDB db = new TaxPlayerInfoDB();
        OrgDB Orgdb = new OrgDB();//组织机构对照表
        ExportDB exportdb = new ExportDB();
        /// <summary>
        /// 查询纳税人信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getPlayerInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.getTaxPlayerInfo(d);
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = dt.Rows.Count;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 新建纳税人信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createTaxPlayerInfo(Dictionary<string, object> d)
        {
            d["S_Id"] = Guid.NewGuid();
            d["S_CreateDate"] = DateTime.Now;
            d["NationalityId"] = 0;
            if (d["WorkerNumber"] != null && d["WorkerNumber"].ToString() != "")
            {
                DataTable dt = db.getTaxPlayerInfoByWorkerNumber(d["WorkerNumber"].ToString());
                if (dt.Rows.Count > 0)
                {
                    return "此用户已经存在";
                }
            }
            if (d["IdNumber"] != null && d["IdNumber"].ToString() != "")
            {
                DataTable dt = db.getTaxPlayInfoByCitizenId(d["IdNumber"].ToString());
                if (dt.Rows.Count > 0)
                {
                    return "此用户已经存在";
                }
            }
            return db.createTaxPlayerInfo(d);
        }
        /// <summary>
        /// 删除纳税人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string deleteTaxPlayerInfo(string id)
        {
            return db.deletePlayerInfo(id);
        }

        public string editTaxPlayerInfo(Dictionary<string, object> d)
        {
            //d["S_UpdateBy"] = "管理员";
            d["S_UpdateDate"] = DateTime.Now;
            return db.editPlayerInfo(d);
        }

        /// <summary>
        /// 获取纳税人信息中所有下拉选项
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> getAllOptions()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.getIdType();//获取证件类型
            DataTable dy = db.getEducation();//获取学历//
            DataTable du = db.getNationally();//获取国籍
            DataTable di = db.getJob();//获取职业
            DataTable dp = db.getWorkPost();//获取职位
            DataTable dz = db.getOtherIdType();//获取其他证照类型
            DataTable dc = db.getJobType();//获取任职受雇类型
            try
            {
                if (dt != null)
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["idtype"] = dt;
                    r["education"] = dy;
                    r["nationally"] = du;
                    r["job"] = di;
                    r["workpost"] = dp;
                    r["jobtype"] = dc;
                    r["otheridtype"] = dz;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    //r["items"] = new Dictionary<string, object>();
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        /// <summary>
        /// 获取纳税人变动数据
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getTaxChange(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = int.Parse(d["limit"].ToString());
                int page = int.Parse(d["page"].ToString());
                DataTable dt = db.getTaxPlayerChange(d);
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                }
                else
                {
                    r["total"] = 0;
                    r["message"] = "成功";
                    r["code"] = 2000;
                    //r["items"] = new Dictionary<string, object>();
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> getTaxContent(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = int.Parse(d["limit"].ToString());
                int page = int.Parse(d["page"].ToString());
                DataTable dt = db.getTaxContent(d);
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                }
                else
                {
                    r["total"] = 0;
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> getTaxpayer(string OrgCode, string systime, string name, string IDNumber, string WorkerNumber, string limit, string page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            int lim = int.Parse(limit);
            int pa = int.Parse(page);
            try
            {
                DataTable dt = db.getTaxpayer(OrgCode, systime, name, IDNumber, WorkerNumber);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, pa, lim));
                    r["code"] = 2000;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = dt.Rows.Count;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> getPayerSalary(string systime, string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getPayerSalary(systime, id);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = dt.Rows.Count;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        #region 导入纳税人信息

        /// <summary>
        /// 验证导入纳税人
        /// </summary>
        /// <param name="filePath">导入文件路径</param>
        /// <param name="importOrgCode">导入部门编码</param>
        /// <param name="dateMonth">导入月份</param>
        /// <returns></returns>
        public Dictionary<string, object> ValidateTaxPayerInfo(string filePath)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            string validateDataMsg = "";//检验数据合法性
            string workcodereplace = "";//工号重复
            int errorNum = 0; //错误行数
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxPayerInfo> list1 = new List<ImportTaxPayerInfo>();//模板一数据集合
            IEnumerable<IGrouping<string, ImportTaxPayerInfo>> lst1 = null;//针对模板一数据进行分组
            DataTable Orgdt = null;//部门信息表
            try
            {
                Init(filePath);//初始化数据
                Orgdt = Orgdb.syncOrgList();
                string msg = validateTemp(HeaderRow);//检验模板是否匹配
                if (msg == "")
                {
                    rows = ExcelConverter.Convert<ImportTaxPayerInfo>(Sheet, HeaderRow, 1);
                    if (rows.Count==0)
                    {
                        r["code"] = -1;
                        r["message"] = "空数据无法导入！";
                        return r;
                    }
                    foreach (var item in rows)
                    {
                        list1.Add(HardCode(item));//将excel数据转换为List对象
                        errorNum = list1.Count;                    
                    }
                    lst1 = list1.GroupBy(x => x.S_OrgName);
                     List<string> orglst1 = lst1.Select(x => x.Key).ToList();
                    #region 员工编号验重
                    var workcode = list1.GroupBy(x => x.WorkerNumber).Where(x => x.Count() > 1).ToList();
                    foreach (var workitem in workcode)
                    {
                        workcodereplace += "【" + workitem.Key + "】";
                    }
                    if (!string.IsNullOrEmpty(workcodereplace))
                    {
                        workcodereplace = "员工编号：" + workcodereplace + "人员信息重复，请确认导入信息";
                        r["code"] = -1;
                        r["message"] = "导入失败！" + workcodereplace;
                        return r;
                    }
                    //DataTable TaxpayerList = db.getPayerInfoList();
                    //foreach (var item in list1)
                    //{
                    //    DataRow[] dr=TaxpayerList.Select("WorkerNumber='" + item.WorkerNumber + "'");
                    //    if (dr.Length >= 1)
                    //    {
                    //        workcodereplace += item.WorkerNumber;
                    //    }
                    //} 
                    //if (!string.IsNullOrEmpty(workcodereplace))
                    //{
                    //    workcodereplace = "员工编号" + workcodereplace + "与系统内已经存在的用户重复";
                    //    r["code"] = -1;
                    //    r["message"] = "导入失败" + workcodereplace;
                    //    return r;
                    //}

                        #endregion
                        #region 验证员工身份证号是否正确
                        //Regex reg = new Regex(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$");
                        //foreach(var item in list1)
                        //{
                        //    if (item.IdType.Contains("身份证"))
                        //    {
                        //        if (!reg.IsMatch(item.IdNumber.ToString()))
                        //        {
                        //            IdCorrect += "员工编号" + item.WorkerNumber + "员工姓名" + item.WorkerName + "身份证格式不正确";
                        //        }
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(IdCorrect))
                        //{
                        //    r["code"] = -1;
                        //    r["message"] = IdCorrect;
                        //    return r;
                        //}
                        #endregion
                        #region 验证导入数据合法性
                        validateDataMsg = validateData(list1, Orgdt, orglst1);
                    if (!string.IsNullOrEmpty(validateDataMsg))
                    {
                        r["code"] = -1;
                        r["message"] = validateDataMsg;
                        r["item"] = filePath;
                        return r;
                    }
                    #endregion
                    
                    else
                    {
                        r["code"] = 2000;
                        r["message"] = "验证通过";
                        r["item"] = filePath;
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
                r["message"] = ex.Message+"错误行数"+(errorNum+1).ToString();
            }
            r["item"] = filePath;
            return r;
        }

        #endregion
        #region 导入纳税人信息
        public Dictionary<string, object> ImportTaxPayerInfo(string filePath, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxPayerInfo> list1 = new List<ImportTaxPayerInfo>();//数据集合
            try
            {
                Init(filePath);//初始化数据
                rows = ExcelConverter.Convert<ImportTaxPayerInfo>(Sheet, HeaderRow, 1);
                foreach (var item in rows)
                {
                    list1.Add(HardCode(item));//将excel数据转换为List对象
                }
                r["message"] = db.createTaxPayerInfo(list1, userId);
                if (!string.IsNullOrEmpty(r["message"].ToString()))
                {
                    r["code"] = -1;
                }
                else
                {
                    r["code"] = 2000;
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


        #endregion


        #region 硬编码方式转换实体对象
        /// <summary>
        /// 针对纳税人信息硬编码
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static ImportTaxPayerInfo HardCode(ExcelDataRow row)
        {
            var t = new ImportTaxPayerInfo();
            t.S_OrgName= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "S_OrgName").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.WorkerNumber= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerNumber").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.WorkerName=row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerName").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
            t.IdType= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IdType").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.IdNumber= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IdNumber").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Nationality= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Nationality").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Sex= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Sex").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.WorkerStatus= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerStatus").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.BirthDate= isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "BirthDate").ColValue.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            t.JobType= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "JobType").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Tel= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Tel").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.EmployeeDate= isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "EmployeeDate").ColValue.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            t.QuitDate= isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "QuitDate").ColValue.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            t.IsDisability= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsDisability").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.IsLieShu= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsLieShu").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.IsAlone= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsAlone").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.DisabilityNo= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "DisabilityNo").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.LiShuZH= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "LiShuZH").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Investment= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Investment").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.PerInvestment =iszero(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "PerInvestment").ColValue));
            t.Remark= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Remark").ColValue.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            t.IsAbroad = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsAbroad").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.BroadName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BroadName").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.BirthPlace = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BirthPlace").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.FirstEntryTime = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "FirstEntryTime").ColValue.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            t.EstimatedDepartureTime = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "EstimatedDepartureTime").ColValue.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            t.OtherIdType= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "OtherIdType").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.OtherIdNumber= isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "OtherIdNumber").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Province = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Province").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.City = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "City").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.County = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "County").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Domicile = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Domicile").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Adress_Province = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adress_Province").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Adress_City = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adress_City").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Adress_County = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adress_County").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.PostalAddress = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "PostalAddress").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.L_Province = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "L_Province").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.L_City = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "L_City").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.L_County = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "L_County").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.L_Adress = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "L_Adress").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Email = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Email").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.Education = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Education").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.BankName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BankName").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.BankNumber = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BankNumber").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.WorkPost = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkPost").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            t.L_Adress = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "L_Adress").ColValue.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            #region 以下是老模板硬编码数据类型转化方法
            //t.WorkerNumber = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerNumber").ColValue);
            //t.WorkerName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerName").ColValue);
            //t.S_OrgName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "S_OrgName").ColValue);
            //t.IdType = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IdType").ColValue);
            ////t.IdTypeCode = dictionaryvalue(t.IdType, "ZJLX");
            //t.IdNumber = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IdNumber").ColValue);
            //t.Nationality = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Nationality").ColValue);
            ////t.NationalityId = dictionaryvalue(t.Nationality, "GJ");
            //t.Sex = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Sex").ColValue);
            //t.BirthDate = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "BirthDate").ColValue);
            //t.WorkerStatus = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerStatus").ColValue);
            //t.IsEmployee = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsEmployee").ColValue);
            //t.Tel = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Tel").ColValue);
            //t.IsDisability = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsDisability").ColValue);
            //t.DisabilityNo = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "DisabilityNo").ColValue);
            //t.IsLieShu = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsLieShu").ColValue);
            //t.LiShuZH = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "LiShuZH").ColValue);
            //t.IsAlone = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsAlone").ColValue);
            //t.EmployeeDate = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "EmployeeDate").ColValue);
            //t.Education = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Education").ColValue);
            ////t.EducationCode = dictionaryvalue(t.Education, "XL");
            //t.Occupation = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Occupation").ColValue);

            ////t.OccupationCode = dictionaryvalue(t.Occupation, "ZY");
            //t.WorkPost = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkPost").ColValue);
            ////t.WorkPostCode = dictionaryvalue(t.WorkPost, "ZW");
            //t.Email = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Email").ColValue);
            //t.BankName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BankName").ColValue);
            //t.BankNumber = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BankNumber").ColValue);
            //t.IsSpecialIndustry = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsSpecialIndustry").ColValue);
            //t.IsWorking = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsWorking").ColValue);
            //t.QuitDate = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "QuitDate").ColValue);
            //t.IsShareholder = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsShareholder").ColValue);
            //t.Investment = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Investment").ColValue);
            //t.Province = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Province").ColValue);
            //t.City = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "City")?.ColValue);
            //t.County = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "County")?.ColValue);
            //t.Domicile = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Domicile")?.ColValue);
            //t.Adress_Province = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adress_Province")?.ColValue);
            //t.Adress_City = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adress_City")?.ColValue);
            //t.Adress_County = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adress_County")?.ColValue);
            //t.PostalAddress = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "PostalAddress")?.ColValue);
            //t.Remark = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Remark")?.ColValue);
            //t.IsAbroad = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsAbroad")?.ColValue);
            //t.BroadName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BroadName")?.ColValue);
            //t.IsLive = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IsLive")?.ColValue);
            //t.BirthPlace = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "BirthPlace")?.ColValue);
            //t.FirstEntryTime = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "FirstEntryTime")?.ColValue);
            //t.ThisYearEntryTime = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "ThisYearEntryTime")?.ColValue);
            //t.EstimatedDepartureTime = isdate(row.DataCols.SingleOrDefault(c => c.PropertyName == "EstimatedDepartureTime")?.ColValue);
            //t.S_Province = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "S_Province")?.ColValue);
            //t.S_City = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "S_City")?.ColValue);
            //t.S_County = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "S_County")?.ColValue);
            //t.S_Address = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "S_Address")?.ColValue);
            //t.PayPlace = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "PayPlace")?.ColValue);
            //t.OtherPayPlace = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "OtherPayPlace")?.ColValue);
            //t.ChinaPost = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "ChinaPost")?.ColValue);
            //t.UnChinaPost = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "UnChinaPost")?.ColValue);
            //t.OfficeTime = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "OfficeTime")?.ColValue);
            //t.TaxpayersNumber = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "TaxpayersNumber")?.ColValue);
            #endregion
            return t;
        }
        public static string dictionaryvalue(string name,string type)
        {
            return TaxPlayerInfoDB.getDictionaryCode(name, type);
        }
        public static string isdate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return Convert.ToDateTime(value).ToString("yyyy-MM-dd hh:mm:ss");
        }
        public static decimal iszero(string value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }
            public static string isnull(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else if (value.Contains("是")|| value.Contains("男")|| value.Contains("正常"))
            {
                return "1";
            }
            else if (value.Contains("否") || value.Contains("女") || value.Contains("非正常"))
            {
                return "0";
            }
            return value.Replace(" ", "");
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
        public string validateTemp(ExcelHeaderRow headerRow)
        {
            string msg = "";
            foreach (var headItem in headerRow.Cells)
            {
                switch (headItem.ColName.Trim())
                {
                    case "单位":
                    case "工号":
                    case "*姓名":
                    case "*证照类型":
                    case "*证照号码":
                    case "*国籍(地区)":
                    case "*性别":
                    case "*人员状态":
                    case "*出生日期":
                    case "*任职受雇从业类型":
                    case "手机号码":
                    case "任职受雇从业日期":
                    case "离职日期":
                    case "是否残疾":
                    case "是否烈属":
                    case "是否孤老":
                    case "残疾证号":
                    case "烈属证号":
                    case "个人投资额":
                    case "个人投资比例(%)":
                    case "备注":
                    case "是否境外人员":
                    case "中文名":
                    case "出生国家(地区)":
                    case "首次入境时间":
                    case "预计离境时间":
                    case "其他证照类型":
                    case "其他证照号码":
                    case "户籍所在地（省）":
                    case "户籍所在地（市）":
                    case "户籍所在地（区县）":
                    case "户籍所在地（详细地址）":
                    case "居住地址（省）":
                    case "居住地址（市）":
                    case "居住地址（区县）":
                    case "居住地址（详细地址）":
                    case "联系地址（省）":
                    case "联系地址（市）":
                    case "联系地址（区县）":
                    case "联系地址（详细地址）":
                    case "电子邮箱":
                    case "学历":
                    case "开户银行":
                    case "银行账号":
                    case "职务":
                        #region 老模板表头验证
                        //case "工号":
                        //case "姓名":
                        //case "单位":
                        //case "证照类型":
                        //case "证照号码":
                        //case "国籍(地区)":
                        //case "性别":
                        //case "出生日期":
                        //case "人员状态":
                        //case "是否雇员":
                        //case "手机号码":
                        //case "是否残疾":
                        //case "残疾证号":
                        //case "是否烈属":
                        //case "烈属证号":
                        //case "是否孤老":
                        //case "任职受雇日期":
                        //case "学历":
                        //case "职业":
                        //case "职务":
                        //case "电子邮箱":
                        //case "开户银行":
                        //case "银行账号":
                        //case "是否特定行业":
                        //case "是否在职":
                        //case "离职时间":
                        //case "是否股东、投资者":
                        //case "个人股本（投资）额":
                        //case "户籍所在地（省)":
                        //case "户籍所在地（市）":
                        //case "户籍所在地（区县）":
                        //case "户籍所在地（详细地址）":
                        //case "居住地址（省）":
                        //case "居住地址（市）":
                        //case "居住地址（区县）":
                        //case "居住地址（详细地址）":
                        //case "备注":
                        //case "是否境外人员":
                        //case "姓名（中文）":
                        //case "境内有无住所":
                        //case "出生地":
                        //case "首次入境时间":
                        //case "本年入境时间":
                        //case "预计离境时间":
                        //case "联系地址（省）":
                        //case "联系地址（市）":
                        //case "联系地址（区县）":
                        //case "联系地址（详细地址）":
                        //case "支付地":
                        //case "境外支付地":
                        //case "境内职务":
                        //case "境外职务":
                        //case "任职期限":
                        //case "境外纳税人识别号":
                        #endregion
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
            return msg;
        }
        #endregion

        #region 验证导入数据合法性

        public string validateData(List<ImportTaxPayerInfo> lst, DataTable orgdt, List<string> orglst1)
        {
            string errorMsg = "";
            string orgValidate = "";//单位名称验证
            string idValidate = "";//身份证号验证
            string workcodeEmpty = "";//工号非空验证
            string worknameEmpty = "";//姓名非空验证
            string idtypeEmpty = "";//身份证类型非空
            string idnumberEmpty = "";//身份证号非空
            foreach (var item in orglst1)
            {
                //DataRow[] taxRows = orgdt.Select("ORG_NAME = '" + item + "' or ORG_SHORT_NAME='" + item + "'");
                DataRow[] taxRows = orgdt.Select("ORG_NAME = '" + item + "'");
                if (taxRows == null || taxRows.Length == 0)
                {
                    orgValidate += "【" + item + "】";
                }
            }
            foreach (var item in lst)
            {
                if (string.IsNullOrEmpty(item.IdType))
                {
                    idtypeEmpty += "【" + item.WorkerName + "】";
                }
                if (string.IsNullOrEmpty(item.IdNumber))
                {
                    idnumberEmpty += "【" + item.WorkerName + "】";
                }
                if (string.IsNullOrEmpty(item.WorkerNumber))
                {
                    workcodeEmpty += "【" + item.WorkerName + "】";
                }
                if (string.IsNullOrEmpty(item.WorkerName))
                {
                    worknameEmpty += "【" + item.WorkerName + "】";
                }
                

                if (item.IdType.Contains("身份证")&&!string.IsNullOrEmpty(item.IdNumber))
                {
                    //^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$
                    if (!Regex.IsMatch(item.IdNumber.Trim(), @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$"))
                    {
                        idValidate += "【" + item.WorkerName + "】";
                    }
                }
            }
            if (!string.IsNullOrEmpty(orgValidate))
            {
                orgValidate = orgValidate + "与系统中组织机构名称不匹配，请确认！";
                errorMsg = errorMsg + orgValidate;
                //return orgValidate;
            }
            if (!string.IsNullOrEmpty(workcodeEmpty))
            {
                workcodeEmpty = workcodeEmpty + "工号不能为空！";
                errorMsg = errorMsg + workcodeEmpty;
                //return workcodeEmpty;
            }
            if (!string.IsNullOrEmpty(idtypeEmpty))
            {
                idtypeEmpty = idtypeEmpty + "身份证类型不能为空！";
                errorMsg = errorMsg + idtypeEmpty;
                //return idtypeEmpty;
            }
            if (!string.IsNullOrEmpty(idnumberEmpty))
            {
                idnumberEmpty = idnumberEmpty + "身份证号不能为空！";
                errorMsg = errorMsg + idnumberEmpty;
                //return idnumberEmpty;
            }
            if (!string.IsNullOrEmpty(worknameEmpty))
            {
                worknameEmpty = worknameEmpty + "姓名不能为空！";
                errorMsg = errorMsg + worknameEmpty;
                //return worknameEmpty;
            }
            if (!string.IsNullOrEmpty(idValidate))
            {
                idValidate = idValidate + "身份证号不正确！";
                errorMsg = errorMsg + idValidate;
                //return idValidate;
            }
            return errorMsg;
        }

        #endregion

        public Dictionary<string,object> exportPayerInfo(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string fileName = "本部门纳税人导出表";
                DataTable payerInfo = exportdb.getExportPayerInfo(d);//获取本部门全部纳税人信息
                if (payerInfo.Rows.Count > 0)
                {
                    List<string> colName = new List<string>()
                    {
                        "WorkerNumber","WorkerName","idt","IdNumber","nat","Sex","BirthDate","WorkerStatusId","jobt","Tel","EmployeeDate","QuitDate","IsDisability","IsLieShu","IsAlone",
                        "DisabilityNo","LiShuZH","Investment","PerInvestment","Remark","IsAbroad","BroadName","BirthPlace","FirstEntryTime","EstimatedDepartureTime","otheridt","OtherIdNumber",
                        "Province","City","County","Domicile","Adress_Province","Adress_City","Adress_County","PostalAddress","L_Province","L_City","L_County","L_Adress","Email","edu","BankName",
                        "BankNumber","wor"
                    };
                    r["item"] = ExportByTemplet(payerInfo, "无标题", "无部门", fileName, 1,payerInfo.Rows.Count, colName);
                    r["code"] = 2000;
                    r["message"] = "成功";
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但是没有人员信息";
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> exportChangePayerInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string fileName = "本部门纳税人变动导出表";
                DataTable payerInfo = exportdb.getChangePayerInfo(d);//获取本部门全部纳税人信息
                if (payerInfo.Rows.Count > 0)
                {
                    List<string> colName = new List<string>()
                    {
                        "WorkerNumber","WorkerName","idt","IdNumber","nat","Sex","BirthDate","WorkerStatusId","jobt","Tel","EmployeeDate","QuitDate","IsDisability","IsLieShu","IsAlone",
                        "DisabilityNo","LiShuZH","Investment","PerInvestment","Remark","IsAbroad","BroadName","BirthPlace","FirstEntryTime","EstimatedDepartureTime","otheridt","OtherIdNumber",
                        "Province","City","County","Domicile","Adress_Province","Adress_City","Adress_County","PostalAddress","L_Province","L_City","L_County","L_Adress","Email","edu","BankName",
                        "BankNumber","wor"
                    };
                    r["item"] = ExportByTemplet(payerInfo, "无标题", "无部门", fileName, 1, payerInfo.Rows.Count, colName);
                    r["code"] = 2000;
                    r["message"] = "成功";
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但是没有人员信息";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        #region 导出工具方法 复制版本 纳税人导出全部依赖于本文件的方法，其他导出方法为公共方法
        public void GetDataTable(System.IO.Stream reader, string path, string modePath, ref string result, ref DataTable dt)
        {
            try
            {
                DataTable dtModel;
                string fileFullName = Path.GetFileName(path);
                string fileExt = fileFullName.Substring(fileFullName.IndexOf(".") + 1);
                if (fileExt != "xls" && fileExt != "xlsx")
                {
                    result = "请选择Excel文件类型导入!";
                    return;
                }
                else
                {
                    if (fileExt == "xls")
                    {
                        dt = RenderDataTableFromExcel(System.IO.File.OpenRead(path));
                    }

                    else
                    {
                        dt = RenderDataTableFromExcel2007(reader);
                    }
                    fileExt = modePath.Substring(modePath.LastIndexOf(".") + 1);

                    if (fileExt == "xls")
                    {
                        dtModel = RenderDataTableFromExcel(System.IO.File.OpenRead(modePath));
                    }
                    else
                    {
                        dtModel = RenderDataTableFormExcelHeader2007(modePath);
                    }

                    if (dt.Columns.Count != dtModel.Columns.Count)
                    {
                        result = "抱歉，您选择的导入文件不正确，当前系统检测到您的导入文件的列数与服务器提供的模板列数不相符！请您仔细检查当前导入文件是否正确！";
                        return;
                    }
                    else if (dt.Columns.Count > 0)
                    {
                        int columnNum = 0;
                        columnNum = dt.Columns.Count;
                        string[] strColumns = new string[columnNum];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dtModel.Columns[i].ColumnName != dt.Columns[i].ColumnName)
                            {
                                result = "抱歉，您选择的导入文件不正确，当前系统检测到您的导入文件中的列名：“" + dtModel.Columns[i].ColumnName + "”，与服务器提供的模板字段不相符！请您仔细检查当前导入文件是否正确！";
                                break;
                            }
                        }
                    }

                    //dtModel.Merge(dt, true, MissingSchemaAction.Ignore);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

        }

        private static void Init1(string fileUrl)
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
                FileStream fileStream = File.Open(fileUrl, FileMode.Open);//初始化文件流
                byte[] array = new byte[fileStream.Length];//初始化字节数组，用来暂存读取到的字节
                fileStream.Read(array, 0, array.Length);//读取流中数据，写入到字节数组中
                if (ext.Equals(".xls"))
                {
                    Workbook = new HSSFWorkbook(fileStream);
                }
                else
                {
                    Workbook = WorkbookFactory.Create(fileStream);
                }
                fileStream.Close(); //关闭流
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Sheet = Workbook.GetSheetAt(0);


        }


        public static DataTable RenderDataTableFromExcel(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);

                ISheet sheet = workbook.GetSheetAt(0);//取第一个表 

                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells 
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1 

                //handling header. 
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        break;

                    if (row != null)
                    {
                        if (row.GetCell(0) == null)
                        {
                            break;
                        }
                        if (row.GetCell(0).ToString().Trim() == "")
                        {
                            break;
                        }
                        DataRow dataRow = table.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        table.Rows.Add(dataRow);
                    }
                }
                workbook = null;
                sheet = null;
                return table;

            }

        }
        public static DataTable RenderDataTableFromExcel2007(Stream excelFileStream)
        {
            DataTable table = new DataTable();
            try
            {
                using (excelFileStream)
                {
                    IWorkbook workbook = new XSSFWorkbook(excelFileStream);

                    ISheet sheet = workbook.GetSheetAt(0);//取第一个表 

                    IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                    int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells 
                    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1 

                    //handling header. 
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        string columnname = headerRow.GetCell(i).StringCellValue;
                        if (columnname == "")
                            continue;
                        DataColumn column = new DataColumn(columnname);
                        table.Columns.Add(column);
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                            break;
                        if (row.FirstCellNum < 0)
                        {
                            continue;
                        }
                        else if (row.GetCell(row.FirstCellNum).ToString().Trim() == "")
                        {
                            continue;
                        }

                        DataRow dataRow = table.NewRow();

                        if (row != null)
                        {
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    { //空数据类型处理
                                        case CellType.Blank:
                                            dataRow[j] = "";
                                            break;
                                        case CellType.String:
                                            dataRow[j] = row.GetCell(j).StringCellValue;
                                            break;
                                        case CellType.Numeric: //数字类型  
                                            if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = row.GetCell(j).DateCellValue;
                                            }
                                            else
                                            {
                                                dataRow[j] = row.GetCell(j).NumericCellValue;
                                            }
                                            break;
                                        case CellType.Formula:
                                            dataRow[j] = row.GetCell(j).NumericCellValue;
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                        }

                        table.Rows.Add(dataRow);
                    }
                    workbook = null;
                    sheet = null;
                    return table;

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
        public static DataTable RenderDataTableFormExcelHeader2007(string filePath)
        {

            DataTable table = new DataTable();
            try
            {
                IWorkbook workbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(file);
                }

                ISheet sheet = workbook.GetSheetAt(0);//取第一个表 

                IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells 
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1 

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    string colName = headerRow.GetCell(i).StringCellValue;
                    if (colName == "")
                        continue;
                    DataColumn column = new DataColumn(colName);
                    table.Columns.Add(column);
                }

                for (int i = 1; i <= 1; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = "";
                        }
                    }

                    table.Rows.Add(dataRow);
                }

                workbook = null;
                sheet = null;
                return table;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 初始化导出模板
        /// </summary>
        /// <param name="fileUrl"></param>
        //private static void Init(string fileUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(fileUrl))
        //    {
        //        throw new ArgumentNullException("fileUrl");
        //    }

        //    if (!File.Exists(fileUrl))
        //    {
        //        throw new FileNotFoundException();
        //    }

        //    string ext = Path.GetExtension(fileUrl).ToLower().Trim();
        //    if (ext != ".xls" && ext != ".xlsx")
        //    {
        //        throw new NotSupportedException("非法文件");
        //    }

        //    try
        //    {
        //        //文件流：读取
        //        FileStream fileStream = File.Open(fileUrl, FileMode.Open);//初始化文件流
        //        byte[] array = new byte[fileStream.Length];//初始化字节数组，用来暂存读取到的字节
        //        fileStream.Read(array, 0, array.Length);//读取流中数据，写入到字节数组中
        //        if (ext.Equals(".xls"))
        //        {
        //            Workbook = new HSSFWorkbook(fileStream);
        //        }
        //        else
        //        {
        //            Workbook = WorkbookFactory.Create(fileStream);
        //        }
        //        fileStream.Close(); //关闭流
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    Sheet = Workbook.GetSheetAt(0);
        //}
        #region NPOI插入数据行
        public static void InsertRow(ISheet sheet, int insertRowIndex, int insertRowCount, IRow formatRow)
        {
            sheet.ShiftRows(insertRowIndex, sheet.LastRowNum + insertRowCount, insertRowCount, true, false);
            for (int i = insertRowIndex; i < insertRowIndex + insertRowCount; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;
                targetRow = sheet.CreateRow(i);
                //targetRow.HeightInPoints = 27;
                for (int m = formatRow.FirstCellNum; m < formatRow.LastCellNum; m++)
                {
                    sourceCell = formatRow.GetCell(m);
                    if (sourceCell == null)
                    {
                        continue;
                    }
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                }
            }

            for (int i = insertRowIndex; i < insertRowIndex + insertRowCount; i++)
            {
                IRow firstTargetRow = sheet.GetRow(i);
                ICell firstSourceCell = null;
                ICell firstTargetCell = null;

                for (int m = formatRow.FirstCellNum; m < formatRow.LastCellNum; m++)
                {
                    firstSourceCell = formatRow.GetCell(m, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    if (firstSourceCell == null)
                    {
                        continue;
                    }
                    firstTargetCell = firstTargetRow.GetCell(m, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    firstTargetCell.CellStyle = firstSourceCell.CellStyle;
                    firstTargetCell.SetCellType(firstSourceCell.CellType);
                }
            }



        }

        #endregion
        /// <summary>
        /// 根据模板导出数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="title">表头</param>
        /// <param name="tempName">模板名称</param>
        /// <param name="dateRowNum">数据行开始索引</param>
        /// <param name="columnnum">列数量</param>
        /// <param name="col">对应的列名集合</param>
        /// <returns></returns>
        public static string ExportByTemplet(DataTable dt, string title, string orgName, string tempName, int dateRowNum, int columnnum, List<string> col)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                string filePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\Templates\\" + "纳税人导出模板" + ".xls";//原始文件
                Init1(filePath);
                IRow mySourceRow = Sheet.GetRow(dateRowNum+1);
                InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = Sheet.CreateRow(1 + i);
                    for (int j = 0; j < col.Count; j++)
                    {
                        if(col[j] == "Investment")
                        {
                            if(dt.Rows[i][col[j]] == null || dt.Rows[i][col[j]].ToString() == "")
                            {
                                row.CreateCell(j).SetCellValue(0);
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                            }
                        }
                        else if (col[j] != "Investment"&&dt.Rows[i][col[j]] == null || dt.Rows[i][col[j]].ToString() == "")
                        {
                            row.CreateCell(j).SetCellValue("");
                        }
                        else
                        {
                            if (col[j].StartsWith("Is"))
                            {
                                row.CreateCell(j).SetCellValue(changeFlag(dt.Rows[i][col[j]].ToString()));
                            }
                            else if (col[j] == "WorkerStatusId")
                            {
                                row.CreateCell(j).SetCellValue(changeWork(dt.Rows[i][col[j]].ToString()));
                            }
                            else if (col[j].EndsWith("Date") || col[j].EndsWith("Time") && col[j] != "OfficeTime")
                            {
                                row.CreateCell(j).SetCellValue(Convert.ToDateTime(dt.Rows[i][col[j]].ToString()).ToString("yyyy-MM-dd"));
                            }
                            else if (col[j] == "Sex")
                            {
                                row.CreateCell(j).SetCellValue(changeSex(dt.Rows[i][col[j]].ToString()));
                            }
                            else if (col[j] == "PerInvestment")
                            {
                                row.CreateCell(j).SetCellValue(Convert.ToDouble(dt.Rows[i][col[j]].ToString()));
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                            }
                        }        
                    }
                }
                //结尾
                Sheet.ForceFormulaRecalculation = true;
                //导出文件  
                string fileName =tempName+".xls";
                string path = System.IO.Directory.GetCurrentDirectory() + "\\Files\\export\\" + fileName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                FileStream Reportfile = new FileStream(path, FileMode.Create);
                Workbook.Write(Reportfile);
                Reportfile.Close();
                return "\\Files\\export\\" + fileName;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static string changeFlag(string value) 
        {
            if (value == "0")
            {
                return "否";
            }
            else
            {
                return "是";
            }
        }

        public static string changeSex(string value)
        {
            if (value == "0")
            {
                return "女";
            }
            else
            {
                return "男";
            }
        }

        public static string changeWork(string value)
        {
            if ( value == "0")
            {
                return "非正常";
            }
            else
            {
                return "正常";
            }
        }

        #endregion
    }
}
