using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.Modules
{
    public class ConfigNode
    {
        public string S_Id { get; set; }
        public string ParentCode { get; set; }
        public string Code { get; set; }
        public string EnglishCode { get; set; }
        public string Name { get; set; }
        public List<ConfigNode> children { get; set; }
        public string SortNo { get; set; }
    }
}
