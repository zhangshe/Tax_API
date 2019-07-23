using UIDP.UTILITY;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace UIDP.ODS
{
    public class OrgDB
    {
        DBTool db = new DBTool("MYSQL");
        public DataTable fetchOrgListByCode(Dictionary<string, object> sysCode)
        {
            return db.GetDataTable("select * from ts_uidp_org where ISINVALID='1' and ISDELETE='1' and ORG_CODE like '" + sysCode["sysCode"] + "%' order by ORG_CODE ");
        }
        /// <summary>
        /// 新增组织结构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createOrgArticle(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO ts_uidp_org(ORG_ID,ORG_CODE,ORG_NAME,ORG_SHORT_NAME,ORG_ID_UPPER,ORG_CODE_UPPER,ISINVALID,ISDELETE,REMARK) VALUES(";
            sql += "'" + GetIsNullStr(d["id"]) + "',";
            sql += "'" + GetIsNullStr(d["orgCode"]) + "',";
            sql += "'" + GetIsNullStr(d["orgName"]) + "',";
            sql += "'" + GetIsNullStr(d["orgShortName"]) + "',";
            sql += "'" + GetIsNullStr(d["parentId"]) + "',";
            sql += "'" + GetIsNullStr(d["orgpCode"]) + "',";
            sql += "'" + GetIsNullStr(d["ISINVALID"]) + "',";
            sql += "'1',";//伪删除 1正常，0伪删除
            sql += "'" + GetIsNullStr(d["remark"]) + "')";
            return db.ExecutByStringResult(sql);
        }

        public string updateOrgPID()
        {
            //            string sql = @"update ts_uidp_org a,ts_uidp_org b set a.ORG_ID_UPPER=b.ORG_ID
            //where a.ORG_CODE_UPPER=b.ORG_CODE";
            string sql = @"update ts_uidp_org a,ts_uidp_org b set a.ORG_ID_UPPER=b.ORG_ID
                where a.ORG_CODE_UPPER = b.ORG_CODE";
            if (db.db.dbType == DB.DBTYPE.MYSQL) {
                 sql = @"update ts_uidp_org a,ts_uidp_org b set a.ORG_ID_UPPER=b.ORG_ID
                where a.ORG_CODE_UPPER=b.ORG_CODE";
            }
            else if (db.db.dbType == DB.DBTYPE.SQLSERVER) {
                 sql = @"update a set a.ORG_ID_UPPER = b.ORG_ID from ts_uidp_org a,ts_uidp_org b
                where a.ORG_CODE_UPPER = b.ORG_CODE";
            }
            //string sql = @"update ts_uidp_org set ORG_ID_UPPER =case 
            //    when ORG_CODE_UPPER = ORG_CODE then ORG_ID
            //    else null
            //    END";
            return db.ExecutByStringResult(sql);
        }


        public string GetIsNullStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 修改组织结构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateOrgData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_org set ";
            sql += " ORG_CODE='" + GetIsNullStr(d["orgCode"]) + "',";
            sql += " ORG_NAME='" + GetIsNullStr(d["orgName"]) + "',";
            sql += " ORG_SHORT_NAME='" + GetIsNullStr(d["orgShortName"]) + "',";
            sql += " ORG_ID_UPPER='" + GetIsNullStr(d["parentId"]) + "',";
            sql += " ORG_CODE_UPPER='" + GetIsNullStr(d["orgpCode"]) + "',";
            sql += " ISINVALID='" + GetIsNullStr(d["ISINVALID"]) + "',";
            sql += " REMARK='" + GetIsNullStr(d["remark"]) + "'";
            sql += " where ORG_ID='" + d["id"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 删除组织机构，变成伪删除
        /// </summary>
        /// <param name="d">带单引号 逗号分隔的id字符串</param>
        /// <returns></returns>
        public string updateOrgArticle(string strid)
        {
            //string sql = "update ts_uidp_org set ISDELETE='0' where ORG_ID in(" + strid + ")";
            //string sql = "update ts_uidp_org set ISDELETE='0' where ORG_CODE like '" + strid + "%'";
            string sql = "delete from  ts_uidp_org  where ORG_CODE like '" + strid + "%'";
            return db.ExecutByStringResult(sql);
        }

        public string clearOrg()
        {
            //string sql = "delete ts_uidp_org set ISDELETE='0' where ISDELETE ='1'";
            string sql = "TRUNCATE TABLE ts_uidp_org";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 分配组织结构给用户
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserOrgArticle(Dictionary<string, object> d)
        {
            // string[] array = d["multipleSelection"].ToString().Split(',');
            var array = (JArray)d["arr"];
            string fengefu = "";
            string sql = " insert into ts_uidp_org_user(ORG_ID,USER_ID)values ";
            string delSql = "delete from ts_uidp_org_user where  USER_ID in (";
            foreach (var item in array)
            {
                delSql += fengefu + "'" + item.ToString() + "'";
                sql += fengefu + "(";
                sql += "'" + d["orgId"].ToString() + "','" + item.ToString() + "'";
                sql += ")";
                fengefu = ",";
            }
            delSql += ")";
            List<string> list = new List<string>();
            list.Add(delSql);
            list.Add(sql);
            return db.Executs(list);
        }
        /// <summary>
        /// 通过组织结构id查询
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable GetOrgById(string orgId)
        {
            string sql = "select * FROM ts_uidp_org where ORG_ID='" + orgId + "' ;";
            return db.GetDataTable(sql);
        }
        public DataTable GetOrgByCode(string orgCode)
        {
            string sql = "select * FROM ts_uidp_org where ORG_CODE_UPPER='" + orgCode + "' ;";
            return db.GetDataTable(sql);
        }

        public string getValidateNum(string orgCode)
        {
            string num = "0";
            string sql = "select count(*) from ts_uidp_org inner join ts_uidp_org_user on ts_uidp_org.ORG_ID = ts_uidp_org_user.ORG_ID and ts_uidp_org.ORG_CODE like '"+ orgCode + "%'";
            num = db.GetString(sql);
            return num;
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public DataTable fetchOrgList()
        {
            //string sql = "select * FROM ts_uidp_org where ISDELETE='1'";
            string sql = @"select a.*,(select ts_uidp_org.ORG_ID from ts_uidp_org where ts_uidp_org.ORG_CODE=a.ORG_CODE_UPPER) PID FROM ts_uidp_org a 
where a.ISDELETE='1' order by ORG_CODE";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询所有同步数据
        /// </summary>
        /// <returns></returns>
        public DataTable syncOrgList()
        {
            string sql = "select * FROM ts_uidp_org";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 清空用户组织机构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteUserOrgArticle(Dictionary<string, object> d)
        {
            var array = (JArray)d["arr"];
            if (array == null || array.Count == 0)
            {
                return "";
            }
            string fengefu = "";
            string delSql = " delete from ts_uidp_org_user where  USER_ID in(";
            foreach (var item in array)
            {
                delSql += fengefu + "'" + item.ToString() + "'";
                fengefu = ",";
            }
            delSql += ")";
            return db.ExecutByStringResult(delSql);
        }
        /// <summary>
        /// 导入org
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string UploadOrgFile(string sql)
        {
            return db.ExecutByStringResult(sql);
        }
        public string UploadOrgFileList(List<string> sqllst)
        {
            return db.Executs(sqllst);
        }
        public string GetDBType()
        {
            return Enum.GetName(typeof(DB.DBTYPE), (int)db.db.dbType);
        }
    }
}
