using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Auth.Cross.Common.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Language;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Common.ValueObjects;
using RSoft.Lib.Common.Enums;
using MassTransit;
using RSoft.Lib.Contracts.Events;

namespace RSoft.Auth.Application.Services
{

    public class CredentialAppService : ICredentialAppService
    {

        #region Local objects/variables

        private readonly IUserDomainService _userDomain;
        private readonly IBusControl _bus;
        private readonly RSApiOptions _apiOptions;
        private readonly PagesOptions _pagesOptions;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IStringLocalizer<AppResource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new CredentialAppService instance
        /// </summary>
        /// <param name="provider">DIP Service provider</param>
        /// <param name="bus">Message bus control</param>
        /// <param name="rsApiOptions">RSoft Api options parameters object</param>
        /// <param name="localizer">Language localizer string</param>
        public CredentialAppService
        (
            IServiceProvider provider,
            IBusControl bus,
            IOptions<RSApiOptions> rsApiOptions,
            IOptions<PagesOptions> pagesOptions,
            IStringLocalizer<AppResource> localizer
        )
        {
            _userDomain = provider.GetService<IUserDomainService>();
            _bus = bus;
            _apiOptions = rsApiOptions?.Value;
            _pagesOptions = pagesOptions?.Value;
            _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Public request access message
        /// </summary>
        /// <param name="isFirstAccess">Indicates whether this is first access</param>
        /// <param name="urlCredential">Url of the page to be informed in the credential creation email
        /// <param name="result">Password process result object</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task PublishMessage(bool isFirstAccess, string urlCredential, PasswordProcessResult result, CancellationToken cancellationToken)
        {
            string urlBase = string.IsNullOrWhiteSpace(urlCredential) ? new Uri(_pagesOptions.InputPassword).AbsoluteUri : urlCredential;
            UserRequestAccessEvent message = new(isFirstAccess, result.FullName, result.Email, result.Token.Value, result.ExpirationDate.Value, urlBase);
            await _bus.Publish(message, cancellationToken);
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<AuthenticateResult<UserDto>> AuthenticateAsync(Guid appKey, string login, string password, CancellationToken cancellationToken = default)
        {

            bool success = false;
            UserDto userDto = null;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            (User user, Guid? userId) = await _userDomain.GetByLoginAsync(appKey, login, password, cancellationToken);
            if (user != null)
            {

                if (user.Type == UserType.User)
                {

                    if (user.Credential.ChangeCredentials)
                    {
                        errors.Add("Authenticate", _localizer["USER_MUST_CHANGE_PASSWORD"]);
                    }
                    else
                    {
                        if (user.IsActive)
                        {

                            if (user.Credential.LockoutUntil.HasValue && user.Credential.LockoutUntil.Value > DateTime.UtcNow)
                            {
                                errors.Add("Authenticate", _localizer["USER_LOCKOUT"]);
                            }
                            else
                            {
                                success = true;
                                userDto = user.Map();
                                await _userDomain.ClearLockout(user.Id, default);
                            }
                        }
                        else
                        {
                            errors.Add("Authenticate", _localizer["USER_INACTIVE"]);
                        }
                    }

                }
                else
                {
                    errors.Add("Authenticate", _localizer["SERVICE_NOT_AS_USER"]);
                }
            }
            else
            {
                if (userId.HasValue)
                    await _userDomain.MarkLoginFail(userId.Value, cancellationToken);
                errors.Add("Authenticate", _localizer["USER_PASSWORD_FAIL"]);
            }

            return new AuthenticateResult<UserDto>(success, userDto, errors);
        }

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetFirstAccessAsync(string email, string appToken, string urlCredential, CancellationToken cancellationToken = default)
        {
            Email checkedEmail = new(email);

            PasswordProcessResult result;
            if (string.IsNullOrWhiteSpace(email) || checkedEmail.Invalid)
            {
                IDictionary<string, string> errors = new Dictionary<string, string>
                {
                    { "Email", _localizer["EMAIL_INVALID_OR_EMPTY"] }
                };
                result = new(errors);
            }
            else
            {

                result = await _userDomain.GetFirstAccessAsync(email, urlCredential, cancellationToken);
                if (result.Success)
                    await PublishMessage(true, urlCredential, result, cancellationToken);
            }
            return result;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken = default)
            => await _userDomain.CreateFirstAccessAsync(tokenId, login, password, cancellationToken);

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetResetAccessAsync(string loginOrEmail, string appToken, string urlCredential, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result;
            if (string.IsNullOrWhiteSpace(loginOrEmail))
            {
                IDictionary<string, string> errors = new Dictionary<string, string>
                {
                    { "Login", _localizer["LOGIN_REQUIRED"] }
                };
                result = new(errors);
            }
            else
            {
                result = await _userDomain.GetResetAccessAsync(loginOrEmail, urlCredential, cancellationToken);
                if (result.Success)
                    await PublishMessage(false, urlCredential, result, cancellationToken);
            }

            return result;
        }


        ///<inheritdoc/>
        public async Task<SimpleOperationResult> SetRecoveryAccessAsync(Guid tokenId, string password, CancellationToken cancellationToken = default)
            => await _userDomain.SetRecoveryAccessAsync(tokenId, password, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> ChangePasswordAsync(string login, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
            => await _userDomain.ChangePasswordAsync(login, currentPassword, newPassword, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> LoginIsAvailableAsync(string login, Guid? userId, CancellationToken cancellationToken)
        {
            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(login))
            {
                errors.Add("Login", _localizer["LOGIN_REQUIRED"]);
            }
            else
            {
                success = await _userDomain.LoginIsAvailableAsync(userId ?? Guid.Empty, login, cancellationToken);
            }

            return new SimpleOperationResult(success, errors);
        }

        #endregion
    }
}