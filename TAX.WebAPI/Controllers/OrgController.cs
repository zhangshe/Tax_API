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
using UIDP.UTILITY;
using System.Net;
using System.Text;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("Org")]
    public class OrgController : WebApiBaseController
    {
        OrgModule mm = new OrgModule();
        SyncResultModule srm = new SyncResultModule();
        SyncConfModule sncm = new SyncConfModule();
        ConfModule cm = new ConfModule();

        [HttpGet("fetchOrgListByCode")]
        public dynamic fetchOrgListByCode(string sysCode)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["sysCode"] = sysCode;

            List<ClsOrgInfo> c;
            try
            {
                c = mm.fetchOrgListByCode(d);

            }
            catch (Exception)
            {
                c = new List<ClsOrgInfo>();
            }
            ////JObject obj=JObject.FromObject(c);
            ////if
            //foreach (var item in c)
            //{
            //    item.children.Count();

            //}
            var str = JsonConvert.SerializeObject(c);
            //var ddd= str.Replace(",\"children\":null", "");
            return str;
        }




        /// <summary>
        /// 查询组织结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchOrgList")]
        public IActionResult fetchOrgList()
        {
            UserModule user = new UserModule();
            string Admin = user.getAdminCode();
            bool isAdmin = UserId.Equals(Admin);
            Dictionary<string, object> res = mm.fetchOrgList(isAdmin);
            return Json(res);
        }
        /// <summary>
        /// 新增组织结构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createOrgArticle")]
        public IActionResult createOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                //DataTable dt = null;
                //string orgpcode = null;
                //if (d.Keys.Contains("parentId") && d["parentId"] != null && !string.IsNullOrEmpty(d["parentId"].ToString()))
                //{
                //    dt = mm.GetOrgById(d["parentId"].ToString());
                //}
                //else
                //{
                //    d["parentId"] = null;
                //}
                //if (dt != null)
                //{
                //    orgpcode = dt.Rows[0]["ORG_CODE"].ToString();
                //}
                //if (!string.IsNullOrEmpty(orgpcode))
                //{
                //    d["orgpCode"] = orgpcode;
                //}
                //else
                //{
                //    d["orgpCode"] = null;
                //}
                //DataTable dt = mm.GetOrgById(d["parentId"].ToString());
                //string orgpcode = dt.Rows[0]["ORG_CODE"].ToString();
                //if (!string.IsNullOrEmpty(orgpcode))
                //{
                //    d["orgpCode"] = orgpcode;
                //}
   
                if (!string.IsNullOrEmpty(d["orgCodeUpper"].ToString()))
                {
                    d["orgpCode"] = d["orgCodeUpper"].ToString();
                }
                else
                {
                    d["orgpCode"] = null;
                }
                DataTable orgdt = mm.GetOrgByCode(d["orgCodeUpper"].ToString());
                int SwiftNumber = 1;
                if (orgdt!=null&&orgdt.Rows.Count>0)
                {
                    SwiftNumber = orgdt.Rows.Count + 1;
                }
                string orgCode = d["orgCodeUpper"].ToString() + SwiftNumber.ToString().PadLeft(3, '0');
                d["orgCode"] = orgCode;
                string b = mm.createOrgArticle(d);
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
        /// 修改组织结构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateOrgData")]
        public IActionResult updateOrgData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataTable dt = null;
                string orgpcode = null;
                if (d.Keys.Contains("parentId") && d["parentId"] != null&&!string.IsNullOrEmpty(d["parentId"].ToString()))
                {
                    dt = mm.GetOrgById(d["parentId"].ToString());
                }
                else
                {
                    d["parentId"] = null;
                }
                if (dt != null)
                {
                    orgpcode = dt.Rows[0]["ORG_CODE"].ToString();
                }
                if (!string.IsNullOrEmpty(orgpcode))
                {
                    d["orgpCode"] = orgpcode;
                }
                else
                {
                    d["orgpCode"] = null;
                }
                //DataTable dt = mm.GetOrgById(d["parentId"].ToString());
                //string orgpcode = dt.Rows[0]["ORG_CODE"].ToString();
                if (!string.IsNullOrEmpty(orgpcode))
                {
                    d["orgpCode"] = orgpcode;
                }
                string b = mm.updateOrgData(d);
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
        /// 删除组织机构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateOrgArticle")]
        public IActionResult updateOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateOrgArticle(d);
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
        /// D.	分配组织结构给用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserOrgArticle")]
        public IActionResult updateUserOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserOrgArticle(d);
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
        /// 清空用户角色
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("deleteUserOrgArticle")]
        public IActionResult deleteUserOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.deleteUserOrgArticle(d);
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
        [Consumes("multipart/form-data")]//此处为新增

        public IActionResult UploadExcelFiles([FromForm] IFormCollection formCollection)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                IFormFileCollection dd = Request.Form.Files;
                IFormFile file = fileCollection[0];
                string path = file.FileName;
                //HttpFileCollection
                Stream aa = file.OpenReadStream();
                Stream BB = dd[0].OpenReadStream();
                string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\组织结构模板.xlsx";//原始文件
                string mes = "";
                DataTable dt = new DataTable();
                ExcelTools tool = new ExcelTools();
                tool.GetDataTable(aa, path, modePath, ref mes, ref dt);
                return Json(r);
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 导入excel(无用)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FileSave()
        {
            try
            {
                var date = Request;
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);
                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {

                        string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，含“.”
                        long fileSize = formFile.Length; //获得文件大小，以字节为单位
                        string newFileName = System.Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                        var filePath = contentRootPath + "\\files\\" + newFileName;
                        string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\组织结构模板.xlsx";//原始文件
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            string aa = mm.UploadOrgFile(filePath);
                        }
                    }
                }
                return Ok(new { count = files.Count, size });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadOrgArticle")]
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
                    r["message"] = mm.UploadOrgFileNew(filename);
                    if (r["message"].ToString() != "")
                    {
                        r["code"] = -1;
                    }
                    else
                    {
                        //string msg = mm.updateOrgPID();
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

        private readonly IHostingEnvironment _hostingEnvironment;

        public OrgController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("clearOrg")]
        public IActionResult clearOrg()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.clearOrg();
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

        //public void SyncOrg()
        //{

        //    mm.createOrgArticle(r);
        //}

        ///// <summary>
        ///// 新增组织结构
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost("createOrg")]
        //public IActionResult createOrg(Dictionary<string, object> d)
        //{
        //    Dictionary<string, object> r = new Dictionary<string, object>();
        //    try
        //    {
        //        string b = mm.createOrgArticle(d);
        //        if (b == "")
        //        {
        //            r["message"] = "成功";
        //            r["code"] = 2000;
        //        }
        //        else
        //        {
        //            r["code"] = -1;
        //            r["message"] = b;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        r["code"] = -1;
        //        r["message"] = e.Message;
        //    }
        //    return Json(r);
        //}
        /// <summary>
        /// 推送云同步组织架构
        /// </summary>
        /// <returns></returns>
        [HttpPost("pushOrgList")]
        public IActionResult pushOrgList([FromBody]JObject value)
        //public IActionResult pushOrgList()
        {
           // sync_list
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                Dictionary<string, object> dd = value.ToObject<Dictionary<string, object>>();
                //var targetlist = sncm.getSyncConfList();
                //foreach (var item in targetlist)
                foreach (var item in (JArray)dd["sync_list"])
                {
                    if (item["SYNC_FLAG"] != null && item["SYNC_FLAG"].ToString() == "0")
                    {
                        //string loginUrl = "http://" + item["SERVER_IP"].ToString() + ":" + item["SERVER_PORT"].ToString() + "/LogIn/apiLogin";
                        string loginUrl = "http://" + item["SERVER_IP"].ToString() + ":" + item["SERVER_PORT"].ToString() + item["AUTHENTICATION_URL"].ToString();
                        //WebRequest req = WebRequest.Create("http://192.168.1.113:12345/LogIn/apiLogin");
                        //string loginUrl = "http://192.168.1.107:12345/LogIn/apiLogin";
                        WebRequest req = WebRequest.Create(loginUrl);
                        Dictionary<string, string> postData = new Dictionary<string, string>();
                        //postData["userCode"] = "ceshi02";
                        //postData["password"] = "123456";
                        postData["userCode"] = item["USER_CODE"].ToString();
                        postData["password"] = item["USER_PASS"].ToString();
                        string jsonString = JsonConvert.SerializeObject(postData);
                        byte[] objectContent = Encoding.UTF8.GetBytes(jsonString);
                        req.ContentLength = objectContent.Length;
                        req.ContentType = "application/json";
                        req.Method = "POST";
                        using (var stream = req.GetRequestStream())
                        {
                            stream.Write(objectContent, 0, objectContent.Length);
                            stream.Close();
                        }


                        var resp = req.GetResponse();
                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            string s = sr.ReadToEnd();
                            if (s != "")
                            {
                                //"/Org/syncOrg"
                                string syncUrl = "http://" + item["SERVER_IP"].ToString() + ":" + item["SERVER_PORT"].ToString() + item["SERVER_URL"].ToString();
                                //string syncUrl = "http://192.168.1.107:12345/Org/syncOrg";
                                //WebRequest pushreq = WebRequest.Create("http://192.168.1.113:12345/Org/syncOrg");
                                WebRequest pushreq = WebRequest.Create(syncUrl);
                                DataTable dt = mm.fetchSyncOrgTable();
                                string pushjsonString = JsonConvert.SerializeObject(dt);
                                byte[] pushobjectContent = Encoding.UTF8.GetBytes(pushjsonString);
                                pushreq.ContentLength = pushobjectContent.Length;
                                pushreq.ContentType = "application/json";
                                pushreq.Headers.Add("X-Token", s.ToString());
                                pushreq.Method = "POST";
                                using (var stream = pushreq.GetRequestStream())
                                {
                                    stream.Write(pushobjectContent, 0, pushobjectContent.Length);
                                    stream.Close();
                                }
                                var pushresp = pushreq.GetResponse();
                                using (StreamReader pushsr = new StreamReader(pushresp.GetResponseStream()))
                                {
                                    string pushs = pushsr.ReadToEnd();
                                    if (pushs == "2000")
                                    {
                                        Dictionary<string, object> d = new Dictionary<string, object>();
                                        d["SEND_URL"] = Extension.GetClientUserIp(Request.HttpContext);
                                        d["RECEIVE_URL"] = item["SERVER_IP"].ToString();
                                        d["SYNC_CONTENT"] = "云组织同步推送成功";
                                        d["SYNC_RESULT"] = 0;
                                        d["ERROR_INFO"] = "";
                                        d["FAIL_CONTENT"] = "";
                                        d["REMARK"] = "";
                                        srm.createSyncResult(d);
                                        r["message"] = "云组织同步推送成功";
                                        r["code"] = 2000;
                                        return Json(r);
                                    }
                                    else if (pushs == "5000")
                                    {
                                        Dictionary<string, object> d = new Dictionary<string, object>();
                                        d["SEND_URL"] = Extension.GetClientUserIp(Request.HttpContext);
                                        d["RECEIVE_URL"] = item["SERVER_IP"].ToString();
                                        d["SYNC_CONTENT"] = "云组织同步推送失败";
                                        d["SYNC_RESULT"] = 0;
                                        d["ERROR_INFO"] = "云组织同步推送失败！" + pushs;
                                        d["FAIL_CONTENT"] = "系统未启用云组织";
                                        d["REMARK"] = "";
                                        srm.createSyncResult(d);
                                        r["code"] = -1;
                                        r["message"] = "系统未启用云组织";
                                        return Json(r);
                                    }
                                    else
                                    {
                                        Dictionary<string, object> d = new Dictionary<string, object>();
                                        d["SEND_URL"] = Extension.GetClientUserIp(Request.HttpContext);
                                        d["RECEIVE_URL"] = item["SERVER_IP"].ToString();
                                        d["SYNC_CONTENT"] = "云组织同步推送失败";
                                        d["SYNC_RESULT"] = 0;
                                        d["ERROR_INFO"] = "云组织同步推送失败！" + pushs;
                                        d["FAIL_CONTENT"] = "同步服务发生异常";
                                        d["REMARK"] = "";
                                        srm.createSyncResult(d);
                                    }
                                }
                            }
                            else {
                                Dictionary<string, object> d = new Dictionary<string, object>();
                                d["SEND_URL"] = Extension.GetClientUserIp(Request.HttpContext);
                                d["RECEIVE_URL"] = item["SERVER_IP"].ToString();
                                d["SYNC_CONTENT"] = "云组织同步推送失败";
                                d["SYNC_RESULT"] = 0;
                                d["ERROR_INFO"] = "云组织同步推送失败！";
                                d["FAIL_CONTENT"] = "云组织同步用户账号或密码不正确";
                                d["REMARK"] = "";
                                srm.createSyncResult(d);
                                r["code"] = -1;
                                r["message"] = "云组织同步用户账号或密码不正确";
                                return Json(r);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return Json(r);
        }


        /// <summary>
        /// 接收云同步组织架构
        /// </summary>
        /// <returns></returns>
        [HttpPost("syncOrg")]
        public IActionResult syncOrg([FromBody]JObject[] value)
        {
            //List<Dictionary<string, object>> f = new List<Dictionary<string, object>>();
            //foreach (JObject item in value)
            //{
            //    var d = UTILITY.JsonConversionExtensions.ToDictionary(item);
            //    f.Add((Dictionary<string, object>)d);
            //}
            if (cm.getConfig())
            {
                try
                {
                    var t = JsonConversionExtensions.ToDictionary(value);
                    var res = mm.syncOrg((List<Dictionary<string, object>>)t);
                    if (res == "2000")
                    {
                        Dictionary<string, object> d = new Dictionary<string, object>();
                        d["SEND_URL"] = Extension.GetClientUserIp(Request.HttpContext);
                        d["RECEIVE_URL"] = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
                        d["SYNC_CONTENT"] = "云组织同步接收成功";
                        d["SYNC_RESULT"] = 0;
                        d["ERROR_INFO"] = "";
                        d["FAIL_CONTENT"] = "";
                        d["REMARK"] = "";
                        srm.createSyncResult(d);
                    }
                    else
                    {
                        Dictionary<string, object> d = new Dictionary<string, object>();
                        d["SEND_URL"] = Extension.GetClientUserIp(Request.HttpContext);
                        d["RECEIVE_URL"] = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
                        d["SYNC_CONTENT"] = "云组织同步接收失败！" + res;
                        d["SYNC_RESULT"] = 0;
                        d["ERROR_INFO"] = "";
                        d["FAIL_CONTENT"] = "";
                        d["REMARK"] = "";
                        srm.createSyncResult(d);
                    }

                    return Content(res);
                }
                catch (Exception ex)
                {

                    return Content(ex.ToString());
                }

            }
            return Content("5000");
        }

    }
}