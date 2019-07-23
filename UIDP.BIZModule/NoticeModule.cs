using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class NoticeModule
    {
        NoticeDB db = new NoticeDB();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> fetchNoticeList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.fetchNoticeList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception ex)
            {
                r["items"] = null;
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createNoticeArticle(Dictionary<string, object> d)
        {
            d["NOTICE_ID"] = Guid.NewGuid().ToString();
            int SwiftNumber = Convert.ToInt32(db.getNoticeNum());
            SwiftNumber++;
            string NoticeCode = "XW" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + SwiftNumber.ToString().PadLeft(3, '0');
            d["NOTICE_CODE"] = NoticeCode;
            return db.createNoticeArticle(d);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateNoticeData(Dictionary<string, object> d)
        {
            if (db.checkNotice(d["NOTICE_ID"].ToString(), d["NOTICE_CODE"].ToString()) != "0")
            {
                return "操作失败，公告编号不能重复！";
            }
            return db.updateNoticeData(d);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateNoticeArticle(string id)
        {
            return db.updateNoticeArticle(id);
        }
    }
}
