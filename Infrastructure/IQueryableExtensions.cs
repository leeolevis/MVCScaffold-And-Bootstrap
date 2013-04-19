using System;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp4.Infrastructure
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// OfType extension method for IQueryable
        /// </summary>
        /// <typeparam name="KEntity">The type to filter the elements of the sequence on. </typeparam>
        /// <param name="queryable">The queryable object</param>
        /// <returns>
        /// A new IQueryable hat contains elements from
        /// the input sequence of type TResult
        /// </returns>
        public static IQueryable<KEntity> OfType<TEntity, KEntity>(this IQueryable<TEntity> queryable)
            where TEntity : class
            where KEntity : class,TEntity
        {
            var objectQuery = queryable as DbQuery<TEntity>;

            if (objectQuery != null) //is DBSET
            {
                return objectQuery.OfType<KEntity>();
            }
            else // probably IDbSet Mock
                return queryable.OfType<KEntity>();
        }

        /// <summary>
        /// OrderBy extension method for IQueryable
        /// http://weblogs.asp.net/davidfowler/archive/2008/12/11/dynamic-sorting-with-linq.aspx
        /// </summary>
        /// <typeparam name="TEntity">Type of elements in IQueryable</typeparam>
        /// <param name="queryable">Queryable object</param>
        /// <param name="orderByPropertyName">Order by string for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>Queryable object with OrderBy information</returns>
        private static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> queryable, string orderByPropertyName, bool ascending) where TEntity : class
        {
            ObjectQuery<TEntity> query = queryable as ObjectQuery<TEntity>;

            if (String.IsNullOrEmpty(orderByPropertyName))
            {
                // We do not need to go further!
                return queryable;
            }

            // Build the query
            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            ParameterExpression parameter = Expression.Parameter(queryable.ElementType, String.Empty);
            //      MemberExpression property = Expression.Property(parameter, orderByPropertyName);

            Expression expr = parameter;
            foreach (string prop in orderByPropertyName.Split('.'))
            {
                // use reflection (not ComponentModel) to mirror LINQ
                expr = Expression.PropertyOrField(expr, prop);
            }

            //    LambdaExpression lambda = Expression.Lambda(property, parameter);
            LambdaExpression lambda = Expression.Lambda(expr, parameter);

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { queryable.ElementType, expr.Type },
                                                queryable.Expression, Expression.Quote(lambda));

            return queryable.Provider.CreateQuery<TEntity>(methodCallExpression);
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> queryable, string orderByPropertyName) where TEntity : class
        {
            return OrderBy<TEntity>(queryable, orderByPropertyName, true);
        }

        public static IQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> queryable, string orderByPropertyName) where TEntity : class
        {
            return OrderBy<TEntity>(queryable, orderByPropertyName, false);
        }
    }
}