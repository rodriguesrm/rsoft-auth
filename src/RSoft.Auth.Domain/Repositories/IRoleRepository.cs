﻿using RSoft.Auth.Domain.Entities;
using RSoft.Lib.Design.Infra.Data;
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
        /// Get all user roles in a specific application-client
        /// </summary>
        /// <param name="userId">User id key value</param>
        ICollection<Role> GetRolesByUser(Guid userId);

        /// <summary>
        /// Find role in the application-client by name
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<Role> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    }

}
