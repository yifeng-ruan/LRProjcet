/***********************************************************
* SystemName:	LR.Library
* ModuleName:	公共模块 - 接口公共层
* CreateDate:	2014/6/9 
* Author:	    Ryan.Ruan
* Description:  公共接口
* Currnet Version:	V1.0
***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR.Utils;

namespace LR.BLL.Interface
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IBusinessLogic
    {
        /// <summary>
        /// 获取列表---分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IList<IT> GetPageList<IT, TDB>(IT ent, PageInfo page, ref int total)
            where IT : new()
            where TDB : new();
        IList<IT> GetPageList<IT, TDB>(string name, IT ent, PageInfo page, ref int total)
            where IT : new()
            where TDB : new();

        /// <summary>
        /// 获取列表---不分页
        /// </summary>
        /// <typeparam name="IT"></typeparam>
        /// <typeparam name="TDB"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        IList<IT> GetListByEntity<IT, TDB>(IT ent)
            where IT : new()
            where TDB : new();
        IList<IT> GetListByEntity<IT, TDB>(string name, IT ent)
            where IT : new()
            where TDB : new();

        /// <summary>
        /// 操作数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Save<IT, TDB>(IT ent, SaveType type)
            where IT : new()
            where TDB : new();
        bool Save<IT, TDB>(IList<IT> ents, SaveType type)
            where IT : new()
            where TDB : new();
        bool Save<IT, TDB>(string name, IT ent, SaveType type)
            where IT : new()
            where TDB : new();

        /// <summary>
        ///  获取单行详细信息
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        IT GetEntity<IT, TDB>(IT ent)
            where IT : new()
            where TDB : new();
        IT GetEntity<IT, TDB>(string name, IT ent)
            where IT : new()
            where TDB : new();

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="IT"></typeparam>
        /// <typeparam name="TDB"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        int? ExePrc<IT, TDB>(ref IT ent)
            where IT : new()
            where TDB : new();
    }
}
