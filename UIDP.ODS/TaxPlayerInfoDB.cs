using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class TaxPlayerInfoDB
    {
        DBTool DB = new DBTool("MYSQL");
        /// <summary>
        /// 查询纳税人信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getTaxPlayerInfo(Dictionary<string, object> d)
        {
            string sql = "select a.*,c.Name AS edu,d.Name AS occ,e.Name AS wor,f.Name AS idt,g.Name AS nat,h.Name AS jobt,j.Name AS otheridt  from tax_taxpayerinfo a";
            sql += " LEFT JOIN tax_dictionary c ON a.Education=c.Code";
            sql += " LEFT JOIN tax_dictionary d ON a.Occupation=d.Code";
            sql += " LEFT JOIN tax_dictionary e ON a.WorkPost=e.Code";
            sql += " LEFT JOIN tax_dictionary f ON a.IdType=f.Code";
            sql += " LEFT JOIN tax_dictionary g ON a.Nationality=g.Code";
            sql += " LEFT JOIN tax_dictionary h ON a.JobType=h.Code";
            sql += " LEFT JOIN tax_dictionary j ON a.OtherIdType=j.Code";
            if (d["S_OrgCode"] != null && d["S_OrgCode"].ToString() != "")
            {
                sql += " WHERE S_OrgCode like" + "'"+d["S_OrgCode"].ToString() + "%'";
            }
            if (d["WorkerNumber"] != null && d["WorkerNumber"].ToString() != "")
            {
                sql += "AND WorkerNumber=" +"'"+ d["WorkerNumber"] + "'";
            }
            if (d["WorkerName"] != null && d["WorkerName"].ToString() != "")
            {
                sql += "AND WorkerName="+ "'"+ d["WorkerName"] + "'";
            }
            if (d["IdNumber"] != null && d["IdNumber"].ToString() != "")
            {
                sql += "AND IdNumber="  +"'" + d["IdNumber"] + "'";
            }
            if (d["Education"] != null && d["Education"].ToString() != "")
            {
                sql += "AND Education='"  + d["Education"]+"'";
            }
            if (d["Occupation"] != null && d["Occupation"].ToString() != "")
            {
                sql += "AND Occupation='"  + d["Occupation"]+"'" ;
            }
            if (d["WorkPost"] != null && d["WorkPost"].ToString() != "")
            {
                sql += "AND WorkPost='"  + d["WorkerNumber"]+"'" ;
            }
            if (d["IsDisability"] != null && d["IsDisability"].ToString() != "")
            {
                sql += "AND IsDisability=" + "'" + d["IsDisability"] + "'";
            }
            sql += " order by S_OrgCode";
            return DB.GetDataTable(sql);
        }
        /// <summary>
        /// 新建纳税人信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createTaxPlayerInfo(Dictionary<string,object> d)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tax_taxpayerinfo" +
                "(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,WorkerNumber," +
                "WorkerName,IdType,IdNumber,Nationality,NationalityId,Sex,BirthDate,WorkerStatus,WorkerStatusId," +
                "IsEmployee,Tel,EmployeeDate,IsDisability,Education,Occupation," +
                "WorkPost,DisabilityNo,IsLieShu,LiShuZH,IsAlone,Email,BankName,BankNumber,IsSpecialIndustry," +
                "PostalAddress,Domicile,Remark,IsWorking,QuitDate,IsShareholder,Investment,Province,City,County,Adress_Province,Adress_City,Adress_County," +
                "IsAbroad,BroadName,IsLive,BirthPlace,FirstEntryTime,ThisYearEntryTime,EstimatedDepartureTime,S_Province,S_City,S_County,S_Address,PayPlace,OtherPayPlace,ChinaPost," +
                "UnChinaPost,OfficeTime,TaxpayersNumber,JobType,OtherIdType,OtherIdNumber,L_Province,L_City,L_County,L_Adress,PerInvestment)values('");
            sql.Append(d["S_Id"] == null ? "" : d["S_Id"]);
            sql.Append("','");
            sql.Append(d["S_CreateDate"] == null ? "" : d["S_CreateDate"] );
            sql.Append("','");
            sql.Append(d["S_CreateBy"] == null ? "" : d["S_CreateBy"]);
            sql.Append("','");
            sql.Append(d["S_OrgName"] == null ? "" : d["S_OrgName"] );
            sql.Append("','");
            sql.Append(d["S_OrgCode"] == null ? "" : d["S_OrgCode"]);
            sql.Append("','");
            sql.Append(d["WorkerNumber"] == null ? "" : d["WorkerNumber"] );
            sql.Append("','");
            sql.Append(d["WorkerName"] == null ? "" : d["WorkerName"] );
            sql.Append("','");
            sql.Append(d["IdType"] == null ? "" : d["IdType"]);
            sql.Append("','");
            sql.Append(d["IdNumber"] == null ? "" : d["IdNumber"] );
            sql.Append("','");
            sql.Append(d["Nationality"] == null ? "" : d["Nationality"] );
            sql.Append("',");
            sql.Append(d["NationalityId"] == null ? "" : d["NationalityId"]);
            sql.Append(",");
            sql.Append(d["Sex"] == null ? "" : d["Sex"] );
            sql.Append(",'");
            sql.Append(d["BirthDate"] == null ? "" : d["BirthDate"] );
            sql.Append("','");
            sql.Append(d["WorkerStatus"] == null ? " " : d["WorkerStatus"]);
            sql.Append("',");
            sql.Append(d["WorkerStatus"] == null ? "" : d["WorkerStatus"]);
            sql.Append(",");
            sql.Append(d["IsEmployee"] == null ? "" : d["IsEmployee"] );
            sql.Append(",'");
            sql.Append(d["Tel"] == null ? "" : d["Tel"] );
            sql.Append("','");
            sql.Append(d["EmployeeDate"] == null ? "" : d["EmployeeDate"] );
            sql.Append("',");
            sql.Append(d["IsDisability"] == null ? "" : d["IsDisability"] );
            sql.Append(",'");
            sql.Append(d["Education"].ToString() == "" ? "" : d["Education"] );
            sql.Append("','");
            sql.Append(d["Occupation"].ToString() == "" ? "" : d["Occupation"]);
            sql.Append("','");
            sql.Append(d["WorkPost"].ToString() == "" ? "" : d["WorkPost"] );
            sql.Append("','");
            sql.Append(d["DisabilityNo"]==null ? "" : d["DisabilityNo"]);
            sql.Append("',");
            sql.Append(d["IsLieShu"].ToString() == "" ? "0" : d["IsLieShu"] );
            sql.Append(",'");
            sql.Append(d["LiShuZH"]== null ? "" : d["LiShuZH"]);
            sql.Append("',");
            sql.Append(d["IsAlone"].ToString() == "" ? "0" : d["IsAlone"]);
            sql.Append(",'");
            sql.Append(d["Email"] ==null ? "" : d["Email"] );
            sql.Append("','");
            sql.Append(d["BankName"] == null ? "" : d["BankName"] );
            sql.Append("','");
            sql.Append(d["BankNumber"] == null? "" : d["BankNumber"] );
            sql.Append("',");
            sql.Append(d["IsSpecialIndustry"].ToString() == "" ? "0" : d["IsSpecialIndustry"] );
            sql.Append(",'");
            sql.Append(d["PostalAddress"] == null ? "" : d["PostalAddress"] );
            sql.Append("','");
            sql.Append(d["Domicile"] == null ? "" : d["Domicile"] );
            sql.Append("','");
            sql.Append(d["Remark"] == null ? "" : d["Remark"]);
            sql.Append("',");
            sql.Append(d["IsWorking"].ToString() == "" ? "NULL" : d["IsWorking"]);
            sql.Append(",");
            if (d["QuitDate"].ToString() == "")
            {
                sql.Append("NULL");
                sql.Append(",");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["QuitDate"] == null ? "" : d["QuitDate"]);
                sql.Append("',");
            }
            sql.Append(d["IsShareholder"].ToString() == "" ? "0" : d["IsShareholder"]);
            sql.Append(",'");
            sql.Append(d["Investment"] == null ? "" : d["Investment"]);
            sql.Append("','");
            sql.Append(d["Province"] == null ? "" : d["Province"]);
            sql.Append("','");
            sql.Append(d["City"] == null ? "" : d["City"]);
            sql.Append("','");
            sql.Append(d["County"] == null ? "" : d["County"]);
            sql.Append("','");
            sql.Append(d["Adress_Province"] == null ? "" : d["Adress_Province"]);
            sql.Append("','");
            sql.Append(d["Adress_City"] == null ? "" : d["Adress_City"]);
            sql.Append("','");
            sql.Append(d["Adress_County"] == null ? "" : d["Adress_County"]);
            sql.Append("',");
            sql.Append(d["IsAbroad"].ToString() == "" ? 0 : d["IsAbroad"]);
            sql.Append(",'");
            sql.Append(d["BroadName"] == null ? "" : d["BroadName"]);
            sql.Append("',");
            sql.Append(d["IsLive"].ToString() == "" ? 0 : d["IsLive"]);
            sql.Append(",'");
            sql.Append(d["BirthPlace"] == null ? "" : d["BirthPlace"]);
            sql.Append("',");
            if (d["FirstEntryTime"].ToString() == "")
            {
                sql.Append("NULL");
                sql.Append(",");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["FirstEntryTime"] == null ? "" : d["FirstEntryTime"]);
                sql.Append("',");
            }
            if (d["ThisYearEntryTime"].ToString() == "")
            {
                sql.Append("NULL");
                sql.Append(",");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["ThisYearEntryTime"] == null ? "" : d["ThisYearEntryTime"]);
                sql.Append("',");
            }
            if (d["EstimatedDepartureTime"].ToString() == "")
            {
                sql.Append("NULL");
                sql.Append(",");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["EstimatedDepartureTime"] == null ? "" : d["EstimatedDepartureTime"]);
                sql.Append("',");
            }
            sql.Append("'");
            sql.Append(d["S_Province"] == null ? "" : d["S_Province"]);
            sql.Append("','");
            sql.Append(d["S_City"] == null ? "" : d["S_City"]);
            sql.Append("','");
            sql.Append(d["S_County"] == null ? "" : d["S_County"]);
            sql.Append("','");
            sql.Append(d["S_Address"] == null ? "" : d["S_Address"]);
            sql.Append("','");
            sql.Append(d["PayPlace"] == null ? "" : d["PayPlace"]);
            sql.Append("','");
            sql.Append(d["OtherPayPlace"] == null ? "" : d["OtherPayPlace"]);
            sql.Append("','");
            sql.Append(d["ChinaPost"] == null ? "" : d["ChinaPost"]);
            sql.Append("','");
            sql.Append(d["UnChinaPost"] == null ? "" : d["UnChinaPost"]);
            sql.Append("','");
            sql.Append(d["OfficeTime"] == null ? "" : d["OfficeTime"]);
            sql.Append("','");
            sql.Append(d["TaxpayersNumber"] == null ? "" : d["TaxpayersNumber"]);
            sql.Append("','");
            sql.Append(d["JobType"] == null ? "" : d["JobType"]);
            sql.Append("','");
            sql.Append(d["OtherIdType"] == null ? "" : d["OtherIdType"]);
            sql.Append("','");
            sql.Append(d["OtherIdNumber"] == null ? "" : d["OtherIdNumber"]);
            sql.Append("','");
            sql.Append(d["L_Province"] == null ? "" : d["L_Province"]);
            sql.Append("','");
            sql.Append(d["L_City"] == null ? "" : d["L_City"]);
            sql.Append("','");
            sql.Append(d["L_County"] == null ? "" : d["L_County"]);
            sql.Append("','");
            sql.Append(d["L_Adress"] == null ? "" : d["L_Adress"]);
            sql.Append("',");
            if(d["PerInvestment"]==null|| d["PerInvestment"].ToString() == "")
            {
                sql.Append("NULL)");
            }
            else
            {
                sql.Append(d["PerInvestment"]);
                sql.Append(")");
            }           
            return DB.ExecutByStringResult(sql.ToString());
        }
        /// <summary>
        /// 根据身份查询用户是否存在（新建方法判断用户信息是否重复录入）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable getTaxPlayInfoByCitizenId(string id)
        {
            string sql = "select * from tax_taxpayerinfo where IdNumber='" + id + "'";
            return DB.GetDataTable(sql);
        }
        /// <summary>
        /// 根据工号查询用户是否存在（新建方法使用此方法判断用户信息是否重复）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable getTaxPlayerInfoByWorkerNumber(string id)
        {
            string sql = "select * from tax_taxpayerinfo where WorkerNumber='" + id + "'";
            return DB.GetDataTable(sql);
        }
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string deletePlayerInfo(string id)
        {
            string sql = "delete from tax_taxpayerinfo where S_Id='" + id + "'";
            return DB.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 修改纳税人信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string editPlayerInfo(Dictionary<string,object> d)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE tax_taxpayerinfo SET S_UpdateBy='");
            sql.Append(d["S_UpdateBy"] == null ? "" : d["S_UpdateBy"]);
            sql.Append("',S_UpdateDate='");
            sql.Append(d["S_UpdateDate"] == null ? "" : d["S_UpdateDate"]);
            sql.Append("',S_OrgName='");
            sql.Append(d["S_OrgName"] == null ? "" : d["S_OrgName"]);
            sql.Append("',S_OrgCode='");
            sql.Append(d["S_OrgCode"] == null ? "" : d["S_OrgCode"]);
            sql.Append("',WorkerNumber='");
            sql.Append(d["WorkerNumber"] == null ? "" : d["WorkerNumber"]);
            sql.Append("',WorkerName='");
            sql.Append(d["WorkerName"] == null ? "" : d["WorkerName"]);
            sql.Append("',IdType='");
            sql.Append(d["IdType"] == null ? "" : d["IdType"]);
            sql.Append("',IdNumber='");
            sql.Append(d["IdNumber"] == null ? "" : d["IdNumber"]);
            sql.Append("',Nationality='");
            sql.Append(d["Nationality"] == null ? "" : d["Nationality"]);
            sql.Append("',NationalityId=");
            sql.Append(d["NationalityId"] == null ? "0" : d["NationalityId"]);
            sql.Append(",Sex=");
            sql.Append(d["Sex"] == null ? "" : d["Sex"]);
            sql.Append(",BirthDate='");
            sql.Append(d["BirthDate"] == null ? "" : d["BirthDate"]);
            sql.Append("',WorkerStatus='");
            sql.Append(d["WorkerStatus"] == null ? " " : d["WorkerStatus"]);
            sql.Append("',WorkerStatusId=");
            sql.Append(d["WorkerStatus"] == null ? "0" : d["WorkerStatus"]);
            sql.Append(",IsEmployee=");
            sql.Append(d["IsEmployee"] == null ? "" : d["IsEmployee"]);
            sql.Append(",Tel='");
            sql.Append(d["Tel"] == null ? "" : d["Tel"]);
            sql.Append("',EmployeeDate='");
            sql.Append(d["EmployeeDate"] == null ? "" : d["EmployeeDate"]);
            sql.Append("',IsDisability=");
            sql.Append(d["IsDisability"] == null ? "" : d["IsDisability"]);
            sql.Append(",Education='");
            sql.Append(d["Education"]== null ? "" : d["Education"]);
            sql.Append("',Occupation='");
            sql.Append(d["Occupation"] == null ? "" : d["Occupation"]);
            sql.Append("',WorkPost='");
            sql.Append(d["WorkPost"]==null ? "" : d["WorkPost"]);
            sql.Append("',DisabilityNo='");
            sql.Append(d["DisabilityNo"] == null ? "" : d["DisabilityNo"]);
            sql.Append("',IsLieShu=");
            sql.Append(d["IsLieShu"]== null ? "0" : d["IsLieShu"]);
            sql.Append(",LiShuZH='");
            sql.Append(d["LiShuZH"] == null ? "" : d["LiShuZH"]);
            sql.Append("',IsAlone=");
            sql.Append(d["IsAlone"] == null ? "0" : d["IsAlone"]);
            sql.Append(",Email='");
            sql.Append(d["Email"] == null ? "" : d["Email"]);
            sql.Append("',BankName='");
            sql.Append(d["BankName"] == null ? "" : d["BankName"]);
            sql.Append("',BankNumber='");
            sql.Append(d["BankNumber"] == null ? "" : d["BankNumber"]);
            sql.Append("',IsSpecialIndustry=");
            sql.Append(d["IsSpecialIndustry"] == null ? "0" : d["IsSpecialIndustry"]);
            sql.Append(",PostalAddress='");
            sql.Append(d["PostalAddress"] == null ? "" : d["PostalAddress"]);
            sql.Append("',Domicile='");
            sql.Append(d["Domicile"] == null ? "" : d["Domicile"]);
            sql.Append("',Remark='");
            sql.Append(d["Remark"] == null ? "" : d["Remark"]);
            sql.Append("',IsWorking=");
            sql.Append(d["IsWorking"]== null ? 0 : d["IsWorking"]);
            sql.Append(",QuitDate=");
            if (d["QuitDate"] == null)
            {
                sql.Append("NULL");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["QuitDate"]);
                sql.Append("'");
            }
            sql.Append(",IsShareholder=");
            sql.Append(d["IsShareholder"]== null ? 0 : d["IsShareholder"]);
            sql.Append(",Investment='");
            sql.Append(d["Investment"] == null ? "" : d["Investment"]);
            sql.Append("',Province='");
            sql.Append(d["Province"] == null ? "" : d["Province"]);
            sql.Append("',City='");
            sql.Append(d["City"] == null ? "" : d["City"]);
            sql.Append("',County='");
            sql.Append(d["County"] == null ? "" : d["County"]);
            sql.Append("',Adress_Province='");
            sql.Append(d["Adress_Province"] == null ? "" : d["Adress_Province"]);
            sql.Append("',Adress_City='");
            sql.Append(d["Adress_City"] == null ? "" : d["Adress_City"]);
            sql.Append("',Adress_County='");
            sql.Append(d["Adress_County"] == null ? "" : d["Adress_County"]);
            sql.Append("',IsAbroad=");
            sql.Append(d["IsAbroad"]== null ? 0 : d["IsAbroad"]);
            sql.Append(",BroadName='");
            sql.Append(d["BroadName"] == null ? "" : d["BroadName"]);
            sql.Append("',IsLive=");
            sql.Append(d["IsLive"]==null ? 0 : d["IsLive"]);
            sql.Append(",BirthPlace='");
            sql.Append(d["BirthPlace"] == null ? "" : d["BirthPlace"]);
            sql.Append("',FirstEntryTime=");
            if (d["FirstEntryTime"] == null)
            {
                sql.Append("NULL");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["FirstEntryTime"]);
                sql.Append("'");
            }
            sql.Append(",ThisYearEntryTime=");
            if (d["ThisYearEntryTime"] == null)
            {
                sql.Append("NULL");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["ThisYearEntryTime"]);
                sql.Append("'");
            }
            sql.Append(",EstimatedDepartureTime=");
            if (d["EstimatedDepartureTime"] == null)
            {
                sql.Append("NULL");
            }
            else
            {
                sql.Append("'");
                sql.Append(d["EstimatedDepartureTime"]);
                sql.Append("'");
            }
            sql.Append(",S_Province='");
            sql.Append(d["S_Province"] == null ? "" : d["S_Province"]);
            sql.Append("',S_City='");
            sql.Append(d["S_City"] == null ? "" : d["S_City"]);
            sql.Append("',S_County='");
            sql.Append(d["S_County"] == null ? "" : d["S_County"]);
            sql.Append("',S_Address='");
            sql.Append(d["S_Address"] == null ? "" : d["S_Address"]);
            sql.Append("',PayPlace='");
            sql.Append(d["PayPlace"] == null ? "" : d["PayPlace"]);
            sql.Append("',OtherPayPlace='");
            sql.Append(d["OtherPayPlace"] == null ? "" : d["OtherPayPlace"]);
            sql.Append("',ChinaPost='");
            sql.Append(d["ChinaPost"] == null ? "" : d["ChinaPost"]);
            sql.Append("',UnChinaPost='");
            sql.Append(d["UnChinaPost"] == null ? "" : d["UnChinaPost"]);
            sql.Append("',OfficeTime='");
            sql.Append(d["OfficeTime"] == null ? "" : d["OfficeTime"]);
            sql.Append("',TaxpayersNumber='");
            sql.Append(d["TaxpayersNumber"] == null ? "" : d["TaxpayersNumber"]);
            sql.Append("',JobType='");
            sql.Append(d["JobType"] == null ? "" : d["JobType"]);
            sql.Append("',OtherIdType='");
            sql.Append(d["OtherIdType"] == null ? "" : d["OtherIdType"]);
            sql.Append("',OtherIdNumber='");
            sql.Append(d["OtherIdNumber"] == null ? "" : d["OtherIdNumber"]);
            sql.Append("',L_Province='");
            sql.Append(d["L_Province"] == null ? "" : d["L_Province"]);
            sql.Append("',L_City='");
            sql.Append(d["L_City"] == null ? "" : d["L_City"]);
            sql.Append("',L_County='");
            sql.Append(d["L_County"] == null ? "" : d["L_County"]);
            sql.Append("',L_Adress='");
            sql.Append(d["L_Adress"] == null ? "" : d["L_Adress"]);
            sql.Append("',PerInvestment=");
            if(d["PerInvestment"] ==null|| d["PerInvestment"].ToString() == "")
            {
                sql.Append("NULL ");
            }
            else
            {
                sql.Append(d["PerInvestment"]);
            }
            sql.Append(" WHERE S_Id='");
            sql.Append(d["S_Id"]);
            sql.Append("'");
            return DB.ExecutByStringResult(sql.ToString());
        }
        /// <summary>
        /// 获取国籍选项
        /// </summary>
        /// <returns></returns>
        public DataTable getNationally()
        {
            string sql = "select * from tax_dictionary where ParentCode='GJ' ORDER BY Code,SortNo";
            
            return DB.GetDataTable(sql);
        }
        /// <summary>
        /// 获取证件类型
        /// </summary>
        /// <returns></returns>
        public DataTable getIdType()
        {
            string sql= "select * from tax_dictionary where ParentCode='ZJLX'ORDER BY Code,SortNo";
            return DB.GetDataTable(sql);
        }

        public DataTable getEducation()
        {
            string sql = "select * from tax_dictionary where ParentCode='XL'ORDER BY Code,SortNo";
            return DB.GetDataTable(sql);
        }
        public DataTable getJob()
        {
            string sql = "select * from tax_dictionary where ParentCode='ZY'ORDER BY Code,SortNo";
            return DB.GetDataTable(sql);
        }
        public DataTable getWorkPost()
        {
            string sql = "select * from tax_dictionary where ParentCode='ZW'ORDER BY Code,SortNo";
            return DB.GetDataTable(sql);
        }
        public DataTable getOtherIdType()
        {
            string sql = "select * from tax_dictionary where ParentCode='QTZZLX'ORDER BY Code,SortNo";
            return DB.GetDataTable(sql);
        }

        public DataTable getJobType()
        {
            string sql = "select * from tax_dictionary where ParentCode='RZSGCYLX' ORDER BY Code, SortNo";
            return DB.GetDataTable(sql);
        }

        /// <summary>
        /// 查询纳税人变动情况（统计人数）
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getTaxPlayerChange(Dictionary<string, object> d)
        {
            StringBuilder SQL = new StringBuilder();
            DateTime dt = Convert.ToDateTime(d["ImportMonth"].ToString());
            SQL.Append("SELECT S_OrgCode,");
            SQL.Append("ImportMonth");
            // SQL.Append(", SUM(CASE WHEN WorkerStatus = 0 then - 1 else 1 end) AS CHANGPEOPLE, ");
            SQL.Append(", SUM(CASE WHEN WorkerStatus IS NOT NULL THEN 1 ELSE 1 END) AS CHANGPEOPLE, ");
            SQL.Append("( SELECT COUNT ( S_Id ) FROM tax_salary WHERE DATEDIFF( mm,'");
            SQL.Append(dt.ToString("yyyyMMdd"));
            SQL.Append("',S_WorkDate)=0 AND S_OrgCode='"+d["S_OrgCode"]+"'");
            SQL.Append(") AS COUNTNUMBER,");
            SQL.Append("ts_uidp_org.ORG_NAME");
            SQL.Append(" FROM tax_taxpayerrecord");
            SQL.Append(" LEFT JOIN ts_uidp_org ON tax_taxpayerrecord.S_OrgCode= ts_uidp_org.ORG_CODE");
            SQL.Append(" WHERE S_OrgCode LIKE'");
            SQL.Append(d["S_OrgCode"]+"%'");
            SQL.Append("AND DATEDIFF( mm, '");
            SQL.Append(d["ImportMonth"]);
            SQL.Append("',ImportMonth)=0");
            SQL.Append(" GROUP BY S_OrgCode,ImportMonth,ts_uidp_org.ORG_NAME");
            string sql = SQL.ToString();
            return DB.GetDataTable(sql);
            //string sql = "SELECT S_OrgCode,ImportMonth,SUM(CASE WHEN WorkerStatus = 0 then -1 else 1 end) AS CHANGPEOPLE,(SELECT COUNT(S_Id)  FROM tax_salary WHERE S_CreateDate BETWEEN";
            //sql += "'" + d["ImportMonth"] + "'";
            //sql += "AND  DATEADD(mm,1,'";
            //sql += d["ImportMonth"] + "'";
            //sql += ")) AS COUNTNUMBER FROM tax_taxpayerrecord where S_OrgCode LIKE '";
            //sql += d["S_OrgCode"] + "%'";
            //sql += "AND ImportMonth BETWEEN'";
            //sql += d["ImportMonth"] + "'";
            //sql += "AND DATEADD(mm,1,'";
            //sql += d["ImportMonth"] + "')";
            //sql += "GROUP BY S_OrgCode,ImportMonth";
        }

        public DataTable getTaxContent(Dictionary<string, object> d)
        {
            DateTime dt = Convert.ToDateTime(d["ImportMonth"].ToString());
            string sql = "select * from tax_taxpayerrecord where S_OrgCode='";
            sql += d["S_OrgCode"] + "'";
            sql += "and ";
            sql += "DATEDIFF( mm,ImportMonth,'" + dt.ToString("yyyyMMdd") + "')=0";
            sql += " order by WorkerStatus";
            return DB.GetDataTable(sql);
        }


        public DataTable getTaxpayer (string OrgCode,string systime,string name,string IDNumber, string WorkerNumber)
        {
            string sql = "SELECT DISTINCT a.*,c.Name AS edu,d.Name AS occ,e.Name AS wor,f.Name AS idt,g.Name AS nat FROM tax_taxpayerinfo a ";
            sql += " LEFT JOIN tax_salary b ON a.WorkerNumber=b.S_WorkerCode";
            sql += " LEFT JOIN tax_dictionary c ON a.Education=c.Code";
            sql += " LEFT JOIN tax_dictionary d ON a.Occupation=c.Code";
            sql += " LEFT JOIN tax_dictionary e ON a.WorkPost=e.Code";
            sql += " LEFT JOIN tax_dictionary f ON a.IdType=f.Code";
            sql += " LEFT JOIN tax_dictionary g ON a.Nationality=g.Code";
            //sql += " WHERE DATEDIFF(mm, b.S_WorkDate, '";
            //sql += systime + "')=0";
            sql += " WHERE a.S_OrgCode LIKE '";
            sql += OrgCode + "%'";
            if (IDNumber != null)
            {
                sql += " AND a.IdNumber='";
                sql += IDNumber + "'";
            }
            if (WorkerNumber != null)
            {
                sql += " AND a.WorkerNumber='";
                sql += WorkerNumber + "'";
            }
            if (name != null)
            {
                sql += " AND a.WorkerName='";
                sql += name + "'";
            }
            sql += " order by a.S_OrgCode";
            return DB.GetDataTable(sql);
        }

        public DataTable getPayerSalary(string systime,string id)
        {
            string sql = "SELECT a.*,c.ImportModel FROM tax_salary a ";
            sql += "LEFT JOIN tax_taxpayerinfo b ON a.S_WorkerCode=b.WorkerNumber ";
            sql += " LEFT JOIN tax_org c ON a.S_OrgCode= c.S_OrgCode ";
            sql += "WHERE b.S_Id='" + id + "'";
            sql += "AND DATEDIFF(YY, a.S_WorkDate, '" + systime + "')=0";
            sql += " ORDER BY a.S_WorkDate desc";
            return DB.GetDataTable(sql);
        }


        public string createTaxPayerInfo(List<ImportTaxPayerInfo> list,string userId)
        {
            //return DB.GetDataTable(sql);
            int errorNum = 1;//错误行数
            DataTable taxpayerdt = DB.GetDataTable(" select WorkerNumber from tax_taxpayerinfo ");
            DataTable dictiondt = DB.GetDataTable(" SELECT * FROM tax_dictionary WHERE ParentCode='ZJLX' OR ParentCode='GJ' OR ParentCode='XL' OR ParentCode='ZW' " +
                "OR ParentCode='ZY' OR ParentCode='QTZJLX' OR ParentCode='RZSGCYLX'");
            List<string> sqllst = new List<string>();
            string fengefu = "";
            string delsql = "DELETE FROM tax_taxpayerinfo WHERE WorkerNumber='";
            string headerSQL = "INSERT INTO tax_taxpayerinfo(S_Id,S_CreateDate,S_CreateBy,S_OrgName,WorkerNumber,WorkerName,IdType,IdNumber,Nationality,Sex,WorkerStatus,WorkerStatusId,BirthDate,JobType," +
                "Tel,EmployeeDate,QuitDate,IsDisability,IsLieShu,IsAlone,DisabilityNo,LiShuZH,Investment,PerInvestment,Remark,IsAbroad,BroadName,BirthPlace,FirstEntryTime,EstimatedDepartureTime,OtherIdType," +
                "OtherIdNumber,Province,City,County,Domicile,Adress_Province,Adress_City,Adress_County,PostalAddress,L_Province,L_City,L_County,L_Adress,Email,Education,BankName,BankNumber,WorkPost)VALUES(";
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            try
            {
                foreach (ImportTaxPayerInfo item in list)
                {
                    sb.Append(headerSQL);
                    DataRow[] rows = taxpayerdt.Select("WorkerNumber='" + item.WorkerNumber + "'");
                    if (rows.Length > 0)
                    {
                        //sqllst.Add("delete from tax_taxpayerinfo where WorkerNumber='" + item.WorkerNumber + "'");
                        sb1.Append(delsql);
                        sb1.Append(item.WorkerNumber + "' ");
                    }
                    DataRow[] GJ = dictiondt.Select("Name='" + item.Nationality + "'");
                    DataRow[] ZJLX = dictiondt.Select("Name='" + item.IdType + "'");
                    DataRow[] XL = dictiondt.Select("Name='" + item.Education + "'");
                    DataRow[] ZW = dictiondt.Select("Name='" + item.WorkPost + "'");
                    DataRow[] QTZJLX = dictiondt.Select("Name='" + item.OtherIdType + "'");
                    DataRow[] RZSGLX = dictiondt.Select("Name='" + item.JobType + "'");

                    if (!string.IsNullOrEmpty(item.IdType))
                    {
                        if (ZJLX.Length > 0)
                        {
                            item.IdTypeCode = ZJLX[0]["Code"].ToString();
                        }

                        //item.IdTypeCode = ZJLX[0]["Code"].ToString();
                    }
                    if (!string.IsNullOrEmpty(item.Nationality))
                    {
                        //item.NationalityId = GJ[0]["Code"].ToString();
                        if (GJ.Length > 0)
                        {
                            item.NationalityId = GJ[0]["Code"].ToString();
                        }

                    }
                    if (!string.IsNullOrEmpty(item.Education))
                    {
                        //item.EducationCode = XL[0]["Code"].ToString();
                        if (XL.Length > 0)
                        {
                            item.EducationCode = XL[0]["Code"].ToString();
                        }

                    }
                    if (!string.IsNullOrEmpty(item.WorkPost))
                    {
                        //item.WorkPostCode = ZW[0]["Code"].ToString();
                        if (ZW.Length > 0)
                        {
                            item.WorkPostCode = ZW[0]["Code"].ToString();
                        }

                    }
                    if (!string.IsNullOrEmpty(item.OtherIdType))
                    {
                        //item.OtherIdTypeCode = QTZJLX[0]["Code"].ToString();
                        if (QTZJLX.Length > 0)
                        {
                            item.OtherIdTypeCode = QTZJLX[0]["Code"].ToString();
                        }

                    }
                    if (!string.IsNullOrEmpty(item.JobType))
                    {
                        //item.JobTypeCode = RZSGLX[0]["Code"].ToString();
                        if (RZSGLX.Length > 0)
                        {
                            item.JobTypeCode = RZSGLX[0]["Code"].ToString();
                        }
                    }
                    sb.Append("'" + Guid.NewGuid() + "',");
                    sb.Append("'" + DateTime.Now + "',");
                    sb.Append("'" + userId + "',");
                    sb.Append(defaultNull(item.S_OrgName) + ",");
                    sb.Append(defaultNull(item.WorkerNumber) + ",");
                    sb.Append(defaultNull(item.WorkerName) + ",");
                    sb.Append(defaultNull(item.IdTypeCode) + ",");
                    sb.Append(defaultNull(item.IdNumber) + ",");
                    sb.Append(defaultNull(item.NationalityId) + ",");
                    sb.Append(defaultZero(item.Sex) + ",");
                    sb.Append(defaultNull(item.WorkerStatus) + ",");
                    sb.Append(Convert.ToInt32(item.WorkerStatus) + ",");
                    sb.Append(defaultNull(item.BirthDate) + ",");
                    sb.Append(defaultNull(item.JobTypeCode) + ",");
                    sb.Append(defaultNull(item.Tel) + ",");
                    sb.Append(defaultNull(item.EmployeeDate) + ",");
                    sb.Append(defaultNull(item.QuitDate) + ",");
                    sb.Append(Convert.ToInt32(item.IsDisability) + ",");
                    sb.Append(Convert.ToInt32(item.IsLieShu) + ",");
                    sb.Append(Convert.ToInt32(item.IsAlone) + ",");
                    sb.Append(defaultNull(item.DisabilityNo) + ",");
                    sb.Append(defaultNull(item.LiShuZH) + ",");
                    sb.Append(defaultNull(item.Investment) + ",");
                    sb.Append(item.PerInvestment + ",");
                    sb.Append(defaultNull(item.Remark) + ",");
                    sb.Append(Convert.ToInt32(item.IsAbroad) + ",");
                    sb.Append(defaultNull(item.BroadName) + ",");
                    sb.Append(defaultNull(item.BirthPlace) + ",");
                    sb.Append(defaultNull(item.FirstEntryTime) + ",");
                    sb.Append(defaultNull(item.EstimatedDepartureTime) + ",");
                    sb.Append(defaultNull(item.OtherIdTypeCode) + ",");
                    sb.Append(defaultNull(item.OtherIdNumber) + ",");
                    sb.Append(defaultNull(item.Province) + ",");
                    sb.Append(defaultNull(item.City) + ",");
                    sb.Append(defaultNull(item.County) + ",");
                    sb.Append(defaultNull(item.Domicile) + ",");
                    sb.Append(defaultNull(item.Adress_Province) + ",");
                    sb.Append(defaultNull(item.Adress_City) + ",");
                    sb.Append(defaultNull(item.Adress_County) + ",");
                    sb.Append(defaultNull(item.PostalAddress) + ",");
                    sb.Append(defaultNull(item.L_Province) + ",");
                    sb.Append(defaultNull(item.L_City) + ",");
                    sb.Append(defaultNull(item.L_County) + ",");
                    sb.Append(defaultNull(item.L_Adress) + ",");
                    sb.Append(defaultNull(item.Email) + ",");
                    sb.Append(defaultNull(item.EducationCode) + ",");
                    sb.Append(defaultNull(item.BankName) + ",");
                    sb.Append(defaultNull(item.BankNumber) + ",");
                    sb.Append(defaultNull(item.WorkPostCode) + ")");
                    fengefu = ",";
                    errorNum++;
                }
                string sql = sb.ToString();
                string sql1 = sb1.ToString();
                if (sql1.Length > 0)
                {
                    sqllst.Add(sql1);
                }
                sqllst.Add(sql);
            }
            catch (Exception e)
            {
                return "转换失败，第" + errorNum + "条";
            }
            #region 以下是老模板的insert语句
            //sb.Append(@" insert into tax_taxpayerinfo (S_Id,S_CreateDate,S_CreateBy,WorkerNumber,WorkerName,S_OrgName,IdType,IdNumber,Nationality,Sex,BirthDate,WorkerStatus,
            //                    IsEmployee,Tel,EmployeeDate,IsDisability,Education,Occupation,WorkPost,DisabilityNo,IsLieShu,LiShuZH,IsAlone,Email,BankName,BankNumber,
            //                    IsSpecialIndustry,PostalAddress,Domicile,Remark,IsWorking,QuitDate,IsShareholder,Investment,Province,City,County,Adress_Province,Adress_City,Adress_County,
            //                    IsAbroad,BroadName,IsLive,BirthPlace,FirstEntryTime,ThisYearEntryTime,EstimatedDepartureTime,S_Province,S_City,S_County,S_Address,PayPlace,
            //                    OtherPayPlace,ChinaPost,UnChinaPost,OfficeTime,TaxpayersNumber) values ");
            //foreach (ImportTaxPayerInfo item in list)
            //{
            //    DataRow[] rows = taxpayerdt.Select("WorkerNumber='" + item.WorkerNumber + "'");
            //    if (rows.Length > 0)
            //    {
            //        sqllst.Add("delete from tax_taxpayerinfo where WorkerNumber='" + item.WorkerNumber + "'");
            //    }
            //    DataRow[] GJ = dictiondt.Select("Name='" + item.Nationality + "'");
            //    DataRow[] ZJLX = dictiondt.Select("Name='" + item.IdType + "'");
            //    DataRow[] XL = dictiondt.Select("Name='" + item.Education + "'");
            //    //DataRow[] ZY = dictiondt.Select("Name='" + item.Occupation + "'");
            //    DataRow[] ZW = dictiondt.Select("Name='" + item.WorkPost + "'");
            //    if (!string.IsNullOrEmpty(item.IdType)){
            //        item.IdTypeCode = ZJLX[0]["Code"].ToString();
            //    }
            //    if (!string.IsNullOrEmpty(item.Nationality))
            //    {
            //        item.NationalityId = GJ[0]["Code"].ToString();
            //    }
            //    if (!string.IsNullOrEmpty(item.Education))
            //    {
            //        item.EducationCode = XL[0]["Code"].ToString();
            //    }
            //    //if (!string.IsNullOrEmpty(item.Occupation))
            //    //{
            //    //    item.OccupationCode = ZY[0]["Code"].ToString();
            //    //}
            //    if (!string.IsNullOrEmpty(item.WorkPost))
            //    {
            //        item.WorkPostCode = ZW[0]["Code"].ToString();
            //    }                  
            //    sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
            //    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',");
            //    sb.Append(defaultNull(userId) + ",");
            //    sb.Append(defaultNull(item.WorkerNumber) + ",");
            //    sb.Append(defaultNull(item.WorkerName) + ",");
            //    sb.Append(defaultNull(item.S_OrgName) + ",");
            //    //sb.Append("'" + orgCode + "',");
            //    sb.Append(defaultNull(item.IdTypeCode) + ",");
            //    sb.Append(defaultNull(item.IdNumber) + ",");
            //    sb.Append(defaultNull(item.NationalityId) + ",");
            //    sb.Append(item.Sex + ",");
            //    sb.Append(defaultNull(item.BirthDate) + ",");
            //    sb.Append(defaultOne(item.WorkerStatus) + ",");
            //    sb.Append(defaultZero(item.IsEmployee) + ",");
            //    sb.Append(defaultNull(item.Tel) + ",");
            //    sb.Append(defaultNull(item.EmployeeDate)+ ",");
            //    sb.Append(defaultZero(item.IsDisability) + ",");
            //    sb.Append(defaultNull(item.EducationCode) + ",");
            //    sb.Append(defaultNull(item.OccupationCode) + ",");
            //    sb.Append(defaultNull(item.WorkPostCode) + ",");
            //    sb.Append(defaultNull(item.DisabilityNo) + ",");
            //    sb.Append(defaultZero(item.IsLieShu) + ",");
            //    sb.Append(defaultNull(item.LiShuZH) + ",");
            //    sb.Append(defaultZero(item.IsAlone) + ",");
            //    sb.Append(defaultNull(item.Email) + ",");
            //    sb.Append(defaultNull(item.BankName) + ",");
            //    sb.Append(defaultNull(item.BankNumber) + ",");
            //    sb.Append(defaultZero(item.IsSpecialIndustry) + ",");
            //    sb.Append(defaultNull(item.PostalAddress) + ",");
            //    sb.Append(defaultNull(item.Domicile) + ",");
            //    sb.Append(defaultNull(item.Remark) + ",");
            //    sb.Append(defaultZero(item.IsWorking) + ",");
            //    sb.Append(defaultNull(item.QuitDate) + ",");
            //    sb.Append(defaultZero(item.IsShareholder) + ",");
            //    sb.Append(defaultNull(item.Investment) + ",");
            //    sb.Append(defaultNull(item.Province) + ",");
            //    sb.Append(defaultNull(item.City) + ",");
            //    sb.Append(defaultNull(item.County) + ",");
            //    sb.Append(defaultNull(item.Adress_Province) + ",");
            //    sb.Append(defaultNull(item.Adress_City) + ",");
            //    sb.Append(defaultNull(item.Adress_County) + ",");
            //    sb.Append(defaultZero(item.IsAbroad)+ ",");
            //    sb.Append(defaultNull(item.BroadName) + ",");
            //    sb.Append(defaultZero(item.IsLive) + ",");
            //    sb.Append(defaultNull(item.BirthPlace) + ",");
            //    sb.Append(defaultNull(item.FirstEntryTime)+ ",");
            //    sb.Append(defaultNull(item.ThisYearEntryTime) + ",");
            //    sb.Append(defaultNull(item.EstimatedDepartureTime) + ",");
            //    sb.Append(defaultNull(item.S_Province) + ",");
            //    sb.Append(defaultNull(item.S_City) + ",");
            //    sb.Append(defaultNull(item.S_County) + ",");
            //    sb.Append(defaultNull(item.S_Address) + ",");
            //    sb.Append(defaultNull(item.PayPlace) + ",");
            //    sb.Append(defaultNull(item.OtherPayPlace) + ",");
            //    sb.Append(defaultNull(item.ChinaPost) + ",");
            //    sb.Append(defaultNull(item.UnChinaPost) + ",");
            //    sb.Append(defaultZero(item.OfficeTime) + ",");
            //    sb.Append(defaultNull(item.TaxpayersNumber)+")");
            //    fengefu = ",";
            //}
            #endregion
            #region 很早就注释了
            ////string sqldel = "delete from tax_reportstatus where datediff(m,S_WorkDate,'" + dateMonth.ToString("yyyy-MM-dd") + "')=0 and S_OrgCode ='" + orgCode + "'";
            ////sqllst.Add(sqldel);
            ////string insertRecord = "insert into tax_reportstatus(S_Id,S_CreateDate,S_CreateBy,S_OrgName,S_OrgCode,ReportStatus,S_WorkDate) values('" + Guid.NewGuid().ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + userId + "','" + orgName + "','" + orgCode + "',0,'" + dateMonth.ToString("yyyy-MM-dd") + "')";
            ////sqllst.Add(insertRecord);
            #endregion

            string upsql = @"update a set a.S_OrgCode = b.ORG_CODE from tax_taxpayerinfo a,ts_uidp_org b
                where a.S_OrgName = b.ORG_NAME";
            sqllst.Add(upsql);
            return DB.Executs(sqllst);
            //return db.ExecutByStringResult(sql);
        }

        public DataTable getPayerInfoList()
        {
            string sql = " SELECT WorkerNumber,WorkerName FROM tax_taxpayerinfo";
            return DB.GetDataTable(sql);
        }
        public string defaultNull(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return "'" + value + "'";
            }
            return "null";
        }
        public string defaultZero(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            return "0";
        }
        public string defaultOne(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return "'" + value + "'";
            }
            return "1";
        }

        /// <summary>
        /// 根据字典下拉值获取编码
        /// </summary>
        /// <returns></returns>
        public static string getDictionaryCode(string name, string type)
        {
            string value = "";
            string sql = "select Code from tax_dictionary where ParentCode='" + type + "' and Name='" + name + "'";
            value = DBTool.GetValue(sql);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return value;
        }
    }
}
