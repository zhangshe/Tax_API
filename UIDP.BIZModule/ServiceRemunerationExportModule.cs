using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
     public class ServiceRemunerationExportModule
    {
        ServiceRemunerationExportDB dB = new ServiceRemunerationExportDB();
        /// <summary>
        /// 查询各单位导入的劳务情况
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> getOrgStatus(Dictionary<string, object> d)
        {
            int page = int.Parse(d["page"].ToString());
            int limit = int.Parse(d["limit"].ToString());
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = dB.getOrgStatus(d);
                if (dt != null&&dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["items"] = new DataTable();
                    r["total"] = 0;
                    r["message"] = "成功,无数据！";
                    r["code"] = 2000;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        /// <summary>
        /// 按税号导出当月劳务明细
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> ExportServiceTaxDetail(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();//导入结果
            try
            {
                DataTable dt = dB.ExportServiceTaxDetail(d);
                if (dt != null)
                {
                    List<string> col = new List<string>() {
                       "WorkerCode","WorkerName","IDType","IDNumber","IncomeItem","Income","Tax","CommercialHealthinsurance","EndowmentInsurance",
"Donation","other","TaxSavings","Remark"
                    };
                    r["item"] = ExcelTools.ExportByTemplet(dt, "劳务报酬明细", "", "劳务报酬明细表", 1, 13, col);
                    r["code"] = 2000;
                    r["message"] = "";
                }
                else
                {
                    r["message"] = "暂无导出数据";
                    r["code"] = -1;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return r;
        }
    }
}
