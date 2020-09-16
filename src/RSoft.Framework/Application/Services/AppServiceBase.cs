using RSoft.Framework.Application.Dto;
using RSoft.Framework.Domain.Entities;
using RSoft.Framework.Domain.Services;
using RSoft.Framework.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Application.Services
{

    /// <summary>
    /// Application service abstract class base
    /// </summary>
    /// <typeparam name="TDto">Dto type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class AppServiceBase<TDto, TEntity, TKey> : IAppServiceBase<TDto, TKey>
        where TDto : IAppDto
        where TEntity : EntityBase<TEntity>
        where TKey : struct
    {

        #region Local objects/variables

        protected IUnitOfWork _uow;
        protected IDomainServiceBase<TEntity, TKey> _dmn;

        #endregion

        #region Constructors

        /// <summary>
        /// Cria uma nova instância do objeto 'AppService'
        /// </summary>
        /// <param name="uow">Objeto de unidade de trabalho / controle de transações</param>
        /// <param name="dmn">Objeto de serviço de domínio</param>
        public AppServiceBase(IUnitOfWork uow, IDomainServiceBase<TEntity, TKey> dmn)
        {
            _uow = uow;
            _dmn = dmn;
        }

        #endregion

        #region Local methods


        /// <summary>
        /// Validate entity
        /// </summary>
        /// <param name="entity">Entity instance</param>
        protected virtual void ValidateEntity(TEntity entity)
        {
            entity.Validate();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Get entity by key
        /// </summary>
        /// <param name="dto">Dto instance</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected abstract Task<TEntity> GetEntityByKeyAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Dto instance</param>
        /// <param name="entity">Entity instance</param>
        protected abstract void MapToEntity(TDto dto, TEntity entity);

        /// <summary>
        /// Map dto to entity
        /// </summary>
        /// <param name="dto">Dto instance</param>
        protected abstract TEntity MapToEntity(TDto dto);

        /// <summary>
        /// Map entity to dto
        /// </summary>
        /// <param name="entity">Entity instance</param>
        protected abstract TDto MapToDto(TEntity entity);

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public virtual async Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken = default)
        {

            bool usarTransacao = !_uow.TransactionStarted;

            TEntity entity = MapToEntity(dto);
            ValidateEntity(entity);

            TDto result;
            if (entity.Valid)
            {
                if (usarTransacao) await _uow.BeginTransactionAsync(cancellationToken);

                TEntity dmnResult = await _dmn.AddAsync(entity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                result = MapToDto(dmnResult);

                if (usarTransacao) await _uow.CommitAsync(cancellationToken);
            }
            else
            {
                result = MapToDto(entity);
            }

            return result;

        }

        ///<inheritdoc/>
        public virtual async Task<TDto> UpdateAsync(TKey[] keys, TDto dto, CancellationToken cancellationToken = default)
        {

            bool usarTransacao = !_uow.TransactionStarted;

            TEntity entity = await GetEntityByKeyAsync(dto, cancellationToken);
            MapToEntity(dto, entity);
            ValidateEntity(entity);

            TDto result;
            if (entity.Valid)
            {
                if (usarTransacao) await _uow.BeginTransactionAsync(cancellationToken);

                TEntity dmnResult = _dmn.Update(keys, entity);
                await _uow.SaveChangesAsync(cancellationToken);
                result = MapToDto(dmnResult);

                if (usarTransacao) await _uow.CommitAsync(cancellationToken);
            }
            else
            {
                result = MapToDto(entity);
            }

            return result;

        }

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<TEntity> dmnResult = await _dmn.GetAllAsync(cancellationToken);
            dmnResult = dmnResult.ToList();
            if (dmnResult?.Count() == 0)
                return null;
            IEnumerable<TDto> result =
                dmnResult
                    .Select(e => MapToDto(e))
                    .ToList();
            return result;
        }

        ///<inheritdoc/>
        public virtual async Task<TDto> GetByKeyAsync(TKey[] keys, CancellationToken cancellationToken = default)
        {
            TEntity entity = await _dmn.GetByKeyAsync(keys, cancellationToken);
            if (entity == null)
                return default;
            TDto result = MapToDto(entity);
            return result;
        }

        ///<inheritdoc/>
        public virtual async Task DeleteAsync(TKey[] keys, CancellationToken cancellationToken = default)
        {

            bool useTransaction = !_uow.TransactionStarted;
            if (useTransaction) await _uow.BeginTransactionAsync(cancellationToken);
            _dmn.Delete(keys);
            await _uow.SaveChangesAsync(cancellationToken);
            if (useTransaction) await _uow.CommitAsync(cancellationToken);

        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Indicate release internal resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _uow?.Dispose();
                    _dmn?.Dispose();
                }

                _uow = null;
                _dmn = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Destroy this object instance
        /// </summary>
        ~AppServiceBase()
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
