using RSoft.Auth.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using RSoft.Lib.DDD.Infra.Data;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// User repository contract interface
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User, Guid>
    {

        /// <summary>
        /// Get user by login or e-mail
        /// </summary>
        /// <param name="login">User login or e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<User> GetByLoginAsync(string login, CancellationToken cancellationToken);

        /// <summary>
        /// List users by login or e-mail
        /// </summary>
        /// <param name="login">User login or e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<User>> ListByLoginAsync(string login, CancellationToken cancellationToken);

        /// <summary>
        /// Get all rows
        /// </summary>
        /// <param name="scopeId">Scope application id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<User>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add scope to user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task AddUserScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove scope from user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task RemoveUserScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken);

        /// <summary>
        /// Add role to user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roles">List of role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task AddUserRoleAsync(Guid userId, IEnumerable<Guid> roles, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove role from user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roleId">Role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task RemoveUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);

        /// <summary>
        /// Get user by document number
        /// </summary>
        /// <param name="document">Document number</param>
        /// <param name="cancellationToken"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<User> GetByDocumentAsync(string document, CancellationToken cancellationToken = default);
    }

}
