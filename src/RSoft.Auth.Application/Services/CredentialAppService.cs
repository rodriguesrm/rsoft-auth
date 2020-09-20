using RSoft.Auth.Application.Model;
using RSoft.Auth.Domain.Services;
using RSoft.Framework.Application.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Services
{

    public class CredentialAppService : ICredentialAppService
    {

        #region Local objects/variables

        private readonly IUserDomainService _userDomain;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new CredentialAppService instance
        /// </summary>
        /// <param name="userDomain"></param>
        public CredentialAppService(IUserDomainService userDomain)
        {
            _userDomain = userDomain;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public Task<AuthenticateResult<UserDto>> AuthenticateAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<SimpleOperationResult> CreateCredentialAsync(Guid tokenId, string password, bool firstAccewss, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<PasswordProcessResult> FirstAccessAsync(string login, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<PasswordProcessResult> ResetPasswordAsync(string login, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<SimpleOperationResult> ChangePasswordAsync(string authenticatedLogin, string login, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<SimpleOperationResult> IsRegistered(string login, string email, CancellationToken cancellationToken)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion
    }
}