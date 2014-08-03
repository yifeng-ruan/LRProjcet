using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Core.UserModule.UserAggregate
{
    public partial class UserLoginHistoryTB : Base.BaseEntity
    {
        #region 构造器

        public UserLoginHistoryTB()
        {
        }

        #endregion 构造器

        #region 属性
        public int? ID { get; set; }
        public string UserName { get; set; }

        public string HostName { get; set; }
        public string HostIP { get; set; }

        public string LoginCity { get; set; }
        public DateTime LoginDate { get; set; }

        public string RecentHostName { get; set; }
        public string RecentHostIP { get; set; }

        public string RecentLoginCity { get; set; }
        public DateTime RecentLoginDate { get; set; }



        #endregion 属性
    }
}
