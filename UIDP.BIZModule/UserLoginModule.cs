using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
   public class UserLoginModule
    {
        UserLoginDB db = new UserLoginDB();
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserLoginList(string limit, string page, string LOGIN_REMARK, string sort)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit1 = limit == null ? 100 : int.Parse(limit);
                int page1 = page == null ? 1 : int.Parse(page);

                DataTable dt = db.fetchUserLoginList(LOGIN_REMARK,  sort);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtName = dt.DefaultView.ToTable(true, "LOGIN_ID", "LOGIN_CODE", "LOGIN_PASS", "LOGIN_REMARK");
                    dtName.Columns.Add("USER_NAME");
                    foreach (DataRow row in dtName.Rows)
                    {
                        string fengefu = "";
                        foreach (DataRow item in dt.Rows)
                        {
                            if (row["LOGIN_ID"].ToString() == item["LOGIN_ID"].ToString() && item["USER_NAME"] != null && item["USER_NAME"].ToString() != "")
                            {
                                if (!row["USER_NAME"].ToString().Contains(item["USER_NAME"].ToString()))
                                {
                                    row["USER_NAME"] += fengefu + item["USER_NAME"].ToString();
                                    fengefu = ",";
                                }
                            }
                        }
                    }
                    r["total"] = dtName.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dtName, page1, limit1));
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

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserForLoginList(string limit, string page, string USER_ID)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit1 = limit == null ? 100 : int.Parse(limit);
                int page1 = page == null ? 1 : int.Parse(page);

                DataTable dt = db.fetchUserForLoginList(USER_ID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page1, limit1));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = 0;
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
        
        public Dictionary<string, object> fetchUserForAllList(string limit, string page, string USER_ID)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit1 = limit == null ? 100 : int.Parse(limit);
                int page1 = page == null ? 1 : int.Parse(page);

                DataTable dt = db.fetchUserForAllList(USER_ID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page1, limit1));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = 0;
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
        public string createUserLoginArticle(Dictionary<string, object> d)
        {
            if (d["LOGIN_CODE"] ==null|| d["LOGIN_CODE"].ToString()=="") {
                return "登录账号不能为空！";
            }
            DataTable dt = db.GetUserLoginByLOGIN_CODE(d["LOGIN_CODE"].ToString());
            if (dt!=null&&dt.Rows.Count>0) {
                return "此账号已经存在,不能重复添加！";
            }
            d["LOGIN_ID"] = CreateId(28);
            return db.createUserLoginArticle(d);
        }

        /// <summary>
        /// 系统自动生成orgId
        /// </summary>
        /// <returns></returns>
        public string CreateId(int CreateIdcount)
        {
            string Id = string.Empty;
            DataTable dt = new DataTable();
            Id = GenerateCheckCode(CreateIdcount);
            dt = db.GetUserLoginById(Id);
            while (dt != null && dt.Rows.Count > 0)
            {
                Id = GenerateCheckCode(CreateIdcount);
                dt = db.GetUserLoginById(Id);
            }
            return Id;
        }
        public DataTable GetUserLoginByLOGIN_CODE(string logCode) {
            return db.GetUserLoginByLOGIN_CODE(logCode);
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
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserLoginData(Dictionary<string, object> d)
        {
            return db.updateUserLoginData(d);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserLoginArticle(Dictionary<string, object> d)
        {
            if (d["LOGIN_ID"] == null)
            {
                return "无组登录用户LOGIN_ID";
            }
            return db.updateUserLoginArticle(d);
        }
        /// <summary>
        /// D.	分配组
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserForLoginArticle(Dictionary<string, object> d)
        {
            return db.updateUserForLoginArticle(d);
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteUserForLoginArticle(Dictionary<string, object> d)
        {
            return db.deleteUserForLoginArticle(d);
        }

        /// <summary>
        /// 通过LOGIN_ID查用户信息
        /// </summary>
        /// <param name="LOGIN_ID"></param>
        /// <returns></returns>
        public DataTable fetchUserLoginListById(string LOGIN_ID)
        {
            return db.fetchUserLoginListById(LOGIN_ID);
        }
        
        public DataTable getUserInfoByName(string userCode)
        {
            return db.getUserInfoByName(userCode);
        }

        public DataTable getLoginByID(string userId)
        {
            return db.getLoginByID(userId);
        }

    }
}
