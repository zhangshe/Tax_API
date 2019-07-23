using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS;
using UIDP.UTILITY;

namespace UIDP.BIZModule
{
    public class ParamsConfigModule
    {
        ParamsConfigDB db = new ParamsConfigDB();

        /// <summary>
        /// 查询减除项
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,object> getSubtrackStandardConfig ()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getSubtrackStandardConfig();
                if (dt.Rows.Count > 0)
                {
                    r["message"] = "成功";
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "成功";
                    r["total"] = dt.Rows.Count;
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
        /// 修改减除项
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string,object> editConfig(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_UpdateDate"] = DateTime.Now;
            try
            {
                DataTable dt = db.judgeEditTaxLevel(d);
                if (dt.Rows.Count == 0)
                {
                    if (int.Parse(d["TaxStart"].ToString()) < int.Parse(d["TaxEnd"].ToString()))
                    {
                        DataTable du = db.judgeEditRepeat(d);
                        if (du.Rows.Count == 0)
                        {
                            string b = db.editConfig(d);
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
                        else
                        {
                            r["message"] = "您输入的范围有误！";
                            r["code"] = -1;
                        }
                    }
                    else
                    {
                        r["message"] = "您输入的范围有误！";
                        r["code"] = -1;
                    }
                }
                else
                {
                    r["message"] ="您所输入的级数已经存在！";
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
        /// 删除减除项
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>

        public Dictionary<string, object> delConfig(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.delConfig(d);
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
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        /// <summary>
        /// 新增减除项
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> createConfig(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_CreateDate"] = DateTime.Now;
            d["S_Id"] = Guid.NewGuid();
            try
            {
                DataTable dt = db.judgeCreateTaxLevel(d);
                if (dt.Rows.Count == 0)
                {
                    if (int.Parse(d["TaxStart"].ToString()) < int.Parse(d["TaxEnd"].ToString()))
                    {
                        DataTable du = db.judgeCreateRepeat(d);
                        if (du.Rows.Count == 0)
                        {
                            string b = db.createConfig(d);
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
                        else
                        {
                            r["message"] = "您输入的范围有误！";
                            r["code"] = -1;
                        }
                    }
                    else
                    {
                        r["message"] = "您所输入的范围有误！";
                        r["code"] = -1;
                    }
                }
                   
                else
                {
                    r["message"] = "系统中已经存在此级数！";
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
       
        #region 扣减项
        public Dictionary<string, object> getDecreasingConfig(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.getDecreasingConfig(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }


        public Dictionary<string, object> createDecreasingConfig(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_CreateDate"] = DateTime.Now;
            d["S_Id"] = Guid.NewGuid();
            try
            {
                DataTable dt = db.judgeCreate(d);
                if (dt.Rows.Count == 0)
                {
                    string b = db.createDecreasingConfig(d);
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
                else
                {
                r["message"] = "您所输入的编码或名称重复！";
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

        public Dictionary<string, object> delDecreasingConfig(string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.delDecreasingConfig(id);
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
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string, object> editDecreasingConfig(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_UpdateDate"] = DateTime.Now;
            d["S_OrgCode"] = "";
            d["S_OrgName"] = "";
            try
            {
                DataTable dt = db.judgeEdit(d);
                if (dt.Rows.Count == 0)
                {
                    string b = db.editDecreasingConfig(d);
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
                else
                {
                    r["message"] = "您所输入的编码或名称重复！";
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

        /// <summary>
        /// 获取组织配置信息中字典下拉选项
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> getAllDictionary()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            DataTable taxOffice = db.getItemByType("JGSZD");//获取机关所在地
            DataTable dCode = db.getDCode();//获取科目
            try
            {
                r["message"] = "成功";
                r["code"] = 2000;
                r["taxOffice"] = taxOffice;
                r["dCode"] = dCode;
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        #endregion


        public Dictionary<string,object> getTaxComputeconfig(string limit,string page)
        {
            int lit = int.Parse(limit);
            int pa = int.Parse(page);
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getTaxComputeconfig();
                if (dt.Rows.Count != 0)
                {
                    r["message"] = "成功";
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, pa, lit));
                    r["code"] = 2000;
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

        public Dictionary<string,object> updateTaxComputeconfig(List<Dictionary<string,object>> arr)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            Dictionary<string, object> d = new Dictionary<string, object>();
            d = arr[arr.Count - 1];
            arr.RemoveAt(arr.Count - 1);
            d["S_UpdateDate"] = DateTime.Now;
            try
            {
                string b = db.updateTaxComputeconfig(arr,d);
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
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
        /// <summary>
        /// 设置起征点5000
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getTaxDateSub()
        {
            return db.getTaxDateSub();
        }
        /// <summary>
        /// 设置起征点5000
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateTaxDateSub(string userId, decimal QZD)
        {
            return db.updateTaxDateSub(userId, QZD);
        }
    }
}
