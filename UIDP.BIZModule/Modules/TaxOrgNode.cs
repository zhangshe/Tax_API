using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.Modules
{
    public class TaxOrgNode
    {
        public string id { get; set; }
        public string orgCode { get; set; }
        public string orgName { get; set; }
        public string orgShortName { get; set; }
        public string parentId { get; set; }
        public string ISINVALID { get; set; }
        public string remark { get; set; }
        public List<TaxOrgNode> children { get; set; }
    }
}
