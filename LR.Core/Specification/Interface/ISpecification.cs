using System;
using System.Linq.Expressions;

namespace LR.Core.Specification.Interface
{
    /// <summary>
    /// 利用规格模式、linq、lambda表达方式
    /// 学习地址如下
    /// Ref : http://martinfowler.com/apsupp/spec.pdf
    /// Ref : http://en.wikipedia.org/wiki/Specification_pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> where T : class
    {
        /// <summary>
        /// 检查是否实体是否满足规格的拉姆达表达式
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> SatisfiedBy();
    }
}
