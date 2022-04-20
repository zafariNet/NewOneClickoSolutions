using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneClickSolutions.Infrastructure.Application;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Querying;
using OneClickSolutions.Infrastructure.Eventing;
using OneClickSolutions.Infrastructure.Functional;
using OneClickSolutions.Infrastructure.Querying;
using System.Linq.Expressions;
using static OneClickSolutions.Infrastructure.Extensions.EntityExtensions;
using static OneClickSolutions.Infrastructure.GuardToolkit.Guard;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Extensions;
namespace OneClickSolutions.Application.Common;
public class BaseEntityHandler<TEntity, TKey> : ApplicationService
            where TEntity : Entity<TKey>
            where TKey : IEquatable<TKey>
{
    protected readonly IEventBus _bus;
    protected readonly IDbContext _context;
    protected readonly IUnitOfWork _uow;
    protected readonly IMapper _mapper;

    protected readonly DbSet<TEntity> EntitySet;
    protected virtual IQueryable<TEntity> FindEntityQueryable => EntitySet.AsNoTracking();
    protected BaseEntityHandler(IEventBus bus, IDbContext context, IUnitOfWork uow, IMapper mapper)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _context = context;
        _uow = uow;
        _mapper = mapper;
        EntitySet = _context.Set<TEntity>();

    }

    /// <summary>
    /// Finde an Entity by Id
    /// </summary>
    /// <typeparam name="TViewModel">Type of view Model</typeparam>
    /// <param name="id">Entity's Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Maybe<TViewModel>> FindAsync<TViewModel>(TKey id, CancellationToken cancellationToken = default) where TViewModel : class 
    {
        var models = await FindAsync<TViewModel>(IdEqualityExpression<TEntity, TKey>(id), cancellationToken);
        
        return models.FirstOrDefault();
    }

    /// <summary>
    /// Finde a model by a predicate
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <param name="predicate">Lambda predicate</param>
    /// <param name="cancellationToken"></param>
    /// <returns>view Model</returns>
    protected async Task<IReadOnlyList<TViewModel>> FindAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate,
    CancellationToken cancellationToken = default)
    {
        var entityList = await FindEntityListAsync(predicate, cancellationToken);

        var modelList = _mapper.Map < IReadOnlyList<TEntity>, IReadOnlyList<TViewModel>  > (entityList);

        await AfterFindAsync();
        return modelList;
    }


    /// <summary>
    /// Find a list of models by Ids
    /// </summary>
    /// <typeparam name="TViewModel">Readonly list of view models</typeparam>
    /// <param name="ids">model Ids</param>
    /// <param name="cancellationToken"></param>
    /// <returns>read only list of view models</returns>
    public Task<IReadOnlyList<TViewModel>> FindListAsync<TViewModel>(IEnumerable<TKey> ids,
    CancellationToken cancellationToken = default)
    {
        return FindAsync<TViewModel>(e => ids.Contains(e.Id), cancellationToken);
    }

    /// <summary>
    /// list of paged models
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IPagedResult<TViewModel>> FindPagedListAsync<TViewModel>(IPagedRequest request,
    CancellationToken cancellationToken = default)
    {
        var pagedList = await FindEntityPagedListAsync(request, cancellationToken);

        var result = new PagedResult<TViewModel>
        {
            ItemList = _mapper.Map<IReadOnlyList<TEntity>, IReadOnlyList<TViewModel>>(pagedList.ItemList),
            TotalCount = pagedList.TotalCount
        };

        await AfterFindAsync();

        return result;
    }

    protected async Task UpdateEntityListAsync(IReadOnlyList<TEntity> entityList,
    CancellationToken cancellationToken)
    {
        _context.UpdateGraph(entityList);
        await _context.SaveChangesAsync(cancellationToken);
        _context.MarkUnchanged(entityList);
    }

    public Task<Result> CreateAsync(TEntity model, CancellationToken cancellationToken = default)
    {
        ArgumentNotNull(model, nameof(model));

        return CreateEntityListAsync(new[] { model }, cancellationToken);
    }

    protected async Task<Result> CreateEntityListAsync(IReadOnlyList<TEntity> entityList,
    CancellationToken cancellationToken)
    {
        try
        {
            await _uow.BeginTransaction();
            await EntitySet.AddRangeAsync(entityList, cancellationToken);
            await DispachDomainEvents(entityList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _context.MarkUnchanged(entityList);
            await _uow.CommitTransaction(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _uow.RollbackTransaction();
            return Fail(ex.Message);
        }
        
    }


    #region Private Methods

    private async Task DispachDomainEvents(IReadOnlyList<TEntity> entityList,
    CancellationToken cancellationToken)
    {
        foreach(var entity in entityList)
        {
            await _bus.Dispatch(entity.DomainEvents, cancellationToken);
        }
    }
    private async Task<IReadOnlyList<TEntity>> FindEntityListAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await FindEntityQueryable.Where(predicate).ToListAsync(cancellationToken);
    }

    private Task<IPagedResult<TEntity>> FindEntityPagedListAsync(IPagedRequest request,
      CancellationToken cancellationToken = default)
    {
        return FindEntityQueryable.ToPagedListAsync(request, cancellationToken);
    }

    protected virtual Task AfterFindAsync()
    {
        return Task.CompletedTask;
    }

    #endregion
}
