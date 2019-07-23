using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule;
using UIDP.BIZModule.Modules;

namespace TAX.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("TaxOrg")]
    public class TaxOrgController : WebApiBaseController
    {
        TaxOrgModule md = new TaxOrgModule();
        /// <summary>
        /// 查询组织结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchTaxOrgList")]
        public IActionResult Get(string S_OrgCode)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string jsonStr = md.getOrg(S_OrgCode);
                r["items"] = JsonConvert.DeserializeObject("[" + jsonStr + "]");
                r["message"] = "成功";
                r["code"] = 2000;
            }
            catch (Exception ex)
            {
                r["items"] = JsonConvert.DeserializeObject("[]");
                r["message"] = ex.Message;
                r["code"] = -1;
            }
            return Json(r);
        }


        [HttpGet("fetchTaxOrgNodeList")]
        public dynamic fetchTaxOrgNodeList(string S_OrgCode)
        {
            List<TaxOrgNode> TaxList = md.getGetlist(S_OrgCode);
            return TaxList;
        }


        /// <summary>
        /// 查询组织机构配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getTaxOrgList")]
        public IActionResult getTaxOrgList(string limit, string page, string S_OrgCode, string ResponsibilityCenter, string TaxOffice, string ImportModel, string TaxNumber,string OrgRegion)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["S_OrgCode"] = S_OrgCode;
            d["TaxNumber"] = TaxNumber;
            d["ImportModel"] = ImportModel;
            d["TaxOffice"] = TaxOffice;
            d["ResponsibilityCenter"] = ResponsibilityCenter;
            d["OrgRegion"] = OrgRegion;
            Dictionary<string, object> res = md.getTaxOrgList(d);
            return Json(res);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createTaxOrg")]
        public IActionResult createTaxOrg([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string allow = md.validateRepeat(d["S_OrgCode"].ToString());
                if (allow == "0")
                {
                    string b = md.createTaxOrg(d);
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
                else {
                    r["code"] = -1;
                    r["message"] = "已存在"+ d["S_OrgName"].ToString()+"配置信息！";
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
        [HttpPost("updateTaxOrg")]
        public IActionResult updateTaxOrg([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = md.updateTaxOrg(d);
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
        [HttpPost("deleteTaxOrg")]
        public IActionResult deleteTaxOrg(string S_ID)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            //Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            try
            {
                string b = md.deleteTaxOrg(S_ID);
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
        /// 获取字典项下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllDictionary")]
        public IActionResult getAllDictionary()
        {
            Dictionary<string, object> res = md.getAllDictionary();
            return Json(res);
        }

    }
}