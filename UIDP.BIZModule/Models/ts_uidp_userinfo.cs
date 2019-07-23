using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.BIZModule.Models
{
    [Serializable]
    public class ts_uidp_userinfo
    {
        public ts_uidp_userinfo()
        { }
        #region Model
        private string _user_id;
        private string _user_code;
        private string _user_name;
       // private string _user_alias;
        private string _user_pass;
        private string _phone_mobile;
        private string _phone_office;
       // private string _phone_org;
        private string _user_email;
       // private string _email_office;
        private string _user_ip;
        private DateTime? _reg_time;
        private int? _flag = 0;
        private string _user_domain;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public string USER_ID
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string USER_CODE
        {
            set { _user_code = value; }
            get { return _user_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string USER_NAME
        {
            set { _user_name = value; }
            get { return _user_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        //public string USER_ALIAS
        //{
        //    set { _user_alias = value; }
        //    get { return _user_alias; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public string USER_PASS
        {
            set { _user_pass = value; }
            get { return _user_pass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PHONE_MOBILE
        {
            set { _phone_mobile = value; }
            get { return _phone_mobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PHONE_OFFICE
        {
            set { _phone_office = value; }
            get { return _phone_office; }
        }
        /// <summary>
        /// 
        /// </summary>
        //public string PHONE_ORG
        //{
        //    set { _phone_org = value; }
        //    get { return _phone_org; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public string USER_EMAIL
        {
            set { _user_email = value; }
            get { return _user_email; }
        }
        /// <summary>
        /// 
        /// </summary>
        //public string EMAIL_OFFICE
        //{
        //    set { _email_office = value; }
        //    get { return _email_office; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public string USER_IP
        {
            set { _user_ip = value; }
            get { return _user_ip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? REG_TIME
        {
            set { _reg_time = value; }
            get { return _reg_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FLAG
        {
            set { _flag = value; }
            get { return _flag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string USER_DOMAIN
        {
            set { _user_domain = value; }
            get { return _user_domain; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REMARK
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
