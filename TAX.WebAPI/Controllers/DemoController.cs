using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIDP.BIZModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class DemoController :Controller
    {
        DemoModule mm = new DemoModule();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="CONF_CODE"></param>
        /// <returns></returns>
        [HttpGet("fetchDemoList")]
        public IActionResult fetchDemoList(string limit, string page, string NAME)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["NAME"] = NAME;
            return Json(mm.fetchDemoList(d));
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createDemoArticle")]
        public IActionResult createDemoArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createDemoArticle(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateDemoData")]
        public IActionResult updateDemoData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateDemoData(d);
                if (b == "")
                {
                    r["message"] = "成功";

                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }

            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateDemoArticle")]
        public IActionResult updateDemoArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateDemoArticle(d);
                if (b == "")
                {
                    r["message"] = "成功";

                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }

            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);


        }

    }
}