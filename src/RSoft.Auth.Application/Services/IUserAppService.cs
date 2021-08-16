using RSoft.Auth.Application.Model;
using RSoft.Lib.Common.Models;
using RSoft.Lib.DDD.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// User application service interface contract
    /// </summary>
    public interface IUserAppService : IAppServiceBase<UserDto, Guid>
    {

        /// <summary>
        /// Get all rows
        /// </summary>
        /// <param name="scopeId">Scope application id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<UserDto>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add scope for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove scope for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> RemoveScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds roles for user
        /// </summary>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="userId">User id key</param>
        /// <param name="roles">List of role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddRoleAsync(Guid scopeId, Guid userId, IEnumerable<Guid> roles, CancellationToken cancellationToken);

        /// <summary>
        /// Remove a role for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roleId">Role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    }

}
