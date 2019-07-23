using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;
using UIDP.LOG;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    public class UserController : WebApiBaseController
    {
        UserModule mm = new UserModule();
        OrgModule om = new OrgModule();
        //public static string loginType { get; set; }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserList")]
        public IActionResult fetchUserList(string limit, string page, string USER_NAME, int? FLAG, string sort)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            Dictionary<string, object> res = mm.fetchUserList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createUserArticle")]
        public IActionResult createUserArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                d["USER_ID"] = Guid.NewGuid();
                string b = mm.createUserArticle(d);
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
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserArticle")]
        public IActionResult updateUserArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserArticle(d);
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
        /// 修改用户信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserData")]
        public IActionResult updateUserData([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.updateUserData(d);
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
        /// 激活或者锁用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserFlag")]
        public IActionResult updateUserFlag([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserFlag(d);
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
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updatePasswordData")]
        public IActionResult updatePasswordData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updatePasswordData(d);
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
        /// 获取用户信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("Info")]
        public IActionResult Info([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string tokenUserId = UIDP.UTILITY.AccessTokenTool.GetUserId(d["token"].ToString());
                string userID= tokenUserId;
                //string _loginType = "";
                //if (d["loginType"].ToString()==null|| d["loginType"].ToString() == "")
                //{
                //    _loginType = loginType;
                //}
                //else
                //{
                //    loginType = d["loginType"].ToString();
                //}
                if (d.Keys.Contains("userId")&&d["userId"] != null && d["userId"].ToString()!= "")
                {
                    userID = d["userId"].ToString();
                }
                if (userID == mm.getAdminCode()) {
                    //if (tokenUserId == mm.getAdminCode()&&(d["userId"]==null|| d["userId"].ToString()=="")){
                    SysLog log = new SysLog();
                    log.Info(DateTime.Now, tokenUserId, "系统超级管理员", ClientIp, 0, "info", "", 1);
                    return Json(new
                    {
                        code = 2000,
                        message = "",
                        roles = JsonConvert.DeserializeObject("['admin']"),
                        name = "系统超级管理员",
                        userCode = tokenUserId,
                        token = d["token"].ToString(),
                        introduction = "",
                        avatar = "",
                        sysCode = "1",
                        sysName = mm.getSysName(),
                        userId = tokenUserId,
                        userSex = 0,
                        departCode = "",
                        departName = "",
                        userType="0",
                        //loginType= loginType
                    });
                }
                //string token = UIDP.UTILITY.AccessTokenTool.GetAccessToken(d["userId"].ToString());
                string token = UIDP.UTILITY.AccessTokenTool.GetAccessToken(userID);
                //DataTable dt = mm.GetUserAndOrgByUserId(d["userId"].ToString());
                DataTable dt = mm.GetUserAndOrgByUserId(userID);
                DataTable du = mm.getSysTime();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string _name = dt.Rows[0]["USER_NAME"] == null ? "" : dt.Rows[0]["USER_NAME"].ToString();
                    string _userCode = dt.Rows[0]["USER_DOMAIN"] == null ? "" : dt.Rows[0]["USER_DOMAIN"].ToString();
                    string _userId = dt.Rows[0]["USER_ID"] == null ? "" : dt.Rows[0]["USER_ID"].ToString();
                    int _userSex = Convert.ToInt32(dt.Rows[0]["USER_SEX"].ToString());
                    string _deptCode = dt.Rows[0]["ORG_CODE"] == null ? "" : dt.Rows[0]["ORG_CODE"].ToString();
                    string _deptName = dt.Rows[0]["ORG_SHORT_NAME"] == null ? "" : dt.Rows[0]["ORG_SHORT_NAME"].ToString();
                    string _userType = dt.Rows[0]["USER_TYPE"] == null ? "" : dt.Rows[0]["USER_TYPE"].ToString();
                    string _orgCode = dt.Rows[0]["ORG_CODE"] == null ? "" : dt.Rows[0]["ORG_CODE"].ToString();
                    string _sysTime = du.Rows[0]["SysOperateDate"] == null ? "" : du.Rows[0]["SysOperateDate"].ToString();
                    string _orgName = dt.Rows[0]["ORG_NAME"] == null ? "" : dt.Rows[0]["ORG_NAME"].ToString();
                    SysLog log = new SysLog();
                    //log.Info(DateTime.Now, d["userId"].ToString(), _name, ClientIp, 0, "info", "",1);
                    log.Info(DateTime.Now, userID, _name, ClientIp, 0, "info", "", 1);
                    return Json(new
                    {
                        code = 2000,
                        message = "",
                        roles = new Dictionary<string, object>(),
                        token = token,
                        introduction = "",
                        avatar = "",
                        name = _name,
                        userCode = _userCode,
                        sysCode = "1",
                        sysName = mm.getSysName(),
                        userId = _userId,
                        userSex = _userSex,
                        departCode = _deptCode,
                        departName = _deptName,
                        userType = _userType,
                        orgCode=_orgCode,
                        orgName= _orgName,
                        sysTime =_sysTime,
                       // loginType = loginType
                    });
                }
                return Json(new
                {
                    code = 2000,
                    message = "",
                    roles = "",
                    name = "",
                    userCode = "",
                    token = token,
                    introduction = "",
                    avatar = "",
                    sysCode = "1",
                    sysName = mm.getSysName(),
                    userId = "",
                    userSex = 0,
                    departCode = "",
                    departName = "",
                    userType = "0",
                    orgCode = "",
                    orgName = "",
                    sysTime = "",
                   // loginType = loginType
                });
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 查询用户信息(包括角色信息)
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserOrgList")]
        public IActionResult fetchUserOrgList(string limit, string page, string USER_NAME, int? FLAG, string sort, string orgId, string USER_DOMAIN)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(orgId))
            {
                DataTable dt = om.GetOrgById(orgId);
                string orgpcode = dt.Rows[0]["ORG_CODE"].ToString();
                if (!string.IsNullOrEmpty(orgpcode))
                {
                    d["orgpCode"] = orgpcode;
                }
            }
            else
            {
                d["orgpCode"] = "";
            }
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["USER_DOMAIN"] = USER_DOMAIN;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            d["orgId"] = orgId;
            Dictionary<string, object> res = mm.fetchUserOrgList(d);
            return Json(res);
        }
        /// <summary>
        /// 查询用户信息(包括角色信息)
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserRoleList")]
        public IActionResult fetchUserRoleList(string limit, string page, string USER_NAME, int? FLAG, string sort, string roleId,string orgcode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            d["roleId"] = roleId;
            d["orgcode"] = orgcode;
            Dictionary<string, object> res = mm.fetchUserRoleList(d);
            return Json(res);
        }
        /// <summary>
        /// 弹窗查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchUserForLoginList")]
        public IActionResult fetchUserForLoginList(string limit, string page, string USER_ID)
        {
            UserLoginModule mm = new UserLoginModule();
            Dictionary<string, object> res = mm.fetchUserForLoginList(limit, page, USER_ID);
            return Json(res);
        }

        [HttpGet("fetchUserForAllList")]
        public IActionResult fetchUserForAllList(string limit, string page, string USER_ID)
        {
            UserLoginModule mm = new UserLoginModule();
            Dictionary<string, object> res = mm.fetchUserForAllList(limit, page, USER_ID);
            return Json(res);
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadUserArticle")]
        public IActionResult PostFile([FromForm] IFormCollection formCollection)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String content = reader.ReadToEnd();
                    String name = file.FileName;
                    String filename = System.IO.Directory.GetCurrentDirectory() + "\\Files\\" + Guid.NewGuid() + name;
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    //r["message"] = mm.UploadUserFile(filename);
                    r["message"] = mm.UploadUserFileNew(filename);
                    if (r["message"].ToString() != "")
                    {
                        r["code"] = -1;
                    }
                    else
                    {
                        r["code"] = 2000;
                    }
                    Json(r);
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }

            return Json(r);
        }
        #region MyRegion
        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //[HttpPost("Info")]
        //public IActionResult Info([FromBody]JObject value)
        //{
        //    Dictionary<string, object> r = new Dictionary<string, object>();
        //    try
        //    {
        //        if (UserId == mm.getAdminCode())
        //        {
        //            string[] arr = new string[1];
        //            arr[0] = "";
        //            return Json(new { code = 2000, message = "", roles = JsonConvert.DeserializeObject("['admin']"), name = "系统超级管理员", userCode = UserId, token = accessToken, introduction = "", avatar = "", sysCode = "1", sysName = mm.getSysName(), userId = UserId, userSex = 0 });
        //        }
        //        Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //        string _token = d["token"] == null ? "" : d["token"].ToString();
        //        string departcode = d["departCode"] == null ? "" : d["departCode"].ToString();
        //        DataTable dt = mm.getUserAndGroupgByToken(_token);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            string[] role = new string[dt.Rows.Count];
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                role[i] = dt.Rows[i]["GROUP_NAME"] == null ? "" : dt.Rows[i]["GROUP_NAME"].ToString();
        //            }
        //            string _name = dt.Rows[0]["USER_NAME"] == null ? "" : dt.Rows[0]["USER_NAME"].ToString();
        //            string _userCode = dt.Rows[0]["USER_CODE"] == null ? "" : dt.Rows[0]["USER_CODE"].ToString();
        //            string _userId = dt.Rows[0]["USER_ID"] == null ? "" : dt.Rows[0]["USER_ID"].ToString();
        //            int _userSex = Convert.ToInt32(dt.Rows[0]["USER_SEX"].ToString());
        //            return Json(new { code = 2000, message = "", roles = role, name = _name, userCode = _userCode, token = _token, introduction = "", avatar = "", sysCode = "1", sysName = mm.getSysName(), userId = _userId, userSex = _userSex });
        //        }
        //        return Json(new { code = 2000, message = "", roles = "", name = "", userCode = "", token = _token, introduction = "", avatar = "", sysCode = "", sysName = mm.getSysName(), userId = "", userSex = 0 });
        //    }
        //    catch (Exception ex)
        //    {
        //        r["code"] = -1;
        //        r["message"] = ex.Message;
        //    }
        //    return Json(r);
        //}
        #endregion

    }
}