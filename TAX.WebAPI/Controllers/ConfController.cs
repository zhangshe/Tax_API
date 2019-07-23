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
    public class ConfController : WebApiBaseController
    {
        ConfModule mm = new ConfModule();
        /// <summary>
        /// 登陆页获取配置信息
        /// </summary>
        /// <param name="CONF_CODE">配置CODE</param>
        /// <returns></returns>
        [HttpGet("loginConfig")]
        public IActionResult loginConfig(string CONF_CODE)
            {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["CONF_CODE"] = CONF_CODE;
            return Json(mm.loginConfig(d));
        }
        /// <summary>
        /// 获取系统颜色
        /// </summary>
        /// <param name="CONF_CODE"></param>
        /// <returns></returns>
        [HttpGet("getColor")]
        public IActionResult getColor(string CONF_CODE)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["CONF_CODE"] = CONF_CODE;
            return Json(mm.ConfigColor(d));
        }
        // GET api/values
        [HttpGet("fetchConfigList")]
        public IActionResult fetchConfigList(string limit, string page, string CONF_NAME)
        {

            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["CONF_NAME"] = CONF_NAME;
            return Json(mm.fetchConfigList(d));
        }

        [HttpPost("createConfigArticle")]
        public IActionResult createConfigArticle([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();

          
            try
            {
                string b = mm.createConfigArticle(d);
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

        [HttpPost("updateConfigData")]
        public IActionResult updateConfigData([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.updateConfigData(d);
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

        [HttpPost("updateConfigArticle")]
        public IActionResult updateConfigArticle([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.updateConfigArticle(d);
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