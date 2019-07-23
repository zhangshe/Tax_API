using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.LOG
{
    public class SysLog
    {
        private string dataType;
        private ClsSysLog clsSysLog;
        private ClsSysLogSqlServer clsSysLogSqlServer;
        private ClsSysLogOracle clsSysLogOracle;
        public SysLog()
        {
            try
            {
                dataType = GetStrConn();
                if (dataType == "MYSQL")
                {
                    clsSysLog = new ClsSysLog();
                }
                else if (dataType == "SQLSERVER")
                {
                    clsSysLogSqlServer = new ClsSysLogSqlServer();
                }
                else if (dataType == "ORACLE") {
                    clsSysLogOracle = new ClsSysLogOracle();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        //public void Close()
        //{
        //    try
        //    {
        //        if (dataType == "MYSQL")
        //        {
        //            clsSysLog.Close();
        //        }
        //        else if (dataType == "SQLSERVER")
        //        {
        //            clsSysLogSqlServer.Close();
        //        }
        //        else if (dataType == "ORACLE")
        //        {
        //            clsSysLogOracle.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
           
        //}
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
        public void Info(DateTime ACCESS_TIME, string USER_ID, string USER_NAME, string IP_ADDR, int LOG_TYPE, string LOG_CONTENT, string REMARK,int? ALARM_LEVEL)
        {
            try
            {
                if (dataType == "MYSQL")
                {
                    clsSysLog.Info(ACCESS_TIME, USER_ID, USER_NAME, IP_ADDR, LOG_TYPE, LOG_CONTENT, REMARK, ALARM_LEVEL);
                }
                else if (dataType == "SQLSERVER")
                {
                    clsSysLogSqlServer.Info(ACCESS_TIME, USER_ID, USER_NAME, IP_ADDR, LOG_TYPE, LOG_CONTENT, REMARK, ALARM_LEVEL);
                }
                else if (dataType == "ORACLE")
                {
                    clsSysLogOracle.Info(ACCESS_TIME, USER_ID, USER_NAME, IP_ADDR, LOG_TYPE, LOG_CONTENT, REMARK, ALARM_LEVEL);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
