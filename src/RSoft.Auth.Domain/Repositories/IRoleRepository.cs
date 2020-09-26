using RSoft.Auth.Domain.Entities;
using RSoft.Framework.Infra.Data;
using System;
using System.Collections.Generic;

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

    }

}
