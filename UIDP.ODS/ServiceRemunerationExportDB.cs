using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class ServiceRemunerationExportDB
    {
        DBTool dB = new DBTool("");
        /// <summary>
        /// 查询各单位导入的劳务情况
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getOrgStatus(Dictionary<string, object> d) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" select distinct c.ORG_CODE S_OrgCode,c.ORG_NAME S_OrgName,'已导入' ReportStatus from [dbo].[tax_serviceremuneration] a ");
            sb.AppendLine(" join [dbo].[tax_org] b on a.ImportOrgCode=b.S_OrgCode and b.TaxCode='" + d["TaxNumber"] + "' and b.S_OrgCode like '" + d["OrgCode"] + "%'");
            sb.AppendLine(" join ts_uidp_org c on b.S_OrgCode=c.ORG_CODE ");
            sb.AppendLine(" WHERE DATEDIFF(m,a.WorkDate,'"+d["WorkDate"] +"')=0 ");
            sb.AppendLine(" order by c.ORG_CODE  ");
            return dB.GetDataTable(sb.ToString());
        }
        /// <summary>
        /// 按税号导出当月劳务明细
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable ExportServiceTaxDetail(Dictionary<string, object> d) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" select a.WorkerCode,a.WorkerName,a.IDType,a.IDNumber,a.IncomeItem,a.Income,a.Tax,a.CommercialHealthinsurance,a.EndowmentInsurance, ");
            sb.AppendLine(" a.Donation,a.other,a.TaxSavings,a.Remark ");
            sb.AppendLine(" from [dbo].[tax_serviceremuneration] a ");
            sb.AppendLine(" join [dbo].[tax_org] b on a.ImportOrgCode=b.S_OrgCode and b.TaxCode='" + d["TaxNumber"] + "' and b.S_OrgCode like '"+d["OrgCode"] +"%'");
            sb.AppendLine(" join ts_uidp_org c on b.S_OrgCode=c.ORG_CODE ");
            sb.AppendLine(" WHERE DATEDIFF(m,a.WorkDate,'" + d["WorkDate"] + "')=0 ");
            sb.AppendLine(" order by c.ORG_CODE  ");
            return dB.GetDataTable(sb.ToString());
        }
    }
}
