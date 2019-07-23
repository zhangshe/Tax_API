// ������:���ݿ���ʽӿ�
// ģ���ţ����ݷ��ʲ�
// ���ã������ṩ��ͬ���ݿ������ͳһ����
// ���ߣ��ŷ�
// ��д���ڣ�2005-11-01

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
namespace UIDP.DB
{
	/// <summary>
	/// ���ݿ�����������
	/// </summary>
	public class ClsDbDataType
	{

	#region ClsDbDataType�����ݳ�Ա

		/// <summary>
		/// ӳ�䵽DbType�е�Binary����
		/// </summary>
		public Type DBTypeBinary;

		/// <summary>
		/// ӳ�䵽DbType�е�Object����
		/// </summary>
		public Type DBTypeBlob;

		/// <summary>
		/// ӳ�䵽DbType�е�Byte����
		/// </summary>
		public Type DBTypeByte;

		/// <summary>
		/// ӳ�䵽DbType�е�Date����
		/// </summary>
		public Type DBTypeDate;

		/// <summary>
		/// ӳ�䵽DbType�е�DateTime����
		/// </summary>
		public Type DBTypeDateTime;

		/// <summary>
		/// ӳ�䵽DbType�е�AnsiStringFixedLength����
		/// </summary>
		public Type DBTypeChar;

		/// <summary>
		/// ӳ�䵽DbType�е�Decimal����
		/// </summary>
		public Type DBTypeDecimal;

		/// <summary>
		/// ӳ�䵽DbType�е�Double����
		/// </summary>
		public Type DBTypeDouble;

		/// <summary>
		/// ӳ�䵽DbType�е�String����
		/// </summary>
		public Type DBTypeVarChar;

		/// <summary>
		/// ӳ�䵽DbType�е�Int16����
		/// </summary>
		public Type DBTypeInt16;

		/// <summary>
		/// ӳ�䵽DbType�е�Int32����
		/// </summary>
		public Type DBTypeInt32;

		/// <summary>
		/// ӳ�䵽DbType�е�Single����
		/// </summary>
		public Type DBTypeSingle;

		#endregion  //ClsDbDataType�����ݳ�Ա�������

	}

    public enum DBTYPE
    {
        MYSQL,
        SQLSERVER,
        ORACLE
    };

	/// <summary>
	/// ���ݿⶨ��ӿ�
	/// </summary>
	public interface IDataBase
	{
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        DBTYPE dbType{get;}

		/// <summary>
		/// ���ݿ���������
		/// </summary>
		IDbTransaction Transaction{get; set;}

		/// <summary>
		/// ���ݿ����Ӷ���
		/// </summary>
		IDbConnection Connection{get; set;}

		/// <summary>
		/// ������ռλ��
		/// </summary>
		string ParamPlaceholder{get; set;}

		//����IDbTransaction �Ľӿ�����
		/// <summary>
		/// �ύ���ϴ��ύ���ݿ����������ݿ����
		/// </summary>
		void Commit();

		/// <summary>
		/// ��savepointName����Ϊ�����
		/// </summary>
		/// <param name="savepointName">�����</param>
		void SavePoint(string savepointName);
		/// <summary>
		/// ��������ϴ��ύ���ݿ����������ݿ����
		/// </summary>
		void Rollback();

		/// <summary>
		/// �����ݿ��������������
		/// </summary>
		/// <param name="savepointName">�����</param>
		void Rollback(string savepointName);

		//����IDbConnection �Ľӿ�����
		/// <summary>
		/// ���ݿ������ַ���
		/// </summary>
		string ConnectionString {get; set;}

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        string TableOwner { get;}

		/// <summary>
		/// ���ݿ����ӳ�ʱʱ�䣬ȱʡΪ15��
		/// </summary>
		int ConnectionTimeout {get;}

		/// <summary>
		/// ���ӵ����ݿ���
		/// </summary>
		string Database {get;}

		/// <summary>
		/// ���ݿ�����״̬
		/// </summary>
		ConnectionState State {get;}

		/// <summary>
		/// �������ݿ���������
		/// </summary>
		/// <returns>IDbTransaction</returns>
		IDbTransaction BeginTransaction();

		/// <summary>
		/// ��IsolationLevel���뼶��������һ�����ݿ�����
		/// </summary>
		/// <param name="isoLevel">���뼶��</param>
		/// <returns>IDbTransaction</returns>
		IDbTransaction BeginTransaction(IsolationLevel isoLevel);

		/// <summary>
		/// ʹ�� ConnectionString ��ָ�����������ô����ݿ�����
		/// </summary>
		void Open();

		/// <summary>
		/// �ر����ݿ�����
		/// </summary>
		void Close();

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        void Dispose();

		/// <summary>
		/// �� DataSet ����ӻ�ˢ������ƥ��ʹ�� DataSet ���Ƶ�����Դ�е��У�������һ����Ϊ��Table���� DataTable
		/// </summary>
		/// <param name="dataSet"></param>
        /// <param name="p_SQL">�������ݼ���sql���</param>
		/// <returns></returns>
        int Fill(string p_SQL, DataSet dataSet);

		/// <summary>
		/// �� DataSet ����ӻ�ˢ������ƥ��ʹ�� DataSet �� DataTable ���Ƶ�����Դ�е���
		/// </summary>
		/// <param name="p_dataset"></param>
		/// <param name="p_srcTable"></param>
		/// <returns></returns>
		int Fill(DataSet p_dataset,string p_srcTable);

        DataSet GetDataSet(string p_SQL,params object[] param);

        /// <summary>
        /// �� DataTable ����ӻ�ˢ������ƥ��ʹ�� DataTable ���Ƶ�����Դ�е���
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="p_SQL">ʹ��sql�����������ݼ�</param>
        /// <returns></returns>
        int Fill(DataTable datatable, string p_SQL);

        /// <summary>
        /// ��ȡsql���ִ�н��ֵ
        /// </summary>
        /// <param name="p_SQL"></param>
        /// <returns></returns>
        object ExecuteScalar(string p_SQL);

        /// <summary>
        /// ִ�� SQL ��䲢������Ӱ��������� 
        /// </summary>
        /// <param name="p_SQL">Ҫִ�е�SQL���</param>
        /// <returns></returns>
        int ExecuteSQL(string p_SQL);
        /// <summary>
        /// ִ�� ��������SQL ��䲢������Ӱ��������� 
        /// </summary>
        /// <param name="p_SQL">Ҫִ�д������ĵ�SQL���</param>
        /// <returns></returns>
        int ExecuteSqlWithParam(string p_SQL,params object[] param);
        /// <summary>
        /// ִ�� SQL ���,��Ӱ������ 
        /// </summary>
        /// <param name="p_SQL">Ҫִ�е�SQL���</param>
        /// <returns></returns>
        int TransExecuteSQL(string p_SQL );
        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="paramenters"></param>
        /// <returns></returns>

        object ExcuteProcedure(string storedProcName, IDataParameter[] paramenters);

        /// <summary>
        /// ִ�д洢���� ��dataset
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="paramenters"></param>
        /// <returns></returns>
        DataSet GetProcedure(string storedProcName, IDataParameter[] paramenters);

    }

}
