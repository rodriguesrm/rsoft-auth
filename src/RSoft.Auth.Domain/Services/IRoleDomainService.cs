using RSoft.Auth.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Threading;
using RSoft.Lib.Design.Domain.Services;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Role domain service interface
    /// </summary>
    public interface IRoleDomainService : IDomainServiceBase<Role, Guid>
    {

        /// <summary>
        /// Find role in the scope by name
        /// </summary>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<Role> GetByNameAsync(Guid scopeId, string roleName, CancellationToken cancellationToken = default);
    }

}
