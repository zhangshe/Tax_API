using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class TaxConfigDB
    {
        DBTool db = new DBTool("MYSQL");
        public DataTable getData()
        {
            string sql = "SELECT * FROM tax_dictionary ORDER BY Code,SortNo";
            return db.GetDataTable(sql);
        }

        public string editNode(Dictionary<string,object> d)
        {
            string sql = "UPDATE tax_dictionary SET ParentCode='" + d["ParentCode"] + "',";
            sql += "Code='" + d["Code"] + "',";
            sql += "Name='" + d["Name"] + "',";
            sql += "S_UpdateBy='" + d["S_UpdateBy"]+"',";
            sql += "S_UpdateDate='" + d["S_UpdateDate"] + "'";
            if (d["EnglishCode"]!=null&& d["EnglishCode"].ToString() != "")
            {
                sql += ",EnglishCode='" + d["EnglishCode"] + "'";
            }
            if(d["SortNo"]!=null&& d["SortNo"].ToString() != "")
            {
                sql += ",SortNo=" + d["SortNo"] + "";
            }
            sql += " WHERE S_Id='" + d["S_Id"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string createNode(Dictionary<string, object> d)
        {
            StringBuilder sql = new StringBuilder();
            //string sql = "INSERT INTO tax_dictionary(S_Id,S_CreateDate,S_CreateBy,ParentCode,Code,Name,EnglishCode,SortNo)VALUES(";
            sql.Append("INSERT INTO tax_dictionary(S_Id,S_CreateDate,S_CreateBy,ParentCode,Code,Name,EnglishCode,SortNo)VALUES('");
            sql.Append(d["S_Id"]);
            sql.Append("','");
            sql.Append(d["S_CreateDate"]);
            sql.Append("','");
            sql.Append(d["S_CreateBy"]);
            sql.Append("','");
            sql.Append(d["ParentCode"]);
            sql.Append("','");
            sql.Append(d["Code"]);
            sql.Append("','");
            sql.Append(d["Name"]);
            sql.Append("','");
            sql.Append(d["EnglishCode"] == null ? "" : d["EnglishCode"]);
            sql.Append("',");
            sql.Append(d["SortNo"] == null ? 0 : d["SortNo"]);
            sql.Append(")");
            return db.ExecutByStringResult(sql.ToString().Trim());
        }

        public DataTable getRepeatInfo(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM  tax_dictionary WHERE 1=1";
            sql+=" AND Code='" + d["Code"] + "'";
            //sql+=" OR Name='" + d["Name"] + "'";
            return db.GetDataTable(sql);
        }

        public string delNode(Dictionary<string,object> d)
        {
            string sql = "DELETE FROM tax_dictionary WHERE S_Id='" + d["S_Id"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataTable search(string param)
        {
            string sql = "SELECT * FROM tax_dictionary WHERE Code='" + param + "'" + " OR Name='" + param + "'" + " OR EnglishCode='" + param + "'";
            return db.GetDataTable(sql);
        }



    }
}
