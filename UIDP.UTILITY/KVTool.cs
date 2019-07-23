using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace UIDP.UTILITY
{
    public class KVTool
    {
        public static List<Dictionary<string, object>> TableToListDic(DataTable dt, bool isStoreDB = true)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> dct;
            foreach (DataRow dr in dt.Rows)
            {
                dct = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    try
                    {
                        dct[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    catch
                    {
                        dct[dc.ColumnName] = "";
                    }
                }
                list.Add(dct);
            }

            return list;
        }


        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)//PageIndex表示第几页，PageSize表示每页的记录数
        {
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        public static IList<T> PaginationDataSource<T>(IList<T> list, int pageIndex, int pageSize, out int totals)
        {
            totals = 0;
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex必须大于0");

            if (pageSize <= 0)
                throw new ArgumentException("pageSize必须大于0");


            totals = list.Count;
            int rowBegin = (pageIndex - 1) * pageSize >= totals ? 0 : (pageIndex - 1) * pageSize;
            int rowEnd = rowBegin + pageSize - 1 >= totals ? totals : rowBegin + pageSize - 1;

            IList<T> result = new List<T>();
            for (int i = rowBegin; i < rowEnd; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }


        public static List<Dictionary<string, object>> RowsToListDic(DataTable dt, Dictionary<string, object> d, bool isStoreDB = true )
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> dct;



            int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
            int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
            page = page - 1;

            var curRows = dt.Rows.Cast<DataRow>().Skip(page* limit).Take(limit).ToArray();


            foreach (var dr in curRows)
            {
                dct = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    try
                    {
                        dct[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    catch
                    {
                        dct[dc.ColumnName] = "";
                    }
                }
                list.Add(dct);
            }

            return list;
        }

        /// <summary>  
        /// DataTable转化为List集合  
        /// </summary>  
        /// <typeparam name="T">实体对象</typeparam>  
        /// <param name="dt">datatable表</param>  
        /// <param name="isStoreDB">是否存入数据库datetime字段，date字段没事，取出不用判断</param>  
        /// <returns>返回list集合</returns>  
        public static List<T> TableToList<T>(DataTable dt, bool isStoreDB = true)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //List<string> listColums = new List<string>();  
            PropertyInfo[] pArray = type.GetProperties(); //集合属性数组  
            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>(); //新建对象实例   
                foreach (PropertyInfo p in pArray)
                {
                    if (!dt.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                    {
                        continue;  //DataTable列中不存在集合属性或者字段内容为空则，跳出循环，进行下个循环     
                    }
                    if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    try
                    {
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为集合字段类型    
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception)
                    {
                        // throw;  
                    }
                    //if (row[p.Name].GetType() == p.PropertyType)  
                    //{  
                    //    p.SetValue(entity, row[p.Name], null); //如果不考虑类型异常，foreach下面只要这一句就行  
                    //}                      
                    //object obj = null;  
                    //if (ConvertType(row[p.Name], p.PropertyType,isStoreDB, out obj))  
                    //{                                          
                    //    p.SetValue(entity, obj, null);  
                    //}                  
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>  
        /// List集合转DataTable  
        /// </summary>  
        /// <typeparam name="T">实体类型</typeparam>  
        /// <param name="list">传入集合</param>  
        /// <param name="isStoreDB">是否存入数据库DateTime字段，date时间范围没事，取出展示不用设置TRUE</param>  
        /// <returns>返回datatable结果</returns>  
        public static DataTable ListToTable<T>(List<T> list, bool isStoreDB = true)
        {
            Type tp = typeof(T);
            PropertyInfo[] proInfos = tp.GetProperties();
            DataTable dt = new DataTable();
            foreach (var item in proInfos)
            {
                dt.Columns.Add(item.Name, item.PropertyType); //添加列明及对应类型  
            }
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                foreach (var proInfo in proInfos)
                {
                    object obj = proInfo.GetValue(item);
                    if (obj == null)
                    {
                        continue;
                    }
                    //if (obj != null)  
                    // {  
                    if (isStoreDB && proInfo.PropertyType == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    // dr[proInfo.Name] = proInfo.GetValue(item);  
                    dr[proInfo.Name] = obj;
                    // }  
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>  
        /// table指定行转对象  
        /// </summary>  
        /// <typeparam name="T">实体</typeparam>  
        /// <param name="dt">传入的表格</param>  
        /// <param name="rowindex">table行索引，默认为第一行</param>  
        /// <returns>返回实体对象</returns>  
        public static T TableToEntity<T>(DataTable dt, int rowindex = 0, bool isStoreDB = true)
        {
            Type type = typeof(T);
            T entity = Activator.CreateInstance<T>(); //创建对象实例  
            if (dt == null)
            {
                return entity;
            }
            //if (dt != null)  
            //{  
            DataRow row = dt.Rows[rowindex]; //要查询的行索引  
            PropertyInfo[] pArray = type.GetProperties();
            foreach (PropertyInfo p in pArray)
            {
                if (!dt.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                {
                    continue;
                }

                if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-02"))
                {
                    continue;
                }
                try
                {
                    var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为对象字段类型  
                    p.SetValue(entity, obj, null);
                }
                catch (Exception)
                {
                    // throw;  
                }
                // p.SetValue(entity, row[p.Name], null);                     
            }
            //  }  
            return entity;
        }


        /// <summary>
        /// 反射遍历对象属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        public static void ForeachListProperties<T>(T model)
        {
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                object value = item.GetValue(model, null);
            }
        }
    }
}
