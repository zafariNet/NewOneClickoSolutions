using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Extensions;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Hooks;
using OneClickSolutions.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using DbException = OneClickSolutions.Infrastructure.Exceptions.DbException;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Context
{
    public abstract class DbContextCore : DbContext, IDbContext, IUnitOfWork
    {
        private readonly IEnumerable<IHook> _hooks;
        private readonly List<string> _ignoredHookList = new();
        private IDbContextTransaction _transaction;
            protected DbContextCore(DbContextOptions options, IEnumerable<IHook> hooks) : base(options)
        {
            _hooks = hooks ?? throw new ArgumentNullException(nameof(hooks));
        }

        public DbConnection Connection => Database.GetDbConnection();
        public DbTransaction Transaction => _transaction?.GetDbTransaction();
        public bool HasTransaction => _transaction != null;

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (HasTransaction) return;

            _transaction = Database.BeginTransaction(isolationLevel);
        }

        public async Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (HasTransaction) return;

            _transaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public void CommitTransaction()
        {
            if (!HasTransaction) throw new NullReferenceException("Please call `BeginTransaction()` method first.");

            try
            {
                _transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (!HasTransaction) throw new NullReferenceException("Please call `BeginTransaction()` method first.");

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        Task<int> IUnitOfWork.SaveChanges(CancellationToken cancellationToken)
        {
            return SaveChangesAsync(cancellationToken);
        }

        Task IUnitOfWork.BeginTransaction(IsolationLevel isolationLevel,
            CancellationToken cancellationToken)
        {
            return BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        Task IUnitOfWork.CommitTransaction(CancellationToken cancellationToken)
        {
            return CommitTransactionAsync(cancellationToken);
        }

        public void RollbackTransaction()
        {
            if (!HasTransaction) throw new NullReferenceException("Please call `BeginTransaction()` method first.");

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void IgnoreHook(string hookName)
        {
            _ignoredHookList.Add(hookName);
        }

        public void UseTransaction(DbTransaction transaction)
        {
            Database.UseTransaction(transaction);
        }

        public void UseConnectionString(string connectionString)
        {
            Connection.ConnectionString = connectionString;
        }

        public string EntityHash<TEntity>(TEntity entity) where TEntity : class
        {
            var row = Entry(entity).ToDictionary(p => p.Metadata.Name != EFCoreShadow.Hash &&
                                                      !p.Metadata.ValueGenerated.HasFlag(ValueGenerated.OnUpdate) &&
                                                      !p.Metadata.IsShadowProperty());
            return EntityHash<TEntity>(row);
        }

        protected virtual string EntityHash<TEntity>(Dictionary<string, object> row) where TEntity : class
        {
            var json = JsonSerializer.Serialize(row, new JsonSerializerOptions {WriteIndented = true});
            using var hashAlgorithm = SHA256.Create();
            var byteValue = Encoding.UTF8.GetBytes(json);
            var byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            ChangeTracker.TrackGraph(rootEntity, callback);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result;
            try
            {
                var entryList = this.FindChangedEntries();

                ExecuteHooks<IPreActionHook>(entryList);

                ChangeTracker.AutoDetectChangesEnabled = false;
                result = await base.SaveChangesAsync(true, cancellationToken);
                
                ChangeTracker.AutoDetectChangesEnabled = true;

                ExecuteHooks<IPostActionHook>(entryList);

                //for RowIntegrity scenarios
                await base.SaveChangesAsync(true, cancellationToken);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message, e);
            }
            catch (DbUpdateException e)
            {
                throw new DbException(e.Message, e);
            }

            return result;
        }

        public override int SaveChanges()
        {
            int result;
            try
            {
                var entryList = this.FindChangedEntries();

                ExecuteHooks<IPreActionHook>(entryList);

                ChangeTracker.AutoDetectChangesEnabled = false;
                result = base.SaveChanges(true);
                ChangeTracker.AutoDetectChangesEnabled = true;

                ExecuteHooks<IPostActionHook>(entryList);

                //for RowIntegrity scenarios
                base.SaveChanges(true);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message, e);
            }
            catch (DbUpdateException e)
            {
                throw new DbException(e.Message, e);
            }

            return result;
        }

        public int ExecuteSqlInterpolatedCommand(FormattableString query)
        {
            return Database.ExecuteSqlInterpolated(query);
        }

        public int ExecuteSqlRawCommand(string query, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(query, parameters);
        }

        public Task<int> ExecuteSqlInterpolatedCommandAsync(FormattableString query)
        {
            return Database.ExecuteSqlInterpolatedAsync(query);
        }

        public Task<int> ExecuteSqlRawCommandAsync(string query, params object[] parameters)
        {
            return Database.ExecuteSqlRawAsync(query, parameters);
        }

        public override void Dispose()
        {
            _transaction?.Dispose();
            base.Dispose();
        }

        protected virtual void ExecuteHooks<THook>(IEnumerable<EntityEntry> entryList) where THook : IHook
        {
            foreach (var entry in entryList)
            {
                var hooks = _hooks.OfType<THook>()
                    .Where(hook => !_ignoredHookList.Contains(hook.Name))
                    .Where(hook => hook.HookState == entry.State).OrderBy(hook => hook.Order);

                foreach (var hook in hooks)
                {
                    var metadata = new HookEntityMetadata(entry);
                    hook.Hook(entry.Entity, metadata, this);
                }
            }
        }
    }
}