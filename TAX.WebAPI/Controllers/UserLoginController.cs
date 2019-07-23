using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule;
using Newtonsoft.Json.Linq;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("UserLogin")]
    public class UserLoginController : WebApiBaseController
    {
        UserLoginModule mm = new UserLoginModule();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchUserLoginList")]
        public IActionResult fetchUserLoginLists(string limit, string page,  string sort, string LOGIN_REMARK = "")
        {
            Dictionary<string, object> res = mm.fetchUserLoginList(limit,  page,  LOGIN_REMARK,  sort);
            return Json(res);
        }
        ///// <summary>
        ///// 弹窗查询
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("fetchUserForLoginList")]
        //public IActionResult fetchUserForLoginLists(string limit, string page, string LOGIN_REMARK="", string sort="")
        //{
        //    Dictionary<string, object> res = mm.fetchUserForLoginList(limit, page, LOGIN_REMARK, sort);
        //    return Json(res);
        //}
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createUserLoginArticle")]
        public IActionResult createUserLoginArticles([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createUserLoginArticle(d);
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
        [HttpPost("updateUserLoginData")]
        public IActionResult updateUserLoginDatas([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserLoginData(d);
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
        [HttpPost("updateUserLoginArticle")]
        public IActionResult updateUserLoginArticles([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserLoginArticle(d);
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
        /// 分配
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserForLoginArticle")]
        public IActionResult updateUserForLoginArticles([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserForLoginArticle(d);
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
        /// 清空挂载的账号信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("deleteUserForLoginArticle")]
        public IActionResult deleteUserForLoginArticles([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.deleteUserForLoginArticle(d);
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