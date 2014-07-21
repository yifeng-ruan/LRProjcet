using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Web;
using LR.Utils.DBHelper;

namespace LR.DAL.Base
{
    /// <summary>
    /// 操作数据库执行类
    /// </summary>
    public class CommonDB
    {
        /// <summary>
        /// 字符串参数
        /// </summary>
        /// <param name="command">操作命令对象</param>
        /// <param name="pm">字符串参数</param>
        /// <param name="value">参数</param>
        protected void Pter<Ty>(Database db, DbCommand command, string pm, Ty value)
        {
            db.AddInParameter(command, pm, SQLUtility.GetDbType(value.GetType()), value);
        }
        /// <summary>
        ///  判断输入参数并赋值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="pm"></param>
        /// <param name="value"></param>
        protected void Pter(Database db, DbCommand command, string pm, object value)
        {
            if (value != null)
            {
                db.AddInParameter(command, pm, SQLUtility.GetDbType(value.GetType()), value);
            }
            else
            {
                db.AddInParameter(command, pm, DbType.String, DBNull.Value);
            }
        }
        /// <summary>
        /// 判断输出参数并赋值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="pm"></param>
        /// <param name="value"></param>
        protected void PterOut(Database db, DbCommand command, string pm, object value)
        {
            if (value != null)
            {
                db.AddParameter(command, pm, SQLUtility.GetDbType(value.GetType()), ParameterDirection.Output, "", DataRowVersion.Current, null);
            }
            else
            {
                db.AddParameter(command, pm, DbType.String, ParameterDirection.Output, "", DataRowVersion.Current, null);
            }
        }
        /// <summary>
        /// 判断输出参数并赋值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="pm"></param>
        /// <param name="valueType"></param>
        protected void PterOut(Database db, DbCommand command, string pm, Type valueType)
        {
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                db.AddParameter(command, pm, SQLUtility.GetDbType(valueType.GetGenericArguments()[0]), ParameterDirection.Output, "", DataRowVersion.Current, null);
            }
            else
            {
                //【黄首益】如果传出参数类型为string时，会报错：“Size属性具有无效大小值 0”，暂定传出参数长度固定为1000。
                if (SQLUtility.GetDbType(valueType) == DbType.String)
                    db.AddOutParameter(command, pm, SQLUtility.GetDbType(valueType), 1000);
                else
                    db.AddParameter(command, pm, SQLUtility.GetDbType(valueType), ParameterDirection.Output, "", DataRowVersion.Current, null);
            }
        }
        /// <summary>
        /// 将参数作为数组判断
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="pm"></param>
        /// <param name="value"></param>
        /// <param name="obj"></param>
        protected void Pter<Tobj>(Database db, DbCommand command, string pm, object value, Tobj obj)
        {
            IList<string> inParameterList = GetProctParametersName<Tobj>(obj, "ProctParametersName");
            IList<string> outParameterList = GetProctParametersName<Tobj>(obj, "ProctOutParametersName");

            foreach (string inParameter in inParameterList)
            {
                if (inParameter == string.Format("[{0}]", pm))
                {
                    Pter(db, command, pm, value);
                    break;
                }
            }

            foreach (string outParameter in outParameterList)
            {
                if (outParameter == string.Format("[{0}]", pm))
                {
                    PterOut(db, command, pm, value);
                    break;
                }
            }
        }

        /// <summary>
        /// 根据实体和字段设置参数
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <param name="cols"></param>
        protected void SetPter<Tobj>(Database db, DbCommand command, Tobj obj, string cols)
        {
            string[] list = cols.Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Trim().Trim(new char[] { '[', ']' });
            }

            foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
            {
                if (viewProperty.CanRead)
                {
                    if (list.Contains<string>(viewProperty.Name))
                    {
                        Pter(db, command, viewProperty.Name, viewProperty.GetValue(obj, null));
                    }
                }
            }
        }

        /// <summary>
        /// 根据实体和字段设置参数
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <param name="cols"></param>
        protected void SetPterOut<Tobj>(Database db, DbCommand command, Tobj obj, string cols)
        {
            string[] list = cols.Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Trim().Trim(new char[] { '[', ']' });
            }

            foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
            {
                if (viewProperty.CanRead)
                {
                    if (list.Contains<string>(viewProperty.Name))
                    {
                        PterOut(db, command, viewProperty.Name, viewProperty.PropertyType);
                    }
                }
            }
        }

        /// <summary>
        /// 根据实体和字段设置参数
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <param name="cols"></param>
        protected IList<string> GetProctParametersName<Tobj>(Tobj obj, string attributeName)
        {
            IList<string> result = new List<string>();
            foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
            {
                if (viewProperty.CanRead && viewProperty.Name == attributeName)
                {
                    string parName = viewProperty.GetValue(obj, null).ToString();

                    if (string.IsNullOrEmpty(parName))
                        return null;

                    foreach (string parNameSub in parName.Split(','))
                    {
                        result.Add(parNameSub);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 根据实体和字段设置参数
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <param name="cols"></param>
        protected Tobj GetProctParametersValue<Tobj>(Database db, DbCommand command, Tobj obj, string attributeName)
        {
            Tobj result = default(Tobj);
            string parName = "";
            foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
            {
                if (viewProperty.CanRead && viewProperty.Name == attributeName && viewProperty.GetValue(obj, null) != null)
                {
                    parName = viewProperty.GetValue(obj, null).ToString();
                }
            }

            if (string.IsNullOrEmpty(parName))
                return result;

            foreach (string parNameSub in parName.Split(','))
            {
                foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
                {
                    if (string.Format("[{0}]", viewProperty.Name) == parNameSub && viewProperty.CanWrite)
                    {
                        var objvalue = db.GetParameterValue(command, viewProperty.Name);
                        if (objvalue != DBNull.Value)
                            viewProperty.SetValue(obj, db.GetParameterValue(command, viewProperty.Name), null);
                    }
                }
            }

            result = obj;
            return result;
        }

        /// <summary>
        /// 转换数组
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        protected string[] ToList(string cols)
        {
            string[] list = cols.Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Trim().Trim(new char[] { '[', ']' });
            }
            return list;
        }

        /// <summary>
        /// 转换数组
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        protected IList<string> ToIList(string cols)
        {
            IList<string> ilist = new List<string>();
            string[] list = cols.Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                ilist.Add(list[i].Trim().Trim(new char[] { '[', ']' }));
            }
            return ilist;
        }

        #region 从DataReader中读取对象的属性值
        /// <summary>
        /// 从DataReader中读取对象的属性值
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="dataReader"></param>
        /// <param name="cols"></param>
        /// <param name="obj"></param>
        /// <param name="cachUserNameMapping">缓存以前出现过的用户名称</param>
        protected void GetEnty<Tobj>(IDataReader dataReader, string cols, ref Tobj obj, Dictionary<string, string> cachUserNameMapping) where Tobj : new()
        {
            string[] list = cols.Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Trim();
            }
            foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
            {
                #region 获取对象的属性值
                //判断列表是否包含本属性
                if (list.Contains<string>(viewProperty.Name))
                {
                    object valObj = dataReader[viewProperty.Name];
                    if (valObj != DBNull.Value)
                    {
                        if (viewProperty.CanRead)
                        {
                            //del by brian.chen 20110421 重复判断列表是否包含本属性
                            //if (list.Contains<string>(viewProperty.Name))
                            //{
                            // ********************** Change by Gary.Xie 2010-08-25 **********************
                            // ********************** 添加了对可空类型的判断 *******************************
                            //object valueToAssign = Convert.ChangeType(valObj, viewProperty.PropertyType);
                            object valueToAssign = null;

                            // 对可空类型的判断
                            if (viewProperty.PropertyType.IsGenericType &&
                                viewProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                valueToAssign = Convert.ChangeType(valObj, viewProperty.PropertyType.GetGenericArguments()[0]);
                            }
                            else
                            {
                                valueToAssign = Convert.ChangeType(valObj, viewProperty.PropertyType);
                            }
                            // ********************** Change end *******************************************

                            if (valueToAssign != null)
                            {
                                viewProperty.SetValue(obj, valueToAssign, null);
                                //del by brian.chen 20110421 已判断有本属性viewProperty，可以直接赋值，不需要viewProperty.Name取该属性赋值
                                //obj.GetType().GetProperty(viewProperty.Name).SetValue(obj, valueToAssign, null);
                            }
                            //}
                        }
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 从DataReader中读取对象的属性值
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="dataReader"></param>
        /// <param name="cols"></param>
        /// <param name="obj"></param>
        protected void GetEnty<Tobj>(IDataReader dataReader, string cols, ref Tobj obj) where Tobj : new()
        {
            GetEnty<Tobj>(dataReader, cols, ref obj, new Dictionary<string, string>());
        }
        #endregion

        #region 简单
        public virtual bool Commit<T>(Database db, string sql, string cols, T ent)
        {
            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
            list.Add(new SubmitCommand<T>() { ActionCols = cols, Enty = ent, Sql = sql });
            return Commit<T>(db, list);
        }
        /// <summary>
        /// 多个业务提交 数据参数是通过实体和 ActionCols 字段对应起来。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmdList"></param>
        /// <returns></returns>
        public virtual bool Commit<T>(Database db, IList<SubmitCommand<T>> cmdList)
        {
            bool result = false;
            string ProcMessageCode = null;
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach (var cmd in cmdList)
                    {
                        DbCommand command = null;

                        if (cmd.isProc)
                        {
                            command = db.GetStoredProcCommand(cmd.ProcName);

                        }
                        else
                        {
                            command = db.GetSqlStringCommand(cmd.Sql);
                            //判断是否有设置数据库连接超时时间
                            if (cmd.TimeOut != null && cmd.TimeOut > 30)
                            {
                                command.CommandTimeout = Convert.ToInt32(cmd.TimeOut);
                            }
                            //与实体相对应的参数
                            SetPter<T>(db, command, cmd.Enty, cmd.ActionCols);
                        }

                        //判断是否有设置数据库连接超时时间
                        if (cmd.TimeOut != null && cmd.TimeOut > 30)
                        {
                            command.CommandTimeout = Convert.ToInt32(cmd.TimeOut);
                        }

                        // 2. Set import parameter
                        if (!string.IsNullOrEmpty(cmd.ProctParametersName))
                        {
                            SetPter<T>(db, command, cmd.Enty, cmd.ProctParametersName);
                        }

                        // 3. Set outport parameter
                        if (!string.IsNullOrEmpty(cmd.ProctOutParametersName))
                        {
                            SetPterOut<T>(db, command, cmd.Enty, cmd.ProctOutParametersName);
                        }
                        //参数列表的参数
                        if (cmd.PList != null)
                        {
                            foreach (Pter pter in cmd.PList)
                            {
                                Pter(db, command, pter.Name, pter.Value);
                            }
                        }
                        db.ExecuteNonQuery(command, transaction);
                        //-----begin---------update by sinbad 2010.12.17 
                        if (!string.IsNullOrEmpty(cmd.ProctOutParametersName))
                        {
                            ProcMessageCode = command.Parameters["@ProcMessageCode"].Value.ToString();
                            if (ProcMessageCode.Substring(0, 1) == "9")
                            {
                                foreach (PropertyInfo viewProperty in cmdList[0].Enty.GetType().GetProperties())
                                {
                                    if (viewProperty.Name == "ProcMessageCode")
                                    {
                                        viewProperty.SetValue(cmdList[0].Enty, Convert.ToInt32(ProcMessageCode), null);
                                    }
                                }
                                transaction.Rollback();
                                return false;
                            }
                        }
                        //-----end---------
                    }
                    // Commit the transaction    
                    transaction.Commit();
                    result = true;

                }
                catch (Exception ex)
                {
                    // Rollback transaction 
                    transaction.Rollback();
                    throw ex;
                }
                connection.Close();
                connection.Dispose();
            }
            return result;
        }
        /// <summary>
        /// 提交事物
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmdList"></param>
        /// <returns></returns>
        public virtual bool CommitGeneral<T>(Database db, IList<SubmitCommand<T>> cmdList)
        {
            bool result = false;
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach (var cmd in cmdList)
                    {
                        //1. create command
                        DbCommand command = db.GetStoredProcCommand(cmd.ProcName);

                        // 2. Set import parameter
                        if (!string.IsNullOrEmpty(cmd.ProctParametersName))
                        {
                            SetPter<T>(db, command, cmd.Enty, cmd.ProctParametersName);
                        }

                        // 3. Set outport parameter
                        if (!string.IsNullOrEmpty(cmd.ProctOutParametersName))
                        {
                            SetPterOut<T>(db, command, cmd.Enty, cmd.ProctOutParametersName);
                        }

                        db.ExecuteNonQuery(command, transaction);

                        //4.get OutParameters
                        if (!string.IsNullOrEmpty(cmd.ProctOutParametersName))
                        {
                            string[] parameters = cmd.ProctOutParametersName.Split(',');
                            string strParameter;
                            PropertyInfo piProperty = null;

                            for (int i = 0; i < parameters.Length; i++)
                            {
                                strParameter = parameters[i].Substring(1, parameters[i].Length - 2);
                                piProperty = cmd.Enty.GetType().GetProperty(strParameter);
                                if (piProperty != null)
                                {
                                    var objvalue = db.GetParameterValue(command, piProperty.Name);
                                    if (objvalue != DBNull.Value)
                                        piProperty.SetValue(cmd.Enty, objvalue, null);
                                }
                            }
                        }

                    }

                    transaction.Commit();
                    result = true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        // 添加对存储过程提交的支持
        /// <summary>
        /// 事务提交 数据参数是通过实体和 ActionCols 字段对应起来。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmdList">值传入类型对象</param>
        /// <returns>存储过程执行返回结果集</returns>
        public virtual DataSet CommitPrc<T>(Database db, IList<SubmitCommand<T>> cmdList)
        {
            // 0. Result object
            DataSet result = null;

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                try
                {
                    foreach (var cmd in cmdList)
                    {
                        // 1. Create database command
                        DbCommand command = db.GetStoredProcCommand(cmd.ProcName);
                        if (cmd.TimeOut != null && cmd.TimeOut > 30)
                        {
                            command.CommandTimeout = cmd.TimeOut.Value;
                        }
                        // 2. Set import parameter
                        if (!string.IsNullOrEmpty(cmd.ProctParametersName))
                        {
                            SetPter<T>(db, command, cmd.Enty, cmd.ProctParametersName);
                        }

                        // 3. Set outport parameter
                        if (!string.IsNullOrEmpty(cmd.ProctOutParametersName))
                        {
                            SetPterOut<T>(db, command, cmd.Enty, cmd.ProctOutParametersName);
                        }

                        // 4. Execute command
                        result = db.ExecuteDataSet(command);
                    }
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
            return result;
        }

        /// <summary>
        /// 事务提交 数据参数是通过实体和 ActionCols 字段对应起来。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmdList">引用类型对象，如果存储过程有输出参数，输出参数将赋对些对象</param>
        /// <returns>存储过程执行返回结果集</returns>
        public virtual DataSet CommitPrc<T>(Database db, IList<SubmitCommand<T>> cmdList, out T outObj)
        {
            // 0. Result object
            DataSet result = null;
            T _OutObj = default(T);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                try
                {
                    foreach (var cmd in cmdList)
                    {
                        // 1. Create database command
                        DbCommand command = db.GetStoredProcCommand(cmd.ProcName);
                        if (cmd.TimeOut != null && cmd.TimeOut > 30)
                        {
                            command.CommandTimeout = cmd.TimeOut.Value;
                        }

                        // 2. Set import parameter
                        if (!string.IsNullOrEmpty(cmd.ProctParametersName))
                        {
                            SetPter<T>(db, command, cmd.Enty, cmd.ProctParametersName);
                        }

                        // 3. Set outport parameter
                        if (!string.IsNullOrEmpty(cmd.ProctOutParametersName))
                        {
                            SetPterOut<T>(db, command, cmd.Enty, cmd.ProctOutParametersName);
                        }

                        // 4. Execute command
                        result = db.ExecuteDataSet(command);

                        // 5. Get out parameter value and set to object
                        _OutObj = GetProctParametersValue<T>(db, command, cmd.Enty, "ProctOutParametersName");
                    }
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
            outObj = _OutObj;
            return result;
        }
        /// <summary>
        /// 获取SQL指定的参数属性
        /// </summary>
        /// <typeparam name="Tobj"></typeparam>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        /// <param name="cols"></param>
        /// <param name="pList"></param>
        /// <returns></returns>
        private IList<Pter> GetPter<Tobj>(Database db, DbCommand command, Tobj obj, string cols, IList<Pter> pList)
        {
            if (pList == null)
                pList = new List<Pter>();


            string[] list = cols.Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Trim().Trim(new char[] { '[', ']' });
            }

            foreach (PropertyInfo viewProperty in obj.GetType().GetProperties())
            {
                if (viewProperty.CanRead)
                {
                    if (list.Contains<string>(viewProperty.Name))
                    {
                        Pter tempPter = new Pter();
                        tempPter.Name = viewProperty.Name;
                        tempPter.Value = viewProperty.GetValue(obj, null);
                        pList.Add(tempPter);
                    }
                }
            }

            return pList;
        }
        #endregion
    }
}
