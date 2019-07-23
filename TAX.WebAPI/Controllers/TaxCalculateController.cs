using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;
namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("TaxCalculate")]
    public class TaxCalculateController : WebApiBaseController
    {
        TaxCalculateModule md = new TaxCalculateModule();
        /// <summary>
        /// 计算个税
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Calculate")]
        public IActionResult Post([FromBody]TaxCalculateParm cal)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(md.CalculateTax(cal.UserId,cal.WorkMonth,cal.OrgCode));
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = "计算失败："+e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 个税测算
        /// </summary>
        /// <param name="S_Department"></param>
        /// <param name="S_WorkerName"></param>
        /// <param name="S_WorkDate"></param>
        /// <param name="S_OrgCode"></param>
        /// <returns></returns>
        [HttpGet("calPreCalculateTax")]
        public IActionResult calPreCalculateTax(string S_Department, string S_WorkerName, string S_WorkDate, string S_OrgCode,string S_UpdateBy)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_Department"] = S_Department;
            d["S_WorkerName"] = S_WorkerName;
            d["S_WorkDate"] = S_WorkDate;
            d["S_OrgCode"] = S_OrgCode;
            d["S_UpdateBy"] = S_UpdateBy;
            string b = md.calPreCalculateTax(d);
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
            return Json(r);
        }
        /// <summary>
        /// 查询当前部门工资信息，是否导入是否计算，是否上报过
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("getCalculateData")]
        public IActionResult Get(string OrgCode,DateTime WorkMonth)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(md.getCalculateData(OrgCode,  WorkMonth));
            }
            catch (Exception e)
            {
                r["TaxStatus"] = -2;
                r["items"] = JsonConvert.SerializeObject(new DataTable());
                r["TaxPayerCount"] = 0;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 查询当前部门工资信息，是否导入是否计算，是否上报过
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("InverseCalculate")]
        public IActionResult GetInverseCalculate(decimal TaxNumber)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(md.GetInverseCalculate(TaxNumber));
            }
            catch (Exception e)
            {
                r["result"] = "计算失败！";
                r["code"] = -1;
                r["message"] = "程序输错："+e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 税额计算机（导入工资，按全年平均核算计算税额）
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("CalculateImportSalary")]
        public IActionResult CalculateImportSalary([FromForm] IFormCollection formCollection, string userId, string importModel,string taxOffice)
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
                    String timestamp = DateTime.Now.ToString("yyyy-MM-dd")+Guid.NewGuid().ToString();
                    String filePath = System.IO.Directory.GetCurrentDirectory() + "\\Files\\salary\\" + timestamp + "_" + name;
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
                    r = md.ImportTaxSalary(filePath,  userId, importModel, taxOffice);
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
        /// 新加个税计算器
        /// </summary>
        /// <param name="TaxNumber"></param>
        /// <returns></returns>
        [HttpGet("cal")]
        public IActionResult GetCal(decimal nashuie)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(md.cal(nashuie));
            }
            catch (Exception e)
            {
                r["result"] = "计算失败！";
                r["code"] = -1;
                r["message"] = "程序输错：" + e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 新加个税计算器
        /// </summary>
        /// <param name="TaxNumber"></param>
        /// <returns></returns>
        [HttpGet("onetimecal")]
        public IActionResult GetOneTimeCal(decimal nashuie2)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(md.onetimecal(nashuie2));
            }
            catch (Exception e)
            {
                r["result"] = "计算失败！";
                r["code"] = -1;
                r["message"] = "程序输错：" + e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 按年计算额度
        /// </summary>
        /// <param name="nashuie"></param>
        /// <returns></returns>
        [HttpGet("Yearcal")]
        public IActionResult GetYearcal(decimal nashuie3,int month)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                return Json(md.Yearcal(nashuie3,month));
            }
            catch (Exception e)
            {
                r["result"] = "计算失败！";
                r["code"] = -1;
                r["message"] = "程序输错：" + e.Message;
            }
            return Json(r);
        }
    }
    public class TaxCalculateParm {
        public string UserId{get;set;}
        public DateTime WorkMonth { get; set; }
        public string OrgCode { get; set; }
    }
}