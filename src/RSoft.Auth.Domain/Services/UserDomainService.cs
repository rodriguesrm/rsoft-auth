using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Cross.Common.Options;
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
using Microsoft.Extensions.Configuration;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// User domain service operations
    /// </summary>
    public class UserDomainService : DomainServiceBase<User, Guid, IUserRepository>, IUserDomainService
    {

        #region Local objects/variables

        private readonly SecurityOptions _securityOptions;
        private readonly int _jwtTimeLife;
        private readonly IUnitOfWork _uow;
        private new readonly IUserRepository _repository;
        private readonly IUserCredentialRepository _credentialRepository;
        private readonly IUserCredentialTokenRepository _tokenRepository;
        private readonly IScopeRepository _scopeRepository;
        private readonly IRoleRepository _roleRepository;
        
        private const int MINUTES_TIME_LIFE = 30;

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
            IUserCredentialRepository credentialRepository,
            IUserCredentialTokenRepository tokenRepository,
            IScopeRepository scopeRepository,
            IRoleRepository roleRepository,
            IConfiguration configuration
        ) : base(repository, authenticatedUser)
        {
            _uow = uow;
            _repository = repository;
            _credentialRepository = credentialRepository;
            _tokenRepository = tokenRepository;
            _scopeRepository = scopeRepository;
            _roleRepository = roleRepository;

            _securityOptions = new SecurityOptions();
            configuration.GetSection("Security").Bind(_securityOptions);
            if (!int.TryParse(configuration["Jwt:TimeLife"], out _jwtTimeLife))
                _jwtTimeLife = MINUTES_TIME_LIFE;
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

        /// <summary>
        /// Create or reset user credentials and password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="firstAccess"></param>
        /// <param name="sendMailCallBack"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<SimpleOperationResult> SaveCredentialAsync(Guid tokenId, string login, string password, bool firstAccess, CancellationToken cancellationToken)
        {

            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (cancellationToken.IsCancellationRequested)
            {
                errors.Add("CreateCredential", "Operation was canceled");
            }
            else
            {

                string convertedPassword = ConvertPassword(password);

                if (string.IsNullOrWhiteSpace(convertedPassword))
                {
                    errors.Add("Password", "Password is required");
                }
                else
                {

                    UserCredentialToken credentialToken = await _tokenRepository.GetByKeyAsync(tokenId, cancellationToken);
                    if (credentialToken == null)
                    {
                        errors.Add("Token", "Token is invalid");
                    }
                    else
                    {

                        if (credentialToken.Expired())
                        {
                            errors.Add("Token", "Expired token");
                        }
                        else
                        {

                            if (credentialToken.FirstAccess != firstAccess)
                            {
                                errors.Add("Token", "Invalid token for this operation");
                                _tokenRepository.Delete(credentialToken.Id);
                                await _uow.SaveChangesAsync(cancellationToken);
                            }
                            else
                            {

                                User user = await _repository.GetByKeyAsync(credentialToken.User.Id, cancellationToken);
                                if (user.Credential != null && firstAccess)
                                {
                                    errors.Add("Credential", "Credentials already exist");
                                    _tokenRepository.Delete(credentialToken.Id);
                                    await _uow.SaveChangesAsync(cancellationToken);
                                }
                                else
                                {

                                    if (firstAccess && (!(await LoginIsAvailableAsync(user.Id, login, cancellationToken))))
                                    {
                                        errors.Add("Login", "Login already in use");
                                    }
                                    else
                                    {

                                        await _uow.BeginTransactionAsync(cancellationToken);

                                        if (firstAccess)
                                        {
                                            UserCredential credential = new UserCredential()
                                            {
                                                UserId = user.Id,
                                                Login = login,
                                                Password = convertedPassword
                                            };
                                            await _credentialRepository.AddAsync(credential, cancellationToken);
                                        }
                                        else
                                        {
                                            user.Credential.Password = convertedPassword;
                                            user.Credential.ChangeCredentials = false;
                                            _credentialRepository.Update(user.Id, user.Credential);
                                        }
                                        _tokenRepository.Delete(credentialToken.Id);
                                        await _uow.SaveChangesAsync(cancellationToken);

                                        if (cancellationToken.IsCancellationRequested)
                                        {
                                            await _uow.RollBackAsync(default);
                                            errors.Add("CreateCredential", "Operation was canceled");
                                        }
                                        else
                                        {
                                            await _uow.CommitAsync(cancellationToken);
                                            success = true;
                                        }

                                    }

                                }

                            }

                        }

                    }

                }

            }

            return new SimpleOperationResult(success, errors);
        }

        /// <summary>
        /// Starts the process of obtaining credentials for first access or retrieving credentials.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="firstAccess"></param>
        /// <param name="sendMailCallBack"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<PasswordProcessResult> RequestNewCredentials(string login, bool firstAccess, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default)
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

                        Guid tokenKey = Guid.NewGuid();
                        UserCredentialToken userCredentialToken = new UserCredentialToken(tokenKey)
                        {
                            UserId = user.Id,
                            FirstAccess = firstAccess,
                            ExpiresOn = DateTime.UtcNow.AddMinutes(_jwtTimeLife)
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

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override void PrepareSave(User entity, bool isUpdate)
        {
            if (isUpdate)
            {
                entity.ChangedAuthor = new AuthorNullable<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
                entity.ChangedOn = DateTime.UtcNow;
            }
            else
            {
                entity.CreatedAuthor = new Author<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
                entity.CreatedOn = DateTime.UtcNow;
            }
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<bool> LoginIsAvailableAsync(Guid userId, string login, CancellationToken cancellationToken = default)
        {

            IEnumerable<User> users = await _repository.ListByLoginAsync(login, cancellationToken);
            bool found = false;

            foreach (var user in users)
            {
                if ((user.Email.Address == login || user.Credential.Login == login) && user.Id != userId)
                {
                    found = true;
                    break;
                }
            }

            return !found;

        }

        ///<inheritdoc/>
        public async Task<User> GetByLoginAsync(Guid appKey, Guid appAccess, string login, string password, CancellationToken cancellationToken = default)
        {

            //BACKLOG: Add LDAP Authenticate

            if (cancellationToken.IsCancellationRequested) return null;
            User user = await _repository.GetByLoginAsync(login, cancellationToken);
            if (user != null)
            {
                if (user.Credential.Password != ConvertPassword(password))
                {
                    user = null;
                }
                else
                {
                    Scope scopeCheck = user.Scopes.FirstOrDefault(x => x.Id == appKey && x.AccessKey == appAccess);
                    if (scopeCheck == null)
                        user = null;
                    else
                        user.Roles = GetRolesByUserAsync(appKey, user.Id);
                }

            }
            return user;
        }

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetFirstAccessAsync(string email, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default)
            => await RequestNewCredentials(email, true, sendMailCallBack, cancellationToken);

        ///<inheritdoc/>
        public ICollection<Role> GetRolesByUserAsync(Guid scopeId, Guid userId)
        {
            ICollection<Role> result = _roleRepository.GetRolesByUser(scopeId, userId);
            return result;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> CreateFirstAccessAsync(Guid tokenId, string login, string password, CancellationToken cancellationToken)
            => await SaveCredentialAsync(tokenId, login, password, true, cancellationToken);

        ///<inheritdoc/>
        public async Task<PasswordProcessResult> GetResetAccessAsync(string email, Func<SendMailArgs, SimpleOperationResult> sendMailCallBack, CancellationToken cancellationToken = default)
            => await RequestNewCredentials(email, false, sendMailCallBack, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> SetRecoveryAccessAsync(Guid tokenId, string password, CancellationToken cancellationToken = default)
            => await SaveCredentialAsync(tokenId, null, password, false, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> ChangePasswordAsync(string login, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {

            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();
            string hashCurrentPassword = ConvertPassword(currentPassword);
            string hashNewPassword = ConvertPassword(newPassword);

            User user = await _repository.GetByLoginAsync(login, cancellationToken);
            if (user != null && user.Credential.Password == hashCurrentPassword)
            {
                if (_authenticatedUser.Login != user.Credential.Login)
                    errors.Add("ChangePassword", "Informed user is different from authenticated");
                else
                {
                    if (user.IsActive)
                    {
                        if (currentPassword == newPassword)
                        {
                            errors.Add("ChangePassword", "The new password cannot be the same as the current password");
                        }
                        else
                        {
                            user.Credential.Password = ConvertPassword(hashNewPassword);
                            user.Credential.ChangeCredentials = false;
                            _credentialRepository.Update(user.Credential.UserId.Value, user.Credential);
                            await _uow.SaveChangesAsync(cancellationToken);
                            success = true;
                        }
                    }
                    else
                    {
                        errors.Add("ChangePassword", "User is inactive or blocked");
                    }
                }
            }
            else
            {
                errors.Add("ChangePassword", "Invalid username or password");
            }
            return new SimpleOperationResult(success, errors);

        }

        ///<inheritdoc/>
        public async Task<IEnumerable<User>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default)
            => await _repository.GetAllAsync(scopeId, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
        {

            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            
            Scope scope = await _scopeRepository.GetByKeyAsync(scopeId, cancellationToken);
            if (scope != null)
            {

                User user = await _repository.GetByKeyAsync(userId, cancellationToken);
                if (user != null)
                {
                    
                    if (!user.Scopes.Any(s => s.Id == scopeId))
                    {
                        await _repository.AddUserScopeAsync(userId, scopeId, cancellationToken);
                        await _uow.SaveChangesAsync(cancellationToken);
                        success = true;
                    }
                    else
                        errors.Add("UserScope", "User is already authorized for this scope");
                }
                else
                    errors.Add("User", "User not found");

            }
            else
                errors.Add("Scope", "Scope not found");

            return new SimpleOperationResult(success, errors);

        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> RemoveScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
        {

            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            User user = await _repository.GetByKeyAsync(userId, cancellationToken);
            if (user != null)
            {

                if (user.Scopes.Any(s => s.Id == scopeId))
                {
                    await _repository.RemoveUserScopeAsync(userId, scopeId, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);
                    success = true;
                }
                else
                    errors.Add("UserScope", "User does not have access to this scope or scope not exists");
            }
            else
                errors.Add("User", "User not found");

            return new SimpleOperationResult(success, errors);

        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddRoleAsync(Guid userId, IEnumerable<Role> roles, CancellationToken cancellationToken)
        {

            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            User user = await _repository.GetByKeyAsync(userId, cancellationToken);
            if (user != null)
            {
                IList<Guid> ids = new List<Guid>();
                foreach (Role role in roles)
                {
                    if (user.Roles.FirstOrDefault(x => x.Id == role.Id) == null)
                        ids.Add(role.Id);
                    else
                        errors.Add(role.Name, $"The user already has this role");
                }
                if (errors.Count == 0)
                {
                    await _repository.AddUserRoleAsync(userId, ids, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);
                    success = true;
                }
            }
            else
                errors.Add("User", "User not found");

            return new SimpleOperationResult(success, errors);

        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            User user = await _repository.GetByKeyAsync(userId, cancellationToken);
            if (user != null)
            {

                if (user.Roles.Any(s => s.Id == roleId))
                {
                    await _repository.RemoveUserRoleAsync(userId, roleId, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);
                    success = true;
                }
                else
                    errors.Add("UserRole", "User does not have this role");
            }
            else
                errors.Add("User", "User not found");

            return new SimpleOperationResult(success, errors);
        }

        #endregion

    }

}
