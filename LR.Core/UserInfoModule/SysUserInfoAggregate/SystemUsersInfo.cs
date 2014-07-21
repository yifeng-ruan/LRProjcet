/***********************************************************
 * SystemName:	LR
 * ModuleName:	公共模块 - 实体层
 * CreateDate:	2014/06/12 14:10:59
 * Author:	Ryan.Ruan
 * Description:	用户实体
 * Currnet Version:	V1.0
 ***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Core.UserInfoModule.SysUserInfoAggreagte
{
    public class SysUserInfoEntity : SysUserInfoEntityTB
    {
        /// <summary>
        /// 用户实体
        /// </summary>
        public SysUserInfoEntity()
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
                    return @"UserName,
                                UserPassword,
                                UserType,
                                UserStatus,
                                StaffNum,
                                Email,
                                Address,
                                Phone,
                                CreationDate,
                                CreateBy,
                                LastUpdateDate,
                                LastUpdateBy,
                                LastUpdateLoginCode";
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
                    return "PublicSystemUser";
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
                    return @"[UserID]";
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
                    return @" UserName,
                                UserPassword,
                                UserType,
                                UserStatus,
                                StaffNum,
                                Email,
                                Address,
                                Phone,
                                CreationDate,
                                CreateBy,
                                LastUpdateDate,
                                LastUpdateBy,
                                LastUpdateLoginCode";
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
                    return @"[UserID]";
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
                    return @"P.UserID,UserName,UserType,UserStatus,StaffNum,PositionName,Email,Phone,CustomerGroupName,BranchName";
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
                    return "p.[UserID]";
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
