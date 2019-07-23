
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class TaxAdjustModule
    {
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion

        TaxAdjustDB db = new TaxAdjustDB();
        TaxSalaryDB taxSalarydb = new TaxSalaryDB();//工资表
        public Dictionary<string, object> getTaxAdjustList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataSet ds = db.getTaxAdjustList(d,page,limit);
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
        public Dictionary<string, object> getExportTaxAdjust(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                //DataTable dt = db.getTaxAdjustList(d,0,0);
                //r["total"] = dt.Rows.Count;
                //r["items"] = KVTool.TableToListDic(dt);
                //r["code"] = 2000;
                //r["message"] = "查询成功";


                DataSet ds = db.getTaxAdjustList(d, 0, 0);
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
        /// 验证导入调整
        /// </summary>
        /// <param name="filePath">导入文件路径</param>
        /// <param name="importOrgCode">导入部门编码</param>
        /// <param name="dateMonth">导入月份</param>
        /// <returns></returns>
        public Dictionary<string, object> ValidateTaxAdjust(string filePath, string importOrgCode, DateTime dateMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            string validateSalary = "";//检验导入的工资合法性
            string validateAdjust = "";//检验导入的调整合法性
            string validateMsg = "";//验证信息
            string errorMsg = "";//错误行数信息
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxAdjust> list = new List<ImportTaxAdjust>();//个税调整数据集合
            DataTable taxSalary = null;//部门工资表
            try
            {
                Init(filePath);//初始化数据
                taxSalary = taxSalarydb.getTaxSalaryInfo(importOrgCode, dateMonth);
                string msg = validateTemp(HeaderRow);//检验模板是否匹配
                if (msg == "")
                {
                    rows = ExcelConverter.Convert<ImportTaxAdjust>(Sheet, HeaderRow, 1);
                    foreach (var item in rows)
                    {
                        list.Add(HardCode(item));//将excel数据转换为List对象
                        errorMsg = "第" + (list.Count() + 1) + "条数据有误,转换失败！";
                    }
                    errorMsg = "";
                    #region 验证是否允许导入
                    foreach (var adjust in list)
                    {
                        if (!string.IsNullOrEmpty(adjust.S_WorkerCode))
                        {
                            DataRow[] taxRows = taxSalary.Select("S_WorkerCode = '" + adjust.S_WorkerCode + "'");
                            if (taxRows == null || taxRows.Length == 0)
                            {
                                validateAdjust += "【" + adjust.S_WorkerCode + ":" + adjust.S_WorkerName + "】";
                            }
                            else if (taxRows.Length != 1)
                            {
                                validateSalary += "【" + adjust.S_WorkerCode + ":" + adjust.S_WorkerName + "】";
                            }
                        }
                        else {
                            r["code"] = -1;
                            r["message"] = adjust.S_WorkerName+ "工号不能为空！";
                            return r;
                        }
                    }
                    if (!string.IsNullOrEmpty(validateAdjust))
                    {
                        validateAdjust = "不存在" + validateAdjust + "工资信息！";
                        validateMsg += validateAdjust;
                    }
                    if (!string.IsNullOrEmpty(validateSalary))
                    {
                        validateSalary = validateSalary + "工资信息重复！";
                        validateMsg += validateSalary;
                    }
                    if (!string.IsNullOrEmpty(validateMsg))
                    {
                        validateMsg = "调整导入失败，" + dateMonth.ToString("yyyy-MM") + "，" + validateMsg;
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
                r["message"] = ex.Message+ errorMsg;
            }
            r["item"] = filePath;
            return r;
        }

        public Dictionary<string, object> ImportTaxAdjust(string filePath, string importOrgCode,string importOrgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxAdjust> list = new List<ImportTaxAdjust>();//个税调整数据集合
            DataTable taxSalary = null;//部门工资表
            try
            {
                Init(filePath);//初始化数据
                taxSalary = taxSalarydb.getTaxSalaryInfo(importOrgCode, dateMonth);
                rows = ExcelConverter.Convert<ImportTaxAdjust>(Sheet, HeaderRow, 1);
                foreach (var item in rows)
                {
                    list.Add(HardCode(item));//将excel数据转换为List对象
                }

                //foreach (var adjust in list)
                //{
                //    DataRow[] taxRows = taxSalary.Select("S_WorkerCode = '" + adjust.S_WorkerCode + "'");
                //    if (taxRows != null && taxRows.Length == 1)
                //    {
                //        adjust.S_OrgName = taxRows[0]["S_OrgName"].ToString();
                //        adjust.S_OrgCode = taxRows[0]["S_OrgCode"].ToString();
                //    }
                //}
                r["message"] = db.createTaxAdjust(list, dateMonth, userId, importOrgCode, importOrgName);
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
        /// 针对工资调整硬编码
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static ImportTaxAdjust HardCode(ExcelDataRow row)
        {
            var t = new ImportTaxAdjust();
            t.S_WorkerCode = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerCode").ColValue.Replace(" ", "");
            t.S_WorkerName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_WorkerName").ColValue.Replace(" ", "");
            //t.S_OrgName = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_OrgName").ColValue;
            t.Adjust9 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust9")?.ColValue));
            t.Adjust10 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust10")?.ColValue));
            t.Adjust11 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust11")?.ColValue));
            t.Adjust12 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust12")?.ColValue));
            t.Adjust13 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust13")?.ColValue));
            t.Adjust14 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust14")?.ColValue));
            t.Adjust15 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust15")?.ColValue));
            t.Adjust16 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust16")?.ColValue));
            t.Adjust17 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust17")?.ColValue));
            t.Adjust18 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust18")?.ColValue));
            t.Adjust19 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust19")?.ColValue));
            t.Adjust20 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust20")?.ColValue));
            t.Adjust21 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust21")?.ColValue));
            t.Adjust22 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust22")?.ColValue));
            t.Adjust23 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust23")?.ColValue));
            t.Adjust24 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust24")?.ColValue));
            t.Adjust25 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust25")?.ColValue));
            t.Adjust26 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust26")?.ColValue));
            t.Adjust27 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust27")?.ColValue));
            t.Adjust28 = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Adjust28")?.ColValue));
            t.S_Remark = row.DataCols.SingleOrDefault(c => c.PropertyName == "S_Remark").ColValue;
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
                    //case "部门":
                    //case "部门编号":
                    case "调增项5":
                    case "调增项6":
                    case "调增项7":
                    case "调增项8":
                    case "调增项9":
                    case "调增项10":
                    case "调增项11":
                    case "调增项12":
                    case "调增项13":
                    case "调增项14":
                    case "调减项5":
                    case "调减项6":
                    case "调减项7":
                    case "调减项8":
                    case "调减项9":
                    case "调减项10":
                    case "调减项11":
                    case "调减项12":
                    case "调减项13":
                    case "调减项14":
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
        /// 删除调整信息
        /// </summary>
        /// <returns></returns>
        public string deleteTaxAdjust(DateTime dateMonth, string userId, string orgCode)
        {
            return db.delTaxAdjust(dateMonth, userId, orgCode);
        }
    }
}
