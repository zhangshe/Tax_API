using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("Notice")]
    public class NoticeController : WebApiBaseController
    {
        NoticeModule mm = new NoticeModule();

        /// <summary>
        /// 查询配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchNoticeList")]
        public IActionResult fetchNoticeList(string limit, string page, string NOTICE_CODE, string NOTICE_TITLE, string NOTICE_CONTENT, string NOTICE_DATETIME, string NOTICE_ORGID, string NOTICE_ORGNAME, string NOTICE_ISSUER, string BEGIN_NOTICE_DATETIME, string END_NOTICE_DATETIME)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["NOTICE_CODE"] = NOTICE_CODE;
            d["NOTICE_TITLE"] = NOTICE_TITLE;
            d["NOTICE_CONTENT"] = NOTICE_CONTENT;
            d["NOTICE_DATETIME"] = NOTICE_DATETIME;
            d["NOTICE_ORGID"] = NOTICE_ORGID;
            d["NOTICE_ORGNAME"] = NOTICE_ORGNAME;
            d["NOTICE_ISSUER"] = NOTICE_ISSUER;
            d["BEGIN_NOTICE_DATETIME"] = BEGIN_NOTICE_DATETIME;
            d["END_NOTICE_DATETIME"] = END_NOTICE_DATETIME;
            Dictionary<string, object> res = mm.fetchNoticeList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createNoticeArticle")]
        public IActionResult createNoticeArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createNoticeArticle(d);
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
        [HttpPost("updateNoticeData")]
        public IActionResult updateNoticeData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateNoticeData(d);
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
        [HttpPost("updateNoticeArticle")]
        public IActionResult updateNoticeArticle([FromBody]JObject value)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            try
            {
                string b = mm.updateNoticeArticle(d["NOTICE_ID"].ToString());
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

        [HttpGet("fetchNoticeList1")]
        public IActionResult fetchNoticeList1(string limit, string page)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            Dictionary<string, object> res = mm.fetchNoticeList(d);
            return Json(res);
        }
    }
}