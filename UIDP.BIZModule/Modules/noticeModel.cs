using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.Modules
{
    public class noticeModel
    {
        public string NOTICE_ID { get; set; }
        public string NOTICE_CODE { get; set; }
        public string NOTICE_TITLE { get; set; }
        public string NOTICE_CONTENT { get; set; }
        public DateTime NOTICE_DATETIME { get; set; }
        public string NOTICE_ORGID { get; set; }
        public string NOTICE_ORGNAME { get; set; }
        public int? IS_DELETE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        //明细
        public List<noticeDetailModel> children { get; set; }
    }
}
