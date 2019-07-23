using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace UIDP.ODS
{
    public class TaxOrgDB
    {
        DBTool db = new DBTool("MYSQL");

        /// <summary>
        /// 查询单位配置信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getTaxOrgList(Dictionary<string, object> d)
        {
            //string sql = @"select a.*,(select name from tax_dictionary where a.ResponsibilityCenter=tax_dictionary.Code) ResponsibilityCenterName,(select name from tax_dictionary where a.OrgRegion=tax_dictionary.Code) OrgRegionName,(select name from tax_dictionary where a.TaxOffice=tax_dictionary.Code) TaxOfficeName,(select name from tax_dictionary where a.TaxCode=tax_dictionary.Code) TaxNumber from tax_org a where 1=1  ";
            string sql = @"select a.S_Id,a.S_CreateDate,a.S_CreateBy,a.S_UpdateBy,a.S_UpdateDate,a.S_OrgCode,(select ORG_NAME from ts_uidp_org where a.S_OrgCode=ts_uidp_org.ORG_CODE) S_OrgName,a.ImportModel,a.TaxOffice,a.ResponsibilityCenter,a.IsComputeTax,a.OrgRegion,a.TaxCode,(select name from tax_dictionary where a.ResponsibilityCenter=tax_dictionary.Code) ResponsibilityCenterName,(select name from tax_dictionary where a.OrgRegion=tax_dictionary.Code) OrgRegionName,(select name from tax_dictionary where a.TaxOffice=tax_dictionary.Code) TaxOfficeName,(select name from tax_dictionary where a.TaxCode=tax_dictionary.Code) TaxNumber from tax_org a where 1=1 ";
            if (d.Keys.Contains("S_OrgCode") && d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " and S_OrgCode like '%" + d["S_OrgCode"].ToString() + "%'";
            }
            if (d.Keys.Contains("TaxNumber") && d["TaxNumber"] != null && d["TaxNumber"].ToString() != "")
            {
                sql += " and TaxCode ='" + d["TaxNumber"].ToString() + "' ";
            }
            if (d.Keys.Contains("TaxOffice") && d["TaxOffice"] != null && d["TaxOffice"].ToString() != "")
            {
                sql += " and TaxOffice ='" + d["TaxOffice"].ToString() + "' ";
            }
            if (d.Keys.Contains("ResponsibilityCenter") && d["ResponsibilityCenter"] != null && d["ResponsibilityCenter"].ToString() != "")
            {
                sql += " and ResponsibilityCenter ='" + d["ResponsibilityCenter"].ToString() + "' ";
            }
            if (d.Keys.Contains("OrgRegion") && d["OrgRegion"] != null && d["OrgRegion"].ToString() != "")
            {
                sql += " and OrgRegion ='" + d["OrgRegion"].ToString() + "' ";
            }
            if (d.Keys.Contains("ImportModel") && d["ImportModel"] != null && d["ImportModel"].ToString() != "")
            {
                sql += " and ImportModel ='" + d["ImportModel"].ToString() + "' ";
            }
            sql += " order by S_OrgCode,S_CreateDate ";
            return db.GetDataTable(sql);
        }



        /// <summary>
        /// 根据部门编码获取本部门下组织机构对照信息
        /// </summary>
        /// <param name="orgCode">部门编码</param>
        /// <returns></returns>
        public DataTable getTaxOrgInfo(string orgCode)
        {
            string sql = @"select a.ORG_ID,a.ORG_CODE,a.ORG_NAME,a.ORG_SHORT_NAME,b.ImportModel,
                                    b.TaxOffice,b.ResponsibilityCenter,b.IsComputeTax,b.OrgRegion from ts_uidp_org a 
                                    left join tax_org b on a.ORG_CODE = b.S_OrgCode 
                                    where  ORG_CODE = '" + orgCode + "'";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 根据部门编码查询单位组织机构
        /// </summary>
        /// <param name="orgCode">部门编码</param>
        /// <returns></returns>
        public DataTable getOrg(string orgCode)
        {
            string sql = "select * from ts_uidp_org where ORG_CODE like '" + orgCode + "%' order by ORG_CODE";
            return db.GetDataTable(sql);
        }

        public string validateRepeat(string orgCode)
        {
            string sql = "select count(*) from tax_org where S_OrgCode='" + orgCode + "'";
            return db.GetString(sql);
        }
        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createTaxOrg(Dictionary<string, object> d)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tax_org(S_Id,S_CreateDate,S_CreateBy,S_OrgCode,S_OrgName,ImportModel,TaxOffice,ResponsibilityCenter,IsComputeTax,OrgRegion,TaxCode)VALUES(");
            sql.Append(GetIsNullStr(d["S_Id"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_CreateDate"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_CreateBy"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_OrgCode"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_OrgName"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["ImportModel"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["TaxOffice"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["ResponsibilityCenter"].ToString()));
            // sql.Append(",1,");
            sql.Append(",");
            sql.Append(GetIsNull(d["IsComputeTax"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["OrgRegion"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["TaxCode"].ToString()));
            sql.Append(")");
            return db.ExecutByStringResult(sql.ToString().Trim());
        }
        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateTaxOrg(Dictionary<string, object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" update tax_org set ");
            sb.Append(" S_UpdateBy=");
            sb.Append(GetIsNullStr(d["S_UpdateBy"].ToString()) + ", ");
            sb.Append(" S_UpdateDate=");
            sb.Append(GetIsNullStr(d["S_UpdateDate"].ToString()) + ", ");
            sb.Append(" S_OrgCode=");
            sb.Append(GetIsNullStr(d["S_OrgCode"].ToString()) + ", ");
            sb.Append(" S_OrgName=");
            sb.Append(GetIsNullStr(d["S_OrgName"].ToString()) + ", ");
            sb.Append(" ImportModel=");
            sb.Append( GetIsNullStr(d["ImportModel"].ToString()) + ", ");
            sb.Append(" TaxOffice=");
            sb.Append(GetIsNullStr(d["TaxOffice"].ToString()) + ", ");
            sb.Append(" ResponsibilityCenter=");
            sb.Append(GetIsNullStr(d["ResponsibilityCenter"].ToString()) + ", ");
            sb.Append(" OrgRegion=");
            sb.Append(GetIsNullStr(d["OrgRegion"].ToString()) + ", ");
            sb.Append(" IsComputeTax=");
            sb.Append(GetIsNull(d["IsComputeTax"].ToString()) + ", ");
            sb.Append(" TaxCode=");
            sb.Append(GetIsNullStr(d["TaxCode"].ToString()));
            sb.Append(" where S_Id=" + GetIsNullStr(d["S_Id"].ToString()));
            return db.ExecutByStringResult(sb.ToString());
        }

        public string GetIsNullStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "null";
            }
            else
            {
                return "'"+str.ToString()+"'";
            }
        }
        public string GetIsNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "null";
            }
            else
            {
                return str.ToString();
            }
        }
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public string delTaxOrg(string sid)
        {
            string sql = "DELETE FROM tax_org WHERE S_Id='" + sid + "'";
            return db.ExecutByStringResult(sql);
        }

        /// <summary>
        /// 根据类型获取字典项
        /// </summary>
        /// <returns></returns>
        public DataTable getItemByType(string parentCode)
        {
            string sql = "select * from tax_dictionary where ParentCode='"+ parentCode + "' ORDER BY SortNo";
            return db.GetDataTable(sql);
        }
    }
}
