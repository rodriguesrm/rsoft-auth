using RSoft.Auth.Domain.Entities;
using RSoft.Framework.Infra.Data;
using System;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// Role repository contract interface
    /// </summary>
    public interface IRoleRepository : IRepositoryBase<Role, Guid> 
    { 
    }

}
