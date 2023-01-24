using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Auth.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Language;
using RSoft.Lib.Design.Application.Services;
using RSoft.Lib.Design.Infra.Data;
using RSoft.Lib.Common.Models;
using MassTransit;
using RSoft.Lib.Contracts.Events;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// User application service
    /// </summary>
    public class UserAppService : AppServiceBase<UserDto, User, Guid>, IUserAppService
    {

        #region Local objects/variables

        private readonly IAppClientDomainService _appClientDomain;
        private readonly IRoleDomainService _roleDomain;
        private new readonly IUserDomainService _dmn;
        private readonly IStringLocalizer<AppResource> _localizer;
        private readonly IBusControl _bus;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new user application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">User domain service object</param>
        /// <param name="appClientDomain">Application-client domain service</param>
        /// <param name="roleDomain">Role domain service</param>
        /// <param name="localizer">Language string localizer</param>
        /// <param name="bus">Message bus control</param>
        public UserAppService
        (
            IUnitOfWork uow,
            IUserDomainService dmn,
            IAppClientDomainService appClientDomain,
            IRoleDomainService roleDomain,
            IStringLocalizer<AppResource> localizer,
            IBusControl bus
        ) : base(uow, dmn)
        {
            _appClientDomain = appClientDomain;
            _roleDomain = roleDomain;
            _dmn = dmn;
            _localizer = localizer;
            _bus = bus;
        }

        #endregion

        #region Local methods

        ///<inheritdoc/>
        protected override async Task<User> GetEntityByKeyAsync(UserDto dto, CancellationToken cancellationToken = default)
            => await _dmn.GetByKeyAsync(dto.Id, cancellationToken);

        ///<inheritdoc/>
        protected override UserDto MapToDto(User entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override void MapToEntity(UserDto dto, User entity)
            => entity.Map(dto);

        ///<inheritdoc/>
        protected override User MapToEntity(UserDto dto)
            => dto.Map();

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void ValidateEntity(User entity)
        {
            base.ValidateEntity(entity);
            if (entity.Valid)
            {
                if (_dmn.GetByDocumentAsync(entity.Document, default).GetAwaiter().GetResult() != null)
                    entity.AddNotification(nameof(User.Document), _localizer["DOC_ALREADY_EXISTS"]);
            }
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override async Task<UserDto> AddAsync(UserDto dto, CancellationToken cancellationToken = default)
        {

            IList<AppClient> appClients = new List<AppClient>();

            foreach (AppClientDto item in dto.ApplicationClients)
            {
                AppClient appClient = await _appClientDomain.GetByKeyAsync(item.Id, cancellationToken);
                if (appClient == null)
                    dto.AddNotification("AppClients", string.Format(_localizer["APPCLIENT_ID_NOT_FOUND"], item.Id));
                else
                    appClients.Add(appClient);
            }

            if (dto.Notifications.Count > 0)
                return dto;

            dto.ApplicationClients = appClients.Select(s => s.Map(false)).ToList();

            UserDto result = await base.AddAsync(dto, cancellationToken);
            if (result.Valid)
            {
                UserCreatedEvent message = new(result.Id, result.Name.FirstName, result.Name.LastName, result.Email, result.BornDate, result.Type.Value, result.IsActive);
                await _bus.Publish(message, cancellationToken);
            }
            return result;

        }

        ///<inheritdoc/>
        public override async Task<UserDto> UpdateAsync(Guid key, UserDto dto, CancellationToken cancellationToken = default)
        {
            UserDto result = await base.UpdateAsync(key, dto, cancellationToken);
            if (result.Valid)
            {
                UserChangedEvent message = new(result.Id, result.Name.FirstName, result.Name.LastName, result.Email, result.BornDate, result.Type.Value, result.IsActive);
                await _bus.Publish(message, cancellationToken);
            }
            return result;
        }

        ///<inheritdoc/>
        public override Task DeleteAsync(Guid key, CancellationToken cancellationToken = default)
        {
            base.DeleteAsync(key, cancellationToken);
            _bus.Publish(new UserDeletedEvent(key), cancellationToken);
            return Task.CompletedTask;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserDto>> GetAllAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            IEnumerable<User> users = await _dmn.GetAllAsync(clientId, cancellationToken);
            IEnumerable<UserDto> dtos = users.Select(u => u.Map());
            return dtos;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default)
            => await _dmn.AddAppClientAsync(userId, clientId, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> RemoveAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default)
            => await _dmn.RemoveAppClientAsync(userId, clientId, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddRoleAsync(Guid clientId, Guid userId, IEnumerable<Guid> roles, CancellationToken cancellationToken)
        {

            bool success = false;
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (roles?.Count() > 0)
            {

                if (roles.Distinct().Count() == roles.Count())
                {

                    IList<Role> rolesList = new List<Role>();

                    foreach (Guid item in roles)
                    {
                        if (item != Guid.Empty)
                        {

                            Role role = await _roleDomain.GetByKeyAsync(item, cancellationToken);
                            if (role != null)
                            {
                                if (role.AppClient.Id == clientId)
                                    rolesList.Add(role);
                                else
                                    errors.Add(item.ToString(), _localizer["ROLE_NOT_BELONG_APPCLIENT"]);
                            }
                            else
                                errors.Add(item.ToString(), _localizer["ROLE_NOT_FOUND"]);
                        }
                        else
                            errors.Add(item.ToString(), _localizer["INVALID_ROLE"]);
                    }

                    if (errors.Count == 0)
                    {
                        return await _dmn.AddRoleAsync(userId, rolesList, cancellationToken);
                    }

                }
                else
                    errors.Add("Roles", _localizer["DUPLICATED_ROLE_LIST"]);

            }
            else
                errors.Add("Roles", _localizer["ROLE_LIST_REQUIRED"]);

            return new SimpleOperationResult(success, errors);
        }

        /// <summary>
        /// Remove a role for user
        /// </summary>
        /// <param name="userId">User id key</param>
        /// <param name="roleId">Role id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        public async Task<SimpleOperationResult> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
            => await _dmn.RemoveRoleAsync(userId, roleId, cancellationToken);

        #endregion

    }

}
