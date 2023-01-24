using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Cross.Common.Model.Args;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using RSoft.Auth.Cross.Common.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Language;
using System.Globalization;
using System.IO;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Common.Web.Models.Request;
using RSoft.Lib.Common.ValueObjects;
using RSoft.Lib.Common.Enums;

namespace RSoft.Auth.Application.Services
{

    public class CredentialAppService : ICredentialAppService
    {

        #region Local objects/variables

        private readonly IUserDomainService _userDomain;
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
        /// <param name="rsApiOptions">RSoft Api options parameters object</param>
        /// <param name="localizer">Language localizer string</param>
        public CredentialAppService
        (
            IServiceProvider provider, 
            IOptions<RSApiOptions> rsApiOptions,
            IOptions<PagesOptions> pagesOptions,
            IStringLocalizer<AppResource> localizer
        )
        {
            _userDomain = provider.GetService<IUserDomainService>();
            _apiOptions = rsApiOptions?.Value;
            _pagesOptions = pagesOptions?.Value;
            _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        ///// <summary>
        ///// Sends email to create credentials (new/reset)
        ///// </summary>
        ///// <param name="args">Call arguments</param>
        ///// <param name="appToken">Application token to access mail-service api</param>
        ///// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        //private async Task<SimpleOperationResult> SendMailTokenPasswordAsync(SendMailArgs args, string appToken, CancellationToken cancellationToken = default)
        //{
        //    //TODO: Create libraty to consuming rsoft apis

        //    HttpClient client = new()
        //    {
        //        BaseAddress = new Uri(_apiOptions.Uri)
        //    };
                
        //    client.DefaultRequestHeaders.Add("User-Agent", "RSoft.Auth");
        //    client.DefaultRequestHeaders.Add("Authorization", $"bearer {appToken}");
        //    client.DefaultRequestHeaders.Add("Accepted-Language", CultureInfo.CurrentCulture.Name);

        //    RsMailRequest request = new()
        //    {
        //        From = new EmailAddressRequest("noreply@rsoft.com", "RSoft.Auth"),
        //        Subject = args.FirstAccess ? _localizer["CREDENTIAL_FIRST_ACCESS_SUBJECT"] : _localizer["CREDENTIAL_RECOVERY_ACCESS_SUBJECT"],
        //        Content = GetEmailBody(args.Name, "RSoft System", args.Token, args.ExpireOn, args.FirstAccess, args.UrlCredential),
        //        EnableHtml = true
        //    };
        //    request.To.Add(new EmailAddressRequest(args.Email, args.Name));

        //    StringContent content = new(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json");
        //    IDictionary<string, string> errors = new Dictionary<string, string>();
        //    Guid requestId = Guid.Empty;

        //    HttpResponseMessage response = await client.PostAsync(_apiOptions.MailService, content, cancellationToken);
        //    bool success = response.IsSuccessStatusCode;
        //    if (!success)
        //    {
        //        string body = await response.Content.ReadAsStringAsync(cancellationToken);
        //        if (response.StatusCode == HttpStatusCode.BadRequest)
        //        {
        //            IEnumerable<GenericNotification> notifications = JsonSerializer.Deserialize<IEnumerable<GenericNotification>>(body, _jsonOptions);
        //            errors = notifications.ToDictionary(k => k.Property, v => v.Message);
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            errors.Add("API SendMail", $"API SendMail | Unauthorized - {body}");
        //        }
        //        else if (response.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            errors.Add("API SendMail", $"API SendMail | Not Found");
        //        }
        //        else
        //        {
        //            errors.Add("API SendMail", $"API SendMail | {response.StatusCode} - {body}");
        //        }
        //    }

        //    return new SimpleOperationResult(success, errors);
        //}

        ///// <summary>
        ///// Get e-mail body with action data and links
        ///// </summary>
        ///// <param name="userName">user name</param>
        ///// <param name="serviceName">Client service name</param>
        ///// <param name="token">Recovery token</param>
        ///// <param name="tokenDeadLine">Token dead limte date/time</param>
        ///// <param name="firstAccess">Is first access flag</param>
        ///// <param name="urlCredential">Url to create/recovery credential pass by header parameter</param>
        //private string GetEmailBody(string userName, string serviceName, Guid token, DateTime tokenDeadLine, bool firstAccess, string urlCredential)
        //{

        //    string file = Path.Combine(AppContext.BaseDirectory, "wwwroot", "assets", "credential-template.html");
        //    string templateContent = File.OpenText(file).ReadToEnd();

        //    string urlBase = string.IsNullOrWhiteSpace(urlCredential) ? new Uri(_pagesOptions.InputPassword).AbsoluteUri : urlCredential;

        //    string url = $"{urlBase}?type={(firstAccess ? "create" : "recovery")}&token={token}";

        //    string credentialAction = firstAccess ? _localizer["CREDENTIAL_CREATE"] : _localizer["CREDENTIAL_RECOVERY"];
        //    templateContent = templateContent.Replace("{CREDENTIAL_ACTION}", credentialAction);
        //    templateContent = templateContent.Replace("{CREDENTIAL_MAIL_BODY_OPEN_TEXT}", _localizer["CREDENTIAL_MAIL_BODY_OPEN_TEXT"]);
        //    templateContent = templateContent.Replace("{SERVICE_NAME}", serviceName);
        //    templateContent = templateContent.Replace("{USERNAME}", userName);
        //    templateContent = templateContent.Replace("{ACTION}", firstAccess ? _localizer["CREDENTIAL_ACTION_CREATE"] : _localizer["CREDENTIAL_ACTION_RECOVERY"]);
        //    templateContent = templateContent.Replace("{ACTION_PASSWORD}", firstAccess ? _localizer["CREDENTIAL_ACTION_CREATE_PASSWORD"] : _localizer["CREDENTIAL_ACTION_RECOVERY_PASSWORD"]);
        //    templateContent = templateContent.Replace("{CREDENTIAL_OR_ELSE_LINK}", _localizer["CREDENTIAL_OR_ELSE_LINK"]);
        //    templateContent = templateContent.Replace("{URL_CLIENT}", url);
        //    templateContent = templateContent.Replace("{BUTTON_LABEL}", firstAccess ? _localizer["CREDENTIAL_BUTTONL_LABEL_CREATE"] : _localizer["CREDENTIAL_BUTTONL_LABEL_RECOVERY"]);
        //    templateContent = templateContent.Replace("{CREDENTIAL_TOKEN_DEADLINE}", _localizer["CREDENTIAL_TOKEN_DEADLINE"]);
            
        //    //TODO: Need future manage DateTimeOffset
        //    templateContent = templateContent.Replace("{TOKEN_DEADLINE}", $"{tokenDeadLine.ToLocalTime().ToShortDateString()} {tokenDeadLine.ToLocalTime().ToShortTimeString()}");
            
        //    templateContent = templateContent.Replace("{CREDENTIAL_DISCARD_MESSAGE}", _localizer["CREDENTIAL_DISCARD_MESSAGE"]);

        //    return templateContent;
        //}

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
                result = new PasswordProcessResult(false, null, null, errors, null);
            }
            else
            {
                result = await _userDomain.GetFirstAccessAsync(email, urlCredential, cancellationToken);
                //TODO: Event Launch
            }
            return result;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken = default)
            => await _userDomain.CreateFirstAccessAsync(tokenId, login, password, cancellationToken);

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetResetAccessAsync(string loginOrEmail, string appToken, string urlCredential, CancellationToken cancellationToken = default)
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
                result = await _userDomain.GetResetAccessAsync(loginOrEmail, urlCredential, (args) => SendMailTokenPasswordAsync(args, appToken, cancellationToken).Result, cancellationToken);
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