using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Utils.DBHelper
{
    /// <summary>
    /// 检查SQL是否存在类
    /// </summary>
    public class SQLHasExist
    {
        /// <summary>
        /// 检查是否惟一SQL语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cols"></param>
        /// <param name="where1"></param>
        /// <param name="where2"></param>
        /// <returns></returns>
        public static string hasExistSqlText(string table, string cols, string where1, string where2)
        {
            string sql1 = @"
                        SELECT COUNT({0}) as RecordCount FROM {1} where {2}=@{3} ;
                        ";
            string sql2 = @"
                        SELECT COUNT({0}) as RecordCount FROM {1} where {2}=@{3} and {4}!=@{5} ;
                        ";
            if (string.IsNullOrEmpty(where2))
            {
                return string.Format(sql1, cols, table, where1, where1);
            }
            else
            {
                return string.Format(sql2, cols, table, where1, where1, where2, where2);
            }
        }

        /// <summary>
        /// 检查是否惟一SQL语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cols"></param>
        /// <param name="where1"></param>
        /// <param name="where2"></param>
        /// <param name="type">区别标识</param>
        /// <returns></returns>
        public static string hasExistSqlText(string table, string cols, string where1, string where2, string type)
        {
            string sql3 = @"
                        SELECT COUNT({0}) as RecordCount FROM {1} where {2}=@{3} or {4}=@{5} ;
                        ";

            string sql4 = @"
                        SELECT COUNT({0}) as RecordCount FROM {1} where {2}=@{3} and {4}=@{5} ;
                        ";
            if (type == "or")
            {
                return string.Format(sql3, cols, table, where1, where1, where2, where2);
            }
            else if (type == "and")
            {
                return string.Format(sql4, cols, table, where1, where1, where2, where2);
            }
            else
            {
                return "";//Error
            }
        }
    }
}
