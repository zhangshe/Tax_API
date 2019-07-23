using UIDP.ODS;
using UIDP.UTILITY;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace UIDP.BIZModule
{
    public class MenuModule
    {
        MenuDB db = new MenuDB();
        public List<ClsMenuInfo> fetchMenuList(Dictionary<string, object> sysCode)
        {


            List<ClsMenuInfo> clsMenuInfos = new List<ClsMenuInfo>();

            GetHierarchicalItem(db.fetchMenuList(sysCode), clsMenuInfos);

            clsMenuInfos=clsMenuInfos.OrderBy(o => o.MENU_ORDER).ToList();

            return clsMenuInfos;
        }

        public string createMenu(Dictionary<string, object> d) => db.createMenu(d);


        public string updateMenu(Dictionary<string, object> d)
        {
            return db.updateMenu(d);
        }
        public string deleteMenu(Dictionary<string, List<string>> d)
        {
            return db.deleteMenu(d);
        }

        public List<Dictionary<string, object>> fetchRoleMenuList(Dictionary<string, object> sysCode)
        {
            List<Dictionary<string, object>> d = KVTool.TableToListDic(db.fetchRoleMenuList(sysCode));

            return d;
        }
        public string setRoleMenus(Dictionary<string, object> d)
        {
            return db.setRoleMenus(d);
        }



        public List<ClsMenuInfo> fetchPermission(Dictionary<string, object> sysCode)
        {
            Dictionary<string, object> r= new Dictionary<string, object>();
          
            r["code"] = 2000;
            r["message"] = "查询成功";
            List<ClsMenuInfo> clsMenuInfos = new List<ClsMenuInfo>();

            GetHierarchicalItem(db.fetchPermission(sysCode), clsMenuInfos);
            clsMenuInfos=clsMenuInfos.OrderBy(o => o.MENU_ORDER).ToList();
            r["items"] = clsMenuInfos;
            

            return clsMenuInfos;
        }


        public void GetHierarchicalItem(DataTable _RptsDepartList, List<ClsMenuInfo> clsMenuInfos)
        {

            try
            {

                ClsMenuInfo clsMenuInfo;
                foreach (DataRow dr in _RptsDepartList.Select("MENU_ID_UPPER is null or MENU_ID_UPPER='' "))
                {
                    clsMenuInfo = new ClsMenuInfo();
                    clsMenuInfo.SYS_CODE = dr["SYS_CODE"].ToString();
                    clsMenuInfo.id = dr["MENU_ID"].ToString();
                    clsMenuInfo.MENU_NAME = dr["MENU_NAME"].ToString();
                    clsMenuInfo.parentId = dr["MENU_ID_UPPER"].ToString();
                    clsMenuInfo.MENU_ICON = dr["MENU_ICON"].ToString();
                    clsMenuInfo.MODULE_URL = dr["MODULE_URL"].ToString();
                    clsMenuInfo.MODULE_ROUTE = dr["MODULE_ROUTE"].ToString();
                    clsMenuInfo.MODULE_OBJ = dr["MODULE_OBJ"].ToString();
                    clsMenuInfo.MENU_PROP = dr["MENU_PROP"].ToString();
                    clsMenuInfo.MENU_ORDER = int.Parse(dr["MENU_ORDER"] == null ? "0" : dr["MENU_ORDER"].ToString());
                    if (dr["MENU_PROP"].ToString() == "0")
                    {
                        clsMenuInfo.disabled = true;
                    }
                    else {
                        clsMenuInfo.disabled = false;
                    }
                    clsMenuInfo.children = new List<ClsMenuInfo>();
                    GetHierarchicalChildItem(_RptsDepartList, clsMenuInfo);
                    clsMenuInfo.children=clsMenuInfo.children.OrderBy(o => o.MENU_ORDER).ToList();
                    clsMenuInfos.Add(clsMenuInfo);
                }

            }
            catch
            {
            }

        }

        private void GetHierarchicalChildItem(DataTable _RptsDepartList, ClsMenuInfo clsMenuInfos)
        {

            ClsMenuInfo clsMenuInfo;
            foreach (DataRow dr in _RptsDepartList.Select("MENU_ID_UPPER ='"+ clsMenuInfos .id+ "'"))
            {
                clsMenuInfo = new ClsMenuInfo();
                clsMenuInfo.SYS_CODE = dr["SYS_CODE"].ToString();
                clsMenuInfo.id = dr["MENU_ID"].ToString();
                clsMenuInfo.MENU_NAME = dr["MENU_NAME"].ToString();
                clsMenuInfo.parentId = dr["MENU_ID_UPPER"].ToString();
                clsMenuInfo.MENU_ICON = dr["MENU_ICON"].ToString();
                clsMenuInfo.MODULE_URL = dr["MODULE_URL"].ToString();
                clsMenuInfo.MODULE_ROUTE = dr["MODULE_ROUTE"].ToString();
                clsMenuInfo.MODULE_OBJ = dr["MODULE_OBJ"].ToString();
                clsMenuInfo.MENU_PROP = dr["MENU_PROP"].ToString();
                clsMenuInfo.MENU_ORDER = int.Parse(dr["MENU_ORDER"] == null ? "0" : dr["MENU_ORDER"].ToString());
                if (dr["MENU_PROP"].ToString() == "0")
                {
                    clsMenuInfo.disabled = true;
                }
                else
                {
                    clsMenuInfo.disabled = false;
                }
                clsMenuInfo.children = new List<ClsMenuInfo>();
                GetHierarchicalChildItem(_RptsDepartList, clsMenuInfo);
                clsMenuInfo.children = clsMenuInfo.children.OrderBy(o => o.MENU_ORDER).ToList();
                clsMenuInfos.children.Add(clsMenuInfo);
            }
        }
    }
}