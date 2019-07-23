using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class TaxReportDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 上报
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string Report(Dictionary<string,object> d)
        {
            string sql = "UPDATE tax_reportstatus SET ReportStatus=2,";
            sql += "S_UpdateBy='" + d["S_UpdateBy"].ToString()+"',";
            sql += "S_UpdateDate='" + d["S_UpdateDate"].ToString() + "'";
            sql += " WHERE S_OrgCode='" + d["S_OrgCode"].ToString() + "'";
            sql += " AND  DATEDIFF(m, S_WorkDate, '" + d["S_WorkDate"] + "') = 0";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 获取本部门是否为无人员的二级部门
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable IsNullDepartment(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM tax_taxpayerinfo WHERE S_OrgCode='" + d["S_OrgCode"] + "'";
            return db.GetDataTable(sql);
        }

        public DataTable IsNull(string OrgCode)
        {
            string sql = "SELECT * FROM tax_taxpayerinfo WHERE S_OrgCode='" + OrgCode + "'";
            return db.GetDataTable(sql);
        }

        public DataTable IsNullRecord(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM tax_reportstatus WHERE S_OrgCode='" + d["S_OrgCode"] + "'";
            //sql += " AND S_WorkDate='" + d["S_WorkDate"] + "'";
            sql += " AND  DATEDIFF(m, S_WorkDate, '" + d["S_WorkDate"] + "') = 0 Order by S_CreateDate desc ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 当本部为二级部门且没有任何人员时，直接插入上报状态记录
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>

        public string InsertRecord(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate)values('";
            sql += d["S_Id"] + "','";
            sql += d["S_UpdateDate"] + "','";
            sql += d["S_UpdateBy"] + "','";
            sql += d["S_OrgName"] + "','";
            sql += d["S_OrgCode"] + "',";
            sql += 2 + ",'";
            sql += d["S_WorkDate"] + "')";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 获取下级是否已经上报
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getSubordinateInfo(Dictionary<string,object> d)
        {
            string sql = "SELECT ORG_CODE,ORG_NAME,b.ReportStatus FROM ts_uidp_org a LEFT JOIN tax_reportstatus b ON a.ORG_CODE=b.S_OrgCode WHERE";
            sql += " a.ORG_CODE LIKE'" + d["S_OrgCode"] + "%'";
            sql += " AND a.ORG_CODE!='" + d["S_OrgCode"] + "'";
            sql += " AND DATEDIFF( mm, b.S_WorkDate, '" + d["S_WorkDate"].ToString() + "' ) = 0 ";
            sql += " AND (b.ReportStatus!= 2 ";
            sql += " OR b.ReportStatus IS NULL)";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询当前单位上报状态
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <returns></returns>
        public DataTable getStatus(string S_OrgCode,string S_WorkDate)
        {
            string sql = "SELECT * FROM tax_reportstatus WHERE";
            sql += " S_OrgCode='" + S_OrgCode + "'";
            //sql += "AND S_WorkDate='" + S_WorkDate + "'";
            sql += " AND  DATEDIFF(m, S_WorkDate, '" + S_WorkDate + "') = 0";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询缴税人数
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="WorkMonth"></param>
        /// <returns></returns>
        public DataSet GetData(string OrgCode, DateTime WorkMonth)
        {
            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            string sql = "select a.ORG_ID,a.ORG_NAME,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,CASE WHEN((b.IsComputeTax = 0 OR b.IsComputeTax IS NULL) and c.ReportStatus is null ) THEN '1' ELSE isnull(c.ReportStatus,-1) END ReportStatus ";
            sql += "  from ts_uidp_org a ";
            sql += "   join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE='" + OrgCode + "' ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            sql += " union  ";
            sql += " select a.ORG_ID,a.ORG_NAME,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,ISNULL(c.ReportStatus,-1) ReportStatus   ";
            sql += " from ts_uidp_org a ";
            sql += "  join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE like '" + OrgCode + "'+'%' AND a.ORG_CODE<>'" + OrgCode + "' ";
            sql += " AND LEN(a.ORG_CODE)=LEN('"+OrgCode+"')+3";
           // sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";//以上是查部门
            sqllist.Add("taxorg", sql);
            sql = " select count(1) TaxPayerCount ";//一下是查缴纳人数
            sql += " from tax_salary a  ";
            sql += " where a.S_OrgCode in ( ";
            sql += " select a.ORG_CODE ";
            sql += "  from ts_uidp_org a ";
            sql += "   join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE='" + OrgCode + "' ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            sql += " union  ";
            sql += " select a.ORG_CODE ";
            sql += " from ts_uidp_org a ";
            sql += " left join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE like '" + OrgCode + "'+'%' AND a.ORG_CODE<>'" + OrgCode + "' ";
           // sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";
            sql += " ) and DATEDIFF(m,a.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sqllist.Add("taxpayercount", sql);
            return db.GetDataSet(sqllist);
        }
        /// <summary>
        /// 查询下一级部门的状态
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getDepartmentStatus(Dictionary<string,object> d)
        {
            string sql = "";
            //string sql = "select isnull(c.S_Id,'') S_Id,isnull(c.S_WorkDate,'')S_WorkDate,a.ORG_ID,a.ORG_NAME S_OrgName,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,ISNULL(c.ReportStatus,-1) ReportStatus  ";
            //sql += "  from ts_uidp_org a ";
            //sql += "  left join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            //sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + DateTime.Parse(d["S_WorkDate"].ToString()).ToString("yyyy-MM-dd") + "')=0 ";
            //sql += " where a.ORG_CODE='" + d["S_OrgCode"].ToString() + "' and len(a.ORG_CODE)>3 ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            //if (d["ReportStatus"] != null)
            //{
            //    sql += "AND c.ReportStatus=" + d["ReportStatus"];
            //}
            //sql += " union  ";
            sql += " select * from(  ";
            sql += " select  isnull(c.S_Id,'') S_Id,isnull(c.S_WorkDate,'"+ d["S_WorkDate"].ToString() + "')S_WorkDate,a.ORG_ID,a.ORG_NAME S_OrgName,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,CASE WHEN((b.IsComputeTax = 0 OR b.IsComputeTax IS NULL) and c.ReportStatus is null ) THEN '1' ELSE isnull(c.ReportStatus,-1) END ReportStatus  ";
            sql += " from ts_uidp_org a ";
            sql += " left join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + DateTime.Parse(d["S_WorkDate"].ToString()).ToString("yyyy-MM-dd") + "')=0 ";
            sql += " ) tab ";
            sql += " where ORG_CODE like '" + d["S_OrgCode"].ToString() + "'+'%' AND ORG_CODE<>'" + d["S_OrgCode"].ToString() + "' and LEN(ORG_CODE)=LEN('" + d["S_OrgCode"].ToString() + "')+3 ";
            //sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
           // sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";//以上是查部门
            if (d["ReportStatus"] != null)
            {
                sql += "AND ReportStatus=" + d["ReportStatus"];
            }
            sql += " order by  ORG_CODE";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 解锁上报功能
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string unlock(Dictionary<string,object> d)
        {
            string sql = "UPDATE tax_reportstatus SET ReportStatus=1 WHERE S_Id='" + d["S_Id"].ToString() + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 锁定上报功能
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string locking(Dictionary<string, object> d)
        {
            string sql = "UPDATE tax_reportstatus SET ReportStatus=2 WHERE S_Id='" + d["S_Id"].ToString() + "'";
            return db.ExecutByStringResult(sql);
        }

        /// <summary>
        /// 返回公司及下属上报状态
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <returns></returns>
        public DataTable AllowReport(string OrgCode,string S_WorkDate)
        {
            string sql = @"select * from(
SELECT
	isnull( c.S_Id, '' ) S_Id,
	isnull( c.S_WorkDate, '" + S_WorkDate + "') S_WorkDate,"
	+@"a.ORG_ID,
	a.ORG_NAME S_OrgName,
	a.ORG_CODE,
	isnull( b.IsComputeTax, 0 ) IsComputeTax,
CASE
		WHEN (( b.IsComputeTax = 0 OR b.IsComputeTax IS NULL ) AND c.ReportStatus IS NULL ) THEN
		'1' ELSE isnull( c.ReportStatus,- 1 ) 
	END ReportStatus 
FROM
	ts_uidp_org a
	LEFT JOIN tax_org b ON a.ORG_CODE= b.S_OrgCode
	LEFT JOIN tax_reportstatus c ON c.S_OrgCode= a.ORG_CODE 
	AND DATEDIFF( m, c.S_WorkDate, '"+ S_WorkDate + "' ) = 0 "
    + @") tab
WHERE
	(ORG_CODE = '" + OrgCode + "'"
	+@"or 	(ORG_CODE LIKE '"+ OrgCode + "' + '%' and LEN( ORG_CODE ) = LEN( '"+ OrgCode + "' ) + 3 ) )   ORDER BY ORG_CODE";
            return db.GetDataTable(sql);
        }
    }
}
