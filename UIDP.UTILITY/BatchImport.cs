using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace UIDP.UTILITY
{
   public class BatchImport
    {


        private readonly string dataType;
        private ImportSqlServer sqlserverImport;
        private ImportMySql mysqlImport;
        private ImportOracle oracleImport;
        public BatchImport()
        {
            try
            {
                dataType = GetStrConn();
                if (dataType == "MYSQL")
                {
                    mysqlImport = new ImportMySql();
                }
                else if (dataType == "SQLSERVER")
                {
                    sqlserverImport = new ImportSqlServer();
                }
                else if (dataType == "ORACLE")
                {
                    oracleImport = new ImportOracle();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="ACCESS_TIME"></param>
        /// <param name="USER_ID"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="IP_ADDR"></param>
        /// <param name="LOG_TYPE"></param>
        /// <param name="LOG_CONTENT"></param>
        /// <param name="REMARK"></param>
        public string ImportInfo(DataTable dt, string tableName)
        {
            try
            {
                if (dataType == "MYSQL")
                {
                   return mysqlImport.Import(dt,tableName);
                }
                else if (dataType == "SQLSERVER")
                {
                    return sqlserverImport.Import(dt, tableName);
                }
                else if (dataType == "ORACLE")
                {
                    return oracleImport.Import(dt, tableName);
                }
                return "未处理异常！";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        /// <summary>
        /// 获取链接数据库类型
        /// </summary>
        /// <returns></returns>
        public static string GetStrConn()
        {
            try
            {
                using (System.IO.StreamReader file = System.IO.File.OpenText(System.IO.Directory.GetCurrentDirectory() + "\\log.json"))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o = (JObject)JToken.ReadFrom(reader);
                        string key = o["DataType"].ToString();
                        return key;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
