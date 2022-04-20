using System;
using System.Linq.Expressions;
using OneClickSolutions.Infrastructure.Domain;

namespace OneClickSolutions.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static Expression<Func<TEntity, bool>> IdEqualityExpression<TEntity, TKey>(TKey id)
            where TEntity : Entity<TKey>
            where TKey : IEquatable<TKey>
        {
            var instanceExpression = Expression.Parameter(typeof(TEntity));

            var bodyExpression = Expression.Equal(
                Expression.PropertyOrField(instanceExpression, nameof(Entity<TKey>.Id)),
                Expression.Constant(id, typeof(TKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(bodyExpression, instanceExpression);
        }
    }
}