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
    [Route("SyncConf")]
    public class SyncConfController : WebApiBaseController
    {
        SyncConfModule mm = new SyncConfModule();
        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchSyncConfList")]
        public IActionResult fetchSyncConfList(string limit, string page, string SERVER_IP,string SERVER_PORT,string SERVER_URL,string SYNC_FLAG,string REMARK,string USER_CODE, string SYNC_TYPE)
        {
            //UserModule user = new UserModule();
            //string Admin = user.getAdminCode();
            //bool isAdmin = UserId.Equals(Admin);
            //Dictionary<string, object> res = mm.GetPagedTable(isAdmin);
            //return Json(res);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["SERVER_IP"] = SERVER_IP;
            d["SERVER_PORT"] = SERVER_PORT;
            d["SERVER_URL"] = SERVER_URL;
            d["SYNC_FLAG"] = SYNC_FLAG;
            d["REMARK"] = REMARK;
            d["USER_CODE"] = USER_CODE;
            d["SYNC_TYPE"] = SYNC_TYPE;
            Dictionary<string, object> res = mm.fetchSyncConfList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createSyncConf")]
        public IActionResult createSyncConf([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createSyncConf(d);
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
        [HttpPost("updateSyncConf")]
        public IActionResult updateSyncConf([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateSyncConf(d);
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
        [HttpPost("deleteSyncConf")]
        public IActionResult deleteSyncConf(string SYNC_ID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            //Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            try
            {
                string b = mm.deleteSyncConf(SYNC_ID);
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