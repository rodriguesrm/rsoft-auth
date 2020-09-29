using RSoft.Auth.Application.Model;
using RSoft.Framework.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// User application service interface contract
    /// </summary>
    public interface IUserAppService : IAppServiceBase<UserDto, Guid>
    {

        /// <summary>
        /// Get all rows
        /// </summary>
        /// <param name="scopeId">Scope application id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<IEnumerable<UserDto>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default);

    }

}
