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
    [Route("LogInfo")]
    public class LogController : WebApiBaseController
    {
        LogModule mm = new LogModule();
        /// <summary>
        /// 日志查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet("fetchLogInfoList")]
        public IActionResult fetchLogInfoList(string limit, string page, string USER_NAME, int? LOG_TYPE, string BEGIN_ACCESS_TIME,string END_ACCESS_TIME,string USER_ID,string LOG_CONTENT,string ALARM_LEVEL)
        {
            //UIDP.LOG.ClsSysLog aa = new UIDP.LOG.ClsSysLog();
            // aa.Info(DateTime.Now,"sdf","dsf","sdf",2,"sdf","sdf");
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["LOG_TYPE"] = LOG_TYPE;
            d["BEGIN_ACCESS_TIME"] = BEGIN_ACCESS_TIME;
            d["END_ACCESS_TIME"] = END_ACCESS_TIME;
            d["USER_ID"] = USER_ID;
            d["LOG_CONTENT"] = LOG_CONTENT;
            d["ALARM_LEVEL"] = ALARM_LEVEL;
            Dictionary<string, object> res = mm.fetchLogInfoList(d);
            return Json(res);
        }
    }
}