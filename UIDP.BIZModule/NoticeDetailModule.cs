using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class NoticeDetailModule
    {
        NoticeDetailDB db = new NoticeDetailDB();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> fetchNoticeDetailList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                //int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                //int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.fetchNoticeDetailList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(dt);
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
        public string createNoticeDetailArticle(Dictionary<string, object> d)
        {
            d["NOTICE_DETAIL_ID"] = Guid.NewGuid().ToString();
            return db.createNoticeDetailArticle(d);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateNoticeDetailArticle(string id)
        {
            return db.updateNoticeDetailArticle(id);
        }
    }
}
