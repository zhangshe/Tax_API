using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;
using Newtonsoft.Json;

namespace UIDP.BIZModule
{
   public class SyncConfModule
    {
        SyncConfDB db = new SyncConfDB();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> fetchSyncConfList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                //DataTable dt = db.fetchSyncConfList(d);
                //string jsonStr = "";
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    jsonStr = GetSubMenu("", dt);
                //}
                //r["items"] = JsonConvert.DeserializeObject("["+jsonStr+"]");
                //r["code"] = 2000;
                //r["message"] = "查询成功";

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchSyncConfList(d);
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
        public string createSyncConf(Dictionary<string, object> d)
        {
            d["SYNC_ID"] = Guid.NewGuid().ToString();
            d["USER_PASS"] = Security.SecurityHelper.StringToMD5Hash(d["USER_PASS"].ToString());
            return db.createSyncConf(d);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateSyncConf(Dictionary<string, object> d)
        {
            string passnew = Security.SecurityHelper.StringToMD5Hash(d["USER_PASS"].ToString());
            return db.updateSyncConf(d,passnew);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteSyncConf(string id)
        {
            return db.deleteSyncConf(id);
        }
        public List<Dictionary<string, object>> getSyncConfList()
        {
            List < Dictionary<string, object>> r = new List<Dictionary<string, object>>();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                DataTable dt = db.fetchSyncConfList(d);
                r = KVTool.TableToListDic(dt);
                return r;
            }
            catch (Exception )
            {
                return null;
            }
        }
    }
}
