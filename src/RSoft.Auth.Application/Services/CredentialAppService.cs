﻿using RSoft.Auth.Application.Model;
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

namespace RSoft.Auth.Application.Services
{

    public class CredentialAppService : ICredentialAppService
    {

        #region Local objects/variables

        private readonly IUserDomainService _userDomain;
        private readonly RSApiOptions _apiOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new CredentialAppService instance
        /// </summary>
        /// <param name="provider">DIP Service provider</param>
        public CredentialAppService(IServiceProvider provider, IOptions<RSApiOptions> options)
        {
            _userDomain = provider.GetService<IUserDomainService>();
            _apiOptions = options?.Value;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Sends email to create credentials (new/reset)
        /// </summary>
        /// <param name="args">Call arguments</param>
        private async Task<SimpleOperationResult> SendMailTokenPasswordAsync(SendMailArgs args, CancellationToken cancellationToken = default)
        {
            //TODO: Create libraty to consuming rsoft apis

            HttpClient client = new HttpClient();

            string json = @"request";
            var content = new StringContent(json);
            IDictionary<string, string> errors = new Dictionary<string, string>();
            Guid requestId = Guid.Empty;

            HttpResponseMessage response = await client.PostAsync(_apiOptions.MailService, content);
            bool success = response.IsSuccessStatusCode;
            if (success)
            {
                string body = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    Guid.TryParse(body, out requestId);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        IEnumerable<GenericNotificationResponse> notifications = JsonSerializer.Deserialize<IEnumerable<GenericNotificationResponse>>(body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    }
                }
            }

            //SendGridClient client = new SendGridClient(Security.SendGridAppKey);
            //SendGridMessage msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("naoresponder@feiraemcasa.com", "Feira em Casa"),
            //    Subject = args.PrimeiroAcesso ? "Primeiro Acesso" : "Reset de Senha",
            //    PlainTextContent = "Texto de envio e-mail via C#",
            //    HtmlContent = $"Para concluir a operação utilize token: <strong>{args.Token}</strong>"
            //};

            //msg.AddTo(new EmailAddress(args.Email, args.Nome));
            //Response response = await client.SendEmailAsync(msg, cancellationToken);


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
                        errors.Add("Authenticate", "User must change password");
                    }
                    else
                    {
                        if (user.IsActive)
                        {

                            if (user.Credential.LockoutUntil.HasValue && user.Credential.LockoutUntil.Value > DateTime.UtcNow)
                            {
                                errors.Add("Authenticate", "User is lockout");
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
                            errors.Add("Authenticate", "Inactive or blocked user");
                        }
                    }

                }
                else
                {
                    errors.Add("Authenticate", "Service user cannot authenticate as a regular user");
                }
            }
            else
            {
                if (userId.HasValue)
                    await _userDomain.MarkLoginFail(userId.Value, cancellationToken);
                errors.Add("Authenticate", "Invalid username and/or password!");
            }

            return new AuthenticateResult<UserDto>(success, userDto, errors);
        }

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetFirstAccessAsync(string email, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result = null;
            Email checkedEmail = new Email(email);
            if (string.IsNullOrWhiteSpace(email) || checkedEmail.Invalid)
            {
                IDictionary<string, string> errors = new Dictionary<string, string>
                {
                    { "Email", "E-mail is invalid or empty" }
                };
                result = new PasswordProcessResult(false, null, null, errors, null);
            }
            else
            {
                result = await _userDomain.GetFirstAccessAsync(email, (args) => SendMailTokenPasswordAsync(args, cancellationToken).Result, cancellationToken);
            }
            return result;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken = default)
            => await _userDomain.CreateFirstAccessAsync(tokenId, login, password, cancellationToken);

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetResetAccessAsync(string loginOrEmail, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result = null;
            if (string.IsNullOrWhiteSpace(loginOrEmail))
            {
                IDictionary<string, string> errors = new Dictionary<string, string>
                {
                    { "Login", "Login is required" }
                };
                result = new PasswordProcessResult(false, null, null, errors, null);
            }
            else
            {
                result = await _userDomain.GetResetAccessAsync(loginOrEmail, (args) => SendMailTokenPasswordAsync(args, cancellationToken).Result, cancellationToken);
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
                errors.Add("Login", "Login is required");
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