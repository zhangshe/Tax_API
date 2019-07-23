using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("YearAverCheck")]
    public class YearAverCheckController : WebApiBaseController
    {
        UIDP.BIZModule.YearAverCheckModule db = new UIDP.BIZModule.YearAverCheckModule();
        [HttpGet("getYearAverCheck")]
        public IActionResult getYearAverCheck(string S_OrgCode, string S_WorkDate, string S_WorkerName, string S_Department, string page, string limit,string queryType)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_WorkerName"] = S_WorkerName;
            d["S_Department"] = S_Department;
            d["page"] = page;
            d["limit"] = limit;
            d["queryType"] = queryType;
            Dictionary<string, object> res = db.getYearAverCheck(d);
            return Json(res);
        }

        [HttpGet("exportYearAverTax")]
        public IActionResult exportYearAverTax(string S_OrgCode, string S_WorkDate, string S_WorkerName, string S_Department, string page, string limit, string queryType,string S_OrgName)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_WorkerName"] = S_WorkerName;
            d["S_Department"] = S_Department;
            d["page"] = page;
            d["limit"] = limit;
            d["queryType"] = queryType;
            d["S_OrgName"] = S_OrgName;
            Dictionary<string, object> res = db.ExportYearAverTax(d);
            return Json(res);
        }

    }
}