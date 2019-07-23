using UIDP.UTILITY;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace UIDP.ODS
{
   public class RoleDB
    {
        DBTool db = new DBTool("MYSQL");

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createRoleArticle(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO ts_uidp_groupinfo(SYS_CODE,GROUP_ID,GROUP_CODE,GROUP_NAME,GROUP_CODE_UPPER,REMARK) VALUES(";
            sql += "'" + GetIsNullStr(d["sysCode"]) + "',";
            sql += "'" + GetIsNullStr(d["id"]) + "',";
            sql += "'" + GetIsNullStr(d["groupCode"]) + "',";
            sql += "'" + GetIsNullStr(d["groupName"]) + "',";
            sql += "'" + GetIsNullStr(d["parentId"]) + "',";
            sql += "'" + GetIsNullStr(d["remark"]) + "')";
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
        /// 修改角色
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateRoleData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_groupinfo set ";
            sql += " SYS_CODE='" + GetIsNullStr(d["sysCode"]) + "',";
            sql += " GROUP_CODE='" + GetIsNullStr(d["groupCode"]) + "',";
            sql += " GROUP_NAME='" + GetIsNullStr(d["groupName"]) + "',";
            sql += " GROUP_CODE_UPPER='" + GetIsNullStr(d["parentId"]) + "',";
            sql += " REMARK='" + GetIsNullStr(d["remark"]) + "'";
            sql += " where GROUP_ID='" + d["id"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        ///<summary>
        /// 删除角色
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateRoleArticle(string ids)
        {
            string sql = "delete FROM ts_uidp_groupinfo where GROUP_ID in(" + ids + ") ;";

            return db.ExecutByStringResult(sql);
        }
        ///<summary>
        /// 根据SYS_CODE查角色表ts_uidp_groupinfo
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetRoleBySYS_CODE(Dictionary<string, object> d)
        {
            string sql = " select * from ts_uidp_groupinfo ";
            if (d["sysCode"]!=null&& d["sysCode"].ToString()!="") {
                sql += " where SYS_CODE='" + d["sysCode"] + "'";
            }
            return db.GetDataTable(sql);
        }
        ///<summary>
        /// 根据GROUP_ID查角色表ts_uidp_groupinfo
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetRoleById(string roleId) {
            string sql = " select * from ts_uidp_groupinfo where GROUP_ID='"+roleId+"'";
            return db.GetDataTable(sql);
        }
        public DataTable GetRoles()
        {
            string sql = " select * from ts_uidp_groupinfo ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 分配角色给用户
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserRoleArticle(Dictionary<string, object> d)
        {
            // string[] array = d["multipleSelection"].ToString().Split(',');
            var array = (JArray)d["arr"];
            if (array==null||array.Count==0) {
                return "";
            }
            string fengefu = "";
            string sql = " insert into ts_uidp_group_user(GROUP_ID,USER_ID)values ";
            string delSql = " delete from ts_uidp_group_user where  GROUP_ID='" + d["GROUP_ID"].ToString()+ "' and USER_ID in(";
            foreach (var item in array)
            {
                delSql += fengefu + "'" + item.ToString()+"'" ;
                sql += fengefu + "(";
                sql += "'" + d["GROUP_ID"].ToString() + "','" + item.ToString() + "'";
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
        /// 清空用户角色
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteUserRoleArticle(Dictionary<string, object> d)
        {
            // string[] array = d["multipleSelection"].ToString().Split(',');
            var array = (JArray)d["arr"];
            if (array == null || array.Count == 0)
            {
                return "";
            }
            string fengefu = "";
            string delSql = " delete from ts_uidp_group_user where GROUP_ID= '"+ d["GROUP_ID"] + "' and  USER_ID in(";
            foreach (var item in array)
            {
                delSql += fengefu + "'" + item.ToString() + "'";
                fengefu = ",";
            }
            delSql += ")";
            return db.ExecutByStringResult(delSql);
        }
    }
}
