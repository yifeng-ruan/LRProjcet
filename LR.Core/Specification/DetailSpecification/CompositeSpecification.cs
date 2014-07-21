using LR.Core.Specification.Interface;
using LR.Core.Specification.Implementation;

namespace LR.Core.Specification.DetailSpecification
{
    /// <summary>
    /// 组合规则
    /// </summary>
    /// <typeparam name="T">规则的泛型类</typeparam>
    public abstract class CompositeSpecification<T> : Specification<T> where T : class
    {
        #region Properties

        /// <summary>
        /// 左边的规则
        /// </summary>
        public abstract ISpecification<T> LeftSideSpecification { get; }

        /// <summary>
        /// 右边的规则
        /// </summary>
        public abstract ISpecification<T> RightSideSpecification { get; }

        #endregion

    }
}
