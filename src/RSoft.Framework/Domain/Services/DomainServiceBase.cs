using RSoft.Framework.Domain.Entities;
using RSoft.Framework.Infra.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Domain.Services
{

    /// <summary>
    /// Abstract class domain service
    /// </summary>
    /// <typeparam name="TEntity">Entity tipe</typeparam>
    /// <typeparam name="TRepository">Repository type</typeparam>
    public abstract class DomainServiceBase<TEntity, TKey, TRepository> : IDomainServiceBase<TEntity, TKey>
        where TEntity : EntityBase<TEntity>
        where TKey : struct
        where TRepository : class, IRepositoryBase<TEntity, TKey>
    {

        #region Local objects/variables

        protected TRepository _repository;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new domain service instance
        /// </summary>
        /// <param name="repository">Principal repository</param>
        public DomainServiceBase(TRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity.Invalid) return entity;
            if (cancellationToken.IsCancellationRequested)
            {
                entity.AddNotification(entity.GetName(), "Operation was cancelled");
                return entity;
            }
            return await _repository.AddAsync(entity, cancellationToken);
        }


        ///<inheritdoc/>
        public TEntity Update(TKey[] keys, TEntity entity)
        {
            if (entity.Invalid) return entity;
            return _repository.Update(keys, entity);
        }

        ///<inheritdoc/>
        public async Task<TEntity> GetByKeyAsync(TKey[] keys, CancellationToken cancellationToken = default)
            => await _repository.GetByKeyAsync(keys, cancellationToken);

        ///<inheritdoc/>
        Task<IEnumerable<TEntity>> ICommonBase<TEntity, TKey>.GetAllAsync(CancellationToken cancellationToken)
            => _repository.GetAllAsync(cancellationToken);

        ///<inheritdoc/>
        public void Delete(TKey[] keys)
            => _repository.Delete(keys);

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Flag indicate dispose objects</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _repository?.Dispose();
                }

                _repository = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Destroy this object instance
        /// </summary>
        ~DomainServiceBase()
        {
            Dispose(false);
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}