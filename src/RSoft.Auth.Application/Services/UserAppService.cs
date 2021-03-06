﻿using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Framework.Application.Services;
using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Auth.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using RSoft.Framework.Application.Model;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Language;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// User application service
    /// </summary>
    public class UserAppService : AppServiceBase<UserDto, User, Guid>, IUserAppService
    {

        #region Local objects/variables

        private readonly IScopeDomainService _scopeDomain;
        private readonly IRoleDomainService _roleDomain;
        private new readonly IUserDomainService _dmn;
        private readonly IStringLocalizer<AppResource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new user application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">User domain service object</param>
        /// <param name="scopeDomain">Scope domain service</param>
        /// <param name="roleDomain">Role domain service</param>
        /// <param name="localizer">Language string localizer</param>
        public UserAppService
        (
            IUnitOfWork uow, 
            IUserDomainService dmn, 
            IScopeDomainService scopeDomain, 
            IRoleDomainService roleDomain,
            IStringLocalizer<AppResource> localizer
        ) : base(uow, dmn) 
        {
            _scopeDomain = scopeDomain;
            _roleDomain = roleDomain;
            _dmn = dmn;
            _localizer = localizer;
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

            IList<Scope> scopes = new List<Scope>();

            foreach (ScopeDto item in dto.Scopes)
            {
                Scope scope = await _scopeDomain.GetByKeyAsync(item.Id, cancellationToken);
                if (scope == null)
                    dto.AddNotification("Scopes", string.Format(_localizer["SCOPE_ID_NOT_FOUND"], item.Id));
                else
                    scopes.Add(scope);
            }

            if (dto.Notifications.Count > 0)
                return dto;

            dto.Scopes = scopes.Select(s => s.Map(false)).ToList();

            return await base.AddAsync(dto, cancellationToken);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserDto>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default)
        {
            IEnumerable<User> users = await _dmn.GetAllAsync(scopeId, cancellationToken);
            IEnumerable<UserDto> dtos = users.Select(u => u.Map());
            return dtos;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
            => await _dmn.AddScopeAsync(userId, scopeId);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> RemoveScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
            => await _dmn.RemoveScopeAsync(userId, scopeId, cancellationToken);

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddRoleAsync(Guid scopeId, Guid userId, IEnumerable<Guid> roles, CancellationToken cancellationToken)
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
                                if (role.Scope.Id == scopeId)
                                    rolesList.Add(role);
                                else
                                    errors.Add(item.ToString(), _localizer["ROLE_NOT_BELONG_SCOPE"]);
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
