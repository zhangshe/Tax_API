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
    [Route("TaxBusinessManagement")]
    public class TaxBusinessManagementController : WebApiBaseController
    {
        TaxBusinessManagementModule TM = new TaxBusinessManagementModule();
        [HttpGet("getTaxInfo")]
        public IActionResult getTaxInfo(string page, string limit, string S_OrgCode, DateTime S_WorkDate,string id,string workNumber)
        {
            Dictionary<string, object> res = TM.getTaxInfo(page, limit, S_OrgCode, S_WorkDate, id,workNumber);
            return Json(res);
        }

        [HttpPost("validateExcel")]
        public IActionResult validateExcel([FromForm] IFormCollection formCollection, string orgCode, string orgName, DateTime dateMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach(var file in fileCollection)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String Content = reader.ReadToEnd();
                    String name = file.FileName;
                    String timestamp= string.Format("{0:yyyyMMddHHmmssffff}", dateMonth) + "_" + orgCode;
                    String filePath = Directory.GetCurrentDirectory() + "\\Files\\taxdetail\\" + timestamp + "_" + name;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using(FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    r = TM.validateExcel(filePath, orgCode, orgName, dateMonth);
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = "导入失败！" + e.Message;
            }
            return Json(r);
        }

        [HttpPost("UploadTaxBusiness")]
        public IActionResult UploadTaxBusiness(string filePath, string orgCode, string orgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    r = TM.ImportTaxBusiness(filePath, orgCode, orgName, dateMonth, userId);
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = "导入失败！" + ex.Message;
            }
            return Json(r);
        }

        [HttpPost("delTaxBusiness")]
        public IActionResult delTaxBusiness([FromBody]JObject value)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            res = TM.delTaxBus(d);
            return Json(res);
        }

        [HttpGet("getCount")]
        public IActionResult getCount(string S_OrgCode, DateTime S_WorkDate, string id)
        {
            Dictionary<string, object> res = TM.getCount(S_OrgCode, S_WorkDate, id);
            return Json(res);
        }
    }
}