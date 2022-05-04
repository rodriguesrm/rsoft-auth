using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Auth.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using RSoft.Lib.Design.Application.Services;
using RSoft.Lib.Design.Infra.Data;
using RSoft.Lib.Common.ValueObjects;
using RSoft.Lib.Common.Enums;
using RSoft.Lib.Common.Models;
using System.Text;
using RSoft.Auth.Application.Language;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Cross.Common.Model.Results;
using System.Collections.Generic;

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
        private readonly IStringLocalizer<AppResource> _localizer;
        private const string fileHeader = "Id;Name;AccessKey;AllowLogin;IsActive";

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Scope application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">Scope domain service object</param>
        /// <param name="userDomain">User domain service object</param>
        /// <param name="localizer">Language string localizer</param>
        /// <param name="logger">Logger object</param>
        public ScopeAppService
        (
            IUnitOfWork uow, 
            IScopeDomainService dmn, 
            IUserDomainService userDomain,
            IStringLocalizer<AppResource> localizer,
            ILogger<ScopeAppService> logger
        ) : base(uow, dmn) 
        {
            _userDomain = userDomain;
            _localizer = localizer;
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

        #region Public methods

        ///<inheritdoc/>
        public async Task<OperationResult<byte[]>> ExportScope(Guid scopeId, CancellationToken cancellationToken)
        {

            OperationResult<byte[]> result = new();

            Scope scope = await _dmn.GetByKeyAsync(scopeId, cancellationToken);
            if (scope != null)
            {

                // Id;Name;AccessKey;AllowLogin;IsActive
                
                StringBuilder sb = new();

                sb.AppendLine(fileHeader);
                sb.Append($"{scope.Id};");
                sb.Append($"{scope.Name};");
                sb.Append($"{scope.AccessKey};");
                sb.Append($"{(scope.AllowLogin ? "1" : "0")};");
                sb.Append($"{(scope.IsActive ? "1" : "0")};");

                result.Sucess = true;
                result.Result = Encoding.ASCII.GetBytes(sb.ToString());

            }
            else
            {
                result.Message = _localizer["SCOPE_NOT_FOUND"];
            }

            return result;

        }

        ///<inheritdoc/>
        public async Task<OperationResult<IEnumerable<RowImportResult>>> ImportScope(byte[] buffer, CancellationToken cancellationToken)
        {

            IList<RowImportResult> rows = new List<RowImportResult>();

            string contentFile = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            string[] lines = contentFile.Split("\r\n");

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {

                try
                {

                    if (lines[lineNumber] != fileHeader)
                    {

                        //"Id;Name;AccessKey;AllowLogin;IsActive";

                        string[] columns = lines[lineNumber].Split(";");

                        Guid id = new(columns[0]);
                        string name = columns[1];
                        Guid accessKey = new(columns[2]);
                        bool allowLogin = columns[3] == "1";
                        bool isActive = columns[4] == "1";

                        ScopeDto scope = new()
                        {
                            Id = id,
                            Name = name,
                            AccessKey = accessKey,
                            AllowLogin = allowLogin,
                            IsActive = isActive
                        };

                        scope = await AddAsync(scope);
                        if (scope.Valid)
                            rows.Add(new RowImportResult((lineNumber + 1), id, true, null));
                        else
                            rows.Add(new RowImportResult((lineNumber + 1), id, false, scope.Notifications.First().Message));

                    }
                }
                catch (Exception ex)
                {
                    rows.Add(new RowImportResult((lineNumber + 1), Guid.Empty, false, ex.Message));
                }

            }

            return new OperationResult<IEnumerable<RowImportResult>>()
            {
                Sucess = true,
                Result = rows
            };

        }

        #endregion

    }

}
