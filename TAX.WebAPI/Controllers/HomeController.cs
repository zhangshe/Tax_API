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
    [Route("Home")]
    public class HomeController : WebApiBaseController
    {
        HomeModule HM = new HomeModule();

        [HttpGet("getMonthData")]
        public IActionResult getMonthData(string orgcode, string systime, string limit, string page)
        {
            Dictionary<string, object> res = HM.getMonthData(orgcode, systime);
            return Json(res);
        }

        [HttpGet("getNotice")]
        public IActionResult getNotice(string orgcode, string systime,string limit,string page,string id)
        {
            Dictionary<string, object> res = HM.getNotice(limit,page,id);
            return Json(res);
        }

        [HttpGet("getNoticeDetail")]
        public IActionResult getNoticeDetail(string orgcode, string systime, string limit, string page, string id)
        {
            Dictionary<string, object> res = HM.getNoticeDetail(id);
            return Json(res);
        }


        [HttpGet("getLv")]
        public IActionResult getLv(string orgcode, string systime, string limit, string page)
        {
            Dictionary<string, object> res = HM.getLv(orgcode, systime);
            return Json(res);
        }

        [HttpGet("CompareData")]
        public IActionResult CompareData(string orgcode, string systime, string limit, string page)
        {
            Dictionary<string, object> res = HM.CompareData(orgcode, systime);
            return Json(res);
        }
    }
}