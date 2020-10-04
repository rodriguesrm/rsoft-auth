using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Framework.Application.Model;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Cross.Common.Model.Args;
using System.Collections.Generic;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// User domain service interface
    /// </summary>
    public interface IUserDomainService : IDomainServiceBase<User, Guid>
    {

        /// <summary>
        /// Get user by login and password
        /// </summary>
        /// <param name="appKey">Authentication scope id value</param>
        /// <param name="appAccess">Authentication scope key access</param>
        /// <param name="login">User login</param>
        /// <param name="password">User passoword</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<User> GetByLoginAsync(Guid appKey, Guid appAccess, string login, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if the informed login is available
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="login">User login/User name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<bool> LoginIsAvailableAsync(Guid userId, string login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create token for generating password credentials and send by email
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="sendMailCallBack">Callback function for sending e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetFirstAccessAsync(string email, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all user roles in a specific scope
        /// </summary>
        /// <param name="scopeId">Scope id key value</param>
        /// <param name="userId">User id key value</param>
        ICollection<Role> GetRolesByUserAsync(Guid scopeId, Guid userId);

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="tokenId">Token id value</param>
        /// <param name="loginOrEmail">User login or e-mail</param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string loginOrEmail, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Create token for reset password credentials and send by email
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="sendMailCallBack">Callback function for sending e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetResetAccessAsync(string email, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default);

        /// <summary>
        /// Save new password for recovery access
        /// </summary>
        /// <param name="tokenId">Token id value</param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> SetRecoveryAccessAsync(Guid tokenId, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="login">Informed user login</param>
        /// <param name="currentPassword">Current user password</param>
        /// <param name="newPassword">New user password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> ChangePasswordAsync(string login, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all rows
        /// </summary>
        /// <param name="scopeId">Scope application id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<User>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add scope for user
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="scopeId"></param>
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
        /// <param name="userId">User id key</param>
        /// <param name="roles">List of roles</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddRoleAsync(Guid userId, IEnumerable<Role> roles, CancellationToken cancellationToken);

    }

}
