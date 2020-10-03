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
    /// Scope application service
    /// </summary>
    public class ScopeAppService : AppServiceBase<ScopeDto, Scope, Guid>, IScopeAppService
    {

        #region Constructors

        /// <summary>
        /// Create a new Scope application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">Scope domain service object</param>
        public ScopeAppService(IUnitOfWork uow, IScopeDomainService dmn) : base(uow, dmn) { }

        #endregion

        #region Local methods

        ///<inheritdoc/>
        protected override async Task<Scope> GetEntityByKeyAsync(ScopeDto dto, CancellationToken cancellationToken = default)
            => await _dmn.GetByKeyAsync(dto.Id, cancellationToken);

        ///<inheritdoc/>
        protected override ScopeDto MapToDto(Scope entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override void MapToEntity(ScopeDto dto, Scope entity)
            => entity.Map(dto);

        ///<inheritdoc/>
        protected override Scope MapToEntity(ScopeDto dto)
            => dto.Map();

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override async Task<ScopeDto> AddAsync(ScopeDto dto, CancellationToken cancellationToken = default)
        {
            dto.AccessKey = dto.AccessKey == Guid.Empty ? Guid.NewGuid() : dto.AccessKey;
            return await base.AddAsync(dto, cancellationToken);
        }

        #endregion

    }

}
