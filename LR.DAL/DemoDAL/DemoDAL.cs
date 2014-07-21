/***********************************************************
* SystemName:	LR.BaseDataAccess
* ModuleName:	公共模块 - 公共数据访问层
* CreateDate:	2014/6/9 
* Author:	    Ryan.Ruan
* Description:  创建商务申请数据库
* Currnet Version:	V1.0
***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR.DAL;
using Microsoft.Practices.EnterpriseLibrary.Data;

/*
 * 不采用旧的拼SQL方式，新模块不需创建一个DAL项目 
*/
namespace LR.DAL.DemoDAL
{
    public class DemoDAL:LRDB
    {
        //重写此方法，读取商务申请数据库   
        public DemoDAL()
        {
            this.DBName = "DemoDB";
            this.db = DatabaseFactory.CreateDatabase(DBName);
        }
    }
}
