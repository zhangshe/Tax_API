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
    [Route("ServiceRemunerationImport")]
    public class ServiceRemunerationImportController : WebApiBaseController
    {
        ServiceRemunerationImportModule SRIM = new ServiceRemunerationImportModule();
        [HttpGet("getlist")]
        public IActionResult getlist(string page, string limit, string S_OrgCode, DateTime S_WorkDate, string id, string workNumber,int flag)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            r = SRIM.getlist(page, limit, S_OrgCode, S_WorkDate, id, workNumber,flag);
            return Json(r);
        }

        [HttpPost("validate")]
        public IActionResult validate([FromForm] IFormCollection formcollection, string orgCode, string orgName, DateTime dateMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                FormFileCollection filecollection = (FormFileCollection)formcollection.Files;
                foreach(var file in filecollection)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    string content = reader.ReadToEnd();
                    string name = file.FileName;
                    string timestamp = string.Format("{0:yyyyMMddHHmmssffff}", dateMonth) + "_" + orgCode;
                    string filepath = Directory.GetCurrentDirectory() + "\\Files\\serviceimport\\" + timestamp + "_" + name;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    using(FileStream fs = System.IO.File.Create(filepath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    r = SRIM.validate(filepath, orgCode, orgName, dateMonth);
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Ok(r);
        }
        [HttpPost("importdata")]
        public IActionResult importdata(string filePath, string orgCode, string orgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                
                if (System.IO.File.Exists(filePath))
                {
                    r = SRIM.importdata(filePath, orgCode, orgName, dateMonth, userId);
                }
                else
                {
                    throw new Exception("系统错误！未找到指定的文件！");
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return Ok(r);
        }

        [HttpGet("getcount")]
        public IActionResult getcount(string S_OrgCode, DateTime S_WorkDate, string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            r = SRIM.getcount(S_OrgCode, S_WorkDate, id);
            return Ok(r);
        }

        [HttpGet("delData")]
        public IActionResult delData(string orgCode, DateTime dateMonth, string id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            res = SRIM.delData(orgCode,dateMonth,id);
            return Json(res);
        }

        [HttpGet("getReportState")]
        public IActionResult getReportState(string orgCode, DateTime dateMonth)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            res = SRIM.getReportState(orgCode, dateMonth);
            return Json(res);
        }

    }
}