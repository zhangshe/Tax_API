using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
namespace UIDP.UTILITY
{
    public class ExcelTools
    {
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">传入文件地址</param>
        /// <param name="modePath"模板地址</param>
        /// <param name="result">返回信息</param>
        /// <param name="dt">返回dt数据</param>
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
                string filePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\Templates\\" + tempName + ".xls";//原始文件
                Init(filePath);
                if (tempName == "月度税务报表模板" || tempName == "年度税务报表模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //for (int j = 0; j < dt.Columns.Count; j++)
                        //{
                        //    dt.Columns[j].ColumnName;
                        //}
                        for (int j = 0; j < col.Count; j++)
                        {
                            if (col[j] == "D_SL")
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j).SetCellValue(dt.Rows[i][col[j]].ToString() + "%");
                            }
                            else
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                            }
                        }
                    }
                }
                else if (tempName == "月度税务核算报表模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表单位：" + orgName);
                    Sheet.GetRow(2).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                    double sumYFHJ = 0.00;
                    double sumIncome = 0.00;
                    //double sumCost = 0.00;
                    double sumDrawback = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //for (int j = 0; j < dt.Columns.Count; j++)
                        //{
                        //    dt.Columns[j].ColumnName;
                        //}
                        for (int j = 0; j < col.Count; j++)
                        {
                            switch (col[j])
                            {
                                case "T_YFHJ":
                                    sumYFHJ = sumYFHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Income":
                                    sumIncome = sumIncome + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                //case "Cost":
                                //    sumCost = sumCost + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                case "Drawback":
                                    sumDrawback = sumDrawback + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                default:
                                    break;
                            }
                            Sheet.GetRow(dateRowNum + i).GetCell(0).SetCellValue(i + 1);//序号列
                            //if (col[j] == "D_SL" && dt.Rows[i][col[j]].ToString() != "合计")
                            //{
                            //    Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString() + "%");
                            //}
                            //else { Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString()); }
                            if (j > 7)
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(Math.Round(Convert.ToDouble(dt.Rows[i][col[j]].ToString()),2));
                            }
                            else { Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString()); }

                        }
                    }
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(8).SetCellValue("￥" + sumYFHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(9).SetCellValue("￥" + sumIncome.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(10).SetCellValue("￥" + sumCost.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(27).SetCellValue("￥" + sumDrawback.ToString("N2"));




                }
                else if (tempName == "月度税务测算模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表单位：" + orgName);
                    Sheet.GetRow(2).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                    double sumYSHJ = 0.00;
                    double sumBQSR = 0.00;
                    //double sumYangLaoBX = 0.00;
                    //double sumYiLiaoBX = 0.00;
                    //double sumSYBX = 0.00;
                    //double sumZFGJJ = 0.00;
                    //double sumQYNJ = 0.00;
                    //double sumSFGZ = 0.00;
                    //double sumChildEdu = 0.00;
                    //double sumHousingLoan = 0.00;
                    //double sumHousingRent = 0.00;
                    //double sumSupport = 0.00;
                    //double sumContinueEdu = 0.00;
                    //double sumYSSDE = 0.00;
                    double sumDJJE = 0.00;
                    double sumKSHJ = 0.00;
                    double sumLJ_KS = 0.00;
                    //double sumLJYJS = 0.00;
                    //double sumWithholdingTax = 0.00;
                    //double sumYCXJJ = 0.00;
                    //double sumYCXJJYKS = 0.00;
                    //double sumSE = 0.00;
                    //double sumKS = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < col.Count; j++)
                        {
                            switch (col[j])
                            {
                                case "T_YSHJ":
                                    sumYSHJ = sumYSHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "T_BQSR":
                                    sumBQSR = sumBQSR + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                //case "K_YangLaoBX":
                                //    sumYangLaoBX = sumYangLaoBX + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "K_YiLiaoBX":
                                //    sumYiLiaoBX = sumYiLiaoBX + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "K_SYBX":
                                //    sumSYBX = sumSYBX + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "K_ZFGJJ":
                                //    sumZFGJJ = sumZFGJJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "K_QYNJ":
                                //    sumQYNJ = sumQYNJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "T_SFGZ":
                                //    sumSFGZ = sumSFGZ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "ChildEdu":
                                //    sumChildEdu = sumChildEdu + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "HousingLoan":
                                //    sumHousingLoan = sumHousingLoan + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "HousingRent":
                                //    sumHousingRent = sumHousingRent + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "Support":
                                //    sumSupport = sumSupport + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "ContinueEdu":
                                //    sumContinueEdu = sumContinueEdu + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                //case "T_YSSDE":
                                //    sumYSSDE = sumYSSDE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                //    break;
                                case "T_DJJE":
                                    sumDJJE = sumDJJE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "KS_HJ":
                                    sumKSHJ = sumKSHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "LJ_KS":
                                    sumLJ_KS = sumLJ_KS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                    
                                    //case "T_LJYJS":
                                    //    sumLJYJS = sumLJYJS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    //    break;
                                    //case "WithholdingTax":
                                    //    sumWithholdingTax = sumWithholdingTax + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    //    break;
                                    //case "G_YCXJJ":
                                    //    sumYCXJJ = sumYCXJJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    //    break;
                                    //case "K_YCXJJYKS":
                                    //    sumYCXJJYKS = sumYCXJJYKS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    //    break;
                                    //case "T_SE":
                                    //    sumSE = sumSE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    //    break;
                                    //case "K_KS":
                                    //    sumKS = sumKS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    //break;
                                default:
                                    break;
                            }
                            Sheet.GetRow(dateRowNum + i).GetCell(0).SetCellValue(i + 1);//序号列
                            if (col[j] == "D_SL" && dt.Rows[i][col[j]].ToString() != "合计")
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString() + "%");
                            }
                            else { Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString()); }

                        }
                    }
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(7).SetCellValue("￥" + sumYSHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(8).SetCellValue("￥" + sumBQSR.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(26).SetCellValue("￥" + sumDJJE.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(32).SetCellValue("￥" + sumKSHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(33).SetCellValue("￥" + sumLJ_KS.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(10).SetCellValue("￥" + sumYiLiaoBX.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(11).SetCellValue("￥" + sumSYBX.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(12).SetCellValue("￥" + sumZFGJJ.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(13).SetCellValue("￥" + sumQYNJ.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(14).SetCellValue("￥" + sumSFGZ.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(15).SetCellValue("￥" + sumChildEdu.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(16).SetCellValue("￥" + sumHousingLoan.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(17).SetCellValue("￥" + sumHousingRent.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(18).SetCellValue("￥" + sumSupport.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(19).SetCellValue("￥" + sumContinueEdu.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(20).SetCellValue("￥" + sumYSSDE.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(24).SetCellValue("￥" + sumDJJE.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(25).SetCellValue("￥" + sumLJYJS.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(26).SetCellValue("￥" + sumWithholdingTax.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(27).SetCellValue("￥" + sumYCXJJ.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(28).SetCellValue("￥" + sumYCXJJYKS.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(29).SetCellValue("￥" + sumSE.ToString("N2"));
                    //Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(30).SetCellValue("￥" + sumKS.ToString("N2"));
                }
                else if (tempName == "月度累计税务核算报表模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表单位：" + orgName);
                    Sheet.GetRow(2).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                    double sumljYFHJ = 0.00;
                    double sumAccumulatedIncome = 0.00;
                    double sumDrawback = 0.00;
                    double sumK_KS = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < col.Count; j++)
                        {
                            switch (col[j])
                            {
                                case "ljYFHJ":
                                    sumljYFHJ = sumljYFHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "AccumulatedIncome":
                                    sumAccumulatedIncome = sumAccumulatedIncome + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Drawback":
                                    sumDrawback = sumDrawback + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "K_KS":
                                    sumK_KS = sumK_KS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                default:
                                    break;
                            }
                            Sheet.GetRow(dateRowNum + i).GetCell(0).SetCellValue(i + 1);//序号列
                            Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString());



                            if (j > 5)
                            {
                                if (col[j] == "TaxRate" && dt.Rows[i][col[j]].ToString() != "合计")
                                {
                                    Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(Convert.ToDouble(dt.Rows[i][col[j]].ToString()).ToString("N2") + "%");
                                }
                                else
                                {
                                    //Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(Convert.ToDouble(dt.Rows[i][col[j]].ToString()).ToString("N2"));
                                    Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(Math.Round(Convert.ToDouble(dt.Rows[i][col[j]].ToString()), 2));
                                }

                            }
                            else { Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString()); }
                        }

                    }
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(7).SetCellValue("￥" + sumljYFHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(8).SetCellValue("￥" + sumAccumulatedIncome.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(30).SetCellValue("￥" + sumDrawback.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(31).SetCellValue("￥" + sumK_KS.ToString("N2"));
                }
                else if (tempName == "年度平均核算报表模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表单位：" + orgName);
                    Sheet.GetRow(2).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                    double sumYSHJ = 0.00;
                    double sumNJSGZHJ = 0.00;
                    double sumYangLaoBX = 0.00;
                    double sumYiLiaoBX = 0.00;
                    double sumSYBX = 0.00;
                    double sumZFGJJ = 0.00;
                    double sumQYNJ = 0.00;
                    double sumNYNSSDE = 0.00;
                    double sumNYNSE = 0.00;
                    double sumCanJiManShuiHou = 0.00;
                    double sumQian11YuJiao = 0.00;
                    double sumYuJiao12Month = 0.00;
                    double sumErpKS = 0.00;
                    double sumShiJiYJSE = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < col.Count; j++)
                        {
                            switch (col[j])
                            {
                                case "totalT_YFHJ":
                                    sumYSHJ = sumYSHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "NJSGZHJ":
                                    sumNJSGZHJ = sumNJSGZHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "totalK_YangLaoBX":
                                    sumYangLaoBX = sumYangLaoBX + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "totalK_YiLiaoBX":
                                    sumYiLiaoBX = sumYiLiaoBX + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "totalK_SYBX":
                                    sumSYBX = sumSYBX + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "totalK_ZFGJJ":
                                    sumZFGJJ = sumZFGJJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "totalK_QYNJ":
                                    sumQYNJ = sumQYNJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "NYNSSDE":
                                    sumNYNSSDE = sumNYNSSDE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "NYNSE":
                                    sumNYNSE = sumNYNSE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "CanJiManShuiHou":
                                    sumCanJiManShuiHou = sumCanJiManShuiHou + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Qian11YuJiao":
                                    sumQian11YuJiao = sumQian11YuJiao + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "YuJiao12Month":
                                    sumYuJiao12Month = sumYuJiao12Month + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "totalErpKS":
                                    sumErpKS = sumErpKS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ShiJiYJSE":
                                    sumShiJiYJSE = sumShiJiYJSE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                default:
                                    break;
                            }
                            Sheet.GetRow(dateRowNum + i).GetCell(0).SetCellValue(i + 1);//序号列
                            if (col[j] == "ShuiLv")
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString() + "%");
                            }
                            else
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString());
                            }
                        }

                    }
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(8).SetCellValue("￥" + sumYSHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(9).SetCellValue("￥" + sumNJSGZHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(10).SetCellValue("￥" + sumYangLaoBX.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(11).SetCellValue("￥" + sumYiLiaoBX.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(12).SetCellValue("￥" + sumSYBX.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(13).SetCellValue("￥" + sumZFGJJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(14).SetCellValue("￥" + sumQYNJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(15).SetCellValue("￥" + sumNYNSSDE.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(19).SetCellValue("￥" + sumNYNSE.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(20).SetCellValue("￥" + sumCanJiManShuiHou.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(21).SetCellValue("￥" + sumQian11YuJiao.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(22).SetCellValue("￥" + sumYuJiao12Month.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(23).SetCellValue("￥" + sumErpKS.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(24).SetCellValue("￥" + sumShiJiYJSE.ToString("N2"));
                }
                else if (tempName == "正常工资薪金所得") {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = Sheet.CreateRow(1 + i);
                        for (int j = 0; j < col.Count; j++)
                        {
                            if (j >= 4 && j <= 20) {//数字类型列
                                if (dt.Rows[i][col[j]] == null || dt.Rows[i][col[j]].ToString() == "") {
                                    row.CreateCell(j).SetCellValue("");
                                }
                                else
                                {
                                    if (dt.Rows[i][col[j]].ToString().IndexOf('.') == -1)
                                    {
                                        row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                                    }
                                    else {//保留两位小数
                                        row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString().Substring(0, dt.Rows[i][col[j]].ToString().IndexOf('.') + 3));
                                    }
                                }
                            }
                            else {
                                row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                            }

                        }
                    }
                }
                else if (tempName == "全年一次性奖金")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = Sheet.CreateRow(1 + i);
                        for (int j = 0; j < col.Count; j++)
                        {
                            if (j >= 4 && j <= 9)
                            {//数字类型列
                                if (dt.Rows[i][col[j]] == null || dt.Rows[i][col[j]].ToString() == "")
                                {
                                    row.CreateCell(j).SetCellValue("");
                                }
                                else
                                {
                                    if (dt.Rows[i][col[j]].ToString().IndexOf('.') == -1)
                                    {
                                        row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                                    }
                                    else
                                    {//保留两位小数
                                        row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString().Substring(0, dt.Rows[i][col[j]].ToString().IndexOf('.') + 3));
                                    }
                                }
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                            }

                        }
                    }
                }
                else if (tempName == "月度税务核算分组报表模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表单位：" + orgName);
                    Sheet.GetRow(2).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);
                    double sumYFHJ = 0.00;
                    double sumIncome = 0.00;

                    double sumCost = 0.00;
                    double sumTax = 0.00;
                    double sumOlderInsurance = 0.00;
                    double sumHeathInsurance = 0.00;
                    double sumJobInsurance = 0.00;
                    double sumHousingFund = 0.00;
                    double sumEnterpriseAnnuity = 0.00;
                    double sumCommercialHealthinsurance = 0.00;
                    double sumEndowmentInsurance = 0.00;
                    double sumOther = 0.00;
                    double sumCumulativeOther = 0.00;
                    double sumMonth_AccumulatedSpecialDeduction = 0.00;
                    double sumMonth_ChildEdu = 0.00;
                    double sumMonth_Support = 0.00;
                    double sumMonth_ContinueEdu = 0.00;
                    double sumMonth_HousingLoan = 0.00;
                    double sumMonth_HousingRent = 0.00;

                    double sumDrawback = 0.00;
                    double sumSE = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < col.Count; j++)
                        {
                            switch (col[j])
                            {
                                case "T_YFHJ":
                                    sumYFHJ = sumYFHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Income":
                                    sumIncome = sumIncome + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                             case "Cost":
                                    sumCost = sumCost + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Tax":
                                    sumTax = sumTax + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "OlderInsurance":
                                    sumOlderInsurance = sumOlderInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "HeathInsurance":
                                    sumHeathInsurance = sumHeathInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "JobInsurance":
                                    sumJobInsurance = sumJobInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "HousingFund":
                                    sumHousingFund = sumHousingFund + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "EnterpriseAnnuity":
                                    sumEnterpriseAnnuity = sumEnterpriseAnnuity + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "CommercialHealthinsurance":
                                    sumCommercialHealthinsurance = sumCommercialHealthinsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "EndowmentInsurance":
                                    sumEndowmentInsurance = sumEndowmentInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Other":
                                    sumOther = sumOther + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "CumulativeOther":
                                    sumCumulativeOther = sumCumulativeOther + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Month_AccumulatedSpecialDeduction":
                                    sumMonth_AccumulatedSpecialDeduction = sumMonth_AccumulatedSpecialDeduction + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Month_ChildEdu":
                                    sumMonth_ChildEdu = sumMonth_ChildEdu + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Month_Support":
                                    sumMonth_Support = sumMonth_Support + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Month_ContinueEdu":
                                    sumMonth_ContinueEdu = sumMonth_ContinueEdu + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Month_HousingLoan":
                                    sumMonth_HousingLoan = sumMonth_HousingLoan + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Month_HousingRent":
                                    sumMonth_HousingRent = sumMonth_HousingRent + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Drawback":
                                    sumDrawback = sumDrawback + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "lj_SE":
                                    sumSE = sumSE + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                default:
                                    break;
                            }
                            Sheet.GetRow(dateRowNum + i).GetCell(0).SetCellValue(i + 1);//序号列
                            if (j > 3 )
                            {
                                Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(Convert.ToDouble(dt.Rows[i][col[j]].ToString()).ToString("N2"));
                            }
                            else { Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString()); }

                        }
                    }
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(4).SetCellValue("￥" + sumYFHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(5).SetCellValue("￥" + sumIncome.ToString("N2"));

                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(6).SetCellValue("￥" + sumCost.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(7).SetCellValue("￥" + sumTax.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(8).SetCellValue("￥" + sumOlderInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(9).SetCellValue("￥" + sumHeathInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(10).SetCellValue("￥" + sumJobInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(11).SetCellValue("￥" + sumHousingFund.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(12).SetCellValue("￥" + sumEnterpriseAnnuity.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(13).SetCellValue("￥" + sumCommercialHealthinsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(14).SetCellValue("￥" + sumEndowmentInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(15).SetCellValue("￥" + sumOther.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(16).SetCellValue("￥" + sumCumulativeOther.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(17).SetCellValue("￥" + sumMonth_AccumulatedSpecialDeduction.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(18).SetCellValue("￥" + sumMonth_ChildEdu.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(19).SetCellValue("￥" + sumMonth_Support.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(20).SetCellValue("￥" + sumMonth_ContinueEdu.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(21).SetCellValue("￥" + sumMonth_HousingLoan.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(22).SetCellValue("￥" + sumMonth_HousingRent.ToString("N2"));



                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(23).SetCellValue("￥" + sumDrawback.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(24).SetCellValue("￥" + sumSE.ToString("N2"));
                }
                else if (tempName == "月度累计税务核算分组报表模板")
                {
                    Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    Sheet.GetRow(1).GetCell(0).SetCellValue("制表单位：" + orgName);
                    Sheet.GetRow(2).GetCell(0).SetCellValue("制表时间：" + DateTime.Now.ToString("yyyy-MM-dd"));
                    IRow mySourceRow = Sheet.GetRow(dateRowNum);
                    InsertRow(Sheet, dateRowNum + 1, dt.Rows.Count - 1, mySourceRow);

                    double sumYFHJ = 0.00;
                    double sumIncome = 0.00;
                    double sumljOlderInsurance = 0.00;
                    double sumljHeathInsurance = 0.00;
                    double sumljJobInsurance = 0.00;
                    double sumljHousingFund = 0.00;
                    double sumljEnterpriseAnnuity = 0.00;
                    double sumDonation = 0.00;
                    double sumDeductions = 0.00;
                    double sumTaxSavings = 0.00;
                    double sumAccumulatedSpecialDeduction = 0.00;
                    double sumCumulativeOther = 0.00;
                    double sumChildEdu = 0.00;
                    double sumHousingLoan = 0.00;
                    double sumHousingRent = 0.00;
                    double sumSupport = 0.00;
                    double sumContinueEdu = 0.00;
                    double sumAccumulatedTax = 0.00;
                    double sumCumulativeWithholding = 0.00;
                    double sumWithholdingTax = 0.00;
                    double sumDrawback = 0.00;
                    double sumK_KS = 0.00;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < col.Count; j++)
                        {
                            switch (col[j])
                            {
                                case "ljYFHJ":
                                    sumYFHJ = sumYFHJ + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "AccumulatedIncome":
                                    sumIncome = sumIncome + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;

                                case "ljOlderInsurance":
                                    sumljOlderInsurance = sumljOlderInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ljHeathInsurance":
                                    sumljHeathInsurance = sumljHeathInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ljJobInsurance":
                                    sumljJobInsurance = sumljJobInsurance + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ljHousingFund":
                                    sumljHousingFund = sumljHousingFund + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ljEnterpriseAnnuity":
                                    sumljEnterpriseAnnuity = sumljEnterpriseAnnuity + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Donation":
                                    sumDonation = sumDonation + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Deductions":
                                    sumDeductions = sumDeductions + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "TaxSavings":
                                    sumTaxSavings = sumTaxSavings + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "AccumulatedSpecialDeduction":
                                    sumAccumulatedSpecialDeduction = sumAccumulatedSpecialDeduction + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "CumulativeOther":
                                    sumCumulativeOther = sumCumulativeOther + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ChildEdu":
                                    sumChildEdu = sumChildEdu + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "HousingLoan":
                                    sumHousingLoan = sumHousingLoan + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "HousingRent":
                                    sumHousingRent = sumHousingRent + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Support":
                                    sumSupport = sumSupport + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "ContinueEdu":
                                    sumContinueEdu = sumContinueEdu + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "AccumulatedTax":
                                    sumAccumulatedTax = sumAccumulatedTax + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "CumulativeWithholding":
                                    sumCumulativeWithholding = sumCumulativeWithholding + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "WithholdingTax":
                                    sumWithholdingTax = sumWithholdingTax + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "K_KS":
                                    sumK_KS = sumK_KS + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                case "Drawback":
                                    sumDrawback = sumDrawback + Convert.ToDouble(dt.Rows[i][col[j]].ToString());
                                    break;
                                default:
                                    break;
                            }
                            Sheet.GetRow(dateRowNum + i).GetCell(0).SetCellValue(i + 1);//序号列
                            if (j > 3)
                            {
                               Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(Convert.ToDouble(dt.Rows[i][col[j]].ToString()).ToString("N2")); 
                            }
                            else { Sheet.GetRow(dateRowNum + i).GetCell(j + 1).SetCellValue(dt.Rows[i][col[j]].ToString()); }
                        }
                    }

                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(3).SetCellValue("￥" + sumYFHJ.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(4).SetCellValue("￥" + sumIncome.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(5).SetCellValue("￥" + sumljOlderInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(6).SetCellValue("￥" + sumljHeathInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(7).SetCellValue("￥" + sumljJobInsurance.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(8).SetCellValue("￥" + sumljHousingFund.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(9).SetCellValue("￥" + sumljEnterpriseAnnuity.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(10).SetCellValue("￥" + sumDonation.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(11).SetCellValue("￥" + sumDeductions.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(12).SetCellValue("￥" + sumTaxSavings.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(13).SetCellValue("￥" + sumAccumulatedSpecialDeduction.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(14).SetCellValue("￥" + sumCumulativeOther.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(15).SetCellValue("￥" + sumChildEdu.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(16).SetCellValue("￥" + sumHousingLoan.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(17).SetCellValue("￥" + sumHousingRent.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(18).SetCellValue("￥" + sumSupport.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(19).SetCellValue("￥" + sumContinueEdu.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(20).SetCellValue("￥" + sumAccumulatedTax.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(21).SetCellValue("￥" + sumCumulativeWithholding.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(22).SetCellValue("￥" + sumWithholdingTax.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(23).SetCellValue("￥" + sumDrawback.ToString("N2"));
                    Sheet.GetRow(dt.Rows.Count + dateRowNum).GetCell(24).SetCellValue("￥" + sumK_KS.ToString("N2"));
                }
                else if (tempName == "劳务报酬明细表")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = Sheet.CreateRow(1 + i);
                        for (int j = 0; j < col.Count; j++)
                        {
                            if (j >= 5 && j <= 11)
                            {//数字类型列
                                if (dt.Rows[i][col[j]] == null || dt.Rows[i][col[j]].ToString() == "")
                                {
                                    row.CreateCell(j).SetCellValue("");
                                }
                                else
                                {
                                    if (dt.Rows[i][col[j]].ToString().IndexOf('.') == -1)
                                    {
                                        row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString());
                                    }
                                    else
                                    {//保留两位小数
                                        row.CreateCell(j).SetCellValue(dt.Rows[i][col[j]].ToString().Substring(0, dt.Rows[i][col[j]].ToString().IndexOf('.') + 3));
                                    }
                                }
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
                string fileName = title.Replace("/", "") + ".xls";
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

    }
}
