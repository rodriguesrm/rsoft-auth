using RSoft.Auth.Application.Model;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Lib.Common.Models;
using System;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// Role Request/Response extensions methods
    /// </summary>
    public static class RoleRequestResponseExtension
    {

        /// <summary>
        /// Map role-dto to simple-identification
        /// </summary>
        /// <param name="dto">Role dto</param>
        public static SimpleIdentification<Guid> MapSimple(this RoleDto dto)
            => new SimpleIdentification<Guid>(dto.Id, dto.Name);

        /// <summary>
        /// Map request to dto
        /// </summary>
        /// <param name="request">Request object instance</param>
        public static RoleDto Map(this RoleRequest request)
        {
            return new RoleDto()
            {
                AppClient = new SimpleIdentification<Guid>(request.ClientId, null),
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };
        }

        /// <summary>
        /// Map dto to response
        /// </summary>
        /// <param name="dto">Data transport object</param>
        public static RoleResponse Map(this RoleDto dto)
        {
            return new RoleResponse(dto.Id)
            {
                AppClient = dto.AppClient,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

    }

}
