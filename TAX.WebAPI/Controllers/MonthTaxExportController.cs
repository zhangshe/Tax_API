using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("MonthTaxExport")]
    public class MonthTaxExportController : WebApiBaseController
    {
        UIDP.BIZModule.MonthTaxExportModule db = new UIDP.BIZModule.MonthTaxExportModule();
        /// <summary>
        /// 月度地税查询
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_Department"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="queryType"></param>
        /// <returns></returns>
        [HttpGet("getMonthTax")]
        public IActionResult getMonthTax(string S_OrgCode, string S_WorkDate,  string S_Department, string page, string limit, string queryType)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_Department"] = S_Department;
            d["page"] = page;
            d["limit"] = limit;
            d["queryType"] = queryType;
            Dictionary<string, object> res = db.getMonthTax(d);
            return Json(res);
        }
        /// <summary>
        /// 年度地税查询
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_Department"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="queryType"></param>
        /// <returns></returns>
        [HttpGet("getYearTax")]
        public IActionResult getYearTax(string S_OrgCode, string S_WorkDate, string S_Department, string page, string limit, string queryType)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_Department"] = S_Department;
            d["page"] = page;
            d["limit"] = limit;
            d["queryType"] = queryType;
            Dictionary<string, object> res = db.getYearTax(d);
            return Json(res);
        }


        [HttpGet("exportMonthTax")]
        public IActionResult exportMonthTax(string S_OrgCode, string S_WorkDate, string S_Department, string page, string limit, string queryType,string S_OrgName)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_Department"] = S_Department;
            d["page"] = page;
            d["limit"] = limit;
            d["queryType"] = queryType;
            d["S_OrgName"] = S_OrgName;
            Dictionary<string, object> res = db.ExportMonthTax(d);
            return Json(res);
        }

        [HttpGet("exportYearTax")]
        public IActionResult exportYearTax(string S_OrgCode, string S_WorkDate, string S_Department, string page, string limit, string queryType,string S_OrgName)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_Department"] = S_Department;
            d["S_OrgName"] = S_OrgName;
            d["page"] = page;
            d["limit"] = limit;
            d["queryType"] = queryType;
            Dictionary<string, object> res = db.ExportYearTax(d);
            return Json(res);
        }
    }
}