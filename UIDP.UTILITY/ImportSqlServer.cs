using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;

namespace UIDP.UTILITY
{
   public class ImportSqlServer
    {
        private static readonly string connStr;
        //private static SqlConnection conn;

        /// <summary>
        /// 静态构造函数实例化连接字符串对象
        /// </summary>
        static ImportSqlServer()
        {
            connStr = GetStrConn();
            //conn = new SqlConnection(connStr);
           // conn.Open();
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
                    string key = o["SQLSERVER"].ToString();
                    return key;
                }
            }
        }



        ///// <summary>
        ///// 关闭连接
        ///// </summary>
        //public void Close()
        //{
        //    if (conn.State != System.Data.ConnectionState.Closed)
        //    {
        //        conn.Close();
        //    }
        //}

        public string Import(DataTable dt, string tableName)
        {
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    string sql = "truncate table " + tableName + ";";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        // MySqlTransaction tran = conn.BeginTransaction();
                        //cmd.Parameters.AddRange(cmdParms);
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            connection.Open();
                            //SqlTransaction tran = connection.BeginTransaction();
                        }
                        cmd.ExecuteNonQuery();//s返回受影响行数

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, null))
                        {
                            //每一批次中的行数
                            bulkCopy.BatchSize = 100000;
                            //超时之前操作完成所允许的秒数
                            bulkCopy.BulkCopyTimeout = 1800;

                            //将DataTable表名作为待导入库中的目标表名
                            bulkCopy.DestinationTableName = tableName;


                            //将数据集合和目标服务器库表中的字段对应 
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                //列映射定义数据源中的列和目标表中的列之间的关系
                                bulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                            }
                            //将DataTable数据上传到数据表中
                            bulkCopy.WriteToServer(dt);
                            return "2000";
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    connection.Close();
                }
            }
        }


    }
}
