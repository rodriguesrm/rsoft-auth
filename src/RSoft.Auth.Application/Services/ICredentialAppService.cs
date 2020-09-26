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
        /// <param name="scopeId">Authentication scope id</param>
        /// <param name="scopeKey">Authentication scope key access</param>
        /// <param name="login">User login/param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<AuthenticateResult<UserDto>> AuthenticateAsync(Guid scopeId, Guid scopeKey, string login, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="tokenId">Token id value</param>
        /// <param name="password">User password</param>
        /// <param name="firstAccewss">Indicates whether the operation is first access</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> CreateCredentialAsync(Guid tokenId, string password, bool firstAccewss, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create token for password generation (first access) and send by email
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> FirstAccessAsync(string login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create token for generating new password and send by email
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> ResetPasswordAsync(string login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="authenticatedLogin">Authenticated user login</param>
        /// <param name="login">Informed user login</param>
        /// <param name="currentPassword">Current user password</param>
        /// <param name="newPassword">New user password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> ChangePasswordAsync(string authenticatedLogin, string login, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether the informed user exists
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="email">User E-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> IsRegistered(string login, string email, CancellationToken cancellationToken);

    }

}
