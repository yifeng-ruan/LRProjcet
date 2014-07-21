using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
using System.Data.SqlClient;
using LR.Utils.DBHelper;

namespace LR.DAL.Base
{
    /// <summary>
    ///  创建数据库操作继承类
    /// </summary>
    public class BaseDAL : CommonDB
    {

        protected Database db;
        public string DBName = "";

        public BaseDAL()
        {
            if (string.IsNullOrEmpty(DBName))
            {
                this.db = DatabaseFactory.CreateDatabase();
            }
            else
            {
                this.db = DatabaseFactory.CreateDatabase(DBName);
            }
        }

        #region 日志
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg">消息</param>
        protected void LogInfo(string msg)
        {

            // Loger.FileLog.Info<T>(msg);
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        protected void LogErro(object msg, Exception ex)
        {
            // Loger.FileLog.Erro<T>(msg, ex);
        }
        #endregion

        #region 参数设置
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="obj"></param>
        /// <param name="cols">参数列</param>
        protected void SetPter<Tobj>(DbCommand command, Tobj obj, string cols)
        {
            base.SetPter<Tobj>(db, command, obj, cols);
        }
        #endregion 参数设置

        #region 提交
        /// <summary>
        /// 提交
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="cmdList">命令对象列表</param>
        /// <returns></returns>
        public virtual bool Commit<TE>(IList<SubmitCommand<TE>> cmdList)
        {
            return base.Commit<TE>(db, cmdList);
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="cols">参数</param>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Commit<TE>(string sql, string cols, TE ent)
        {
            return base.Commit<TE>(db, sql, cols, ent);
        }


        // 添加对存储过程提交的支持
        /// <summary>
        /// 提交存储过程
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="cmdList">值传入类型对象</param>
        /// <returns>存储过程执行返回结果集</returns>
        public virtual DataSet CommitPrc<TE>(IList<SubmitCommand<TE>> cmdList)
        {
            return base.CommitPrc<TE>(db, cmdList);
        }

        /// <summary>
        /// 提交存储过程
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="cmdList">引用类型对象，如果存储过程有输出参数，输出参数将赋对些对象</param>
        /// <returns>存储过程执行返回结果集</returns>
        public virtual DataSet CommitPrc<TE>(IList<SubmitCommand<TE>> cmdList, out TE outObj)
        {
            return base.CommitPrc<TE>(db, cmdList, out outObj);
        }
        // *********** Add end ********************************
        #endregion

        #region 简单的分页，没有参数自动赋值
        /// <summary>
        /// 简单的分页，没有参数自动赋值
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield">字段</param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="strwhere">条件</param>
        /// <param name="strorder">排序</param>
        /// <param name="sqlCol">字段</param>
        /// <param name="fieldlist">属性</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, string strwhere, string strorder, string sqlCol, string fieldlist, out int total) where TEnty : new()
        {
            int _total = 0;
            string sqlText = SQLPage.PageSqlText(tablename, sqlCol, keyfield, strwhere, strorder);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageindex);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pagesize);
            db.AddOutParameter(dbCommand, "RecordCount", DbType.Int64, 1);

            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            object obj = db.GetParameterValue(dbCommand, "@RecordCount");
            _total = int.Parse(obj.ToString());
            total = _total;
            return infoList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield"></param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="strwhere">条件</param>
        /// <param name="strorder"></param>
        /// <param name="fieldlist">字段列表</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, string strwhere, string strorder, string fieldlist, out int total) where TEnty : new()
        {

            return GetListByPage<TEnty>(tablename, keyfield, pageindex, pagesize, strwhere, strorder, fieldlist, fieldlist, out  total);

        }

        /// <summary>
        /// 带条件的分页
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield">字段列</param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="strwhere">条件</param>
        /// <param name="wList">条件字段列表</param>
        /// <param name="strorder"></param>
        /// <param name="sqlCol"></param>
        /// <param name="fieldlist"></param>
        /// <param name="total">总数</param>
        /// <returns>返回值</returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, string strwhere, IList<Pter> wList, string strorder, string fieldlist, out int total) where TEnty : new()
        {
            return GetListByPage<TEnty>(tablename, keyfield, pageindex, pagesize, strwhere, wList, strorder, fieldlist, fieldlist, out  total);
        }

        #endregion

        #region 有参数赋值列表的分页

        /// <summary>
        /// 有参数赋值列表的分页
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield"></param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="strwhere"></param>
        /// <param name="wList">参数列表</param>
        /// <param name="strorder"></param>
        /// <param name="sqlCol"></param>
        /// <param name="fieldlist">跟实体相关的字段</param>
        /// <param name="total">总数</param>
        /// <returns>返回值</returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, string strwhere, IList<Pter> wList, string strorder, string sqlCol, string fieldlist, out int total) where TEnty : new()
        {
            int _total = 0;
            string sqlText = SQLPage.PageSqlText(tablename, sqlCol, keyfield, strwhere, strorder);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageindex);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pagesize);
            db.AddOutParameter(dbCommand, "RecordCount", DbType.Int64, 1);

            foreach (Pter pter in wList)
            {
                base.Pter(db, dbCommand, pter.Name, pter.Value);
            }

            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            object obj = db.GetParameterValue(dbCommand, "@RecordCount");
            _total = int.Parse(obj.ToString());
            total = _total;
            return infoList;
        }


        /// <summary>
        /// 根据实体和条件字段自动赋值分页
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield"></param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="whereCols">条件字段</param>
        /// <param name="ent">条件值</param>
        /// <param name="strorder"></param>
        /// <param name="sqlCol">要选择的字段</param>
        /// <param name="fieldlist">字段和属性</param>
        /// <param name="total"></param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, string whereCols, TEnty ent, string strorder, string sqlCol, string fieldlist, out int total) where TEnty : new()
        {
            string where = "";
            if (!string.IsNullOrEmpty(whereCols))
            {
                where = SQLUtility.WhereText(whereCols);
            }

            int _total = 0;

            string sqlText = SQLPage.PageSqlText(tablename, sqlCol, keyfield, where, strorder);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageindex);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pagesize);
            db.AddOutParameter(dbCommand, "RecordCount", DbType.Int64, 1);

            if (!string.IsNullOrEmpty(whereCols))
            {
                SetPter<TEnty>(db, dbCommand, ent, whereCols);
            }

            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            object obj = db.GetParameterValue(dbCommand, "@RecordCount");
            _total = int.Parse(obj.ToString());
            total = _total;
            return infoList;

        }

        /// <summary>
        /// 根据实体和条件语句自动赋值分页
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield">字段列</param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="whereCols">条件列</param>
        /// <param name="ent">条件值</param>
        /// <param name="sqlWhere">SQLWhere条件</param>
        /// <param name="strorder"></param>
        /// <param name="sqlCol">要选择的字段</param>
        /// <param name="fieldlist"></param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, string whereCols, TEnty ent, string sqlWhere, string strorder, string sqlCol, string fieldlist, out int total) where TEnty : new()
        {
            string where = sqlWhere;

            int _total = 0;

            string sqlText = SQLPage.PageSqlText(tablename, sqlCol, keyfield, where, strorder);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageindex);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pagesize);
            db.AddOutParameter(dbCommand, "RecordCount", DbType.Int64, 1);

            if (!string.IsNullOrEmpty(whereCols))
            {
                SetPter<TEnty>(db, dbCommand, ent, whereCols);
            }

            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            object obj = db.GetParameterValue(dbCommand, "@RecordCount");
            _total = int.Parse(obj.ToString());
            total = _total;
            return infoList;
        }

        /// <summary>
        /// 根据条件分页，自动简单的条件 and
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">表名</param>
        /// <param name="keyfield"></param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="wList">条件列</param>
        /// <param name="strorder">参数</param>
        /// <param name="fieldlist">字段列</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, IList<Pter> wList, string strorder, string fieldlist, out int total) where TEnty : new()
        {
            return GetListByPage<TEnty>(tablename, keyfield, pageindex, pagesize, wList, strorder, fieldlist, fieldlist, out  total);
        }
        #endregion

        #region 自动形成条件和赋值
        /// <summary>
        /// 自动形成条件和赋值
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="tablename">物理表</param>
        /// <param name="keyfield">字段列</param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="wList">参数和条件列表</param>
        /// <param name="strorder"></param>
        /// <param name="sqlCol">SQL列</param>
        /// <param name="fieldlist"></param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string tablename, string keyfield, int pageindex, int pagesize, IList<Pter> wList, string strorder, string sqlCol, string fieldlist, out int total) where TEnty : new()
        {
            string where = " where 1=1";
            foreach (Pter pter in wList)
            {
                where += string.Format(" and {0}=@{0}", pter.Name);
            }

            int _total = 0;
            string sqlText = SQLPage.PageSqlText(tablename, sqlCol, keyfield, where, strorder);

            LogInfo(sqlText);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageindex);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pagesize);
            db.AddOutParameter(dbCommand, "RecordCount", DbType.Int64, 1);

            foreach (Pter pter in wList)
            {
                base.Pter(db, dbCommand, pter.Name, pter.Value);
            }

            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            object obj = db.GetParameterValue(dbCommand, "@RecordCount");
            _total = int.Parse(obj.ToString());
            total = _total;
            return infoList;
        }

        #endregion

        #region 使用存储过程
        /// <summary>
        /// 自动形成条件和赋值
        /// </summary>
        /// <typeparam name="TEnty"></typeparam>
        /// <param name="tablename">物理表</param>
        /// <param name="keyfield">字段值</param>
        /// <param name="pageindex">分页索引</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="wList">参数和条件列表</param>
        /// <param name="strorder"></param>
        /// <param name="sqlCol">SQL列</param>
        /// <param name="fieldlist"></param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        protected IList<TEnty> GetListByPage<TEnty>(string procedure, string tablename, string PrimaryKey, string Sort, int CurrentPage, int pagesize, string Fields, string Filter, string Group, string fieldlist, out int total) where TEnty : new()
        {
            string[] Params = new string[8] { tablename, PrimaryKey, Sort, CurrentPage.ToString(), pagesize.ToString(), Fields, Filter, Group };
            LogInfo(procedure);
            DbCommand dbCommand = db.GetSqlStringCommand(procedure);
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddRange(SQLProcedures.GetParameter(procedure, Params));
            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            total = infoList.Count;
            return infoList;
        }

        #endregion

        #region 不分页的列表

        /// <summary>
        ///  不分页的列表
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="fieldlist">字段列表</param>
        /// <param name="wList">参数</param>
        /// <returns></returns>
        protected IList<TEnty> GetList<TEnty>(string sql, string fieldlist, IList<Pter> wList) where TEnty : new()
        {
            string sqlText = sql;
            DbCommand dbCommand = db.GetSqlStringCommand(sqlText);
            foreach (Pter pter in wList)
            {
                base.Pter(db, dbCommand, pter.Name, pter.Value);
            }
            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            return infoList;
        }


        /// <summary>
        ///  根据实体参数查询不分页的列表 
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="fieldlist">字段列表</param>
        /// <param name="whereCols">条件</param>
        /// <returns></returns>
        protected IList<TEnty> GetList<TEnty>(string sql, string fieldlist, TEnty ent, string whereCols) where TEnty : new()
        {
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            if (!string.IsNullOrEmpty(whereCols))
            {
                SetPter<TEnty>(db, dbCommand, ent, whereCols);
            }
            IList<TEnty> infoList = new List<TEnty>();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    TEnty info = new TEnty();
                    GetEnty<TEnty>(dataReader, fieldlist.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                    infoList.Add(info);
                }
            }
            return infoList;
        }
        // ************ Add end *******************************
        #endregion

        #region 获取实体
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="TEnty">实体</typeparam>
        /// <param name="dbCommand">命令对象</param>
        /// <param name="id">参数</param>
        /// <param name="selCols">列</param>
        /// <returns></returns>
        protected TEnty GetEnty<TEnty>(DbCommand dbCommand, string id, string selCols) where TEnty : new()
        {
            db.AddInParameter(dbCommand, "id", DbType.Int32, id);

            TEnty info = new TEnty();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    GetEnty<TEnty>(dataReader, selCols.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                }
            }
            return info;
        }


        protected TEnty GetEnty<TEnty>(DbCommand dbCommand, string pName, object value, string selCols) where TEnty : new()
        {
            Pter(db, dbCommand, pName, value);

            TEnty info = new TEnty();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    GetEnty<TEnty>(dataReader, selCols.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                }
            }
            return info;
        }

        /// <summary>
        /// 根据实体和条件字段（必须唯一）读取某一实体
        /// </summary>
        /// <typeparam name="TEnty"></typeparam>
        /// <param name="dbCommand"></param>
        /// <param name="pNames"></param>
        /// <param name="ent"></param>
        /// <param name="selCols"></param>
        /// <returns></returns>
        protected TEnty GetEnty<TEnty>(DbCommand dbCommand, TEnty ent, string pNames, string selCols) where TEnty : new()
        {
            //Pter(db, dbCommand, pName, value);
            SetPter<TEnty>(db, dbCommand, ent, pNames);

            TEnty info = new TEnty();
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                Dictionary<string, string> cachUserNameMaping = new Dictionary<string, string>();
                while (dataReader.Read())
                {
                    GetEnty<TEnty>(dataReader, selCols.Replace("[", "").Replace("]", "").Replace("\r\n", ""), ref info, cachUserNameMaping);
                }
            }
            return info;
        }
        #endregion

        #region 检查某字段是否已存在
        protected virtual bool HasExist<TEnty>(DbCommand dbCommand, TEnty ent, string whereColText)
        {
            SetPter<TEnty>(db, dbCommand, ent, whereColText);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    if (int.Parse(dataReader["RecordCount"].ToString()) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region

        /// <summary>
        /// 执行存储过程 返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// </summary>
        /// <param name="command">OracleCommand</param>
        /// <returns>OracleDataReader</returns>
        public SqlDataReader RunProcedure(SqlCommand command)
        {
            SqlConnection connection = new SqlConnection(db.ConnectionString);
            SqlDataReader returnReader;
            connection.Open();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }


        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(db.ConnectionString);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();

                sqlDA.SelectCommand = new SqlCommand(storedProcName, connection);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;

                try
                {
                    sqlDA.Fill(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(db.ConnectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
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
        #endregion
    }
}
