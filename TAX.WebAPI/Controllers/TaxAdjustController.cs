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
    [Route("TaxAdjust")]
    public class TaxAdjustController : WebApiBaseController
    {
        TaxAdjustModule tam = new TaxAdjustModule();
        /// <summary>
        /// 调整查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet("getTaxAdjustList")]
        public IActionResult getTaxAdjustList(string limit, string page,  string S_WorkerName,string S_WorkDate, string S_OrgCode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            Dictionary<string, object> res = tam.getTaxAdjustList(d);
            return Json(res);
        }
        /// <summary>
        /// 基础工资调整数据导出
        /// </summary>
        /// <param name="S_WorkerName"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_OrgCode"></param>
        /// <returns></returns>
        [HttpGet("exportBaseTaxAdjust")]
        public IActionResult exportBaseTaxAdjust(string S_WorkerName, string S_WorkDate, string S_OrgCode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            Dictionary<string, object> res = tam.getExportTaxAdjust(d);
            return Json(res);
        }
        /// <summary>
        /// 上传调整的工资excel并验证合法性
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("validateTaxAdjust")]
        public IActionResult validateTaxAdjust([FromForm] IFormCollection formCollection, string orgCode, DateTime dateMonth)
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
                    String timestamp = string.Format("{0:yyyyMMdd}", dateMonth) + "_" + orgCode;
                    String filePath = System.IO.Directory.GetCurrentDirectory() + "\\Files\\adjust\\" + timestamp + "_" + name;
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
                    r = tam.ValidateTaxAdjust(filePath, orgCode, dateMonth);
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
        /// 导入调整
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadTaxAdjust")]
        public IActionResult uploadTaxAdjust(string filePath, string orgCode,string orgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    r = tam.ImportTaxAdjust(filePath, orgCode, orgName, dateMonth, userId);
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
        /// 删除调整信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("delTaxAdjust")]
        public IActionResult delTaxAdjust([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = tam.deleteTaxAdjust(Convert.ToDateTime(d["dateMonth"].ToString()), d["userId"].ToString(), d["orgCode"].ToString());
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "失败";
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
    }
}