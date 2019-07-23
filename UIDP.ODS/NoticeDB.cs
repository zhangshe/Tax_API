using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class NoticeDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 查询公告信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchNoticeList(Dictionary<string, object> d)
        {
            string sql = "select * from ts_store_notice a ";
            sql += " where 1=1 and IS_DELETE=0 ";
            if (d.Count > 0)
            {
                if (d["NOTICE_CODE"] != null && d["NOTICE_CODE"].ToString() != "")
                {
                    sql += " and a.NOTICE_CODE like '%" + d["NOTICE_CODE"].ToString() + "%'";
                }
                if (d["NOTICE_TITLE"] != null && d["NOTICE_TITLE"].ToString() != "")
                {
                    sql += " and a.NOTICE_TITLE like '%" + d["NOTICE_TITLE"].ToString() + "%'";
                }
                if (d["NOTICE_CONTENT"] != null && d["NOTICE_CONTENT"].ToString() != "")
                {
                    sql += " and a.NOTICE_CONTENT like '%" + d["NOTICE_CONTENT"].ToString() + "%'";
                }
                if (d["NOTICE_ORGID"] != null && d["NOTICE_ORGID"].ToString() != "")
                {
                    sql += " and a.NOTICE_ORGID=" + d["NOTICE_ORGID"].ToString();
                }
                if (d["NOTICE_ISSUER"] != null && d["NOTICE_ISSUER"].ToString() != "")
                {
                    sql += " and a.NOTICE_ISSUER like '%" + d["NOTICE_ISSUER"].ToString() + "%'";
                }
                if (d["BEGIN_NOTICE_DATETIME"] != null && d["BEGIN_NOTICE_DATETIME"].ToString() != "" && (d["END_NOTICE_DATETIME"] == null || d["END_NOTICE_DATETIME"].ToString() == ""))
                {
                    DateTime date = Convert.ToDateTime(d["BEGIN_NOTICE_DATETIME"].ToString());
                    sql += " and NOTICE_DATETIME > '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00'";
                    //sql += " and NOTICE_DATETIME between '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00' and '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";
                }
                else if (d["END_NOTICE_DATETIME"] != null && d["END_NOTICE_DATETIME"].ToString() != "" && (d["BEGIN_NOTICE_DATETIME"] == null || d["BEGIN_NOTICE_DATETIME"].ToString() == ""))
                {
                    DateTime date = Convert.ToDateTime(d["END_NOTICE_DATETIME"].ToString());
                    sql += " and NOTICE_DATETIME < '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";

                }
                else if (d["BEGIN_NOTICE_DATETIME"] != null && d["BEGIN_NOTICE_DATETIME"].ToString() != "" && d["END_NOTICE_DATETIME"] != null && d["END_NOTICE_DATETIME"].ToString() != "")
                {
                    DateTime bdate = Convert.ToDateTime(d["BEGIN_NOTICE_DATETIME"].ToString());
                    DateTime edate = Convert.ToDateTime(d["END_NOTICE_DATETIME"].ToString());
                    sql += " and NOTICE_DATETIME between '" + bdate.Year + "-" + bdate.Month + "-" + bdate.Day + " 00:00:00' and '" + edate.Year + "-" + edate.Month + "-" + edate.Day + " 23:59:59'";
                }
                sql += " order by NOTICE_DATETIME desc ";
            }
            return db.GetDataTable(sql);
        }
        public string createNoticeArticle(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO ts_store_notice(NOTICE_ID,NOTICE_CODE,NOTICE_TITLE,NOTICE_CONTENT,NOTICE_DATETIME,NOTICE_ORGID,NOTICE_ORGNAME,CREATER,CREATE_DATE,IS_DELETE) VALUES(";
            sql += "'" + GetIsNullStr(d["NOTICE_ID"]) + "',";
            sql += "'" + GetIsNullStr(d["NOTICE_CODE"]) + "',";
            sql += "'" + GetIsNullStr(d["NOTICE_TITLE"]) + "',";
            sql += "'" + GetIsNullStr(d["NOTICE_CONTENT"]) + "',";
            sql += "'" + GetIsNullStr(d["NOTICE_DATETIME"]) + "',";
            sql += "'" + GetIsNullStr(d["NOTICE_ORGID"]) + "',";
            sql += "'" + GetIsNullStr(d["NOTICE_ORGNAME"]) + "',";
            sql += "'" + GetIsNullStr(d["CREATER"]) + "',";
            sql += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0 )";
            //sql += "'" + GetIsNullStr(d["REMARK"]) + "')";
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


        public string updateNoticeData(Dictionary<string, object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" update ts_store_notice set ");
            sb.Append(" NOTICE_CODE='");
            sb.Append(d["NOTICE_CODE"] == null ? "" : GetIsNullStr(d["NOTICE_CODE"]) + "', ");
            sb.Append(" NOTICE_TITLE='");
            sb.Append(d["NOTICE_TITLE"] == null ? "" : GetIsNullStr(d["NOTICE_TITLE"]) + "', ");
            sb.Append(" NOTICE_CONTENT='");
            sb.Append(d["NOTICE_CONTENT"] == null ? "" : GetIsNullStr(d["NOTICE_CONTENT"]) + "', ");

            sb.Append(" NOTICE_DATETIME='");
            sb.Append(d["NOTICE_DATETIME"] == null ? "" : GetIsNullStr(d["NOTICE_DATETIME"]) + "', ");
            //sb.Append(" NOTICE_ORGID='");
            //sb.Append(d["NOTICE_ORGID"] == null ? "" : GetIsNullStr(d["NOTICE_ORGID"]) + "', ");
            //sb.Append(" NOTICE_ORGNAME='");
            //sb.Append(d["NOTICE_ORGNAME"] == null ? "" : GetIsNullStr(d["NOTICE_ORGNAME"]) + "', ");

            sb.Append(" CREATER='");
            sb.Append(d["CREATER"] == null ? "" : GetIsNullStr(d["CREATER"]));
            sb.Append("' where NOTICE_ID='" + GetIsNullStr(d["NOTICE_ID"].ToString()) + "' ");
            return db.ExecutByStringResult(sb.ToString());
        }


        public string updateNoticeArticle(string id)
        {
            string sql = "update ts_store_notice set IS_DELETE=1 where NOTICE_ID ='" + id + "'";

            return db.ExecutByStringResult(sql);
        }

        public DataTable GetNoticeById(string noticeId)
        {
            string sql = " select * from ts_store_notice where NOTICE_ID='" + noticeId + "'";
            return db.GetDataTable(sql);
        }

        public string getNoticeNum()
        {
            string num = "0";
            string sql = "";
            if (DateTime.Now.Month != 12)
            {
                sql = "select count(*) from ts_store_notice where  CREATE_DATE  between '" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-01" + " 00:00:00' and '" + DateTime.Now.Year + "-" + Convert.ToInt32(DateTime.Now.Month + 1) + "-01" + " 00:00:00'";
            }
            else
            {
                sql= "select count(*) from ts_store_notice where  CREATE_DATE  between '" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-01" + " 00:00:00' and '" + Convert.ToInt32(DateTime.Now.Year+1)+ "-" + Convert.ToInt32(DateTime.Now.Month) + "-01" + " 00:00:00'";
            }
            num = db.GetString(sql);
            if (string.IsNullOrEmpty(num))
            {
                num = "0";
            }
            return num;
        }
        public string checkNotice(string id, string code)
        {
            string num = "0";
            string sql = "select count(*) from ts_store_notice where NOTICE_ID !='" + id + "' and NOTICE_CODE='" + code + "' and IS_DELETE=0";
            num = db.GetString(sql);
            if (string.IsNullOrEmpty(num))
            {
                num = "0";
            }
            return num;
        }
    }
}
