using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UIDP.LOG
{
    public class ClsSysLog
    {
        private static readonly string connStr;
        //private static MySqlConnection conn;
        
        /// <summary>
        /// 静态构造函数实例化连接字符串对象
        /// </summary>
        static ClsSysLog()
        {
            connStr=GetStrConn();
            //conn = new MySqlConnection(connStr);
            //conn.Open();
        }
        #region MyRegion


        //public static string GetStrConn() {
        //    try
        //    {
        //        StreamReader sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + "\\log.json", Encoding.Default);
        //        String line;
        //        string jsonobj = "";
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            jsonobj = jsonobj + line.ToString();
        //        }
        //        LogConn log = JsonConvert.DeserializeObject<LogConn>(jsonobj);
        //        return log.strConn;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}

        #endregion
        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetStrConn()
        {
            using (System.IO.StreamReader file = System.IO.File.OpenText(System.IO.Directory.GetCurrentDirectory() + "\\log.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    string key = o["MYSQL"].ToString();
                    return key;
                }
            }
        }
        ///// <summary>
        ///// 关闭连接
        ///// </summary>
        //public  void Close()
        //{
        //    if (conn.State != System.Data.ConnectionState.Closed)
        //    {
        //        conn.Close();
        //    }
        //}
        public void Info(DateTime ACCESS_TIME, string USER_ID, string USER_NAME, string IP_ADDR, int LOG_TYPE, string LOG_CONTENT, string REMARK,int? ALARM_LEVEL) {
            LogMod mod = new LogMod();
            mod.ACCESS_TIME = ACCESS_TIME;
            mod.USER_ID = USER_ID;
            mod.USER_NAME = USER_NAME;
            mod.IP_ADDR = IP_ADDR;
            mod.LOG_TYPE = LOG_TYPE;
            mod.LOG_CONTENT = "执行了" + LOG_CONTENT + "方法";
            mod.REMARK = REMARK;
            mod.ALARM_LEVEL = ALARM_LEVEL == null? 1 : ALARM_LEVEL;
            //Thread thread = new Thread(ThreadLog);
            // thread.Start(mod);
            ThreadLog(mod);
        }
        /// <summary>
        /// 写日志到数据库
        /// </summary>
        /// <param name="md"></param>
        public void ThreadLog(object obj)
        {
            MySqlConnection conn2 = new MySqlConnection(connStr);
            try
            {
                LogMod mod = (LogMod)obj;
                string SQLString = "insert into ts_uidp_loginfo(ACCESS_TIME,USER_ID,USER_NAME,IP_ADDR,LOG_TYPE,LOG_CONTENT,REMARK,ALARM_LEVEL)"
         + " VALUES(@ACCESS_TIME, @USER_ID, @USER_NAME, @IP_ADDR, @LOG_TYPE, @LOG_CONTENT, @REMARK,@ALARM_LEVEL)";
                MySqlParameter[] cmdParms = new MySqlParameter[8];
                cmdParms[0] = new MySqlParameter("@ACCESS_TIME", mod.ACCESS_TIME == null ? DateTime.Now : mod.ACCESS_TIME);
                cmdParms[1] = new MySqlParameter("@USER_ID", mod.USER_ID == null ? "" : mod.USER_ID);
                cmdParms[2] = new MySqlParameter("@USER_NAME", mod.USER_NAME == null ? "" : mod.USER_NAME);
                cmdParms[3] = new MySqlParameter("@IP_ADDR", mod.IP_ADDR == null ? "" : mod.IP_ADDR);
                cmdParms[4] = new MySqlParameter("@LOG_TYPE", mod.LOG_TYPE);
                cmdParms[5] = new MySqlParameter("@LOG_CONTENT", mod.LOG_CONTENT == null ? "" : mod.LOG_CONTENT);
                cmdParms[6] = new MySqlParameter("@REMARK", mod.REMARK == null ? "" : mod.REMARK);
                cmdParms[7] = new MySqlParameter("@ALARM_LEVEL", mod.ALARM_LEVEL == null ? 1 : mod.ALARM_LEVEL);
               
                
                using (MySqlCommand cmd = new MySqlCommand(SQLString, conn2))
                {
                   // MySqlTransaction tran = conn.BeginTransaction();
                    cmd.Parameters.AddRange(cmdParms);
                    if (conn2.State!=System.Data.ConnectionState.Open) {
                        conn2.Open();
                    }
                    cmd.ExecuteNonQuery();//s返回受影响行数
                    conn2.Close();
                   // tran.Commit();
                }
            }
            catch (MySqlException e)
            {
                conn2.Close();
                throw e;
            }
        }

    }
    public class LogConn
    {
        public string strConn { get; set; }
    }
}
