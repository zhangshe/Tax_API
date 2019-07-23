using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;

namespace UIDP.BIZModule
{
    public class BusinessKontModule
    {
        BusinessKontDB DB = new BusinessKontDB();
        public Dictionary<string,object> MonthKonts(Dictionary<string,object> d)
        {
            d["UpdateDate"] = DateTime.Now;
            Dictionary<string, object> r = new Dictionary<string, object>();
            //DataTable result = DB.judgeStauts(Convert.ToDateTime(d["SysOperateDate"]));//判断工资信息和金税明细信息是否匹配
            DataTable dt = new DataTable();
            if (dt.Rows.Count== 0)
            {
                string b = DB.MonthKonts(d);
                try
                {
                    if (b == "")
                    {
                        r["message"] = "成功";
                        r["code"] = 2000;
                    }
                    else
                    {
                        r["message"] = "失败";
                        r["code"] = -1;
                    }
                }
                catch (Exception e)
                {
                    r["message"] = e.Message;
                    r["code"] = -1;
                }
            }
            else
            {
                r["message"] = "请确认金税明细导入情况！";
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> getCalculateData(string OrgCode, DateTime SysOperateDate)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            DataSet ds =DB.GetData(OrgCode, SysOperateDate);
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
            else
            {
                res["TaxStatus"] = -1;
                res["items"] = new DataTable();
                res["TaxPayerCount"] = 0;
                res["total"] = 0;
                res["message"] = "成功";
                res["code"] = 2000;
            }
            return res;
        }

        public Dictionary<string,object> judgeStatus(DateTime SysOperateDate)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = DB.judgeStauts(SysOperateDate);
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["status"] = 0;
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["message"] = "还有部门未导入金税明细";
                }
                else
                {
                    r["status"] = 1;
                    r["code"] = 2000;
                    r["message"] = "全部导入，可以结转";
                }
            }
            catch(Exception e)
            {
                r["status"] = -1;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }


        public Dictionary<string, object> getJudgeCount(DateTime SysOperateDate)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = DB.getJudgeCount(SysOperateDate);
                r["code"] = 2000;
                r["items"] = dt;
            }
            catch (Exception e)
            {
                r["status"] = -1;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
    }
}
