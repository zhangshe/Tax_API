using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;
using UIDP.UTILITY;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
    [Route("TaxSalary")]
    public class TaxSalaryController : WebApiBaseController
    {
        TaxSalaryModule tsm = new TaxSalaryModule();
        TaxOrgModule tom = new TaxOrgModule();
        /// <summary>
        /// 工资信息查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet("getTaxSalaryList")]
        public IActionResult GetTaxSalaryList(string limit, string page, string S_Department, string S_WorkerName, string S_WorkDate,string S_OrgCode,string S_WorkerCode,string Is_Page,string S_BeginWorkDate,string S_EndWorkDate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkerCode"] = S_WorkerCode;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["Is_Page"] = Is_Page;
            d["S_BeginWorkDate"] = S_BeginWorkDate;
            d["S_EndWorkDate"] = S_EndWorkDate;
            Dictionary<string, object> res = tsm.GetTaxSalaryList(d);
            return Json(res);
        }
        /// <summary>
        /// 基础工资数据导出
        /// </summary>
        /// <param name="S_OrgName"></param>
        /// <param name="S_WorkerName"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_OrgCode"></param>
        /// <returns></returns>
        [HttpGet("exportBaseTaxSalary")]
        public IActionResult exportBaseTaxSalary( string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            Dictionary<string, object> res = tsm.GetExportTaxSalary(d);
            return Json(res);
        }

        /// <summary>
        /// 月度个税统计
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="S_Department"></param>
        /// <param name="S_WorkerName"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_OrgCode"></param>
        /// <returns></returns>
        [HttpGet("getMonthTaxSalaryDetail")]
        public IActionResult getMonthTaxSalaryDetail(string limit, string page, string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode,string Is_Page,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["Is_Page"] = Is_Page;
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.getMonthTaxSalaryDetail(d);
            return Json(res);
        }
        
             [HttpGet("getGroupMonthTaxSalary")]
        public IActionResult getGroupMonthTaxSalary(string S_Department, string S_WorkDate, string S_OrgCode,string TaxNumber,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxNumber"] = TaxNumber;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.getGroupMonthTaxSalary(d);
            return Json(res);
        }
        [HttpGet("getGroupSumMonthTaxSalary")]
        public IActionResult getGroupSumMonthTaxSalary(string S_Department, string S_WorkDate, string S_OrgCode, string TaxNumber,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxNumber"] = TaxNumber;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.getGroupSumMonthTaxSalary(d);
            return Json(res);
        }
        

        [HttpGet("getMonthTaxSalary")]
        public IActionResult getMonthTaxSalary(string limit, string page, string S_Department, string S_WorkerName, string S_WorkDate,  string S_OrgCode, string Is_Page,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["Is_Page"] = Is_Page;
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.getMonthTaxSalary(d);
            return Json(res);
        }


        [HttpGet("exportMonthTaxSalaryDetail")]
        public IActionResult exportMonthTaxSalaryDetail(string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode,string S_OrgName,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["S_OrgName"] = S_OrgName;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.ExportMonthTaxSalaryDetail(d);
            return Json(res);
        }
        [HttpGet("exportGroupMonthTaxSalary")]
        public IActionResult exportGroupMonthTaxSalary(string S_Department, string S_WorkDate, string S_OrgCode, string TaxNumber, string S_OrgName,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxNumber"] = TaxNumber;
            d["S_OrgName"] = S_OrgName;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.exportGroupMonthTaxSalary(d);
            return Json(res);
        }
        [HttpGet("exportGroupSumMonthTaxSalary")]
        public IActionResult exportGroupSumMonthTaxSalary(string S_Department, string S_WorkDate, string S_OrgCode, string TaxNumber,string S_OrgName,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxNumber"] = TaxNumber;
            d["S_OrgName"] = S_OrgName;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.exportGroupSumMonthTaxSalary(d);
            return Json(res);
        }
        [HttpGet("exportPreCalTax")]
        public IActionResult exportPreCalTax(string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode, string S_OrgName,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["S_OrgName"] = S_OrgName;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.ExportPreCalTax(d);
            return Json(res);
        }
        
        [HttpGet("exportMonthTaxSalary")]
        public IActionResult exportMonthTaxSalary(string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode, string S_OrgName,string TaxRate)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["S_OrgName"] = S_OrgName;
            d["TaxRate"] = TaxRate;
            Dictionary<string, object> res = tsm.ExportMonthTaxSalary(d);
            return Json(res);
        }

        /// <summary>
        /// 上传导入的工资excel并验证合法性
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("validateTaxSalary")]
        public IActionResult ValidateTaxSalary([FromForm] IFormCollection formCollection, string orgCode,string orgName, DateTime dateMonth)
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
                    String filePath = System.IO.Directory.GetCurrentDirectory() + "\\Files\\salary\\" + timestamp + "_" + name;
                    if (System.IO.File.Exists(filePath))
                    {
                        //// 改名方法
                        //FileInfo fi = new FileInfo(oldStr);
                        //fi.MoveTo(Path.Combine(newStr));
                        ////Process tool = new Process();
                        ////tool.StartInfo.FileName = "handle.exe";
                        ////tool.StartInfo.Arguments = filePath + " /accepteula";
                        ////tool.StartInfo.UseShellExecute = false;
                        ////tool.StartInfo.RedirectStandardOutput = true;
                        ////tool.Start();
                        ////tool.WaitForExit();
                        ////string outputTool = tool.StandardOutput.ReadToEnd();

                        ////string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
                        ////foreach (Match match in Regex.Matches(outputTool, matchPattern))
                        ////{
                        ////    Process.GetProcessById(int.Parse(match.Value)).Kill();
                        ////}
                        //Process[] pcs = Process.GetProcesses();
                        //foreach (Process p in pcs)
                        //{
                        //    if (p.MainModule != null)
                        //    {
                        //        if (p.MainModule.FileName == filePath)
                        //        {
                        //            p.Kill();
                        //        }
                        //    }
                        //}
                        System.IO.File.Delete(filePath);
                    }
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    r = tsm.ValidateTaxSalary(filePath, orgCode, orgName, dateMonth);
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
        /// 导入工资
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadTaxSalary")]
        //public IActionResult UploadTaxSalary([FromForm] IFormCollection formCollection, string orgCode, DateTime dateMonth,string userId)
        public IActionResult UploadTaxSalary(string filePath, string orgCode,string orgName, DateTime dateMonth, string userId)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    //System.IO.File.Delete(filePath);
                    r = tsm.ImportTaxSalary(filePath, orgCode, orgName, dateMonth, userId);
                }
                //using (FileStream fs = System.IO.File.Create(filePath))
                //{
                //    // 复制文件
                //    file.CopyTo(fs);
                //    // 清空缓冲区数据
                //    fs.Flush();
                //}
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = "导入失败！" + ex.Message;
            }
            return Json(r);
        }

        //[HttpPost("getIsComputeByCode")]
        //public ContentResult getIsComputeByCode([FromBody]JObject value)
        //{
        //    Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //    string orgCode= d["orgCode"].ToString();
        //    string result = tom.getIsComputeByCode(orgCode);
        //    return Content(result);
        //}

        [HttpPost("getReportStatus")]
        public IActionResult getReportStatus([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            string orgCode = d["orgCode"].ToString();
            DateTime dateMonth = Convert.ToDateTime(d["dateMonth"].ToString());
            Dictionary<string, object> result = tsm.validateReportStatus(orgCode, dateMonth);
            return Json(result);
        }

        /// <summary>
        /// 删除工资信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("delTaxSalary")]
        public IActionResult delTaxSalary([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = tsm.deleteTaxSalary(Convert.ToDateTime(d["dateMonth"].ToString()), d["orgCode"].ToString());
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


        /// <summary>
        /// 个税测算
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="S_Department"></param>
        /// <param name="S_WorkerName"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_OrgCode"></param>
        /// <returns></returns>
        [HttpGet("getPreCalculateTax")]
        public IActionResult getPreCalculateTax(string limit, string page, string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode, string Is_Page, string TaxCode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["Is_Page"] = Is_Page;
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxCode"] = TaxCode;
            Dictionary<string, object> res = tsm.getPreCalculateTax(d);
            return Json(res);
        }

        [HttpGet("getSumPreCalculateTax")]
        public IActionResult getSumPreCalculateTax(string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode,string TaxCode)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxCode"] = TaxCode;
            Dictionary<string, object> res = tsm.getSumPreCalculateTax(d);
            return Json(res);
        }
        
    }
}