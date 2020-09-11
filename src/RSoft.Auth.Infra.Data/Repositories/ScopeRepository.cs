using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Scope repository
    /// </summary>
    public class ScopeRepository : RepositoryBase<Scope, Guid>, IScopeRepository
    {

        ///<inheritdoc/>
        public ScopeRepository(AuthContext ctx) : base(ctx) { }

    }

}
