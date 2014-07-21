using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Reflection;

namespace LR.DTO.Base
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class BaseEntity
    {
        #region  对应的数据库表名，用于生成Xml
        string sTableName = string.Empty;
        public string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(sTableName))
                {
                    sTableName = this.GetType().Name;
                    sTableName = sTableName.Substring(0, sTableName.Length - 6);//去Entity，留表名
                }
                return sTableName;
            }
            set { sTableName = value; }
        }
        #endregion  对应的数据库表名，用于生成Xml

        #region  提交关键字，传到存储过程中作区分标志
        string sSubmitKey = string.Empty;
        /// <summary>
        /// 提交关键字
        /// </summary>
        public string SubmitKey { get; set; }
        #endregion  提交关键字

        #region Entity - XML 转换函数
        /// <summary>
        /// 把当前的Entity转为XML文本 
        /// 
        /// </summary>
        public virtual void GenerateXml()
        {
            GenerateXml(this.TableName);
        }
        /// <summary>
        /// 把当前的Entity转为XML文本 
        /// 
        /// </summary>
        public virtual void GenerateXml(string sTableName)
        {
            StringBuilder sbXml = new StringBuilder();
            sbXml.Append("<root>");
            sbXml.Append("<" + sTableName + ">");

            PropertyInfo[] Properties = this.GetType().GetProperties();
            object obj = null;
            foreach (PropertyInfo viewProperty in Properties)
            {
                //&& !viewProperty.Name.StartsWith("Proc")
                if (viewProperty.CanRead && !viewProperty.Name.StartsWith("_") && !viewProperty.Name.EndsWith("DateDisplay"))
                {
                    obj = viewProperty.GetValue(this, null);
                    if (obj != null)
                        sbXml.Append(string.Format("<{0}>{1}</{0}>", viewProperty.Name, obj));
                }
            }
            sbXml.Append("</" + sTableName + ">");
            sbXml.Append("</root>");


            this.xmlStr = sbXml.ToString();
        }

        #endregion Entity - XML 转换函数

        #region 日期字段的显示格式
        /// <summary>
        /// 日期字段的显示格式
        /// </summary>
        public static readonly string DtDisplayFormat = ConfigurationManager.AppSettings["DtDisplayFormat"] != null ? ConfigurationManager.AppSettings["DtDisplayFormat"] : "yyyy-MM-dd";
        #endregion

        #region 拼Sql模块专用

        /// <summary>
        /// 添加的字段
        /// </summary>
        public virtual string _ActiveInsertCols
        {
            get;
            set;
        }
        /// <summary>
        /// 修改的字段
        /// </summary>
        public virtual string _ActiveUpdateCols
        {
            get;
            set;
        }

        /// <summary>
        /// 选择的字段
        /// </summary>
        public virtual string _ActiveSelectCols
        {
            get;
            set;
        }
        /// <summary>
        /// 修改条件字段
        /// </summary>
        public virtual string _UpdateWhereCols
        {
            get;
            set;
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        public virtual string _DeleteWhereCols
        {
            get;
            set;
        }

        /// <summary>
        /// 选择的字段
        /// </summary>
        public virtual string _SelectWhereCols
        {
            get;
            set;
        }

        /// <summary>
        /// 数据物理表
        /// </summary>
        public virtual string _DBTable
        {
            get;
            set;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual string _SelectOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual string _SelectOrderType
        {
            get;
            set;
        }

        /// <summary>
        /// 唯一条件字段名称
        /// </summary>
        public virtual string _UniqueName
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义Sql语句
        /// </summary>
        public virtual string _Sql
        {
            get;
            set;
        }

        /// <summary>
        /// 是否执行自定义Sql语句
        /// </summary>
        public virtual bool IsSelfSql
        {
            set;
            get;
        }

        /// <summary>
        /// 自定义where条件语句
        /// </summary>
        public virtual string _SelfWhere
        {
            get;
            set;
        }
        #endregion

        #region 存储过程
        /// <summary>
        /// 是否存储过程
        /// </summary>
        public virtual bool isProc
        {
            set;
            get;
        }
        /// <summary>
        /// 存储过程名称
        /// </summary>
        public virtual string ProcName
        {
            get;
            set;
        }

        /// <summary>
        /// 存储过程参数名称名称，多个用，号分开
        /// </summary>
        public virtual string ProctParametersName
        {
            get;
            set;
        }

        /// <summary>
        /// 存储过程输出参数名称名称，多个用，号分开
        /// </summary>
        public virtual string ProctOutParametersName
        {
            get;
            set;
        }
        int? sProcMessageCode;
        /// <summary>
        /// 存储过程返回信息编码
        /// </summary>
        public int? ProcMessageCode
        {
            get { return sProcMessageCode; }
            set
            {
                sProcMessageCode = value;
                if (string.IsNullOrEmpty(MegText))
                {
                    MegText = sProcMessageCode.ToString();
                }
                if (string.IsNullOrEmpty(MegValue))
                {
                    MegValue = sProcMessageCode.ToString();
                }
            }
        }

        #endregion

        #region 分页面存储过程参数
        private int? pageIndex = null;
        /// <summary>
        /// 页面码
        /// </summary>
        public int? PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int? pageSize = null;
        /// <summary>
        /// 页大少
        /// </summary>
        public int? PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int? total = null;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int? Total
        {
            get { return total; }
            set
            {
                total = value;
                recordCount = value;
            }
        }

        private int? recordCount = null;
        /// <summary>
        /// 记录数
        /// </summary>
        public int? RecordCount
        {
            get { return recordCount; }
            set
            {
                recordCount = value;
                total = value;
            }
        }

        private string primaryKey;
        /// <summary>
        /// 主键字段
        /// </summary>
        public string PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }

        private string filter;
        /// <summary>
        /// 数据筛选条件
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        private string group;
        /// <summary>
        /// 分组筛选条件
        /// </summary>
        public string Group
        {
            get { return group; }
            set { group = value; }
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 多个唯一字段（例：1,2,3,6）jimmy 2010-11-18 添加 
        /// delete by brian.chen 20110506
        /// </summary>
        public virtual string _ItemsID
        {
            get;
            set;
        }

        /// <summary>
        /// 批量 传递值(例:1,2,3,4)
        /// </summary>
        public virtual string ItemsID
        {
            get;
            set;
        }

        /// <summary>
        /// 存储过程中Xml参数
        /// </summary>
        public virtual string xmlStr
        {
            get;
            set;
        }

        /// <summary>
        /// 删除模式：0根据商机销售团队表主键单个删除，1根据主键ID传（逗号相隔）批量删除。
        /// </summary>
        public virtual int? SPCommandType
        {
            get;
            set;
        }
        #endregion

        #region 自定义
        /// <summary>
        /// 用户类别
        /// </summary>
        public string UserType
        {
            get;
            set;
        }

        /// <summary>
        /// 客户群
        /// </summary>
        public string CustomerGroup
        {
            get;
            set;
        }
        public int Timeout
        {
            get;
            set;
        }
        #endregion

        #region
        /// <summary>
        /// 命令超时时间
        /// </summary>
        private int? _TimeOut = 30;

        public int? TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }

        #endregion

        #region 表通用字段
        #region 生效日期
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime? ActiveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 生效日期
        /// </summary>
        public string ActiveDateDisplay
        {
            get
            {
                return ActiveDate != null && this.ActiveDate.HasValue ? this.ActiveDate.Value.ToString(DtDisplayFormat) : string.Empty;
            }
        }
        #endregion

        #region 失效日期
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? InactiveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 失效日期
        /// </summary>
        public string InactiveDateDisplay
        {
            get
            {
                return InactiveDate != null && this.InactiveDate.HasValue ? this.InactiveDate.Value.ToString(DtDisplayFormat) : string.Empty;
            }
        }
        #endregion

        #region 创建人ID
        /// <summary>
        /// 新规范-创建人ID
        /// </summary>
        public string CreatedBy
        {
            get;
            set;
        }
        /// <summary>
        /// 新规范-创建人名称
        /// </summary>
        public string CreatedByDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// 旧规范-创建人ID-[公共,客户,产品,价格专用]
        /// </summary>
        public string CreateBy
        {
            get;
            set;
        }
        /// <summary>
        /// 旧规范-创建人名称-[公共,客户,产品,价格专用]
        /// </summary>
        public string CreateByDisplay
        {
            get;
            set;
        }
        #endregion

        #region 创建日期
        /// <summary>
        /// 新规范-创建日期
        /// </summary>
        public DateTime? CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期显示名称
        /// </summary>
        public string CreateDateDisplay
        {
            get
            {
                return CreateDate != null && this.CreateDate.HasValue ? this.CreateDate.Value.ToString(DtDisplayFormat) : string.Empty;
            }
        }

        /// <summary>
        /// 旧规范-创建日期-[公共,客户,产品,价格专用]
        /// </summary>
        public DateTime? CreationDate
        {
            get;
            set;
        }

        /// <summary>
        /// 旧规范-创建日期显示名称-[公共,客户,产品,价格专用]
        /// </summary>
        public string CreationDateDisplay
        {
            get
            {
                return CreationDate != null && CreationDate.HasValue ? CreationDate.Value.ToString(DtDisplayFormat) : string.Empty;
            }
        }
        #endregion

        #region 最后更新人ID
        /// <summary>
        /// 新规范-最后更新人ID
        /// </summary>
        public string LastUpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 新规范-最后更新人名称
        /// </summary>
        public string LastUpdatedByDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// 旧规范-最后更新人ID-[公共,客户,产品,价格专用]
        /// </summary>
        public string LastUpdateBy
        {
            get;
            set;
        }

        /// <summary>
        /// 旧规范-最后更新人名称-[公共,客户,产品,价格专用]
        /// </summary>
        public string LastUpdateByDisplay
        {
            get;
            set;
        }
        #endregion

        #region 最后更新日期
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime? LastUpdateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 最后更新日期显示名称
        /// </summary>
        public string LastUpdateDateDisplay
        {
            get
            {
                return LastUpdateDate != null && LastUpdateDate.HasValue ? LastUpdateDate.Value.ToString(DtDisplayFormat) : string.Empty;
            }
        }
        #endregion

        #region 最后更新LoginCode
        /// <summary>
        /// 新规范-最后更新LoginCode
        /// </summary>
        public string LastUpdateLogin
        {
            get;
            set;
        }
        /// <summary>
        /// 旧规范-最后更新LoginCode-[公共,客户,产品,价格专用]
        /// </summary>
        public string LastUpdateLoginCode
        {
            get;
            set;
        }
        #endregion

        public string MegText { get; set; }
        public string MegValue { get; set; }

        #endregion

    }
}
