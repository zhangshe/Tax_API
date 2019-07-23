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
using UIDP.UTILITY.ExcelOperation.Model;

namespace UIDP.BIZModule
{
    public class ServiceRemunerationImportModule
    {
        ServiceRemunerationImportDB dB = new ServiceRemunerationImportDB();
        #region
        private static IWorkbook workbook { get; set; }
        private static ISheet sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion

        public Dictionary<string, object> getlist(string page, string limit, string S_OrgCode, DateTime S_WorkDate, string id, string workNumber, int flag)
        {
            int pa = Convert.ToInt32(page);
            int li = Convert.ToInt32(limit);
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = dB.getlist(S_OrgCode, S_WorkDate, id, workNumber, flag);
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, pa, li);
                }
                else
                {
                    r["total"] = 0;
                    r["message"] = "没有数据";
                    r["code"] = 2000;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public Dictionary<string, object> validate(string filepath, string orgCode, string orgName, DateTime dateMonth)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string errorMsg = "";
            string coderepeat = "";
            string idrepeat = "";
            string headerMsg = "";
            List<ExcelDataRow> rows = new List<ExcelDataRow>();
            List<importService> list = new List<importService>();
            try
            {
                init(filepath);
                headerMsg = validateHeader(HeaderRow);
                if (!string.IsNullOrEmpty(headerMsg))
                {
                    result["code"] = -1;
                    result["message"] = headerMsg;
                    return result;
                }
                rows = ExcelConverter.Convert<importService>(sheet, HeaderRow, 1);
                foreach (ExcelDataRow row in rows)
                {
                    list.Add(HardCode(row));
                    errorMsg = "第" + (list.Count() + 1) + "条数据有误!";
                }
                errorMsg = "";
                #region 验证必填项是否为空
                foreach (importService item in list)
                {
                    if (item.IDtype == "" || item.IDNumber == "" || item.IncomeItem == "" || item.Income == "")
                    {
                        errorMsg += "工号为【" + item.WorkerCode + "】姓名为【" + item.WorkerName + "】的员工必填项为空,请仔细检查数据表!";
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result["code"] = -1;
                    result["message"] = errorMsg;
                    return result;
                }
                errorMsg = "";
                #endregion
                //#region 工号验证重复
                //var workcode = list.GroupBy(t => t.WorkerCode).Where(t => t.Count() > 1).ToList();
                //foreach (var item in workcode)
                //{
                //    coderepeat = "【" + item.Key + "】";
                //}
                //if (!string.IsNullOrEmpty(coderepeat))
                //{
                //    result["code"] = -1;
                //    result["message"] = "工号" + coderepeat + "重复！";
                //    return result;
                //}
                //#endregion
                #region 身份证规则验证
                Regex reg = new Regex(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$");
                foreach (var item in list)
                {
                    if (item.IDtype.Contains("居民身份证"))
                    {
                        if (!reg.IsMatch(item.IDNumber))
                        {
                            idrepeat += "姓名【" + item.WorkerName + "】" + "身份证号为【" + item.IDNumber + "】身份证格式不正确!";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(idrepeat))
                {
                    result["code"] = -1;
                    result["message"] = idrepeat;
                    return result;
                }
                #endregion

            }
            catch (Exception e)
            {
                result["code"] = -1;
                result["message"] = e.Message + errorMsg;
            }
            result["code"] = 2000;
            result["message"] = "成功";
            result["item"] = filepath;
            return result;
        }

        public Dictionary<string, object> importdata(string filePath, string orgCode, string orgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<ExcelDataRow> rows = new List<ExcelDataRow>();
            List<importService> list = new List<importService>();
            init(filePath);
            rows = ExcelConverter.Convert<importService>(sheet, HeaderRow, 1);
            foreach (ExcelDataRow row in rows)
            {
                list.Add(HardCode(row));
            }
            try
            {
                string msg = dB.createImportData(list, orgCode, orgName, dateMonth, userId);
                if (!string.IsNullOrEmpty(msg))
                {
                    result["code"] = -1;
                    result["message"] = msg;
                }
                else
                {
                    result["code"] = 2000;
                    result["message"] = "成功";
                }
            }
            catch (Exception e)
            {
                result["code"] = -1;
                result["message"] = e.Message;
            }
            return result;
        }

        public Dictionary<string, object> getcount(string S_OrgCode, DateTime S_WorkDate, string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            r["code"] = 2000;
            r["items"] = dB.getcount(S_OrgCode, S_WorkDate, id);
            return r;
        }

        public Dictionary<string, object> delData(string orgCode, DateTime dateMonth, string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            string str = dB.delData(orgCode, dateMonth, id);
            if (!string.IsNullOrEmpty(str))
            {
                r["code"] = -1;
                r["message"] = str;
            }
            else
            {
                r["code"] = 2000;
                r["message"] = "成功";
            }
            return r;
        }
        #region 导入辅助方法
        /// <summary>
        /// 硬编码
        /// </summary>
        /// <param name="row">excel数据</param>
        /// <returns></returns>
        public importService HardCode(ExcelDataRow row)
        {
            importService t = new importService();
            t.WorkerCode = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerCode").ColValue);
            t.WorkerName = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkerName").ColValue);
            t.IDtype = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IDtype").ColValue);
            t.IDNumber = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IDNumber").ColValue);
            t.IncomeItem = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "IncomeItem").ColValue);
            t.Income = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Income").ColValue);
            t.Tax = iszero(row.DataCols.SingleOrDefault(c => c.PropertyName == "Tax").ColValue);
            t.CommercialHealthinsurance = iszero(row.DataCols.SingleOrDefault(c => c.PropertyName == "CommercialHealthinsurance").ColValue);
            t.EndowmentInsurance = iszero(row.DataCols.SingleOrDefault(c => c.PropertyName == "EndowmentInsurance").ColValue);
            t.Donation = iszero(row.DataCols.SingleOrDefault(c => c.PropertyName == "Donation").ColValue);
            t.other = iszero(row.DataCols.SingleOrDefault(c => c.PropertyName == "other").ColValue);
            t.TaxSavings = iszero(row.DataCols.SingleOrDefault(c => c.PropertyName == "TaxSavings").ColValue);
            t.Remark = isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Remark").ColValue);
            return t;

        }

        public static string isnull(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Replace("\n", "").Replace("\t", "").Replace("\r", "").Trim();
            }
            else
            {
                return str;
            }
        }

        public static decimal iszero(string num)
        {
            if (!string.IsNullOrEmpty(num))
            {
                return Convert.ToDecimal(num);
            }
            else
            {
                return 0;
            }
        }
        public string validateHeader(ExcelHeaderRow headerRow)
        {
            string msg = "";
            foreach (ExcelCol headerName in headerRow.Cells)
            {
                switch (headerName.ColName.Trim())
                {
                    case "工号":
                    case "姓名":
                    case "*证照类型":
                    case "*证照号码":
                    case "*所得项目":
                    case "*本期收入":
                    case "本期免税收入":
                    case "商业健康保险":
                    case "税延养老保险":
                    case "准予扣除的捐赠额":
                    case "其他":
                    case "减免税额":
                    case "备注":
                        break;
                    default:
                        msg += "【" + headerName.ColName + "】" + "错误！";
                        break;
                }
            }
            return msg;
        }


        public static void init(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                throw new ArgumentException("fileUrl");
            }
            if (!File.Exists(fileUrl))
            {
                throw new FileNotFoundException();
            }
            string extention = Path.GetExtension(fileUrl).ToLower().Trim();
            if (extention != ".xls" && extention != ".xlsx")
            {
                throw new NotSupportedException("非法文件!");
            }

            try
            {
                //FileStream fs = File.Open(fileUrl, FileMode.Open);
                //byte[] array = new byte[fs.Length];
                //fs.Read(array, 0, array.Length);
                //if (extention == ".xls")
                //{
                //    workbook = new HSSFWorkbook(fs);
                //}
                //else
                //{
                //    workbook = WorkbookFactory.Create(fs);
                //}
                //fs.Close();//关闭流
                //fs.Dispose();//释放资源
                using (FileStream fs = File.Open(fileUrl, FileMode.Open))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    if (extention == ".xls")
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        workbook = WorkbookFactory.Create(fs);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            sheet = workbook.GetSheetAt(0);
            SetDictHeader();
        }

        private static void SetDictHeader()
        {
            HeaderRow = new ExcelHeaderRow();
            IRow row = sheet.GetRow(0);
            ICell cell;
            for (int i = 0; i < row.PhysicalNumberOfCells; i++)
            {
                cell = row.GetCell(i);
                HeaderRow.Cells.Add(new ExcelCol()
                {
                    ColIndex = i,
                    ColName = cell.GetStringValue()
                });
            }
        }
        #endregion

        public Dictionary<string,object> getReportState(string OrgCode, DateTime WorkDate)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                DataTable dt = dB.getReportState(OrgCode, WorkDate);
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("ReportStatus<>2");
                    if (dr.Count() != 0)
                    {
                        res["state"] = 1;
                        res["code"] = 2000;
                        res["message"] = "成功";
                    }
                    else
                    {
                        res["state"] = 0;
                        res["code"] = 2000;
                        res["message"] = "成功";
                    }
                }
                else
                {
                    res["state"] = 1;
                    res["code"] = 2000;
                    res["message"] = "成功";
                }
            }
            catch(Exception e)
            {
                res["state"] = -1;//异常状态
                res["code"] = -1;
                res["message"] = e.Message;
            }
            return res;
        }
    }
}
