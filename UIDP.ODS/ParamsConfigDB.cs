using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class ParamsConfigDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 获取减除项标准配置信息
        /// </summary>
        /// <returns></returns>
        public DataTable getSubtrackStandardConfig()
        {
            string sql = "SELECT * FROM tax_subtractconfig ORDER BY TaxLevel";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 修改配置信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string editConfig(Dictionary<string,object> d)
        {
            string sql = "UPDATE tax_subtractconfig SET S_UpdateBy='" + d["username"] + "',";
            sql += "S_UpdateDate='" + d["S_UpdateDate"] + "',";
            sql += "TaxLevel=" + d["TaxLevel"] + ",";
            if (d["TaxStart"]== null|| d["TaxStart"].ToString()=="")
            {
                sql += "TaxStart = NULL,";
            }
            else
            {
                sql += "TaxStart=" + d["TaxStart"] + ",";
            }
            if (d["TaxEnd"] == null|| d["TaxEnd"].ToString()=="")
            {
                sql += "TaxEnd = NULL,";
            }
            else
            {
                sql += "TaxEnd=" + d["TaxEnd"] + ",";
            }
            sql += "TaxRate='" + d["TaxRate"] + "',";
            sql += "TaxDeduction='" + d["TaxDeduction"] + "'";
            sql += " WHERE S_Id='" + d["S_Id"] + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 删除配置项
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string delConfig(Dictionary<string,object> d)
        {
            string sql = "DELETE FROM tax_subtractconfig WHERE S_Id='" + d["S_Id"] + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 新建配置项
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createConfig(Dictionary<string,object> d)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tax_subtractconfig(S_Id,S_CreateDate,S_CreateBy,TaxLevel,TaxStart,TaxEnd,TaxRate,TaxDeduction)VALUES('");
            sql.Append(d["S_Id"]);
            sql.Append("','");
            sql.Append(d["S_CreateDate"]);
            sql.Append("','");
            sql.Append(d["username"]);
            sql.Append("',");
            sql.Append(d["TaxLevel"]);
            sql.Append(",");
            sql.Append(d["TaxStart"]);
            sql.Append(",");
            sql.Append(d["TaxEnd"]);
            sql.Append(",");
            sql.Append(d["TaxRate"]);
            sql.Append(",");
            sql.Append(d["TaxDeduction"]);
            sql.Append(")");
            return db.ExecutByStringResult(sql.ToString().Trim());
        }
        /// <summary>
        /// 创建时判断级数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable judgeCreateTaxLevel(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM tax_subtractconfig WHERE ";
            sql += "TaxLevel='" + d["TaxLevel"] + "'";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 新建时判断范围
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable judgeCreateRepeat(Dictionary<string,object> d)
        {
            string sql = "SELECT * FROM tax_subtractconfig WHERE (" + d["TaxStart"];
            sql += " BETWEEN TaxStart AND TaxEnd ";
            sql += " AND TaxEnd!=" + d["TaxStart"];
            sql += " AND TaxEnd!=" + d["TaxStart"] + ")";
            sql += " OR(";
            sql += d["TaxEnd"];
            sql += " BETWEEN TaxStart AND TaxEnd";
            sql += " AND TaxStart!=" + d["TaxEnd"] + ")";
            return db.GetDataTable(sql);
        }

        public DataTable judgeEditTaxLevel(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM tax_subtractconfig WHERE ";
            sql += "TaxLevel='" + d["TaxLevel"] + "'";
            sql += " AND S_Id!='" + d["S_Id"] + "'";
            return db.GetDataTable(sql);
        }


        public DataTable judgeEditRepeat(Dictionary<string, object> d)
        {
            //string sql = "SELECT * FROM tax_subtractconfig WHERE (" + d["TaxStart"];
            //sql += " BETWEEN TaxStart AND TaxEnd OR ";
            //sql += d["TaxEnd"];
            //sql += " BETWEEN TaxStart AND TaxEnd) ";
            //sql += " AND S_Id!='" + d["S_Id"] + "'";
            string sql = "SELECT * FROM tax_subtractconfig WHERE( (" + d["TaxStart"];
            sql += " BETWEEN TaxStart AND TaxEnd ";
            sql += " AND TaxEnd!=" + d["TaxStart"];
            sql += " AND TaxEnd!=" + d["TaxStart"] + ")";
            sql += " OR(";
            sql += d["TaxEnd"];
            sql += " BETWEEN TaxStart AND TaxEnd";
            sql += " AND TaxStart!=" + d["TaxEnd"] + "))";
            sql += " AND S_Id!='" + d["S_Id"] + "'";
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 以上查询是针对减除项标准配置功能
        /// 下面的SQL是针对扣减项配置
        /// 查询扣减项配置
        /// </summary>
        #region 扣减项配置操作
        public DataTable getDecreasingConfig(Dictionary<string, object> d)
        {
            string sql = "select a.*,(select name from tax_dictionary where a.TaxOffice = tax_dictionary.Code) TaxOfficeName from tax_deduction a where 1 = 1 ";
            if (d.Keys.Contains("DCode") && d["DCode"] != null && d["DCode"].ToString() != "")
            {
                sql += " and DCode = '" + d["DCode"].ToString() + " '";
            }
           
            if (d.Keys.Contains("TaxOffice") && d["TaxOffice"] != null && d["TaxOffice"].ToString() != "")
            {
                sql += " and TaxOffice ='" + d["TaxOffice"].ToString() + "' ";
            }
            sql += " order by TaxOffice,S_CreateDate ";
            return db.GetDataTable(sql);
        }
        public string GetIsNullStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "null";
            }
            else
            {
                return "'" + str.ToString() + "'";
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createDecreasingConfig(Dictionary<string, object> d)
        {
            StringBuilder sql = new StringBuilder();
            decimal num = Convert.ToDecimal(d["Proportion"]) / 100;
            sql.Append("INSERT INTO tax_deduction(S_Id,S_OrgCode,S_OrgName,S_CreateDate,S_CreateBy,DName,DCode,DLimit,TaxOffice,Proportion,Pattern) VALUES(");
            sql.Append(GetIsNullStr(d["S_Id"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_OrgCode"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_OrgName"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_CreateDate"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["S_CreateBy"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["DName"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(d["DCode"].ToString()));
            sql.Append(",");
            sql.Append(string.IsNullOrEmpty(d["DLimit"].ToString())?"null": d["DLimit"].ToString());
            sql.Append(",");
            sql.Append(GetIsNullStr(d["TaxOffice"].ToString()));
            sql.Append(",");
            sql.Append(GetIsNullStr(num.ToString()));
            sql.Append(",");
            if (d["Pattern"] == null || d["Pattern"].ToString() == "")
            {
                sql.Append("NULL");
            }
            else
            {
                sql.Append(d["Pattern"]);
            }
            sql.Append(")");
            return db.ExecutByStringResult(sql.ToString().Trim());
        }

        public string delDecreasingConfig(string sid)
        {
            string sql = "DELETE FROM tax_deduction WHERE S_Id='" + sid + "'";
            return db.ExecutByStringResult(sql);
        }

        public string editDecreasingConfig(Dictionary<string, object> d)
        {
            //string sql = "UPDATE tax_deduction SET S_OrgCode='" + d["S_OrgCode"] + "',";
            //sql += "S_OrgName='" + d["S_OrgName"] + "',";
            //sql += "S_UpdateBy='" + d["username"] + "',";
            //sql += "S_UpdateDate='" + d["S_UpdateDate"] + "',";
            //sql += "DName='" + d["DName"] + "',";
            //sql += "DCode='" + d["DCode"] + "',";
            //sql += "DLimit='" + d["DLimit"] + "',";
            //sql += "TaxOffice='" + d["TaxOffice"] + "'";
            //sql += " WHERE S_Id='" + d["S_Id"] + "'";
            //return db.ExecutByStringResult(sql);
            StringBuilder sb = new StringBuilder();
            decimal num = Convert.ToDecimal(d["Proportion"]) / 100;
            sb.Append(" update tax_deduction set ");
            sb.Append(" S_UpdateBy=");
            sb.Append(GetIsNullStr(d["S_UpdateBy"].ToString()) + ", ");
            sb.Append(" S_UpdateDate=");
            sb.Append(GetIsNullStr(d["S_UpdateDate"].ToString()) + ", ");
            sb.Append(" S_OrgCode=");
            sb.Append(GetIsNullStr(d["S_OrgCode"].ToString()) + ", ");
            sb.Append(" S_OrgName=");
            sb.Append(GetIsNullStr(d["S_OrgName"].ToString()) + ", ");
            sb.Append(" DName=");
            sb.Append(GetIsNullStr(d["DName"].ToString()) + ", ");
            sb.Append(" DCode=");
            sb.Append(GetIsNullStr(d["DCode"].ToString()) + ", ");
            sb.Append(" DLimit=");
            sb.Append(string.IsNullOrEmpty(d["DLimit"].ToString()) ? "null" : d["DLimit"].ToString() + ", ");
            sb.Append(" TaxOffice=");
            sb.Append(GetIsNullStr(d["TaxOffice"].ToString()));
            sb.Append(",");
            sb.Append(" Proportion=");
            sb.Append(num);
            sb.Append(",");
            sb.Append(" Pattern=");
            sb.Append(d["Pattern"]);
            sb.Append(" where S_Id=" + GetIsNullStr(d["S_Id"].ToString()));
            return db.ExecutByStringResult(sb.ToString());
        }

        public DataTable judgeCreate(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM tax_deduction WHERE DCode='" + d["DCode"] + "'";
            sql += " and DName='" + d["DName"] + "' and TaxOffice='"+ d["TaxOffice"] + "'";
            return db.GetDataTable(sql);
        }

        public DataTable judgeEdit(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM tax_deduction WHERE DCode='" + d["DCode"] + "'";
            sql += " and DName='" + d["DName"] + "' and TaxOffice='" + d["TaxOffice"] + "'";
            sql += " AND S_Id!='" + d["S_Id"] + "'";
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 根据类型获取字典项
        /// </summary>
        /// <returns></returns>
        public DataTable getItemByType(string parentCode)
        {
            string sql = "select * from tax_dictionary where ParentCode='" + parentCode + "' ORDER BY SortNo";
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 根据科目下拉
        /// </summary>
        /// <returns></returns>
        public DataTable getDCode()
        {
            string sql = "select 序号 id,列名 code,列说明 name from View_TableStructure where 表名='tax_salary' and (列名 like 'A%' or 列名 like 'G%' or 列名 like 'K%') ORDER BY 序号";
            return db.GetDataTable(sql);
        }
        #endregion



        public DataTable getTaxComputeconfig()
        {
            string sql = "SELECT * FROM tax_computeconfig where TermCode not in ('G_DSZNJT','G_DSZNBJFS')";
            return db.GetDataTable(sql);
        }

        public string updateTaxComputeconfig(List<Dictionary<string,object>> arr,Dictionary<string,object> d)
        {
            List<string> SQL = new List<string>();
            foreach(Dictionary<string,object> obj in arr)
            {
                string sql = "UPDATE tax_computeconfig SET TermName='"+ obj["TermName"]+"',";
                sql += "TermCode='" + obj["TermCode"] + "',";
                sql += "MonthCompute=" + obj["MonthCompute"] + ",";
                sql += "MonthProportion='" + obj["MonthProportion"] + "',";
                sql += " S_UpdateBy='" + d["userid"] + "',";
                sql += " S_UpdateDate='" + d["S_UpdateDate"] + "'";
                sql += " WHERE S_Id='" + obj["S_Id"] + "'";
                SQL.Add(sql);
            }
            return db.Executs(SQL);
        }
        /// <summary>
        /// 设置起征点5000
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateTaxDateSub(string userId,decimal QZD)
        {
            string sql = "update  tax_date set QZD="+ QZD + " ,UpdateBy ='"+ userId + "' ,UpdateDate=GETDATE()";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 查询起征点5000
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getTaxDateSub()
        {
            string sql = "select top 1 QZD from tax_date ";
            return db.GetDataTable(sql);
        }
    }
}
