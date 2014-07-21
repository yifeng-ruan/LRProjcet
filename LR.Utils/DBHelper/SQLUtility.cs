using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LR.Utils.DBHelper
{
    /// <summary>
    /// SQL帮助类
    /// </summary>
    public class SQLUtility
    {
        #region 转换
        public static int ToInt(string obj, int defVal)
        {
            try
            {
                return int.Parse(obj);
            }
            catch
            {
                return defVal;
            }

        }

        public static bool ToBool(string obj, bool defVal)
        {
            try
            {
                return bool.Parse(obj);
            }
            catch
            {
                return defVal;
            }
        }
        public static double ToDouble(string obj, double defVal)
        {
            try
            {
                return double.Parse(obj);
            }
            catch
            {
                return defVal;
            }
        }

        public static DateTime ToDateTime(string obj, DateTime defVal)
        {
            try
            {
                return DateTime.Parse(obj);
            }
            catch
            {
                return defVal;
            }
        }
        #endregion

        #region sqltext

        /// <summary>
        /// 添加语句
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static String InsertText(string tb, string cols)
        {
            string[] list = cols.Split(new char[] { ',' });
            string ps = string.Empty;
            for (int i = 0; i < list.Length; i++)
            {
                ps += "@" + list[i].Trim().Trim(new char[] { '[', ']' }) + ",";
            }

            string sql = @"INSERT INTO {0}
                               ({1})
                         VALUES({2});";
            sql = string.Format(sql, tb, cols, ps.TrimEnd(','));

            return sql;
        }

        /// <summary>
        /// 更新SQL
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static String UpdateText(string tb, string cols)
        {
            string sql = @"UPDATE {0}
                           SET {1} ";
            sql = string.Format(sql, tb, EQStr(cols));

            return sql;
        }
        /// <summary>
        /// 返回值得的Column
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static string EQStr(string cols)
        {
            string[] list = cols.Split(',');
            string ps = string.Empty;
            for (int i = 0; i < list.Length; i++)
            {
                ps += list[i] + "=@" + list[i].Trim().TrimStart('[').TrimEnd(']') + ",";
            }

            return ps.TrimEnd(',');
        }

        /// <summary>
        /// 删除SQL
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string DeleteText(string tb)
        {
            return "DELETE FROM " + tb;
        }
        /// <summary>
        /// 返回SQL指定条件
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static string WhereText(string cols)
        {
            return " Where " + EQStr(cols.Trim(new char[] { ',' })).Replace(",", " and ");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static string SelectText(string tb, string cols)
        {
            return string.Format("Select {0} from {1} ", cols, tb);
        }

        #endregion
        /// <summary>
        /// 返回数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static System.Data.DbType GetDbType(Type type)
        {
            DbType result = DbType.String;
            if (type.Equals(typeof(int)) || type.IsEnum)
                result = DbType.Int32;
            else if (type.Equals(typeof(long)))
                result = DbType.Double;
            else if (type.Equals(typeof(double)) || type.Equals(typeof(Double)))
                result = DbType.Decimal;
            else if (type.Equals(typeof(DateTime)))
                result = DbType.DateTime;
            else if (type.Equals(typeof(bool)))
                result = DbType.Boolean;
            else if (type.Equals(typeof(string)))
                result = DbType.String;
            else if (type.Equals(typeof(decimal)))
                result = DbType.Decimal;
            else if (type.Equals(typeof(byte[])))
                result = DbType.Binary;
            else if (type.Equals(typeof(Guid)))
                result = DbType.Guid;

            return result;

        }
    }
}
