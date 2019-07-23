using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;
using UIDP.UTILITY.ExcelOperation.Model;

namespace UIDP.BIZModule
{
    public class TaxBusinessManagementModule
    {
        TaxBusinessManagementDB db = new TaxBusinessManagementDB();
        #region
        private static IWorkbook workbook { get; set; }
        private static ISheet sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion

        public Dictionary<string,object> getTaxInfo(string page,string limit,string S_OrgCode,DateTime S_WorkDate,string id,string workerNumber)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            int li = int.Parse(limit);
            int pa = int.Parse(page);
            try
            {
                DataSet ds = db.createTemp(S_OrgCode, S_WorkDate, id, workerNumber,li,pa);
                DataTable dt = ds.Tables[0];//数据
                DataTable du = ds.Tables[1];
                //DataTable dt = db.getTaxInfo(S_OrgCode, S_WorkDate,id,workerNumber);
                r["message"] = "成功";
                //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, pa, li));
                r["items"] = dt;
                r["code"] = 2000;
                r["total"] = du.Rows[0][0];
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = 2000;
            }
            return r;           
        }

        public Dictionary<string,object> validateExcel(string filePath, string importOrgCode, string importOrgName, DateTime dateMonth)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string workcoderepeat = "";//验证导入excel内是否有重复人员
            string Idrepeat = "";//验证导入excel内是否有重复身份账号
            List<ExcelDataRow> rows = new List<ExcelDataRow>();
            List<ImportTaxBusiness> list = new List<ImportTaxBusiness>();
            string hardCodeMsg = "";
            try
            {
                init(filePath);
                hardCodeMsg = validateHeader(HeaderRow);
                if (!string.IsNullOrEmpty(hardCodeMsg))
                {
                    result["code"] = -1;
                    result["message"] = hardCodeMsg;
                    return result;
                }
                rows = ExcelConverter.Convert<ImportTaxBusiness>(sheet, HeaderRow, 1);
                foreach (var row in rows)
                {
                    list.Add(hardCode(row));
                    hardCodeMsg = "第" + list.Count() + 1 + "条数据错误!";
                }
                var workcode = list.GroupBy(t => t.WorkNumber).Where(t => t.Count() > 1).ToList();
                foreach(var item in workcode)
                {
                    workcoderepeat += "【" + item.Key + "】";
                }
                if (!string.IsNullOrEmpty(workcoderepeat))
                {
                    workcoderepeat = "员工编号：" + workcoderepeat + "金税人员信息重复，请确认导入信息";
                    result["code"] = -1;
                    result["message"] = "导入失败！" + workcoderepeat;
                    return result;
                }
                var idNumber = list.GroupBy(t => t.IDNumber).Where(t => t.Count() > 1).ToList();
                foreach(var item in idNumber)
                {
                    Idrepeat+= "【" + item.Key + "】";
                }
                if (!string.IsNullOrEmpty(Idrepeat))
                {
                    Idrepeat = "员工身份证号：" + Idrepeat + "金税人员信息重复，请确认导入信息";
                    result["code"] = -1;
                    result["message"] = "导入失败！" + Idrepeat;
                    return result;
                }
                //验证数据库中本月是否存在重复人员的金税信息
                //DataTable dt = db.judgeTaxInfo( dateMonth);
                //foreach (var item in list)
                //{
                //    DataRow[] dr = dt.Select("S_WorkNumber='" + item.WorkNumber + "'");
                //    if (dr.Count() > 0)
                //    {
                //        result["code"] = -1;
                //        result["message"] = "金税人员重复，请检查工号为" + dr[0]["S_WorkNumber"].ToString() + "姓名为" + dr[0]["S_WorkName"].ToString() + "的员工信息";
                //        return result;
                //    }
                //    DataRow[] du = dt.Select("IDNumber='" + item.IDNumber + "'");
                //    if (du.Count() > 0)
                //    {
                //        result["code"] = -1;
                //        result["message"] = "金税人员重复，请检查身份证号为" + du[0]["IDNumber"].ToString() + "姓名为" + du[0]["S_WorkName"].ToString() + "的员工信息";
                //        return result;
                //    }
                //}
                result["code"] = 2000;
                result["item"] = filePath;
            }
            catch(Exception e)
            {
                result["code"] = -1;
                result["message"] = e.Message+hardCodeMsg;
            }
            return result;
        }

        public Dictionary<string,object> ImportTaxBusiness(string filePath, string importOrgCode, string importOrgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<ExcelDataRow> rows = new List<ExcelDataRow>();
            List<ImportTaxBusiness> list = new List<ImportTaxBusiness>();
            try
            {
                init(filePath);
                rows = ExcelConverter.Convert<ImportTaxBusiness>(sheet, HeaderRow, 1);
                foreach (var row in rows)
                {
                    list.Add(hardCode(row));//将excel数据转换为List对象
                }
                result["message"] = db.createTaxBusiness(list, dateMonth, userId, importOrgCode, importOrgName);
                if (result["message"].ToString()=="")
                {
                    result["code"] = 2000;
                }
                else
                {
                    result["code"] = -1;
                }
            }
            catch(Exception e)
            {
                result["code"] = -1;
                result["message"] = e.Message;
            }
            return result;
        }

        public Dictionary<string,object> delTaxBus(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.delTaxBusiness(d);
                if (string.IsNullOrEmpty(b))
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = b;
                    r["code"] = -1;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        /// <summary>
        /// 判断当前
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="id"></param>
        /// <param name="workNumber"></param>
        /// <returns></returns>
        public Dictionary<string,object> getCount(string S_OrgCode, DateTime S_WorkDate, string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getCount(S_OrgCode, S_WorkDate, id);
                if (dt.Rows.Count != 0)
                {
                    r["message"] = "已导入数据！";
                    r["items"] = dt;
                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = 2001;
                    r["items"] = dt;
                    r["message"] = "未导入数据";
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public string validateHeader(ExcelHeaderRow headerRow)
        {
            string errorMsg = "";
            foreach(var headerName in headerRow.Cells)
            {
                switch (headerName.ColName.Trim())
                {
                    case "工号":
                    case "姓名":
                    case "证照类型":
                    case "证照号码":
                    case "税款所属期起":
                    case "税款所属期止":
                    case "所得项目":
                    case "本期收入":
                    case "本期费用":
                    case "本期免税收入":
                    case "本期基本养老保险费":
                    case "本期基本医疗保险费":
                    case "本期失业保险费":
                    case "本期住房公积金":
                    case "本期企业(职业)年金":
                    case "本期商业健康保险费":
                    case "本期税延养老保险费":
                    case "本期其他扣除(其他)":
                    case "累计收入额":
                    case "累计减除费用":
                    case "累计专项扣除":
                    case "累计子女教育支出扣除":
                    case "累计住房贷款利息支出扣除":
                    case "累计住房租金支出扣除":
                    case "累计赡养老人支出扣除":
                    case "累计继续教育支出扣除":
                    case "累计其他扣除":
                    case "累计准予扣除的捐赠":
                    case "累计应纳税所得额":
                    case "税率":
                    case "速算扣除数":
                    case "累计应纳税额":
                    case "累计减免税额":
                    case "累计应扣缴税额":
                    case "累计已预缴税额":
                    case "累计应补(退)税额":
                    case "备注":
                        break;
                    default:
                        errorMsg += "表头错误!请验证【" + headerName.ColName + "】列的表头名称！";
                        break;
                }
            }
            return errorMsg;
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
                using(FileStream fs = File.Open(fileUrl, FileMode.Open))
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
            catch(Exception e)
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
            for(int i = 0; i < row.PhysicalNumberOfCells; i++)
            {
                cell = row.GetCell(i);
                HeaderRow.Cells.Add(new ExcelCol()
                {
                    ColIndex = i,
                    ColName = cell.GetStringValue()
                });
            }
        }

        public static ImportTaxBusiness hardCode(ExcelDataRow row)
        {
            var t = new ImportTaxBusiness();
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            t.WorkNumber = row.DataCols.SingleOrDefault(c => c.PropertyName == "WorkNumber").ColValue.Replace(" ", "");//工号
            t.Name = row.DataCols.SingleOrDefault(c => c.PropertyName == "Name").ColValue.Replace(" ", "");
            t.IDType = row.DataCols.SingleOrDefault(c => c.PropertyName == "IDType").ColValue.Replace(" ", "");
            t.IDNumber = row.DataCols.SingleOrDefault(c => c.PropertyName == "IDNumber").ColValue.Replace(" ", "");
            t.StartTime = Convert.ToDateTime(row.DataCols.SingleOrDefault(c => c.PropertyName == "StartTime").ColValue==""?" ": row.DataCols.SingleOrDefault(c => c.PropertyName == "StartTime").ColValue, dtFormat);
            t.EndTime = Convert.ToDateTime(row.DataCols.SingleOrDefault(c => c.PropertyName == "EndTime").ColValue==""?" ":row.DataCols.SingleOrDefault(c => c.PropertyName == "EndTime").ColValue, dtFormat);
            t.Income= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Income").ColValue));
            t.Tax= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Tax").ColValue));
            t.OlderInsurance= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "OlderInsurance").ColValue));
            t.HeathInsurance= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "HeathInsurance").ColValue));
            t.JobInsurance= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "JobInsurance").ColValue));
            t.HousingFund= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "HousingFund").ColValue));
            t.ChildEdu = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "ChildEdu").ColValue));
            t.ContinueEdu= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "ContinueEdu").ColValue));
            t.HousingLoan = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "HousingLoan").ColValue));
            t.HousingRent = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "HousingRent").ColValue));
            t.Support = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Support").ColValue));
            t.EnterpriseAnnuity= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "EnterpriseAnnuity").ColValue));
            t.CommercialHealthinsurance= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "CommercialHealthinsurance").ColValue));
            t.EndowmentInsurance= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "EndowmentInsurance").ColValue));
            t.Other = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Other").ColValue));
            t.Donation = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Donation").ColValue));
            t.Remark = row.DataCols.SingleOrDefault(c => c.PropertyName == "Remark").ColValue.Replace(" ", "");
            t.Deductions = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Deductions").ColValue));
            t.TaxSavings = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "TaxSavings").ColValue));
            t.Reduction = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Reduction").ColValue));
            t.WithholdingTax = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "WithholdingTax").ColValue));
            t.AccumulatedIncome = Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "AccumulatedIncome").ColValue));
            t.AccumulatedSpecialDeduction= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "AccumulatedSpecialDeduction").ColValue));
            t.CumulativeOther= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "CumulativeOther").ColValue));
            t.TaxRate= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "TaxRate").ColValue));
            t.QuickDeduction= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "QuickDeduction").ColValue));
            t.AccumulatedTax= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "AccumulatedTax").ColValue));
            t.CumulativeWithholding= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "CumulativeWithholding").ColValue));
            t.Drawback= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Drawback").ColValue));
            t.IncomeItem= row.DataCols.SingleOrDefault(c => c.PropertyName == "IncomeItem").ColValue.Replace(" ", "");
            t.Cost= Convert.ToDecimal(isnull(row.DataCols.SingleOrDefault(c => c.PropertyName == "Cost").ColValue));
            return t;
        }

        public static string isnull(string value)
        {
            if(value=="")
            {
                return "0";
            }
            return value.Replace(" ", "");
        }

    }
}
