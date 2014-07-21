using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Utils.DBHelper
{
    /// <summary>
    /// 提交数据库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SubmitCommand<T>
    {
        /// <summary>
        /// Sql语句
        /// </summary>
        public string Sql
        {
            get;
            set;
        }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string ProcName
        {
            get;
            set;
        }
        /// <summary>
        /// 命令超时时间
        /// </summary>
        private int? _TimeOut = 30;

        public int? TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        /// <summary>
        /// 参与的字段，必须跟实体对应，区分大小写
        /// </summary>
        public string ActionCols
        {
            get;
            set;
        }

        public T Enty
        {
            get;
            set;
        }

        public IList<Pter> PList
        {
            set;
            get;
        }

        /// <summary>
        /// 是否存储过程
        /// </summary>
        public bool isProc
        {
            set;
            get;
        }

        /// <summary>
        /// 存储过程参数名称名称，多个用，号分开
        /// </summary>
        public string ProctParametersName
        {
            get;
            set;
        }

        /// <summary>
        /// 存储过程输出参数名称名称，多个用，号分开
        /// </summary>
        public string ProctOutParametersName
        {
            get;
            set;
        }

        private int? _ProctPageIndex = null;

        public int? ProctPageIndex
        {
            get { return _ProctPageIndex; }
            set { _ProctPageIndex = value; }
        }


        private int? _ProctPageSize = null;

        public int? ProctPageSize
        {
            get { return _ProctPageSize; }
            set { _ProctPageSize = value; }
        }


        private int? _ProctPageTotal = null;

        public int? ProctPageTotal
        {
            get { return _ProctPageTotal; }
            set { _ProctPageTotal = value; }
        }
    }
}
