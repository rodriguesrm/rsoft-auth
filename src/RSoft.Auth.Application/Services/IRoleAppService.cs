using RSoft.Auth.Application.Model;
using RSoft.Framework.Application.Services;
using System;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// Role application service interface contract
    /// </summary>
    public interface IRoleAppService : IAppServiceBase<RoleDto, Guid>
    {
    }

}
