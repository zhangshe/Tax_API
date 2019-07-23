using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace UIDP.ODS
{
    public class TaxOneTimeBonusDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询一次性奖金信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataSet getOneTimeBonusList(Dictionary<string, object> d, int page, int limit)
        {
            string sql = "select a.S_WorkerCode,max(a.S_WorkDate)S_WorkDate into #temp from [dbo].[tax_salary] a  where 1=1 ";//temp 查询工作月份最新的人
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                sql += " and a.S_Department like '%" + d["S_Department"].ToString() + "%' ";
            }
            sql += " and exists (select 1 from [dbo].[tax_onetimebonus] b where a.S_WorkerCode=b.S_WorkerCode ";
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                sql += " and b.S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
                sql += " AND DATEDIFF(mm, b.S_WorkDate, '";

                sql += bdate.ToString("yyyyMMdd") + "')=0";
            }
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " and b.S_OrgCode like '" + d["S_OrgCode"].ToString() + "%' ";
            }
            sql += " ) ";
            sql += " group by a.S_WorkerCode ";
            sql += @"select ROW_NUMBER () OVER ( ORDER BY a.S_WorkerCode ) AS non,a.*,[dbo].[fn_onetimecal](a.onetimebonus-a.FreeIncome) as Tax,c.S_Department into #tempBonus from tax_onetimebonus a ";
            sql += " join #temp b on a.S_WorkerCode=b.S_WorkerCode ";
            sql += " join  [dbo].[tax_salary] c on c.S_WorkerCode=b.S_WorkerCode ";
            sql += " order by a.S_WorkDate desc ,a.S_WorkerCode";

            // return db.GetDataTable(sql);
            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            if (page == 0 && limit == 0)
            {
                sql += " SELECT * FROM #tempBonus  order by S_WorkDate desc ";
            }
            else
            {
                sql += " SELECT * FROM #tempBonus where non between " + ((page - 1) * limit + 1) + " AND " + page * limit + " order by S_WorkDate desc ";
            }
            string sql1 = " SELECT COUNT(*) FROM #tempBonus";
            sqllist.Add("data", sql);
            sqllist.Add("total", sql1);
            return db.GetDataSet(sqllist);
        }

        public DataTable getOneTimeBonusListImport(Dictionary<string, object> d)
        {
            string sql = @"select * from tax_onetimebonus where 1=1 ";
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
                sql += " AND DATEDIFF(mm, S_WorkDate, '";
                sql += bdate.ToString("yyyy-MM-dd") + "')=0";
            }
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " and S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            }
            sql += " order by S_WorkDate desc ,S_WorkerCode";
            return db.GetDataTable(sql);
        }

        public string createOneTimeBonus(List<ImportOneTimeBonus> d, DateTime dateMonth, string userId, string orgCode, string orgName)
        {
            DataTable dictiondt = db.GetDataTable(" SELECT * FROM tax_dictionary WHERE ParentCode='ZJLX' ");
            List<string> sqllst = new List<string>();
            int truckNum = Convert.ToInt32(Convert.ToDecimal(d.Count / 500));
            int yushu = d.Count % 500;
            if (yushu > 0)
            {
                truckNum++;
            }
            for (int i = 1; i < truckNum + 1; i++)
            {
                string fengefu = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(@" insert into tax_onetimebonus (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,OneTimeBonus,DeductibleTax,IdTypeName,IdType,IdNumber,FreeIncome,Other,AllowDeduction,TaxSaving,Remark,S_WorkDate) values ");
                int rowbegin = (i - 1) * 500;
                int rowend = i * 500;
                if (rowend > d.Count) { rowend = d.Count; }
                for (int j = rowbegin; j < rowend; j++)
                //foreach (ImportOneTimeBonus item in d)
                {
                    DataRow[] ZJLX = dictiondt.Select("Name='" + d[j].IdTypeName + "'");
                    if (!string.IsNullOrEmpty(d[j].IdTypeName))
                    {
                        d[j].IdType = ZJLX[0]["Code"].ToString();
                    }
                    sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
                    sb.Append("'" + userId + "',");
                    sb.Append("'" + d[j].S_WorkerCode + "',");
                    sb.Append("'" + d[j].S_WorkerName + "',");
                    sb.Append("'" + orgName + "',");
                    sb.Append("'" + orgCode + "',");
                    sb.Append(d[j].OneTimeBonus + ",");
                    sb.Append(d[j].DeductibleTax + ",");
                    sb.Append(defaultNull(d[j].IdTypeName) + ",");
                    sb.Append(defaultNull(d[j].IdType) + ",");
                    sb.Append(defaultNull(d[j].IdNumber) + ",");
                    sb.Append(d[j].FreeIncome + ",");
                    sb.Append(d[j].Other + ",");
                    sb.Append(d[j].AllowDeduction + ",");
                    sb.Append(d[j].TaxSaving + ",");
                    sb.Append(defaultNull(d[j].Remark) + ",");
                    sb.Append("'" + dateMonth.ToString("yyyy-MM-dd") + "')");
                    fengefu = ",";
                    //string updatesql = @"update tax_salary set  S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',G_YCXJJ=isnull(G_YCXJJ,0)+isnull(" + item.OneTimeBonus + ",0),K_YCXJJYKS=isnull(K_YCXJJYKS,0)+isnull(" + item.DeductibleTax 
                    // + ",0) where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_WorkerCode='" + item.S_WorkerCode + "'";
                    //sqllst.Add(updatesql);
                }
                string sql = sb.ToString();
                sqllst.Add(sql);
            }
               
          

            //string insertRecord = "insert into tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate) values('" + Guid.NewGuid().ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + userId + "','" + v.S_OrgName + "','" + v.S_OrgCode + "',0,'" + dateMonth.ToString("yyyy-MM-dd") + "')";
            //sqllst.Add(insertRecord);

            //string sqlupdatesalary = "update tax_salary set T_DJJE=0 WHERE S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqlupdatesalary += " UPDATE tax_reportstatus SET ReportStatus=0, S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqllst.Add(sqlupdatesalary);
            return db.Executs(sqllst);
            //return db.ExecutByStringResult(sql);

        }
        public string defaultNull(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return "'" + value + "'";
            }
            return "null";
        }
        public string defaultZero(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            return "0";
        }
        /// <summary>
        /// 清空一次性奖金
        /// </summary>
        /// <param name="dateMonth"></param>
        /// <param name="userId"></param>
        /// <param name="orgCode"></param>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public string clearOneTimeBonus(DateTime dateMonth, string userId, string orgCode)
        {
            List<string> sqllst = new List<string>();
            string delBonus = @"delete from tax_onetimebonus where S_OrgCode ='" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delBonus);
            //string updatesql = @"update tax_salary set S_UpdateBy='" + userId + "',S_UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +"', G_YCXJJ =0,K_YCXJJYKS=0 "
            //     + "where S_OrgCode = '" + orgCode + "' and DATEDIFF(m,S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            //sqllst.Add(updatesql);
            return db.Executs(sqllst);
        }

        /// <summary>
        /// 删除一次性奖金
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public string delOneTimeBonus(string sid)
        {
            string sql = "DELETE FROM tax_onetimebonus WHERE S_Id='" + sid + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataTable validateOnetimeTaxByWorkDate(DateTime WorkDate)
        {
            string sql = "Select * from tax_onetimebonus where Year(S_WorkDate)=Year('"+WorkDate+"') ";
            return db.GetDataTable(sql);
        }
    }
}
