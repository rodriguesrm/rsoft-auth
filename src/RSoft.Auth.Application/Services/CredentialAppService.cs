using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Services;
using RSoft.Framework.Application.Model;
using System;
using System.Collections.Generic;
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
        public async Task<AuthenticateResult<UserDto>> AuthenticateAsync(string login, string password, CancellationToken cancellationToken = default)
        {

            bool success = false;
            UserDto userDto = null;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            User user = await _userDomain.GetByLoginAsync(login, password, cancellationToken);
            if (user != null)
            {
                if (user.Credential.ChangeCredentials)
                {
                    errors.Add("Authenticate", "User must change password");
                }
                else
                { 
                    if (user.IsActive)
                    {
                        success = true;
                        userDto = user.Map();
                    }
                    else
                    {
                        errors.Add("Authenticate", "Inactive or blocked user");
                    }
                }
            }
            else
            {
                errors.Add("Authenticate", "Invalid username and/or password!");
            }

            return new AuthenticateResult<UserDto>(success, userDto, errors);
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> CreateCredentialAsync(Guid tokenId, string password, bool firstAccewss, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> FirstAccessAsync(string login, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> ResetPasswordAsync(string login, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> ChangePasswordAsync(string authenticatedLogin, string login, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> IsRegistered(string login, string email, CancellationToken cancellationToken)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion
    }
}