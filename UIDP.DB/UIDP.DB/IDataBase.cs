// 工程名:数据库访问接口
// 模块编号：数据访问层
// 作用：用于提供不同数据库引擎的统一访问
// 作者：张凡
// 编写日期：2005-11-01

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
namespace UIDP.DB
{
	/// <summary>
	/// 数据库数据类型类
	/// </summary>
	public class ClsDbDataType
	{

	#region ClsDbDataType的数据成员

		/// <summary>
		/// 映射到DbType中的Binary类型
		/// </summary>
		public Type DBTypeBinary;

		/// <summary>
		/// 映射到DbType中的Object类型
		/// </summary>
		public Type DBTypeBlob;

		/// <summary>
		/// 映射到DbType中的Byte类型
		/// </summary>
		public Type DBTypeByte;

		/// <summary>
		/// 映射到DbType中的Date类型
		/// </summary>
		public Type DBTypeDate;

		/// <summary>
		/// 映射到DbType中的DateTime类型
		/// </summary>
		public Type DBTypeDateTime;

		/// <summary>
		/// 映射到DbType中的AnsiStringFixedLength类型
		/// </summary>
		public Type DBTypeChar;

		/// <summary>
		/// 映射到DbType中的Decimal类型
		/// </summary>
		public Type DBTypeDecimal;

		/// <summary>
		/// 映射到DbType中的Double类型
		/// </summary>
		public Type DBTypeDouble;

		/// <summary>
		/// 映射到DbType中的String类型
		/// </summary>
		public Type DBTypeVarChar;

		/// <summary>
		/// 映射到DbType中的Int16类型
		/// </summary>
		public Type DBTypeInt16;

		/// <summary>
		/// 映射到DbType中的Int32类型
		/// </summary>
		public Type DBTypeInt32;

		/// <summary>
		/// 映射到DbType中的Single类型
		/// </summary>
		public Type DBTypeSingle;

		#endregion  //ClsDbDataType的数据成员定义结束

	}

    public enum DBTYPE
    {
        MYSQL,
        SQLSERVER,
        ORACLE
    };

	/// <summary>
	/// 数据库定义接口
	/// </summary>
	public interface IDataBase
	{
        /// <summary>
        /// 数据库类型
        /// </summary>
        DBTYPE dbType{get;}

		/// <summary>
		/// 数据库连接事务
		/// </summary>
		IDbTransaction Transaction{get; set;}

		/// <summary>
		/// 数据库连接对象
		/// </summary>
		IDbConnection Connection{get; set;}

		/// <summary>
		/// 参数的占位符
		/// </summary>
		string ParamPlaceholder{get; set;}

		//关于IDbTransaction 的接口声明
		/// <summary>
		/// 提交自上次提交数据库以来的数据库操作
		/// </summary>
		void Commit();

		/// <summary>
		/// 将savepointName设置为保存点
		/// </summary>
		/// <param name="savepointName">保存点</param>
		void SavePoint(string savepointName);
		/// <summary>
		/// 将会滚自上次提交数据库以来的数据库操作
		/// </summary>
		void Rollback();

		/// <summary>
		/// 将数据库操作会滚到保存点
		/// </summary>
		/// <param name="savepointName">保存点</param>
		void Rollback(string savepointName);

		//关于IDbConnection 的接口声明
		/// <summary>
		/// 数据库连接字符串
		/// </summary>
		string ConnectionString {get; set;}

        /// <summary>
        /// 表的所属信息
        /// </summary>
        string TableOwner { get;}

		/// <summary>
		/// 数据库连接超时时间，缺省为15秒
		/// </summary>
		int ConnectionTimeout {get;}

		/// <summary>
		/// 连接的数据库名
		/// </summary>
		string Database {get;}

		/// <summary>
		/// 数据库连接状态
		/// </summary>
		ConnectionState State {get;}

		/// <summary>
		/// 创建数据库连接事务
		/// </summary>
		/// <returns>IDbTransaction</returns>
		IDbTransaction BeginTransaction();

		/// <summary>
		/// 以IsolationLevel隔离级别来创建一个数据库事务
		/// </summary>
		/// <param name="isoLevel">隔离级别</param>
		/// <returns>IDbTransaction</returns>
		IDbTransaction BeginTransaction(IsolationLevel isoLevel);

		/// <summary>
		/// 使用 ConnectionString 所指定的属性设置打开数据库连接
		/// </summary>
		void Open();

		/// <summary>
		/// 关闭数据库连接
		/// </summary>
		void Close();

        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();

		/// <summary>
		/// 在 DataSet 中添加或刷新行以匹配使用 DataSet 名称的数据源中的行，并创建一个名为“Table”的 DataTable
		/// </summary>
		/// <param name="dataSet"></param>
        /// <param name="p_SQL">生成数据集的sql语句</param>
		/// <returns></returns>
        int Fill(string p_SQL, DataSet dataSet);

		/// <summary>
		/// 在 DataSet 中添加或刷新行以匹配使用 DataSet 和 DataTable 名称的数据源中的行
		/// </summary>
		/// <param name="p_dataset"></param>
		/// <param name="p_srcTable"></param>
		/// <returns></returns>
		int Fill(DataSet p_dataset,string p_srcTable);

        DataSet GetDataSet(string p_SQL,params object[] param);

        /// <summary>
        /// 在 DataTable 中添加或刷新行以匹配使用 DataTable 名称的数据源中的行
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="p_SQL">使用sql语句来填充数据集</param>
        /// <returns></returns>
        int Fill(DataTable datatable, string p_SQL);

        /// <summary>
        /// 获取sql语句执行结果值
        /// </summary>
        /// <param name="p_SQL"></param>
        /// <returns></returns>
        object ExecuteScalar(string p_SQL);

        /// <summary>
        /// 执行 SQL 语句并返回受影响的行数。 
        /// </summary>
        /// <param name="p_SQL">要执行的SQL语句</param>
        /// <returns></returns>
        int ExecuteSQL(string p_SQL);
        /// <summary>
        /// 执行 带参数的SQL 语句并返回受影响的行数。 
        /// </summary>
        /// <param name="p_SQL">要执行带参数的的SQL语句</param>
        /// <returns></returns>
        int ExecuteSqlWithParam(string p_SQL,params object[] param);
        /// <summary>
        /// 执行 SQL 语句,并影响事务。 
        /// </summary>
        /// <param name="p_SQL">要执行的SQL语句</param>
        /// <returns></returns>
        int TransExecuteSQL(string p_SQL );
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="paramenters"></param>
        /// <returns></returns>

        object ExcuteProcedure(string storedProcName, IDataParameter[] paramenters);

        /// <summary>
        /// 执行存储过程 查dataset
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="paramenters"></param>
        /// <returns></returns>
        DataSet GetProcedure(string storedProcName, IDataParameter[] paramenters);

    }

}
