using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Framework.Application.Services;
using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Auth.Domain.Services;
using RSoft.Framework.Domain.ValueObjects;
using RSoft.Framework.Cross.Enums;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// Scope application service
    /// </summary>
    public class ScopeAppService : AppServiceBase<ScopeDto, Scope, Guid>, IScopeAppService
    {

        #region Local objects/variables

        private readonly ILogger<ScopeAppService> _logger;
        private readonly IUserDomainService _userDomain;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Scope application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">Scope domain service object</param>
        /// <param name="userDomain">User domain service object</param>
        /// <param name="logger">Logger object</param>
        public ScopeAppService
        (
            IUnitOfWork uow, 
            IScopeDomainService dmn, 
            IUserDomainService userDomain,
            ILogger<ScopeAppService> logger
        ) : base(uow, dmn) 
        {
            _userDomain = userDomain;
            _logger = logger;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Generate a random user service document
        /// </summary>
        private string GeneratedUserServiceDocument()
        {
            Thread.Sleep(150);
            return DateTime.UtcNow.ToString("yy MM dd HH mm ss ff").Replace(" ", string.Empty);
        }

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
            dto = await base.AddAsync(dto, cancellationToken);
            if (dto.AllowLogin)
            {
                User user = new User(dto.Id)
                {
                    Document = GeneratedUserServiceDocument(),
                    Email = new Email($"{dto.Name.ToLower().Replace(" ", string.Empty)}@service.na"),
                    Name = new Name(dto.Name, "Service"),
                    BornDate = DateTime.UtcNow,
                    IsActive = false,
                    Type = UserType.Service
                };
                user = await _userDomain.AddAsync(user, cancellationToken);
                if (user.Valid)
                    await _uow.SaveChangesAsync(cancellationToken);
                else
                    _logger.LogWarning($"User is invalid => {string.Join("|", user.Notifications.Select(n => $"{n.Property}-{n.Message}"))}");
            }
            return dto;
        }

        ///<inheritdoc/>
        public override Task DeleteAsync(Guid key, CancellationToken cancellationToken = default)
        {
            _userDomain.Delete(key);
            return base.DeleteAsync(key, cancellationToken);
        }

        #endregion

    }

}
