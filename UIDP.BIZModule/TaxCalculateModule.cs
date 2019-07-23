using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class TaxCalculateModule
    {
        ODS.TaxCalculateDB db = new ODS.TaxCalculateDB();
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        private static ExcelHeaderRow HeaderRow { get; set; }
        #endregion
        /// <summary>
        /// 根据单位和当前工作月份查询数据状态
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="WorkMonth"></param>
        /// <returns></returns>
        public DataSet GetData(string OrgCode, DateTime WorkMonth)
        {
            return db.GetData(OrgCode, WorkMonth);
        }
        public Dictionary<string, object> getCalculateData(string OrgCode, DateTime WorkMonth) {
            Dictionary<string, object> res = new Dictionary<string, object>();
            DataSet ds = GetData(OrgCode, WorkMonth);
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select("ReportStatus=-1");
                if (rows.Length > 0)
                {
                    res["TaxStatus"] = -1;
                    res["items"] = dt;
                    res["total"] = dt.Rows.Count;
                    res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                    res["message"] = "成功";
                    res["code"] = 2000;
                    return res;
                }
                rows = dt.Select("ReportStatus=0");
                if (rows.Length > 0)
                {
                    res["TaxStatus"] = 0;
                    res["items"] = dt;
                    res["total"] = dt.Rows.Count;
                    res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                    res["message"] = "成功";
                    res["code"] = 2000;
                    return res;
                }
                rows = dt.Select("ReportStatus=1");
                if (rows.Length > 0)
                {
                    res["TaxStatus"] = 1;
                    res["items"] = dt;
                    res["message"] = "成功";
                    res["total"] = dt.Rows.Count;
                    res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                    res["code"] = 2000;
                    return res;
                }
                rows = dt.Select("ReportStatus=2");
                if (rows.Length > 0)
                {
                    res["TaxStatus"] = 2;
                    res["items"] = dt;
                    res["total"] = dt.Rows.Count;
                    res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                    res["message"] = "成功";
                    res["code"] = 2000;
                    return res;
                }
            }
            else {
                res["TaxStatus"] = -1;
                res["items"] = new DataTable();
                res["TaxPayerCount"] = 0;
                res["total"] = 0;
                res["message"] = "成功";
                res["code"] = 2000;
            }
            return res;
        }
        public Dictionary<string, object> CalculateTax(string  UserId, DateTime WorkMonth, string OrgCode)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            string result = db.CalculateTax(UserId, WorkMonth, OrgCode);
            if (result == "")
            {
                res["message"] = "计算完成！";
                res["code"] = 2000;
            }
            else {
                res["message"] = result;
                res["code"] = -1;
            }
            return res;
        }

        /// <summary>
        /// 工资到算器
        /// </summary>
        /// <param name="taxNumber"></param>
        public Dictionary<string, object> GetInverseCalculate(decimal taxNumber)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt= db.GetInverseCalculate(taxNumber);
            if (dt != null & dt.Rows.Count > 0)
            {
                r["result"] = dt.Rows[0][0].ToString();
                r["message"] = "成功";
                r["code"] = 2000;
            }
            else {
                r["result"] = 0;
                r["message"] = "成功";
                r["code"] = 2000;
            }
            return r;
        }
 public string calPreCalculateTax(Dictionary<string, object> d)
        {
            d["S_UpdateDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            return db.calPreCalculateTax(d);
        }
        /// <summary>
        /// 个税计算器
        /// </summary>
        /// <param name="taxNumber"></param>
        public Dictionary<string, object> cal(decimal taxNumber)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.cal(taxNumber);
            if (dt != null & dt.Rows.Count > 0)
            {
                r["result"] = Convert.ToDouble(dt.Rows[0][0].ToString());
                r["message"] = "成功";
                r["code"] = 2000;
            }
            else
            {
                r["result"] = 0;
                r["message"] = "成功";
                r["code"] = 2000;
            }
            return r;
        }
        /// <summary>
        /// 一次性奖金计算器
        /// </summary>
        /// <param name="taxNumber"></param>
        public Dictionary<string, object> onetimecal(decimal taxNumber)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.onetimecal(taxNumber);
            if (dt != null & dt.Rows.Count > 0)
            {
                r["result"] = Convert.ToDouble(dt.Rows[0][0].ToString());
                r["message"] = "成功";
                r["code"] = 2000;
            }
            else
            {
                r["result"] = 0;
                r["message"] = "成功";
                r["code"] = 2000;
            }
            return r;
        }
        public Dictionary<string, object> ImportTaxSalary(string filePath, string userId,string importModel, string taxOffice)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            List<ExcelDataRow> rows = new List<ExcelDataRow>();//导入数据转成List集合
            List<ImportTaxSalary1> list1 = new List<ImportTaxSalary1>();//模板一数据集合
            List<ImportTaxSalary2> list2 = new List<ImportTaxSalary2>();//模板二数据集合

            try
            {
                Init(filePath);//初始化数据
                if (importModel == "样表一")
                {
                    rows = ExcelConverter.Convert<ImportTaxSalary1>(Sheet, HeaderRow, 1);
                    foreach (var item in rows)
                    {
                        list1.Add(HardCode1(item));//将excel数据转换为List对象
                    }
                    string guid = Guid.NewGuid().ToString();
                    string result  = db.createTaxSalary1(list1,  userId, guid);
                    if (result == "")
                    {
                        //调用（全年平均核算）计算存储过程
                        string salary = db.SalaryTaxCalculator(guid, taxOffice);
                        string[] arr = salary.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        r["YeaTax"] = arr[0].ToString();
                        r["MonthTax"] = arr[1].ToString();
                        r["message"] = "成功";
                        r["code"] = 2000;
                    }
                    else {
                        r["YeaTax"] = "";
                        r["MonthTax"] = "";
                        r["message"] = "成功";
                        r["code"] = 2000;
                    }
                    
                }
                else if (importModel == "样表二")
                {
                    rows = ExcelConverter.Convert<ImportTaxSalary2>(Sheet, HeaderRow, 1);
                    foreach (var item in rows)
                    {
                        list2.Add(HardCode2(item));//将excel数据转换为List对象
                    }
                    string guid = Guid.NewGuid().ToString();
                    string result = db.createTaxSalary2(list2, userId, guid);
                    if (result == "")
                    {
                        //调用（全年平均核算）计算存储过程
                        string salary = db.SalaryTaxCalculator(guid, taxOffice);
                        string[] arr = salary.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        r["YeaTax"] = arr[0].ToString();
                        r["MonthTax"] = arr[1].ToString();
                        r["message"] = "成功";
                        r["code"] = 2000;
                    }
                    else {
                        r["YeaTax"] ="";
                        r["MonthTax"] = "";
                        r["message"] = "成功";
                        r["code"] = 2000;
                    }
                }

            }
            catch (Exception ex)
            {
                r["SalaryTax"] = 0;
                r["code"] = -1;
                r["message"] = ex.Message;
                return r;
            }
            return r;
        }
       
        #region MyRegion
        public static ImportTaxSalary1 HardCode1(ExcelDataRow row)
        {
            var t = new ImportTaxSalary1();
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

        public static string isnull(string value) {
            if (value=="") {
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
        /// <summary>
        /// 年计算器
        /// </summary>
        /// <param name="taxNumber"></param>
        /// <returns></returns>
        public Dictionary<string, object> Yearcal(decimal taxNumber,int month)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.Yearcal(taxNumber,month);
            if (dt != null & dt.Rows.Count > 0)
            {
                r["result"] = Convert.ToDouble(dt.Rows[0][0].ToString());
                r["message"] = "成功";
                r["code"] = 2000;
            }
            else
            {
                r["result"] = 0;
                r["message"] = "成功";
                r["code"] = 2000;
            }
            return r;
        }
    }
}
