using RSoft.Auth.Application.Model;
using RSoft.Framework.Application.Services;
using System;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// Scope application service interface contract
    /// </summary>
    public interface IScopeAppService : IAppServiceBase<ScopeDto, Guid>
    {
    }

}
