using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace LR.Utils.DBHelper
{
    /// <summary>
    /// 存储过程管理类
    /// </summary>
    public class SQLProcedures
    {
        /// <summary>
        /// sp_Pagination
        /// </summary>
        /// <param name="Parms">参数值集</param>
        /// <returns></returns>
        private static SqlParameter[] SetParameter(string[] Parms, string Procedure)
        {
            SqlParameter[] parmeters = new SqlParameter[8];
            if (Parms.Length > 0)
            {
                switch (Procedure)
                {
                    case "sp_Pagination":
                        parmeters[0] = new SqlParameter("@Tables", Parms[0]);
                        parmeters[1] = new SqlParameter("@PrimaryKey", Parms[1]);
                        parmeters[2] = new SqlParameter("@Sort", Parms[2]);
                        parmeters[3] = new SqlParameter("@CurrentPage", Parms[3]);
                        parmeters[4] = new SqlParameter("@PageSize", Parms[4]);
                        parmeters[5] = new SqlParameter("@Fields", Parms[5]);
                        parmeters[6] = new SqlParameter("@Filter", Parms[6]);
                        parmeters[7] = new SqlParameter("@Group", Parms[7]);
                        break;
                    case "sp_PaginationCount":
                        parmeters[0] = new SqlParameter("@Tables", Parms[0]);
                        parmeters[1] = new SqlParameter("@PrimaryKey", Parms[1]);
                        parmeters[2] = new SqlParameter("@Sort", Parms[2]);
                        parmeters[3] = new SqlParameter("@CurrentPage", Parms[3]);
                        parmeters[4] = new SqlParameter("@PageSize", Parms[4]);
                        parmeters[5] = new SqlParameter("@Fields", Parms[5]);
                        parmeters[6] = new SqlParameter("@Filter", Parms[6]);
                        parmeters[7] = new SqlParameter("@Group", Parms[7]);
                        break;
                }
            }
            else
            {

            }
            return parmeters;
        }
        /// <summary>
        /// 根据存储过程名获取相关的参数集
        /// </summary>
        /// <param name="Procedure">Procedure Name</param>
        /// <param name="Parms">参数值集</param>
        /// <returns></returns>
        public static SqlParameter[] GetParameter(string Procedure, string[] Parms)
        {
            SqlParameter[] parmeters = new SqlParameter[] { };
            switch (Procedure)
            {
                case "sp_Pagination":
                    parmeters = SetParameter(Parms, "sp_Pagination");
                    break;
            }
            return parmeters;
        }
    }
}
