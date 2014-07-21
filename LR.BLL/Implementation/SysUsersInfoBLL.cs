/***********************************************************
* SystemName:	LR.WorkspacesBLL
* ModuleName:	公共模块 - 公共业务逻辑层
* CreateDate:	2014/06/12 
* Author:	    Ryan.Ruan
* Description:  操作数据业务逻辑方法
* Currnet Version:	V1.0
***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR.Core.UserInfoModule.SysUserInfoAggreagte;
using LR.Utils;
using LR.DAL;
using LR.DAL.DemoDAL;

namespace LR.BLL.Implementation
{
    /// <summary>
    /// 操作数据方法类
    /// </summary>
    public class SysUsersInfoBLL : BaseBLL
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        public bool Insert(SysUserInfoEntity UsersInfo)
        {
            return Save<SysUserInfoEntity, DemoDAL>(UsersInfo, SaveType.Insert);

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        public bool Delete(SysUserInfoEntity UsersInfo)
        {
            return Save<SysUserInfoEntity, DemoDAL>(UsersInfo, SaveType.Delete);

        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        public bool Update(SysUserInfoEntity UsersInfo)
        {
            if (UsersInfo.UserID.HasValue)
            {
                //局部更新
                UsersInfo._DBTable = "PublicSystemUser";//在SysUserInfoEntity已经有默认设置 
                UsersInfo._ActiveUpdateCols = "UserName";
                UsersInfo._UpdateWhereCols = "UserID";
            }
            return Save<SysUserInfoEntity, DemoDAL>(UsersInfo, SaveType.Update);
        }
        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="UsersInfo">对象数据实体</param>
        /// <returns>返回结果</returns>
        public bool HasExist(SysUserInfoEntity UsersInfo)
        {
            UsersInfo._ActiveSelectCols = @"[UserID]";
            return HasExist<SysUserInfoEntity, DemoDAL>(UsersInfo);
        }

        #region 获取信息列表
 

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="ent">实体</param>
        /// <param name="page">页</param>
        /// <param name="total">总数</param>
        /// <returns>返回结果</returns>
        public IList<SysUserInfoEntity> GetUsersInfoList(SysUserInfoEntity ent, PageInfo page, ref int total)
        {
            ent.isProc = true;
            ent.ProcName = "Pagination_SP";
            ent.ProctParametersName = "_DBTable,PrimaryKey,_SelectOrder,PageIndex,PageSize,_ActiveSelectCols,Filter,Group";
            ent.ProctOutParametersName = "[Total]";
            ent.PrimaryKey = "p.UserID";
            ent._DBTable = @"PublicSystemUser p 
                                left join PublicUserInfo b 
                                    on p.UserID=b.UserID 
                                left join PublicBranch c 
                                    on p.branchid=c.BranchID 
                                left join PublicCustomerGroup d 
                                    on p.CustomerGroupID=d.CustomerGroupID 
                                left join PublicPosition e 
                                    on p.PositionID=e.PositionID";
            StringBuilder filter = new StringBuilder();
            filter.Append("1=1 ");
            if (!string.IsNullOrEmpty(ent.UserType))
            {
                filter.Append(" and UserType='" + ent.UserType + "'");
            }
            if (ent.StaffNum.HasValue)
            {
                filter.Append(" and StaffNum='" + ent.StaffNum.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(ent.UserName))
            {
                filter.Append(" and UserName='" + ent.UserName + "'");
            }
            return GetPageList<SysUserInfoEntity, DemoDAL>(ent, page, ref total);
        }
        #endregion

        #region 获取单个实体
        /// <summary>
        /// 获取单行详细信息
        /// </summary>
        /// <param name="ent">实体</param>
        /// <returns>返回结果</returns>
        public SysUserInfoEntity GetUsersInfoEntity(SysUserInfoEntity UsersInfo)
        {
            return GetEntity<SysUserInfoEntity, DemoDAL>(UsersInfo);
        }
        #endregion

        public IList<SysUserInfoEntity> GetEntityList(SysUserInfoEntity entity, PageInfo pageInfo, ref int total)
        {
            return base.GetPageList<SysUserInfoEntity, DemoDAL>(entity, pageInfo, ref total);
        }

        public IList<SysUserInfoEntity> ExecProc(SysUserInfoEntity entity, PageInfo pageInfo, ref int total)
        {
            entity.isProc = true;
            //entity.ProcName = WorkspacesProc.PublicGetUserByFlowRole;
            //entity.ProctParametersName = WorkspacesProc.PublicGetUserByFlowRoleParams;
            //entity.ProctOutParametersName = WorkspacesProc.PublicGetUserByFlowRoleOutParams;
            return base.GetPageList<SysUserInfoEntity, DemoDAL>(entity, pageInfo, ref total);

        }

    }
}
