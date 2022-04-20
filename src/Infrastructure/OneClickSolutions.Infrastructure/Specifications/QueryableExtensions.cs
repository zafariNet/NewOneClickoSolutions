using System.Linq;

namespace OneClickSolutions.Infrastructure.Specifications
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Specify<T>(this IQueryable<T> query, Specification<T> specification) where T : class
        {
            return query.Where(specification);
        }
    }
}