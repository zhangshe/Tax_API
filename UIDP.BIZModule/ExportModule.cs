using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
     public class ExportModule
    {
        ExportDB db=new ExportDB();
        /// <summary>
        /// 获取当月金税部门纳税人信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string,object> getExportPayerInfo(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.getExportPayerInfo(d);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = dt;
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
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        /// <summary>
        /// 获取当月金税部门工资详情
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getExportSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.getExportSalary(d);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = dt;
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
        /// 金税导出工资
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> ExportSalary(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                DataTable dt = db.getExportSalary(d);
                if (dt != null)
                {
                    List<string> col = new List<string>() {
                        "S_WorkerCode","S_WorkerName","Name","IdNumber","SRE","MSSD",
                        "K_YangLaoBX","K_YiLiaoBX","K_SYBX","K_ZFGJJ","K_LJZVJY","K_LJJXJY","K_LJDKLX","K_LJZFZJ","K_LJSYLR","K_QYNJ",
                        "SYJKBXF","SYYLBXF","K_QTKX","ZYKCDJZE",
                        "JMSE","Remark"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, "正常工资薪金所得", "", "正常工资薪金所得", 1, 22, col);
                    r["code"] = 2000;
                    r["message"] = "";
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
        /// 获取当月金税部门一次性奖金详情
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getExportOnceBonus(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.getExportOnceBonus(d);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = dt;
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
        /// 金税导出一次性奖金
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> ExportOnceBonus(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                DataTable dt = db.getExportOnceBonus(d);
                if (dt != null)
                {
                    List<string> col = new List<string>() {
                        "S_WorkerCode","S_WorkerName","IdTypeName","IdNumber","OneTimeBonus","FreeIncome",
                        "Other","AllowDeduction","TaxSaving","DeductibleTax","Remark"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, "全年一次性奖金", "", "全年一次性奖金", 1, 11, col);
                    r["code"] = 2000;
                    r["message"] = "";
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
            return r;
        }
        
        /// <summary>
        /// 获取单位上报状态
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getOrgStatus(Dictionary<string, object> d)
        {
            int page = int.Parse(d["page"].ToString());
            int limit = int.Parse(d["limit"].ToString());
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getOrgStatus(d);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "ReportStatus ASC,S_OrgCode";
                    dt = dv.ToTable();
                }
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
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
        ///获取金税部门税号
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>

        public Dictionary<string, object> getTaxNumberOptions(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = db.getTaxNumberOptions(d);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = dt;
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
    }
}
