using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("BusinessComplete")]
    public class BusinessCompleteController : WebApiBaseController
    {
        TaxReportModule TR = new TaxReportModule();
        [HttpPost("Report")]
        public IActionResult Report([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = TR.Report(d);
            return Json(res);
        }

        [HttpGet("getStatus")]
        public IActionResult getStatus(string S_OrgCode, string S_WorkDate)
        {
            Dictionary<string, object> res = TR.getStatus(S_OrgCode, S_WorkDate);
            return Json(res);
        }

        /// <summary>
        /// 上报查询
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_UpdateBy"></param>
        /// <param name="S_OrgName"></param>
        /// <param name="S_Id"></param>
        /// <returns></returns>
        [HttpGet("getCalculateData")]
        public IActionResult Get(string S_OrgCode, DateTime S_WorkDate, string S_UpdateBy,string S_OrgName,string S_Id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(TR.getCalculateData(S_OrgCode, S_WorkDate));
            }
            catch (Exception e)
            {
                r["TaxStatus"] = -2;
                r["items"] = JsonConvert.SerializeObject(new DataTable());
                r["TaxPayerCount"] = 0;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }

        [HttpGet("getDepartmentStatus")]
        public IActionResult getDepartmentStatus(string limit,string page,string S_OrgCode,string S_WorkDate,string ReportStatus)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["S_OrgCode"] = S_OrgCode;
            d["S_WorkDate"] = S_WorkDate;
            d["ReportStatus"] = ReportStatus;
            Dictionary<string, object> res = TR.getDepartmentStatus(d);
            return Json(res);
        }
        [HttpPost("unlock")]
        public IActionResult unlock ([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = TR.unlock(d);
            return Json(res);
        }

        [HttpPost("locking")]
        public IActionResult locking([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = TR.locking(d);
            return Json(res);
        }
    }
}