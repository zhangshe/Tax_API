using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
    [Route("noticedetail")]
    public class NoticeDetailController : WebApiBaseController
    {
        NoticeDetailModule mm = new NoticeDetailModule();
        [HttpGet("fetchNoticeDetailList")]
        public IActionResult fetchNoticeDetailList(string NOTICE_ID)
        {
            //UserModule user = new UserModule();
            //string Admin = user.getAdminCode();
            //bool isAdmin = UserId.Equals(Admin);
            //Dictionary<string, object> res = mm.GetPagedTable(isAdmin);
            //return Json(res);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["NOTICE_ID"] = NOTICE_ID;
            Dictionary<string, object> res = mm.fetchNoticeDetailList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createNoticeDetailArticle")]
        public IActionResult createNoticeDetailArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createNoticeDetailArticle(d);
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
        [HttpPost("updateNoticeDetailArticle")]
        public IActionResult updateNoticeDetailArticle([FromBody]JObject value)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            try
            {
                string b = mm.updateNoticeDetailArticle(d["NOTICE_DETAIL_ID"].ToString());
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
        /// 上传附件
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadNoticeFile")]
        public IActionResult PostFile([FromForm]IFormCollection formCollection, string noticeId, string creater)
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
                    string suffix = name.Substring(name.LastIndexOf("."), (name.Length - name.LastIndexOf("."))); //扩展名
                    double filesize = Math.Round(Convert.ToDouble(file.Length / 1024.00 / 1024.00), 2);
                    string filepath = @"\\UploadFiles\\notice\\" + Guid.NewGuid().ToString() + suffix;
                    string filename = System.IO.Directory.GetCurrentDirectory() + filepath;
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
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d["NOTICE_ID"] = noticeId;
                    d["FILE_URL"] = filepath;
                    d["FILE_NAME"] = name;
                    d["FILE_SIZE"] = filesize;
                    d["CREATER"] = creater;
                    r["message"] = mm.createNoticeDetailArticle(d);
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
    }
}