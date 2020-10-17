using RSoft.Auth.Application.Model;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// Scope Request/Response extensions methods
    /// </summary>
    public static class ScopeRequestResponseExtension
    {

        /// <summary>
        /// Map dto to response
        /// </summary>
        /// <param name="dto">Scope dto object instance</param>
        public static ScopeResponse Map(this ScopeDto dto)
            => new ScopeResponse(dto.Id)
            {
                Name = dto.Name,
                AccessKey = dto.AccessKey,
                AllowLogin = dto.AllowLogin,
                IsActive = dto.IsActive
            };

        /// <summary>
        /// Map request to dto
        /// </summary>
        /// <param name="request">User request object instance</param>
        public static ScopeDto Map(this ScopeRequest request)
            => new ScopeDto
            {
                Name = request.Name,
                IsActive = request.IsActive.Value,
                AllowLogin = request.AllowLogin.Value
            };

    }

}
