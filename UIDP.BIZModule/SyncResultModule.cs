using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;
using Newtonsoft.Json;

namespace UIDP.BIZModule
{
   public class SyncResultModule
    {
        SyncResultDB db = new SyncResultDB();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> fetchSyncResultList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.fetchSyncResultList(d);
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
        public Dictionary<string, object> createSyncResult(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                d["RESULT_ID"] = Guid.NewGuid().ToString();
                string result = db.createSyncResult(d);
                r["result"] = true;
                r["code"] = 2000;
                r["message"] = result;
            }
            catch (Exception ex)
            {
                r["result"] = false;
                r["code"] = -1;
                r["message"] = ex.Message; ;
            }
            return r;
        }
    }
}
