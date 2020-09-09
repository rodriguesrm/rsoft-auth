using RSoft.Framework.Infra.Data;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSofth.Auth.Domain.Repositories
{

    /// <summary>
    /// Role repository contract interface
    /// </summary>
    public interface IRoleRepository : IRepositoryBase<Role, Guid> { }

}
