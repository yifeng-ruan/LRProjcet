using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Utils
{
    /// <summary>
    /// 分页管理类
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前索引号
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 页面长度
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
    }
}
