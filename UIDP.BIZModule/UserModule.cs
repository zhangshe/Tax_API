using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class UserModule
    {
        UserDB db = new UserDB();
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchUserList(d);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtName = dt.DefaultView.ToTable(true, "USER_ID", "REG_TIME", "USER_NAME", "USER_CODE",  "USER_PASS", "PHONE_MOBILE", "PHONE_OFFICE",  "USER_EMAIL", "USER_IP", "FLAG", "USER_DOMAIN", "REMARK", "USER_SEX", "USER_ERP");
                    dtName.Columns.Add("ORG_ID");
                    dtName.Columns.Add("ORG_NAME");
                    foreach (DataRow row in dtName.Rows)
                    {
                        string fengefu = "";
                        foreach (DataRow item in dt.Rows)
                        {
                            if (row["USER_ID"].ToString() == item["USER_ID"].ToString() && item["ORG_ID"] != null && item["ORG_ID"].ToString() != "")
                            {
                                if (!row["ORG_ID"].ToString().Contains(item["ORG_ID"].ToString()))
                                {
                                    row["ORG_ID"] += fengefu + item["ORG_ID"].ToString();
                                    row["ORG_NAME"] += fengefu + item["ORG_NAME"].ToString();
                                    fengefu = ",";
                                }
                            }
                        }
                    }
                    r["total"] = dtName.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dtName, page, limit));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = new Dictionary<string, object>();
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = new Dictionary<string, object>();
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public string createUserArticle(Dictionary<string, object> d)
        {
            if (d["USER_CODE"] != null && d["USER_CODE"].ToString().Length > 0)
            {
                DataTable dt = db.GetUserInfoByUserCode(d["USER_CODE"].ToString(), "");//USER_DOMAIN
                if (dt != null && dt.Rows.Count > 0)
                {
                    return "此员工编号已存在！";
                }
            }
            if (d["USER_DOMAIN"] != null)
            {
                DataTable dt = db.GetUserInfoByUSER_DOMAIN(d["USER_DOMAIN"].ToString(), "");//USER_DOMAIN
                if (dt != null && dt.Rows.Count > 0)
                {
                    return "此账号已存在！";
                }
            }
            if (d["USER_PASS"]!=null&& d["USER_PASS"].ToString()!="") {
                d["USER_PASS"] = UIDP.Security.SecurityHelper.StringToMD5Hash(d["USER_PASS"].ToString());
            }
            
            return db.createUserArticle(d);
        }
        public string updateUserArticle(Dictionary<string, object> d)
        {
            return db.updateUserArticle(d);
        }
        public string updateUserFlag(Dictionary<string, object> d)
        {
            return db.updateUserFlag(d);
        }
        public string updatePasswordData(Dictionary<string, object> d)
        {
            if (d.Keys.Contains("roleLevel") && d["roleLevel"] != null && d["roleLevel"].ToString() == "admin")
            {
                string userId = getAdminCode();
                string pass = getAdminPass();
                if (d["userid"].ToString() != userId || d["password"].ToString() != pass)
                {
                    return "用户名或密码不正确！";
                }
                return db.updateAdminPasswordData(d);
            }
            //if (d.Keys.Contains("roleLevel") && d["roleLevel"].ToString() == "admin")
            //{
            //    string userId = getAdminCode();
            //    string pass = getAdminPass();
            //    if (d["userid"].ToString() != userId || d["password"].ToString() != pass)
            //    {
            //        return "用户名或密码不正确！";
            //    }
            //    return db.updateAdminPasswordData(d);
            //}
            else
            {
                d["password"] = Security.SecurityHelper.StringToMD5Hash(d["password"].ToString());
                d["newpassword"] = Security.SecurityHelper.StringToMD5Hash(d["newpassword"].ToString());
                DataTable dt = db.IsInvalidPassword(d);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return "用户名或密码不正确！";
                }
                return db.updatePasswordData(d);
            }
        }
        public string updatePTRpass(Dictionary<string, object> d) {
            d["newpassword"] = Security.SecurityHelper.StringToMD5Hash(d["newpassword"].ToString());
            return db.updatePTRpass(d);
        }
        public string updateUserData(Dictionary<string, object> d)
        {
            string newpass = "";
            if (d["USER_PASS"] != null && d["USER_PASS"].ToString() != "")
            {
                newpass = UIDP.Security.SecurityHelper.StringToMD5Hash(d["USER_PASS"].ToString());
            }
            //if (d["USER_CODE"] != null)
            //{
            //    DataTable dt = db.GetUserInfoByUserCode(d["USER_CODE"].ToString(), d["USER_ID"].ToString());//USER_DOMAIN
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        return "此员工账号已存在！";
            //    }
            //}
            //if (d["USER_DOMAIN"] != null)
            //{
            //    DataTable dt = db.GetUserInfoByUSER_DOMAIN(d["USER_DOMAIN"].ToString(), d["USER_ID"].ToString());//USER_DOMAIN
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        return "此员工编号已存在！";
            //    }
            //}
            return db.updateUserData(d,newpass);
        }
        public UIDP.BIZModule.Models.ts_uidp_userinfo getUserInfoByUserId(string userId)
        {
            DataTable dt = db.GetUserInfoByUserId(userId);
            UIDP.BIZModule.Models.ts_uidp_userinfo mod = new Models.ts_uidp_userinfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                mod = DataRowToModel(dt.Rows[0]);
            }
            return mod;
        }
        public UIDP.BIZModule.Models.ts_uidp_userinfo getUserInfoByLogin(string username, string userDomain)
        {
            DataTable dt = db.GetUserInfoBylogin(username, userDomain);
            UIDP.BIZModule.Models.ts_uidp_userinfo mod = new Models.ts_uidp_userinfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                mod = DataRowToModel(dt.Rows[0]);
            }
            return mod;
        }
        public UIDP.BIZModule.Models.ts_uidp_userinfo getUserInfoByToken(string token)
        {
            string userid = AccessTokenTool.GetUserId(token);
            return getUserInfoByUserId(userid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public DataTable getUserAndGroupgByToken(string token)
        {
            string userid = AccessTokenTool.GetUserId(token);
            return db.GetUserAndGroup(userid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public DataTable getUserAndGroupgByUserId(string userid)
        {
            return db.GetUserAndGroup(userid);
        }
        /// <summary>
        /// 根据userid 获取用户组织机构信息列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetUserOrg(string userId)
        {
            return db.GetUserOrg(userId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UIDP.BIZModule.Models.ts_uidp_userinfo DataRowToModel(DataRow row)
        {
            UIDP.BIZModule.Models.ts_uidp_userinfo model = new UIDP.BIZModule.Models.ts_uidp_userinfo();
            if (row != null)
            {
                if (row["USER_ID"] != null)
                {
                    model.USER_ID = row["USER_ID"].ToString();
                }
                if (row["USER_CODE"] != null)
                {
                    model.USER_CODE = row["USER_CODE"].ToString();
                }
                if (row["USER_NAME"] != null)
                {
                    model.USER_NAME = row["USER_NAME"].ToString();
                }
                //if (row["USER_ALIAS"] != null)
                //{
                //    model.USER_ALIAS = row["USER_ALIAS"].ToString();
                //}
                if (row["USER_PASS"] != null)
                {
                    model.USER_PASS = row["USER_PASS"].ToString();
                }
                if (row["PHONE_MOBILE"] != null)
                {
                    model.PHONE_MOBILE = row["PHONE_MOBILE"].ToString();
                }
                if (row["PHONE_OFFICE"] != null)
                {
                    model.PHONE_OFFICE = row["PHONE_OFFICE"].ToString();
                }
                //if (row["PHONE_ORG"] != null)
                //{
                //    model.PHONE_ORG = row["PHONE_ORG"].ToString();
                //}
                if (row["USER_EMAIL"] != null)
                {
                    model.USER_EMAIL = row["USER_EMAIL"].ToString();
                }
                //if (row["EMAIL_OFFICE"] != null)
                //{
                //    model.EMAIL_OFFICE = row["EMAIL_OFFICE"].ToString();
                //}
                if (row["USER_IP"] != null)
                {
                    model.USER_IP = row["USER_IP"].ToString();
                }
                if (row["REG_TIME"] != null && row["REG_TIME"].ToString() != "")
                {
                    model.REG_TIME = DateTime.Parse(row["REG_TIME"].ToString());
                }
                if (row["FLAG"] != null && row["FLAG"].ToString() != "")
                {
                    model.FLAG = int.Parse(row["FLAG"].ToString());
                }
                if (row["USER_DOMAIN"] != null)
                {
                    model.USER_DOMAIN = row["USER_DOMAIN"].ToString();
                }
                if (row["REMARK"] != null)
                {
                    model.REMARK = row["REMARK"].ToString();
                }
            }
            return model;
        }
        /// <summary>
        /// 系统自动生成userid
        /// </summary>
        /// <returns></returns>
        public string CreateUserId(int userIdCount)
        {
            string userId = string.Empty;
            DataTable dt = new DataTable();
            userId = GenerateCheckCode(userIdCount);
            dt = db.GetUserInfoByUserId(userId);
            while (dt != null && dt.Rows.Count > 0)
            {
                userId = GenerateCheckCode(userIdCount);
                dt = db.GetUserInfoByUserId(userId);
            }
            return userId;
        }
        /// <summary>
        /// 
        /// </summary>
        private int rep = 0;
        /// 
        /// 生成随机字母字符串(数字字母混和)
        /// 
        /// 待生成的位数
        /// 生成的字母字符串
        private string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 查询用户信息(包含组织结构)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserOrgList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchUserOrgList(d);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //DataTable dtName = dt.DefaultView.ToTable(true, "USER_ID", "USER_DOMAIN", "USER_NAME", "USER_CODE", "USER_PASS", "PHONE_MOBILE", "PHONE_OFFICE",
                    //    "USER_EMAIL", "USER_IP", "USER_SEX", "FLAG", "AUTHENTICATION_TYPE", "ASSOCIATED_ACCOUNT", "REMARK");
                    //dtName.Columns.Add("orgId");
                    //dtName.Columns.Add("orgName");
                    //foreach (DataRow row in dtName.Rows)
                    //{
                    //    string fengefu = "";
                    //    foreach (DataRow item in dt.Rows)
                    //    {
                    //        if (row["USER_ID"].ToString() == item["USER_ID"].ToString() && item["orgId"] != null && item["orgId"].ToString() != "")
                    //        {
                    //            if (!row["orgId"].ToString().Contains(item["orgId"].ToString()))
                    //            {
                    //                row["orgId"] += fengefu + item["orgId"].ToString();
                    //                row["orgName"] += fengefu + item["orgName"].ToString();
                    //                fengefu = ",";
                    //            }
                    //        }
                    //    }
                    //}
                    //r["total"] = dtName.Rows.Count;
                    //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dtName, page, limit));
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = 0;
                    r["items"] = null;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
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
        /// 查询用户信息（包含角色信息）
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserRoleList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchUserRoleList(d);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // "USER_ID", "REG_TIME", "USER_NAME", "USER_CODE", "USER_ALIAS", "USER_PASS", "PHONE_MOBILE", "PHONE_OFFICE", "PHONE_ORG", "USER_EMAIL", "EMAIL_OFFICE", "USER_IP", "FLAG", "USER_DOMAIN", "REMARK"
                    //"USER_ID", "USER_NAME", "USER_CODE", "PHONE_MOBILE","PHONE_ORG",  "FLAG", "USER_DOMAIN", "REMARK"
                    DataTable dtName = dt.DefaultView.ToTable(true, "USER_ID");
                    DataTable dtrole = KVTool.GetPagedTable(dtName, page, limit);
                    dtrole.Columns.Add("USER_NAME");
                    dtrole.Columns.Add("USER_CODE");
                    dtrole.Columns.Add("PHONE_MOBILE");
                    dtrole.Columns.Add("FLAG");
                    dtrole.Columns.Add("USER_DOMAIN");
                    dtrole.Columns.Add("REMARK");
                    dtrole.Columns.Add("roleId");
                    dtrole.Columns.Add("groupName");
                    foreach (DataRow row in dtrole.Rows)
                    {
                        DataRow[] arr = dt.Select("USER_ID='"+row["USER_ID"].ToString() +"'");
                        if (arr.Length>0) {
                            row["USER_NAME"] = arr[0]["USER_NAME"] == null ? "" : arr[0]["USER_NAME"].ToString();
                            row["USER_CODE"] = arr[0]["USER_CODE"] == null ? "" : arr[0]["USER_CODE"].ToString();
                            row["PHONE_MOBILE"] = arr[0]["PHONE_MOBILE"] == null ? "" : arr[0]["PHONE_MOBILE"].ToString();
                            row["USER_DOMAIN"] = arr[0]["USER_DOMAIN"] == null ? "" : arr[0]["USER_DOMAIN"].ToString();
                            row["FLAG"] = arr[0]["FLAG"] == null ? "" : arr[0]["FLAG"].ToString();
                            row["REMARK"] = arr[0]["REMARK"] == null ? "" : arr[0]["REMARK"].ToString();
                            string fengefu = "";
                            foreach (var item in arr)
                            {
                                if (item["roleId"] != null && item["roleId"].ToString() != "")
                                {
                                    if (!row["roleId"].ToString().Contains(item["roleId"].ToString()))
                                    {
                                        row["roleId"] += fengefu + item["roleId"].ToString();
                                        row["groupName"] += fengefu + item["groupName"].ToString();
                                        fengefu = ",";
                                    }
                                }
                            }
                        }
                        
                    }
                    r["total"] = dtName.Rows.Count;
                    r["items"] = KVTool.TableToListDic(dtrole);
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = 0;
                    r["items"] = null;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
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
        /// 获取管理员账号
        /// </summary>
        /// <returns></returns>
        public string getAdminCode()
        {
            return db.getAdminCode();
        }/// <summary>
         /// 获取管理员密码
         /// </summary>
         /// <returns></returns>
        public string getAdminPass()
        {
            return db.getAdminPass();
        }
        /// <summary>
        /// 获取sysname
        /// </summary>
        /// <returns></returns>
        public string getSysName()
        {
            return db.getSysName();
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable GetUserAndOrgByUserId(string USER_ID)
        {
            return db.GetUserAndOrgByUserId(USER_ID);
        }
        public DataTable getSysTime()
        {
            return db.getSysTime();
        }

        public string UploadUserFileNew(string filePath)
        {
            //string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\用户.xlsx";//原始文件
            //string path = filePath;//原始文件
            //string mes = "";
            //
            //DataTable dt = new DataTable();
            //UTILITY.ExcelTools tool = new UTILITY.ExcelTools();
            //tool.GetDataTable(System.IO.File.OpenRead(path), path, modePath, ref mes, ref dt);

            List<string> list = new List<string>();
            string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\用户.xls";//原始文件
            string path = filePath;//原始文件
            string mes = "";
            DataTable dt = new DataTable();
            DataTable userdt = db.fetchUserList();
            UTILITY.ExcelTools tool = new UTILITY.ExcelTools();
            tool.GetDataTable(System.IO.File.OpenRead(path), path, modePath, ref mes, ref dt);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "空数据，导入失败！";
            }
            string error = GetDistinctSelf(dt, "账号");
            //if (error != null && error.Length > 0)
            //{
            //    return error;
            //}
            int truckNum = Convert.ToInt32(Convert.ToDecimal(dt.Rows.Count / 500));
            int yushu = dt.Rows.Count% 500;
            if (yushu > 0)
            {
                truckNum++;
            }
            for (int j = 1; j < truckNum + 1; j++)
            {
                string fengefu = "";
                StringBuilder sb = new StringBuilder();
                StringBuilder sbOrgUser = new StringBuilder();
                //sbOrgUser.Append("insert into ts_uidp_org_user(ORG_ID,USER_ID)values ");
                //sb.Append(" INSERT INTO ts_uidp_userinfo(USER_ID,USER_DOMAIN,USER_CODE,USER_NAME,USER_PASS,PHONE_MOBILE,PHONE_OFFICE," +
                //    "USER_EMAIL,USER_IP,USER_SEX,AUTHENTICATION_TYPE,FLAG,REG_TIME,REMARK) values ");
                OrgDB orgDB = new OrgDB();
                DataTable dtOrg = orgDB.fetchOrgList();
                string result = "";
                string fengefu2 = "";
                int rowbegin = (j - 1) * 500;
                int rowend = j * 500;
                if (rowend > dt.Rows.Count) { rowend = dt.Rows.Count; }
                for (int i = rowbegin; i < rowend; i++)
                {
                    var usercode = getString(dt.Rows[i]["账号"]);
                    DataRow[] rows = userdt.Select("USER_DOMAIN='" + usercode + "'");
                    if (dt.Rows[i]["组织机构编码"] == null || dt.Rows[i]["账号"] == null)
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，组织机构编码或者账号不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (dt.Rows[i]["组织机构编码"].ToString() == "" || dt.Rows[i]["账号"].ToString() == "")
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，组织机构编码或者账号不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (dt.Rows[i]["账号类型"]==null || dt.Rows[i]["账号类型"].ToString() == "")
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，账号类型不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (dt.Rows[i]["用户类型"]==null|| dt.Rows[i]["用户类型"].ToString() == "")
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，用户类型不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (dt.Rows[i]["员工姓名"]==null || dt.Rows[i]["员工姓名"].ToString() == "")
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，员工姓名不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (dt.Rows[i]["性别"]==null|| dt.Rows[i]["性别"].ToString() == "")
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，性别不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    DataRow[] OrgRow = dtOrg.Select("ORG_CODE='" + dt.Rows[i]["组织机构编码"].ToString().Trim() + "'");
                    if (OrgRow.Length <= 0)
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，系统中不存在此组织机构编码！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (rows.Length == 0)
                    {
                        string id = Guid.NewGuid().ToString();
                        sbOrgUser.Append(fengefu + "('" + dt.Rows[i]["组织机构编码"].ToString().Trim() + "','" + id + "')");
                        sb.Append(fengefu + "('" + id + "',");
                        if (dt.Rows[i]["账号类型"] != null && dt.Rows[i]["账号类型"].ToString() == "PTR账号")
                        {
                            sb.Append("1,");
                        }
                        else
                        {
                            sb.Append("0,");
                        }
                        sb.Append("'" + getString(dt.Rows[i]["账号"]) + "',");
                        if (dt.Rows[i]["用户类型"] != null && dt.Rows[i]["用户类型"].ToString() == "普通用户")
                        {
                            sb.Append("1,");
                        }
                        else
                        {
                            sb.Append("0,");
                        }
                        sb.Append("'");
                            sb.Append(getString(dt.Rows[i]["用户密码"]) == "" ? UIDP.Security.SecurityHelper.StringToMD5Hash("123456") : UIDP.Security.SecurityHelper.StringToMD5Hash(getString(dt.Rows[i]["用户密码"])));
                        sb.Append( "',");
                        sb.Append("'" + getString(dt.Rows[i]["员工姓名"]) + "',");
                        sb.Append("'" + getString(dt.Rows[i]["员工编号"]) + "',");
                        if (dt.Rows[i]["性别"] != null && dt.Rows[i]["性别"].ToString() == "男")
                        {
                            sb.Append("1,");
                        }
                        else
                        {
                            sb.Append("0,");
                        }
                        sb.Append("'" + getString(dt.Rows[i]["办公电话"]) + "',");
                        sb.Append("'" + getString(dt.Rows[i]["手机"]) + "',");
                        sb.Append("'" + getString(dt.Rows[i]["电子邮箱"]) + "',");
                      
                        if (dt.Rows[i]["账号状态"] != null && dt.Rows[i]["账号状态"].ToString() == "禁用")
                        {
                            sb.Append("0,");
                        }
                        else
                        {
                            sb.Append("1,");
                        }
                        sb.Append("'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        sb.Append("'" + getString(dt.Rows[i]["备注"]) + "')");
                        fengefu = ",";
                    }
                    else {
                        foreach (var item in rows)
                        {
                            string sql = "update  ts_uidp_userinfo set ";
                            sql += " AUTHENTICATION_TYPE='";
                            sql += getString((dt.Rows[i]["账号类型"] != null && dt.Rows[i]["账号类型"].ToString() == "PTR账号") ? 1 : 0) + "',";
                            sql += " USER_DOMAIN='" + getString(dt.Rows[i]["账号"]) + "',";
                            sql += " USER_TYPE='";
                            sql+= getString((dt.Rows[i]["用户类型"] != null && dt.Rows[i]["用户类型"].ToString() == "普通用户") ? 1 : 0) + "',";
                            sql += " USER_PASS='";
                            sql+= getString(dt.Rows[i]["用户密码"]) == "" ? UIDP.Security.SecurityHelper.StringToMD5Hash("123456") + "'," : UIDP.Security.SecurityHelper.StringToMD5Hash(getString(dt.Rows[i]["用户密码"])) + "',";
                            sql += " USER_NAME='" + getString(dt.Rows[i]["员工姓名"]) + "',";
                            sql += " USER_CODE='" + getString(dt.Rows[i]["员工编号"]) + "',";
                            sql += " USER_SEX='";
                            sql+= getString((dt.Rows[i]["性别"] != null && dt.Rows[i]["性别"].ToString() == "男") ? 1 : 0) + "',";
                            
                            sql += " PHONE_MOBILE='" + getString(dt.Rows[i]["手机"]) + "',";
                            sql += " PHONE_OFFICE='" + getString(dt.Rows[i]["办公电话"]) + "',";
                            sql += " USER_EMAIL='" + getString(dt.Rows[i]["电子邮箱"]) + "',";
                            sql += " FLAG='";
                            sql+= getString((dt.Rows[i]["账号状态"] != null && dt.Rows[i]["账号状态"].ToString() == "禁用") ? 0: 1) + "',";
                            sql += " REG_TIME='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                            sql += " REMARK='" + getString(dt.Rows[i]["备注"]) + "'";
                            sql += " where USER_ID='" + item["USER_ID"].ToString() + "' ;";
                            list.Add(sql);
                            string sql2 = "update ts_uidp_org_user set ORG_ID='" + dt.Rows[i]["组织机构编码"].ToString().Trim() + "' where USER_ID='" + item["USER_ID"].ToString() + "' ;";
                            list.Add(sql2);
                        }

                    }
                    //if (sbOrgUser != null && sbOrgUser.Length > 0)
                    //{
                    //    list.Add(sbOrgUser.ToString());
                    //}
                    //if (sb != null && sb.Length > 0)
                    //{
                    //    list.Add(sb.ToString());
                    //}
                }
                if (sb.Length > 0)
                {
                    sb.Insert(0, " INSERT INTO ts_uidp_userinfo(USER_ID,AUTHENTICATION_TYPE,USER_DOMAIN,USER_TYPE,USER_PASS,USER_NAME,USER_CODE,USER_SEX,PHONE_OFFICE,PHONE_MOBILE," +
                    "USER_EMAIL,FLAG,REG_TIME,REMARK) values ");
                }
                if (sbOrgUser.Length > 0)
                {
                    sbOrgUser.Insert(0, " insert into ts_uidp_org_user(ORG_ID,USER_ID)values ");
                }
                if (sb != null && sb.Length > 0)
                {
                    list.Add(sb.ToString());
                }
                if (sbOrgUser != null && sbOrgUser.Length > 0)
                {
                    list.Add(sbOrgUser.ToString());
                }
            }

            if (db.GetDBType() == "MYSQL")
            {
                string sqlUpdate = "   update ts_uidp_org_user a ,ts_uidp_org b set a.ORG_ID = b.ORG_ID where a.ORG_ID = b.ORG_CODE";

                list.Add(sqlUpdate);
            }
            else if (db.GetDBType() == "SQLSERVER")
            {
                string sqlUpdate = "   update a  set a.ORG_ID=b.ORG_ID from ts_uidp_org_user a ,ts_uidp_org b where  a.ORG_ID=b.ORG_CODE ";

                list.Add(sqlUpdate);
            }
            else if (db.GetDBType() == "ORACLE")
            {
                string sqlUpdate = "  update ts_uidp_org_user a ,ts_uidp_org b set a.ORG_ID = b.ORG_ID where a.ORG_ID = b.ORG_CODE";

                list.Add(sqlUpdate);
            }

            return db.UploadUserFile(list);
            //DataView dv = new DataView(dt);
            //if (dt.Rows.Count != dv.ToTable(true, "账号").Rows.Count)
            //{
            //    return "账号列存在重复数据，导入失败！";
            //}
            //List<string> list = new List<string>();
            //string fengefu = "";
            //StringBuilder sb = new StringBuilder();
            //StringBuilder sbOrgUser = new StringBuilder();

            //string result = "";
            //string fengefu2 = "";
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    var usercode = getString(dt.Rows[i]["账号"]);
            //    var dtt = userdt;
            //    DataRow[] rows = userdt.Select("USER_DOMAIN='" + usercode + "'");
            //    if (rows.Length == 0)
            //    {
            //        //sb.Append(" insert into ts_uidp_org (ORG_ID,ORG_CODE,ORG_NAME,ORG_SHORT_NAME,ORG_CODE_UPPER,ISINVALID,ISDELETE,REMARK) values ");

            //        string id = Guid.NewGuid().ToString();
            //        sbOrgUser.Append(fengefu + "('" + dt.Rows[i]["组织机构编码"].ToString().Trim() + "','" + id + "')");
            //        sb.Append(fengefu + "('" + id + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["账号"]) + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["员工编号"]) + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["姓名"]) + "',");
            //        sb.Append("'123456',");
            //        sb.Append("'" + getString(dt.Rows[i]["手机"]) + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["办公电话"]) + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["邮箱"]) + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["访问IP"]) + "',");
            //        if (dt.Rows[i]["性别"] != null && dt.Rows[i]["性别"].ToString() == "男")
            //        {
            //            sb.Append("1,");
            //        }
            //        else
            //        {
            //            sb.Append("0,");
            //        }
            //        if (dt.Rows[i]["账号类型"] != null && dt.Rows[i]["账号类型"].ToString() == "PTR账号")
            //        {
            //            sb.Append("'1',");
            //        }
            //        else
            //        {
            //            sb.Append("'0',");
            //        }
            //        sb.Append("1,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
            //        sb.Append("'" + getString(dt.Rows[i]["备注"]) + "')");
            //        fengefu = ",";
            //    }
            //    else
            //    {
            //        foreach (var item in rows)
            //        {
            //            string sql = "update  ts_uidp_org set ";
            //            sql += " USER_DOMAIN='" + getString(dt.Rows[i]["组织机构编码"]) + "',";
            //            sql += " USER_CODE='" + getString(dt.Rows[i]["组织机构编码"]) + "',";
            //            sql += " USER_NAME='" + getString(dt.Rows[i]["组织机构编码"]) + "',";
            //            sql += " USER_PASS='" + getString(dt.Rows[i]["组织机构编码"]) + "',";
            //            sql += " PHONE_MOBILE='" + getString(dt.Rows[i]["组织机构编码"]) + "',";
            //            sql += " PHONE_OFFICE='" + getString(dt.Rows[i]["组织机构编码"]) + "',";
            //            sql += " USER_EMAIL='" + getString(dt.Rows[i]["组织机构名称"]) + "',";
            //            sql += " USER_IP='" + getString(dt.Rows[i]["组织机构简称"]) + "',";
            //            sql += " USER_SEX='" + getString(dt.Rows[i]["上级组织机构编码"]) + "',";
            //            sql += " AUTHENTICATION_TYPE='" + getString((row["是否有效"] != null && row["是否有效"].ToString() == "是") ? 1 : 0) + "',";
            //            sql += " FLAG='" + getString(row["上级组织机构编码"]) + "',";
            //            sql += " REG_TIME='" + getString(row["上级组织机构编码"]) + "',";
            //            sql += " REMARK='" + getString(row["备注"]) + "'";
            //            sql += " where USER_ID='" + item["USER_ID"].ToString() + "' ;";
            //            list.Add(sql);
            //        }
            //    }
            //    //sqllst.Add(sb.ToString());
            //}
            //if (sb.Length > 0)
            //{
            //    sb.Insert(0, " INSERT INTO ts_uidp_userinfo(USER_ID,USER_DOMAIN,USER_CODE,USER_NAME,USER_PASS,PHONE_MOBILE,PHONE_OFFICE," +
            //    "USER_EMAIL,USER_IP,USER_SEX,AUTHENTICATION_TYPE,FLAG,REG_TIME,REMARK) values  ");
            //    sqllst.Add(sb.ToString());
            //}
            //if (sbOrgUser.Length > 0)
            //{
            //    sb.Insert(0, " insert into ts_uidp_org_user(ORG_ID,USER_ID)values ");
            //    sqllst.Add(sb.ToString());
            //}

            //string sqlUpdate = "update ts_uidp_org_user a ,ts_uidp_org b set a.ORG_ID = b.ORG_ID where a.ORG_ID = b.ORG_CODE";

            //list.Add(sbOrgUser.ToString());
            //list.Add(sb.ToString());
            //list.Add(sqlUpdate);
            //return db.UploadUserFile(list);
        }

        public string UploadUserFile(string filePath)
        {
            List<string> list = new List<string>();
            string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\用户.xlsx";//原始文件
            string path = filePath;//原始文件
            string mes = "";
            DataTable dt = new DataTable();
            UTILITY.ExcelTools tool = new UTILITY.ExcelTools();
            tool.GetDataTable(System.IO.File.OpenRead(path), path, modePath, ref mes, ref dt);

            if (dt == null || dt.Rows.Count == 0)
            {
                return "空数据，导入失败！";
            }
            //DataView dv = new DataView(dt);
            //String[] str = {  "组织机构编码", "组织机构名称", "账号", "姓名", "员工编号", "性别", "办公电话", "手机", "邮箱", "访问IP", "账号类型","备注" };
            //dt = dv.ToTable(true, str);

            //if (dt.Rows.Count != dv.ToTable(true, "账号").Rows.Count)
            //{
            //    return "账号列存在重复数据，导入失败！";
            //}
            string error = GetDistinctSelf(dt, "账号");
            if (error != null && error.Length > 0)
            {
                return error;
            }
            int truckNum = Convert.ToInt32(Convert.ToDecimal(dt.Rows.Count / 500));
            int yushu = dt.Rows.Count % 500;
            if (yushu > 0)
            {
                truckNum++;
            }

            for (int j = 1; j < truckNum + 1; j++)
            {
                string fengefu = "";
                StringBuilder sb = new StringBuilder();
                StringBuilder sbOrgUser = new StringBuilder();
                sbOrgUser.Append("insert into ts_uidp_org_user(ORG_ID,USER_ID)values ");
                sb.Append(" INSERT INTO ts_uidp_userinfo(USER_ID,USER_DOMAIN,USER_CODE,USER_NAME,USER_PASS,PHONE_MOBILE,PHONE_OFFICE," +
                    "USER_EMAIL,USER_IP,USER_SEX,AUTHENTICATION_TYPE,FLAG,REG_TIME,REMARK) values ");
                OrgDB orgDB = new OrgDB();
                DataTable dtOrg = orgDB.fetchOrgList();
                string result = "";
                string fengefu2 = "";
                int rowbegin = (j - 1) * 500;
                int rowend = j * 500;
                if (rowend > dt.Rows.Count) { rowend = dt.Rows.Count; }
                for (int i = rowbegin; i < rowend; i++)
                {
                    if (dt.Rows[i]["组织机构编码"] == null || dt.Rows[i]["账号"] == null)
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，组织机构编码或者账号不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    if (dt.Rows[i]["组织机构编码"].ToString() == "" || dt.Rows[i]["账号"].ToString() == "")
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，组织机构编码或者账号不能为空！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    DataRow[] OrgRow = dtOrg.Select("ORG_CODE='" + dt.Rows[i]["组织机构编码"].ToString().Trim() + "'");
                    if (OrgRow.Length <= 0)
                    {
                        result += fengefu2 + "第" + (i + 2) + "行，系统中不存在此组织机构编码！，导入失败！";
                        fengefu2 = ",";
                        continue;
                    }
                    string id = Guid.NewGuid().ToString();
                    sbOrgUser.Append(fengefu + "('" + dt.Rows[i]["组织机构编码"].ToString().Trim() + "','" + id + "')");
                    sb.Append(fengefu + "('" + id + "',");
                    sb.Append("'" + getString(dt.Rows[i]["账号"]) + "',");
                    sb.Append("'" + getString(dt.Rows[i]["员工编号"]) + "',");
                    sb.Append("'" + getString(dt.Rows[i]["姓名"]) + "',");
                    sb.Append("'123456',");
                    sb.Append("'" + getString(dt.Rows[i]["手机"]) + "',");
                    sb.Append("'" + getString(dt.Rows[i]["办公电话"]) + "',");
                    sb.Append("'" + getString(dt.Rows[i]["邮箱"]) + "',");
                    sb.Append("'" + getString(dt.Rows[i]["访问IP"]) + "',");
                    if (dt.Rows[i]["性别"] != null && dt.Rows[i]["性别"].ToString() == "男")
                    {
                        sb.Append("1,");
                    }
                    else
                    {
                        sb.Append("0,");
                    }
                    if (dt.Rows[i]["账号类型"] != null && dt.Rows[i]["账号类型"].ToString() == "PTR账号")
                    {
                        sb.Append("'1',");
                    }
                    else
                    {
                        sb.Append("'0',");
                    }
                    sb.Append("1,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                    sb.Append("'" + getString(dt.Rows[i]["备注"]) + "')");
                    fengefu = ",";
                }
                if (sbOrgUser != null && sbOrgUser.Length > 0)
                {
                    list.Add(sbOrgUser.ToString());
                }
                if (sb != null && sb.Length > 0)
                {
                    list.Add(sb.ToString());
                }
            }

            //string sqlUpdate = "   update a  set a.ORG_ID=b.ORG_ID from ts_uidp_org_user a ,ts_uidp_org b where  a.ORG_ID=b.ORG_CODE ";
            //string sqlUpdate = "update ts_uidp_org_user a ,ts_uidp_org b set a.ORG_ID = b.ORG_ID where a.ORG_ID = b.ORG_CODE";
            if (db.GetDBType() == "MYSQL")
            {
                string sqlUpdate = "   update ts_uidp_org_user a ,ts_uidp_org b set a.ORG_ID = b.ORG_ID where a.ORG_ID = b.ORG_CODE";

                list.Add(sqlUpdate);
            }
            else if (db.GetDBType() == "SQLSERVER")
            {
                string sqlUpdate = "   update a  set a.ORG_ID=b.ORG_ID from ts_uidp_org_user a ,ts_uidp_org b where  a.ORG_ID=b.ORG_CODE ";

                list.Add(sqlUpdate);
            }
            else if (db.GetDBType() == "ORACLE")
            {
                string sqlUpdate = "  update ts_uidp_org_user a ,ts_uidp_org b set a.ORG_ID = b.ORG_ID where a.ORG_ID = b.ORG_CODE";

                list.Add(sqlUpdate);
            }

            return db.UploadUserFile(list);
        }

        public string GetDistinctSelf(DataTable SourceDt, string filedName)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = SourceDt.Rows.Count - 2; i > 0; i--)
            {
                DataRow[] rows = SourceDt.Select(string.Format("{0}='{1}'", filedName, SourceDt.Rows[i][filedName]));
                if (rows.Length > 1)
                {
                    //SourceDt.Rows.RemoveAt(i);
                    //SourceDt.Rows.re
                    sb.Append("【" + SourceDt.Rows[i][filedName] + "】,");
                }
            }
            StringCollection sc = new StringCollection();
            string[] arr = sb.ToString().TrimEnd(',').Split(',');
            foreach (string str in arr)
            {
                if (!sc.Contains(str))
                {
                    sc.Add(str);
                }
            }
            StringBuilder sb2 = new StringBuilder();
            foreach (string str in sc)
            {
                sb2.Append(str + ",");
            }

            sb2.Append("账号信息重复，请确认！");
            return sb2.ToString();

        }


        public string getString(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }

        /// <summary>
        /// 获取云组织推送账号
        /// </summary>
        /// <returns></returns>
        public string getCloudCode()
        {
            return db.getCloud_Code();
        }

        /// <summary>
        /// 获取云组织推送账号密码
        /// </summary>
        /// <returns></returns>
        public string getCloudPass()
        {
            return db.getCloud_Password();
        }
    }
}
