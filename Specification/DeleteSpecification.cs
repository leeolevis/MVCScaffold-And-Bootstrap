namespace WebApp4.Specification
{
    using System;
    using System.Linq.Expressions;
    using WebApp4.Entities;

    public sealed class DeleteSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : Entity
    {
        #region Specification overrides

        public override System.Linq.Expressions.Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            Expression<Func<TEntity, bool>> deleteExpression = t => t.IsDeleted == false;
            return deleteExpression;
        }

        #endregion Specification overrides
    }
}