using RSoft.Auth.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using RSoft.Lib.Design.Infra.Data;

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
        /// <param name="clientId">Application-Client application id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<User>> GetAllAsync(Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add application-client to user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="clientId">Application-client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task AddUserAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove application-client from user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="clientId">Application-client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task RemoveUserAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken);

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
