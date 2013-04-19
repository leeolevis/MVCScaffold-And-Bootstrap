using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using WebApp4.Entities;
using WebApp4.Specification;

namespace WebApp4.Condition
{
    public static class SpecificationBuilder
    {
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Type GetNonNullableType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        private static Expression<Func<T, bool>> BuildEqualExpression<T>(string propertyName, object propertyValue)
        {
            var parameterExp = Expression.Parameter(typeof(T), typeof(T).ToString());
            var constant = Expression.Constant(propertyValue);

            Expression propertyExp = parameterExp;
            foreach (string prop in propertyName.Split('.'))
            {
                propertyExp = Expression.PropertyOrField(propertyExp, prop);
            }

            Type type = propertyExp.Type;
            Expression nonNullProperty = propertyExp; //转换nullable的字段类型

            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(propertyExp, type);
            }

            var methodExp = Expression.Equal(nonNullProperty, constant);
            return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
        }

        public static Expression<Func<TInput, object>> AddBox<TInput, TOutput>
        (Expression<Func<TInput, TOutput>> expression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert
                 (expression.Body, typeof(object));
            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<TInput, object>>
                 (converted, expression.Parameters);
        }

        public static Expression<Func<T, object>> GetExpression<T>(string property)
        {
            ParameterExpression arg = Expression.Parameter(typeof(T), "x");
            Expression expr = arg;
            foreach (string prop in property.Split('.'))
            {
                // use reflection (not ComponentModel) to mirror LINQ
                expr = Expression.PropertyOrField(expr, prop);
            }

            Type type = expr.Type;
            Expression nonNullProperty = expr;
            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(nonNullProperty, type);
            }
            //    Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), expr.Type);

            if (type == typeof(Guid))
            {
                var temp = Expression.Lambda<Func<T, Guid>>(nonNullProperty, arg);

                //Expression converted = Expression.Convert(temp.Body, typeof(object));

                //return Expression.Lambda<Func<T, object>>(converted, temp.Parameters);

                return AddBox(temp);
            }

            if (type == typeof(Int32))
            {
                var temp = Expression.Lambda<Func<T, Int32>>(nonNullProperty, arg);

                Expression converted = Expression.Convert(temp.Body, typeof(object));

                return Expression.Lambda<Func<T, object>>(converted, temp.Parameters);
            }

            if (type == typeof(DateTime))
            {
                var temp = Expression.Lambda<Func<T, DateTime>>(nonNullProperty, arg);

                Expression converted = Expression.Convert(temp.Body, typeof(object));

                return Expression.Lambda<Func<T, object>>(converted, temp.Parameters);
            }

            return Expression.Lambda<Func<T, object>>(nonNullProperty, arg);
        }

        public static Expression<Func<T, bool>> BuildQuery<T>(string propertyName, SearchOperationEnum operation, object propertyValue)
        {
            var parameterExp = Expression.Parameter(typeof(T), typeof(T).ToString());
            //   var propertyExp = Expression.PropertyOrField(parameterExp, propertyName);
            var constant = Expression.Constant(propertyValue);

            Expression propertyExp = parameterExp;
            foreach (string prop in propertyName.Split('.'))
            {
                propertyExp = Expression.PropertyOrField(propertyExp, prop);
            }

            Type type = propertyExp.Type;
            Expression nonNullProperty = propertyExp; //转换nullable的字段类型

            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(propertyExp, type);
            }

            switch (operation)
            {
                case SearchOperationEnum.Contains:
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var someValue = Expression.Constant(propertyValue, typeof(string));
                        var methodExp = Expression.Call(propertyExp, method, someValue);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }

                case SearchOperationEnum.Equal:
                    {
                        var methodExp = Expression.Equal(nonNullProperty, constant);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }

                case SearchOperationEnum.NotEqual:
                    {
                        var methodExp = Expression.NotEqual(nonNullProperty, constant);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }

                case SearchOperationEnum.GreaterThan:
                    {
                        var methodExp = Expression.GreaterThan(nonNullProperty, constant);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }
                case SearchOperationEnum.LesserThan:
                    {
                        var methodExp = Expression.LessThan(nonNullProperty, constant);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }
                case SearchOperationEnum.GreaterThanOrEqual:
                    {
                        var methodExp = Expression.GreaterThanOrEqual(nonNullProperty, constant);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }
                case SearchOperationEnum.LesserThanOrEqual:
                    {
                        var methodExp = Expression.LessThanOrEqual(nonNullProperty, constant);
                        return Expression.Lambda<Func<T, bool>>(methodExp, parameterExp);
                    }
            }

            //var parameterExp = Expression.Parameter(typeof(T), "type");
            //var propertyExp = Expression.Property(parameterExp, propertyName);
            //MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            //var someValue = Expression.Constant(propertyValue, typeof(string));
            //var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            //return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);

            return null;
        }

        public static Specification<TEntity> BuildSpecification<TEntity>(List<SearchCondition> list) where TEntity : Entity
        {
            Specification<TEntity> spec = new TrueSpecification<TEntity>();
            if (list != null)
            {
                foreach (SearchCondition sc in list)
                {
                    Expression<Func<TEntity, bool>> e = BuildQuery<TEntity>(sc.PropertyName, sc.Operation, sc.PropertyValue);
                    var ds = new DirectSpecification<TEntity>(e);
                    spec &= ds;
                }
            }
            return spec;
        }

        public static Specification<TEntity> BuildSpecification<TEntity>(List<SearchConditionGroup> list) where TEntity : Entity
        {
            Specification<TEntity> spec = new TrueSpecification<TEntity>();

            if (list != null)
            {
                foreach (var scg in list)
                {
                    var childSpec = BuildSpecification<TEntity>(scg.ConditionList);

                    if (scg.ConstraintType == ConstraintType.And)
                        spec &= childSpec;

                    if (scg.ConstraintType == ConstraintType.Or)
                        spec |= childSpec;
                }
            }

            return spec;
        }
    }
}