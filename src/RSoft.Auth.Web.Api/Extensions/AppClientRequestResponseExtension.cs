using RSoft.Auth.Application.Model;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// Application-Client Request/Response extensions methods
    /// </summary>
    public static class AppClientRequestResponseExtension
    {

        /// <summary>
        /// Map dto to response
        /// </summary>
        /// <param name="dto">Application-Client dto object instance</param>
        public static AppClientResponse Map(this AppClientDto dto)
            => new(dto.Id)
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
        public static AppClientDto Map(this AppClientRequest request)
            => new()
            {
                Name = request.Name,
                IsActive = request.IsActive.Value,
                AllowLogin = request.AllowLogin.Value
            };

    }

}
