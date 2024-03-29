﻿using RSoft.Auth.Application.Model;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Lib.Common.Models;
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
        /// <param name="appKey">Authentication application-client id</param>
        /// <param name="login">User login/param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<AuthenticateResult<UserDto>> AuthenticateAsync(Guid appKey, string login, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create token for password generation (first access) and send by email
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="appToken">Application token to access mail-service api</param>
        /// <param name="urlCredential">Url of the page to be informed in the credential creation email. The parameters 'type=create' and 'token={token}' will be added via query-string</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetFirstAccessAsync(string email, string appToken, string urlCredential, CancellationToken cancellationToken = default);

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
        /// <param name="appToken">Application token to access mail-service api</param>
        /// <param name="urlCredential">Url of the page to be informed in the credential creation email. The parameters 'type=create' and 'token={token}' will be added via query-string</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<PasswordProcessResult> GetResetAccessAsync(string loginOrEmail, string appToken, string urlCredential, CancellationToken cancellationToken = default);

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
