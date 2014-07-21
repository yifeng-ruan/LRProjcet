/***********************************************************
 * SystemName:	LR
 * ModuleName:	公共模块 - 实体层
 * CreateDate:	2011/7/26 14:10:59
 * Author:	Ben.Wu
 * Description:	用户属性
 * Currnet Version:	V1.0
 ***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LR.Core.UserInfoModule.UserInfoAggregate
{
    /// <summary>
    /// 用户属性
    /// </summary>

    public class UserInfoEntity : UserInfoEntityTB
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserInfoEntity()
        {
        }

        /// <summary>
        /// 插入时的列
        /// </summary>
        public override string _ActiveInsertCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._ActiveInsertCols))
                {
                    return @"[UserID]
                            ,[UserPassword]
                            ,[Age]
                            ,[BirthDay]
                            ,[HireDate]
                            ,[ZIPCode]
                            ,[Phone]
                            ,[ActiveDate]
                            ,[InactiveDate]
                            ,[CreateBy]
                            ,[CreationDate]
                            ,[LastUpdateDate]
                            ,[LastUpdateBy]
                            ,[LastUpdateLoginCode]
                            ,[LRType]
                            ,[Job]
                            ,[IdentityCard]";
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
        /// 更新时的列
        /// </summary>
        public override string _ActiveUpdateCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._ActiveUpdateCols))
                {
                    return @"[UserID]
                              ,[StaffNum]
                              ,[FirstName]
                              ,[LastName]
                              ,[ChineseName]
                              ,[EnglishName]
                              ,[UserLoginName]
                              ,[UserPassword]
                              ,[UserType]
                              ,[UserStatus]
                              ,[Sex]
                              ,[Age]
                              ,[BirthDay]
                              ,[HireDate]
                              ,[Email1]
                              ,[Email2]
                              ,[Address]
                              ,[ZIPCode]
                              ,[Phone]
                              ,[MobilePhone]
                              ,[LastUpdateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateLoginCode]
                              ,[LRType]
                              ,[Job]
                              ,[IdentityCard]
                              ,[EMRDate]";
                }
                else
                {
                    return base._ActiveUpdateCols;
                }

            }
            set
            {
                base._ActiveUpdateCols = value;
            }
        }

        /// <summary>
        /// 查询时的列
        /// </summary>
        public override string _ActiveSelectCols
        {
            get
            {
                if (string.IsNullOrEmpty(base._ActiveSelectCols))
                {
                    return @"[UserID]
                              ,[UserType]
                              ,[LRType]
                              ,[PartnerID]
                              ,[IdentityCard]
                              ,[Age]
                              ,[BirthDay]
                              ,[HireDate]
                              ,[Job]
                              ,[EMRDate]
                              ,[ZIPCode]
                              ,[Phone]
                              ,[ChineseName]
                              ,[EnglishName]
                              ,[UserLoginName]
                              ,[UserPassword]
                              ,[UserStatus]
                              ,[StaffNum]
                              ,[FirstName]
                              ,[LastName]
                              ,[Sex]
                              ,[Email1]
                              ,[Email2]
                              ,[MobilePhone]
                              ,[Address]
                              ,[ActiveDate]
                              ,[InactiveDate]
                              ,[CreateBy]
                              ,[CreationDate]
                              ,[LastUpdateDate]
                              ,[LastUpdateBy]
                              ,[LastUpdateLoginCode]";
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
        /// 数据库表名
        /// </summary>
        public override string _DBTable
        {
            get
            {
                if (string.IsNullOrEmpty(base._DBTable))
                {
                    return "PublicUserInfo_v";
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
        /// 删除时的以什么列作为条件
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
        /// 更新时的以什么列作为条件
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



    }
}
