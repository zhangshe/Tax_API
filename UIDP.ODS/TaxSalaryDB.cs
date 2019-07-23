using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;

namespace UIDP.ODS
{
    public class TaxSalaryDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询工资表信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getTaxSalaryList(Dictionary<string, object> d)
        {
            string sql = @"select a.S_Id,a.S_CreateDate,a.S_OrgName,a.S_OrgCode,a.S_WorkerName,a.S_WorkerCode,a.G_GWJGZ,a.G_GWJGZB,a.G_BLGZ,a.G_GLJT,a.G_SGJT,a.G_JSJNJT,a.G_ZFBT,a.G_BLJT,a.G_BYKT,a.G_ZFJT,a.G_HMJT,a.G_QTJT,a.G_JBJDGZ,a.G_YBJT,a.G_JCYJ,a.G_YJJJ,a.G_DSZNBJFS,a.G_WCBTSF,a.G_BFK,a.T_YFHJ,a.K_YiLiaoBX,a.K_SYBX,a.K_YangLaoBX,a.K_ZFGJJ,a.K_QYNJ,a.K_QTKX,a.T_YSHJ,a.K_KS,a.T_SFHJ,a.G_JSJT,a.G_XFGZJT,a.G_FLGDZJT,a.G_BZRJT,a.G_SYYLJT,a.G_XQJB,a.G_PSJB,a.G_JRJB,a.G_ZXJ,a.G_C01,a.G_C02,a.G_C03,a.G_C04,a.G_C05,a.G_DSZNJT,a.G_BJSPJT,a.G_FSJWF,a.G_WCBZ,a.G_TXBZ,a.G_JTBZ,a.G_HLHJYJ,a.G_LYF,a.T_XJ1,a.T_XJ2,a.G_ZENBFBK,a.G_ZEWBFBK,a.Adjust1,a.Adjust2,a.Adjust3,a.Adjust4,a.Adjust5,a.Adjust6,a.Adjust7,a.Adjust8,a.Adjust9,a.Adjust10,a.Adjust11,a.Adjust12,a.Adjust13,a.Adjust14,a.Adjust15,a.Adjust16,a.Adjust17,a.Adjust18,a.Adjust19,a.Adjust20,a.Adjust21,a.Adjust22,a.Adjust23,a.Adjust24,a.Adjust25,a.Adjust26,a.Adjust27,a.Adjust28,a.T_DJJE,isnull(G_YCXJJ,0)G_YCXJJ,isnull(K_YCXJJYKS,0)K_YCXJJYKS,a.S_WorkDate,a.S_Department,a.T_SFGZ,a.T_LJYJS,b.ImportModel from tax_salary a left join tax_org b on a.S_OrgCode=b.S_OrgCode where 1=1 ";
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                sql += " and S_Department like '%" + d["S_Department"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkerCode") && d["S_WorkerCode"] != null && d["S_WorkerCode"].ToString() != "")
            {
                sql += " and S_WorkerCode = '" + d["S_WorkerCode"].ToString() + "'";
            }
            if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
                sql += " and DATEDIFF(m, S_WorkDate, '" + bdate.ToString("yyyy-MM-dd") + "') = 0 ";
            }
            if (d.Keys.Contains("S_BeginWorkDate") && d["S_BeginWorkDate"] != null && d["S_BeginWorkDate"].ToString() != "" && d.Keys.Contains("S_EndWorkDate") && d["S_EndWorkDate"] != null && d["S_EndWorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_BeginWorkDate"].ToString());
                DateTime edate = Convert.ToDateTime(d["S_EndWorkDate"].ToString());
                sql += "and  S_WorkDate BETWEEN '" + bdate.ToString("yyyy-MM-dd") + "' AND '" + edate.ToString("yyyy-MM-dd") + "'";
            }
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " and a.S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            }
            sql += " order by S_WorkerCode  ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 通过临时表查询工资数据
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataSet createSalaryTemp(Dictionary<string, object> d)
        {
            int limit = Convert.ToInt32(d["limit"]);
            int page = Convert.ToInt32(d["page"]);
            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            string sql = "SELECT ROW_NUMBER () OVER(order by a.S_Id) as non, a.*,b.ImportModel INTO #tempSalary FROM tax_salary a left join tax_org b on a.S_OrgCode=b.S_OrgCode where 1=1";
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                sql += " and S_Department like '%" + d["S_Department"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%'";
            }
            if (d.Keys.Contains("S_WorkerCode") && d["S_WorkerCode"] != null && d["S_WorkerCode"].ToString() != "")
            {
                sql += " and S_WorkerCode = '" + d["S_WorkerCode"].ToString() + "'";
            }
            if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
                sql += " and DATEDIFF(m, S_WorkDate, '" + bdate.ToString("yyyy-MM-dd") + "') = 0 ";
            }
            if (d.Keys.Contains("S_BeginWorkDate") && d["S_BeginWorkDate"] != null && d["S_BeginWorkDate"].ToString() != "" && d.Keys.Contains("S_EndWorkDate") && d["S_EndWorkDate"] != null && d["S_EndWorkDate"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["S_BeginWorkDate"].ToString());
                DateTime edate = Convert.ToDateTime(d["S_EndWorkDate"].ToString());
                sql += "and  S_WorkDate BETWEEN '" + bdate.ToString("yyyy-MM-dd") + "' AND '" + edate.ToString("yyyy-MM-dd") + "'";
            }
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " and a.S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            }
            sql += " order by S_WorkerCode  ";
            sql += " SELECT * FROM #tempSalary WHERE  non BETWEEN " + ((page - 1) * limit + 1) + " AND " + (page * limit);
            sqllist.Add("data", sql);

            string sql1 = " SELECT COUNT(*) FROM #tempSalary ";

            sqllist.Add("total", sql1);
            return db.GetDataSet(sqllist);
        }

        /// <summary>
        /// 获取导入人员信息
        /// </summary>
        /// <param name="orgCode">部门编号</param>
        /// <param name="dateMonth">年月</param>
        /// <param name="importModel">导入模板</param>
        /// <returns></returns>
        public DataTable getTaxSalaryPersonByOrgMonth(string orgCode, DateTime dateMonth, string importModel)
        {
            dateMonth = dateMonth.AddMonths(-1);
            if (importModel == "样表一")
            {
                string sql = @"select S_WorkerCode,S_WorkerName,S_OrgName,G_GWJGZ,G_BLGZ,G_GLJT,G_SGJT,G_JSJNJT,G_ZFBT,
                            G_BLJT,G_BYKT,G_QTJT,G_YBJT,G_JBJDGZ,G_JCYJ,G_YJJJ,G_DSZNBJFS,G_WCBTSF,G_BFK,T_YFHJ,K_YiLiaoBX,
                            K_SYBX,K_YangLaoBX,K_ZFGJJ,K_QYNJ,K_QTKX,T_YSHJ,K_KS,T_SFHJ,Adjust1,Adjust2,Adjust3,Adjust4,Adjust5,
                            Adjust6,Adjust7,Adjust8
                            from tax_salary a where S_OrgCode='" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 order by S_WorkerCode ";

                return db.GetDataTable(sql);
            }
            else if (importModel == "样表二")
            {
                string sql = @"select S_WorkerCode,S_WorkerName,S_OrgName,G_GWJGZ,G_GWJGZB,G_BLGZ,G_GLJT,G_SGJT,G_JSJNJT,G_ZFBT,
                            G_BLJT,G_BYKT,G_ZFJT,G_HMJT,G_JSJT,G_XFGZJT,G_FLGDZJT,G_BZRJT,G_SYYLJT,G_YBJT,
                            G_XQJB,G_PSJB,G_JRJB,G_JCYJ,G_YJJJ,G_ZENBFBK,G_ZXJ,G_C01,G_C02,G_C03,
                            G_C04,T_XJ1,G_DSZNJT,G_BJSPJT,G_FSJWF,G_WCBZ,G_TXBZ,G_JTBZ,G_HLHJYJ,G_C05,
                            G_ZEWBFBK,G_LYF,T_XJ2,T_YFHJ,K_YiLiaoBX,K_SYBX,K_YangLaoBX,K_ZFGJJ,K_QYNJ,K_QTKX,
                            T_YSHJ,K_KS,T_SFHJ,Adjust1,Adjust2,Adjust3,Adjust4,Adjust5,Adjust6,Adjust7,Adjust8 
                            from tax_salary a where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 order by S_WorkerCode ";
                // from tax_salary a where S_OrgCode='" + orgCode + "' and S_CreateDate='" + dateMonth + "' ";
                return db.GetDataTable(sql);
            }
            return null;
        }


        //public string createTaxSalary1(List<ImportInfo1> d,DateTime dateMonth,string userId,string orgCode,string orgName)
        public string createTaxSalary1(List<ImportTaxSalary1> list, DateTime dateMonth, string userId, string orgCode, string orgName, string personChange)
        {
            List<string> sqllst = new List<string>();
            string delrecord = "delete from tax_salary where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode='" + orgCode + "'";
            string deladjust = "delete from tax_adjust where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode='" + orgCode + "'";
            string delonebonus = "delete from tax_onebonus where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode='" + orgCode + "'";
            string deltaxpayerrecord = "delete from tax_taxpayerrecord where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, ImportMonth, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delrecord);
            sqllst.Add(deladjust);
            sqllst.Add(delonebonus);
            sqllst.Add(deltaxpayerrecord);
            int truckNum = Convert.ToInt32(Convert.ToDecimal(list.Count / 500));
            int yushu = list.Count % 500;
            if (yushu > 0)
            {
                truckNum++;
            }
            for (int i = 1; i < truckNum + 1; i++)
            {
                string fengefu = "";
                StringBuilder sb = new StringBuilder();
                int rowbegin = (i - 1) * 500;
                int rowend = i * 500;
                if (rowend > list.Count) { rowend = list.Count; }
                sb.Append(@" insert into tax_salary (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,G_GWJGZ,G_BLGZ,G_GLJT,G_SGJT,G_JSJNJT,G_ZFBT,G_BLJT,G_BYKT,
                                G_QTJT,G_YBJT,G_JBJDGZ,G_JCYJ,G_YJJJ,G_DSZNBJFS,G_WCBTSF,G_BFK,T_YFHJ,K_YiLiaoBX,K_SYBX,K_YangLaoBX,K_ZFGJJ,K_QYNJ,
                                K_QTKX,T_YSHJ,K_KS,T_SFHJ,Adjust1,Adjust2,Adjust3,Adjust4,Adjust5,Adjust6,Adjust7,Adjust8,S_Department,S_WorkDate) values ");
                for (int j = rowbegin; j < rowend; j++)
                //foreach (ImportTaxSalary1 item in list)
                {
                    sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
                    sb.Append("'" + userId + "',");
                    sb.Append("'" + list[j].S_WorkerCode + "',");
                    sb.Append("'" + list[j].S_WorkerName + "',");
                    sb.Append("'" + orgName + "',");
                    sb.Append("'" + orgCode + "',");
                    sb.Append(list[j].G_GWJGZ + ",");
                    sb.Append(list[j].G_BLGZ + ",");
                    sb.Append(list[j].G_GLJT + ",");
                    sb.Append(list[j].G_SGJT + ",");
                    sb.Append(list[j].G_JSJNJT + ",");
                    sb.Append(list[j].G_ZFBT + ",");
                    sb.Append(list[j].G_BLJT + ",");
                    sb.Append(list[j].G_BYKT + ",");
                    sb.Append(list[j].G_QTJT + ",");
                    sb.Append(list[j].G_YBJT + ",");
                    sb.Append(list[j].G_JBJDGZ + ",");
                    sb.Append(list[j].G_JCYJ + ",");
                    sb.Append(list[j].G_YJJJ + ",");
                    sb.Append(list[j].G_DSZNBJFS + ",");
                    sb.Append(list[j].G_WCBTSF + ",");
                    sb.Append(list[j].G_BFK + ",");
                    sb.Append(list[j].T_YFHJ + ",");
                    sb.Append(list[j].K_YiLiaoBX + ",");
                    sb.Append(list[j].K_SYBX + ",");
                    sb.Append(list[j].K_YangLaoBX + ",");
                    sb.Append(list[j].K_ZFGJJ + ",");
                    sb.Append(list[j].K_QYNJ + ",");
                    sb.Append(list[j].K_QTKX + ",");
                    sb.Append(list[j].T_YSHJ + ",");
                    sb.Append(list[j].K_KS + ",");
                    sb.Append(list[j].T_SFHJ + ",");
                    sb.Append(list[j].Adjust1 + ",");
                    sb.Append(list[j].Adjust2 + ",");
                    sb.Append(list[j].Adjust3 + ",");
                    sb.Append(list[j].Adjust4 + ",");
                    sb.Append(list[j].Adjust5 + ",");
                    sb.Append(list[j].Adjust6 + ",");
                    sb.Append(list[j].Adjust7 + ",");
                    sb.Append(list[j].Adjust8 + ",");
                    sb.Append("'" + list[j].S_OrgName + "',");
                    sb.Append("'" + dateMonth.ToString("yyyy-MM-dd") + "')");
                    fengefu = ",";
                }
                string sql = sb.ToString();
                sqllst.Add(sql);
            }


            //    string fengefu = "";
            //StringBuilder sb = new StringBuilder();

            //foreach (var v in d)
            //{

            //foreach (ImportTaxSalary1 item in list)
            //{
            //    sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
            //    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
            //    sb.Append("'" + userId + "',");
            //    sb.Append("'" + item.S_WorkerCode + "',");
            //    sb.Append("'" + item.S_WorkerName + "',");
            //    sb.Append("'" + orgName + "',");
            //    sb.Append("'" + orgCode + "',");
            //    sb.Append(item.G_GWJGZ + ",");
            //    sb.Append(item.G_BLGZ + ",");
            //    sb.Append(item.G_GLJT + ",");
            //    sb.Append(item.G_SGJT + ",");
            //    sb.Append(item.G_JSJNJT + ",");
            //    sb.Append(item.G_ZFBT + ",");
            //    sb.Append(item.G_BLJT + ",");
            //    sb.Append(item.G_BYKT + ",");
            //    sb.Append(item.G_QTJT + ",");
            //    sb.Append(item.G_YBJT + ",");
            //    sb.Append(item.G_JBJDGZ + ",");
            //    sb.Append(item.G_JCYJ + ",");
            //    sb.Append(item.G_YJJJ + ",");
            //    sb.Append(item.G_DSZNBJFS + ",");
            //    sb.Append(item.G_WCBTSF + ",");
            //    sb.Append(item.G_BFK + ",");
            //    sb.Append(item.T_YFHJ + ",");
            //    sb.Append(item.K_YiLiaoBX + ",");
            //    sb.Append(item.K_SYBX + ",");
            //    sb.Append(item.K_YangLaoBX + ",");
            //    sb.Append(item.K_ZFGJJ + ",");
            //    sb.Append(item.K_QYNJ + ",");
            //    sb.Append(item.K_QTKX + ",");
            //    sb.Append(item.T_YSHJ + ",");
            //    sb.Append(item.K_KS + ",");
            //    sb.Append(item.T_SFHJ + ",");
            //    sb.Append(item.Adjust1 + ",");
            //    sb.Append(item.Adjust2 + ",");
            //    sb.Append(item.Adjust3 + ",");
            //    sb.Append(item.Adjust4 + ",");
            //    sb.Append(item.Adjust5 + ",");
            //    sb.Append(item.Adjust6 + ",");
            //    sb.Append(item.Adjust7 + ",");
            //    sb.Append(item.Adjust8 + ",");
            //    sb.Append("'" + item.S_OrgName + "',");
            //    sb.Append("'" + dateMonth.ToString("yyyy-MM-dd") + "')");
            //    fengefu = ",";
            //}
            string sqldel = "delete from tax_reportstatus where datediff(m,S_WorkDate,'" + dateMonth.ToString("yyyy-MM-dd") + "')=0 and S_OrgCode ='" + orgCode + "'";
            sqllst.Add(sqldel);
            string insertRecord = "insert into tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate) values('" + Guid.NewGuid().ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + userId + "','" + orgName + "','" + orgCode + "',1,'" + dateMonth.ToString("yyyy-MM-dd") + "')";//YZ修改 插入工资Excel根据需求从状态0修改为1
            sqllst.Add(insertRecord);
            //}


            if (!string.IsNullOrEmpty(personChange))
            {
                sqllst.Add(personChange);
            }
            return db.Executs(sqllst);
            //return db.ExecutByStringResult(sql);
        }
        public string createTaxSalary2(List<ImportTaxSalary2> list, DateTime dateMonth, string userId, string orgCode, string orgName, string personChange)
        //public string createTaxSalary2(List<ImportInfo2> d, DateTime dateMonth, string userId, string orgCode, string orgName)
        {
            List<string> sqllst = new List<string>();
            string delrecord = "delete from tax_salary where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode='" + orgCode + "'";
            string deladjust = "delete from tax_adjust where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode='" + orgCode + "'";
            string delonebonus = "delete from tax_onebonus where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode='" + orgCode + "'";
            string deltaxpayerrecord = "delete from tax_taxpayerrecord where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, ImportMonth, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delrecord);
            sqllst.Add(deladjust);
            sqllst.Add(delonebonus);
            sqllst.Add(deltaxpayerrecord);

            //string insertRecord = "insert into tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate) values('"+ Guid.NewGuid().ToString() + "','"+ DateTime.Now.ToString("yyyy-MM-dd") + "','"+userId+"','"+v.S_OrgName+"','"+v.S_OrgCode+"',0,'"+ dateMonth.ToString("yyyy-MM-dd") + "')";

            int truckNum = Convert.ToInt32(Convert.ToDecimal(list.Count / 500));
            int yushu = list.Count % 500;
            if (yushu > 0)
            {
                truckNum++;
            }
            for (int i = 1; i < truckNum + 1; i++)
            {
                string fengefu = "";
                StringBuilder sb = new StringBuilder();
                int rowbegin = (i - 1) * 500;
                int rowend = i * 500;
                if (rowend > list.Count) { rowend = list.Count; }
                sb.Append(@" insert into tax_salary (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,G_GWJGZ,G_GWJGZB,G_BLGZ,
                                G_GLJT,G_SGJT,G_JSJNJT,G_ZFBT,G_BLJT,G_BYKT,G_ZFJT,G_HMJT,G_JSJT,G_XFGZJT,G_FLGDZJT,G_BZRJT,
                                G_SYYLJT,G_YBJT,G_XQJB,G_PSJB,G_JRJB,G_JCYJ,G_YJJJ,G_ZENBFBK,G_ZXJ,G_C01,G_C02,G_C03,G_C04,T_XJ1,
                                G_DSZNJT,G_BJSPJT,G_FSJWF,G_WCBZ,G_TXBZ,G_JTBZ,G_HLHJYJ,G_C05,G_ZEWBFBK,G_LYF,T_XJ2,T_YFHJ,K_YiLiaoBX,
                                K_SYBX,K_YangLaoBX,K_ZFGJJ,K_QYNJ,K_QTKX,T_YSHJ,K_KS,T_SFHJ,Adjust1,Adjust2,Adjust3,Adjust4,Adjust5,Adjust6,Adjust7,Adjust8,S_Department,S_WorkDate) values ");
                for (int j = rowbegin; j < rowend; j++)
                //foreach (ImportTaxSalary2 item in list)
                {
                    sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
                    sb.Append("'" + userId + "',");
                    sb.Append("'" + list[j].S_WorkerCode + "',");
                    sb.Append("'" + list[j].S_WorkerName + "',");
                    sb.Append("'" + orgName + "',");
                    sb.Append("'" + orgCode + "',");
                    sb.Append(list[j].G_GWJGZ + ",");
                    sb.Append(list[j].G_GWJGZB + ",");
                    sb.Append(list[j].G_BLGZ + ",");
                    sb.Append(list[j].G_GLJT + ",");
                    sb.Append(list[j].G_SGJT + ",");
                    sb.Append(list[j].G_JSJNJT + ",");
                    sb.Append(list[j].G_ZFBT + ",");
                    sb.Append(list[j].G_BLJT + ",");
                    sb.Append(list[j].G_BYKT + ",");
                    sb.Append(list[j].G_ZFJT + ",");
                    sb.Append(list[j].G_HMJT + ",");
                    sb.Append(list[j].G_JSJT + ",");
                    sb.Append(list[j].G_XFGZJT + ",");
                    sb.Append(list[j].G_FLGDZJT + ",");
                    sb.Append(list[j].G_BZRJT + ",");
                    sb.Append(list[j].G_SYYLJT + ",");
                    sb.Append(list[j].G_YBJT + ",");
                    sb.Append(list[j].G_XQJB + ",");
                    sb.Append(list[j].G_PSJB + ",");
                    sb.Append(list[j].G_JRJB + ",");
                    sb.Append(list[j].G_JCYJ + ",");
                    sb.Append(list[j].G_YJJJ + ",");
                    sb.Append(list[j].G_ZENBFBK + ",");
                    sb.Append(list[j].G_ZXJ + ",");
                    sb.Append(list[j].G_C01 + ",");
                    sb.Append(list[j].G_C02 + ",");
                    sb.Append(list[j].G_C03 + ",");
                    sb.Append(list[j].G_C04 + ",");
                    sb.Append(list[j].T_XJ1 + ",");
                    sb.Append(list[j].G_DSZNJT + ",");
                    sb.Append(list[j].G_BJSPJT + ",");
                    sb.Append(list[j].G_FSJWF + ",");
                    sb.Append(list[j].G_WCBZ + ",");
                    sb.Append(list[j].G_TXBZ + ",");
                    sb.Append(list[j].G_JTBZ + ",");
                    sb.Append(list[j].G_HLHJYJ + ",");
                    sb.Append(list[j].G_C05 + ",");
                    sb.Append(list[j].G_ZEWBFBK + ",");
                    sb.Append(list[j].G_LYF + ",");
                    sb.Append(list[j].T_XJ2 + ",");
                    sb.Append(list[j].T_YFHJ + ",");
                    sb.Append(list[j].K_YiLiaoBX + ",");
                    sb.Append(list[j].K_SYBX + ",");
                    sb.Append(list[j].K_YangLaoBX + ",");
                    sb.Append(list[j].K_ZFGJJ + ",");
                    sb.Append(list[j].K_QYNJ + ",");
                    sb.Append(list[j].K_QTKX + ",");
                    sb.Append(list[j].T_YSHJ + ",");
                    sb.Append(list[j].K_KS + ",");
                    sb.Append(list[j].T_SFHJ + ",");
                    sb.Append(list[j].Adjust1 + ",");
                    sb.Append(list[j].Adjust2 + ",");
                    sb.Append(list[j].Adjust3 + ",");
                    sb.Append(list[j].Adjust4 + ",");
                    sb.Append(list[j].Adjust5 + ",");
                    sb.Append(list[j].Adjust6 + ",");
                    sb.Append(list[j].Adjust7 + ",");
                    sb.Append(list[j].Adjust8 + ",");
                    sb.Append("'" + list[j].S_OrgName + "',");
                    sb.Append("'" + dateMonth.ToString("yyyy-MM-dd") + "')");
                    fengefu = ",";
                }
                string sql = sb.ToString();
                sqllst.Add(sql);
            }


            //    string fengefu = "";
            //StringBuilder sb = new StringBuilder();
            //foreach (var v in d)
            //{
            string sqldel = "delete from tax_reportstatus where datediff(m,S_WorkDate,'" + dateMonth.ToString("yyyy-MM-dd") + "')=0 and S_OrgCode ='" + orgCode + "'";
            sqllst.Add(sqldel);
            string insertRecord = "insert into tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate) values('" + Guid.NewGuid().ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + userId + "','" + orgName + "','" + orgCode + "',1,'" + dateMonth.ToString("yyyy-MM-dd") + "')";//YZ修改 插入工资Excel根据需求从状态0修改为1
            sqllst.Add(insertRecord);
            //}

            if (!string.IsNullOrEmpty(personChange))
            {
                sqllst.Add(personChange);
            }
            return db.Executs(sqllst);
            //return db.ExecutByStringResult(sql);
        }


        /// <summary>
        /// 根据部门编码获取本部门下组织机构工资表信息
        /// </summary>
        /// <param name="orgCode">部门编码</param>
        /// <returns></returns>
        public DataTable getTaxSalaryInfo(string orgCode, DateTime dateMonth)
        {
            string sql = @"select * from tax_salary where DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 and S_OrgCode = '" + orgCode + "'";
            return db.GetDataTable(sql);
        }
        public DataTable getTaxSalaryByYear(DateTime dateMonth)
        {
            string sql = @"select * from tax_salary where year(S_WorkDate)=year('" + dateMonth.ToString("yyyy-MM-dd") + "') ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 清空工资
        /// </summary>
        /// <param name="dateMonth"></param>
        /// <param name="userId"></param>
        /// <param name="orgCode"></param>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public string delTaxSalary(DateTime dateMonth, string orgCode)
        {
            List<string> sqllst = new List<string>();
            string delSalary = @"delete from tax_salary where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delSalary);
            string delAdjust = @"delete from tax_adjust where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delAdjust);
            string delBonus = @"delete from tax_onebonus where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delBonus);
            string delStatus = @"delete from tax_reportstatus where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(delStatus);
            string deltaxpayerrecord = "delete from tax_taxpayerrecord where S_OrgCode = '" + orgCode + "' and DATEDIFF(m, ImportMonth, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 ";
            sqllst.Add(deltaxpayerrecord);
            return db.Executs(sqllst);
        }



        public DataTable getSumPreCalculateTax(Dictionary<string, object> d)
        {
            //string sql = " select TaxCode,S_OrgCode,S_OrgName,CONVERT(varchar(10), S_WorkDate, 120)S_WorkDate,count(*) Person_Num,sum(T_YSHJ) T_YSHJ,sum(T_SRE) T_SRE,sum(K_SJZFGJJ) K_SJZFGJJ,sum(K_YangLaoBX) K_YangLaoBX,sum(K_YiLiaoBX)K_YiLiaoBX,sum(K_SYBX)K_SYBX,sum(K_ZFGJJ) K_ZFGJJ,sum(K_QYNJ) K_QYNJ,sum(T_SFGZ) T_SFGZ,sum(T_DYYSSDE) T_DYYSSDE,sum(T_YSSDE)T_YSSDE,sum(T_DJJE) T_DJJE,sum(WithholdingTax) WithholdingTax,sum(G_YCXJJ)G_YCXJJ,sum(K_YCXJJYKS)K_YCXJJYKS,sum(T_SE)T_SE,sum(K_KS)K_KS from View_PreCalculateTax ";
            //sql += " where 1=1  ";
            //if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            //{
            //    sql += " and S_Department like '%" + d["S_Department"].ToString() + "%'";
            //}
            //if (d.Keys.Contains("TaxCode") && d["TaxCode"] != null && d["TaxCode"].ToString() != "")
            //{
            //    sql += " and TaxCode = '" + d["TaxCode"].ToString() + "'";
            //}
            //if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            //{
            //    sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%' ";
            //}
            //if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            //{
            //    DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            //    sql += " and DATEDIFF(m, S_WorkDate, '" + bdate.ToString("yyyy-MM-dd") + "') = 0  ";
            //}
            //if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            //{
            //    sql += " and S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            //}
            //sql += "  GROUP BY TaxCode,S_OrgCode, S_OrgName, S_WorkDate  order by S_OrgCode ";
            //return db.GetDataTable(sql);
            IDataParameter[] parm = new SqlParameter[4];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkerName", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[0].Value = d["S_OrgCode"].ToString();
            DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            parm[1].Value = bdate.ToString("yyyy-MM-dd");

            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                parm[2].Value = d["S_WorkerName"].ToString();
            }
            else
            {
                parm[2].Value = "";
            }
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                parm[3].Value = d["S_Department"].ToString();
            }
            else
            {
                parm[3].Value = "";
            }
            DataSet ds = db.GetProcedure("pro_GroupPreCalTax", parm);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        public DataTable getPreCalculateTax(Dictionary<string, object> d)
        {
            //string sql = "select * from View_PreCalculateTax ";
            //sql += " where 1=1  ";
            //if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            //{
            //    sql += " and S_Department like '%" + d["S_Department"].ToString() + "%'";
            //}
            //if (d.Keys.Contains("TaxCode") && d["TaxCode"] != null && d["TaxCode"].ToString() != "")
            //{
            //    sql += " and TaxCode = '" + d["TaxCode"].ToString() + "'";
            //}
            //if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            //{
            //    sql += " and S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%' ";
            //}
            //if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            //{
            //    DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            //    sql += " and DATEDIFF(m, S_WorkDate, '" + bdate.ToString("yyyy-MM-dd") + "') = 0  ";
            //}
            //if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            //{
            //    sql += " and S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'";
            //}
            //sql += " order by S_WorkerCode  ";
            //return db.GetDataTable(sql);
            IDataParameter[] parm = new SqlParameter[4];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkerName", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[0].Value = d["S_OrgCode"].ToString();
            DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            parm[1].Value = bdate.ToString("yyyy-MM-dd");
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                parm[3].Value = d["S_Department"].ToString();
            }
            else
            {
                parm[3].Value = "";
            }
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                parm[2].Value = d["S_WorkerName"].ToString();
            }
            else
            {
                parm[2].Value = "";
            }

            DataSet ds = db.GetProcedure("pro_PreCalTax", parm);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// <summary>
        /// 月度个税明细查询
        /// </summary>
        /// <returns></returns>
        public DataTable getMonthTaxSalaryDetail(Dictionary<string, object> d, out int total)
        {
            IDataParameter[] parm = new SqlParameter[8];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkerName", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[4] = new SqlParameter("@pageSize", SqlDbType.Int);
            parm[5] = new SqlParameter("@pageIndex", SqlDbType.Int);
            parm[6] = new SqlParameter("@total", SqlDbType.Int);
            parm[7] = new SqlParameter("@TaxRate", SqlDbType.Decimal,18);
            parm[6].Direction = ParameterDirection.Output; //设定参数的输出方向  
            parm[0].Value = d["S_OrgCode"].ToString();
            parm[7].Value= d["TaxRate"];
            DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            parm[1].Value = bdate.ToString("yyyy-MM-dd");
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                parm[3].Value = d["S_Department"].ToString();
            }
            else
            {
                parm[3].Value = "";
            }
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                parm[2].Value = d["S_WorkerName"].ToString();
            }
            else
            {
                parm[2].Value = "";
            }
            if (d.Keys.Contains("limit") && d["limit"] != null && d["limit"].ToString() != "")
            {
                parm[4].Value = int.Parse(d["limit"].ToString());
            }
            else
            {
                parm[4].Value = 0;
            }
            if (d.Keys.Contains("page") && d["page"] != null && d["page"].ToString() != "")
            {
                parm[5].Value = int.Parse(d["page"].ToString());
            }
            else
            {
                parm[5].Value = 0;
            }
           // DataSet ds = db.GetProcedure("pro_MonthTaxSalary", parm);
            DataSet ds = db.GetProcedure("pro_MonthTaxSalary", parm);
            if (ds != null && ds.Tables.Count > 0)
            {
                total = Convert.ToInt32(parm[6].Value);
                return ds.Tables[0];

            }
            else
            {
                total = 0;
                return new DataTable();
            }
        }

        public DataTable getGroupMonthTaxSalary(Dictionary<string, object> d)
        {
            IDataParameter[] parm = new SqlParameter[5];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@TaxNumber", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[4] = new SqlParameter("@TaxRate", SqlDbType.Decimal, 18);
            parm[4].Value = d["TaxRate"];
            parm[0].Value = d["S_OrgCode"].ToString();
            DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            parm[1].Value = bdate.ToString("yyyy-MM-dd");
            //parm[2].Value = d["TaxNumber"].ToString();
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                parm[3].Value = d["S_Department"].ToString();
            }
            else
            {
                parm[3].Value = "";
            }
            if (d.Keys.Contains("TaxNumber") && d["TaxNumber"] != null && d["TaxNumber"].ToString() != "")
            {
                parm[2].Value = d["TaxNumber"].ToString();
            }
            else
            {
                parm[2].Value = "";
            }
            //if (d.Keys.Contains("TaxRate") && d["TaxRate"] != null && d["TaxRate"].ToString() != "")
            //{
            //    parm[4].Value = Convert.ToDecimal(d["TaxRate"]);
            //}
            //else
            //{
            //    parm[4].Value = -1;
            //}
            DataSet ds = db.GetProcedure("pro_GroupMonthTaxSalary", parm);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        public DataTable getGroupSumMonthTaxSalary(Dictionary<string, object> d)
        {
            IDataParameter[] parm = new SqlParameter[5];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@TaxNumber", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[4] = new SqlParameter("@TaxRate", SqlDbType.Decimal, 18);
            parm[0].Value = d["S_OrgCode"].ToString();
            parm[4].Value = d["TaxRate"];
            DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            parm[1].Value = bdate.ToString("yyyy-MM-dd");
            //parm[2].Value = d["TaxNumber"].ToString();
            if (d.Keys.Contains("TaxNumber") && d["TaxNumber"] != null && d["TaxNumber"].ToString() != "")
            {
                parm[2].Value = d["TaxNumber"].ToString();
            }
            else
            {
                parm[2].Value = "";
            }
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                parm[3].Value = d["S_Department"].ToString();
            }
            else
            {
                parm[3].Value = "";
            }
            DataSet ds = db.GetProcedure("pro_GroupSumMonthTaxSalary", parm);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        public DataTable getMonthTaxSalary(Dictionary<string, object> d, out int total)
        {
            IDataParameter[] parm = new SqlParameter[8];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            //parm[2] = new SqlParameter("@EWorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkerName", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[4] = new SqlParameter("@pageSize", SqlDbType.Int);
            parm[5] = new SqlParameter("@pageIndex", SqlDbType.Int);
            parm[6] = new SqlParameter("@total", SqlDbType.Int);
            parm[6].Direction = ParameterDirection.Output; //设定参数的输出方向
            parm[7] = new SqlParameter("@TaxRate", SqlDbType.Decimal, 18);
            parm[7].Value = d["TaxRate"];
            parm[0].Value = d["S_OrgCode"].ToString();
            DateTime bdate = DateTime.Now;
            bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            //DateTime edate = DateTime.Now;
            //if (d.Keys.Contains("S_BeginWorkDate") && d["S_BeginWorkDate"] != null && d["S_BeginWorkDate"].ToString() != "" && d.Keys.Contains("S_EndWorkDate") && d["S_EndWorkDate"] != null && d["S_EndWorkDate"].ToString() != "")
            //{
            //    bdate = Convert.ToDateTime(d["S_BeginWorkDate"].ToString());
            //    edate = Convert.ToDateTime(d["S_EndWorkDate"].ToString());
            //}
            //parm[1].Value = bdate.ToString("yyyy-MM-dd");
            //parm[2].Value = edate.ToString("yyyy-MM-dd");
            parm[1].Value = bdate.ToString("yyyy-MM-dd");
            if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            {
                parm[2].Value = d["S_WorkerName"].ToString();
            }
            else
            {
                parm[2].Value = "";
            }
            if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            {
                parm[3].Value = d["S_Department"].ToString();
            }
            else
            {
                parm[3].Value = "";
            }
            if (d.Keys.Contains("limit") && d["limit"] != null && d["limit"].ToString() != "")
            {
                parm[4].Value = int.Parse(d["limit"].ToString());
            }
            else
            {
                parm[4].Value = 0;
            }
            if (d.Keys.Contains("page") && d["page"] != null && d["page"].ToString() != "")
            {
                parm[5].Value = int.Parse(d["page"].ToString());
            }
            else
            {
                parm[5].Value = 0;
            }
            DataSet ds = db.GetProcedure("pro_SumMonthTaxSalary", parm);
            if (ds != null && ds.Tables.Count > 0)
            {
                total = Convert.ToInt32(parm[6].Value);
                return ds.Tables[0];
            }
            else
            {
                total = 0;
                return new DataTable();
            }
        }
    }
}
