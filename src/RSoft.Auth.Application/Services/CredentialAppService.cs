using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Cross.Common.Model.Args;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Services;
using RSoft.Framework.Application.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Framework.Cross.Enums;
using RSoft.Framework.Domain.ValueObjects;
using System.Net.Http;
using RSoft.Auth.Cross.Common.Options;
using Microsoft.Extensions.Options;
using System.Net;
using RSoft.Framework.Web.Model.Response;
using System.Text.Json;
using System.Linq;
using RSoft.Framework.Cross.Model.Request;
using System.Text;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Language;

namespace RSoft.Auth.Application.Services
{

    public class CredentialAppService : ICredentialAppService
    {

        #region Local objects/variables

        private readonly IUserDomainService _userDomain;
        private readonly RSApiOptions _apiOptions;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IStringLocalizer<AppResource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new CredentialAppService instance
        /// </summary>
        /// <param name="provider">DIP Service provider</param>
        /// <param name="options">RSoft Api options parameters object</param>
        /// <param name="localizer">Language localizer string</param>
        public CredentialAppService
        (
            IServiceProvider provider, 
            IOptions<RSApiOptions> options,
            IStringLocalizer<AppResource> localizer
        )
        {
            _userDomain = provider.GetService<IUserDomainService>();
            _apiOptions = options?.Value;
            _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Sends email to create credentials (new/reset)
        /// </summary>
        /// <param name="args">Call arguments</param>
        /// <param name="appToken">Application token to access mail-service api</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<SimpleOperationResult> SendMailTokenPasswordAsync(SendMailArgs args, string appToken, CancellationToken cancellationToken = default)
        {
            //TODO: Create libraty to consuming rsoft apis

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_apiOptions.Uri)
            };
                
            client.DefaultRequestHeaders.Add("User-Agent", "RSoft.Auth");
            client.DefaultRequestHeaders.Add("Authorization", $"bearer {appToken}");
            //TODO: Add Accepted-Language

            //BACKLOG: Add parameters to define sender data and redirect to or body content
            RsMailRequest request = new RsMailRequest()
            {
                From = new EmailAddressRequest("noreply@rsoft.com", "RSoft.Auth"),
                Subject = $"{(args.FirstAccess ? "First" : "Recovery")} access",
                Content = $"TOKEN TO RECOVERY: {args.Token}",
                EnableHtml = true
            };
            request.To.Add(new EmailAddressRequest(args.Email, args.Name));

            StringContent content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json");
            IDictionary<string, string> errors = new Dictionary<string, string>();
            Guid requestId = Guid.Empty;

            HttpResponseMessage response = await client.PostAsync(_apiOptions.MailService, content);
            bool success = response.IsSuccessStatusCode;
            if (!success)
            {
                string body = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    IEnumerable<GenericNotificationResponse> notifications = JsonSerializer.Deserialize<IEnumerable<GenericNotificationResponse>>(body, _jsonOptions);
                    errors = notifications.ToDictionary(k => k.Property, v => v.Message);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    errors.Add("API SendMail", $"API SendMail | Unauthorized - {body}");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    errors.Add("API SendMail", $"API SendMail | Not Found");
                }
                else
                {
                    errors.Add("API SendMail", $"API SendMail | {response.StatusCode} - {body}");
                }
            }

            return new SimpleOperationResult(success, errors);
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<AuthenticateResult<UserDto>> AuthenticateAsync(Guid appKey, Guid appAccess, string login, string password, CancellationToken cancellationToken = default)
        {

            bool success = false;
            UserDto userDto = null;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            (User, Guid?) resultLogin = await _userDomain.GetByLoginAsync(appKey, appAccess, login, password, cancellationToken);
            User user = resultLogin.Item1;
            Guid? userId = resultLogin.Item2;
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
        public async Task<PasswordProcessResult> GetFirstAccessAsync(string email, string appToken, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result = null;
            Email checkedEmail = new Email(email);
            if (string.IsNullOrWhiteSpace(email) || checkedEmail.Invalid)
            {
                IDictionary<string, string> errors = new Dictionary<string, string>
                {
                    { "Email", _localizer["EMAIL_INVALID_OR_EMPTY"] }
                };
                result = new PasswordProcessResult(false, null, null, errors, null);
            }
            else
            {
                result = await _userDomain.GetFirstAccessAsync(email, (args) => SendMailTokenPasswordAsync(args, appToken, cancellationToken).Result, cancellationToken);
            }
            return result;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken = default)
            => await _userDomain.CreateFirstAccessAsync(tokenId, login, password, cancellationToken);

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetResetAccessAsync(string loginOrEmail, string appToken, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result = null;
            if (string.IsNullOrWhiteSpace(loginOrEmail))
            {
                IDictionary<string, string> errors = new Dictionary<string, string>
                {
                    { "Login", _localizer["LOGIN_REQUIRED"] }
                };
                result = new PasswordProcessResult(false, null, null, errors, null);
            }
            else
            {
                result = await _userDomain.GetResetAccessAsync(loginOrEmail, (args) => SendMailTokenPasswordAsync(args, appToken, cancellationToken).Result, cancellationToken);
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