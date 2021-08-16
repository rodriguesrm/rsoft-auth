using RSoft.Auth.Domain.Entities;
using RSoft.Lib.DDD.Infra.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// Role repository contract interface
    /// </summary>
    public interface IRoleRepository : IRepositoryBase<Role, Guid>
    {

        /// <summary>
        /// Get all user roles in a specific scope
        /// </summary>
        /// <param name="scopeId">Scope id key value</param>
        /// <param name="userId">User id key value</param>
        ICollection<Role> GetRolesByUser(Guid scopeId, Guid userId);

        /// <summary>
        /// Find role in the scope by name
        /// </summary>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<Role> GetByNameAsync(Guid scopeId, string roleName, CancellationToken cancellationToken = default);
    }

}
