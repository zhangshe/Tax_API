using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UIDP.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UIDP.UTILITY
{
    public class DBTool
    {
        private static  string strMySqlCon;//"server=localhost;user id=root;pwd=root;database=uidp;SslMode=none;charset=UTF8";
        public  IDataBase db;
        //public static IDataBase db2;
        public static string dbType;
        public DBTool(string dd)
        {
            try
            {
                GetStrConn();
                db = new ClsDBFactory(dbType, strMySqlCon).DataBase;
                //db2 = db;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询获取dataset
        /// </summary>
        /// <param name="sql"></param>
        public  DataSet GetDataSet(Dictionary<string, string> data)
        {
            try
            {
                db.Open();
                DataSet ds = new DataSet();
                foreach (var item in data)
                {
                    DataTable dt = new DataTable();
                    db.Fill(dt, item.Value);
                    dt.TableName = item.Key;
                    ds.Tables.Add(dt);
                }
                db.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 查询获取datatable
        /// </summary>
        /// <param name="sql"></param>
        public  DataTable GetDataTable(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                db.Fill(dt, sql);
                db.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }

        /// <summary>
        /// 查询获取string
        /// </summary>
        /// <param name="sql"></param>
        public  string GetString(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                db.Fill(dt, sql);
                db.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        public static string GetValue(string sql)
        {
            IDataBase db2 = new ClsDBFactory(dbType, strMySqlCon).DataBase;
            try
            {
                db2.Open();
                DataTable dt = new DataTable();
                db2.Fill(dt, sql);
                db2.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db2.Close();
            }
        }
        /// <summary>
        /// 执行sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Execut(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                int rows = db.ExecuteSQL(sql);
                db.Close();
                if (rows > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 执行sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecutByStringResult(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                int rows = db.ExecuteSQL(sql);
                db.Close();
                if (rows > 0)
                {
                    return "";
                }
                return "-1";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 执行sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecutByStringResult(Dictionary<string, string> sql)
        {
            try
            {

                db.Open();
                db.BeginTransaction();
                foreach (var str in sql)
                {
                    db.ExecuteSQL(str.Value);
                }
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                return ex.Message.ToString();
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 执行多语句sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Executs(List<string> sql)
        {
            try
            {
                db.Open();
                db.BeginTransaction();
                foreach (var item in sql)
                {
                    db.ExecuteSQL(item);
                }
                db.Commit();
                db.Close();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                return ex.Message;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 执行多语句sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExcuteProcedure(string storedProcName, IDataParameter[] paramenters)
        {
            try
            {
                db.Open();
                object result = db.ExcuteProcedure(storedProcName, paramenters);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 执行多语句sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetProcedure(string storedProcName, IDataParameter[] paramenters)
        {
            try
            {
                db.Open();
                DataSet ds = db.GetProcedure(storedProcName, paramenters);
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 获取链接数据库类型
        /// </summary>
        /// <returns></returns>
        public static void GetStrConn()
        {
            try
            {
                using (System.IO.StreamReader file = System.IO.File.OpenText(System.IO.Directory.GetCurrentDirectory() + "\\DBConfig.json"))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o = (JObject)JToken.ReadFrom(reader);
                        dbType = o["DataType"].ToString();
                        strMySqlCon= o[dbType].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #region MyRegion


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private  string GetStrConn(string ConnType)
        //{
        //    try
        //    {
        //        StreamReader sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + "\\DBConfig.json", Encoding.Default);
        //        String line;
        //        string jsonobj = "";
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            jsonobj = jsonobj + line.ToString();
        //        }
        //        DBConn dbConn = JsonConvert.DeserializeObject<DBConn>(jsonobj);
        //        System.Reflection.PropertyInfo[] properties = dbConn.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        //        foreach (System.Reflection.PropertyInfo item in properties)
        //        {
        //            string name = item.Name;
        //            object value = item.GetValue(dbConn, null);
        //            if (name==ConnType)
        //            {
        //                return value.ToString();
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        #endregion
    }
    public class DBConn
    {
        public string MYSQL { get; set; }
        public string SQLSERVER { get; set; }
        public string ORACLE { get; set; }
    }
}
