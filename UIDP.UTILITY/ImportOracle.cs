using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using System.Threading;

namespace UIDP.UTILITY
{
   public class ImportOracle
    {
        private static readonly string connStr;
        private static OracleConnection conn;

        /// <summary>
        /// 静态构造函数实例化连接字符串对象
        /// </summary>
        static ImportOracle()
        {
            connStr = GetStrConn();
            conn = new OracleConnection(connStr);
            conn.Open();
        }
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
                    string key = o["ORACLE"].ToString();
                    return key;
                }
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }


        public string getString(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString().Replace("\\", "").Trim();
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name=""></param>
        public string Import(DataTable dt, string tableName)
        {
            OracleConnection oracleconn = new OracleConnection(connStr);
            try
            {
                string fengefu = "";
                StringBuilder sb = new StringBuilder();
                try
                {
                    sb.Append("truncate table " + tableName + "; insert into " + tableName + " (ORG_ID,ORG_CODE,ORG_NAME,ORG_SHORT_NAME,ORG_CODE_UPPER,ISINVALID,ISDELETE,REMARK) values ");
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append(fengefu + "('" + getString(row["ORG_ID"]) + "',");
                        sb.Append("'" + getString(row["ORG_CODE"]) + "',");
                        sb.Append("'" + getString(row["ORG_NAME"]) + "',");
                        sb.Append("'" + getString(row["ORG_SHORT_NAME"]) + "',");
                        sb.Append("'" + getString(row["ORG_CODE_UPPER"]) + "',");
                        sb.Append("'" + getString(row["ISINVALID"]) + "',");
                        sb.Append("'1',");
                        sb.Append("'" + getString(row["REMARK"]) + "')");
                        fengefu = ",";
                    }
                    using (OracleCommand cmd = new OracleCommand(sb.ToString(), oracleconn))
                    {
                        // MySqlTransaction tran = conn.BeginTransaction();
                        //cmd.Parameters.AddRange(cmdParms);
                        if (oracleconn.State != System.Data.ConnectionState.Open)
                        {
                            oracleconn.Open();
                        }
                        cmd.ExecuteNonQuery();//s返回受影响行数
                        oracleconn.Close();
                        return "2000";
                        // tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            catch (OracleException e)
            {
                oracleconn.Close();
                return e.ToString();
            }
        }
    }
}
