using System;

namespace UIDP.DB
{
	/// <summary>
	/// ClsDBFactory 的摘要说明。
	/// </summary>
	public class ClsDBFactory
	{
		private IDataBase m_Database;

		/// <summary>
		/// 数据库对象的接口
		/// </summary>
		public IDataBase DataBase
		{
			get
			{
				return m_Database;
			}
		}

		/// <summary>
		/// 析取类数据库连接对象生成工厂
		/// </summary>
		/// <param name="p_strDbType">数据库类型</param>
		/// <param name="p_strConn">连接数据库的信息</param>
		public ClsDBFactory(string p_strDbType,string p_strConn)
		{
			//
			// TODO: 在此处添加构造函数逻辑
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
