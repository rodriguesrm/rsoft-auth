using RSoft.Framework.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Infra.Data
{

    /// <summary>
    /// Generic repository interface
    /// </summary>
    public interface IRepositoryBase<TEntity, TKey>
        where TEntity : IEntity
        where TKey : struct
    {

        /// <summary>
        /// Add entity to context
        /// </summary>
        /// <param name="entity">Entity instance</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update entity on context
        /// </summary>
        /// <param name="entity">Entity instance</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Get row by id
        /// </summary>
        /// <param name="key">Row key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<TEntity> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entity rows
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove entity of context
        /// </summary>
        /// <param name="key">Entity id value</param>
        void Delete(TKey key);

    }

}