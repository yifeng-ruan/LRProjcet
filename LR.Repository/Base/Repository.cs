
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR.BLL.Implementation;
using LR.Utils.Logging;
using LR.BLL.Interface;
using LR.Utils;
using LR.Repository.Resource;

namespace LR.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        #region Members

        IBLL _UnitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(IBLL unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(T item)
        {
            if (item != (T)null)
                _UnitOfWork.Insert<T>(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotAddNullEntity, typeof(T).ToString());

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Remove(T item)
        {
            if (item != (T)null)
            {
                _UnitOfWork.Delete<T>(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotRemoveNullEntity, typeof(T).ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Modify(T item)
        {
            if (item != (T)null)
                _UnitOfWork.Update<T>(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotRemoveNullEntity, typeof(T).ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get(T item)
        {
            if (item != (T)null)
                return _UnitOfWork.GetEntity<T>(item);
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll(T item,PageInfo pageInfo,ref int total)
        {
           return _UnitOfWork.GetList<T>(item, pageInfo, ref total);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetALlByProc(T item, PageInfo pageInfo, ref int total)
        {
            return _UnitOfWork.ExecProc<T>(item, pageInfo, ref total);
        }

        #endregion


        public IBLL UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }
    }
}
