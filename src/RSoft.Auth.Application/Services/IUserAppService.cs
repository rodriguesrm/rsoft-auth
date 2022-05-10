using Microsoft.AspNetCore.Http;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Design.Application.Services;
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
        /// <param name="clientId">Application-Client application id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<UserDto>> GetAllAsync(Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add application-client for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="clientId">Application-Client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove application-client for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="clientId">Application-Client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> RemoveAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds roles for user
        /// </summary>
        /// <param name="clientId">Application-client id key</param>
        /// <param name="userId">User id key</param>
        /// <param name="roles">List of role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddRoleAsync(Guid clientId, Guid userId, IEnumerable<Guid> roles, CancellationToken cancellationToken);

        /// <summary>
        /// Remove a role for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roleId">Role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);

    }

}
