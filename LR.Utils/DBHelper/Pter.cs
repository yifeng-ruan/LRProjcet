using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Utils.DBHelper
{
    /// <summary>
    /// Sql参数对象
    /// </summary>
    public class Pter
    {
        public string Name
        {
            set;
            get;
        }
        public object Value
        {
            set;
            get;
        }

        public string LinkTag
        {
            set;
            get;
        }
        public bool IsSame
        {
            set;
            get;
        }

        public string Where
        {
            set;
            get;
        }
        public string WherePs
        {
            set;
            get;
        }
    }
}
