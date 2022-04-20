using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Cqrs.Commands;
using OneClickSolutions.Infrastructure.Functional;
using MediatR;

namespace OneClickSolutions.Infrastructure.Cqrs.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
       // where TRequest : ICommand
        where TResponse : Result
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            throw new System.NotImplementedException();
        }
    }
}