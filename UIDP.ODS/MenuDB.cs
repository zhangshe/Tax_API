using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace UIDP.ODS
{
    public class MenuDB
    {
        DBTool db = new DBTool("MYSQL");
        public DataTable fetchMenuList(Dictionary<string,object> sysCode)
        {
          return  db.GetDataTable("select * from ts_uidp_menuinfo where   SYS_CODE='" + sysCode ["sysCode"] + "' order by MENU_ORDER ");
        }

        public string createMenu(Dictionary<string, object> d)
        {
            string col = "";
            string val = "";
            foreach (var v in d)
            {
                if (v.Value != null)
                {
                    col += "," + v.Key;
                    val += ",'" + v.Value + "'";
                }
                else
                {
                    col += "," + v.Key;
                    val += ",''";
                }
            }
            if (col != "")
            {
                col = col.Substring(1);
            }
            if (val != "")
            {
                val = val.Substring(1);
            }

            string sql = "INSERT INTO ts_uidp_menuinfo(" + col + ") VALUES(" + val + ")";

            return db.ExecutByStringResult(sql);
        }


        public string updateMenu(Dictionary<string, object> d)
        {

            d.Remove("id");
            d.Remove("parentId");
            d.Remove("children");
            d.Remove("disabled");
            string col = "";
            
            foreach (var v in d)
            {
                if (v.Value == null)
                {
                    col += "," + v.Key + "=''";
                }
                else
                {
                    col += "," + v.Key + "='" + v.Value.ToString() + "'";
                }
               
            
            }
            if (col != "")
            {
                col = col.Substring(1);
            }

            string sql = "update  ts_uidp_menuinfo set "+ col + " where MENU_ID='" + d["MENU_ID"].ToString() + "'";

            return db.ExecutByStringResult(sql);
        }

        public string deleteMenu(Dictionary<string, List<string>> d)
        {
            List<string> m = d["MENU_ID"];

            string col = "";
            foreach (string v in m)
            {
               
                    col += " or  MENU_ID='" + v+"'";
               
                  
            }
            if (col != "")
            {
                col = col.Substring(3);
            }

            string sql = "delete FROM ts_uidp_menuinfo where " + col;

            return db.ExecutByStringResult(sql);
        }

        public DataTable fetchRoleMenuList(Dictionary<string, object> sysCode)
        {

            string sql = "select p.* from ts_uidp_group_powerinfo p , ts_uidp_menuinfo m  where p.GROUP_ID = '"+ sysCode["roleId"] + "' AND p.MENU_ID = m.MENU_ID and m.SYS_CODE = '"+ sysCode["sysCode"] + "'";
            return db.GetDataTable(sql);
            

        }

        public string setRoleMenus(Dictionary<string, object> d)
        {

            Dictionary<string, string> sql = new Dictionary<string, string>();

            sql["GROUP_ID"] = "delete from TS_UIDP_GROUP_POWERINFO where group_id = '"+d["GROUP_ID"].ToString() + "'";
            foreach (string str in d["MENU_ID"].ToString().Split(','))
            {
                sql[Guid.NewGuid().ToString()] = "INSERT INTO TS_UIDP_GROUP_POWERINFO(group_id, menu_id) VALUES('"+ d["GROUP_ID"].ToString() + "', '"+ str + "')";
            }

            return db.ExecutByStringResult(sql);
        }

        public DataTable fetchPermission(Dictionary<string, object> sysCode)
        {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Admin_Code' ";

          string user=  db.GetString(sqluser);
            string sql = "";
            if (user == sysCode["userId"].ToString())
            {
                sql = "SELECT  * from ts_uidp_menuinfo where   SYS_CODE = '" + sysCode["sysCode"].ToString() + "' order by MENU_ORDER ";
            }
            else
            {
                sql = "SELECT distinct * " +
      "from(select f.* " +
              "from ts_uidp_group_user ur, " +
                   "TS_UIDP_GROUP_POWERINFO rf, " +
                  "ts_uidp_menuinfo        f " +
             "where ur.user_id = '" + sysCode["userId"].ToString() + "' " +
             " and f.SYS_CODE='" + sysCode["sysCode"].ToString() + "' " +
               "and ur.group_id = rf.group_id  " +  //and f.MENU_PROP=1
               "and rf.menu_id = f.menu_id ) a union select * from ts_uidp_menuinfo where MENU_ID='0fea0012-b259-43b9-9c49-1a993cf3defa'";
            }

            //string sql = "SELECT * from ts_uidp_menuinfo";

            return db.GetDataTable(sql);


        }


    }
}
