using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LR.Utils;

namespace LR.DAL.Interface
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IDataAccess
    {
        #region General 系列方法
        /// <summary>
        /// 分页获取数据列表--通用方法系列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IList<T> GetListByPageGeneral<T>(T ent, PageInfo page, ref int total) where T : new();
        /// <summary>
        /// 取出一个实体,通过GetListByPageGeneral来取数据的
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        T GetEntityByProcGeneral<T>(T ent) where T : new();


        bool AddGeneral<T>(T ent) where T : new();
        bool UpdateGeneral<T>(T ent) where T : new();
        bool DeleteGeneral<T>(T ent) where T : new();
        bool AddUpdateDeleteGeneral<T>(T ent) where T : new();

        #endregion General 系列方法


        /// <summary>
        /// 获取列表---分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        IList<T> GetPageList<T>(T ent, PageInfo page, ref int total) where T : new();

        /// <summary>
        /// 获取列表---分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">调用的方法名称</param>
        /// <param name="ent"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IList<T> GetPageList<T>(string name, T ent, PageInfo page, ref int total) where T : new();

        /// <summary>
        /// 获取列表---不分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent">过滤条件实体</param>
        /// <returns></returns>
        IList<T> GetListByEntity<T>(T ent) where T : new();

        /// <summary>
        /// 获取列表---不分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">调用的方法名称</param>
        /// <param name="ent"></param>
        /// <returns></returns>
        IList<T> GetListByEntity<T>(string name, T ent) where T : new();

        /// <summary>
        /// 数据库基本操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Save<T>(T ent, SaveType type) where T : new();
        bool Save<T>(IList<T> ent, SaveType type) where T : new();
        bool Save<T>(string name, T ent, SaveType type) where T : new();

        /// <summary>
        /// 获取单行详细信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetEntity<T>(T ent) where T : new();
        T GetEntity<T>(string name, T ent) where T : new();

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        bool HasExist<T>(T ent) where T : new();


        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="ent">Entity</param>
        /// <returns>Message code</returns>
        int? ExePrc<T>(ref T ent) where T : new();

        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="ent">Entity</param>
        /// <returns>Message code</returns>
        bool ExePrc<T>(ref IList<T> ent) where T : new();
        // ************ Add end ***********************************

        DataSet ExePrcEx<T>(T ent) where T : new();
    }
}
