using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
   public class YearAverCheckModule
    {
        UIDP.ODS.YearAverCheckDB db = new ODS.YearAverCheckDB();
        /// <summary>
        /// 全年平均核算
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getYearAverCheck(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.getYearAverCheck(d);
                if (ds!=null&& ds.Tables.Count> 0)
                {
                    r["total"] = ds.Tables.Count==2?ds.Tables[1].Rows[0]["total"]:0;
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

        public Dictionary<string, object> ExportYearAverTax(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                DataSet ds = db.getYearAverCheck(d);
                if (ds != null && ds.Tables.Count > 0)
                {
                    string str = d["S_OrgName"].ToString() + d["S_WorkDate"].ToString().Substring(0, 4) + "年";
                    if (d["S_Department"] != null && !string.IsNullOrEmpty(d["S_Department"].ToString()))
                    { str = str + d["S_Department"].ToString(); }
                    string title = str + "按单位个税汇总（全年平均核算）";
                    List<string> col = new List<string>() {
                        "S_WorkerCode","S_WorkerName","IdNumber","S_OrgName","S_Department",
                        "WorkerStatus","dateYear","totalT_YFHJ","NJSGZHJ","totalK_YangLaoBX","totalK_YiLiaoBX","totalK_SYBX",
                        "totalK_ZFGJJ","totalK_QYNJ","NYNSSDE","FYKCBZHJ","ShuiLv","TaxDeduction","NYNSE","CanJiManShuiHou",
                        "Qian11YuJiao","YuJiao12Month","totalErpKS","ShiJiYJSE"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(ds.Tables[0], title, d["S_OrgName"].ToString(), "年度平均核算报表模板", 4, 24, col);
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
