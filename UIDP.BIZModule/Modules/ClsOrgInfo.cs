using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule
{
    public class ClsOrgInfo
    {
        public string ORG_CODE { get; set; }
        public string id { get; set; }
        public string ORG_NAME { get; set; }
        public string parentId { get; set; }
        public string ORG_SHORT_NAME { get; set; }
        public string ORG_CODE_UPPER { get; set; }
        public string ORG_ID_UPPER { get; set; }
        public string REMARK { get; set; }
        public string ISINVALID { get; set; }

        public List<ClsOrgInfo> children { get; set; }


    }
}
