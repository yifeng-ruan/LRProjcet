/***********************************************************
* SystemName:	LR.BusinessLogic
* ModuleName:	公共模块 - 公共业务逻辑层
* CreateDate:	2014/6/10 
* Author:	    Ryan.Ruan
* Description:  操作数据业务逻辑方法
* Currnet Version:	V1.0
***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR.BLL.Interface;
using LR.Utils;
using System.Data;
using LR.DAL.Interface;
using LR.DAL.UserDAL;
using LR.DAL.DemoDAL;

///继承实现接口方法
namespace LR.BLL.Implementation
{
    /// <summary>
    /// 继承接口，实现数据处理方法
    /// </summary>
    public class BaseBLL : IBusinessLogic
    {

        #region 【数据访问执行方法】
        #region General 系列方法
        /// <summary>
        ///分页获取数据列表--通用方法系列
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <param name="page">分页</param>
        /// <param name="total">参数总数</param>
        /// <returns>返回值</returns>
        public IList<IT> GetListByPageGeneral<IT, TDB>(IT ent, PageInfo page, ref int total)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetListByPageGeneral<IT>(ent, page, ref total);
        }
        /// <summary>
        /// 取出一个实体,通过GetListByPageGeneral来取数据的
        /// </summary>
        /// <typeparam name="TE">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <returns>返回值</returns>
        public TE GetEntityByProcGeneral<TE, TDB>(TE ent)
            where TE : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetEntityByProcGeneral<TE>(ent);
        }


        public bool AddGeneral<T, TDB>(T ent)
            where T : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.AddGeneral<T>(ent);
        }
        public bool UpdateGeneral<T, TDB>(T ent)
            where T : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.UpdateGeneral<T>(ent);
        }
        public bool DeleteGeneral<T, TDB>(T ent)
            where T : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.DeleteGeneral<T>(ent);
        }
        public bool AddUpdateDeleteGeneral<T, TDB>(T ent)
            where T : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.AddUpdateDeleteGeneral<T>(ent);
        }

        #endregion General 系列方法


        /// <summary>
        /// 获取列表---分页
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <param name="page">分页信息</param>
        /// <param name="total">返回列表总条数</param>
        /// <returns>返回列表数据记录</returns>
        public IList<IT> GetPageList<IT, TDB>(IT ent, PageInfo page, ref int total)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetPageList<IT>(ent, page, ref total);
        }
        public IList<IT> GetPageList<IT, TDB>(string name, IT ent, PageInfo page, ref int total)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetPageList<IT>(name, ent, page, ref total);
        }

        /// <summary>
        /// 获取列表---不分页
        /// </summary>
        /// <typeparam name="TIt">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <returns>返回列表数据记录</returns>
        public IList<TIt> GetListByEntity<TIt, TDB>(TIt ent)
            where TIt : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetListByEntity<TIt>(ent);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="name">参数</param>
        /// <param name="ent">查询类实例信息</param>
        /// <returns>返回值</returns>
        public IList<IT> GetListByEntity<IT, TDB>(string name, IT ent)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetListByEntity<IT>(name, ent);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <param name="type">数据操作类型</param>
        /// <returns>true/false</returns>
        public bool Save<IT, TDB>(IT ent, SaveType type)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.Save<IT>(ent, type);
        }
        public bool Save<IT, TDB>(IList<IT> ents, SaveType type)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.Save<IT>(ents, type);
        }

        public bool Save<IT, TDB>(string name, IT ent, SaveType type)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.Save<IT>(name, ent, type);
        }

        /// <summary>
        /// 获取单行详细信息
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <returns></returns>
        public IT GetEntity<IT, TDB>(IT ent)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetEntity<IT>(ent);
        }
        public IT GetEntity<IT, TDB>(string name, IT ent)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.GetEntity<IT>(name, ent);
        }

        /// <summary>
        /// 检查字段是否存在
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <returns></returns>
        public bool HasExist<IT, TDB>(IT ent)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.HasExist<IT>(ent);
        }

        /// <summary>
        /// 执行程序过程
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <returns>返回MessageCode,如无则为空</returns>
        public int? ExePrc<IT, TDB>(ref IT ent)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.ExePrc<IT>(ref ent);
        }

        /// <summary>
        /// 执行程序过程
        /// </summary>
        /// <typeparam name="IT">查询类</typeparam>
        /// <typeparam name="TDB">要执行的数据层类</typeparam>
        /// <param name="ent">查询类实例信息</param>
        /// <returns>返回MessageCode,如无则为空</returns>
        public bool ExePrc<IT, TDB>(ref IList<IT> ents)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.ExePrc<IT>(ref ents);
        }

        public DataSet ExePrcEx<IT, TDB>(IT ent)
            where IT : new()
            where TDB : new()
        {
            var iDa = new TDB() as IDataAccess;
            return iDa.ExePrcEx<IT>(ent);
        }

        #endregion
    }
}
