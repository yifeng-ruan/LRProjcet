/***********************************************************
 * SystemName:	LR
 * ModuleName:	公共模块 - 实体层
 * CreateDate:	2014/06/12 14:10:59
 * Author:	Ryan.Ruan
 * Description:	用户实体 TB
 * Currnet Version:	V1.0
 ***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR.Core.Base;

namespace LR.Core.UserInfoModule.UserInfoAggregate
{
    /// <summary>
    /// 用户实体 TB
    /// </summary>
    public class UserInfoEntityTB : BaseEntity
    {
        int? _UserID;
        /// <summary>
        /// 用户ID
        /// </summary>	
        public virtual int? UserID
        {
            get { return _UserID; }
            set
            {
                _UserID = value;

            }
        }

        int? _PartnerID;
        /// <summary>
        /// 合作伙伴ID
        /// </summary>	
        public virtual int? PartnerID
        {
            get { return _PartnerID; }
            set
            {
                _PartnerID = value;

            }
        }

        string _FirstName;
        /// <summary>
        /// 首名字
        /// </summary>	
        public virtual string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;

            }
        }

        string _LastName;
        /// <summary>
        /// 尾名字
        /// </summary>	
        public virtual string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;

            }
        }

        string _ChineseName;
        /// <summary>
        /// 中文名
        /// </summary>	
        public virtual string ChineseName
        {
            get { return _ChineseName; }
            set
            {
                _ChineseName = value;

            }
        }

        string _EnglishName;
        /// <summary>
        /// 英文名
        /// </summary>	
        public virtual string EnglishName
        {
            get { return _EnglishName; }
            set
            {
                _EnglishName = value;

            }
        }

        string _UserLoginName;
        /// <summary>
        /// 登录名
        /// </summary>	
        public virtual string UserLoginName
        {
            get { return _UserLoginName; }
            set
            {
                _UserLoginName = value;

            }
        }

        string _UserPassword;
        /// <summary>
        /// 用户密码
        /// </summary>	
        public virtual string UserPassword
        {
            get { return _UserPassword; }
            set
            {
                _UserPassword = value;

            }
        }

        string _UserType;
        /// <summary>
        /// 用户类型
        /// </summary>	
        public virtual string UserType
        {
            get { return _UserType; }
            set
            {
                _UserType = value;

            }
        }

        int? _UserStatus;
        /// <summary>
        /// 用户状态
        /// </summary>	
        public virtual int? UserStatus
        {
            get { return _UserStatus; }
            set
            {
                _UserStatus = value;

            }
        }

        string _Sex;
        /// <summary>
        /// 性别
        /// </summary>	
        public virtual string Sex
        {
            get { return _Sex; }
            set
            {
                _Sex = value;

            }
        }

        int? _Age;
        /// <summary>
        /// 年龄
        /// </summary>	
        public virtual int? Age
        {
            get { return _Age; }
            set
            {
                _Age = value;

            }
        }

        DateTime? _BirthDay;
        /// <summary>
        /// 生日
        /// </summary>	
        public virtual DateTime? BirthDay
        {
            get { return _BirthDay; }
            set
            {
                _BirthDay = value;

            }
        }

        string _Email;
        /// <summary>
        /// Email
        /// </summary>	
        public virtual string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;

            }
        }

        string _Address;
        /// <summary>
        /// 地址
        /// </summary>	
        public virtual string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;

            }
        }

        string _ZIPCode;
        /// <summary>
        /// 邮编
        /// </summary>	
        public virtual string ZIPCode
        {
            get { return _ZIPCode; }
            set
            {
                _ZIPCode = value;

            }
        }

        string _Phone;
        /// <summary>
        /// 电话
        /// </summary>	
        public virtual string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;

            }
        }

        string _MobilePhone;
        /// <summary>
        /// 移动电话
        /// </summary>	
        public virtual string MobilePhone
        {
            get { return _MobilePhone; }
            set
            {
                _MobilePhone = value;

            }
        }

        /// <summary>
        /// 身份证
        /// </summary>
        public virtual string IdentityCard { get; set; }

        /// <summary>
        /// 是否会员
        /// </summary>
        public virtual int? IsAlliance { get; set; }

    }
}
