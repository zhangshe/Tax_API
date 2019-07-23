using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UIDP.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UIDP.UTILITY
{
    public class AccessTokenTool
    {
        /// <summary>
        /// 根据员工编号获取oken
        /// </summary>
        /// <param name="userId">系统内部的员工编号</param>
        /// <returns>token</returns>
        public static string GetAccessToken(string userId) {
            string accessToken = string.Empty;
            //判断参数是否合法
            if (string.IsNullOrEmpty(userId))
            {
                return accessToken;
            }
            string key = GetDesEncryptKey();//获取加密的key
            if (string.IsNullOrEmpty(key))
                return accessToken;
            //如果要修改此算法，一定要把GetUserId（）里的算法也改掉
            accessToken = userId + string.Format("{0:HHmmss}", DateTime.Now) + GetRandomString(10);//加密前的字符串 userid+6位时分秒+10位随机数
            accessToken = SecurityHelper.DesEncrypt(accessToken, key);//加密后
            return accessToken;
        }
        /// <summary>
        /// 获取加密的key
        /// </summary>
        /// <returns></returns>
        public static string GetDesEncryptKey()
        {
            using (System.IO.StreamReader file = System.IO.File.OpenText(System.IO.Directory.GetCurrentDirectory() + "\\Security.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    string key = o["DesEncryptKey"].ToString();
                    return key;
                }
            }
        }
        /// <summary>
        /// 产生制定位数的随机数
        /// </summary>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string GetRandomString(int iLength)
        {
            string buffer = "0123456789";// 随机字符中也可以为汉字（任何）  
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = buffer.Length;
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(buffer.Substring(r.Next(range), 1));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 掉借口时判断token是否过期，如果有token则更新时间 过期则删除
        /// </summary>
        /// <param name="staffId">系统内部用户id</param>
        /// <returns>2000：合法；50008:非法的token; 50012:其他客户端登录了;  50014:Token 过期了;</returns>
        public static Message IsInValidUser(string userId, string token,string admin) {
            Message mes = new Message();
            DataTable dt = GetToken(userId,token,admin);
            if (dt == null || dt.Rows.Count == 0)
            {
                mes.code = 50008;
                mes.message = "非法的token";
                return mes;
            }
            else {
                if (DateTime.Now <= Convert.ToDateTime(dt.Rows[0]["EXPIRED_TIME"].ToString())) {
                    UpdateToken(userId, DateTime.Now.AddHours(1));
                    mes.code = 2000;
                    mes.message = "合法的token";
                    return mes;
                }
                else
                {
                    DeleteToken(userId);//过期的话删掉token
                    mes.code = 50014;
                    mes.message = "Token 过期了";
                    return mes;
                }
            }
        }
        /// <summary>
        /// 通过token解析出userid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetUserId(string token) {
            string userId =string.Empty;
            string key = GetDesEncryptKey();//获取加密的key
            userId = SecurityHelper.DesDecrypt(token,key).Trim().Replace("\0", "");
            if (userId.Length>16) {
                userId = userId.Substring(0, userId.Length - 16);
            }
            return userId;
        }
        /// <summary>
        /// 根据userid和token去查表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static DataTable GetToken(string userId,string token,string admin) {
            DBTool tool = new DBTool("MYSQL");
            string sql = "";
            if (admin == "")
            {
                 sql = "select a.* from ts_uidp_accesstoken a join ts_uidp_userinfo b on a.USER_ID=b.USER_ID where a.USER_ID='" + userId + "' and ACCESS_TOKEN='" + token + "' ";
            }
            else {
                 sql = "select a.* from ts_uidp_accesstoken a  where a.USER_ID='" + userId + "' and a.ACCESS_TOKEN='" + token + "' ";
            }
            return tool.GetDataTable(sql);
        }
        /// <summary>
        /// 根据userid删除token
        /// </summary>
        public static void DeleteToken(string userId) {
            DBTool tool = new DBTool("MYSQL");
            string sql = "delete from ts_uidp_accesstoken where USER_ID='" + userId + "' ;";
            tool.Execut(sql);
        }
        /// <summary>
        /// 插入token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="datetime"></param>
        public static void InsertToken(string userId,string token,DateTime datetime)
        {
            DBTool tool = new DBTool("MYSQL");
            string sql = "insert into ts_uidp_accesstoken (USER_ID,ACCESS_TOKEN,EXPIRED_TIME) VALUES('"+userId+"','"+token+ "','"+datetime.ToString("yyyy-MM-dd HH:mm:ss") +"'); ";
            tool.Execut(sql);
        }
        /// <summary>
        /// 更新token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="datetime"></param>
        public static void UpdateToken(string userId, DateTime datetime)
        {
            DBTool tool = new DBTool("MYSQL");
            string sql = "update ts_uidp_accesstoken set EXPIRED_TIME='"+datetime.ToString("yyyy-MM-dd HH:mm:ss") + "' where USER_ID='" + userId+"'";
            tool.Execut(sql);
        }
    }
    /*
       USER_ID              varchar(30),
   ACCESS_TOKEN            varchar(100),
   EXPIRED_TIME          datetime comment '格式：yyyy-mm-dd:hh:mm:ss'
         
         */
    public class Message {
        /// <summary>
        /// 2000：合法；50008:非法的token; 50012:其他客户端登录了;  50014:Token 过期了;
        /// </summary>
        public int code{get;set;}
        /// <summary>
        /// 状态信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string result { get; set; }
    }
}
