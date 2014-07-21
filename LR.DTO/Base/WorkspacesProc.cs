using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.DTO.Base
{
    public class WorkspacesProc
    {
        #region 通用参数
        public const string GeneralSelectParams = "xmlStr,SubmitKey,PageIndex,PageSize";
        public const string GeneralSelectOutParams = "[RecordCount]";

        public const string GeneralSaveParams = "xmlStr,SubmitKey";
        public const string GeneralSaveOutParams = "[ProcMessageCode]";
        #endregion 通用参数
    }
}
