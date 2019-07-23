using UIDP.ODS;
using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace UIDP.BIZModule
{
    public class DemoModule
    {
        DemoDB db = new DemoDB();
        public Dictionary<string, object> fetchDemoList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchDemoList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.RowsToListDic(dt, d);
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public string createDemoArticle(Dictionary<string, object> d)
        {
            return db.createDemoArticle(d);
        }
        public string updateDemoData(Dictionary<string, object> d)
        {
            return db.updateDemoData(d);
        }
        public string updateDemoArticle(Dictionary<string, object> d)
        {
            return db.updateDemoArticle(d);
        }


    }
}
