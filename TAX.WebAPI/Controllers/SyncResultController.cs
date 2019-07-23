using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.Internal;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("SyncResult")]
    public class SyncResultController : WebApiBaseController
    {
        SyncResultModule mm = new SyncResultModule();
        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchSyncResultList")]
        public IActionResult fetchSyncResultList(string limit, string page, string SEND_URL, string RECEIVE_URL, string SYNC_CONTENT, string SYNC_RESULT,string ERROR_INFO, string REMARK)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["SEND_URL"] = SEND_URL;
            d["RECEIVE_URL"] = RECEIVE_URL;
            d["SYNC_CONTENT"] = SYNC_CONTENT;
            d["SYNC_RESULT"] = SYNC_RESULT;
            d["ERROR_INFO"] = ERROR_INFO;
            d["REMARK"] = REMARK;
            Dictionary<string, object> res = mm.fetchSyncResultList(d);
            return Json(res);
        }

        [HttpPost("createSyncResult")]
        public IActionResult createSyncResult(Dictionary<string, object> d)
        {
            Dictionary<string, object> res = mm.createSyncResult(d);
            return Json(res);
        }

    }
}