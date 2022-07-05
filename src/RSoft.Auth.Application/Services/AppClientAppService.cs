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
    /// Application-Client application service
    /// </summary>
    public class AppClientAppService : AppServiceBase<AppClientDto, AppClient, Guid>, IAppClientAppService
    {

        #region Local objects/variables

        private readonly ILogger<AppClientAppService> _logger;
        private readonly IUserDomainService _userDomain;
        private readonly IStringLocalizer<AppResource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Application-Client application service
        /// </summary>
        /// <param name="uow">Unit of work object</param>
        /// <param name="dmn">Application-Client domain service object</param>
        /// <param name="userDomain">User domain service object</param>
        /// <param name="localizer">Language string localizer</param>
        /// <param name="logger">Logger object</param>
        public AppClientAppService
        (
            IUnitOfWork uow, 
            IAppClientDomainService dmn, 
            IUserDomainService userDomain,
            IStringLocalizer<AppResource> localizer,
            ILogger<AppClientAppService> logger
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
        protected override async Task<AppClient> GetEntityByKeyAsync(AppClientDto dto, CancellationToken cancellationToken = default)
            => await _dmn.GetByKeyAsync(dto.Id, cancellationToken);

        ///<inheritdoc/>
        protected override AppClientDto MapToDto(AppClient entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override void MapToEntity(AppClientDto dto, AppClient entity)
            => entity.Map(dto);

        ///<inheritdoc/>
        protected override AppClient MapToEntity(AppClientDto dto)
            => dto.Map();

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override async Task<AppClientDto> AddAsync(AppClientDto dto, CancellationToken cancellationToken = default)
        {

            dto.AccessKey = dto.AccessKey == Guid.Empty ? Guid.NewGuid() : dto.AccessKey;
            dto = await base.AddAsync(dto, cancellationToken);
            if (dto.Valid && dto.AllowLogin)
            {
                User user = new(dto.Id)
                {
                    Document = GeneratedUserServiceDocument(),
                    Email = new Email($"{dto.Name.ToLower().Replace(" ", string.Empty)}@service.na"),
                    Name = new Name(dto.Name.Replace("Service", "").Trim(), "Service"),
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
        public async Task<OperationResult<byte[]>> ExportAppClient(Guid? clientId, CancellationToken cancellationToken)
        {

            OperationResult<byte[]> result = new();

            IList<AppClient> appClients = new List<AppClient>();
            if (clientId.HasValue)
                appClients.Add(await _dmn.GetByKeyAsync(clientId.Value, cancellationToken));
            else
                appClients = (await _dmn.GetAllAsync(cancellationToken)).ToList();

            if (appClients.Count > 0)
            {

                StringBuilder sb = new();

                foreach (var appClient in appClients)
                {

                    // Id;Name;AccessKey;AllowLogin;IsActive
                    sb.Append($"{appClient.Id};");
                    sb.Append($"{appClient.Name};");
                    sb.Append($"{appClient.AccessKey};");
                    sb.Append($"{(appClient.AllowLogin ? "1" : "0")};");
                    sb.AppendLine($"{(appClient.IsActive ? "1" : "0")}");

                }

                result.Sucess = true;
                result.Result = Encoding.ASCII.GetBytes(sb.ToString());

            }
            else
            {
                result.Message = _localizer["APPCLIENT_NOT_FOUND"];
            }

            return result;

        }

        ///<inheritdoc/>
        public async Task<OperationResult<IEnumerable<RowImportResult>>> ImportAppClient(byte[] buffer, CancellationToken cancellationToken)
        {

            IList<RowImportResult> rows = new List<RowImportResult>();

            string contentFile = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            string[] lines = contentFile.Split("\r\n");

            for (int pos = 0; pos < lines.Length; pos++)
            {

                int lineNumber = pos + 1;

                try
                {

                    if (lines[pos].Trim() == String.Empty)
                    {
                        rows.Add(new RowImportResult(lineNumber, null, false, _localizer["LINE_EMPTY"]));
                    }
                    else
                    {

                        string[] columns = lines[pos].Split(";");

                        if (columns.Length != 5)
                        {
                            rows.Add(new RowImportResult(lineNumber, null, false, _localizer["INVALID_LAYOUT"]));
                        }
                        else
                        {

                            Guid id = new(columns[0]);
                            string name = columns[1];
                            Guid accessKey = new(columns[2]);
                            bool allowLogin = columns[3] == "1";
                            bool isActive = columns[4] == "1";

                            AppClientDto appClient = new()
                            {
                                Id = id,
                                Name = name,
                                AccessKey = accessKey,
                                AllowLogin = allowLogin,
                                IsActive = isActive
                            };

                            if (await GetByKeyAsync(id, cancellationToken) == null)
                            {
                                appClient = await AddAsync(appClient, cancellationToken);
                                if (appClient.Valid)
                                    rows.Add(new RowImportResult(lineNumber, id, true, null));
                                else
                                    rows.Add(new RowImportResult(lineNumber, id, false, appClient.Notifications.First().Message));
                            }
                            else
                            {
                                rows.Add(new RowImportResult(lineNumber, id, false, _localizer["APPCLIENT_ALREADY_EXISTS"]));
                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    rows.Add(new RowImportResult(lineNumber, null, false, ex.GetBaseException().Message));
                }

            }

            return new OperationResult<IEnumerable<RowImportResult>>()
            {
                Sucess = true,
                Result = rows
            };

        }

        ///<inheritdoc/>
        public async Task<(AppClientDto, IEnumerable<string>)> GetByCredentialsAsync(Guid appKey, Guid appAccess, CancellationToken cancellationToken = default)
        {
            IEnumerable<string> appClients = null;
            AppClientDto appClient = await base.GetByKeyAsync(appKey, cancellationToken);
            if (appAccess == appClient.AccessKey)
            {
                User user = await _userDomain.GetByKeyAsync(appKey);
                appClients = user.ApplicationClients.Select(s => s.Id.ToString());
            }
            else
            {
                appClient = null;
            }
            return (appClient, appClients);
        }

        #endregion

    }

}
