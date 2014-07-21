using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Xml;
using System.Data;
using System.Collections;
using System.Web;

using LR.DTO;
using LR.Core;
using LR.Core.UserInfoModule.UserInfoAggregate;
using LR.Core.Base;

using LR.Utils.DBHelper;
using LR.Utils;
using LR.Utils.DataUtils;
using LR.Utils.ASP;

using LR.DAL.Interface;

namespace LR.DAL.Base
{
    /// <summary>
    /// 创建数据库操作继承类
    /// </summary>
    public class BaseOperate : BaseDAL, IDataAccess
    {
        #region General 系列
        /// <summary>
        /// 通过GetListByPageGeneral来取出一个实体
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual T GetEntityByProcGeneral<T>(T ent) where T : new()
        {
            int total = 0;
            IList<T> listResult = GetListByPageGeneral<T>(ent, 1, 1, ref total);

            if (listResult.Count > 0)
            {
                return listResult[0];
            }
            else
            {
                return new T();
            }
        }
        /// <summary>
        /// 通过存储过程来获取数据列表，存储过程必须采用通用参数
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IList<T> GetListByPageGeneral<T>(T ent, int pageindex, int pagesize, ref int total) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            baseEnt.isProc = true;
            if (string.IsNullOrEmpty(baseEnt.xmlStr))
            {
                baseEnt.GenerateXml();
            }
            if (string.IsNullOrEmpty(baseEnt.ProcName))
            {
                baseEnt.ProcName = baseEnt.TableName + "_Select_SP";//命名规则：表名+"_Select_SP"
            }
            if (string.IsNullOrEmpty(baseEnt.ProctOutParametersName))
            {
                baseEnt.ProctOutParametersName = WorkspacesProc.GeneralSelectOutParams;
            }
            if (string.IsNullOrEmpty(baseEnt.ProctParametersName))
            {
                baseEnt.ProctParametersName = WorkspacesProc.GeneralSelectParams;
            }

            T temp = default(T);
            return GetListByPrc<T>(ent, pageindex, pagesize, out total, out temp);
        }
        public IList<T> GetListByPageGeneral<T>(T ent, PageInfo pi, ref int total) where T : new()
        {
            return GetListByPageGeneral<T>(ent, pi.PageIndex, pi.PageSize, ref total);
        }
        private bool ExecProcGeneral<T>(T ent) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            baseEnt.isProc = true;
            if (string.IsNullOrEmpty(baseEnt.xmlStr))
            {
                SetDateAndUserInfo(baseEnt);
                baseEnt.GenerateXml();
            }
            else
            {
                AppendDateAndUserInfoToXmlStr(baseEnt);
            }

            if (string.IsNullOrEmpty(baseEnt.ProctOutParametersName))
            {
                baseEnt.ProctOutParametersName = WorkspacesProc.GeneralSaveOutParams;
            }
            if (string.IsNullOrEmpty(baseEnt.ProctParametersName))
            {
                baseEnt.ProctParametersName = WorkspacesProc.GeneralSaveParams;
            }

            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();//ActionCols = baseEnt.ProctParametersName,
            list.Add(new SubmitCommand<T> { Enty = ent, ProcName = baseEnt.ProcName, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName, isProc = true });
            return base.CommitGeneral<T>(db, list);
        }
        /// <summary>
        /// 新增实体_存储过程方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public bool AddGeneral<T>(T ent) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (string.IsNullOrEmpty(baseEnt.ProcName))
            {
                baseEnt.ProcName = baseEnt.TableName + "_Add_SP";//命名规则：表名+"_Add_SP"
            }
            return ExecProcGeneral<T>(ent);
        }
        /// <summary>
        /// 修改实体_存储过程方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public bool UpdateGeneral<T>(T ent) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (string.IsNullOrEmpty(baseEnt.ProcName))
            {
                baseEnt.ProcName = baseEnt.TableName + "_Update_SP";//命名规则：表名+"_Update_SP"
            }
            return ExecProcGeneral<T>(ent);
        }
        /// <summary>
        /// 删除实体_存储过程方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public bool DeleteGeneral<T>(T ent) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (string.IsNullOrEmpty(baseEnt.ProcName))
            {
                baseEnt.ProcName = baseEnt.TableName + "_Delete_SP";//命名规则：表名+"_Delete_SP"
            }
            return ExecProcGeneral<T>(ent);
        }
        /// <summary>
        /// 删除实体_存储过程方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public bool AddUpdateDeleteGeneral<T>(T ent) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (string.IsNullOrEmpty(baseEnt.ProcName))
            {
                baseEnt.ProcName = baseEnt.TableName + "_XML_Save_SP";//命名规则：表名+"_XML_Save_SP"
            }
            return ExecProcGeneral<T>(ent);
        }

        /// <summary>
        /// 把 ActiveDate,InactiveDate,CreatedBy,CreateDate,LastUpdatedBy,LastUpdateDate,LastUpdateLogin
        /// 的信息增加到Entity的xmlStr中
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AppendDateAndUserInfoToXmlStr(BaseEntity entity)
        {
            if (string.IsNullOrEmpty(entity.xmlStr)) return;
            string sNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (System.Web.HttpContext.Current.Session["CurrentUser"] == null)
            {
                throw new Exception("用户未登录或超过登录时间，请重新登录！");
            }
            UserInfoEntity userEntity = (UserInfoEntity)System.Web.HttpContext.Current.Session["CurrentUser"];

            string sUserID = userEntity.UserID.ToString();
            string sUserLoginCode = userEntity.LastUpdateLogin;

            XmlHelper xh = new XmlHelper(entity.xmlStr);

            XmlNodeList xnlTable = xh.GetNode("root").ChildNodes;
            XmlElement xmlElement = null;
            foreach (XmlNode node in xnlTable)
            {
                if (!XmlHelper.HasChildNode(node, "ActiveDate"))
                {
                    xmlElement = xh.Document.CreateElement("ActiveDate");
                    xmlElement.InnerText = sNow;
                    node.AppendChild(xmlElement);
                }
                if (!XmlHelper.HasChildNode(node, "InactiveDate"))
                {
                    xmlElement = xh.Document.CreateElement("InactiveDate");
                    xmlElement.InnerText = sNow;
                    node.AppendChild(xmlElement);
                }
                if (!XmlHelper.HasChildNode(node, "CreatedBy"))
                {
                    xmlElement = xh.Document.CreateElement("CreatedBy");
                    xmlElement.InnerText = sUserID;
                    node.AppendChild(xmlElement);
                }
                if (!XmlHelper.HasChildNode(node, "CreateDate"))
                {
                    xmlElement = xh.Document.CreateElement("CreateDate");
                    xmlElement.InnerText = sNow;
                    node.AppendChild(xmlElement);
                }
                if (!XmlHelper.HasChildNode(node, "LastUpdatedBy"))
                {
                    xmlElement = xh.Document.CreateElement("LastUpdatedBy");
                    xmlElement.InnerText = sUserID;
                    node.AppendChild(xmlElement);
                }
                if (!XmlHelper.HasChildNode(node, "LastUpdateDate"))
                {
                    xmlElement = xh.Document.CreateElement("LastUpdateDate");
                    xmlElement.InnerText = sNow;
                    node.AppendChild(xmlElement);
                }
                if (!XmlHelper.HasChildNode(node, "LastUpdateLogin"))
                {
                    xmlElement = xh.Document.CreateElement("LastUpdateLogin");
                    xmlElement.InnerText = sUserLoginCode;
                    node.AppendChild(xmlElement);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<root>");
            foreach (XmlNode node in xnlTable)
            {
                sb.Append(node.OuterXml);
            }
            sb.Append("</root>");
            entity.xmlStr = sb.ToString();
        }
        public virtual void SetDateAndUserInfo(BaseEntity entity)
        {
            if (string.IsNullOrEmpty(entity.LastUpdateLogin) && string.IsNullOrEmpty(entity.LastUpdateLoginCode))
            {
                //设置7个字段的值
                UserInfoEntity userEntity = (UserInfoEntity)System.Web.HttpContext.Current.Session["CurrentUser"];
                string sUserID = userEntity.UserID.ToString();
                entity.CreatedBy = sUserID;
                entity.CreateBy = sUserID;
                entity.CreationDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                entity.LastUpdatedBy = sUserID;
                entity.LastUpdateBy = sUserID;
                entity.LastUpdateDate = DateTime.Now;
                entity.LastUpdateLogin = userEntity.LastUpdateLogin;
                entity.LastUpdateLoginCode = userEntity.LastUpdateLogin;
            }
            if (entity.ActiveDate == null)
            {
                entity.ActiveDate = DateTime.Now;
            }
            if (entity.InactiveDate == null)
            {
                entity.InactiveDate = DateTime.Now;
            }
        }

        #endregion General 系列

        #region 【检查字段是否存在】

        public virtual bool HasExistName<T>(T ent)
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._ActiveSelectCols))
            {
                string selectCol = baseEnt._ActiveSelectCols;
                string[] whereCol = baseEnt._SelectWhereCols.Split(',');

                string id = baseEnt._UniqueName;
                string sqlText = "";
                if (whereCol.Length == 2)
                {
                    //通过传进来的惟一标识进行区别，并重新组成SQL语句
                    if (baseEnt._Sql == "or")
                    {
                        sqlText = SQLHasExist.hasExistSqlText(baseEnt._DBTable, selectCol, whereCol[0].Trim().TrimStart('[').TrimEnd(']'), whereCol[1].Trim().TrimStart('[').TrimEnd(']'), baseEnt._Sql);
                    }
                    else if (baseEnt._Sql == "and")
                    {
                        sqlText = SQLHasExist.hasExistSqlText(baseEnt._DBTable, selectCol, whereCol[0].Trim().TrimStart('[').TrimEnd(']'), whereCol[1].Trim().TrimStart('[').TrimEnd(']'), baseEnt._Sql);
                    }
                    else
                    {
                        sqlText = SQLHasExist.hasExistSqlText(baseEnt._DBTable, selectCol, whereCol[0].Trim().TrimStart('[').TrimEnd(']'), whereCol[1].Trim().TrimStart('[').TrimEnd(']'));
                    }
                }
                else if (whereCol.Length == 1)
                {
                    sqlText = SQLHasExist.hasExistSqlText(baseEnt._DBTable, selectCol, whereCol[0].Trim().TrimStart('[').TrimEnd(']'), "");
                }

                DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

                return HasExist<T>(dbCommand, ent, baseEnt._SelectWhereCols);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region【增、删、改操作】

        #region【新增】

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Insert<T>(T ent)
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._ActiveInsertCols))
            {
                string col = baseEnt._ActiveInsertCols;

                string sqlCommand = SQLUtility.InsertText(baseEnt._DBTable, col);

                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ent, Sql = sqlCommand });
                return Commit<T>(list);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Insert<T>(IList<T> ents)
        {
            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
            for (int i = 0; i < ents.Count; i++)
            {
                BaseEntity baseEnt = ents[i] as BaseEntity;
                if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._ActiveInsertCols))
                {
                    string col = baseEnt._ActiveInsertCols;

                    string sqlCommand = SQLUtility.InsertText(baseEnt._DBTable, col);

                    list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ents[i], Sql = sqlCommand });
                }
                else
                {
                    return false;
                }

            }
            return Commit<T>(list);
        }

        #endregion

        #region【删除】

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Delete<T>(T ent)
        {
            bool result = false;

            BaseEntity baseEnt = ent as BaseEntity;
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._DeleteWhereCols))
            {
                string sqlCommand = SQLUtility.DeleteText(baseEnt._DBTable) + SQLUtility.WhereText(baseEnt._DeleteWhereCols);

                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();

                //添加
                list.Add(new SubmitCommand<T> { ActionCols = baseEnt._DeleteWhereCols, Enty = ent, Sql = sqlCommand });

                result = Commit<T>(list);
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Delete<T>(IList<T> ents)
        {
            bool result = false;
            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
            for (int i = 0; i < ents.Count; i++)
            {
                #region sql
                BaseEntity baseEnt = ents[i] as BaseEntity;
                if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._DeleteWhereCols))
                {

                    string sqlCommand = SQLUtility.DeleteText(baseEnt._DBTable) + SQLUtility.WhereText(baseEnt._DeleteWhereCols);
                #endregion
                    //添加
                    list.Add(new SubmitCommand<T> { ActionCols = baseEnt._DeleteWhereCols, Enty = ents[i], Sql = sqlCommand });
                }
                else
                {
                    return false;
                }
            }
            result = Commit<T>(list);
            return result;
        }

        #endregion

        #region【修改】
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Update<T>(T ent)
        {
            bool result = false;

            BaseEntity baseEnt = ent as BaseEntity;
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._ActiveUpdateCols))
            {
                string col = baseEnt._ActiveUpdateCols;
                string sqlCommand = SQLUtility.UpdateText(baseEnt._DBTable, col);
                sqlCommand += SQLUtility.WhereText(baseEnt._UpdateWhereCols);

                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();

                //添加
                list.Add(new SubmitCommand<T> { ActionCols = baseEnt._ActiveUpdateCols + "," + baseEnt._UpdateWhereCols, Enty = ent, Sql = sqlCommand });

                result = Commit<T>(list);
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量编辑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool Update<T>(IList<T> ents)
        {
            bool result = false;
            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
            for (int i = 0; i < ents.Count; i++)
            {
                #region sql
                BaseEntity baseEnt = ents[i] as BaseEntity;
                if (baseEnt != null && !string.IsNullOrEmpty(baseEnt._DBTable) && !string.IsNullOrEmpty(baseEnt._ActiveUpdateCols))
                {

                    string col = baseEnt._ActiveUpdateCols;
                    string sqlCommand = SQLUtility.UpdateText(baseEnt._DBTable, col);
                    sqlCommand += SQLUtility.WhereText(baseEnt._UpdateWhereCols);

                #endregion
                    //添加
                    list.Add(new SubmitCommand<T> { ActionCols = baseEnt._ActiveUpdateCols + "," + baseEnt._UpdateWhereCols, Enty = ents[i], Sql = sqlCommand });
                }
                else
                {
                    return false;
                }
            }
            result = Commit<T>(list);
            return result;
        }

        #endregion

        #endregion

        #region【存储过程】
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual bool ExePrc<T>(T ent)
        {

            BaseEntity baseEnt = ent as BaseEntity;
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = baseEnt.ProctParametersName;
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { TimeOut = baseEnt.Timeout, ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, ProctParametersName = baseEnt.ProctParametersName, isProc = true });
                return Commit<T>(list);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>    bool ExePrc<T>(ref IList<T> ent) where T : new();
        public virtual bool ExePrc<T>(IList<T> ents)
        {
            bool result = false;
            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
            for (int i = 0; i < ents.Count; i++)
            {
                BaseEntity baseEnt = ents[i] as BaseEntity;
                if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
                {
                    string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                    list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ents[i], ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });
                }
                else
                {
                    return false;
                }
            }
            result = Commit<T>(list);
            return result;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual DataSet ExePrcEx<T>(T ent) where T : new()
        {

            BaseEntity baseEnt = ent as BaseEntity;

            string sqlCommand = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { TimeOut = baseEnt.Timeout, ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });

                return CommitPrc<T>(list);
            }
            else
            {
                return null;
            }
        }

        // 添加对存储过程提交的支持
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual DataSet ExePrcEx<T>(T ent, out T outObj)
        {
            BaseEntity baseEnt = ent as BaseEntity;
            T _OutObj = default(T);

            string sqlCommand = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { TimeOut = baseEnt.Timeout, ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });

                DataSet reslut = CommitPrc<T>(list, out _OutObj);
                outObj = _OutObj;
                return reslut;
            }
            else
            {
                outObj = _OutObj;
                return null;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual IList<T> GetListByPrc<T>(T ent) where T : new()
        {

            BaseEntity baseEnt = ent as BaseEntity;

            string sqlCommand = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });

                DataSet dsResult = CommitPrc<T>(list);

                if (dsResult == null || dsResult.Tables == null || dsResult.Tables.Count <= 0 || dsResult.Tables[0].Rows.Count <= 0)
                {
                    // ****** Change by Gary.Xie 2010-11-24 ******** //
                    // return null;    // Old code
                    return new List<T>();
                    // ****** Change end ******** //
                }

                IList<T> reslut = TypeConvert.DataTableToIList<T>(dsResult.Tables[0]);
                //--处理7个公共字段
                HandlePublicDBField<T>(reslut);
                return reslut;
            }
            else
            {
                return null;
            }
        }


        // 添加对存储过程提交的支持
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual IList<T> GetListByPrc<T>(T ent, out T outObj) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            T _OutObj = default(T);

            string sqlCommand = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });

                DataSet dsResult = CommitPrc<T>(list, out _OutObj);

                if (dsResult == null || dsResult.Tables == null || dsResult.Tables.Count <= 0 || dsResult.Tables[0].Rows.Count <= 0)
                {
                    outObj = _OutObj;

                    // ****** Change by Gary.Xie 2010-11-24 ******** //
                    // return null;    // Old code
                    return new List<T>();
                    // ****** Change end ******** //
                }

                IList<T> reslut = TypeConvert.DataTableToIList<T>(dsResult.Tables[0]);
                outObj = _OutObj;
                //--处理7个公共字段
                HandlePublicDBField<T>(reslut);
                return reslut;
            }
            else
            {
                outObj = _OutObj;
                return null;
            }
        }

        /// <summary>
        /// 调用存储过程返回DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual DataSet GetDataSet<T>(T ent)
        {
            BaseEntity baseEnt = ent as BaseEntity;
            //T _OutObj = default(T);

            DataSet dsResult = null;
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });

                dsResult = CommitPrc<T>(list);

            }
            return dsResult;
        }

        // 添加对存储过程提交的支持
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual IList<T> GetListByPrc<T>(T ent, int pageIndex, int pageSize, out int total, out T outObj) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            T _OutObj = default(T);

            (ent as BaseEntity).PageIndex = pageIndex;
            (ent as BaseEntity).PageSize = pageSize;

            string sqlCommand = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
            if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
                list.Add(new SubmitCommand<T>
                {
                    ActionCols = col,
                    Enty = ent,
                    ProcName = baseEnt.ProcName,
                    isProc = true,
                    ProctParametersName = baseEnt.ProctParametersName,
                    ProctOutParametersName = baseEnt.ProctOutParametersName,
                    TimeOut = baseEnt.TimeOut
                });

                DataSet dsResult = CommitPrc<T>(list, out _OutObj);

                if (dsResult == null || dsResult.Tables == null || dsResult.Tables.Count <= 0 || dsResult.Tables[0].Rows.Count <= 0)
                {
                    total = 0;
                    outObj = _OutObj;

                    // ****** Change by Gary.Xie 2010-11-24 ******** //
                    // return null;    // Old code
                    return new List<T>();
                    // ****** Change end ******** //


                }

                IList<T> reslut = TypeConvert.DataTableToIList<T>(dsResult.Tables[0]);
                total = (_OutObj == null || !(_OutObj as BaseEntity).Total.HasValue) ? 0 : (_OutObj as BaseEntity).Total.Value;
                outObj = _OutObj;
                //--处理7个公共字段
                HandlePublicDBField<T>(reslut);
                return reslut;
            }
            else
            {
                total = 0;
                outObj = _OutObj;
                return null;
            }
        }
        // ************ Add end *******************************
        #endregion

        #region【获取单个实体】
        /// <summary>
        /// 获取某一条记录 
        /// 增加存储过程操作 update by brian.chen 20120313
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual TE GetEntityInfo<TE>(TE ent) where TE : new()
        {

            BaseEntity baseEnt = ent as BaseEntity;
            if (!string.IsNullOrEmpty(baseEnt._UniqueName))
            {
                string sqlText = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
                sqlText = sqlText + SQLUtility.WhereText(baseEnt._UniqueName);

                DbCommand dbCommand = db.GetSqlStringCommand(sqlText);

                return GetEnty<TE>(dbCommand, ent, baseEnt._UniqueName, baseEnt._ActiveSelectCols);
            }
            else if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            {
                return GetEntyInfo<TE>(ent);
            }
            else
            {
                return new TE();
            }
        }

        /// <summary>
        /// 调用存储过程返回实体类
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual TE GetEntyInfo<TE>(TE ent) where TE : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            TE _OutObj = default(TE);
            //if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
            //{
            string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
            IList<SubmitCommand<TE>> list = new List<SubmitCommand<TE>>();
            list.Add(new SubmitCommand<TE> { ActionCols = col, Enty = ent, ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });

            DataSet dsResult = CommitPrc<TE>(list, out _OutObj);

            IList<TE> reslut = TypeConvert.DataTableToIList<TE>(dsResult.Tables[0]);
            if (reslut.Count() > 0)
            {
                return reslut.First();
            }
            else
            {
                return new TE();
            }
            //}
            //else
            //{
            //    return new TE();
            //}
        }
        #endregion

        #region【获取实体列表】


        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IList<T> GetListByPage<T>(T ent, int pageindex, int pagesize, out int total) where T : new()
        {

            BaseEntity baseEnt = ent as BaseEntity;
            if (!baseEnt.isProc)
            {
                string cols = baseEnt._ActiveSelectCols;

                //非自定义条件语句
                if (!string.IsNullOrEmpty(baseEnt._SelfWhere))
                {
                    return base.GetListByPage<T>(baseEnt._DBTable, baseEnt._UniqueName, pageindex, pagesize, baseEnt._SelectWhereCols, ent, baseEnt._SelfWhere, baseEnt._SelectOrder, cols, cols, out total);
                }
                else
                {
                    return base.GetListByPage<T>(baseEnt._DBTable, baseEnt._UniqueName, pageindex, pagesize, baseEnt._SelectWhereCols, ent, baseEnt._SelectOrder, cols, cols, out total);
                }
            }
            else
            {
                T temp = default(T);
                return GetListByPrc<T>(ent, pageindex, pagesize, out total, out temp);

                // return GetListByPage<T>(baseEnt.ProcName, baseEnt._DBTable, baseEnt.PrimaryKey, baseEnt._SelectOrder, pageindex, pagesize, baseEnt._SelectWhereCols, baseEnt._SelfWhere, baseEnt.Group, baseEnt._SelectWhereCols, out total);
            }
        }


        /// <summary>
        /// 获取数据不分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        protected IList<T> GetList<T>(T ent) where T : new()
        {
            BaseEntity baseEnt = ent as BaseEntity;
            if (!baseEnt.isProc)
            {
                string cols = SQLUtility.SelectText(baseEnt._DBTable, baseEnt._ActiveSelectCols);
                if (!string.IsNullOrEmpty(baseEnt._SelectWhereCols))
                {
                    cols += SQLUtility.WhereText(baseEnt._SelectWhereCols) + baseEnt._SelfWhere;
                    if (!string.IsNullOrEmpty(baseEnt._SelectOrder))
                        cols += string.Format(" Order By {0}", baseEnt._SelectOrder);

                    return base.GetList<T>(cols, baseEnt._ActiveSelectCols, ent, baseEnt._SelectWhereCols);
                }
                else
                {
                    cols += baseEnt._SelfWhere;
                    if (!string.IsNullOrEmpty(baseEnt._SelectOrder))
                        cols += string.Format(" Order By {0}", baseEnt._SelectOrder); //modiy by Gary 2011-3-3，Old Code:  cols += baseEnt._SelfWhere+baseEnt._SelectOrder;

                    IList<Pter> wList = new List<Pter>();
                    return base.GetList<T>(cols, baseEnt._ActiveSelectCols, wList);
                }
            }
            else
            {
                T temp = default(T);
                return GetListByPrc<T>(ent, out temp);
            }
        }
        // ************ Add end *******************************
        #endregion

        #region 【IDataAccess Members】

        /// <summary>
        /// 获取分页数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public virtual IList<T> GetPageList<T>(T ent, PageInfo page, ref int total) where T : new()
        {
            return this.GetListByPage<T>(ent, page.PageIndex, page.PageSize, out  total);
        }

        public virtual IList<T> GetPageList<T>(string name, T ent, PageInfo page, ref int total) where T : new()
        {
            //return GetListByPage<T>(ent, page.PageIndex, page.PageSize, out  total);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取不分页数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public virtual IList<T> GetListByEntity<T>(T ent) where T : new()
        {
            return this.GetList<T>(ent);
        }

        public virtual IList<T> GetListByEntity<T>(string name, T ent) where T : new()
        {
            throw new NotImplementedException();
        }

        public IList<T> GetPageListByPrc<T>(T ent, PageInfo page, out int total, out T obj) where T : new()
        {
            return GetListByPrc<T>(ent, page.PageIndex, page.PageSize, out  total, out obj);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <typeparam name="IList"></typeparam>
        /// <param name="ent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Save<T>(IList<T> ents, SaveType type) where T : new()
        {
            if (type == SaveType.Insert)
            {
                return Insert<T>(ents);
            }
            else if (type == SaveType.Delete)
            {
                return Delete<T>(ents);
            }
            else if (type == SaveType.Update)
            {
                return Update<T>(ents);
            }
            else if (type == SaveType.ExePrc)
            {
                return ExePrc<T>(ents);
            }
            return false;
        }
        /// <summary>
        /// 批量保存方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Save<T>(T ent, SaveType type) where T : new()
        {
            if (type == SaveType.Insert)
            {
                return Insert<T>(ent);
            }
            else if (type == SaveType.Delete)
            {
                return Delete<T>(ent);
            }
            else if (type == SaveType.Update)
            {
                return Update<T>(ent);
            }
            else if (type == SaveType.ExePrc)
            {
                return ExePrc<T>(ent);
            }
            return false;
        }
        /// <summary>
        /// 保存方法抛出异常信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="ent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Save<T>(string name, T ent, SaveType type) where T : new()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public T GetEntity<T>(T ent) where T : new()
        {
            return GetEntityInfo<T>(ent);
        }

        public virtual T GetEntity<T>(string name, T ent) where T : new()
        {
            throw new NotImplementedException();
            //return GetEntityInfo<T>(ent);
        }

        public bool HasExist<T>(T ent) where T : new()
        {
            return HasExistName<T>(ent);
        }
        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="ent"></param>
        /// <returns>Message code</returns>
        public int? ExePrc<T>(ref T ent) where T : new()
        {
            // 0. Declare value
            T _OutObj = default(T);

            // 1. Execute stored procedure
            GetListByPrc<T>(ent, out _OutObj);

            // 2. Set ref ent
            ent = _OutObj;

            // End. Return value
            if (_OutObj == null)
                return null;
            else
                return (_OutObj as BaseEntity).ProcMessageCode;
        }

        /// <summary>
        /// 批量执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>    bool ExePrc<T>(ref IList<T> ent) where T : new();
        public bool ExePrc<T>(ref IList<T> ents) where T : new()
        {
            bool result = false;
            IList<SubmitCommand<T>> list = new List<SubmitCommand<T>>();
            for (int i = 0; i < ents.Count; i++)
            {
                BaseEntity baseEnt = ents[i] as BaseEntity;
                if (baseEnt != null && !string.IsNullOrEmpty(baseEnt.ProcName) && baseEnt.isProc)
                {
                    string col = string.Format("{0},{1}", baseEnt.ProctParametersName, baseEnt.ProctOutParametersName);
                    list.Add(new SubmitCommand<T> { ActionCols = col, Enty = ents[i], ProcName = baseEnt.ProcName, isProc = true, ProctParametersName = baseEnt.ProctParametersName, ProctOutParametersName = baseEnt.ProctOutParametersName });
                }
                else
                {
                    return false;
                }
            }
            result = Commit<T>(list);
            return result;
        }
        // ************ Add end ***********************************

        #endregion

        #region 【处理数据库7个公共字段的显示问题 】
        /// <summary>
        /// 处理数据库7个公共字段的显示问题
        /// </summary>
        /// <param name="entityList"></param>
        private void HandlePublicDBField<T>(IList<T> entityList) where T : new()
        {
            //缓存首次出现的名称，以便下次再次出现时使用
            Dictionary<string, string> cachUserNameMapping = new Dictionary<string, string>();
            if (entityList != null)
            {
                for (int n = 0; n < entityList.Count; n++)
                {
                    #region 处理用户ID转换名称-anyuan.xiao 2011-5-5
                    BaseEntity baseEnt = entityList[n] as BaseEntity;
                    if (baseEnt != null)
                    {
                        try
                        {
                            #region 处理创建人ID转换为名称
                            #region 旧规范-CreateBy
                            if (!string.IsNullOrEmpty(baseEnt.CreateBy))
                            {
                                int userId;
                                if (int.TryParse(baseEnt.CreateBy, out userId))
                                {
                                    if (!cachUserNameMapping.ContainsKey(userId.ToString()))
                                    {//如果用户ID以前未出现过，则从缓存中读取对应的用户名称，并缓存起来，以备下次再次出现时用
                                        var tempUser = (from u in (IList<UserInfoEntity>)CacheOperator.GetCache("UserInfo") where u.UserID == userId select u).ToList<UserInfoEntity>();
                                        if (tempUser.Count > 0)
                                        {
                                            baseEnt.CreateByDisplay = string.Format("{0}[{1}]", tempUser[0].ChineseName, tempUser[0].UserLoginName);
                                            cachUserNameMapping[userId.ToString()] = baseEnt.CreateByDisplay;
                                        }
                                    }
                                    else
                                        baseEnt.CreateByDisplay = cachUserNameMapping[userId.ToString()];
                                }
                                else
                                    baseEnt.CreateByDisplay = baseEnt.CreateBy; //-兼容旧的代码
                            }
                            #endregion
                            #region 新规范-CreatedBy
                            if (!string.IsNullOrEmpty(baseEnt.CreatedBy))
                            {
                                int userId;
                                if (int.TryParse(baseEnt.CreatedBy, out userId))
                                {
                                    if (!cachUserNameMapping.ContainsKey(userId.ToString()))
                                    {//如果用户ID以前未出现过，则从缓存中读取对应的用户名称，并缓存起来，以备下次再次出现时用
                                        var tempUser = (from u in (IList<UserInfoEntity>)CacheOperator.GetCache("UserInfo") where u.UserID == userId select u).ToList<UserInfoEntity>();
                                        if (tempUser.Count > 0)
                                        {
                                            baseEnt.CreatedByDisplay = string.Format("{0}[{1}]", tempUser[0].ChineseName, tempUser[0].UserLoginName);
                                            cachUserNameMapping[userId.ToString()] = baseEnt.CreatedByDisplay;
                                        }
                                    }
                                    else
                                        baseEnt.CreatedByDisplay = cachUserNameMapping[userId.ToString()];
                                }
                                else
                                    baseEnt.CreatedByDisplay = baseEnt.CreatedBy; //-兼容旧的代码
                            }
                            #endregion
                            #endregion

                            #region 处理最后更新人ID转换为名称
                            #region 旧规范-LastUpdateBy
                            if (!string.IsNullOrEmpty(baseEnt.LastUpdateBy))
                            {
                                int userId;
                                if (int.TryParse(baseEnt.LastUpdateBy, out userId))
                                {
                                    if (!cachUserNameMapping.ContainsKey(userId.ToString()))
                                    {//如果用户ID以前未出现过，则从缓存中读取对应的用户名称，并缓存起来，以备下次再次出现时用
                                        var tempUser = (from u in (IList<UserInfoEntity>)CacheOperator.GetCache("UserInfo") where u.UserID == userId select u).ToList<UserInfoEntity>();
                                        if (tempUser.Count > 0)
                                        {
                                            baseEnt.LastUpdateByDisplay = string.Format("{0}[{1}]", tempUser[0].ChineseName, tempUser[0].UserLoginName);
                                            cachUserNameMapping[userId.ToString()] = baseEnt.LastUpdateByDisplay;
                                        }
                                    }
                                    else
                                        baseEnt.LastUpdateByDisplay = cachUserNameMapping[userId.ToString()];
                                }
                                else
                                    baseEnt.LastUpdateByDisplay = baseEnt.LastUpdateBy; //-兼容旧的代码
                            }
                            #endregion
                            #region 新规范-LastUpdatedBy
                            if (!string.IsNullOrEmpty(baseEnt.LastUpdatedBy))
                            {
                                int userId;
                                if (int.TryParse(baseEnt.LastUpdatedBy, out userId))
                                {
                                    if (!cachUserNameMapping.ContainsKey(userId.ToString()))
                                    {//如果用户ID以前未出现过，则从缓存中读取对应的用户名称，并缓存起来，以备下次再次出现时用
                                        var tempUser = (from u in (IList<UserInfoEntity>)CacheOperator.GetCache("UserInfo") where u.UserID == userId select u).ToList<UserInfoEntity>();
                                        if (tempUser.Count > 0)
                                        {
                                            baseEnt.LastUpdatedByDisplay = string.Format("{0}[{1}]", tempUser[0].ChineseName, tempUser[0].UserLoginName);
                                            cachUserNameMapping[userId.ToString()] = baseEnt.LastUpdatedByDisplay;
                                        }
                                    }
                                    else
                                        baseEnt.LastUpdatedByDisplay = cachUserNameMapping[userId.ToString()];
                                }
                                else
                                    baseEnt.LastUpdatedByDisplay = baseEnt.LastUpdatedBy; //-兼容旧的代码
                            }
                            #endregion
                            #endregion
                        }
                        catch
                        {
                            //NO Handle
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion
    }
}
