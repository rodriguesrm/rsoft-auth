﻿using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Cross.Common.Options;
using Microsoft.Extensions.Options;
using RSoft.Helpers.Security;
using System.Threading.Tasks;
using System.Threading;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Cross.Common.Model.Args;
using RSoft.Framework.Application.Model;
using System.Collections.Generic;
using RSoft.Framework.Infra.Data;
using System.Linq;
using RSoft.Framework.Cross;
using RSoft.Framework.Domain.ValueObjects;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// User domain service operations
    /// </summary>
    public class UserDomainService : DomainServiceBase<User, Guid, IUserRepository>, IUserDomainService
    {

        #region Local objects/variables

        private readonly SecurityOptions _securityOptions;
        private readonly IUnitOfWork _uow;
        private readonly IUserCredentialTokenRepository _tokenRepository;
        private readonly IScopeRepository _scopeRepository;
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        /// <param name="uow">Unit of Work object</param>
        /// <param name="repository">User repository service</param>
        /// <param name="tokenRepository">User credential token repository service</param>
        /// <param name="scopeRepository">Scope repository</param>
        /// <param name="roleRepository">Role repository</param>
        /// <param name="securityOptions">Security options configuration</param>
        public UserDomainService
        (
            IAuthenticatedUser authenticatedUser,
            IUnitOfWork uow, 
            IUserRepository repository, 
            IUserCredentialTokenRepository tokenRepository, 
            IScopeRepository scopeRepository,
            IRoleRepository roleRepository,
            IOptions<SecurityOptions> securityOptions
        ) : base(repository, authenticatedUser)
        {
            _uow = uow;
            _tokenRepository = tokenRepository;
            _scopeRepository = scopeRepository;
            _roleRepository = roleRepository;
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
            MD5.HashMD5($"{password}|{_securityOptions.Secret}", out string pwdMD5);
            return pwdMD5.ToLower();
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override void PrepareSave(User entity, bool isUpdate)
        {
            if (isUpdate)
                entity.ChangedAuthor = new AuthorNullable<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
            else
                entity.CreatedAuthor = new Author<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<User> GetByLoginAsync(Guid scopeId, Guid scopeKey, string login, string password, CancellationToken cancellationToken = default)
        {
            
            //BACKLOG: Add LDAP Authenticate

            if (cancellationToken.IsCancellationRequested) return null;
            User user = await _repository.GetByLoginAsync(login, cancellationToken);
            if (user != null)
            {
                if (user.Credential.Password != ConvertPassword(password))
                    user = null;

                Scope scopeCheck = user.Scopes.FirstOrDefault(x => x.Id == scopeId && x.AccessKey == scopeKey);
                if (scopeCheck == null)
                    user = null;
                else
                    user.Roles = GetRolesByUserAsync(scopeId, user.Id);

             }
            return user;
        }

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> StartNewCredentialProcessAsync(string login, bool firstAccess, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default)
        {

            bool success = false;
            Guid? token = null;
            DateTime? expiresOn = null;
            IDictionary<string, string> errors = new Dictionary<string, string>();
            Exception exception = null;

            if (cancellationToken.IsCancellationRequested)
            {
                exception = new Exception("Operation was canceled");
            }
            else
            {

                User user = await _repository.GetByLoginAsync(login, cancellationToken);
                if (user == null)
                {
                    errors.Add("Credential", "Credentials not found");
                }
                else
                {

                    if (user.Credential == null || !firstAccess)
                    {

                        //TODO: Create options-configuration for 'ExpiresOn'
                        UserCredentialToken userCredentialToken = new UserCredentialToken()
                        {
                            UserId = user.Id,
                            FirstAccess = firstAccess,
                            ExpiresOn = DateTime.UtcNow.AddMinutes(30)
                        };

                        if (userCredentialToken.Valid)
                        {

                            token = userCredentialToken.Id;
                            expiresOn = userCredentialToken.ExpiresOn;

                            await _uow.BeginTransactionAsync(cancellationToken);

                            await _tokenRepository.AddAsync(userCredentialToken, cancellationToken);
                            await _uow.SaveChangesAsync(cancellationToken);

                            cancellationToken.ThrowIfCancellationRequested();

                            SimpleOperationResult emailResult = sendMailCallBack(new SendMailArgs()
                            {
                                FirstAccess = firstAccess,
                                Name = user.Name.GetFullName(),
                                Email = user.Email.Address,
                                Token = userCredentialToken.Id
                            });

                            cancellationToken.ThrowIfCancellationRequested();

                            if (emailResult.Success)
                            {
                                await _uow.CommitAsync(default);
                                success = true;
                            }
                            else
                            {
                                await _uow.RollBackAsync(default);
                                errors = emailResult.Errors;
                                exception = new Exception(emailResult.ErrorsMessage);
                            }

                        }
                        else
                        {
                            userCredentialToken.Notifications
                                .ToList()
                                .ForEach(nt => errors.Add(nt.Property, nt.Message));
                        }

                    }
                    else
                    {
                        errors.Add("Credentials", (firstAccess ? "Credentials already exist" : "Credentials not found"));
                    }

                }

            }

            return new PasswordProcessResult(success, token, expiresOn, errors, exception);

        }

        ///<inheritdoc/>
        public ICollection<Role> GetRolesByUserAsync(Guid scopeId, Guid userId)
        {
            ICollection<Role> result = _roleRepository.GetRolesByUser(scopeId, userId);
            return result;
        }

        #endregion

    }

}
