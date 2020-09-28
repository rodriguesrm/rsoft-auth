using RSoft.Auth.Application.Model;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Framework.Application.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// Credential application service interface
    /// </summary>
    public interface ICredentialAppService
    {

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="appKey">Authentication scope id</param>
        /// <param name="appAccess">Authentication scope key access</param>
        /// <param name="login">User login/param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<AuthenticateResult<UserDto>> AuthenticateAsync(Guid appKey, Guid appAccess, string login, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create token for password generation (first access) and send by email
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetFirstAccessAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="tokenId">Token id value</param>
        /// <param name="login">Login/User name</param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create token for generating new password and send by email
        /// </summary>
        /// <param name="loginOrEmail">User login or email</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetResetAccessAsync(string loginOrEmail, CancellationToken cancellationToken = default);

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
        /// Checks whether the informed login is available
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> LoginIsAvailableAsync(string login, Guid? userId,CancellationToken cancellationToken);

    }

}
