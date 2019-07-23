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
    [Route("[controller]")]
    public class MenuController : WebApiBaseController
    {
        MenuModule mm = new MenuModule();
        // GET api/values
        [HttpGet("fetchMenuList")]
        public dynamic fetchMenuList(string sysCode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["sysCode"] = sysCode;

            List<ClsMenuInfo> c;
            try
            {
                c = mm.fetchMenuList(d);
               
            }
            catch (Exception )
            {
                c = new List<ClsMenuInfo>();
            }

            return JsonConvert.SerializeObject(c);

        }


        [HttpPost("createMenu")]
        public IActionResult createMenu([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                d["MENU_ID"] = Guid.NewGuid().ToString();
                string b = mm.createMenu(d);
                if (b == "")
                {
                    r["message"] = "成功";

                    r["code"] = 2000;

                    r["items"] = d;
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

        [HttpPost("updateMenu")]
        public IActionResult updateMenu([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.updateMenu(d);
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

        [HttpPost("deleteMenu")]
        public IActionResult deleteMenu([FromBody]JObject value)
        {

            Dictionary<string, List<string>> d = value.ToObject<Dictionary<string, List<string>>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.deleteMenu(d);
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

        
        // GET api/values
        [HttpGet("fetchRoleMenuList")]
        public IActionResult fetchRoleMenuList(string sysCode,string roleId)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["sysCode"] = sysCode;
            d["roleId"] = roleId;

            List<Dictionary<string, object>> c;
            try
            {
                c = mm.fetchRoleMenuList(d);

            }
            catch (Exception )
            {
                c = new List<Dictionary<string, object>>();
            }
            return Json(c);

        }

        [HttpPost("setRoleMenus")]
        public IActionResult setRoleMenus([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.setRoleMenus(d);
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
        [HttpGet("fetchPermission")]
        public dynamic fetchPermission(string sysCode, string userId)
        {

            Dictionary<string, object> d = new Dictionary<string, object>();
            d["sysCode"] = sysCode;
            d["userId"] = userId;
            List<ClsMenuInfo> c;
            try
            {
                c = mm.fetchPermission(d);

            }
            catch (Exception )
            {
                c = new List<ClsMenuInfo>();
            }
            return JsonConvert.SerializeObject(c);


        }

        



        //[HttpPost("Post")]
        //public IActionResult Post([FromBody]dynamic filed)
        //{
        //string sss = Request.Body.ToString();
        //Dictionary<string, object> d = new Dictionary<string, object>();
        //    try
        //    {
        //        d = JsonConvert.DeserializeObject<Dictionary<string, object>>(strQuery);
        //    }
        //    catch
        //    {

        //    }
        //    Filed ss = new Filed();
        //    ss.message = "ddddd";

        //    return Json(ss);
        //}

    }
}