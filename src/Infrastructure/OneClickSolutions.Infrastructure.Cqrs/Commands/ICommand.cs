using OneClickSolutions.Infrastructure.Functional;
using MediatR;

namespace OneClickSolutions.Infrastructure.Cqrs.Commands
{
    public interface ICommand : ICommand<Result>
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult> where TResult : Result
    {
    }
}