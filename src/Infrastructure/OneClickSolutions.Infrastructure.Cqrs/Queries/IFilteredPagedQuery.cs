using OneClickSolutions.Infrastructure.Querying;

namespace OneClickSolutions.Infrastructure.Cqrs.Queries
{
    public interface IFilteredPagedQuery<out TReadModel> : IQuery<IPagedResult<TReadModel>>, IFilteredPagedRequest
    {
    }

    public abstract class FilteredPagedQuery<TReadModel> : FilteredPagedRequest, IFilteredPagedQuery<TReadModel>
    {
    }
}