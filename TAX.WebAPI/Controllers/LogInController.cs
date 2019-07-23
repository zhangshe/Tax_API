using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule;
using UIDP.UTILITY;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.DirectoryServices;
using Microsoft.Extensions.Configuration;
using UIDP.LOG;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("LogIn")]
    public class LogInController : Controller
    {
        SysLog log = new SysLog();
        public static IConfiguration Configuration { get; set; }
        [HttpPost("login")]
        public IActionResult loginByUsernames([FromBody]JObject value)
        {
            string userId = "";
            string userName = "";
            try
            {
                Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
                string username = d["username"] == null ? "" : d["username"].ToString();
                string password = d["password"] == null ? "" : d["password"].ToString();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Json(new { code = -1, message = "用户名或密码不能为空！" });
                }
                UserModule mm = new UserModule();
                userId = mm.getAdminCode();
                string pass = mm.getAdminPass();
                if ((username == userId))
                {
                    if (password != pass)
                    {
                        return Json(new { code = -1, message = "管理员密码不正确！" });
                    }
                    userName = "系统超级管理员";
                    string accessToken = AccessTokenTool.GetAccessToken(userId);
                    UIDP.UTILITY.AccessTokenTool.DeleteToken(userId);
                    UIDP.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                    log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "", 1);
                    return Json(new
                    {
                        code = 2000,
                        message = "",
                        token = accessToken,
                        orgList = new DataTable(),
                        userList = new DataTable(),
                        roleLevel = "admin"
                    });
                }
                else
                {
                    UserLoginModule um = new UserLoginModule();
                    if (d["userDomain"].ToString() == "PTR_IDENT")
                    {
                        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
                        Configuration = builder.Build();
                        string LDAPPATH = Configuration["LdapPath"];
                        DirectoryEntry entry = new DirectoryEntry(LDAPPATH, username, password);
                        DirectorySearcher mySearcher = new DirectorySearcher(entry);
                        mySearcher.Filter = "(SAMAccountName=" + username + ")";
                        SearchResult result = mySearcher.FindOne();
                        if (result == null)
                        {
                            throw new Exception("用户认证错误");
                        }
                        else
                        {
                            DataTable userdt = um.getUserInfoByName(username);
                            if (userdt == null || userdt.Rows.Count == 0)
                            {
                                return Json(new { code = -1, message = "本地用户不存在,请同步用户信息！" });
                            }
                            Dictionary<string, object> dinfo = new Dictionary<string, object>();
                            if (password != userdt.Rows[0]["USER_PASS"].ToString())
                            {
                                //dinfo["password"] = userdt.Rows[0]["USER_PASS"].ToString();
                                dinfo["newpassword"] =password;
                                dinfo["userid"] = userdt.Rows[0]["USER_ID"].ToString();
                                mm.updatePTRpass(dinfo);
                            }
                        }
                    }
                   
                    DataTable dt = um.getUserInfoByName(username);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return Json(new { code = -1, message = "此用户不存在！" });
                    }
                    password = UIDP.Security.SecurityHelper.StringToMD5Hash(password);
                    if (password != dt.Rows[0]["USER_PASS"].ToString())
                    {
                        return Json(new { code = -1, message = "密码错误！" });
                    }
                    userId = dt.Rows[0]["USER_ID"].ToString();
                    //userName = dt.Rows[0]["LOGIN_REMARK"].ToString();
                    string accessToken = AccessTokenTool.GetAccessToken(userId);
                    UIDP.UTILITY.AccessTokenTool.DeleteToken(userId);
                    UIDP.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                    DataTable dtUser = um.getLoginByID(userId);
                    int level = 1;
                    if (Extension.GetClientUserIp(Request.HttpContext).ToString() != dt.Rows[0]["USER_IP"].ToString())
                    {
                        level = 2;
                    }
                    log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "", level);
                    return Json(new
                    {
                        code = 2000,
                        message = "",
                        token = accessToken,
                        orgList = new DataTable(),
                        userList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtUser)),
                        roleLevel = ""
                    });
                }
            }
            catch (Exception ex)
            {
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message, 1);
                return Json(new { code = -1, message = "登录时程序发生错误" + ex.Message });

            }

        }

        #region 备用


        //[HttpPost("login")]
        //public IActionResult LogIn([FromBody]JObject value)
        //{
        //    string userId = "";
        //    string userName = "";
        //    try
        //    {
        //        Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //        string username = d["username"]==null?"" : d["username"].ToString();
        //        string password = d["password"]==null?"": d["password"].ToString();
        //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //        {
        //            return Json(new { code = -1, message = "用户名或密码不能为空！" });
        //        }
        //        UserModule mm = new UserModule();
        //         userId = mm.getAdminCode();
        //        string pass = mm.getAdminPass();
        //        if ((username== userId) &&(password==pass)) {
        //            userName = "系统超级管理员";
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            UIDP.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            UIDP.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));

        //            log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
        //            return Json(new { code = 2000, message = "", token = accessToken,roleLevel = "admin" });
        //        }
        //        else {
        //            UIDP.BIZModule.Models.ts_uidp_userinfo mode = mm.getUserInfoByLogin(username, d["userDomain"].ToString());
        //            if (mode == null)
        //            {
        //                return Json(new { code = -1, message = "此用户不存在！" });
        //            }
        //            if (password != mode.USER_PASS)
        //            {
        //                return Json(new { code = -1, message = "密码错误！" });
        //            }
        //            userId = mode.USER_ID;
        //            userName = mode.USER_NAME;
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            UIDP.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            UIDP.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
        //            DataTable dtUserOrg = mm.GetUserOrg(mode.USER_ID);
        //            log.Info(DateTime.Now, userId, mode.USER_NAME, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
        //            return Json(new { code = 2000, message = "", token = accessToken, orgList = dtUserOrg, roleLevel = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length>120?ex.Message.Substring(0,100):ex.Message);
        //        return Json(new { code = -1, message = "登录时程序发生错误"+ex.Message});

        //    }

        //}
        #endregion

        [HttpPost("apiLogin")]
        public IActionResult apiLogin([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            string userCode = d["userCode"] == null ? "" : d["userCode"].ToString();
            string password = d["password"] == null ? "" : d["password"].ToString();
            string userId = "";
            string userName = "云主机推送服务";
            string accessToken = "";
            try
            {
                //if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(password))
                //{
                //    //return Json(new { code = -1, message = "推送接口用户名或密码不能为空！" });
                //    return Content("");
                //}
                //UserLoginModule um = new UserLoginModule();
                //DataTable dt = um.getUserInfoByName(userCode);
                //if (dt == null || dt.Rows.Count == 0)
                //{
                //    //return Json(new { code = -1, message = "云同步用户不存在！" });
                //    return Content("");
                //}
                //if (password != dt.Rows[0]["USER_PASS"].ToString())
                //{
                //    //return Json(new { code = -1, message = "云同步用户密码错误！" });
                //    return Content("");
                //}
                //userId = dt.Rows[0]["USER_ID"].ToString();
                //userName = dt.Rows[0]["USER_NAME"].ToString();
                //accessToken = AccessTokenTool.GetAccessToken(userId);
                //UIDP.UTILITY.AccessTokenTool.DeleteToken(userId);
                //UIDP.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                //log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "云组织数据同步", "", 1);
                //return Content(accessToken);
                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(password))
                {
                    //return Json(new { code = -1, message = "推送接口用户名或密码不能为空！" });
                    return Content("");
                }
                UserModule mm = new UserModule();
                var CloudCode = mm.getCloudCode();
                var CloudPass = mm.getCloudPass();
                if (userCode != CloudCode || CloudPass != password)
                {
                    //return Json(new { code = -1, message = "云同步用户不存在！" });
                    return Content("");
                }
                userId = userCode;
                accessToken = AccessTokenTool.GetAccessToken(userId);
                AccessTokenTool.DeleteToken(userId);
                AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "云组织数据同步", "", 1);
                return Content(accessToken);

            }
            catch (Exception ex)
            {
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "云组织数据同步", ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message, 1);
                return Content("");

            }

        }
    }
}