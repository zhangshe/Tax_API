using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class MonthTaxExportModule
    {
        #region Excel定义的变量
        private static IWorkbook Workbook { get; set; }
        private static ISheet Sheet { get; set; }
        #endregion
        UIDP.ODS.MonthTaxExportDB db = new ODS.MonthTaxExportDB();
        /// <summary>
        /// 按月地税查询和导出
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getMonthTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.getMonthTax(d);
                if (ds != null && ds.Tables.Count > 0)
                {
                    r["total"] = ds.Tables.Count == 2 ? ds.Tables[1].Rows[0]["total"] : 0;
                    r["items"] = ds.Tables[0];
                    r["message"] = "成功";
                    r["code"] = 2000;
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
        /// <summary>
        /// 全年地税查询和导出
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getYearTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.getYearTax(d);
                if (ds != null && ds.Tables.Count > 0)
                {
                    r["total"] = ds.Tables.Count == 2 ? ds.Tables[1].Rows[0]["total"] : 0;
                    r["items"] = ds.Tables[0];
                    r["message"] = "成功";
                    r["code"] = 2000;
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
        }

        #region NPOI插入数据行
        public void InsertRow(ISheet sheet, int insertRowIndex, int insertRowCount, IRow formatRow)
        {
            sheet.ShiftRows(insertRowIndex, sheet.LastRowNum+ insertRowCount, insertRowCount, true, false);
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
        public Dictionary<string, object> ExportMonthTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                string filePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\Templates\\月度税务报表模板.xls";//原始文件
                //Init(filePath);//初始化数据
                DataSet ds = db.getMonthTax(d);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0,7);
                    if (d["S_Department"]!=null&&!string.IsNullOrEmpty(d["S_Department"].ToString()))
                    { str = str + d["S_Department"].ToString(); }
                    string title = str+ "应交地税税费报表";
                    //Sheet.GetRow(0).GetCell(0).SetCellValue(title);
                    //Sheet.GetRow(1).GetCell(0).SetCellValue("制表时间："+DateTime.Now.ToString("yyyy-MM-dd"));
                    //IRow mySourceRow = Sheet.GetRow(4);
                    //InsertRow(Sheet, 5, ds.Tables[0].Rows.Count - 1, mySourceRow);
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    Sheet.GetRow(4 + i).GetCell(0).SetCellValue(ds.Tables[0].Rows[i]["D_SL"].ToString());
                    //    Sheet.GetRow(4 + i).GetCell(1).SetCellValue(ds.Tables[0].Rows[i]["total2"].ToString());
                    //    Sheet.GetRow(4 + i).GetCell(2).SetCellValue(ds.Tables[0].Rows[i]["totalJS2"].ToString());
                    //    Sheet.GetRow(4 + i).GetCell(3).SetCellValue(ds.Tables[0].Rows[i]["totalDJJE2"].ToString());
                    //    Sheet.GetRow(4 + i).GetCell(4).SetCellValue(ds.Tables[0].Rows[i]["total"].ToString());
                    //    Sheet.GetRow(4 + i).GetCell(5).SetCellValue(ds.Tables[0].Rows[i]["totalJS"].ToString());
                    //    Sheet.GetRow(4 + i).GetCell(6).SetCellValue(ds.Tables[0].Rows[i]["totalDJJE"].ToString());
                    //}

                    ////结尾
                    //Sheet.ForceFormulaRecalculation = true;
                    ////导出文件  
                    //string fileName = title+".xls";
                    //string path = System.IO.Directory.GetCurrentDirectory() + "\\Files\\export\\" + fileName;
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}
                    //FileStream Reportfile = new FileStream(path, FileMode.Create);
                    //Workbook.Write(Reportfile);
                    //Reportfile.Close();
                    List<string> col =new List<string>() {
                        "D_SL","total2","totalJS2","totalDJJE2","total","totalJS","totalDJJE"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(ds.Tables[0], title, d["S_OrgName"].ToString(), "月度税务报表模板",4,6, col);
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

        public Dictionary<string, object> ExportYearTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                DataSet ds = db.getYearTax(d);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0,4)+"年";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    { str = str + d["S_Department"].ToString(); }
                    string title = str + "应交地税税费报表";
                    List<string> col = new List<string>() {
                        "D_SL","total","totalJS","totalDJJE"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(ds.Tables[0], title, d["S_OrgName"].ToString(), "年度税务报表模板", 4, 4, col);
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
    }
}
