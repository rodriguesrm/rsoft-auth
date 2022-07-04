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
        /// Find role in the application-client by name
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<Role> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    }

}
