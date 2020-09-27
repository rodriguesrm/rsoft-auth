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
        /// Create token for generating password credentials (first access/reset password) and send by email
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
        /// <param name="login">Login/User name</param>
        /// <param name="password">User password</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken);

    }

}
