using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Language;
using RSoft.Auth.Cross.Common.Model.Args;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Common.Options;
using RSoft.Lib.Common.Web.Models.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Lib.Contracts.Events;
using Microsoft.Extensions.Logging;
using RSoft.Auth.Cross.Common.Model;

namespace RSoft.Auth.Application.Services
{


    /// <summary>
    /// User request access application service object
    /// </summary>
    public class UserRequestAccessAppService : IUserRequestAccessAppService
    {

        #region Local objects/variables

        private readonly RSApiOptions _apiOptions;
        private readonly PagesOptions _pagesOptions;
        private readonly AppClientOptions _appClientOptions;
        private readonly ILogger<UserRequestAccessAppService> _logger;
        private readonly IAppLanguageLocalizer _localizer;

        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        private readonly string _cultureName;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new application service instance
        /// </summary>
        public UserRequestAccessAppService
        (
            IOptions<RSApiOptions> apiOptions, 
            IOptions<PagesOptions> pagesOptions, 
            IOptions<AppClientOptions> appClientOptions,
            IOptions<CultureOptions> optionsCulture,
            ILogger<UserRequestAccessAppService> logger,
            IAppLanguageLocalizer localizer
        )
        {
            _apiOptions = apiOptions?.Value;
            _pagesOptions = pagesOptions?.Value;
            _appClientOptions = appClientOptions?.Value;
            _cultureName = optionsCulture?.Value?.DefaultLanguage ?? "en-US";
            _logger = logger;
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        public async Task<string> GetToken(CancellationToken cancellationToken = default)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(_apiOptions.Auth.Uri)
            };

            client.DefaultRequestHeaders.Add("User-Agent", "RSoft.Auth");
            client.DefaultRequestHeaders.Add("Accepted-Language", CultureInfo.CurrentCulture.Name);

            FormUrlEncodedContent formData = new
            (
                new[]
                {
                    new KeyValuePair<string, string>("app-key", $"{_appClientOptions.ClientId}"),
                    new KeyValuePair<string, string>("app-access", $"{_appClientOptions.ClientSecret}")
                }
            );

            HttpResponseMessage response = await client.PostAsync(_apiOptions.Auth.Path, formData, cancellationToken);
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                TokenApplication token = JsonSerializer.Deserialize<TokenApplication>(body, _jsonOptions);
                return token.Token;
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        _logger?.LogWarning($"Fail on autheticate application => {body}");
                        break;
                    default:
                        _logger?.LogWarning($"Fail on autheticate application => {response.StatusCode} - {body}");
                        break;
                }
            }
            return "AUTH_FAIL";

        }

        /// <summary>
        /// Sends email to create credentials (new/reset)
        /// </summary>
        /// <param name="cmd">Command call arguments</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<SimpleOperationResult> SendMailTokenPasswordAsync(UserRequestAccessArgs cmd, CancellationToken cancellationToken = default)
        {

            HttpClient client = new()
            {
                BaseAddress = new Uri(_apiOptions.Mail.Uri)
            };

            string appToken = await GetToken(cancellationToken);

            client.DefaultRequestHeaders.Add("User-Agent", "RSoft.Auth");
            client.DefaultRequestHeaders.Add("Authorization", $"bearer {appToken}");
            client.DefaultRequestHeaders.Add("Accepted-Language", CultureInfo.CurrentCulture.Name);

            RsMailRequest request = new()
            {
                From = new EmailAddressRequest("noreply@rsoft.com", "RSoft.Auth"),
                Subject = cmd.FirstAccess ? _localizer["CREDENTIAL_FIRST_ACCESS_SUBJECT"] : _localizer["CREDENTIAL_RECOVERY_ACCESS_SUBJECT"],
                Content = GetEmailBody(cmd.Name, "RSoft System", cmd.Token, cmd.ExpireOn, cmd.FirstAccess, cmd.UrlCredential),
                EnableHtml = true
            };
            request.To.Add(new EmailAddressRequest(cmd.Email, cmd.Name));

            StringContent content = new(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json");
            IDictionary<string, string> errors = new Dictionary<string, string>();

            HttpResponseMessage response = await client.PostAsync(_apiOptions.Mail.Path, content, cancellationToken);
            bool success = response.IsSuccessStatusCode;
            if (!success)
            {
                string body = await response.Content.ReadAsStringAsync(cancellationToken);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    IEnumerable<GenericNotification> notifications = JsonSerializer.Deserialize<IEnumerable<GenericNotification>>(body, _jsonOptions);
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

        /// <summary>
        /// Get e-mail body with action data and links
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="serviceName">Client service name</param>
        /// <param name="token">Recovery token</param>
        /// <param name="tokenDeadLine">Token dead limte date/time</param>
        /// <param name="firstAccess">Is first access flag</param>
        /// <param name="urlCredential">Url to create/recovery credential pass by header parameter</param>
        private string GetEmailBody(string userName, string serviceName, Guid token, DateTime tokenDeadLine, bool firstAccess, string urlCredential)
        {

            string file = Path.Combine(AppContext.BaseDirectory, "assets", "credential-template.html");
            string templateContent = File.OpenText(file).ReadToEnd();

            string urlBase = string.IsNullOrWhiteSpace(urlCredential) ? new Uri(_pagesOptions.InputPassword).AbsoluteUri : urlCredential;

            string url = $"{urlBase}?type={(firstAccess ? "create" : "recovery")}&token={token}";

            string credentialAction = firstAccess ? _localizer["CREDENTIAL_CREATE"] : _localizer["CREDENTIAL_RECOVERY"];
            templateContent = templateContent.Replace("{CREDENTIAL_ACTION}", credentialAction);
            templateContent = templateContent.Replace("{CREDENTIAL_MAIL_BODY_OPEN_TEXT}", _localizer["CREDENTIAL_MAIL_BODY_OPEN_TEXT"]);
            templateContent = templateContent.Replace("{SERVICE_NAME}", serviceName);
            templateContent = templateContent.Replace("{USERNAME}", userName);
            templateContent = templateContent.Replace("{ACTION}", firstAccess ? _localizer["CREDENTIAL_ACTION_CREATE"] : _localizer["CREDENTIAL_ACTION_RECOVERY"]);
            templateContent = templateContent.Replace("{ACTION_PASSWORD}", firstAccess ? _localizer["CREDENTIAL_ACTION_CREATE_PASSWORD"] : _localizer["CREDENTIAL_ACTION_RECOVERY_PASSWORD"]);
            templateContent = templateContent.Replace("{CREDENTIAL_OR_ELSE_LINK}", _localizer["CREDENTIAL_OR_ELSE_LINK"]);
            templateContent = templateContent.Replace("{URL_CLIENT}", url);
            templateContent = templateContent.Replace("{BUTTON_LABEL}", firstAccess ? _localizer["CREDENTIAL_BUTTONL_LABEL_CREATE"] : _localizer["CREDENTIAL_BUTTONL_LABEL_RECOVERY"]);
            templateContent = templateContent.Replace("{CREDENTIAL_TOKEN_DEADLINE}", _localizer["CREDENTIAL_TOKEN_DEADLINE"]);

            //TODO: Need future manage DateTimeOffset
            templateContent = templateContent.Replace("{TOKEN_DEADLINE}", tokenDeadLine.ToLocalTime().ToString(new CultureInfo(_cultureName)));

            templateContent = templateContent.Replace("{CREDENTIAL_DISCARD_MESSAGE}", _localizer["CREDENTIAL_DISCARD_MESSAGE"]);

            return templateContent;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> SendMail(UserRequestAccessArgs args, CancellationToken cancellationToken = default)
        {
            try
            {
                SimpleOperationResult result = await SendMailTokenPasswordAsync(args, cancellationToken);
                if (!result.Success)
                    throw new Exception(result.ErrorsMessage);
                return new SimpleOperationResult(true, null);
            }
            catch
            {
                _logger?.LogInformation($"Process {nameof(UserRequestAccessEvent)} FAIL", args.MessageId);
                throw;
            }

        }

        #endregion

    }
}
