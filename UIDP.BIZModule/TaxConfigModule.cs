using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.BIZModule.Modules;
using UIDP.ODS;

namespace UIDP.BIZModule
{
    public class TaxConfigModule
    {
        TaxConfigDB db = new TaxConfigDB();

        public List<ConfigNode> getLeftTree()
        {
            List<ConfigNode> nodeList = new List<ConfigNode>();
            Tree(nodeList);
            return nodeList;
        }

        public void Tree(List<ConfigNode> nodeList)
        {
            DataTable dt = db.getData();
            foreach(DataRow du in dt.Select("ParentCode is NULL"))
            {
                ConfigNode node = new ConfigNode();
                node.S_Id = du["S_Id"].ToString();
                node.ParentCode = du["ParentCode"].ToString();
                node.Code = du["Code"].ToString();
                node.EnglishCode = du["EnglishCode"].ToString();
                node.SortNo = du["SortNo"].ToString();
                node.Name = du["Name"].ToString();
                node.children = new List<ConfigNode>();
                childTree(dt, node);
                node.children = node.children.OrderBy(t => t.SortNo).ToList();
                nodeList.Add(node);
            }
        }

        public void childTree(DataTable dt,ConfigNode node)
        {
            foreach(DataRow du in dt.Select("ParentCode='" + node.Code + "'"))
            {
                ConfigNode childNode = new ConfigNode();
                childNode.S_Id = du["S_Id"].ToString();
                childNode.ParentCode = du["ParentCode"].ToString();
                childNode.Code = du["Code"].ToString();
                childNode.EnglishCode = du["EnglishCode"].ToString();
                childNode.SortNo = du["SortNo"].ToString();
                childNode.Name = du["Name"].ToString();
                childNode.children = new List<ConfigNode>();
                childTree(dt, childNode);
                childNode.children = childNode.children.OrderBy(t => t.SortNo).ToList();
                node.children.Add(childNode);
            }
        }

        public Dictionary<string,object> editNode(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_UpdateDate"] = DateTime.Now;
            d["S_UpdateBy"] = d["username"];
            try
            {
                string b = db.editNode(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] =b;
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

        public Dictionary<string,object> createNode(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d["S_CreateDate"] = DateTime.Now;
            d["S_CreateBy"] = d["username"];
            d["S_Id"] = Guid.NewGuid();
            try
            {
                DataTable dt = db.getRepeatInfo(d);
                if (dt.Rows.Count == 0)
                {
                    string b = db.createNode(d);
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
                    r["message"] = "编码或名称已存在！";
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
        public Dictionary<string,object> delNode(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.delNode(d);
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
    }
}
