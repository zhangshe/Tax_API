using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;
namespace UIDP.ODS
{
    public class UserDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchUserList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.ORG_NAME,b.ORG_ID from ts_uidp_userinfo a ";
            sql += " left join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
            sql += " left join ts_uidp_org b on b.ORG_ID=c.ORG_ID  ";
            sql += " where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["FLAG"] != null && d["FLAG"].ToString() != "")
            {
                sql += " and a.FLAG=" + d["FLAG"].ToString();
            }
            if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
            {
                sql += " order by a.USER_ID DESC";
            }
            else {
                sql += " order by a.USER_ID ASC";
            }
            return db.GetDataTable(sql);
        }
        public string createUserArticle(Dictionary<string, object> d)
        {
            List<string> list = new List<string>();
            string sql = "insert into ts_uidp_org_user(ORG_ID,USER_ID)values ('";
            sql +=d["orgId"].ToString()+ "','"+d["USER_ID"].ToString()+"')";
            list.Add(sql);
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ts_uidp_userinfo(USER_ID,USER_DOMAIN,USER_CODE,USER_NAME,USER_PASS,PHONE_MOBILE,PHONE_OFFICE," +
                "USER_EMAIL,USER_SEX,AUTHENTICATION_TYPE,FLAG,REG_TIME,USER_TYPE,REMARK) VALUES(");
            sb.Append("'");
            sb.Append(d["USER_ID"] == null ? "" : d["USER_ID"] + "', ");
            sb.Append("'");
            sb.Append(d["USER_DOMAIN"] == null ? "" : d["USER_DOMAIN"] + "', ");
            sb.Append("'");
            sb.Append(d["USER_CODE"] == null ? "" : d["USER_CODE"] + "', ");
            sb.Append("'");
            sb.Append(d["USER_NAME"] == null ? "" : d["USER_NAME"] + "', ");
            sb.Append("'");
            sb.Append(d["USER_PASS"] == null ? "" : d["USER_PASS"] + "', ");
            sb.Append("'");
            sb.Append(d["PHONE_MOBILE"] == null ? "" : d["PHONE_MOBILE"] + "', ");
            sb.Append("'");
            sb.Append(d["PHONE_OFFICE"] == null ? "" : d["PHONE_OFFICE"] + "', ");
            sb.Append("'");
            sb.Append(d["USER_EMAIL"] == null ? "" : d["USER_EMAIL"] + "', ");
            sb.Append(d["USER_SEX"] == null ? "1" : d["USER_SEX"] + ",");
            sb.Append(d["AUTHENTICATION_TYPE"] == null ? "" : d["AUTHENTICATION_TYPE"] + ", ");
            sb.Append(d["FLAG"] == null ? "1" : d["FLAG"] + ", ");
            sb.Append("'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"',");
            sb.Append(d["USER_TYPE"]==null?"1" : d["USER_TYPE"].ToString()+",'");
            sb.Append(d["REMARK"] == null ? "" : d["REMARK"] + "' )");
            list.Add(sb.ToString());
            return db.Executs(list);
        }

        public string updateUserArticle(Dictionary<string, object> d)
        {
            List<string> list = new List<string>();
            string sql = "delete FROM ts_uidp_userinfo where USER_ID='" + d["USER_ID"].ToString() + "' ;";
            string sql2= " delete from ts_uidp_org_user where USER_ID='" + d["USER_ID"].ToString() + "' ;";
            list.Add(sql);
            list.Add(sql2);
            return db.Executs(list);
        }

        public string updateUserFlag(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_userinfo set FLAG=" + d["FLAG"] + " where USER_ID='" + d["USER_ID"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        public string updatePasswordData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_userinfo set USER_PASS='" + d["newpassword"].ToString() + "' where USER_ID='" + d["userid"].ToString() + "' and USER_PASS='" + d["password"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        public string updatePTRpass(Dictionary<string, object> d) {
            string sql = "update ts_uidp_userinfo set USER_PASS='" + d["newpassword"].ToString() + "' where USER_ID='" + d["userid"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        public string updateAdminPasswordData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_config set CONF_VALUE='" + d["newpassword"].ToString() + "' where CONF_CODE='Admin_Password' ;";

            return db.ExecutByStringResult(sql);
        }
        public DataTable IsInvalidPassword(Dictionary<string, object> d)
        {
            string sql = "select * from  ts_uidp_userinfo  where USER_ID='" + d["userid"].ToString() + "' and USER_PASS='" + d["password"].ToString() + "' ;";

            return db.GetDataTable(sql);
        }
        //public string updateLoginPasswordData(Dictionary<string, object> d)
        //{
        //    string sql = "update  ts_uidp_login set LOGIN_PASS=" + d["newpassword"].ToString() + " where LOGIN_CODE='" + d["userid"].ToString() + "' and LOGIN_PASS='" + d["password"].ToString() + "' ;";

        //    return db.ExecutByStringResult(sql);
        //}
        //public DataTable IsInvalidLoginPassword(Dictionary<string, object> d)
        //{
        //    string sql = "select * from  ts_uidp_login  where LOGIN_CODE='" + d["userid"].ToString() + "' and LOGIN_PASS='" + d["password"].ToString() + "' ;";

        //    return db.GetDataTable(sql);
        //}
        public string updateUserData(Dictionary<string, object> d,string newpass) {
            StringBuilder sb = new StringBuilder();
            sb.Append(" update ts_uidp_userinfo set ");
            sb.Append(" USER_DOMAIN='");
            sb.Append(d["USER_DOMAIN"] == null ? "" : d["USER_DOMAIN"] + "', ");
            sb.Append(" USER_CODE='");
            sb.Append(d["USER_CODE"] == null ? "" : d["USER_CODE"] + "', ");
            sb.Append(" USER_NAME='");
            sb.Append(d["USER_NAME"] == null ? "" : d["USER_NAME"] + "', ");
            if (d["USER_PASS"]!=null&& d["USER_PASS"].ToString()!="") {
                sb.Append(" USER_PASS= case when USER_PASS<>'"+ d["USER_PASS"].ToString() + "' then '"+newpass+ "' else USER_PASS end , ");
            }
            sb.Append(" PHONE_MOBILE='");
            sb.Append(d["PHONE_MOBILE"] == null ? "" : d["PHONE_MOBILE"] + "', ");
            sb.Append(" PHONE_OFFICE='");
            sb.Append(d["PHONE_OFFICE"] == null ? "" : d["PHONE_OFFICE"] + "', ");
            sb.Append(" USER_EMAIL='");
            sb.Append(d["USER_EMAIL"] == null ? "" : d["USER_EMAIL"] + "', ");
            sb.Append(" USER_SEX=");
            sb.Append(d["USER_SEX"] == null ? "1" : d["USER_SEX"] + ",");
            sb.Append(" AUTHENTICATION_TYPE=");
            sb.Append(d["AUTHENTICATION_TYPE"] == null ? "" : d["AUTHENTICATION_TYPE"] + ", ");
            sb.Append(" FLAG=");
            sb.Append(d["FLAG"] == null ? "1" : d["FLAG"] + ", ");
            sb.Append(" REMARK='");
            sb.Append(d["REMARK"] == null ? "" : d["REMARK"] + "', ");
            sb.Append(" USER_TYPE=");
            if (d["USER_TYPE"] == null || d["USER_TYPE"].ToString() == "" || d["USER_TYPE"].ToString() == "1")
            {
                sb.Append("1");//普通用户
            }
            else {
                sb.Append("0");//管理员
            }
            sb.Append(" where USER_ID='" + d["USER_ID"].ToString() + "' ");
            string sql= "UPDATE ts_uidp_org_user SET ORG_ID='"+d["orgId"] + "'";
            sql += " WHERE USER_ID='" + d["USER_ID"] + "'";
            List<string> sqllist = new List<string>();
            sqllist.Add(sb.ToString());
            sqllist.Add(sql);
            return db.Executs(sqllist);
            //return db.ExecutByStringResult(sb.ToString());
        }
        public DataTable GetUserInfoByUserId(string userId)
        {
            string sql = "select * from ts_uidp_userinfo where USER_ID='" + userId + "' ";
            return db.GetDataTable(sql);
        }
        public DataTable GetUserInfoByUserCode(string userCode, string userid)
        {
            string sql = "select * from ts_uidp_userinfo where USER_CODE='" + userCode + "' ";
            if (!string.IsNullOrEmpty(userid))
            {
                sql += " and USER_ID!='" + userid + "'";
            }
            return db.GetDataTable(sql);
        }
        public DataTable GetUserInfoBylogin(string username, string userDomain)
        {
            string sql = "select * from ts_uidp_userinfo where ";
            if (userDomain == "userDomain") {
                sql += " USER_DOMAIN = '" + username + "' ";
            }
            else
            {//userDomain=user
                sql += " USER_CODE = '" + username + "' ";
            }
            return db.GetDataTable(sql);
        }
        public DataTable GetUserInfoByUSER_DOMAIN(string USER_DOMAIN, string userid)
        {
            string sql = "select * from ts_uidp_userinfo where USER_DOMAIN='" + USER_DOMAIN + "' ";
            if (!string.IsNullOrEmpty(userid)) {
                sql += " and USER_ID!='" + userid + "'";
            }
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取管理员账号
        /// </summary>
        /// <returns></returns>
        public string getAdminCode() {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Admin_Code'";
            return db.GetString(sqluser);
        }/// <summary>
         /// 获取管理员密码
         /// </summary>
         /// <returns></returns>
        public string getAdminPass()
        {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Admin_Password'";
            return db.GetString(sqluser);
        }
        /// <summary>
        /// 根据userid获取组织机构信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserOrg(string userId) {
            string sql = "select a.ORG_ID orgId ,a.ORG_NAME orgName, a.ORG_CODE orgCode from ts_uidp_org a ";
            sql += " join ts_uidp_org_user b on a.ORG_ID=b.ORG_ID ";
            sql += " where b.USER_ID='" + userId + "'; ";
            return db.GetDataTable(sql);

        }
        /// <summary>
        /// 根据userid获取用户和角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserAndGroup(string userId)
        {
            string sql = "select a.*,c.* from  ts_uidp_userinfo a ";
            sql += " left join ts_uidp_group_user b on a.USER_ID=b.USER_ID ";
            sql += " left join ts_uidp_groupinfo c on   c.GROUP_ID=b.GROUP_ID ";
            sql += " where a.USER_ID='" + userId + "'; ";
            return db.GetDataTable(sql);

        }
        /// <summary>
        /// 查询用户信息（包括组织机构）
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchUserOrgList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.ORG_ID orgId,b.ORG_CODE,b.ORG_SHORT_NAME orgName from ts_uidp_userinfo a";
            sql += "  join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
            sql += "  join ts_uidp_org b on b.ORG_ID=c.ORG_ID  where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["USER_DOMAIN"] != null && d["USER_DOMAIN"].ToString() != "")
            {
                sql += " and a.USER_DOMAIN like '%" + d["USER_DOMAIN"].ToString() + "%'";
            }
            if (d["FLAG"] != null && d["FLAG"].ToString() != "")
            {
                sql += " and a.FLAG=" + d["FLAG"].ToString();
            }
            if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
            {
                sql += " order by a.USER_ID DESC";
            }
            else
            {
                sql += " order by a.USER_ID ASC";
            }
            if (d["orgId"] != null) {
                sql = "select a.*,b.ORG_ID orgId,b.ORG_CODE,b.ORG_SHORT_NAME orgName from ts_uidp_userinfo a";
                sql += "  join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_org b on b.ORG_ID=c.ORG_ID  where 1=1 ";
                sql += " and a.USER_ID in (select a.USER_ID from ts_uidp_userinfo a";
                sql += "  join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_org b on b.ORG_ID=c.ORG_ID  where 1=1 ";
                if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
                {
                    sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
                }
                if (d["USER_DOMAIN"] != null && d["USER_DOMAIN"].ToString() != "")
                {
                    sql += " and a.USER_DOMAIN like '%" + d["USER_DOMAIN"].ToString() + "%'";
                }
                if (d["FLAG"] != null && d["FLAG"].ToString() != "")
                {
                    sql += " and a.FLAG=" + d["FLAG"].ToString();
                }
                if (d["orgId"] != null && d["orgId"].ToString() != "" && d["orgpCode"] != null && d["orgpCode"].ToString() != "")
                {
                    //sql += " and b.ORG_ID='" + d["orgId"].ToString() + "' ";
                    sql += " and b.ORG_CODE like '" + d["orgpCode"].ToString() + "%'";
                }
                sql += " ) ";
                if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
                {
                    sql += " order by a.USER_ID DESC";
                }
                else
                {
                    sql += " order by a.USER_ID ASC";
                }
            }
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询用户信息包括角色信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchUserRoleList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.GROUP_ID roleId,b.GROUP_NAME groupName,e.ORG_NAME orgname from ts_uidp_userinfo a";
            sql += " LEFT JOIN ts_uidp_org_user d ON d.USER_ID=a.USER_ID";
            sql+=" LEFT JOIN ts_uidp_org e ON e.ORG_ID=d.ORG_ID";
            sql += " left join ts_uidp_group_user c on c.USER_ID=a.USER_ID ";
            sql += " left join ts_uidp_groupinfo b on b.GROUP_ID=c.GROUP_ID where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["FLAG"] != null && d["FLAG"].ToString() != "")
            {
                sql += " and a.FLAG=" + d["FLAG"].ToString();
            }
            if (d["orgcode"] != null && d["orgcode"].ToString() != "")
            {
                sql += " and e.ORG_CODE LIKE'" + d["orgcode"] + "%'";
            }
            if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
            {
                sql += " order by a.USER_ID desc";
            }
            
            else
            {
                sql += " order by a.USER_ID asc";
            }
            //如果roleid不为空时，只差有roleid 的
            if (d["roleId"] != null && d["roleId"].ToString() != "")
            {
                sql = "select a.*,b.GROUP_ID roleId,b.GROUP_NAME groupName,e.ORG_NAME orgname from ts_uidp_userinfo a";
                sql += " LEFT JOIN ts_uidp_org_user d ON d.USER_ID = a.USER_ID";
                sql += " LEFT JOIN ts_uidp_org e ON e.ORG_ID= d.ORG_ID";
                sql += "  join ts_uidp_group_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_groupinfo b on b.GROUP_ID=c.GROUP_ID where 1=1 ";
                sql += " and a.USER_ID IN ( select a.USER_ID from ts_uidp_userinfo a";
                sql += "  join ts_uidp_group_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_groupinfo b on b.GROUP_ID=c.GROUP_ID where 1=1 ";
                if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
                {
                    sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
                }
                if (d["FLAG"] != null && d["FLAG"].ToString() != "")
                {
                    sql += " and a.FLAG=" + d["FLAG"].ToString();
                }
                if (d["orgcode"] != null && d["orgcode"].ToString() != "")
                {
                    sql += " and e.ORG_CODE LIKE'" + d["orgcode"] + "%'";
                }
                if (d["roleId"] != null && d["roleId"].ToString() != "")
                {
                    sql += " and b.GROUP_ID='" + d["roleId"].ToString() + "' ";
                }
                sql += ")";
                if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
                {
                    sql += " order by a.USER_ID desc";
                }
                else
                {
                    sql += " order by a.USER_ID asc";
                }
            }

            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取sysname
        /// </summary>
        /// <returns></returns>
        public string getSysName() {
            string sql = "select CONF_VALUE from ts_uidp_config where CONF_CODE='SYS_NAME'";
            string sysName = db.GetString(sql);
            return sysName == "" ? "大港油田软件研发平台" : sysName;
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable GetUserAndOrgByUserId(string USER_ID)
        {
            string sql = "select a.*,b.ORG_NAME,b.ORG_SHORT_NAME,b.ORG_ID,b.ORG_CODE from ts_uidp_userinfo a ";
            sql += " left join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
            sql += " left join ts_uidp_org b on b.ORG_ID=c.ORG_ID  ";
            sql += " where a.USER_ID='"+USER_ID+"'";
            return db.GetDataTable(sql);
        }
        public DataTable getSysTime()
        {
            string sql = "SELECT  * FROM tax_date ";
            return db.GetDataTable(sql);
        }
        public string UploadUserFile(List<string> sqlList) {
            return db.Executs(sqlList);
        }

        public DataTable fetchUserList()
        {
            //string sql = "select * FROM ts_uidp_org where ISDELETE='1'";
            string sql = @"select * from ts_uidp_userinfo ";
            return db.GetDataTable(sql);
        }

        public string GetDBType()
        {
            return Enum.GetName(typeof(DB.DBTYPE), (int)db.db.dbType);
        }

        /// <summary>
        /// 获取云组织推送账号
        /// </summary>
        /// <returns></returns>
        public string getCloud_Code()
        {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Cloud_Code'";
            return db.GetString(sqluser);
        }

        /// <summary>
        /// 获取云组织推送账号密码
        /// </summary>
        /// <returns></returns>
        public string getCloud_Password()
        {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Cloud_Password'";
            return db.GetString(sqluser);
        }
    }
}