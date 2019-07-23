using System;

namespace UIDP.DB
{
	/// <summary>
	/// ClsDBFactory ��ժҪ˵����
	/// </summary>
	public class ClsDBFactory
	{
		private IDataBase m_Database;

		/// <summary>
		/// ���ݿ����Ľӿ�
		/// </summary>
		public IDataBase DataBase
		{
			get
			{
				return m_Database;
			}
		}

		/// <summary>
		/// ��ȡ�����ݿ����Ӷ������ɹ���
		/// </summary>
		/// <param name="p_strDbType">���ݿ�����</param>
		/// <param name="p_strConn">�������ݿ����Ϣ</param>
		public ClsDBFactory(string p_strDbType,string p_strConn)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			switch (p_strDbType.ToUpper())
			{
				case "MYSQL":
					m_Database = new ClsMySqlDb(p_strConn);
					break;
                case "ORACLE":
                    m_Database = new ClsOracleDb(p_strConn);
                    break;
                case "SQLSERVER":
					m_Database = new ClsSqlServerDb(p_strConn);
					break;
				default:
					break;
			}
		}
	}
}
