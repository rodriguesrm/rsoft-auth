using RSoft.Framework.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Application.Services
{

    /// <summary>
    /// Interface base de Application
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IAppServiceBase<TDto, TKey> : IDisposable
        where TDto : IAppDto
        where TKey : struct
    {

        /// <summary>
        /// Add an entity to the context
        /// </summary>
        /// <param name="dto">Dto object instance</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update entity in context
        /// </summary>
        /// <param name="dto">Instância do dto</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<TDto> UpdateAsync(TDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all rows
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get entity registration by key
        /// </summary>
        /// <param name="key">Row key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<TDto> GetByKeyAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove entity from context
        /// </summary>
        /// <param name="key">Row key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task DeleteAsync(TKey key, CancellationToken cancellationToken = default);

    }

}
