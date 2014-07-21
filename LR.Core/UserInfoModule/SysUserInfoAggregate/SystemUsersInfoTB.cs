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

namespace LR.Core.UserInfoModule.SysUserInfoAggreagte
{
    /// <summary>
    /// 用户实体 TB
    /// </summary>
    public class SysUserInfoEntityTB : BaseEntity
    {
        string state;
        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        int? userID;
        /// <summary>
        /// UserID
        /// </summary>	
        public int? UserID
        {
            get { return userID; }
            set
            {
                userID = value;

            }
        }

        string userName;
        /// <summary>
        /// 用户名
        /// </summary>	
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;

            }
        }

        string userPassword;
        /// <summary>
        /// 密码
        /// </summary>	
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;

            }
        }

        string userType;
        /// <summary>
        /// 类型
        /// </summary>	
        public string UserType
        {
            get { return userType; }
            set
            {
                userType = value;

            }
        }

        int? userStatus;
        /// <summary>
        /// 状态
        /// </summary>	
        public int? UserStatus
        {
            get { return userStatus; }
            set
            {
                userStatus = value;

            }
        }

        int? staffNum;
        /// <summary>
        /// 员工号
        /// </summary>	
        public int? StaffNum
        {
            get { return staffNum; }
            set
            {
                staffNum = value;

            }
        }

        string email;
        /// <summary>
        /// Email
        /// </summary>	
        public string Email
        {
            get { return email; }
            set
            {
                email = value;

            }
        }

        string address;
        /// <summary>
        /// 地址
        /// </summary>	
        public string Address
        {
            get { return address; }
            set
            {
                address = value;

            }
        }

        string phone;
        /// <summary>
        /// 电话
        /// </summary>	
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;

            }
        }
        int? positionID;
        /// <summary>
        /// 职位ID
        /// </summary>
        public int? PositionID
        {
            get { return positionID; }
            set { positionID = value; }
        }

        string positionName;
        /// <summary>
        /// 职位名
        /// </summary>
        public string PositionName
        {
            get { return positionName; }
            set { positionName = value; }
        }
        int? customerGroupID;
        /// <summary>
        /// 客户群
        /// </summary>
        public int? CustomerGroupID
        {
            get { return customerGroupID; }
            set { customerGroupID = value; }
        }
        string customerGroupName;
        /// <summary>
        /// 客户群名称
        /// </summary>
        public string CustomerGroupName
        {
            get { return customerGroupName; }
            set { customerGroupName = value; }
        }
        int? branchID;
        /// <summary>
        /// 办事处
        /// </summary>
        public int? BranchID
        {
            get { return branchID; }
            set { branchID = value; }
        }
        string branchName;
        /// <summary>
        /// 办事处名
        /// </summary>
        public string BranchName
        {
            get { return branchName; }
            set { branchName = value; }
        }

    }
}
