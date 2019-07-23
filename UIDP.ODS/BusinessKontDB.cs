using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class BusinessKontDB
    {
        DBTool DB = new DBTool("MYSQL");
        /// <summary>
        /// 结转功能
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string MonthKonts(Dictionary<string,object> d)
        {
            string sql = "UPDATE tax_date SET UpdateDate='" + d["UpdateDate"].ToString()+"',";
            sql += "UpdateBy='" + d["UpdateBy"].ToString() +"',";
            sql += "SysOperateDate=DATEADD(mm,1,'" + d["SysOperateDate"].ToString() + "')";
            //sql += " insert into View_GouXuanHistry SELECT '" + d["SysOperateDate"].ToString() + "',* from [dbo].[View_GouXuan]";
            //sql += "insert into View_GouXuanBLHistry SELECT '" + d["SysOperateDate"].ToString() + "',* from [dbo].[View_GouXuanBL]";
            //sql += "insert into View_KouJianXiangHistry SELECT'" + d["SysOperateDate"].ToString() + "',* from [dbo].[View_KouJianXiang]";
            return DB.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 获取当月下级上报情况
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable KontsStatus(Dictionary<string,object> d)
        {
            //string sql = "SELECT * FROM tax_reportstatus";
            //sql += " WHERE S_OrgCode LIKE'" + d["OrgCode"] + "%'";
            //sql += " AND LEN(S_OrgCode)>LEN('" + d["OrgCode"] + "')";
            //sql += " AND DATEDIFF(mm, S_WorkDate, '"+d["SysOperateDate"].ToString()+"')=0";
            //sql += "  AND ReportStatus!=2";
            //string sql = "SELECT ORG_CODE,ORG_NAME,b.ReportStatus FROM ts_uidp_org a LEFT JOIN tax_reportstatus b ON a.ORG_CODE=b.S_OrgCode WHERE";
            //sql += " a.ORG_CODE LIKE'" + d["OrgCode"] + "%'";
            //sql += " AND DATEDIFF( mm, b.S_WorkDate, '" + d["SysOperateDate"].ToString() + "' ) = 0 ";
            //sql += " AND b.ReportStatus!= 2 ";
            //sql += " OR b.ReportStatus IS NULL";
            string sql = "SELECT * FROM tax_org a";
            sql += " LEFT JOIN tax_reportstatus b ON a.S_OrgCode=b.S_OrgCode";
            sql += " WHERE 1=1";
            sql += " AND DATEDIFF(mm, b.S_WorkDate,'";
            sql += d["SysOperateDate"] + "')=0";
            sql += " AND b.S_OrgCode LIKE '" + d["OrgCode"] + "%'";
            sql += " AND LEN(b.S_OrgCode)=6";
            sql += " AND b.ReportStatus!=2";
            return DB.GetDataTable(sql);
        }


        public DataSet GetData(string OrgCode, DateTime WorkMonth)
        {
            Dictionary<string, string> sqllist = new Dictionary<string, string>();

            string sql="";
            //string sql = "select a.ORG_ID,a.ORG_NAME,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,ISNULL(c.ReportStatus,-1) ReportStatus ";
            //sql += "  from ts_uidp_org a ";
            //sql += "  left join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            //sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            //sql += " where a.ORG_CODE='" + OrgCode + "' ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //sql += " union  ";
            sql += " select a.ORG_ID,a.ORG_NAME,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,CASE WHEN((b.IsComputeTax = 0 OR b.IsComputeTax IS NULL) and c.ReportStatus is null ) THEN '1' ELSE isnull(c.ReportStatus,-1) END ReportStatus   ";
            sql += " from ts_uidp_org a ";
            sql += "  join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE like '" + OrgCode + "'+'%' AND a.ORG_CODE<>'" + OrgCode + "' and len(a.ORG_CODE)="+(OrgCode.Length+3)+" ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";//以上是查部门
            sqllist.Add("taxorg", sql);
            sql = " select count(1) TaxPayerCount ";//一下是查缴纳人数
            sql += " from tax_taxpayerinfo a  ";
            sql += " where a.S_OrgCode in ( ";
            //sql += " select a.ORG_CODE ";
            //sql += "  from ts_uidp_org a ";
            //sql += "  left join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            //sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            //sql += " where a.ORG_CODE='" + OrgCode + "' ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //sql += " union  ";
            sql += " select a.ORG_CODE ";
            sql += " from ts_uidp_org a ";
            sql += " join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE like '" + OrgCode + "'+'%' AND a.ORG_CODE<>'" + OrgCode + "' and len(a.ORG_CODE)=" + (OrgCode.Length + 3) + " ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";
            sql += " ) ";
            sqllist.Add("taxpayercount", sql);
            return DB.GetDataSet(sqllist);
        }
        /// <summary>
        /// 判断金税导入明细的信息和工资信息是否匹配
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable judgeStauts(DateTime SysOperateDate)
        {
            string sql = "SELECT a.S_WorkerCode,a.S_WorkerName,b.ORG_NAME FROM tax_salary a ";
            sql += "LEFT JOIN ts_uidp_org b ON a.S_OrgCode= b.ORG_CODE ";
            sql += " WHERE S_WorkDate='" + SysOperateDate.ToString("yyyyMMdd") + "'";
            //sql += " AND S_WorkerCode NOT IN (SELECT S_WorkNumber FROM tax_specialdeductions WHERE S_WorkDate='" + SysOperateDate.ToString("yyyyMMdd") + "')";
            sql += " AND NOT EXISTS ( SELECT 1 FROM tax_specialdeductions WHERE S_WorkNumber = S_WorkerCode AND S_WorkDate = '" + SysOperateDate.ToString("yyyyMMdd") + "')";
            sql += " ORDER BY a.S_OrgCode";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
            return DB.GetDataTable(sql);
        }

        public DataTable getJudgeCount(DateTime SysOperateDate)
        {
            string sql = " SELECT COUNT(*) AS num FROM ";
            sql+= "(SELECT a.S_WorkerCode,a.S_WorkerName,b.ORG_NAME FROM tax_salary a ";
            sql += "LEFT JOIN ts_uidp_org b ON a.S_OrgCode= b.ORG_CODE ";
            sql += " WHERE S_WorkDate='" + SysOperateDate.ToString("yyyyMMdd") + "'";
            //sql += " AND S_WorkerCode NOT IN (SELECT S_WorkNumber FROM tax_specialdeductions WHERE S_WorkDate='" + SysOperateDate.ToString("yyyyMMdd") + "')";
            sql += " AND NOT EXISTS ( SELECT 1 FROM tax_specialdeductions WHERE S_WorkNumber = S_WorkerCode AND S_WorkDate = '" + SysOperateDate.ToString("yyyyMMdd") + "')) a";
            return DB.GetDataTable(sql);
        }
    }
}
