using AutoMapper;
using OneClickSolutions.Application.Common;
using OneClickSolutions.Application.DocumentTypes.Commands;
using OneClickSolutions.Domain.DocumentTypes;
using OneClickSolutions.Domain.DocumentTypes.Repositories;
using OneClickSolutions.Infrastructure.Cqrs.Commands;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.Eventing;
using OneClickSolutions.Infrastructure.Functional;
using OneClickSolutions.Infrastructure.Querying;
using System.Data;

namespace OneClickSolutions.Application.DocumentTypes.CommandHandlers
{
    public class CreateDocumentTypeCommandHandler : BaseEntityHandler<DocumentType,Guid>,
        ICommandHandler<CreateDocumentTypeCommand>
    {
        public CreateDocumentTypeCommandHandler(IEventBus bus,IDbContext dbCOntext,IUnitOfWork uow,IMapper mapper)
            :base(bus,dbCOntext,uow,mapper)
        {
        }
        public async Task<Result> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var pageRequest = new PagedRequest();
                //pageRequest.Page = 1;
                //pageRequest.PageSize = 10;
                //pageRequest.SetSortExpressions(new[] { new SortExpression("Name", true) });
                //var documentType = await FindPagedListAsync<DocumentType>(pageRequest);

                // await _uow.BeginTransaction(IsolationLevel.ReadCommitted, cancellationToken);
                //_documentRepository.Add(documentType);
                //await _uow.SaveChanges(cancellationToken);
                //await _bus.DispatchDomainEvents(documentType.FirstOrDefault());
                //await _uow.CommitTransaction(cancellationToken);

                var documentType = _mapper.Map<DocumentType>(request);
                await CreateAsync(documentType);

                return Result.Ok(documentType);
            }
            catch (Exception ex)
            {
                _uow.RollbackTransaction();
                return Result.Fail(ex.Message, "Some details");
            }


        }

        private TResult RunInsideTransaction<TResult>(IUnitOfWork dbContext, Func<TResult> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            TResult result;
            try
            {
                dbContext.BeginTransaction(isolationLevel);
                result = action.Invoke();
                dbContext.CommitTransaction();
            }
            catch (Exception)
            {
                dbContext.RollbackTransaction();
                throw;
            }

            return result;
        }
    }
}
