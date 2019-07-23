// ������:���ݿ���ʽӿ�
// ģ���ţ����ݷ��ʲ�
// ���ã������ṩ��SqlServer���ݿ�����ķ���
// ���ߣ��ŷ�
// ��д���ڣ�2005-11-01

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
namespace UIDP.DB
{
	/// <summary>
	/// ClsSqlServerDb ��ժҪ˵����
	/// </summary>
	public class ClsSqlServerDb:IDataBase
	{
		private string m_ParamPlaceholder;	//������ռλ��
		private SqlConnection m_Connection;
		private SqlTransaction m_Transaction;
		private string m_ConnectionString = "";
        private SqlCommand sqlCommand;
        private int m_ConnectionTimeout = 3600;
		private string m_Database = "";
		private ConnectionState m_ConnectionState = ConnectionState.Closed;
        private DBTYPE m_dbType = DBTYPE.SQLSERVER;

		/// <summary>
		/// ����SQL SERVER���ݿ����
		/// </summary>
		public ClsSqlServerDb()
		{
			m_Connection = new SqlConnection();
            sqlCommand = new SqlCommand();
            InitDB();
		}

		/// <summary>
		/// �������Ӳ�������SQL SERVER ���ݿ����
		/// </summary>
		/// <param name="strConnect"></param>
		public ClsSqlServerDb(string strConnect)
		{
			m_ConnectionString = strConnect;
			m_Connection = new SqlConnection(m_ConnectionString);
            sqlCommand = new SqlCommand("", m_Connection);
			InitDB();
		}

		private void InitDB()
		{
			//����ռλ��Ϊ������
			m_ParamPlaceholder = "@";

        }

        #region IDataBase ��Ա

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DBTYPE dbType
        {
            get
            {
                return m_dbType;
            }
        }
		/// <summary>
		/// �������
		/// </summary>
		public IDbTransaction Transaction
		{
			get
			{
				return m_Transaction;
			}
			set
			{
				m_Transaction = (value as SqlTransaction);
			}
		}

		/// <summary>
		/// ���ݿ����Ӷ���
		/// </summary>
		public IDbConnection Connection
		{
			get
			{
				return m_Connection;
			}
			set
			{
				m_Connection = (value as SqlConnection);
			}
		}

		/// <summary>
		/// ռλ��
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
		/// �����ύ
		/// </summary>
		public void Commit()
		{
			m_Transaction.Commit();
		}

		/// <summary>
		/// ���ü���
		/// </summary>
		/// <param name="savepointName">��������</param>
		public void SavePoint(string savepointName)
		{
			m_Transaction.Save(savepointName);
		}

		/// <summary>
		/// ����ع�
		/// </summary>
		public void Rollback()
		{
			m_Transaction.Rollback();
		}

		/// <summary>
		/// ���ݼ���ع�����
		/// </summary>
		/// <param name="savepointName">��������</param>
		public void Rollback(string savepointName)
		{
			m_Transaction.Rollback(savepointName);
		}

		/// <summary>
		/// ���ݿ����Ӳ���
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
        /// �������
        /// </summary>
        public string TableOwner
        {
            get
            {
                return "";
            }
        }

		/// <summary>
		/// ���ݿ����ӳ�ʱʱ��
		/// </summary>
		public int ConnectionTimeout
		{
			get
			{
				return m_ConnectionTimeout;
			}
		}

		/// <summary>
		/// ���ݿ����
		/// </summary>
		public string Database
		{
			get
			{
				return m_Database;
			}
		}

		/// <summary>
		/// ���Ӷ���״̬
		/// </summary>
		public ConnectionState State
		{
			get
			{
				return m_ConnectionState;
			}
		}

		/// <summary>
		/// ��ʼһ��������
		/// </summary>
		/// <returns>�������</returns>
		public IDbTransaction BeginTransaction()
		{
			m_Transaction = m_Connection.BeginTransaction();
            sqlCommand = new SqlCommand("",m_Connection,m_Transaction);
            return m_Transaction;
		}

		/// <summary>
		/// �����������������ʼһ��������
		/// </summary>
		/// <param name="isoLevel">������������</param>
		/// <returns>�������</returns>
		public IDbTransaction BeginTransaction(System.Data.IsolationLevel isoLevel)
		{
			m_Transaction = m_Connection.BeginTransaction(isoLevel);
			return m_Transaction;
		}

		/// <summary>
		/// �����ݿ�����
		/// </summary>
		public void Open()
		{
			m_Connection.Open();
			m_ConnectionState = m_Connection.State;
		}

		/// <summary>
		/// �ر����ݿ�����
		/// </summary>
		public void Close()
		{
			m_Connection.Close();
			m_ConnectionState = m_Connection.State;
		}

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            m_Connection.Dispose();
            m_Transaction.Dispose();
        }

        /// <summary>
		/// ��ָ�������ݼ����������
		/// </summary>
		/// <param name="dataSet">���ݼ�</param>
        /// <param name="p_SQL">�������ݼ���sql���</param>
		/// <returns>��������</returns>
		public int Fill(string p_SQL,DataSet dataSet)
		{
            SqlDataAdapter dataAdapter = new SqlDataAdapter(p_SQL, m_Connection);
            return dataAdapter.Fill(dataSet);
		}

		/// <summary>
		/// ���ݸ����ı�����ָ�������ݼ����������
		/// </summary>
		/// <param name="p_dataset">���ݼ�</param>
		/// <param name="p_srcTable">���ݱ���</param>
		/// <returns>�������</returns>
		public int Fill(DataSet p_dataset, string p_srcTable)
		{
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            return dataAdapter.Fill(p_dataset, p_srcTable);
		}

        /// <summary>
        /// ��̬���ݼ����������
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="p_SQL">ʹ��sql�����������ݼ�</param>
        /// <returns></returns>
        public int Fill(DataTable datatable, string p_SQL)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(p_SQL, m_Connection);
            return dataAdapter.Fill(datatable);
        }

        /// <summary>
        /// ��ȡsql���ִ�н��ֵ
        /// </summary>
        /// <param name="p_SQL"></param>
        /// <returns></returns>
        public object ExecuteScalar(string p_SQL)
        {
            SqlCommand Cmd = new SqlCommand(p_SQL, m_Connection);
            return Cmd.ExecuteScalar();
        }

        /// <summary>
        /// ִ�� SQL ��䲢������Ӱ��������� 
        /// </summary>
        /// <param name="p_SQL">Ҫִ�е�SQL���</param>
        /// <returns></returns>
        public int ExecuteSQL(string p_SQL)
        {

            //SqlCommand Cmd = new SqlCommand(p_SQL, m_Connection);
            sqlCommand.CommandText = p_SQL;
            sqlCommand.CommandTimeout = 60;
            return sqlCommand.ExecuteNonQuery();
        }
        /// <summary>
        /// ִ�� ��������SQL ��䲢������Ӱ��������� 
        /// </summary>
        /// <param name="p_SQL">Ҫִ�д������ĵ�SQL���</param>
        /// <returns></returns>
        public int ExecuteSqlWithParam(string p_SQL, params object[] param)
        {
            SqlCommand Cmd = new SqlCommand(p_SQL, m_Connection);
            if (param != null)
            {
                Cmd.Parameters.AddRange(param);
            }
            return Cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// ִ�� SQL ���,��Ӱ������ 
        /// </summary>
        /// <param name="p_SQL">Ҫִ�е�SQL���</param>
        /// <returns></returns>
        public int TransExecuteSQL(string p_SQL)
        {

            SqlCommand Cmd = new SqlCommand(p_SQL, m_Connection);
            Cmd.Transaction = m_Transaction;
            return Cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// ���ݴ�������sql��ȡdataset
        /// </summary>
        /// <param name="p_SQL"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string p_SQL, params object[] param)
        {
            DataSet ds = null;
            DbDataReader reader = null;
            SqlCommand Cmd = new SqlCommand(p_SQL, m_Connection);
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
        /// ��DataReader�ж�ȡ���ݵ�DataSet
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
        /// ִ�д洢���̣�����Output�������ֵ        
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>object</returns>
        public  object ExcuteProcedure(string storedProcName, IDataParameter[] paramenters)
        {
                SqlCommand command = BuildQueryCommand(m_Connection, storedProcName, paramenters);
                command.Parameters["@Output_Value"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                object obj = command.Parameters["@Output_Value"].Value; //@Output_Value�;���Ĵ洢���̲�����Ӧ
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
        /// ���� SqlCommand ����(��������һ���������������һ������ֵ)
        /// </summary>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // ���δ����ֵ���������,���������DBNull.Value.
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
        /// ִ�д洢���̣�����dataset      
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>object</returns>
        public DataSet GetProcedure(string storedProcName, IDataParameter[] paramenters)
        {
            SqlCommand command = BuildQueryCommand(m_Connection, storedProcName, paramenters);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        #endregion

    }
}
