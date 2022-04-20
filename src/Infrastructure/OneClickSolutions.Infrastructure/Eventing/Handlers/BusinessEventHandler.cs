using OneClickSolutions.Infrastructure.Functional;
using Microsoft.Extensions.DependencyInjection;

namespace OneClickSolutions.Infrastructure.Eventing.Handlers
{
    internal abstract class BusinessEventHandler
    {
        public abstract Task<Result> Handle(IBusinessEvent businessEvent, CancellationToken cancellationToken,
            IServiceProvider serviceFactory,
            Func<IEnumerable<Func<IBusinessEvent, CancellationToken, Task<Result>>>, IBusinessEvent,
                    CancellationToken, Task<Result>>
                dispatch);
    }

    internal class BusinessEventHandlerImpl<TBusinessEvent> : BusinessEventHandler
        where TBusinessEvent : IBusinessEvent
    {
        public override Task<Result> Handle(IBusinessEvent businessEvent, CancellationToken cancellationToken,
            IServiceProvider serviceFactory,
            Func<IEnumerable<Func<IBusinessEvent, CancellationToken, Task<Result>>>, IBusinessEvent,
                    CancellationToken, Task<Result>>
                dispatch)
        {
            var handlers = serviceFactory.GetServices<IBusinessEventHandler<TBusinessEvent>>()
                .Select(handler => new Func<IBusinessEvent, CancellationToken, Task<Result>>(
                    (theBusinessEvent, theCancellationToken) =>
                        handler.Handle((TBusinessEvent)theBusinessEvent, theCancellationToken)));

            return dispatch(handlers, businessEvent, cancellationToken);
        }
    }

}