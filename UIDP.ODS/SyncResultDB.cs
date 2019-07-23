using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;
namespace UIDP.ODS
{
    public class SyncResultDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询同步配置信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchSyncResultList(Dictionary<string, object> d)
        {
            string sql = "select * from ts_uidp_synchro_result a ";
            sql += " where 1=1 ";
            if (d["SEND_URL"] != null && d["SEND_URL"].ToString() != "")
            {
                sql += " and a.SEND_URL like '%" + d["SEND_URL"].ToString() + "%'";
            }
       
            if (d["RECEIVE_URL"] != null && d["RECEIVE_URL"].ToString() != "")
            {
                sql += " and a.RECEIVE_URL like '%" + d["RECEIVE_URL"].ToString() + "%'";
            }
            if (d["SYNC_CONTENT"] != null && d["SYNC_CONTENT"].ToString() != "")
            {
                sql += " and a.SYNC_CONTENT like '%" + d["SYNC_CONTENT"].ToString() + "%'";
            }
            if (d["SYNC_RESULT"] != null && d["SYNC_RESULT"].ToString() != "")
            {
                sql += " and a.SYNC_RESULT=" + d["SYNC_RESULT"].ToString();
            }
            if (d["ERROR_INFO"] != null && d["ERROR_INFO"].ToString() != "")
            {
                sql += " and a.ERROR_INFO like '%" + d["ERROR_INFO"].ToString() + "%'";
            }
            return db.GetDataTable(sql);
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


        public string createSyncResult(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO ts_uidp_synchro_result(RESULT_ID,SEND_URL,RECEIVE_URL,SYNC_CONTENT,SYNC_RESULT,ERROR_INFO,FAIL_CONTENT,SYNC_TIME,REMARK) VALUES(";
            sql += "'" + GetIsNullStr(d["RESULT_ID"]) + "',";
            sql += "'" + GetIsNullStr(d["SEND_URL"]) + "',";
            sql += "'" + GetIsNullStr(d["RECEIVE_URL"]) + "',";
            sql += "'" + GetIsNullStr(d["SYNC_CONTENT"]) + "',";
            sql += GetIsNullStr(d["SYNC_RESULT"]) + ",";
            sql += "'" + GetIsNullStr(d["ERROR_INFO"]) + "',";
            sql += "'" + GetIsNullStr(d["FAIL_CONTENT"]) + "',";
            sql += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
            sql += "'" + GetIsNullStr(d["REMARK"]) + "')";
            return db.ExecutByStringResult(sql);
        }
        public DataTable GetSyncResultById(string resultId)
        {
            string sql = " select * from ts_uidp_synchro_result where RESULT_ID='" + resultId + "'";
            return db.GetDataTable(sql);
        }
    }
}