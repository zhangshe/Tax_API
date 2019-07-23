using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("ServiceRemunerationExport")]
    public class ServiceRemunerationExportController : WebApiBaseController
    {
        ServiceRemunerationExportModule SREM = new ServiceRemunerationExportModule();
        /// <summary>
        /// 查询各单位导入劳务工资情况
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="WorkDate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getOrgStatus")]
        public IActionResult getOrgStatus(string OrgCode, string WorkDate,  string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["TaxNumber"] = TaxNumber;
            d["page"] = page;
            d["limit"] = limit;
            Dictionary<string, object> res = SREM.getOrgStatus(d);
            return Json(res);
        }
        /// <summary>
        /// 按税号导出劳务明细
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="WorkDate"></param>
        /// <param name="TaxNumber"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("ExportServiceTaxDetail")]
        public IActionResult ExportServiceTaxDetail(string OrgCode, string WorkDate,  string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["TaxNumber"] = TaxNumber;
            Dictionary<string, object> res = SREM.ExportServiceTaxDetail(d);
            return Json(res);
        }
    }
}