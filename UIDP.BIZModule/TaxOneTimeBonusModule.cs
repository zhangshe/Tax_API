using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UIDP.ODS;
using UIDP.UTILITY;
namespace UIDP.BIZModule
{
   public class TaxOneTimeBonusModule
    {
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion
        TaxOneTimeBonusDB db = new TaxOneTimeBonusDB();
        TaxSalaryDB taxSalarydb = new TaxSalaryDB();//工资表
        
            public Dictionary<string, object> getOneTimeBonusListImport(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.getOneTimeBonusListImport(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
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
        public Dictionary<string, object> getOneTimeBonusList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                //DataTable dt = db.getOneTimeBonusList(d);
                //r["total"] = dt.Rows.Count;
                //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                //r["code"] = 2000;
                //r["message"] = "查询成功";
                DataSet ds = db.getOneTimeBonusList(d, page, limit);
                DataTable dt = ds.Tables[0];//数据
                DataTable du = ds.Tables[1];
                r["items"] = dt;
                r["total"] = du.Rows[0][0];
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
        public Dictionary<string, object> getExportOneTimeBonus(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                //DataTable dt = db.getOneTimeBonusList(d);
                //r["total"] = dt.Rows.Count;
                //r["items"] = KVTool.TableToListDic(dt);
                //r["code"] = 2000;
                //r["message"] = "查询成功";
                DataSet ds = db.getOneTimeBonusList(d, 0, 0);
                DataTable dt = ds.Tables[0];//数据
                DataTable du = ds.Tables[1];
                //r["total"] = dt.Rows.Count;
                //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["items"] = dt;
                r["total"] = du.Rows[0][0];
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

        /// <summary>
        /// 验证导入奖金
        /// </summary>
        /// <param name="filePath">导入文件路径</param>
        /// <param name="importOrgCode">导入部门编码</param>
        /// <param name="dateMonth">导入月份</param>
        /// <returns></returns>
        public Dictionary<string, object> ValidateOneTimeBonus(string filePath, string importOrgCode, DateTime dateMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            //string validateSalary = "";//检验导入的工资合法性
            string validateIdNum = "";//检验导入的身份证号
            string validateBonus = "";//检验导入的调整合法性
            string validateOnce = "";//检验唯一性
            string validateMsg = "";//验证信息
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportOneTimeBonus> list = new List<ImportOneTimeBonus>();//一次性奖金数据集合
            //DataTable taxSalary = null;//工资表
            DataTable onetimeBonus = null;//一次性奖金表
            try
            {
                Init(filePath);//初始化数据
                //taxSalary = taxSalarydb.getTaxSalaryInfo(importOrgCode, dateMonth);
                //taxSalary = taxSalarydb.getTaxSalaryByYear(dateMonth);
                
                onetimeBonus = db.validateOnetimeTaxByWorkDate(dateMonth);
                string msg = validateTemp(HeaderRow);//检验模板是否匹配
                if (msg == "")
                {
                    rows = ExcelConverter.Convert<ImportOneTimeBonus>(Sheet, HeaderRow, 1);
                    foreach (var item in rows)
                    {
                        list.Add(HardCode(item));//将excel数据转换为List对象
                    }

                    #region 验证是否允许导入
                    foreach (var bonus in list)
                    {
                        //DataRow[] taxRows = taxSalary.Select("S_WorkerCode = '" + bonus.S_WorkerCode + "'");
                        //if (taxRows == null || taxRows.Length == 0)
                        //{
                        //    validateBonus += "【" + bonus.S_WorkerCode + ":" + bonus.S_WorkerName + "】";
                        //}

                        Regex reg = new Regex(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$");
                        if (bonus.IdTypeName.Contains("身份证"))
                        {
                            if (!reg.IsMatch(bonus.IdNumber.ToString()))
                            {
                                validateIdNum += "【" + bonus.S_WorkerCode + ":" + bonus.S_WorkerName + "】";
                            }
                        }
                        //else if (taxRows.Length != 1)
                        //{
                        //    validateSalary += "【" + bonus.S_WorkerCode + ":" + bonus.S_WorkerName + "】";
                        //}
                        DataRow[] onceRows = onetimeBonus.Select("S_WorkerCode = '" + bonus.S_WorkerCode + "'");
                        if (onceRows.Length != 0)
                        {
                            validateOnce += "【" + bonus.S_WorkerCode + ":" + bonus.S_WorkerName + "】";
                        }
                     
                    }
                    //if (!string.IsNullOrEmpty(validateBonus))
                    //{
                    //    validateBonus = "不存在" + validateBonus + "工资信息！";
                    //    validateMsg += validateBonus;
                    //}
                    //if (!string.IsNullOrEmpty(validateSalary))
                    //{
                    //    validateSalary = validateSalary + "工资信息重复！";
                    //    validateMsg += validateSalary;
                    //}
                    if (!string.IsNullOrEmpty(validateIdNum))
                    {
                        validateIdNum =  validateBonus + "身份证号不正确！";
                        validateMsg += validateIdNum;
                    }
                    if (!string.IsNullOrEmpty(validateOnce))
                    {
                        validateOnce = validateOnce + "已存在一次性奖金记录，无法重复导入！";
                        validateMsg += validateOnce;
                    }
                    
                    if (!string.IsNullOrEmpty(validateMsg))
                    {
                        validateMsg =  "全年一次性奖金导入失败，"   + validateMsg;
                        r["code"] = -1;
                        r["message"] = validateMsg;
                        return r;
                    }
                    else
                    {
                        r["code"] = 2000;
                        r["message"] = "验证通过";
                        r["item"] = filePath;
                    }
                    #endregion
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
                r["message"] = ex.Message;
            }
            r["item"] = filePath;
            return r;
        }

        public Dictionary<string, object> ImportOneTimeBonus(string filePath, string importOrgCode, string importOrgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportOneTimeBonus> list = new List<ImportOneTimeBonus>();//一次性奖金数据集合
           // DataTable taxSalary = null;//部门工资表
            try
            {
                Init(filePath);//初始化数据
                //taxSalary = taxSalarydb.getTaxSalaryInfo(importOrgCode, dateMonth);
                rows = ExcelConverter.Convert<ImportOneTimeBonus>(Sheet, HeaderRow, 1);
                foreach (var item in rows)
                {
                    list.Add(HardCode(item));//将excel数据转换为List对象
                }

                //foreach (var bonus in list)
                //{
                //    DataRow[] taxRows = taxSalary.Select("S_WorkerCode = '" + bonus.S_WorkerCode + "'");
                //    if (taxRows != null && taxRows.Length == 1)
                //    {
                //        bonus.S_OrgName = taxRows[0]["S_OrgName"].ToString();
                //        bonus.S_OrgCode = taxRows[0]["S_OrgCode"].ToString();
                //    }
                //}
                r["message"] = db.createOneTimeBonus(list, dateMonth, userId, importOrgCode, importOrgName);
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

        #region 硬编码方式转换实体对象
        public static string isnull(string value)
        {
            if (value == "")
            {
                return "0";
            }
            return value.Replace(" ", "");
        }
        /// <summary>
        /// 针对一次性奖金硬编码
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static ImportOneTimeBonus HardCode(ExcelDataRow row)
        {
            var t = new ImportOneTimeBonus();
            t.S_WorkerCode = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerCode").ColValue.Replace(" ", "");
            t.S_WorkerName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerName").ColValue.Replace(" ", "");
            //t.S_OrgName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_OrgName").ColValue;
            t.OneTimeBonus = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "OneTimeBonus").ColValue));
            t.DeductibleTax = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "DeductibleTax").ColValue));

            t.IdTypeName = row.DataCols.SingleOrDefault(c => c.PropertyName == "IdTypeName").ColValue.Replace(" ", "");
            //t.IdType = row.DataCols.SingleOrDefault(c => c.PropertyName == "IdType").ColValue.Replace(" ", "");
            t.IdNumber = row.DataCols.SingleOrDefault(c => c.PropertyName == "IdNumber").ColValue.Replace(" ", "");

            t.FreeIncome = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "FreeIncome").ColValue));
            t.Other = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Other").ColValue));
            t.AllowDeduction = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "AllowDeduction").ColValue));
            t.TaxSaving = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "TaxSaving").ColValue));

            t.Remark = row.DataCols.SingleOrDefault(c => c.PropertyName == "Remark").ColValue.Replace(" ", "");
          
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
        public string validateTemp(ExcelHeaderRow headerRow)
        {
            string msg = "";
            foreach (var headItem in headerRow.Cells)
            {
                switch (headItem.ColName)
                {
                    case "工号":
                    case "姓名":
                    case "*证照类型":
                    case "*证照号码":
                    case "*全年一次性奖金额":
                    case "免税收入":
                    case "其他":
                    case "准予扣除的捐赠额":
                    case "减免税额":
                    case "已扣缴税额":
                    case "备注":
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

        /// <summary>
        /// 清空一次性奖金信息
        /// </summary>
        /// <returns></returns>
        public string clearTaxBonus(DateTime dateMonth, string userId, string orgCode)
        {
            return db.clearOneTimeBonus(dateMonth, userId, orgCode);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string delOneTimeBonus(string id)
        {
            return db.delOneTimeBonus(id);
        }
    }
}
