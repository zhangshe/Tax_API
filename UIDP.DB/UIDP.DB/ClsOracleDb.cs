// 工程名:数据库访问接口
// 模块编号：数据访问层
// 作用：用于提供对Oracle数据库引擎的访问
// 作者：张凡
// 编写日期：2005-11-01

using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace UIDP.DB
{

	/// <summary>
	/// ClsOracleDb 的摘要说明。
	/// </summary>
	public class ClsOracleDb:IDataBase
	{
		private string m_ParamPlaceholder;	//参数的占位符
		private OracleConnection m_Connection;
		private OracleTransaction m_Transaction;
		private string m_ConnectionString = "";
        private int m_ConnectionTimeout = 600;
		private string m_Database = "";
		private ConnectionState m_ConnectionState = ConnectionState.Closed;
        private DBTYPE m_dbType = DBTYPE.ORACLE;

        /// <summary>
        /// 所属
        /// </summary>
        private string m_TableOwner = "";

		/// <summary>
		/// 构造ORACLE数据库对象
		/// </summary>
		public ClsOracleDb()
		{
			m_Connection = new OracleConnection();
			InitDB();
		}

		/// <summary>
		/// 根据连接参数构造ORACLE数据库对象
		/// </summary>
		/// <param name="strConnect">连接参数</param>
		public ClsOracleDb(string strConnect)
		{
            m_ConnectionString = GetConnString(strConnect);
            //m_ConnectionString += ";Connect Timeout=" + m_ConnectionTimeout;
			m_Connection = new OracleConnection(m_ConnectionString);          
			InitDB();
		}

        /// <summary>
        /// 获得连接字符串
        /// </summary>
        /// <param name="strBaseString">保存到数据库信息</param>
        /// <returns>返回连接字符串</returns>
        private string GetConnString(string strBaseString)
        {
            string strResult = "";
            string[] strConnstring = strBaseString.Split('$');

            strResult = strConnstring[0].ToString();

            if (strConnstring.Length > 1)
            {
                //数据库所属
                this.m_TableOwner = strConnstring[1];
            }

            return strResult;
        }

		#region IDataBase 成员

		private void InitDB()
		{
			//设置占位符为“：”
			m_ParamPlaceholder = ":";
		}

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBTYPE dbType
        {
            get
            {
                return m_dbType;
            }
        }

		/// <summary>
		/// 事务对象
		/// </summary>
		public IDbTransaction Transaction
		{
			get
			{
				return m_Transaction;
			}
			set
			{
				m_Transaction = (value as OracleTransaction);
			}
		}

		/// <summary>
		/// 连接对象
		/// </summary>
		public IDbConnection Connection
		{
			get
			{
				return m_Connection;
			}
			set
			{
				m_Connection = (value as OracleConnection);
			}
		}

		/// <summary>
		/// 占位符
		/// </summary>
		public string ParamPlaceholder
		{
			get
			{
				
				return m_ParamPlaceholder;
			}
			set
			{
				m_ParamPlaceholder = value;
			}
		}

		/// <summary>
		/// 提交事务
		/// </summary>
		public void Commit()
		{
			m_Transaction.Commit();
		}

		/// <summary>
		/// 设置监测点
		/// </summary>
		/// <param name="savepointName">监测点名称</param>
		public void SavePoint(string savepointName)
		{
			return;
		}

		/// <summary>
		/// 事务回滚
		/// </summary>
		public void Rollback()
		{
			m_Transaction.Rollback();
		}

		/// <summary>
		/// 根据监测点回滚事务
		/// </summary>
		/// <param name="savepointName"></param>
		public void Rollback(string savepointName)
		{
            m_Transaction.Rollback();
		}

		/// <summary>
		/// 数据库连接串
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return m_ConnectionString;
			}
			set
			{
				m_ConnectionString = value;
			}
		}

		/// <summary>
		/// 连接超时时限
		/// </summary>
		public int ConnectionTimeout
		{
			get
			{
				return m_ConnectionTimeout;
			}
		}

		/// <summary>
		/// 数据库对象
		/// </summary>
		public string Database
		{
			get
			{
				return m_Database;
			}
		}

		/// <summary>
		/// 数据库连接状态
		/// </summary>
		public ConnectionState State
		{
			get
			{
				return m_ConnectionState;
			}
		}

        /// <summary>
        /// 表的所属
        /// </summary>
        public string TableOwner
        {
            get
            {
                return this.m_TableOwner;
            }
        }

		/// <summary>
		/// 关闭数据库连接
		/// </summary>
		public void Close()
		{
			m_Connection.Close();
			m_ConnectionState = m_Connection.State;
		}

		/// <summary>
		/// 打开数据库连接
		/// </summary>
		public void Open()
		{
			m_Connection.Open();
			m_ConnectionState = m_Connection.State;
		}

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            m_Connection.Dispose();
            if (m_Transaction != null)
                m_Transaction.Dispose();
        }

		/// <summary>
		/// 开始一个新事务
		/// </summary>
		/// <returns>事务对象</returns>
		public IDbTransaction BeginTransaction()
		{
			m_Transaction = m_Connection.BeginTransaction();
			return m_Transaction;
		}

		/// <summary>
		/// 根据事务锁定级别创建一个新事务
		/// </summary>
		/// <param name="isoLevel">事务锁定级别</param>
		/// <returns>事务对象</returns>
		public IDbTransaction BeginTransaction(System.Data.IsolationLevel isoLevel)
		{
			m_Transaction = m_Connection.BeginTransaction(isoLevel);
			return m_Transaction;
		}

		/// <summary>
		/// 向指定的数据集中填充数据
		/// </summary>
		/// <param name="dataSet">数据集</param>
        /// <param name="p_SQL">生成数据集的sql语句</param>
		/// <returns>填充行数</returns>
        public int Fill(string p_SQL, DataSet dataSet)
		{
            OracleDataAdapter dataAdapter = new OracleDataAdapter(p_SQL, m_Connection);
            return dataAdapter.Fill(dataSet);
		}

		/// <summary>
		/// 向指定的数据集中的指定的数据表中填充数据
		/// </summary>
		/// <param name="dataSet">数据集</param>
		/// <param name="srcTable">数据表</param>
		/// <returns>填充行数</returns>
		public int Fill(DataSet dataSet, string srcTable)
		{
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            return dataAdapter.Fill(dataSet, srcTable);
		}

        /// <summary>
        /// 向动态数据集中填充数据
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="p_SQL">使用sql语句来填充数据集</param>
        /// <returns></returns>
        public int Fill(DataTable datatable, string p_SQL)
        {
            OracleDataAdapter dataAdapter = new OracleDataAdapter(p_SQL, m_Connection);
            return dataAdapter.Fill(datatable);
        }

        /// <summary>
        /// 获取sql语句执行结果值
        /// </summary>
        /// <param name="p_SQL"></param>
        /// <returns></returns>
        public object ExecuteScalar(string p_SQL)
        {
            OracleCommand Cmd = new OracleCommand(p_SQL, m_Connection);
            return Cmd.ExecuteScalar();
        }

        /// <summary>
        /// 执行 SQL 语句并返回受影响的行数。 
        /// </summary>
        /// <param name="p_SQL">要执行的SQL语句</param>
        /// <returns></returns>
        public int ExecuteSQL(string p_SQL)
        {
            OracleCommand Cmd = new OracleCommand(p_SQL, m_Connection);
            return Cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行 带参数的SQL 语句并返回受影响的行数。 
        /// </summary>
        /// <param name="p_SQL">要执行带参数的的SQL语句</param>
        /// <returns></returns>
        public int ExecuteSqlWithParam(string p_SQL, params object[] param)
        {
            OracleCommand Cmd = new OracleCommand(p_SQL, m_Connection);
            if (param != null)
            {
                Cmd.Parameters.AddRange(param);
            }
            return Cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行 SQL 语句,并影响事务。 
        /// </summary>
        /// <param name="p_SQL">要执行的SQL语句</param>
        /// <returns></returns>
        public int TransExecuteSQL(string p_SQL)
        {

            OracleCommand Cmd = new OracleCommand(p_SQL, m_Connection);
            Cmd.Transaction = m_Transaction;
            return Cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 根据带参数的sql获取dataset
        /// </summary>
        /// <param name="p_SQL"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string p_SQL, params object[] param)
        {
            DataSet ds = null;
            DbDataReader reader = null;
            OracleCommand Cmd = new OracleCommand(p_SQL, m_Connection);
            if (param != null)
            {
                Cmd.Parameters.AddRange(param);
            }
            ds = new DataSet();
            reader = Cmd.ExecuteReader();
            GetDataFromDataReader(reader, ds);
            return ds;
        }
        /// <summary>
        /// 从DataReader中读取数据到DataSet
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ds"></param>
        private static void GetDataFromDataReader(DbDataReader reader, DataSet ds)
        {
            DataTable dt = new DataTable();
            int j = reader.FieldCount;
            for (int i = 0; i < j; i++)
            {
                dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            while (reader.Read())
            {
                object[] values = new object[j];
                reader.GetValues(values);
                dt.Rows.Add(values);
            }
            ds.Tables.Add(dt);
            if (reader.NextResult())
                GetDataFromDataReader(reader, ds);

        }
        /// <summary>
        /// 执行存储过程，返回Output输出参数值        
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>object</returns>
        public object ExcuteProcedure(string storedProcName, IDataParameter[] paramenters)
        {
            OracleCommand command = BuildQueryCommand(m_Connection, storedProcName, paramenters);
            command.ExecuteNonQuery();
            object obj = command.Parameters["@Output_Value"].Value; //@Output_Value和具体的存储过程参数对应
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }
        }
        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        /// <summary>
        /// 执行存储过程，返回dataset      
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>object</returns>
        public DataSet GetProcedure(string storedProcName, IDataParameter[] paramenters)
        {
            OracleCommand command = BuildQueryCommand(m_Connection, storedProcName, paramenters);
            OracleDataAdapter da = new OracleDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        #endregion

    }
}
