using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LR.Core.DemoModule.DemoAggregate;
using LR.Utils;
using LR.DAL;
using LR.DAL.DemoDAL;
using LR.Core.Base;
using LR.BLL.Interface;
namespace LR.BLL.Implementation
{
    public class DemoBLL : BaseBLL,IBLL
    {

        public bool Insert<T>(T demo)
        {
            return Save<Demo, DemoDAL>(demo as Demo, SaveType.Insert);
        }

        public bool Delete<T>(T demo)
        {
            return Save<Demo, DemoDAL>(demo as Demo, SaveType.Delete);
        }

        public bool Update<T>(T _demo)
        {
            Demo demo = _demo as Demo;
            if (demo.DemoId.HasValue)
            {
                //局部更新
                demo._DBTable = "DemoTable";//更改更新的表名
                demo._ActiveUpdateCols = "DemoName";//只更改表的Demo名称字段
                demo._UpdateWhereCols = "DemoId";//通过条件DemoId
            }
            return Save<Demo, DemoDAL>(demo, SaveType.Update);
        }

        public bool HasExist<T>(T _demo)
        {
            Demo demo = _demo as Demo;
            demo._ActiveSelectCols = @"[DemoId]";
            return HasExist<Demo, DemoDAL>(demo);
        }

        public T GetEntity<T>(T _demo)
        {
            Demo demo = _demo as Demo;
            return (T)Convert.ChangeType(GetEntity<Demo, DemoDAL>(demo), typeof(T));
        }

        public IList<T> GetList<T>(T _entity, PageInfo pageInfo, ref int total)
        {
            Demo entity = _entity as Demo;
            return (IList<T>)Convert.ChangeType(base.GetPageList<Demo, DemoDAL>(entity, pageInfo, ref total), typeof(IList<T>));
        }

        public IList<T> ExecProc<T>(T _entity, PageInfo pageInfo, ref int total)
        {
            Demo entity = _entity as Demo;
            entity.isProc = true;
            entity.ProcName = WorkspacesProc.DemoComplexSelect;
            entity.ProctParametersName = WorkspacesProc.DemoComplexSelectParams;
            entity.ProctOutParametersName = WorkspacesProc.DemoComplexSelectOutParams;
            return (IList<T>)Convert.ChangeType(base.GetPageList<Demo, DemoDAL>(entity, pageInfo, ref total), typeof(IList<T>));
        }

    }
}
