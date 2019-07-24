using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class TaxReportModule
    {
        TaxReportDB DB = new TaxReportDB();
        public Dictionary<string,object> Report(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            // d["S_UpdateDate"] = DateTime.Now;
            // d["S_Id"] = Guid.NewGuid();
            // DataTable dt = DB.IsNullRecord(d);//判断当前单位是否存在上报记录
            // //DataTable du = DB.IsNullDepartment(d);//判断当前单位是否存在人员
            ////if (du.Rows.Count == 0 && dt.Rows.Count == 0)//如果当前单位下没有人员且不存在上报记录，则直接插入一条已上报状态的记录
            // if (dt.Rows.Count == 0)
            // {
            //     string c = DB.InsertRecord(d);
            //     if (c == "")
            //     {
            //         r["message"] = "成功";
            //         r["code"] = 2000;
            //     }
            //     else
            //     {
            //         r["message"] = c;
            //         r["code"] = -1;
            //     }
            // }
            // else
            // {
            //     DataTable m = DB.getSubordinateInfo(d);//根据组织机构表查询当前单位的下级单位是否已经上报
            //     if (m.Rows.Count == 0)
            //     {
            //        string b = DB.Report(d);
            //         if (b == "")
            //         {
            //             r["message"] = "成功";
            //             r["code"] = 2000;
            //         }
            //         else
            //         {
            //             r["message"] = b;
            //             r["code"] = -1;
            //         }
            //     }
            //     else
            //     {
            //         r["message"] = "当前还有下级单位未上报!";
            //         r["code"] = -1;
            //     }
            // }

            d["S_UpdateDate"] = DateTime.Now;
            try
            {
                DataTable dt = DB.IsNullRecord(d);//判断当前单位是否存在上报记录
                DataTable validatedt = DB.AllowReport(d["S_OrgCode"].ToString(), d["S_WorkDate"].ToString());
                DataRow[] childrows = validatedt.Select(" IsComputeTax=1 and ReportStatus<>2 and ORG_CODE<>'" + d["S_OrgCode"].ToString() + "'");
                DataRow[] rows = validatedt.Select(" ORG_CODE='" + d["S_OrgCode"].ToString() + "' and ReportStatus=1 ");
                if (childrows.Length == 0)
                {
                    if (rows.Length == 1)
                    {
                        if (dt.Rows.Count == 0)
                        {

                            d["S_Id"] = Guid.NewGuid();
                            string c = DB.InsertRecord(d);
                            if (c == "")
                            {
                                r["message"] = "成功";
                                r["code"] = 2000;
                            }
                            else
                            {
                                r["message"] = c;
                                r["code"] = -1;
                            }
                        }
                        else
                        {
                            string b = DB.Report(d);
                            if (b == "")
                            {
                                r["message"] = "成功";
                                r["code"] = 2000;
                            }
                            else
                            {
                                r["message"] = b;
                                r["code"] = -1;
                            }
                        }
                    }
                    else
                    {
                        r["message"] = "锁定失败，本单位上报记录异常，请联系管理员！";
                        r["code"] = -1;
                    }
                }
                else
                {
                    r["message"] = "锁定失败，本单位的下级单位存在未上报的记录！";
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> getStatus(string S_OrgCode,string S_WorkDate)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = DB.getStatus(S_OrgCode, S_WorkDate);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = dt;
                }
                else
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = dt;
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = 1;
            }
            return r;
        }

        public Dictionary<string, object> getCalculateData(string OrgCode, DateTime SysOperateDate)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            DataSet ds = DB.GetData(OrgCode, SysOperateDate);
            //DataTable du = DB.IsNull(OrgCode);//查询本部门下是否存在人员，若不存在，则直接允许上报，在上报时判断其下级部门是否全部上报完成
            //if (du.Rows.Count == 0)
            //{

            //    res["TaxStatus"] = 1;
            //    res["TaxPayerCount"] = 0;
            //    res["message"] = "二级单位，允许直接上报";
            //    res["code"] = 2000;
            //}
            //else
            //{
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] rows = dt.Select("ReportStatus=-1 AND IsComputeTax=1");
                    if (rows.Length > 0)
                    {
                        res["TaxStatus"] = -1;
                        res["items"] = dt;
                        res["total"] = dt.Rows.Count;
                        res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                        res["message"] = "成功";
                        res["code"] = 2000;
                        return res;
                    }
                    rows = dt.Select("ReportStatus=0 AND IsComputeTax=1");
                    if (rows.Length > 0)
                    {
                        res["TaxStatus"] = 0;
                        res["items"] = JsonConvert.SerializeObject(dt);
                        res["total"] = dt.Rows.Count;
                        res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                        res["message"] = "成功";
                        res["code"] = 2000;
                        return res;
                    }
                    rows = dt.Select("ReportStatus=1 AND IsComputeTax=1");
                    if (rows.Length > 0)
                    {
                        res["TaxStatus"] = 1;
                        res["items"] = JsonConvert.SerializeObject(dt);
                        res["message"] = "成功";
                        res["total"] = dt.Rows.Count;
                        res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                        res["code"] = 2000;
                        return res;
                    }
                    rows = dt.Select("ReportStatus=2 AND IsComputeTax=1");
                    if (rows.Length > 0)
                    {
                        res["TaxStatus"] = 2;
                        res["items"] = JsonConvert.SerializeObject(dt);
                        res["total"] = dt.Rows.Count;
                        res["TaxPayerCount"] = ds.Tables[1].Rows[0][0];
                        res["message"] = "成功";
                        res["code"] = 2000;
                        return res;
                    }
                }
                else
                {
                    res["TaxStatus"] = -1;
                    res["items"] = JsonConvert.SerializeObject(new DataTable());
                    res["TaxPayerCount"] = 0;
                    res["total"] = 0;
                    res["message"] = "成功";
                    res["code"] = 2000;
                }
            //}
            return res;
        }

        public Dictionary<string, object> getDepartmentStatus(Dictionary<string,object> d)
        {
            int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
            int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable dt = DB.getDepartmentStatus(d);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功";
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["code"] = 2000;
                }
                else
                {
                    r["total"] = 0;
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }



        /// <summary>
        /// 解锁上报功能
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string,object> unlock(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (d["P_OrgCode"].ToString()!="100")
                {
                    d["S_OrgCode"] = d["P_OrgCode"];
                    DataTable dt= DB.IsNullRecord(d);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["ReportStatus"].ToString() == "2")
                        {
                            r["message"] = "解锁失败，本单位记录未解锁，请联系上级单位！";
                            r["code"] = -1;
                            return r;
                        }
                    }
                }
                string b = DB.unlock(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else{
                    r["message"] = b;
                    r["code"] = -1;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        /// <summary>
        /// 锁定上报功能
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> locking(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_OrgCode"] = d["ORG_CODE"];
            DataTable dt = DB.IsNullRecord(d);//判断当前单位是否存在上报记录
            DataTable validatedt = DB.AllowReport(d["S_OrgCode"].ToString(), d["S_WorkDate"].ToString());
            try
            {
                DataRow[] childrows = validatedt.Select(" IsComputeTax=1 and ReportStatus<>2 and ORG_CODE<>'" + d["S_OrgCode"].ToString() + "'");
                DataRow[] rows = validatedt.Select(" ORG_CODE='" + d["S_OrgCode"].ToString() + "' and ReportStatus=1 ");
                if (childrows.Length == 0)
                {
                    if (rows.Length == 1)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            d["S_UpdateDate"] = DateTime.Now;
                            d["S_Id"] = Guid.NewGuid();
                            string c = DB.InsertRecord(d);
                            if (c == "")
                            {
                                r["message"] = "成功";
                                r["code"] = 2000;
                            }
                            else
                            {
                                r["message"] = c;
                                r["code"] = -1;
                            }
                        }
                        else
                        {
                            string b = DB.locking(d);
                            if (b == "")
                            {
                                r["message"] = "成功";
                                r["code"] = 2000;
                            }
                            else
                            {
                                r["message"] = b;
                                r["code"] = -1;
                            }
                        }
                    }
                    else {
                        r["message"] = "锁定失败，本单位上报记录异常，请联系管理员！";
                        r["code"] = -1;
                    }
                }
                else
                {
                    r["message"] = "锁定失败，本单位的下级单位存在未上报的记录！";
                    r["code"] = -1;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
    }
}
