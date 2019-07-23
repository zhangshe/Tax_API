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
    [Route("TaxPlayerInfo")]
    public class TaxPlayerInfoController : WebApiBaseController
    {
        TaxPlayerInfoModule TPI = new TaxPlayerInfoModule();
        [HttpGet("fetchTaxPlayerInfo")]
        public IActionResult fetchTaxPlayerInfo(string limit, string page, string S_OrgCode, string WorkerNumber, string WorkerName, string IdNumber, string Education, string Occupation, string WorkPost, string IsDisability)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["S_OrgCode"] = S_OrgCode;
            d["WorkerNumber"] = WorkerNumber;
            d["WorkerName"] = WorkerName;
            d["IdNumber"] = IdNumber;
            d["Education"] = Education;
            d["Occupation"] = Occupation;
            d["WorkPost"] = WorkPost;
            d["IsDisability"] = IsDisability;
            Dictionary<string, object> res = TPI.getPlayerInfo(d);
            return Json(res);
        }
        [HttpPost("createTaxPlayerInfo")]
        public IActionResult createTaxPlayerInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = TPI.createTaxPlayerInfo(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = b;
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return Json(r);
        }
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("delTaxPlayerInfo")]
        public IActionResult delTaxPlayerInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = TPI.deleteTaxPlayerInfo(d["S_Id"].ToString());
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = b;
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return Json(r);
        }
        /// <summary>
        /// 修改纳税人信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("editTaxPlayerInfo")]
        public IActionResult editTaxPlayerInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = TPI.editTaxPlayerInfo(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = b;
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return Json(r);
        }
        /// <summary>
        /// 获取全部下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllOptions")]
        public IActionResult getAllOptions()
        {
            Dictionary<string, object> res = TPI.getAllOptions();
            return Json(res);
        }
        /// 获取人员变动信息
        /// </summary>
        /// <param name="S_OrgCode"></param>
        /// <param name="ImportMonth"></param>
        /// <returns></returns>
        [HttpGet("fetchTaxPlayerChange")]
        public IActionResult fetchTaxPlayerChange(string S_OrgCode,string ImportMonth,string page,string limit)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["ImportMonth"] = ImportMonth;
            d["limit"] = limit;
            d["page"] = page;
            Dictionary<string, object> res = TPI.getTaxChange(d);
            return Json(res);
        }

        [HttpGet("getTaxContent")]
        public IActionResult getTaxContent(string S_OrgCode,string ImportMonth,string limit,string page)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_OrgCode"] = S_OrgCode;
            d["ImportMonth"] = ImportMonth;
            d["limit"] = limit;
            d["page"] = page;
            return Json(TPI.getTaxContent(d));
        }

        ///以上接口为纳税人管理功能
        ///以下是纳税人查询
        ///
        [HttpGet("getTaxpayer")]
        public IActionResult getTaxpayer(string OrgCode, string systime, string name, string IDNumber, string WorkerNumber, string limit, string page)
        {
            Dictionary<string, object> res = TPI.getTaxpayer(OrgCode, systime, name, IDNumber, WorkerNumber, limit, page);
            return Json(res);
        }

        [HttpGet("getPayerSalary")]
        public IActionResult getPayerSalary(string systime,string id)
        {
            return Json(TPI.getPayerSalary(systime, id));
        }
        /// <summary>
        /// 上传导入的工资excel并验证合法性
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("ValidateTaxPayerInfo")]
        public IActionResult ValidateTaxPayerInfo([FromForm] IFormCollection formCollection)
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
                    String timestamp = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
                    String filePath = System.IO.Directory.GetCurrentDirectory() + "\\Files\\taxplayer\\" + timestamp + "_" + name;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    r = TPI.ValidateTaxPayerInfo(filePath);
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = "导入失败！" + ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 导入纳税人信息
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadTaxPayerInfo")]
        //public IActionResult UploadTaxSalary([FromForm] IFormCollection formCollection, string orgCode, DateTime dateMonth,string userId)
        public IActionResult uploadTaxPayerInfo(string filePath, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    r = TPI.ImportTaxPayerInfo(filePath, userId);
                }

            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = "导入失败！" + ex.Message;
            }
            return Json(r);
        }

        [HttpPost("exportPayerInfo")]
        public IActionResult exportPayerInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = TPI.exportPayerInfo(d);
            return Json(r);
        }

        [HttpPost("exportChangePayerInfo")]
        public IActionResult exportChangePayerInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = TPI.exportChangePayerInfo(d);
            return Json(r);
        }

    }
}
