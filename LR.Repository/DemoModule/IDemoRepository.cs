using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Core.DemoModule.DemoAggregate;

namespace LR.Repository.DemoModule
{
    public interface IDemoRepository : IRepository<Demo>
    {
    }
}
