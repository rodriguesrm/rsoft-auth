using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Cross.Common.Options;
using Microsoft.Extensions.Options;
using RSoft.Helpers.Security;
using System.Threading.Tasks;
using System.Threading;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// User domain service operations
    /// </summary>
    public class UserDomainService : DomainServiceBase<User, Guid, IUserRepository>, IUserDomainService
    {

        #region Local objects/variables

        private readonly SecurityOptions _securityOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        /// <param name="securityOptions">Security options configuration</param>
        public UserDomainService(IUserRepository repository, IOptions<SecurityOptions> securityOptions) : base(repository)
        {
            _securityOptions = securityOptions?.Value;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Convert password to MD5 Hash
        /// </summary>
        /// <param name="password">Password to convert</param>
        private string ConvertPassword(string password)
        {
            MD5.HashMD5($"{password}|{_securityOptions.Secret}", out string senhaMD5);
            return senhaMD5.ToLower();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get user by login and password
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User passoword</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        public async Task<User> GetByLoginAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            
            //BACKLOG: Add LDAP Authenticate

            if (cancellationToken.IsCancellationRequested) return null;
            User user = await _repository.GetByLoginAsync(login, cancellationToken);
            if (user != null)
            {
                if (user.Credential.Password != ConvertPassword(password))
                    user = null;
             }
            return user;
        }

        #endregion

    }

}
