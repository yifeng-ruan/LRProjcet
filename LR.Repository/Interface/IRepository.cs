using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LR.Core.Specification.Interface;
using LR.BLL.Interface;
using LR.Utils;

namespace LR.Repository
{
    public interface IRepository<T> where T:class
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IBLL UnitOfWork { get; }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(T item);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(T item);

        /// <summary>
        /// Set item as modified
        /// </summary>
        /// <param name="item">Item to modify</param>
        void Modify(T item);

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">Entity key value</param>
        /// <returns></returns>
        T Get(T item);

        /// <summary>
        /// Get all elements of type T in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IEnumerable<T> GetAll(T item, PageInfo pageInfo, ref int total);

        IEnumerable<T> GetALlByProc(T item, PageInfo pageInfo, ref int total);
    }
}
