using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace UIDP.ODS
{
    public class TaxReportStatusDB
    {
        DBTool db = new DBTool("MYSQL");

        /// <summary>
        /// 获取组织机构报税状态
        /// </summary>
        /// <returns></returns>
        public DataTable getReportStatus(string orgCode,DateTime dateMonth)
        {
            string sql = @"select DISTINCT ISNULL(ReportStatus,-1) ReportStatus ,a.S_OrgCode,a.IsComputeTax from tax_org a
LEFT JOIN tax_reportstatus b
on a.S_OrgCode=b.S_OrgCode
                        where  a.S_OrgCode='" + orgCode + "' and (DATEDIFF(m, b.S_WorkDate, '" + dateMonth.ToString("yyyy-MM-dd") + "') = 0 or b.S_WorkDate is null)";
            return db.GetDataTable(sql);
        }
    }
}
