using RSoft.Framework.Infra.Data;
using RSofth.Auth.Domain.Entities;
using RSofth.Auth.Domain.Repositories;
using System;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Role repository
    /// </summary>
    public class RoleRepository : RepositoryBase<Role, Guid>, IRoleRepository
    {

        ///<inheritdoc/>
        public RoleRepository(AuthContext ctx) : base(ctx) { }

    }

}
