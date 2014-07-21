using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Utils.DBHelper
{
    /// <summary>
    /// SQL分页管理类
    /// </summary>
    public class SQLPage
    {

        /// <summary>
        /// Sql 分页语句 页数从0开始 @RecordCount，@PageIndex，@PageSize
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cols"></param>
        /// <param name="orderRow"></param>
        /// <param name="where"></param>
        /// <param name="orderShow"></param>
        /// <returns></returns>
        public static string PageSqlText(string table, string cols, string orderRow, string where, string orderShow)
        {
            string sql = @"
                                    SELECT @RecordCount=COUNT(*) FROM {0} {3} ;
                                    --计算最大页数
                                    declare @totalPage int
                                    declare @nowtotal int
                                    set @totalPage=@RecordCount/@PageSize --页总数
                                    set @nowtotal=@totalPage * @PageSize
            
                                    if(@nowtotal<@RecordCount)
                                     --表示少一页
                                    set @totalPage=@totalPage+1
            
                                    --取最大页数
                                    if(@PageIndex>@totalPage)
                                    set @PageIndex=@totalPage
            
                                    SELECT {1} FROM  
                                    (   
                                     SELECT ROW_NUMBER() OVER (ORDER BY {2}) AS RowNumber,{1} FROM {0} {3}   
                                    ) AS A WHERE A.RowNumber 
                                    BETWEEN ((@PageIndex-1) * @PageSize+1 ) AND (@PageIndex * @PageSize)  
                                    ";
            // 修改人：Jimmy yao
            // 修改时间：2010.8.12
            //            string sql = @"
            //                        SELECT @RecordCount=COUNT(*) FROM {0} {3} ;
            //
            //                        SELECT {1} FROM  
            //                        (   
            //                         SELECT ROW_NUMBER() OVER (ORDER BY {2}) AS RowNumber,{1} FROM {0} {3}   
            //                        ) AS A WHERE A.RowNumber 
            //                        BETWEEN @pageindex and @pageSize+@pageindex-1
            //                        ";
            if (string.IsNullOrEmpty(orderShow))
            {
                return string.Format(sql, table, cols, orderRow, where);
            }
            else
            {
                return string.Format(sql, table, cols, orderShow, where);
            }
        }

        /// <summary>
        /// Sql 分页语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string PageSqlText(string table)
        {
            return PageSqlText(table, "*", "ID", string.Empty, string.Empty);
        }

        /// <summary>
        /// Sql 分页语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string PageSqlText(string table, string where)
        {
            return PageSqlText(table, "*", "ID", where, string.Empty);
        }

        /// <summary>
        ///  Sql 分页语句 :
        /// </summary>
        /// <param name="table"></param>
        /// <param name="orderRow">排序</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string PageSqlText(string table, string orderRow, string where)
        {
            return PageSqlText(table, "*", orderRow, where, string.Empty);
        }


    }
}
