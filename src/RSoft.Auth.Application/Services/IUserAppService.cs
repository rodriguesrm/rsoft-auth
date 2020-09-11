using RSoft.Auth.Application.Model;
using RSoft.Framework.Application.Services;
using System;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// User application service interface contract
    /// </summary>
    public interface IUserAppService : IAppServiceBase<UserDto, Guid>
    {
    }

}
