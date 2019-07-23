using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.LOG
{
  public  class LogMod
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string USER_ID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// ip
        /// </summary>

        public string IP_ADDR { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>

        public int? LOG_TYPE { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>

        public string LOG_CONTENT { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime ACCESS_TIME { get; set; }
        /// <summary>
        /// 告警级别
        /// </summary>
        public int? ALARM_LEVEL { get; set; }

    }
}
