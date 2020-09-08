using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RSoft.Framework.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Infra.Data
{

    public abstract class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>
        where TEntity : EntityBase<TEntity>
        where TKey : struct
    {

        #region Local objects/variables

        /// <summary>
        /// Database context object
        /// </summary>
        protected DbContext _ctx;

        /// <summary>
        /// Dbset object
        /// </summary>
        protected DbSet<TEntity> _dbSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new repository instance
        /// </summary>
        /// <param name="ctx">Data base context object</param>
        public RepositoryBase(DbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<TEntity>();
        }

        #endregion

        #region Public Methods

        ///<inheritdoc/>
        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<TEntity> tsk = await _dbSet.AddAsync(entity, cancellationToken).AsTask();
            return tsk.Entity;
        }

        ///<inheritdoc/>
        public virtual TEntity Update(TEntity entity)
            => _dbSet.Update(entity).Entity;

        ///<inheritdoc/>
        public virtual async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.Run(() =>
            {
                return _dbSet;
            }, cancellationToken);
        }

        ///<inheritdoc/>
        public virtual async Task<TEntity> GetByIdAsync(TKey key, CancellationToken cancellationToken = default)
            => await GetByIdAsync(key, false, cancellationToken);

        ///<inheritdoc/>
        public virtual async Task<TEntity> GetByIdAsync(TKey key, bool includeDeleted, CancellationToken cancellationToken = default)
        {

            if (cancellationToken.IsCancellationRequested)
                return null;

            object[] keyValues = new object[] { key };

            TEntity entity = await _dbSet.FindAsync(keyValues: keyValues, cancellationToken: cancellationToken);

            if (!includeDeleted)
            {
                if (entity != null)
                {
                    if (entity is ISoftDeletion x && x.IsDeleted)
                        entity = default;
                }
            }

            if (cancellationToken.IsCancellationRequested)
                return null;

            return entity;

        }

        ///<inheritdoc/>
        public Task<IQueryable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            Task<IQueryable<TEntity>> task = Task.Factory.StartNew(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return _dbSet.Where(predicate);
            });
            return task;
        }

        ///<inheritdoc/>
        public virtual void Delete(TEntity entity)
            => _dbSet.Remove(entity);

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Flag to indicate to release internal resource</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }

                _dbSet = null;
                _ctx = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Destroy object instance and release resources
        /// </summary>
        ~RepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
