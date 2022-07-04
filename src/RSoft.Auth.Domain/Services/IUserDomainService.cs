using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Cross.Common.Model.Args;
using System.Collections.Generic;
using RSoft.Lib.Design.Domain.Services;
using RSoft.Lib.Common.Models;

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
        /// <param name="appKey">Authentication application-client id value</param>
        /// <param name="login">User login</param>
        /// <param name="password">User passoword</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <returns>A user instance if sucess / Id key user when password is incorrect</returns>
        Task<(User, Guid?)> GetByLoginAsync(Guid appKey, string login, string password, CancellationToken cancellationToken = default);

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
        /// <param name="urlCredential">Url of the page to be informed in the credential creation email. The parameters 'type=create' and 'token={token}' will be added via query-string</param>
        /// <param name="sendMailCallBack">Callback function for sending e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetFirstAccessAsync(string email, string urlCredential, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all user roles in a specific application-client
        /// </summary>
        /// <param name="userId">User id key value</param>
        ICollection<Role> GetRolesByUserAsync(Guid userId);

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
        /// <param name="urlCredential">Url of the page to be informed in the credential creation email. The parameters 'type=create' and 'token={token}' will be added via query-string</param>
        /// <param name="sendMailCallBack">Callback function for sending e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetResetAccessAsync(string email, string urlCredential, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get user by document number
        /// </summary>
        /// <param name="document">Document number</param>
        /// <param name="cancellationToken"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<User> GetByDocumentAsync(string document, CancellationToken cancellationToken = default); 

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
        /// Logs the user authentication failure count and controls the lockout
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task MarkLoginFail(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Clear lockout user information
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task ClearLockout(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Get all rows
        /// </summary>
        /// <param name="clientId">Application-Client id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<User>> GetAllAsync(Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add application-client for user
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="clientId"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove application-client for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="clientId">Application-client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> RemoveAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds roles for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roles">List of roles</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> AddRoleAsync(Guid userId, IEnumerable<Role> roles, CancellationToken cancellationToken);

        /// <summary>
        /// Remove a role for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roleId">Role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);

    }

}
