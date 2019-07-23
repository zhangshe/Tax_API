using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.BIZModule.Modules;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class TaxOrgModule
    {
        TaxOrgDB db = new TaxOrgDB();

        public Dictionary<string, object> getTaxOrgList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.getTaxOrgList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createTaxOrg(Dictionary<string, object> d)
        {
            d["S_Id"] = Guid.NewGuid().ToString();
            d["S_CreateDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            return db.createTaxOrg(d);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateTaxOrg(Dictionary<string, object> d)
        {
            d["S_UpdateDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            return db.updateTaxOrg(d);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteTaxOrg(string id)
        {
            return db.delTaxOrg(id);
        }

        /// <summary>
        /// 根据部门编码获取对照表信息
        /// </summary>
        /// <param name="orgCode">部门编码</param>
        /// <returns></returns>
        public DataTable getTaxOrgInfo(string orgCode)
        {
            return db.getTaxOrgInfo(orgCode);
        }
        /// <summary>
        /// 根据部门编码查询组织单位
        /// </summary>
        /// <param name="orgCode">部门编码</param>
        /// <returns></returns>
        public string getOrg(string orgCode)
        {
            DataTable dt = db.getOrg(orgCode);
            string orgList = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rows = rows = dt.Select("ORG_CODE='" + orgCode + "'");
                orgList = GetSubMenu(rows[0]["ORG_ID"].ToString(), dt);
            }
            return orgList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dt"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public string GetSubMenu(string pid, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = rows = dt.Select("ORG_ID_UPPER='" + pid + "'");
            if (string.IsNullOrEmpty(pid))
            {
                rows = dt.Select("ORG_ID_UPPER is null or ORG_ID_UPPER ='' ");
            }
            sb.Append("{");
            sb.AppendFormat("\"id\":\"{0}\",", dt.Rows[0]["ORG_ID"] == null ? "" : dt.Rows[0]["ORG_ID"]);
            sb.AppendFormat("\"orgCode\":\"{0}\",", dt.Rows[0]["ORG_CODE"] == null ? "" : dt.Rows[0]["ORG_CODE"]);
            sb.AppendFormat("\"orgName\":\"{0}\",", dt.Rows[0]["ORG_NAME"] == null ? "" : dt.Rows[0]["ORG_NAME"]);
            sb.AppendFormat("\"orgShortName\":\"{0}\",", dt.Rows[0]["ORG_SHORT_NAME"] == null ? "" : dt.Rows[0]["ORG_SHORT_NAME"]);
            sb.AppendFormat("\"parentId\":\"{0}\",", dt.Rows[0]["ORG_ID_UPPER"] == null ? "" : dt.Rows[0]["ORG_ID_UPPER"]);
            sb.AppendFormat("\"ISINVALID\":\"{0}\",", dt.Rows[0]["ISINVALID"] == null ? "" : dt.Rows[0]["ISINVALID"]);
            sb.AppendFormat("\"remark\":\"{0}\"", dt.Rows[0]["REMARK"] == null ? "" : dt.Rows[0]["REMARK"]);
            sb.Append(",\"children\":[");

            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (dr["ISINVALID"].ToString() == "0")
                    {
                        continue;
                    }
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["ORG_ID"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",", dr["ORG_ID"] == null ? "" : dr["ORG_ID"]);
                    sb.AppendFormat("\"orgCode\":\"{0}\",", dr["ORG_CODE"] == null ? "" : dr["ORG_CODE"]);
                    sb.AppendFormat("\"orgName\":\"{0}\",", dr["ORG_NAME"] == null ? "" : dr["ORG_NAME"]);
                    sb.AppendFormat("\"orgShortName\":\"{0}\",", dr["ORG_SHORT_NAME"] == null ? "" : dr["ORG_SHORT_NAME"]);
                    sb.AppendFormat("\"parentId\":\"{0}\",", dr["ORG_ID_UPPER"] == null ? "" : dr["ORG_ID_UPPER"]);
                    sb.AppendFormat("\"ISINVALID\":\"{0}\",", dr["ISINVALID"] == null ? "" : dr["ISINVALID"]);
                    sb.AppendFormat("\"remark\":\"{0}\"", dr["REMARK"] == null ? "" : dr["REMARK"]);
                    sb.Append(",\"children\":[");
                    sb.Append(GetSubMenuChildren(id, dt));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }
        public string GetSubMenuChildren(string pid, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = rows = dt.Select("ORG_ID_UPPER='" + pid + "'");
            if (string.IsNullOrEmpty(pid))
            {
                rows = dt.Select("ORG_ID_UPPER is null or ORG_ID_UPPER ='' ");
            }
            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (dr["ISINVALID"].ToString() == "0")
                    {
                        continue;
                    }
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["ORG_ID"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",", dr["ORG_ID"] == null ? "" : dr["ORG_ID"]);
                    sb.AppendFormat("\"orgCode\":\"{0}\",", dr["ORG_CODE"] == null ? "" : dr["ORG_CODE"]);
                    sb.AppendFormat("\"orgName\":\"{0}\",", dr["ORG_NAME"] == null ? "" : dr["ORG_NAME"]);
                    sb.AppendFormat("\"orgShortName\":\"{0}\",", dr["ORG_SHORT_NAME"] == null ? "" : dr["ORG_SHORT_NAME"]);
                    sb.AppendFormat("\"parentId\":\"{0}\",", dr["ORG_ID_UPPER"] == null ? "" : dr["ORG_ID_UPPER"]);
                    sb.AppendFormat("\"ISINVALID\":\"{0}\",", dr["ISINVALID"] == null ? "" : dr["ISINVALID"]);
                    sb.AppendFormat("\"remark\":\"{0}\"", dr["REMARK"] == null ? "" : dr["REMARK"]);
                    sb.Append(",\"children\":[");
                    sb.Append(GetSubMenuChildren(id, dt));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }

        public List<TaxOrgNode> getGetlist(string S_OrgCode)
        {
            List<TaxOrgNode> LIST = new List<TaxOrgNode>();
            nodeList(S_OrgCode, LIST);
            return LIST;
        }


        public void nodeList(string S_OrgCode, List<TaxOrgNode> LIST)
        {
            DataTable dt = db.getOrg(S_OrgCode);
            foreach (DataRow dr in dt.Select("ORG_CODE = '" + S_OrgCode + "'"))
            {
                TaxOrgNode NODE = new TaxOrgNode();
                NODE.id = dr["ORG_ID"].ToString();
                NODE.orgName = dr["ORG_NAME"].ToString();
                NODE.orgCode = dr["ORG_CODE"].ToString();
                NODE.orgShortName = dr["ORG_SHORT_NAME"].ToString();
                NODE.parentId = dr["ORG_ID_UPPER"].ToString();
                NODE.ISINVALID = dr["ISINVALID"].ToString();
                NODE.remark = dr["remark"].ToString();
                NODE.children = new List<TaxOrgNode>();
                childList(dt, NODE);
                LIST.Add(NODE);
            }
        }

        public void childList(DataTable dt, TaxOrgNode NODE)
        {
            foreach (DataRow du in dt.Select("ORG_ID_UPPER='" + NODE.id + "'"))
            {
                TaxOrgNode childNode = new TaxOrgNode();
                childNode.id = du["ORG_ID"].ToString();
                childNode.orgName = du["ORG_NAME"].ToString();
                childNode.orgCode = du["ORG_CODE"].ToString();
                childNode.orgShortName = du["ORG_SHORT_NAME"].ToString();
                childNode.parentId = du["ORG_ID_UPPER"].ToString();
                childNode.ISINVALID = du["ISINVALID"].ToString();
                childNode.remark = du["remark"].ToString();
                childNode.children = new List<TaxOrgNode>();
                childList(dt, childNode);
                NODE.children.Add(childNode);
            }
        }


        public string validateRepeat(string orgCode)
        {
            return db.validateRepeat(orgCode);
        }

        /// <summary>
        /// 获取组织配置信息中字典下拉选项
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> getAllDictionary()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable taxOffice = db.getItemByType("JGSZD");//获取机关所在地
            DataTable responsibilityCenter = db.getItemByType("ZRZX");//获取责任中心
            DataTable orgRegion = db.getItemByType("SWJG");//获取税务机关
            DataTable taxCode = db.getItemByType("SH");//获取税号编码
            try
            {
                r["message"] = "成功";
                r["code"] = 2000;
                r["taxOffice"] = taxOffice;
                r["responsibilityCenter"] = responsibilityCenter;
                r["orgRegion"] = orgRegion;
                r["taxCode"] = taxCode;
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
    }
}
