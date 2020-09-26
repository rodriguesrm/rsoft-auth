using Microsoft.EntityFrameworkCore.Scaffolding;
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
        /// <param name="provider">DIP Service provider</param>
        public CredentialAppService(IServiceProvider provider)
        {
            _userDomain = provider.GetService<IUserDomainService>();
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Sends email to create credentials (new/reset)
        /// </summary>
        /// <param name="args">Call arguments</param>
        private async Task<SimpleOperationResult> SendMailTokenPasswordAsync(SendMailArgs args, CancellationToken cancellationToken = default)
        {
            //TODO: SEND MAIL REFACTOR

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
            //IDictionary<string, string> erros = new Dictionary<string, string>();

            //bool sucesso = (response.StatusCode == HttpStatusCode.Accepted);
            //if (!sucesso)
            //{
            //    string msgErro = string.Empty;
            //    string body = await response.Body.ReadAsStringAsync();
            //    if (!string.IsNullOrWhiteSpace(body))
            //    {
            //        SendGridResult sendGridResult = JsonSerializer.Deserialize<SendGridResult>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            //        msgErro = string.Join('|', sendGridResult.Errors.Select(x => x.Message));
            //    }
            //    else
            //    {
            //        msgErro = response.StatusCode.ToString();
            //    }
            //    erros.Add("Email", $"Falha no envio do e-mail => {msgErro}");
            //}

            //return new SimpleOperationResult(sucesso, erros);
            return await Task.Run(() =>
            {
                Console.WriteLine($"TOKEN: {args.Token.ToString()}");
                return new SimpleOperationResult(true, null);
            });
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<AuthenticateResult<UserDto>> AuthenticateAsync(Guid scopeId, Guid scopeKey, string login, string password, CancellationToken cancellationToken = default)
        {

            bool success = false;
            UserDto userDto = null;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            User user = await _userDomain.GetByLoginAsync(scopeId, scopeKey, login, password, cancellationToken);
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
            => await _userDomain.StartNewCredentialProcessAsync(login, true, (args) => SendMailTokenPasswordAsync(args, cancellationToken).Result, cancellationToken);

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