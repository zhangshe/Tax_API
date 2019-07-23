using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;
namespace UIDP.ODS
{
    public class SyncConfDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询同步配置信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchSyncConfList(Dictionary<string, object> d)
        {
            string sql = "select * from ts_uidp_synchro_config a ";
            sql += " where 1=1 and IS_DELETE=0 ";
            if (d.Count > 0)
            {
                if (d["SERVER_IP"] != null && d["SERVER_IP"].ToString() != "")
                {
                    sql += " and a.SERVER_IP like '%" + d["SERVER_IP"].ToString() + "%'";
                }
                if (d["SERVER_PORT"] != null && d["SERVER_PORT"].ToString() != "")
                {
                    sql += " and a.SERVER_PORT='" + d["SERVER_PORT"].ToString()+"' ";
                }
                if (d["SERVER_URL"] != null && d["SERVER_URL"].ToString() != "")
                {
                    sql += " and a.SERVER_URL like '%" + d["SERVER_URL"].ToString() + "%'";
                }
                if (d["SYNC_FLAG"] != null && d["SYNC_FLAG"].ToString() != "")
                {
                    sql += " and a.SYNC_FLAG=" + d["SYNC_FLAG"].ToString();
                }
            }
            return db.GetDataTable(sql);
        }
        public string createSyncConf(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO ts_uidp_synchro_config(SYNC_ID,SERVER_IP,SERVER_PORT,SERVER_URL,AUTHENTICATION_URL,USER_CODE,USER_PASS,SYNC_TYPE,SYNC_FLAG,IS_DELETE,REMARK) VALUES(";
            sql += "'" + GetIsNullStr(d["SYNC_ID"]) + "',";
            sql += "'" + GetIsNullStr(d["SERVER_IP"]) + "',";
            sql += "'" + GetIsNullStr(d["SERVER_PORT"]) + "',";
            sql += "'" + GetIsNullStr(d["SERVER_URL"]) + "',";
            sql += "'" + GetIsNullStr(d["AUTHENTICATION_URL"]) + "',";
            sql += "'" + GetIsNullStr(d["USER_CODE"]) + "',";
            sql += "'" + GetIsNullStr(d["USER_PASS"]) + "',";
            sql += "'" + GetIsNullStr(d["SYNC_TYPE"]) + "',";
            sql += GetIsNullStr(d["SYNC_FLAG"]) + ",0,";
            sql += "'" + GetIsNullStr(d["REMARK"]) + "')";
            return db.ExecutByStringResult(sql);
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


        public string updateSyncConf(Dictionary<string, object> d,string passnew)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" update ts_uidp_synchro_config set ");
            sb.Append(" SERVER_IP='");
            sb.Append(d["SERVER_IP"] == null ? "" : GetIsNullStr(d["SERVER_IP"]) + "', ");
            sb.Append(" SERVER_PORT='");
            sb.Append(d["SERVER_PORT"] == null ? "" : GetIsNullStr(d["SERVER_PORT"]) + "', ");
            sb.Append(" SERVER_URL='");
            sb.Append(d["SERVER_URL"] == null ? "" : GetIsNullStr(d["SERVER_URL"]) + "', ");
            sb.Append(" AUTHENTICATION_URL='");
            sb.Append(d["AUTHENTICATION_URL"] == null ? "" : GetIsNullStr(d["AUTHENTICATION_URL"]) + "', ");

            sb.Append(" USER_CODE='");
            sb.Append(d["USER_CODE"] == null ? "" : GetIsNullStr(d["USER_CODE"]) + "', ");
            sb.Append(" USER_PASS= case when USER_PASS='");
            sb.Append(d["USER_PASS"] == null ? "" : GetIsNullStr(d["USER_PASS"]) + "' THEN USER_PASS else '"+passnew+"' end , ");
            sb.Append(" SYNC_TYPE='");
            sb.Append(d["SYNC_TYPE"] == null ? "" : GetIsNullStr(d["SYNC_TYPE"]) + "', ");

            sb.Append(" SYNC_FLAG=");
            sb.Append(d["SYNC_FLAG"] == null ? "0" : GetIsNullStr(d["SYNC_FLAG"]) + ", ");
            sb.Append(" REMARK='");
            sb.Append(d["REMARK"] == null ? "" : GetIsNullStr(d["REMARK"]));
            sb.Append("' where SYNC_ID='" + GetIsNullStr(d["SYNC_ID"].ToString()) + "' ");
            return db.ExecutByStringResult(sb.ToString());
        }


        public string deleteSyncConf(string id)
        {
            string sql = "update ts_uidp_synchro_config set IS_DELETE=1 where SYNC_ID ='" + id + "'";

            return db.ExecutByStringResult(sql);
        }

        public DataTable GetSyncConfById(string syncId)
        {
            string sql = " select * from ts_uidp_synchro_config where SYNC_ID='" + syncId + "'";
            return db.GetDataTable(sql);
        }
    }
}