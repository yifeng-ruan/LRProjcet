using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Utils;

namespace LR.BLL.Interface
{
    public interface IBLL
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>`
        bool Insert<T>(T demo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        bool Delete<T>(T demo);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        bool Update<T>(T demo);
        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        bool HasExist<T>(T demo);

        #region 获取单个实体
        /// <summary>
        /// 获取单行详细信息
        /// </summary>
        /// <param name="ent">实体</param>
        /// <returns>返回结果</returns>
        T GetEntity<T>(T demo);
        #endregion

        IList<T> GetList<T>(T entity, PageInfo pageInfo, ref int total);

        IList<T> ExecProc<T>(T entity, PageInfo pageInfo, ref int total);
    }
}
