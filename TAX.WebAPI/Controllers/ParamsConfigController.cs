using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("ParamsConfig")]
    public class ParamsConfigController : WebApiBaseController
    {
        ParamsConfigModule PCM = new ParamsConfigModule();

        [HttpGet("getSubtrackStandardConfig")]
        public IActionResult getSubtrackStandardConfig()
        {
            Dictionary<string, object> res = PCM.getSubtrackStandardConfig();
            return Json(res);
        }
        [HttpPost("editConfig")]
        public IActionResult editConfig([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = PCM.editConfig(d);
            return Json(res);
        }

        [HttpPost("delConfig")]
        public IActionResult delConfig([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = PCM.delConfig(d);
            return Json(res);
        }

        [HttpPost("createConfig")]
        public IActionResult createConfig([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = PCM.createConfig(d);
            return Json(res);
        }

        /// <summary>
        /// 以上接口是针对减除项配置
        /// 以下接口是扣减项配置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //[HttpGet("getDecreasingConfig")]
        //public IActionResult getDecreasingConfig(string param,string S_OrgCode, string limit,string page)
        //{
        //    Dictionary<string, object> res = PCM.getDecreasingConfig(param, S_OrgCode,limit, page);
        //    return Json(res);
        //}
        /// <summary>
        /// 查询组织机构配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getDecreasingConfig")]
        public IActionResult getDecreasingConfig(string limit, string page, string DCode,  string TaxOffice)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["DCode"] = DCode;
            d["TaxOffice"] = TaxOffice;
            Dictionary<string, object> res = PCM.getDecreasingConfig(d);
            return Json(res);
        }
        [HttpPost("createDecreasingConfig")]
        public IActionResult createDecreasingConfig([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = PCM.createDecreasingConfig(d);
            return Json(res);
        }
        [HttpPost("delDecreasingConfig")]
        public IActionResult delDecreasingConfig(string S_ID)
        {
            Dictionary<string, object> res = PCM.delDecreasingConfig(S_ID);
            return Json(res);
        }
        [HttpPost("editDecreasingConfig")]
        public IActionResult editDecreasingConfig([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = PCM.editDecreasingConfig(d);
            return Json(res);
        }
        /// <summary>
        /// 获取字典项下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllDictionary")]
        public IActionResult getAllDictionary()
        {
            Dictionary<string, object> res = PCM.getAllDictionary();
            return Json(res);
        }


        [HttpGet("getTaxComputeconfig")]
        public IActionResult getTaxComputeconfig(string limit, string page)
        {
            Dictionary<string, object> res = PCM.getTaxComputeconfig(limit, page);
            return Json(res);
        }

        [HttpPost("updateTaxComputeconfig")]
        public IActionResult updateTaxComputeconfig([FromBody]JArray list)
        {
            List<Dictionary<string, object>> arr = list.ToObject<List<Dictionary<string, object>>>();
            Dictionary<string, object> r = PCM.updateTaxComputeconfig(arr);
            return Json(r);
        }
        /// <summary>
        /// 查询起征点5000
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("getTaxDateSub")]
        public IActionResult getTaxDateSub()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = PCM.getTaxDateSub();
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["QZD"] = dt.Rows[0]["QZD"];
                    r["code"] = 2000;
                    r["message"] = "查询成功！";
                }
                else {
                    r["QZD"] = 0;
                    r["code"] = -1;
                    r["message"] = "系统不存在起征点，请联系管理员！";
                }
            }
            catch (Exception ex)
            {
                r["QZD"] = 0;
                r["code"] = -1;
                r["message"] = "起征点查询失败，"+ex.Message;
            }
           
            return Json(r);
        }
       /// <summary>
       /// 设置起征点5000
       /// </summary>
       /// <param name="limit"></param>
       /// <param name="page"></param>
       /// <returns></returns>
        [HttpGet("updateTaxDateSub")]
        public IActionResult updateTaxDateSub(string userId, decimal QZD)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string res = PCM.updateTaxDateSub(userId, QZD);
                if (res=="") {
                    r["code"] = 2000;
                    r["message"] = "设置成功！";
                }
                else {
                    r["code"] = -1;
                    r["message"] = res;
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = "程序出错"+ex.Message;
            }
            

            return Json(r);
        }
    }
}