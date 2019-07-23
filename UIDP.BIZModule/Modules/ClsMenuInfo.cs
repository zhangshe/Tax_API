using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule
{
    public class ClsMenuInfo
    {
        public string SYS_CODE { get; set; }
        public string id { get; set; }
        public string MENU_NAME { get; set; }
        public string parentId { get; set; }
        public string MENU_ICON { get; set; }
        public string MODULE_URL { get; set; }
        public string MODULE_ROUTE { get; set; }
        public string MODULE_OBJ { get; set; }
        public string  MENU_PROP { get; set; }
        public int MENU_ORDER { get; set; }
        public bool disabled { get; set; }
        public List<ClsMenuInfo> children { get; set; }


    }
}
