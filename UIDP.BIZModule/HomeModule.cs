using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.BIZModule.Modules;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class HomeModule
    {
        HomeDB db = new HomeDB();

        public Dictionary<string,object> getMonthData(string orgcode, string systime)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getMonthData(orgcode, systime);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = 0;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> getNotice(string limit,string page,string id)
        {
            int lim = int.Parse(limit);
            int pa = int.Parse(page);
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getNotice(id);
                if (dt.Rows.Count > 0)
                {
                    DataTable du = db.getNoticeDetail(id);
                    List<noticeModel> noticelist = new List<noticeModel>();
                    foreach(DataRow dr in dt.Select())
                    {
                        noticeModel nm = new noticeModel();
                        nm.NOTICE_ID = dr["NOTICE_ID"].ToString();
                        nm.NOTICE_CODE = dr["NOTICE_CODE"].ToString();
                        nm.NOTICE_TITLE = dr["NOTICE_TITLE"].ToString();
                        nm.NOTICE_CONTENT = dr["NOTICE_CONTENT"].ToString();
                        nm.NOTICE_DATETIME = Convert.ToDateTime(dr["NOTICE_DATETIME"]);
                        nm.NOTICE_ORGID = dr["NOTICE_ORGID"].ToString();
                        nm.NOTICE_ORGNAME = dr["NOTICE_ORGNAME"].ToString();
                        nm.IS_DELETE = int.Parse(dr["IS_DELETE"].ToString());
                        nm.CREATER = dr["CREATER"].ToString();
                        nm.CREATE_DATE = Convert.ToDateTime(dr["CREATE_DATE"].ToString());
                        List<noticeDetailModel> detailList = new List<noticeDetailModel>();
                        if (du.Rows.Count > 0)
                        {
                            foreach(DataRow drr in du.Select("NOTICE_ID='" + nm.NOTICE_ID + "'"))
                            {
                                noticeDetailModel ndm = new noticeDetailModel();
                                ndm.NOTICE_DETAIL_ID = drr["NOTICE_DETAIL_ID"].ToString();
                                ndm.NOTICE_ID = drr["NOTICE_ID"].ToString();
                                ndm.FILE_URL = drr["FILE_URL"].ToString();
                                ndm.FILE_NAME = drr["FILE_NAME"].ToString();
                                ndm.FILE_SIZE = drr["FILE_SIZE"].ToString();
                                detailList.Add(ndm);
                            }
                            nm.children = detailList;
                        }
                        noticelist.Add(nm);
                    }
                    int total = 0;
                    noticelist = (List<noticeModel>)KVTool.PaginationDataSource<noticeModel>(noticelist, pa, lim, out total);
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(noticelist));
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> getLv(string orgcode, string systime)
        {
            //DataTable dt = db.getThreshold();
            
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                //DataTable du = db.getNum(dt, orgcode, systime);
                DataTable du = db.getNum(orgcode, systime);
                if (du.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = du;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> getNoticeDetail(string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getNoticeDetail(id);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = dt;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> CompareData(string orgcode, string systime)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.CompareData(orgcode, systime);
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["items"] = dt;
                }
                else
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

    //    public DataTable Rcc(DataTable dt)
    //    {
    //        DataTable du = dt.DefaultView.ToTable(true, "MM");
    //        DataTable dj = new DataTable();
    //        DataColumn dc = new DataColumn();
    //        dc.ColumnName=
    //    }
    }
}
