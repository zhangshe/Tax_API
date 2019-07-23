using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace UIDP.ODS
{
    public class DemoDB
    {
        DBTool db = new DBTool("");
        public DataTable fetchDemoList(Dictionary<string ,object> d)
        {
            string sql = "select * from ts_uidp_Demo";
            if (d["NAME"]!=null&& d["NAME"].ToString() != "")
            {
                sql += " where NAME like '%" + d["NAME"].ToString() + "%'";
            }
            return  db.GetDataTable(sql);
        }
        public string createDemoArticle(Dictionary<string, object> d)
        {
            string col = "";
            string val = "";
            d["ID"] = Guid.NewGuid();
            foreach (var v in d)
            {
                col += "," + v.Key;
                val += ",'" + v.Value + "'";
            }
            if (col != "")
            {
                col = col.Substring(1);
            }
            if (val != "")
            {
                val = val.Substring(1);
            }

            string sql = "INSERT INTO ts_uidp_Demo(" + col + ") VALUES(" + val + ")";

            return db.ExecutByStringResult(sql);
        }


        public string updateDemoData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_Demo set NAME='" + d["NAME"].ToString()+ "' ,AGE='" + d["AGE"].ToString() + "' where ID='" + d["ID"].ToString() + "'";

            return db.ExecutByStringResult(sql);
        }

        public string updateDemoArticle(Dictionary<string, object> d)
        {
            string sql = "delete FROM ts_uidp_Demo where ID='" + d["ID"].ToString() + "'";

            return db.ExecutByStringResult(sql);
        }

    }
}
