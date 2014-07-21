using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Core.DemoModule.DemoAggregate;
using LR.Core.Base;
using LR.BLL.Implementation;
using LR.BLL.Interface;

namespace LR.Repository.DemoModule
{
    public class DemoRepository : Repository<Demo>, IDemoRepository
    {
        #region 构造函数

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public DemoRepository(DemoBLL unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion
    }
}
