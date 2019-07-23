using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace UIDP.ODS
{
   public class TaxAdjustDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询调整表信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataSet getTaxAdjustList(Dictionary<string, object> d,int page,int limit)
        {
            //string sql = @"select * from tax_adjust where 1=1 ";
            ////if (d.Keys.Contains("S_OrgName") && d["S_OrgName"] != null && d["S_OrgName"].ToString() != "")
            ////{
            ////    sql += " and S_OrgName like '%" + d["S_OrgName"].ToString() + "%'";
            ////}
            //if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            //{
            //    sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString()+"%'";
            //}
            //if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            //{
            //    DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());

            //    sql += " AND DATEDIFF(mm, S_WorkDate, '";
            //    sql += bdate.ToString("yyyyMMdd") + "')=0";
            //}
            //if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            //{
            //    sql += " and S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            //}
            ////if (d["BEGIN_ACCESS_TIME"] != null && d["BEGIN_ACCESS_TIME"].ToString() != "" && (d["END_ACCESS_TIME"] == null || d["END_ACCESS_TIME"].ToString() == ""))
            ////{
            ////    DateTime date = Convert.ToDateTime(d["BEGIN_ACCESS_TIME"].ToString());
            ////    sql += " and ACCESS_TIME between '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00' and '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";
            ////}
            ////else if (d["END_ACCESS_TIME"] != null && d["END_ACCESS_TIME"].ToString() != "" && (d["BEGIN_ACCESS_TIME"] == null || d["BEGIN_ACCESS_TIME"].ToString() == ""))
            ////{
            ////    DateTime date = Convert.ToDateTime(d["END_ACCESS_TIME"].ToString());
            ////    sql += " and ACCESS_TIME < '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";
            ////}
            ////else if (d["BEGIN_ACCESS_TIME"] != null && d["BEGIN_ACCESS_TIME"].ToString() != "" && d["END_ACCESS_TIME"] != null && d["END_ACCESS_TIME"].ToString() != "")
            ////{
            ////    DateTime bdate = Convert.ToDateTime(d["BEGIN_ACCESS_TIME"].ToString());
            ////    DateTime edate = Convert.ToDateTime(d["END_ACCESS_TIME"].ToString());
            ////    sql += " and ACCESS_TIME between '"+bdate.Year+"-"+bdate.Month+"-"+bdate.Day+" 00:00:00' and '" + edate.Year + "-" + edate.Month + "-" + edate.Day + " 23:59:59'" ;
            ////}
            //sql += " order by S_WorkDate desc , S_WorkerCode";
            //return db.GetDataTable(sql);


            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            string sql = " SELECT ROW_NUMBER () OVER ( ORDER BY S_WorkerCode ) AS non,a.* INTO #temp FROM tax_adjust a ";
            sql += " WHERE 1=1";
       
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
                sql += " AND DATEDIFF(mm, S_WorkDate, '";
                sql += bdate.ToString("yyyyMMdd") + "')=0";
            }
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " and S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            }
            if (page == 0 && limit == 0)
            {
                sql += " SELECT * FROM #temp  order by S_WorkDate desc ";
            }
            else {
                sql += " SELECT * FROM #temp WHERE non between " + ((page - 1) * limit + 1) + " AND " + page * limit + "order by S_WorkDate desc ";
            }
            string sql1 = " SELECT COUNT(*) FROM #temp";
            sqllist.Add("data", sql);
            sqllist.Add("total", sql1);
            return db.GetDataSet(sqllist);
        }


        public string createTaxAdjust(List<ImportTaxAdjust> d, DateTime dateMonth, string userId,string orgCode,string orgName)
        {
            List<string> sqllst = new List<string>();
            string batch = Guid.NewGuid().ToString();
            string fengefu = "";
            int i = 0;//循环数，用于判断添加SQL语句头部；
            const int Num = 600;//Insert插入分条数目
            string headersql = @" insert into tax_adjust (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,Adjust9,Adjust10,Adjust11,Adjust12,Adjust13,Adjust14,Adjust15,Adjust16,Adjust17,Adjust18,Adjust19,Adjust20,Adjust21,Adjust22,Adjust23,Adjust24,Adjust25,Adjust26,Adjust27,Adjust28,S_Remark,S_Batch,S_WorkDate) values ";
            StringBuilder sb = new StringBuilder();
            //StringBuilder updatesb = new StringBuilder();
            //           string delAdjust = @"delete from tax_adjust 
            //where S_OrgCode in (select S_OrgCode from tax_org where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and ((S_OrgCode like '" + orgCode + "%' and IsComputeTax = 0) or S_OrgCode = '"+ orgCode + "'))";
            //sb.Append(@" insert into tax_adjust (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,Adjust9,Adjust10,Adjust11,Adjust12,Adjust13,Adjust14,Adjust15,Adjust16,Adjust17,Adjust18,Adjust19,Adjust20,Adjust21,Adjust22,Adjust23,Adjust24,Adjust25,Adjust26,Adjust27,Adjust28,S_Remark,S_WorkDate) values ");

            foreach (ImportTaxAdjust item in d)
            {
                if (i % Num == 0)
                {
                    sb.Append(headersql);
                }
                sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
                sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
                sb.Append("'" + userId + "',");
                sb.Append("'" + item.S_WorkerCode + "',");
                sb.Append("'" + item.S_WorkerName + "',");
                //sb.Append("'" + item.S_OrgName + "',");
                //sb.Append("'" + item.S_OrgCode + "',");
                sb.Append("'" + orgName + "',");
                sb.Append("'" + orgCode + "',");
                sb.Append(item.Adjust9 + ",");
                sb.Append(item.Adjust10 + ",");
                sb.Append(item.Adjust11 + ",");
                sb.Append(item.Adjust12 + ",");
                sb.Append(item.Adjust13 + ",");
                sb.Append(item.Adjust14 + ",");
                sb.Append(item.Adjust15 + ",");
                sb.Append(item.Adjust16 + ",");
                sb.Append(item.Adjust17 + ",");
                sb.Append(item.Adjust18 + ",");
                sb.Append(item.Adjust19 + ",");
                sb.Append(item.Adjust20 + ",");
                sb.Append(item.Adjust21 + ",");
                sb.Append(item.Adjust22 + ",");
                sb.Append(item.Adjust23 + ",");
                sb.Append(item.Adjust24 + ",");
                sb.Append(item.Adjust25 + ",");
                sb.Append(item.Adjust26 + ",");
                sb.Append(item.Adjust27 + ",");
                sb.Append(item.Adjust28 + ",");
                sb.Append("'" + item.S_Remark + "',");
                sb.Append("'" + batch + "',");
                sb.Append("'" + dateMonth.ToString("yyyy-MM-dd") + "')");
                i++;
                if (i % Num == 0 || item.Equals(d[d.Count - 1]))
                {
                    sb.Append(fengefu);
                }
                else
                {
                    sb.Append(",");
                }
                //fengefu = ",";
                //string updatesql = @"update tax_salary set  S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',Adjust9=isnull(Adjust9,0)+isnull(" + item.Adjust9 + ",0),Adjust10=isnull(Adjust10,0)+isnull(" + item.Adjust10 + ",0),Adjust11=isnull(Adjust11,0)+isnull(" + item.Adjust11
                //    + ",0),Adjust12=isnull(Adjust12,0)+isnull(" + item.Adjust12 + ",0),Adjust13=isnull(Adjust13,0)+isnull(" + item.Adjust13 + ",0),Adjust14=isnull(Adjust14,0)+isnull(" + item.Adjust14 + ",0),Adjust15=isnull(Adjust15,0)+isnull(" + item.Adjust15
                //    + ",0),Adjust16=isnull(Adjust16,0)+isnull(" + item.Adjust16 + ",0),Adjust17=isnull(Adjust17,0)+isnull(" + item.Adjust17 + ",0),Adjust18=isnull(Adjust18,0)+isnull(" + item.Adjust18 + ",0),Adjust19=isnull(Adjust19,0)+isnull(" + item.Adjust19
                //    + ",0),Adjust20=isnull(Adjust20,0)+isnull(" + item.Adjust20 + ",0),Adjust21=isnull(Adjust21,0)+isnull(" + item.Adjust21 + ",0),Adjust22=isnull(Adjust22,0)+isnull(" + item.Adjust22 + ",0),Adjust23=isnull(Adjust23,0)+isnull(" + item.Adjust23
                //    + ",0),Adjust24=isnull(Adjust24,0)+isnull(" + item.Adjust24 + ",0),Adjust25=isnull(Adjust25,0)+isnull(" + item.Adjust25 + ",0),Adjust26=isnull(Adjust26,0)+isnull(" + item.Adjust26 + ",0),Adjust27=isnull(Adjust27,0)+isnull(" + item.Adjust27
                //    + ",0),Adjust28=isnull(Adjust28,0)+isnull(" + item.Adjust28+ ",0) where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_WorkerCode='"+ item.S_WorkerCode + "'; ";
                //string updatesql = @"update tax_salary set  S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',Adjust9=isnull(Adjust9,0)+isnull(" + item.Adjust9 + ",0) where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_WorkerCode='" + item.S_WorkerCode + "'; ";
                //sqllst.Add(updatesql);
                //updatesb.Append(updatesql);
            }
            //string sql = sb.ToString();
            sqllst.Add(sb.ToString());
            string updateSql = @"update a set a.S_UpdateBy ='" + userId + "',a.S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," + @"
a.Adjust9=isnull(a.Adjust9,0)+isnull(b.Adjust9,0),
a.Adjust10=isnull(a.Adjust10,0)+isnull(b.Adjust10,0),
a.Adjust11=isnull(a.Adjust11,0)+isnull(b.Adjust11,0),
a.Adjust12=isnull(a.Adjust12,0)+isnull(b.Adjust12,0),
a.Adjust13=isnull(a.Adjust13,0)+isnull(b.Adjust13,0),
a.Adjust14=isnull(a.Adjust14,0)+isnull(b.Adjust14,0),
a.Adjust15=isnull(a.Adjust15,0)+isnull(b.Adjust15,0),
a.Adjust16=isnull(a.Adjust16,0)+isnull(b.Adjust16,0),
a.Adjust17=isnull(a.Adjust17,0)+isnull(b.Adjust17,0),
a.Adjust18=isnull(a.Adjust18,0)+isnull(b.Adjust18,0),
a.Adjust19=isnull(a.Adjust19,0)+isnull(b.Adjust19,0),
a.Adjust20=isnull(a.Adjust20,0)+isnull(b.Adjust20,0),
a.Adjust21=isnull(a.Adjust21,0)+isnull(b.Adjust21,0),
a.Adjust22=isnull(a.Adjust22,0)+isnull(b.Adjust22,0),
a.Adjust23=isnull(a.Adjust23,0)+isnull(b.Adjust23,0),
a.Adjust24=isnull(a.Adjust24,0)+isnull(b.Adjust24,0),
a.Adjust25=isnull(a.Adjust25,0)+isnull(b.Adjust25,0),
a.Adjust26=isnull(a.Adjust26,0)+isnull(b.Adjust26,0),
a.Adjust27=isnull(a.Adjust27,0)+isnull(b.Adjust27,0),
a.Adjust28=isnull(a.Adjust28,0)+isnull(b.Adjust28,0)
from tax_salary a,tax_adjust b
where DATEDIFF(m,b.S_WorkDate,'" + dateMonth.ToString("yyyy-MM-dd") + "') = 0  and b.S_Batch='" + batch + "' and DATEDIFF(m,a.S_WorkDate,b.S_WorkDate ) = 0 and a.S_WorkerCode = b.S_WorkerCode ";
            sqllst.Add(updateSql);
            //string sqlupdatesalary = "update tax_salary set T_DJJE=0 WHERE S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqlupdatesalary += " UPDATE tax_reportstatus SET ReportStatus=0, S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqllst.Add(sqlupdatesalary);
            return db.Executs(sqllst);
            //return db.ExecutByStringResult(sql);

        }


        /// <summary>
        /// 清空调整
        /// </summary>
        /// <param name="dateMonth"></param>
        /// <param name="userId"></param>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string delTaxAdjust(DateTime dateMonth, string userId, string orgCode)
        {
            List<string> sqllst = new List<string>();
            string delBonus = @"delete from tax_adjust where S_OrgCode ='" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delBonus);
            string updatesql = @"update tax_salary set S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',Adjust9=0,Adjust10=0,Adjust11=0"+
                     ",Adjust12=0,Adjust13=0,Adjust14=0,Adjust15=0,Adjust16=0,Adjust17=0,Adjust18=0,Adjust19=0,Adjust20=0,Adjust21=0,Adjust22=0,Adjust23=0"+
                     ",Adjust24=0,Adjust25=0,Adjust26=0,Adjust27=0,Adjust28=0 where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(updatesql);
            return db.Executs(sqllst);
        }
    }
}
