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
    [Route("BusinessKonts")]
    public class BusinessKontsController : WebApiBaseController
    {
        BusinessKontModule BK = new BusinessKontModule();
        [HttpPost("MonthKonts")]
        public IActionResult MonthKonts([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = BK.MonthKonts(d);
            return Json(r);
        }

        [HttpGet("getCalculateData")]
        public IActionResult Get(string OrgCode, DateTime SysOperateDate,string UpdateBy) 
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(BK.getCalculateData(OrgCode, SysOperateDate));
            }
            catch (Exception e)
            {
                r["TaxStatus"] = -2;
                r["items"] = new DataTable();
                r["TaxPayerCount"] = 0;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        [HttpGet("judgeStatus")]
        public  IActionResult  judgeStatus(string OrgCode, DateTime SysOperateDate, string UpdateBy)
        {
            return Json(BK.judgeStatus(SysOperateDate));
        }

        [HttpGet("getJudgeCount")]
        public IActionResult getJudgeCount(string OrgCode, DateTime SysOperateDate, string UpdateBy)
        {
            return Json(BK.getJudgeCount(SysOperateDate));
        }
    }
}