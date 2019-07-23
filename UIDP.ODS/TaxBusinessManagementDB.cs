using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;
using UIDP.UTILITY.ExcelOperation.Model;

namespace UIDP.ODS
{
    public class TaxBusinessManagementDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 获取金税导入信息
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <returns></returns>
        //public DataTable getTaxInfo(string importOrgCode, DateTime dateMonth,string id,string workerNumber)
        //{
        //    string sql = "SELECT * FROM tax_specialdeductions ";
        //    sql += " WHERE 1=1";
        //    //sql += " AND a.ImportOrgCode like'" + importOrgCode + "%'";
        //    if (workerNumber != null && workerNumber != "")
        //    {
        //        sql += " AND S_WorkNumber='" + workerNumber + "'";
        //    }
        //    sql += " AND CreateBy='" + id + "'";
        //    sql += " AND DATEDIFF(mm, S_WorkDate, '"+dateMonth+"')=0";
            
        //    return db.GetDataTable(sql);
        //}

        public DataTable judgeTaxInfo(DateTime dateMonth)
        {
            string sql = "SELECT a.* FROM tax_specialdeductions a";
            sql += " WHERE 1=1";
            sql += " AND a.S_WorkDate='" + dateMonth + "'";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 因为金税明细查询太慢，所以把数据存入临时表中，并进行查询
        /// </summary>
        /// <param name="importOrgCode"></param>
        /// <param name="dateMonth"></param>
        /// <param name="id"></param>
        /// <param name="workerNumber"></param>
        /// <returns></returns>
        public DataSet createTemp(string importOrgCode, DateTime dateMonth, string id, string workerNumber,int limit,int page)
        {
            Dictionary<string, string> sqllist = new Dictionary<string, string>();
            string sql = " SELECT ROW_NUMBER () OVER ( ORDER BY S_WorkNumber ) AS non,a.* INTO #temp FROM tax_specialdeductions a ";
            sql += " WHERE 1=1";
            if (workerNumber != null && workerNumber != "")
            {
                sql += " AND S_WorkNumber='" + workerNumber + "'";
            }
            sql += " AND CreateBy='" + id + "'";
            sql += " AND DATEDIFF(mm, S_WorkDate, '" + dateMonth + "')=0";
            sql+= " SELECT * FROM #temp WHERE non between " + ((page - 1) * limit+1) + " AND " + page * limit;
            string sql1 = " SELECT COUNT(*) FROM #temp";
            sqllist.Add("data", sql);
            sqllist.Add("total", sql1);
            return db.GetDataSet(sqllist);
        }
        public DataTable getConfig()
        {
            string sql = "SELECT Code,Name FROM tax_dictionary where ParentCode='ZJLX'";
            return db.GetDataTable(sql);
        }

        public string createTaxBusiness(List<ImportTaxBusiness>list, DateTime dateMonth,string userId, string importOrgCode, string importOrgName)
        {
            //string fengefu = "";
            //int i = 0;//循环数，用于判断添加SQL语句头部；
            //const int Num= 699;//Insert插入分条数目
            //string headersql = "INSERT INTO tax_specialdeductions (S_Id,CreateBy,CreateDate,ImportOrgCode,S_WorkName,S_WorkNumber,IDtype,IDNumber,income,Tax,OlderInsurance,HeathInsurance,JobInsurance,HousingFund," +
            //    "ChildEdu,HousingLoan,HousingRent,Support,ContinueEdu,EnterpriseAnnuity,CommercialHealthinsurance,EndowmentInsurance,Other,Donation,Deductions," +
            //    "TaxSavings,Reduction,WithholdingTax,Remark,StartTime,EndTime,S_WorkDate,AccumulatedIncome,AccumulatedSpecialDeduction,CumulativeOther,TaxRate,QuickDeduction," +
            //    "AccumulatedTax,CumulativeWithholding,Drawback,IncomeItem,Cost) VALUES";
            //StringBuilder sb = new StringBuilder();
            //foreach (var item in list)
            //{
            //    if (i%Num==0)
            //    {
            //        sb.Append(headersql);
            //    }  
            //    sb.Append("('" + Guid.NewGuid() + "',");
            //    sb.Append("'" + userId + "',");
            //    sb.Append("'" + DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss") + "',");
            //    sb.Append("'" + importOrgCode + "',");
            //    sb.Append("'" + item.Name + "',");
            //    sb.Append("'" + item.WorkNumber + "',");
            //    sb.Append("'" + item.IDType + "',");
            //    sb.Append("'" + item.IDNumber + "',");     
            //    sb.Append(item.Income + ",");
            //    sb.Append(item.Tax + ",");
            //    sb.Append(item.OlderInsurance + ",");
            //    sb.Append(item.HeathInsurance + ",");
            //    sb.Append(item.JobInsurance + ",");
            //    sb.Append(item.HousingFund + ",");
            //    sb.Append(item.ChildEdu + ",");
            //    sb.Append(item.HousingLoan + ",");
            //    sb.Append(item.HousingRent + ",");
            //    sb.Append(item.Support + ",");
            //    sb.Append(item.ContinueEdu + ",");
            //    sb.Append(item.EnterpriseAnnuity + ",");
            //    sb.Append(item.CommercialHealthinsurance + ",");
            //    sb.Append(item.EndowmentInsurance + ",");
            //    sb.Append(item.Other + ",");
            //    sb.Append(item.Donation + ",");
            //    sb.Append(item.Deductions + ",");
            //    sb.Append(item.TaxSavings + ",");
            //    sb.Append(item.Reduction + ",");
            //    sb.Append(item.WithholdingTax + ",");
            //    sb.Append("'"+item.Remark + "',");
            //    sb.Append("'" + item.StartTime + "',");
            //    sb.Append("'" + item.EndTime + "',");
            //    sb.Append("'" + dateMonth + "',");
            //    sb.Append(item.AccumulatedIncome + ",");
            //    sb.Append(item.AccumulatedSpecialDeduction + ",");
            //    sb.Append(item.CumulativeOther + ",");
            //    sb.Append(item.TaxRate + ",");
            //    sb.Append(item.QuickDeduction + ",");
            //    sb.Append(item.AccumulatedTax + ",");
            //    sb.Append(item.CumulativeWithholding + ",");
            //    sb.Append(item.Drawback + ",");
            //    sb.Append("'"+item.IncomeItem + "',");
            //    sb.Append(item.Cost + ") ");
            //    i++;
            //    if (i% Num == 0||item.Equals(list[list.Count-1]))
            //    {
            //        sb.Append(fengefu);
            //    }
            //    else
            //    {
            //        sb.Append(",");
            //    }   
            //}
            //string sql = sb.ToString();
            //return db.ExecutByStringResult(sql);
            ImportSqlServer importSql = new ImportSqlServer();
            string sql = "SELECT * from tax_specialdeductions where CreateBy='构造新的dt'";
            DataTable dt = db.GetDataTable(sql);
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr["S_Id"] = Guid.NewGuid();
                dr["CreateBy"] = userId;
                dr["CreateDate"] = DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss");
                dr["S_WorkName"] = item.Name;
                dr["S_WorkNumber"] = item.WorkNumber;
                dr["IDType"] = item.IDType;
                dr["IDNumber"] = item.IDNumber;
                dr["Income"] = item.Income;
                dr["Tax"] = item.Tax;
                dr["OlderInsurance"] = item.OlderInsurance;
                dr["HeathInsurance"] = item.HeathInsurance;
                dr["JobInsurance"] = item.JobInsurance;
                dr["HousingFund"] = item.HousingFund;
                dr["ChildEdu"] = item.ChildEdu;
                dr["HousingLoan"] = item.HousingLoan;
                dr["HousingRent"] = item.HousingRent;
                dr["Support"] = item.Support;
                dr["ContinueEdu"] = item.ContinueEdu;
                dr["EnterpriseAnnuity"] = item.EnterpriseAnnuity;
                dr["CommercialHealthinsurance"] = item.CommercialHealthinsurance;
                dr["EndowmentInsurance"] = item.EndowmentInsurance;
                dr["Other"] = item.Other;
                dr["Donation"] = item.Donation;
                dr["Deductions"] = item.Deductions;
                dr["TaxSavings"] = item.TaxSavings;
                dr["Reduction"] = item.Reduction;
                dr["WithholdingTax"] = item.WithholdingTax;
                dr["Remark"] = item.Remark;
                dr["S_WorkDate"] = dateMonth;
                dr["ImportOrgCode"] = importOrgCode;
                dr["StartTime"] = item.StartTime;
                dr["EndTime"] = item.EndTime;
                dr["AccumulatedIncome"] = item.AccumulatedIncome;
                dr["AccumulatedSpecialDeduction"] = item.AccumulatedSpecialDeduction;
                dr["CumulativeOther"] = item.CumulativeOther;
                dr["TaxRate"] = item.TaxRate;
                dr["QuickDeduction"] = item.QuickDeduction;
                dr["AccumulatedTax"] = item.AccumulatedTax;
                dr["CumulativeWithholding"] = item.CumulativeWithholding;
                dr["Drawback"] = item.Drawback;
                dr["IncomeItem"] = item.IncomeItem;
                dr["Cost"] = item.Cost;
                dt.Rows.Add(dr);
            }
            string msg=importSql.Import(dt, "tax_specialdeductions_copy1");
            if (msg == "2000")
            {
                List<string> sqllist = new List<string>();
                string sql1 = "INSERT INTO tax_specialdeductions SELECT * FROM tax_specialdeductions_copy1";
                sql1 += " WHERE S_Id NOT IN";
                sql1 += "  (SELECT S_Id FROM tax_specialdeductions WHERE DATEDIFF(mm, S_WorkDate, '" + dateMonth + "')=0)";
                string sql2 = " DELETE FROM tax_specialdeductions_copy1";
                sqllist.Add(sql1);
                sqllist.Add(sql2);
                return db.Executs(sqllist);
            }
            else
            {
                return "失败";
            }
        }
        public string delTaxBusiness(Dictionary<string,object> d)
        {
            string sql = "DELETE FROM tax_specialdeductions WHERE CreateBy='" + d["id"] + "'" + " AND DATEDIFF(mm,S_WorkDate,'" + d["dateMonth"] + "')=0";
            return db.ExecutByStringResult(sql);
        }

        public DataTable getCount(string S_OrgCode, DateTime S_WorkDate, string id)
        {
            string sql= " SELECT COUNT(*) as num FROM tax_specialdeductions WHERE CreateBy='" + id + "'" + " AND DATEDIFF(mm,S_WorkDate,'" + S_WorkDate + "')=0";
            return db.GetDataTable(sql);
        }
    }
}
