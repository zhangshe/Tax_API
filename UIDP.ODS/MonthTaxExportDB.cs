using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
   public class MonthTaxExportDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 按月地税查询和导出
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataSet getMonthTax(Dictionary<string, object> d)
        {
            IDataParameter[] parm = new SqlParameter[6];
            parm[0] = new SqlParameter("@actionType", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkDate", SqlDbType.DateTime);
            parm[3] = new SqlParameter("@Page", SqlDbType.Int);
            parm[4] = new SqlParameter("@PageSize", SqlDbType.Int);
            parm[5] = new SqlParameter("@S_Department", SqlDbType.NVarChar, 50);
            parm[0].Value = d["queryType"];
            parm[1].Value = d["S_OrgCode"];
            parm[2].Value = d["S_WorkDate"];
            parm[3].Value = d["page"];
            parm[4].Value = d["limit"];
            parm[5].Value = d["S_Department"];
            DataSet ds = db.GetProcedure("pro_MonthTaxExport", parm);
            return ds;
        }
        /// <summary>
        /// 全年地税查询和导出
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataSet getYearTax(Dictionary<string, object> d)
        {
            IDataParameter[] parm = new SqlParameter[6];
            parm[0] = new SqlParameter("@actionType", SqlDbType.NVarChar, 50);
            parm[1] = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
            parm[2] = new SqlParameter("@WorkDate", SqlDbType.DateTime);
            parm[3] = new SqlParameter("@Page", SqlDbType.Int);
            parm[4] = new SqlParameter("@PageSize", SqlDbType.Int);
            parm[5] = new SqlParameter("@S_Department", SqlDbType.NVarChar, 50);
            parm[0].Value = d["queryType"];
            parm[1].Value = d["S_OrgCode"];
            parm[2].Value = d["S_WorkDate"];
            parm[3].Value = d["page"];
            parm[4].Value = d["limit"];
            parm[5].Value = d["S_Department"];
            DataSet ds = db.GetProcedure("pro_YearTaxExport", parm);
            return ds;
        }
    }
}
