﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.Eventing;
using OneClickSolutions.Infrastructure.Functional;
using OneClickSolutions.Infrastructure.Mapping;
using OneClickSolutions.Infrastructure.Querying;
using OneClickSolutions.Infrastructure.Transaction;
using OneClickSolutions.Infrastructure.Validation;
using static OneClickSolutions.Infrastructure.GuardToolkit.Guard;
using static OneClickSolutions.Infrastructure.Mapping.MappingExtensions;
using static OneClickSolutions.Infrastructure.Extensions.EntityExtensions;
namespace OneClickSolutions.Infrastructure.Application
{
    public abstract class EntityServiceBase<TEntity, TKey, TReadModel, TModel,
        TFilteredPagedRequest> : ApplicationService,
        IEntityService<TKey, TReadModel, TModel, TFilteredPagedRequest>
        where TEntity : Entity<TKey>, new()
        where TModel : MasterModel<TKey>
        where TReadModel : ReadModel<TKey>
        where TFilteredPagedRequest : IFilteredPagedRequest
        where TKey : IEquatable<TKey>
    {
        protected readonly IEventBus EventBus;

        protected EntityServiceBase(IEventBus bus)
        {
            EventBus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        [SkipValidation]
        //What ?
        public abstract Task<IPagedResult<TReadModel>> FetchPagedListAsync(TFilteredPagedRequest request,
            CancellationToken cancellationToken = default);

        public async Task<Maybe<TModel>> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var models = await FindAsync(IdEqualityExpression<TEntity, TKey>(id), cancellationToken);

            return models.FirstOrDefault();
        }

        public Task<IReadOnlyList<TModel>> FindListAsync(IEnumerable<TKey> ids,
            CancellationToken cancellationToken = default)
        {
            return FindAsync(e => ids.Contains(e.Id), cancellationToken);
        }

        public Task<IReadOnlyList<TModel>> FindListAsync(CancellationToken cancellationToken = default)
        {
            return FindAsync(_ => true, cancellationToken);
        }

        [SkipValidation]
        public async Task<IPagedResult<TModel>> FindPagedListAsync(IPagedRequest request,
            CancellationToken cancellationToken = default)
        {
            var pagedList = await FindEntityPagedListAsync(request, cancellationToken);

            var result = new PagedResult<TModel>
            {
                ItemList = pagedList.ItemList.MapReadOnlyList(MapToModel),
                TotalCount = pagedList.TotalCount
            };

            await AfterFindAsync(result.ItemList, cancellationToken);

            return result;
        }

        public virtual Task<TModel> CreateNewAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<TModel>(null);

        [Transactional]
        public Task<Result> CreateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            ArgumentNotNull(model, nameof(model));

            return CreateAsync(new[] { model }, cancellationToken);
        }

        [Transactional]
        public async Task<Result> CreateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
        {
            var modelList = models.ToList();

            var result = await BeforeCreateAsync(modelList, cancellationToken);
            if (result.Failed) return result;

            var entityList = modelList.MapReadOnlyList<TModel, TEntity>(MapToEntity);

            await AfterMappingAsync(modelList, entityList, cancellationToken);

            result = await EventBus.DispatchCreatingEvent<TModel, TKey>(modelList, cancellationToken);
            if (result.Failed) return result;

            await CreateEntityListAsync(entityList, cancellationToken);

            Map(entityList, modelList, MapToModel);

            result = await AfterCreateAsync(modelList, cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.DispatchCreatedEvent<TModel, TKey>(modelList, cancellationToken);

            return result;
        }

        [Transactional]
        public Task<Result> EditAsync(TModel model, CancellationToken cancellationToken = default)
        {
            ArgumentNotNull(model, nameof(model));

            return EditAsync(new[] { model }, cancellationToken);
        }

        [Transactional]
        public async Task<Result> EditAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
        {
            var modelList = models.ToList();

            var ids = modelList.Select(m => m.Id).ToList();
            var entityList = await FindEntityListAsync(e => ids.Contains(e.Id), cancellationToken);

            var modifiedList = modelList.ToModifiedList<TEntity, TModel, TKey>(entityList, MapToModel);

            var result = await BeforeEditAsync(modifiedList, entityList, cancellationToken);
            if (result.Failed) return result;

            Map(modelList, entityList, MapToEntity);

            await AfterMappingAsync(modelList, entityList, cancellationToken);

            result = await EventBus.DispatchEditingEvent<TModel, TKey>(modifiedList, cancellationToken);
            if (result.Failed) return result;

            await UpdateEntityListAsync(entityList, cancellationToken);

            Map(entityList, modelList, MapToModel);

            result = await AfterEditAsync(modifiedList, entityList, cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.DispatchEditedEvent<TModel, TKey>(modifiedList, cancellationToken);

            return result;
        }

        [Transactional, SkipValidation]
        public Task<Result> DeleteAsync(TModel model, CancellationToken cancellationToken = default)
        {
            ArgumentNotNull(model, nameof(model));

            return DeleteAsync(new[] { model }, cancellationToken);
        }

        [Transactional, SkipValidation]
        public virtual async Task<Result> DeleteAsync(IEnumerable<TModel> models,
            CancellationToken cancellationToken = default)
        {
            var modelList = models.ToList();

            var result = await BeforeDeleteAsync(modelList, cancellationToken);
            if (result.Failed) return result;

            var entityList = modelList.MapReadOnlyList<TModel, TEntity>(MapToEntity);

            result = await EventBus.DispatchDeletingEvent<TModel, TKey>(modelList, cancellationToken);
            if (result.Failed) return result;

            await RemoveEntityListAsync(entityList, cancellationToken);

            result = await AfterDeleteAsync(modelList, cancellationToken);
            if (result.Failed) return result;

            result = await EventBus.DispatchDeletedEvent<TModel, TKey>(modelList, cancellationToken);

            return result;
        }

        [Transactional, SkipValidation]
        public async Task<Result> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var model = await FindAsync(id, cancellationToken);
            if (model.HasValue) return await DeleteAsync(model.Value, cancellationToken);

            return Ok();
        }

        [Transactional, SkipValidation]
        public async Task<Result> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            var models = await FindListAsync(ids, cancellationToken);
            if (models.Any()) return await DeleteAsync(models, cancellationToken);

            return Ok();
        }

        protected async Task<IReadOnlyList<TModel>> FindAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var entityList = await FindEntityListAsync(predicate, cancellationToken);

            var modelList = entityList.MapReadOnlyList(MapToModel);

            await AfterFindAsync(modelList, cancellationToken);

            return modelList;
        }

        protected virtual Task AfterFindAsync(IReadOnlyList<TModel> models, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterMappingAsync(IReadOnlyList<TModel> models, IReadOnlyList<TEntity> entityList,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<Result> BeforeCreateAsync(IReadOnlyList<TModel> models,
            CancellationToken cancellationToken)
        {
            return Result.NoneTask;
        }

        protected virtual Task<Result> AfterCreateAsync(IReadOnlyList<TModel> models,
            CancellationToken cancellationToken)
        {
            return Result.NoneTask;
        }

        protected virtual Task<Result> BeforeEditAsync(IReadOnlyList<ModifiedModel<TModel>> models,
            IReadOnlyList<TEntity> entityList,
            CancellationToken cancellationToken)
        {
            return Result.NoneTask;
        }

        protected virtual Task<Result> AfterEditAsync(IReadOnlyList<ModifiedModel<TModel>> models,
            IReadOnlyList<TEntity> entityList,
            CancellationToken cancellationToken)
        {
            return Result.NoneTask;
        }

        protected virtual Task<Result> BeforeDeleteAsync(IReadOnlyList<TModel> models,
            CancellationToken cancellationToken)
        {
            return Result.NoneTask;
        }

        protected virtual Task<Result> AfterDeleteAsync(IReadOnlyList<TModel> models,
            CancellationToken cancellationToken)
        {
            return Result.NoneTask;
        }

        protected abstract void MapToEntity(TModel model, TEntity entity);
        protected abstract TModel MapToModel(TEntity entity);

        protected abstract Task<IReadOnlyList<TEntity>> FindEntityListAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken);

        protected abstract Task<IPagedResult<TEntity>> FindEntityPagedListAsync(IPagedRequest request,
            CancellationToken cancellationToken = default);

        protected abstract Task UpdateEntityListAsync(IReadOnlyList<TEntity> entityList,
            CancellationToken cancellationToken);

        protected abstract Task CreateEntityListAsync(IReadOnlyList<TEntity> entityList,
            CancellationToken cancellationToken);

        protected abstract Task RemoveEntityListAsync(IReadOnlyList<TEntity> entityList,
            CancellationToken cancellationToken);
    }
}