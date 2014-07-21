using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Core.DemoModule.DemoAggregate
{
    public class Demo : DemoTB
    {
        /// <summary>
        /// 用户实体
        /// </summary>
        public Demo()
        {

        }

        /// <summary>
        /// 新增
        /// </summary>
        public override string _ActiveInsertCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._ActiveInsertCols))
                {
                    return @"DemoName,
                                DemoEmail,
                                CreationDate,
                                CreateBy,
                                LastUpdateDate,
                                LastUpdateBy";
                }
                else
                {
                    return base._ActiveInsertCols;
                }
            }
            set
            {
                base._ActiveInsertCols = value;
            }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public override string _DBTable
        {
            get
            {
                if (string.IsNullOrEmpty(base._DBTable))
                {
                    return "DemoTable";
                }
                else
                {
                    return base._DBTable;
                }
            }
            set
            {
                base._DBTable = value;
            }
        }
        /// <summary>
        /// 删除字段数
        /// </summary>
        public override string _DeleteWhereCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._DeleteWhereCols))
                {
                    return @"[DemoId]";
                }
                else
                {
                    return base._DeleteWhereCols;
                }
            }
            set
            {
                base._DeleteWhereCols = value;
            }
        }
        /// <summary>
        /// 更新字段
        /// </summary>
        public override string _ActiveUpdateCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._ActiveUpdateCols))
                {
                    return @"DemoName,
                                DemoEmail,
                                CreationDate,
                                CreateBy,
                                LastUpdateDate,
                                LastUpdateBy";
                }
                else
                {
                    return base._ActiveUpdateCols;
                }

            }
            set
            {
                base._ActiveInsertCols = value;
            }
        }
        /// <summary>
        /// 修改条件
        /// </summary>
        public override string _UpdateWhereCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._UpdateWhereCols))
                {
                    return @"[DemoId]";
                }
                else
                {
                    return base._UpdateWhereCols;
                }
            }
            set
            {
                base._UpdateWhereCols = value;
            }
        }


        /// <summary>
        /// 查询条件
        /// </summary>
        public override string _ActiveSelectCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._ActiveSelectCols))
                {
                    return @"DemoId,DemoName,
                                DemoEmail,
                                CreationDate,
                                CreateBy,
                                LastUpdateDate,
                                LastUpdateBy";
                }
                else
                {
                    return base._ActiveSelectCols;
                }
            }
            set
            {
                base._ActiveSelectCols = value;
            }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public override string _SelectWhereCols
        {
            get
            {
                return base._SelectWhereCols;
            }
            set
            {
                base._SelectWhereCols = value;
            }
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public override string _SelfWhere
        {
            get
            {
                return base._SelfWhere;
            }
            set
            {
                base._SelfWhere = value;
            }
        }
        /// <summary>
        /// 唯一键 
        /// </summary>
        public override string _UniqueName
        {
            get
            {
                if (string.IsNullOrEmpty(base._UniqueName))
                {
                    return "p.[DemoId]";
                }
                else
                {
                    return base._UniqueName;
                }
            }
            set
            {
                base._UniqueName = value;
            }
        }
        /// <summary>
        /// 流程业务名称ID
        /// </summary>
        public int? FlowRole
        {
            get;
            set;
        }
    }
}
