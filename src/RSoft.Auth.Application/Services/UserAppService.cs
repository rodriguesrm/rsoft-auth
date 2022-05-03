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
using System.Text;
using RSoft.Auth.Cross.Common.Model.Results;
using Microsoft.AspNetCore.Http;
using RSoft.Lib.Common.Enums;
using RSoft.Lib.Common.ValueObjects;

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
        private readonly IBusControl _bus;
        private const string fileHeader = "Id;CreatedOn;CreatedBy;IsActive;Document;FirstName;LastName;BornDate;Email;Type";

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
        /// <param name="bus">Message bus control</param>
        public UserAppService
        (
            IUnitOfWork uow,
            IUserDomainService dmn,
            IScopeDomainService scopeDomain,
            IRoleDomainService roleDomain,
            IStringLocalizer<AppResource> localizer,
            IBusControl bus
        ) : base(uow, dmn)
        {
            _scopeDomain = scopeDomain;
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
        public async Task<IEnumerable<UserDto>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default)
        {
            IEnumerable<User> users = await _dmn.GetAllAsync(scopeId, cancellationToken);
            IEnumerable<UserDto> dtos = users.Select(u => u.Map());
            return dtos;
        }

        ///<inheritdoc/>
        public async Task<SimpleOperationResult> AddScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
            => await _dmn.AddScopeAsync(userId, scopeId, cancellationToken);

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

        ///<inheritdoc/>
        public async Task<OperationResult<byte[]>> ExportUser(Guid userId, CancellationToken cancellationToken)
        {

            OperationResult<byte[]> result = new();

            User user = await _dmn.GetByKeyAsync(userId, cancellationToken);
            if (user != null)
            {

                StringBuilder sb = new();

                sb.AppendLine(fileHeader);
                sb.Append($"{user.Id};");
                sb.Append($"{user.CreatedOn:u};");
                sb.Append($"{user.CreatedAuthor.Id};");
                sb.Append($"{(user.IsActive ? "1" : "0")};");
                sb.Append($"{user.Document};");
                sb.Append($"{user.Name.FirstName};");
                sb.Append($"{user.Name.LastName};");
                sb.Append($"{user.BornDate?.ToString("u")};");
                sb.Append($"{user.Email.Address};");
                sb.Append(((int)user.Type).ToString());

                result.Sucess = true;
                result.Result = Encoding.ASCII.GetBytes(sb.ToString());

            }
            else
            {
                result.Message = _localizer["USER_NOT_FOUND"];
            }

            return result;

        }

        ///<inheritdoc/>
        public async Task<OperationResult<IEnumerable<RowImportResult>>> ImportUser(IFormFile file, CancellationToken cancellationToken)
        {

            IList<RowImportResult> rows = new List<RowImportResult>();

            System.IO.Stream fileStream = file.OpenReadStream();
            byte[] buffer = new byte[fileStream.Length];
            await fileStream.ReadAsync(buffer, cancellationToken);

            string contentFile = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            string[] lines = contentFile.Split("\r\n");

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {

                try
                {

                    if (lines[lineNumber] != fileHeader)
                    {

                        string[] columns = lines[lineNumber].Split(";");

                        Guid id = new(columns[0]);
                        DateTime createdOn = DateTime.Parse(columns[1]);
                        Guid createdBy = new(columns[2]);
                        bool isActive = columns[3] == "1";
                        string document = columns[4];
                        string firstName = columns[5];
                        string lastName = columns[6];
                        DateTime bornDate = DateTime.Parse(columns[7]);
                        string email = columns[8];
                        UserType userType = (UserType)int.Parse(columns[9]);

                        UserDto user = new()
                        {
                            Id = id,
                            CreatedBy = new AuditAuthor<Guid>(createdOn, createdBy, string.Empty),
                            IsActive = isActive,
                            Document = document,
                            Name = new Name(firstName, lastName),
                            BornDate = bornDate,
                            Email = email,
                            Type = userType,
                            Scopes = new List<ScopeDto>()
                        };

                        user = await AddAsync(user);
                        if (user.Valid)
                            rows.Add(new RowImportResult((lineNumber + 1), id, true, null));
                        else
                            rows.Add(new RowImportResult((lineNumber + 1), id, false, user.Notifications.First().Message));

                    }
                }
                catch (Exception ex)
                {
                    rows.Add(new RowImportResult((lineNumber + 1), Guid.Empty, false, ex.Message));
                }

            }

            fileStream.Close();

            return new OperationResult<IEnumerable<RowImportResult>>()
            {
                Sucess = true,
                Result = rows
            };

        }

        #endregion

    }

}
