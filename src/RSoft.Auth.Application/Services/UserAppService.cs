using RSoft.Auth.Application.Model;
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
using FluentValidator;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new user application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">User domain service object</param>
        /// <param name="scopeDomain">Scope domain service</param>
        /// <param name="roleDomain">Role domain service</param>
        public UserAppService(IUnitOfWork uow, IUserDomainService dmn, IScopeDomainService scopeDomain, IRoleDomainService roleDomain) : base(uow, dmn) 
        {
            _scopeDomain = scopeDomain;
            _roleDomain = roleDomain;
            _dmn = dmn;
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

        #region Public methods

        ///<inheritdoc/>
        public override async Task<UserDto> AddAsync(UserDto dto, CancellationToken cancellationToken = default)
        {

            IList<Scope> scopes = new List<Scope>();
            //IList<Role> roles = new List<Role>();

            foreach (ScopeDto item in dto.Scopes)
            {
                Scope scope = await _scopeDomain.GetByKeyAsync(item.Id, cancellationToken);
                if (scope == null)
                    dto.AddNotification("Scopes", $"Scope '{item.Id}' not found");
                else
                    scopes.Add(scope);
            }

            //foreach (RoleDto item in dto.Roles)
            //{
            //    Role role = await _roleDomain.GetByKeyAsync(item.Id, cancellationToken);
            //    if (role == null)
            //        dto.AddNotification("Roles", $"Role '{item.Id}' not found");
            //    else
            //        roles.Add(role);
            //}

            //foreach (Role role in roles)
            //{
            //    if (scopes.FirstOrDefault(s => s.Id == role.Scope?.Id) == null)
            //        dto.AddNotification("Roles", $"The {role.Name} role belongs to a scope to which the user is not assigned.");
            //}

            if (dto.Notifications.Count > 0)
                return dto;

            dto.Scopes = scopes.Select(s => s.Map(false)).ToList();
            //dto.Roles = roles.Select(r => r.Map(false)).ToList();

            return await base.AddAsync(dto, cancellationToken);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserDto>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default)
        {
            IEnumerable<User> users = await _dmn.GetAllAsync(scopeId, cancellationToken);
            IEnumerable<UserDto> dtos = users.Select(u => u.Map());
            return dtos;
        }

        #endregion

    }

}
