using OneClickSolutions.Infrastructure.Querying;

namespace OneClickSolutions.Infrastructure.Cqrs.Queries
{
    public interface IPagedQuery<out TReadModel> : IQuery<IPagedResult<TReadModel>>, IPagedRequest
    {
    }

    public abstract class PagedQuery<TReadModel> : PagedRequest, IPagedQuery<TReadModel>
    {
    }
}