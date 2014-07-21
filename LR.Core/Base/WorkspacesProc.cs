using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Core.Base
{
    public class WorkspacesProc
    {
        #region 通用参数
        public const string GeneralSelectParams = "xmlStr,SubmitKey,PageIndex,PageSize";
        public const string GeneralSelectOutParams = "[RecordCount]";

        public const string GeneralSaveParams = "xmlStr,SubmitKey";
        public const string GeneralSaveOutParams = "[ProcMessageCode]";
        #endregion 通用参数

        #region Demo
        public const string DemoComplexSelect = "DemoComplexSelect_Sql";
        public const string DemoComplexSelectParams = "xmlStr,PageIndex,PageSize";
        public const string DemoComplexSelectOutParams = "[RecordCount]";
        #endregion
    }
}
