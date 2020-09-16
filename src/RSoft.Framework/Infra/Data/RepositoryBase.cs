using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RSoft.Framework.Domain.Entities;
using RSoft.Framework.Exception;
using RSoft.Framework.Infra.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Infra.Data
{

    public abstract class RepositoryBase<TEntity, TTable, TKey> : IRepositoryBase<TEntity, TKey>
        where TEntity : EntityBase<TEntity>
        where TTable : TableBase<TTable>
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
        protected DbSet<TTable> _dbSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new repository instance
        /// </summary>
        /// <param name="ctx">Data base context object</param>
        public RepositoryBase(DbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<TTable>();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Map table to entity
        /// </summary>
        /// <param name="table">Table object</param>
        protected abstract TEntity Map(TTable table);

        /// <summary>
        /// Map entity to table for add action
        /// </summary>
        /// <param name="entity">Domain Entity object</param>
        protected abstract TTable MapForAdd(TEntity entity);

        /// <summary>
        /// Map entity to table for update action
        /// </summary>
        /// <param name="entity">Domain entity object</param>
        /// <param name="table">Table entity object</param>
        protected abstract TTable MapForUpdate(TEntity entity, TTable table);

        #endregion

        #region Public Methods

        ///<inheritdoc/>
        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!entity.Valid)
                throw new InvalidEntityException(nameof(entity));
            TTable table = MapForAdd(entity);
            EntityEntry<TTable> tsk = await _dbSet.AddAsync(table, cancellationToken).AsTask();
            entity = Map(tsk.Entity);
            return entity;
        }

        ///<inheritdoc/>
        public virtual TEntity Update(TKey[] keys, TEntity entity)
        {

            if (!entity.Valid)
                throw new InvalidEntityException(nameof(entity));

            TTable table = _dbSet.FindAsync(keyValues: keys).GetAwaiter().GetResult();
            if (table == null)
                throw new InvalidOperationException($"[{keys}] The data update operation cannot be completed because the entity does not exist in the database. The same may have been deleted.");

            table = MapForUpdate(entity, table);
            table = _dbSet.Update(table).Entity;

            entity = Map(table);
            return entity;
        }

        ///<inheritdoc/>
        public virtual async Task<TEntity> GetByKeyAsync(TKey[] keys, CancellationToken cancellationToken = default)
        {

            if (cancellationToken.IsCancellationRequested)
                return null;

            TTable table = await _dbSet.FindAsync(keys, cancellationToken);
            TEntity entity = Map(table);

            if (cancellationToken.IsCancellationRequested)
                return null;

            return entity;

        }

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            if (cancellationToken.IsCancellationRequested)
                return null;

            IEnumerable<TTable> rows = await _dbSet.ToListAsync(cancellationToken);
            IEnumerable<TEntity> entities = rows.ToList().Select(r => Map(r));

            if (cancellationToken.IsCancellationRequested)
                return null;

            return entities;

        }

        ///<inheritdoc/>
        public virtual void Delete(TKey[] keys)
        {
            TTable table = _dbSet.FindAsync(keyValues: keys).GetAwaiter().GetResult();
            _dbSet.Remove(table);
        }

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
