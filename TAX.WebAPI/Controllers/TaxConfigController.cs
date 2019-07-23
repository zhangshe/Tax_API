using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;
using UIDP.BIZModule.Modules;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("TaxConfig")]
    public class TaxConfigController : WebApiBaseController
    {
        TaxConfigModule TC = new TaxConfigModule();
        [HttpGet("getLeftTree")]
        public dynamic getLeftTree()
        {
            //Dictionary<string, object> res = new Dictionary<string, object>();
            //try
            //{
            //    List<ConfigNode> nodeList = TC.getLeftTree();
            //    res["items"] = JsonConvert.SerializeObject(nodeList);
            //    res["message"] = "成功";
            //    res["code"] = 2000;
            //}
            //catch(Exception e)
            //{
            //    res["message"] = e.Message;
            //    res["code"] = -1;
            //}
            //return Json(res);
            List<ConfigNode> nodeList = TC.getLeftTree();
            return nodeList;
        }
        [HttpPost("editNode")]
        public IActionResult editNode([FromBody]JObject value )
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = TC.editNode(d);
            return Json(res);
        }

        [HttpPost("createNode")]
        public IActionResult createNode([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = TC.createNode(d);
            return Json(res);
        }

        [HttpPost("delNode")]
        public IActionResult delNode([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = TC.delNode(d);
            return Json(res);
        }


    }
}