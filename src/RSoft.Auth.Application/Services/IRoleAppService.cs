using RSoft.Auth.Application.Model;
using RSoft.Lib.Design.Application.Services;
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
