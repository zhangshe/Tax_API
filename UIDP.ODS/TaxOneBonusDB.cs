using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace UIDP.ODS
{
    public class TaxOneBonusDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询一次性奖金信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataSet getOneBonusList(Dictionary<string, object> d, int page, int limit)
        {
            //string sql = @"select * from tax_onebonus where 1=1 ";
            ////if (d.Keys.Contains("S_OrgName") && d["S_OrgName"] != null && d["S_OrgName"].ToString() != "")
            ////{
            ////    sql += " and S_OrgName like '%" + d["S_OrgName"].ToString() + "%'";
            ////}
            //if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            //{
            //    sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%'";
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
            //sql += " order by S_WorkDate desc ,S_WorkerCode";
            //return db.GetDataTable(sql);


            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            string sql = " SELECT ROW_NUMBER () OVER ( ORDER BY S_WorkerCode ) AS non,a.* INTO #temp FROM tax_onebonus a ";
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
            else
            {
                sql += " SELECT * FROM #temp WHERE non between " + ((page - 1) * limit + 1) + " AND " + page * limit + "order by S_WorkDate desc ";
            }
            string sql1 = " SELECT COUNT(*) FROM #temp";
            sqllist.Add("data", sql);
            sqllist.Add("total", sql1);
            return db.GetDataSet(sqllist);
        }


        public string createTaxBonus(List<ImportTaxBonus> d, DateTime dateMonth, string userId, string orgCode, string orgName)
        {
            List<string> sqllst = new List<string>();
            string fengefu = "";
            int i = 0;//循环数，用于判断添加SQL语句头部；
            const int Num = 600;//Insert插入分条数目
            string headersql = " insert into tax_onebonus (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,OneTimeBonus,DeductibleTax,S_Batch,S_WorkDate) values ";
           StringBuilder sb = new StringBuilder();
            //StringBuilder updatesb = new StringBuilder();
            string batch = Guid.NewGuid().ToString();
            //sb.Append(@" insert into tax_onebonus (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,OneTimeBonus,DeductibleTax,S_WorkDate) values ");

            foreach (ImportTaxBonus item in d)
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
                sb.Append("'" + orgName + "',");
                sb.Append("'" + orgCode + "',");
                sb.Append(item.OneTimeBonus + ",");
                sb.Append(item.DeductibleTax + ",");
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
                //string updatesql = @"update tax_salary set  S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',G_YCXJJ=isnull(G_YCXJJ,0)+isnull(" + item.OneTimeBonus + ",0),K_YCXJJYKS=isnull(K_YCXJJYKS,0)+isnull(" + item.DeductibleTax 
                // + ",0) where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_WorkerCode='" + item.S_WorkerCode + "'; ";
                //updatesb.Append(updatesql);
                //sqllst.Add(updatesql);
            }
            //string insertRecord = "insert into tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate) values('" + Guid.NewGuid().ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + userId + "','" + v.S_OrgName + "','" + v.S_OrgCode + "',0,'" + dateMonth.ToString("yyyy-MM-dd") + "')";
            //sqllst.Add(insertRecord);
            //string sql = sb.ToString();
            sqllst.Add(sb.ToString());
            //sqllst.Add(updatesb.ToString());
            //string sqlupdatesalary = "update tax_salary set T_DJJE=0 WHERE S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqlupdatesalary += " UPDATE tax_reportstatus SET ReportStatus=0, S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqllst.Add(sqlupdatesalary);

            string updateSql = @"update a set a.S_UpdateBy ='" + userId + "',a.S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," + @"
a.G_YCXJJ=isnull(a.G_YCXJJ,0)+isnull(b.OneTimeBonus,0),
a.K_YCXJJYKS=isnull(a.K_YCXJJYKS,0)+isnull(b.DeductibleTax,0)
from tax_salary a,tax_onebonus b
where b.S_WorkDate='" + dateMonth.ToString("yyyy-MM-dd") + "' and b.S_Batch='" + batch + "' and DATEDIFF(m,a.S_WorkDate,b.S_WorkDate ) = 0 and a.S_WorkerCode = b.S_WorkerCode ";
            sqllst.Add(updateSql);

            return db.Executs(sqllst);
            //return db.ExecutByStringResult(sql);

        }

        /// <summary>
        /// 清空奖金
        /// </summary>
        /// <param name="dateMonth"></param>
        /// <param name="userId"></param>
        /// <param name="orgCode"></param>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public string delTaxBonus(DateTime dateMonth, string userId, string orgCode)
        {
            List<string> sqllst = new List<string>();
            string delBonus = @"delete from tax_onebonus where S_OrgCode ='" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delBonus);
            string updatesql = @"update tax_salary set S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +"', G_YCXJJ =0,K_YCXJJYKS=0 "
                 + "where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(updatesql);
            return db.Executs(sqllst);
        }
    }
}
