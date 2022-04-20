using MediatR;

namespace OneClickSolutions.Infrastructure.Cqrs.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
