using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Framework.Application.Services;
using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Auth.Domain.Services;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// Role application service
    /// </summary>
    public class RoleAppService : AppServiceBase<RoleDto, Role, Guid>, IRoleAppService
    {

        #region Constructors

        /// <summary>
        /// Create a new Role application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">Role domain service object</param>
        public RoleAppService(IUnitOfWork uow, IRoleDomainService dmn) : base(uow, dmn) { }

        #endregion

        #region Local methods

        ///<inheritdoc/>
        protected override async Task<Role> GetEntityByKeyAsync(RoleDto dto, CancellationToken cancellationToken = default)
            => await _dmn.GetByKeyAsync(new Guid[] { dto.Id }, cancellationToken);

        ///<inheritdoc/>
        protected override RoleDto MapToDto(Role entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override void MapToEntity(RoleDto dto, Role entity)
            => entity.Map(dto);

        ///<inheritdoc/>
        protected override Role MapToEntity(RoleDto dto)
            => dto.Map();

        #endregion

    }

}
