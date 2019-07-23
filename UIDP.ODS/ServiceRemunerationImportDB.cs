using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;
using UIDP.UTILITY.ExcelOperation.Model;

namespace UIDP.ODS
{
    public class ServiceRemunerationImportDB
    {
        DBTool dB = new DBTool("");

        public DataTable getlist(string S_OrgCode, DateTime S_WorkDate, string id, string workNumber,int flag)
        {
            string sql = " SELECT a.*,b. ORG_NAME FROM tax_serviceremuneration a ";
            sql += " LEFT JOIN ts_uidp_org b ON a.ImportOrgCode=b.ORG_CODE";
            //sql += " and Create_By='" + id + "'";
            sql += " WHERE 1=1 ";
            sql += " and datediff(mm,a.WorkDate,'" + S_WorkDate + "')=0";
            if (flag == 1)
            {
                sql += " and a.ImportOrgCode ='" + S_OrgCode +"'";
            }
            else if (flag == 2)
            {
                sql += " and a.ImportOrgCode like '" + S_OrgCode + "%'";
            }
            if (!string.IsNullOrEmpty(workNumber))
            {
                sql += " and a.WorkerCode='" + workNumber + "'";
            }
            return dB.GetDataTable(sql);
        }

        public string createImportData(List<importService> list, string orgCode, string orgName, DateTime dateMonth, string userId)
        {
            int num =10;//分页条数
            int i = 0;//循环判断数
            StringBuilder sb = new StringBuilder();
            string sqlHeader = " INSERT INTO tax_serviceremuneration (ID,Create_Date,Create_By,WorkDate,WorkerCode,WorkerName,IDtype,IDNumber,IncomeItem,Tax,Income," +
                "CommercialHealthinsurance,EndowmentInsurance,Donation,other,TaxSavings,Remark,ImportOrgCode) VALUES";
            foreach(importService item in list)
            {
                if (i % num == 0)
                {
                    sb.Append(sqlHeader);
                }
                sb.Append("('" + Guid.NewGuid() + "',");
                sb.Append("'" + DateTime.Now.ToString("yyyyMMdd") + "',");
                sb.Append("'" + userId + "',");
                sb.Append("'" + dateMonth.ToString("yyyyMMdd") + "',");
                sb.Append(isstr(item.WorkerCode));
                sb.Append(isstr(item.WorkerName));
                sb.Append(isstr(item.IDtype));
                sb.Append(isstr(item.IDNumber));
                sb.Append(isstr(item.IncomeItem));
                sb.Append(isnum(item.Tax));
                sb.Append(isnum(Convert.ToDecimal(item.Income)));
                sb.Append(isnum(item.CommercialHealthinsurance));
                sb.Append(isnum(item.EndowmentInsurance));
                sb.Append(isnum(item.Donation));
                sb.Append(isnum(item.other));
                sb.Append(isnum(item.TaxSavings));
                sb.Append(isstr(item.Remark));
                sb.Append("'" + orgCode + "')");
                i++;
                if (i % num == 0||item.Equals(list[list.Count-1]))
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(",");
                }
            }
            string sql = sb.ToString();
            return dB.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 获取当月已经存在的劳务报酬信息条数
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable getcount(string S_OrgCode, DateTime S_WorkDate, string id)
        {
            string sql = " SELECT COUNT(*) as num FROM tax_serviceremuneration WHERE 1=1";
            sql += " AND DATEDIFF(MM,WorkDate,'" + S_WorkDate.ToString("yyyyMMdd") + "')=0";
            sql += " AND Create_By='" + id + "'";
            sql += " AND ImportOrgCode='" + S_OrgCode + "'";
            return dB.GetDataTable(sql);
        }

        public string delData(string orgCode, DateTime dateMonth, string id)
        {
            string sql = " DELETE FROM tax_serviceremuneration WHERE 1=1";
            sql += " AND DATEDIFF(MM,WorkDate,'" + dateMonth.ToString("yyyyMMdd") + "')=0";
            sql += " AND Create_By='" + id + "'";
            //sql += " AND ImportOrgCode='" + orgCode + "'";
            return dB.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 文字类型数据通过本方法构造sql语句
        /// </summary>
        /// <returns></returns>
        public string isstr(string str)
        {
            return "'" + str + "',";
        }
        public string isnum(decimal num)
        {
            return num.ToString() + ",";
        }

        public DataTable getReportState(string OrgCode, DateTime WorkDate)
        {
            string sql = " SELECT * FROM tax_reportstatus WHERE S_OrgCode='" + OrgCode + "'";
            sql += " AND DATEDIFF(MM,S_WorkDate,'" + WorkDate.ToString("yyyyMMdd") + "')=0";
            return dB.GetDataTable(sql);
        }
    }
}
