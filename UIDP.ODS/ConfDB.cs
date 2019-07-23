using UIDP.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace UIDP.ODS
{
    public class ConfDB
    {
        DBTool db = new DBTool("MYSQL");
        public DataTable fetchConfigList(Dictionary<string ,object> d)
        {
            string sql = "select * from ts_uidp_config";
            if (d["CONF_NAME"]!=null&& d["CONF_NAME"].ToString() != "")
            {
                sql += " where CONF_NAME like '%" + d["CONF_NAME"].ToString() + "%'";
            }

           //int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
           // int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
           // page = page - 1;

           // sql += " limit " + limit * page + "," + limit;

            return  db.GetDataTable(sql);
        }
        /// <summary>
        /// 登录获取系统配置信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable loginConfig(Dictionary<string, object> d)
        {
            string sql = "select * from ts_uidp_config where CONF_CODE in ("+ d["CONF_CODE"].ToString()+ ") order by conf_code ";
            return db.GetDataTable(sql);
            
        }
        /// <summary>
        /// 获取系统配置颜色
        /// </summary>
        /// <returns></returns>
        public DataTable SysColor(Dictionary<string, object> d)
        {
            string sql = "select CONF_VALUE from ts_uidp_config where CONF_CODE='COLOR'";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 登录获取系统配置信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getConfig()
        {
            string sql = "select * from ts_uidp_config where CONF_CODE='CLOUD_ORG'";

            return db.GetDataTable(sql);
        }
        public string createConfigArticle(Dictionary<string, object> d)
        {
            string col = "";
            string val = "";
            foreach (var v in d)
            {
                col += "," + v.Key;
                val += ",'" + v.Value + "'";
            }
            if (col != "")
            {
                col = col.Substring(1);
            }
            if (val != "")
            {
                val = val.Substring(1);
            }

            string sql = "INSERT INTO ts_uidp_config(" + col + ") VALUES(" + val + ")";

            return db.ExecutByStringResult(sql);
        }


        public string updateConfigData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_config set  CONF_VALUE='" + d["CONF_VALUE"].ToString() + "' ";
            if (d["CONF_NAME"]!=null) {
                sql += " , CONF_NAME='" + d["CONF_NAME"].ToString() + "' ";
            }
            sql += " where CONF_CODE='" + d["CONF_CODE"].ToString() + "'";
            return db.ExecutByStringResult(sql);
        }

        public string updateConfigArticle(Dictionary<string, object> d)
        {
            string sql = "delete FROM ts_uidp_config where CONF_CODE='" + d["CONF_CODE"].ToString() + "'";

            return db.ExecutByStringResult(sql);
        }

    }
}
