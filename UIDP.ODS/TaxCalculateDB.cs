using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
   public class TaxCalculateDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 个税计算页面根据单位和当前工作月份查询数据状态
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="WorkMonth"></param>
        /// <returns></returns>
        public DataSet GetData(string OrgCode,DateTime WorkMonth) {
            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            string sql = "select a.ORG_ID,a.ORG_NAME,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,CASE WHEN((b.IsComputeTax = 0 OR b.IsComputeTax IS NULL) and c.ReportStatus is null ) THEN '1' ELSE isnull(c.ReportStatus,-1) END ReportStatus ";
            sql += "  from ts_uidp_org a ";
            sql += "   join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd")+"')=0 ";
            sql += " where a.ORG_CODE='"+ OrgCode + "' ";
          //  sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            sql += " union  ";
            sql += " select a.ORG_ID,a.ORG_NAME,a.ORG_CODE,isnull(b.IsComputeTax,0) IsComputeTax,ISNULL(c.ReportStatus,-1) ReportStatus   ";
            sql += " from ts_uidp_org a ";
            sql += "  join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE like '" + OrgCode + "'+'%' AND a.ORG_CODE<>'" + OrgCode + "' ";
           // sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";//以上是查部门
            sqllist.Add("taxorg",sql);
            sql = " select count(1) TaxPayerCount ";//一下是查缴纳人数
            sql += " from tax_salary a  ";
            sql += " where a.S_OrgCode in ( ";
            sql += " select a.ORG_CODE ";
            sql += "  from ts_uidp_org a ";
            sql += "   join tax_org  b on a.ORG_CODE=b.S_OrgCode ";
            sql += "  left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE='" + OrgCode + "' ";
          //  sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            sql += " union  ";
            sql += " select a.ORG_CODE ";
            sql += " from ts_uidp_org a ";
            sql += "  join tax_org  b on a.ORG_CODE=b.S_OrgCode  ";
            sql += " left join tax_reportstatus c on c.S_OrgCode=a.ORG_CODE and DATEDIFF(m,c.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sql += " where a.ORG_CODE like '" + OrgCode + "'+'%' AND a.ORG_CODE<>'" + OrgCode + "' ";
          //  sql += " and  exists (select  1  from tax_taxpayerinfo where S_OrgCode=a.ORG_CODE ) ";
            sql += " and not exists (select 1 from tax_org  d where a.ORG_CODE=d.S_OrgCode and d.IsComputeTax=1) ";
            sql += " ) and DATEDIFF(m,a.S_WorkDate,'" + WorkMonth.ToString("yyyy-MM-dd") + "')=0 ";
            sqllist.Add("taxpayercount", sql);
            return db.GetDataSet(sqllist);
        }
  /// <summary>
        /// 个税测算
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string calPreCalculateTax(Dictionary<string, object> d)
        {
            //           string w = @"	update a set T_DJJE = dbo.fn_TaxRateOfPersonalYear ( b.T_YSSDE ),K_YCXJJYKS = dbo.fn_TaxRateOfPersonalYear ( b.G_YCXJJ )  from tax_salary a,View_PreCalculateTax b
            // where a.S_WorkerCode = b.S_WorkerCode AND DATEDIFF( m, a.S_WorkDate, '2018-11-01' ) = 0 AND DATEDIFF( m, b.S_WorkDate, '2018-11-01' ) = 0 
            //AND a.S_OrgCode LIKE '100%' 	AND b.S_OrgCode LIKE '100%'";
            //StringBuilder sb = new StringBuilder();
            //sb.Append(" update a set  ");
            //sb.Append(" a.S_UpdateBy=");
            //sb.Append(GetIsNullStr(d["S_UpdateBy"].ToString()) + ", ");
            //sb.Append(" a.S_UpdateDate=");
            //sb.Append(GetIsNullStr(d["S_UpdateDate"].ToString()) + ", ");
            //sb.Append(" T_LJYJS = dbo.fn_TaxRateOfPersonalYear ( b.T_YSSDE,b.MonthNum ),");
            //sb.Append(" T_DJJE =case when IsDisability=1 then  CONVERT(DECIMAL(18,4),(dbo.fn_TaxRateOfPersonalYear ( b.T_YSSDE,b.MonthNum )-b.WithholdingTax)/2)  when IsDisability<>1 then dbo.fn_TaxRateOfPersonalYear ( b.T_YSSDE,b.MonthNum )-b.WithholdingTax  end,");
            //sb.Append(" T_BQSR =  b.T_SRE,");
            //sb.Append(" D_SL =  (select TaxRate from [dbo].[fn_getsubstructconfigyear]( b.T_YSSDE,b.MonthNum )),");
            //sb.Append(" D_SSKCS = (select TaxDeduction from [dbo].[fn_getsubstructconfigyear]( b.T_YSSDE,b.MonthNum )),");
            //sb.Append(" D_FYJCBZ = (select Deduction from [dbo].[fn_getsubstructconfigyear]( b.T_YSSDE,b.MonthNum )),");

            //sb.Append(" K_YCXJJYKS = dbo.fn_onetimecal ( b.G_YCXJJ )  from tax_salary a,View_PreCalculateTax b");

            ////sb.Append(" where S_Id=" + GetIsNullStr(d["S_Id"].ToString()));
            //sb.Append(" where a.S_WorkerCode = b.S_WorkerCode ") ;
            //if (d.Keys.Contains("S_Department") && d["S_Department"] != null && d["S_Department"].ToString() != "")
            //{
            //    sb.Append(" and a.S_Department like '%" + d["S_Department"].ToString() + "%'");
            //    sb.Append(" and b.S_Department like '%" + d["S_Department"].ToString() + "%'");
            //}
            //if (d.Keys.Contains("S_WorkerName") && d["S_WorkerName"] != null && d["S_WorkerName"].ToString() != "")
            //{
            //    sb.Append(" and a.S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%' ");
            //    sb.Append(" and b.S_WorkerName like '%" + d["S_WorkerName"].ToString() + "%' ");
            //}
            //if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            //{
            //    DateTime bdate = Convert.ToDateTime(d["S_WorkDate"].ToString());
            //    sb.Append(" and DATEDIFF(m, a.S_WorkDate, '" + bdate.ToString("yyyy-MM-dd") + "') = 0  ");
            //    sb.Append(" and DATEDIFF(m, b.S_WorkDate, '" + bdate.ToString("yyyy-MM-dd") + "') = 0  ");
            //}
            //if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            //{
            //    sb.Append(" and a.S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'");
            //    sb.Append(" and b.S_OrgCode like '" + d["S_OrgCode"].ToString() + "%'");
            //}
            //return db.ExecutByStringResult(sb.ToString());

            IDataParameter[] parm = new SqlParameter[6];
            parm[0] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@WorkDate", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkerName", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Department", SqlDbType.NVarChar, 50);
            parm[4] = new SqlParameter("@UpdateBy", SqlDbType.NVarChar, 50);
            parm[5] = new SqlParameter("@Output_Value", SqlDbType.NVarChar, 2000);
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                parm[0].Value = d["S_OrgCode"].ToString();
            }
            else
            {
                parm[0].Value = "";
            }
            if (d.Keys.Contains("S_WorkDate") && d["S_WorkDate"] != null && d["S_WorkDate"].ToString() != "")
            {
                parm[1].Value = d["S_WorkDate"].ToString();
            }
            else
            {
                parm[1].Value = "";
            }
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
            if (d.Keys.Contains("S_UpdateBy") && d["S_UpdateBy"] != null && d["S_UpdateBy"].ToString() != "")
            {
                parm[4].Value = d["S_UpdateBy"].ToString();
            }
            else
            {
                parm[4].Value = "";
            }
            string result = db.ExcuteProcedure("pro_PreCalculateTax", parm).ToString();
            return result;
        }

        public string GetIsNullStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "null";
            }
            else
            {
                return "'" + str.ToString() + "'";
            }
        }
        public string GetIsNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "0";
            }
            else
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// 计算个税
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="WorkMonth"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public string CalculateTax(string UserId, DateTime WorkMonth, string OrgCode) {
            IDataParameter[] parm = new SqlParameter[4];
            parm[0] = new SqlParameter("@UserId",SqlDbType.NVarChar,50);
            parm[1] = new SqlParameter("@WorkMonth", SqlDbType.DateTime);
            parm[2] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[3] = new SqlParameter("@Output_Value", SqlDbType.NVarChar, 2000);
            parm[0].Value = UserId;
            parm[1].Value = WorkMonth;
            parm[2].Value = OrgCode;
            parm[3].Value = "";
            string result = db.ExcuteProcedure("pro_CalculateTax",parm).ToString();
                return result;
        }
        /// <summary>
        /// 工资到算器
        /// </summary>
        /// <param name="taxNumber"></param>
        public DataTable GetInverseCalculate(decimal taxNumber) {
            string sql = "SELECT dbo.fn_InverseCalculate("+ taxNumber.ToString()+ ")";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 新加个税计算器
        /// </summary>
        /// <param name="taxNumber"></param>
        public DataTable cal(decimal nashuie)
        {
            string sql = "SELECT [dbo].[fn_TaxRateOfPersonal](" + nashuie.ToString() + ")";
            return db.GetDataTable(sql);
        } 
        /// <summary>
          /// 一次性奖金计算器
          /// </summary>
          /// <param name="taxNumber"></param>
        public DataTable onetimecal(decimal nashuie)
        {
            string sql = "SELECT [dbo].[fn_onetimecal](" + nashuie.ToString() + ")";
            return db.GetDataTable(sql);
        }
        public string createTaxSalary1(List<ImportTaxSalary1> d,  string userId, string guid)
        {
            List<string> sqllst = new List<string>();
            string fengefu = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@" insert into tax_calculatoryearsalary (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,G_GWJGZ,G_BLGZ,G_GLJT,G_SGJT,G_JSJNJT,G_ZFBT,G_BLJT,G_BYKT,
                                G_QTJT,G_YBJT,G_JBJDGZ,G_JCYJ,G_YJJJ,G_DSZNBJFS,G_WCBTSF,G_BFK,T_YFHJ,K_YiLiaoBX,K_SYBX,K_YangLaoBX,K_ZFGJJ,K_QYNJ,
                                K_QTKX,T_YSHJ,K_KS,T_SFHJ,Adjust1,Adjust2,Adjust3,Adjust4,Adjust5,Adjust6,Adjust7,Adjust8,S_WorkDate) values ");
                foreach (ImportTaxSalary1 item in d)
                {
                    sb.Append(fengefu + "('" + guid + "',");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
                    sb.Append("'" + userId + "',");
                    sb.Append("'" + "',");
                    sb.Append("'" +  "',");
                    sb.Append("'"  + "',");
                    sb.Append("'" + "',");
                    sb.Append(item.G_GWJGZ + ",");
                    sb.Append(item.G_BLGZ + ",");
                    sb.Append(item.G_GLJT + ",");
                    sb.Append(item.G_SGJT + ",");
                    sb.Append(item.G_JSJNJT + ",");
                    sb.Append(item.G_ZFBT + ",");
                    sb.Append(item.G_BLJT + ",");
                    sb.Append(item.G_BYKT + ",");
                    sb.Append(item.G_QTJT + ",");
                    sb.Append(item.G_YBJT + ",");
                    sb.Append(item.G_JBJDGZ + ",");
                    sb.Append(item.G_JCYJ + ",");
                    sb.Append(item.G_YJJJ + ",");
                    sb.Append(item.G_DSZNBJFS + ",");
                    sb.Append(item.G_WCBTSF + ",");
                    sb.Append(item.G_BFK + ",");
                    sb.Append(item.T_YFHJ + ",");
                    sb.Append(item.K_YiLiaoBX + ",");
                    sb.Append(item.K_SYBX + ",");
                    sb.Append(item.K_YangLaoBX + ",");
                    sb.Append(item.K_ZFGJJ + ",");
                    sb.Append(item.K_QYNJ + ",");
                    sb.Append(item.K_QTKX + ",");
                    sb.Append(item.T_YSHJ + ",");
                    sb.Append(item.K_KS + ",");
                    sb.Append(item.T_SFHJ + ",");
                    sb.Append(item.Adjust1 + ",");
                    sb.Append(item.Adjust2 + ",");
                    sb.Append(item.Adjust3 + ",");
                    sb.Append(item.Adjust4 + ",");
                    sb.Append(item.Adjust5 + ",");
                    sb.Append(item.Adjust6 + ",");
                    sb.Append(item.Adjust7 + ",");
                    sb.Append(item.Adjust8 + ",");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd") + "')");
                    fengefu = ",";
                }

            string sql = sb.ToString();
            sqllst.Add(sql);
            return db.Executs(sqllst);
        }

        public string createTaxSalary2(List<ImportTaxSalary2> d, string userId,string guid)
        {
            List<string> sqllst = new List<string>();
            string fengefu = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(@" insert into tax_calculatoryearsalary (S_Id,S_CreateDate,S_CreateBy,S_WorkerCode,S_WorkerName,S_OrgName,S_OrgCode,G_GWJGZ,G_GWJGZB,G_BLGZ,
                                G_GLJT,G_SGJT,G_JSJNJT,G_ZFBT,G_BLJT,G_BYKT,G_ZFJT,G_HMJT,G_JSJT,G_XFGZJT,G_FLGDZJT,G_BZRJT,
                                G_SYYLJT,G_YBJT,G_XQJB,G_PSJB,G_JRJB,G_JCYJ,G_YJJJ,G_ZENBFBK,G_ZXJ,G_C01,G_C02,G_C03,G_C04,T_XJ1,
                                G_DSZNJT,G_BJSPJT,G_FSJWF,G_WCBZ,G_TXBZ,G_JTBZ,G_HLHJYJ,G_C05,G_ZEWBFBK,G_LYF,T_XJ2,T_YFHJ,K_YiLiaoBX,
                                K_SYBX,K_YangLaoBX,K_ZFGJJ,K_QYNJ,K_QTKX,T_YSHJ,K_KS,T_SFHJ,Adjust1,Adjust2,Adjust3,Adjust4,Adjust5,Adjust6,Adjust7,Adjust8,S_WorkDate) values ");
                foreach (ImportTaxSalary2 item in d)
                {
                    sb.Append(fengefu + "('" + guid + "',");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
                    sb.Append("'" + userId + "',");
                    sb.Append("'" + "',");
                    sb.Append("'" +  "',");
                    sb.Append("'" +  "',");
                    sb.Append("'" +  "',");
                    sb.Append(item.G_GWJGZ + ",");
                    sb.Append(item.G_GWJGZB + ",");
                    sb.Append(item.G_BLGZ + ",");
                    sb.Append(item.G_GLJT + ",");
                    sb.Append(item.G_SGJT + ",");
                    sb.Append(item.G_JSJNJT + ",");
                    sb.Append(item.G_ZFBT + ",");
                    sb.Append(item.G_BLJT + ",");
                    sb.Append(item.G_BYKT + ",");
                    sb.Append(item.G_ZFJT + ",");
                    sb.Append(item.G_HMJT + ",");
                    sb.Append(item.G_JSJT + ",");
                    sb.Append(item.G_XFGZJT + ",");
                    sb.Append(item.G_FLGDZJT + ",");
                    sb.Append(item.G_BZRJT + ",");
                    sb.Append(item.G_SYYLJT + ",");
                    sb.Append(item.G_YBJT + ",");
                    sb.Append(item.G_XQJB + ",");
                    sb.Append(item.G_PSJB + ",");
                    sb.Append(item.G_JRJB + ",");
                    sb.Append(item.G_JCYJ + ",");
                    sb.Append(item.G_YJJJ + ",");
                    sb.Append(item.G_ZENBFBK + ",");
                    sb.Append(item.G_ZXJ + ",");
                    sb.Append(item.G_C01 + ",");
                    sb.Append(item.G_C02 + ",");
                    sb.Append(item.G_C03 + ",");
                    sb.Append(item.G_C04 + ",");
                    sb.Append(item.T_XJ1 + ",");
                    sb.Append(item.G_DSZNJT + ",");
                    sb.Append(item.G_BJSPJT + ",");
                    sb.Append(item.G_FSJWF + ",");
                    sb.Append(item.G_WCBZ + ",");
                    sb.Append(item.G_TXBZ + ",");
                    sb.Append(item.G_JTBZ + ",");
                    sb.Append(item.G_HLHJYJ + ",");
                    sb.Append(item.G_C05 + ",");
                    sb.Append(item.G_ZEWBFBK + ",");
                    sb.Append(item.G_LYF + ",");
                    sb.Append(item.T_XJ2 + ",");
                    sb.Append(item.T_YFHJ + ",");
                    sb.Append(item.K_YiLiaoBX + ",");
                    sb.Append(item.K_SYBX + ",");
                    sb.Append(item.K_YangLaoBX + ",");
                    sb.Append(item.K_ZFGJJ + ",");
                    sb.Append(item.K_QYNJ + ",");
                    sb.Append(item.K_QTKX + ",");
                    sb.Append(item.T_YSHJ + ",");
                    sb.Append(item.K_KS + ",");
                    sb.Append(item.T_SFHJ + ",");
                    sb.Append(item.Adjust1 + ",");
                    sb.Append(item.Adjust2 + ",");
                    sb.Append(item.Adjust3 + ",");
                    sb.Append(item.Adjust4 + ",");
                    sb.Append(item.Adjust5 + ",");
                    sb.Append(item.Adjust6 + ",");
                    sb.Append(item.Adjust7 + ",");
                    sb.Append(item.Adjust8 + ",");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd") + "')");
                    fengefu = ",";
                }
          
            string sql = sb.ToString();
            sqllst.Add(sql);
            return db.Executs(sqllst);
            //return db.ExecutByStringResult(sql);
        }

        /// <summary>
        /// 工资计算器
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="WorkMonth"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public string SalaryTaxCalculator(string guid,  string TaxOffice)
        {
            IDataParameter[] parm = new SqlParameter[3];
            parm[0] = new SqlParameter("@guid", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@TaxOffice", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@Output_Value", SqlDbType.NVarChar, 2000);
            parm[0].Value = guid;
            parm[1].Value = TaxOffice;
            parm[2].Value = "";
            string result = db.ExcuteProcedure("pro_ImportCalculateTax", parm).ToString();
            return result;
        }

        public DataTable Yearcal(decimal nashuie,int month)
        {
            string sql = "SELECT [dbo].[fn_TaxRateOfPersonalYear](" + nashuie.ToString() +","+month+ ")";
            return db.GetDataTable(sql);
        }
    }
}
