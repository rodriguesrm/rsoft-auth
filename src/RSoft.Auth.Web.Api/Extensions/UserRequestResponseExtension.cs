using RSoft.Auth.Application.Model;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Lib.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// User Request/Response extensions methods
    /// </summary>
    public static class UserRequestResponseExtension
    {

        /// <summary>
        /// Map user dto to Generic model T
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="dto">User data transport object</param>
        public static T Map<T>(this UserDto dto)
            where T : UserListResponse
        {

            if (dto == null)
                return null;

            T result = (T)Activator.CreateInstance(typeof(T), args: dto.Id);

            result.Document = dto.Document;
            result.Name = new FullNameResponse(dto.Name.FirstName, dto.Name.LastName);
            result.Email = dto.Email;
            result.BornDate = dto.BornDate;
            result.Type = dto.Type;
            result.IsActive = dto.IsActive;

            UserDetailResponse userDetail = result as UserDetailResponse;
            if (userDetail != null)
            {
                if (dto.Credential != null)
                {
                    userDetail.Credential = new SimpleUserCredentialResponse()
                    {
                        Login = dto.Credential.Login,
                        ChangeCredentials = dto.Credential.ChangeCredentials
                    };
                }

                IEnumerable<SimpleIdentification<Guid>> scopes = dto
                    .Scopes
                    .Select(s => new SimpleIdentification<Guid>(s.Id, s.Name))
                    .Distinct()
                    .ToList();

                IList<UserPermissonsResponse> permissions = new List<UserPermissonsResponse>();

                foreach (SimpleIdentification<Guid> scope in scopes)
                {
                    IEnumerable<RoleDto> roles = dto.Roles?.Where(x => x.Scope.Id == scope.Id).ToList();
                    UserPermissonsResponse permission = new UserPermissonsResponse()
                    {
                        Id = scope.Id.Value,
                        Name = scope.Name,
                        Roles = roles?.Select(r => r.MapSimple())
                    };
                    permissions.Add(permission);
                }

                userDetail.Permissions = permissions;

            }

            return result;

        }

        /// <summary>
        /// Map user-request to user-dto
        /// </summary>
        /// <param name="request">User request object</param>
        /// <param name="scopeId">Scope id key value</param>
        public static UserDto Map(this UserRequest request, Guid? scopeId)
        {

            UserDto dto = null;
            if (request != null)
            {
                dto = new UserDto()
                {
                    Document = request.Document,
                    Name = request.Name,
                    BornDate = request.BornDate,
                    Email = request.Email,
                    Type = request.Type,
                    IsActive = request.IsActive,
                    Scopes = scopeId.HasValue ? new List<ScopeDto>() { new ScopeDto() { Id = scopeId.Value } } : null
                };
            }
            return dto;

        }

    }

}
