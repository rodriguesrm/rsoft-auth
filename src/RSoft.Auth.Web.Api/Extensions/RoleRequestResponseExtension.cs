using RSoft.Auth.Application.Model;
using RSoft.Framework.Application.Model;
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

    }

}
