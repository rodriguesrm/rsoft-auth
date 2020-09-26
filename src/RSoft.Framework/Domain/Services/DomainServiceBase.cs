using RSoft.Framework.Cross;
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
        protected IAuthenticatedUser _authenticatedUser;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new domain service instance
        /// </summary>
        /// <param name="repository">Principal repository</param>
        /// <param name="authenticatedUser">Authenticated user</param>
        public DomainServiceBase(TRepository repository, IAuthenticatedUser authenticatedUser)
        {
            _repository = repository;
            _authenticatedUser = authenticatedUser;
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Prepare save
        /// </summary>
        /// <param name="entity">Entity to save</param>
        /// <param name="isUpdate">Flag to indicate update action</param>
        public abstract void PrepareSave(TEntity entity, bool isUpdate);

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity.Invalid) return entity;
            if (cancellationToken.IsCancellationRequested)
            {
                entity.AddNotification(entity.GetName(), "Operation was cancelled");
                return entity;
            }
            PrepareSave(entity, false);
            TEntity savedEntity = await _repository.AddAsync(entity, cancellationToken);
            return savedEntity;
        }


        ///<inheritdoc/>
        public virtual TEntity Update(TKey key, TEntity entity)
        {
            if (entity.Invalid) return entity;
            PrepareSave(entity, true);
            TEntity savedEntity = _repository.Update(key, entity);
            return savedEntity;
        }

        ///<inheritdoc/>
        public virtual async Task<TEntity> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default)
            => await _repository.GetByKeyAsync(key, cancellationToken);

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await _repository.GetAllAsync(cancellationToken);

        ///<inheritdoc/>
        public virtual void Delete(TKey key)
            => _repository.Delete(key);

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
                _authenticatedUser = null;

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