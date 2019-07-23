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
    [Route("Export")]
    public class ExportController : WebApiBaseController
    {
        ExportModule mm = new ExportModule();
        [HttpGet("getExportPayerInfo")]
        public IActionResult getExportPayerInfo(string OrgCode,string WorkDate,int Status,string TaxNumber, string page,string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            d["page"] = page;
            d["limit"] = limit;
            Dictionary<string, object> res = mm.getExportPayerInfo(d);
            return Json(res);
        }

        [HttpGet("getExportSalary")]
        public IActionResult getExportSalary(string OrgCode, string WorkDate, int Status, string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            Dictionary<string, object> res = mm.getExportSalary(d);
            return Json(res);
        }
        /// <summary>
        /// 金税导出工资zp19.1.30
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportSalary")]
        public IActionResult exportSalary(string OrgCode, string WorkDate, int Status, string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            Dictionary<string, object> res = mm.ExportSalary(d);
            return Json(res);
        }
        [HttpGet("getExportOnceBonus")]
        public IActionResult getExportOnceBonus(string OrgCode, string WorkDate, int Status, string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            Dictionary<string, object> res = mm.getExportOnceBonus(d);
            return Json(res);
        }
        [HttpGet("exportOnceBonus")]
        public IActionResult exportOnceBonus(string OrgCode, string WorkDate, int Status, string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            Dictionary<string, object> res = mm.ExportOnceBonus(d);
            return Json(res);
        }
        /// <summary>
        /// 获取当前单位状态
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="WorkDate"></param>
        /// <param name="Status"></param>
        /// <param name="param"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getOrgStatus")]
        public IActionResult getOrgStatus(string OrgCode, string WorkDate, int? Status, string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            d["page"] = page;
            d["limit"] = limit;
            Dictionary<string, object> res = mm.getOrgStatus(d);
            return Json(res);
        }


        [HttpGet("getTaxNumberOptions")]
        public IActionResult getTaxNumberOptions(string OrgCode, string WorkDate, int Status, string TaxNumber, string page, string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["OrgCode"] = OrgCode;
            d["WorkDate"] = WorkDate;
            d["Status"] = Status;
            d["TaxNumber"] = TaxNumber;
            d["page"] = page;
            d["limit"] = limit;
            Dictionary<string, object> res = mm.getTaxNumberOptions(d);
            return Json(res);
        }
    }
}