using ScopeDomain = RSoft.Auth.Domain.Entities.Scope;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Lib.Design.Infra.Data;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Scope repository
    /// </summary>
    public class ScopeRepository : RepositoryBase<ScopeDomain, Scope, Guid>, IScopeRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public ScopeRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override ScopeDomain Map(Scope table)
            => table.Map();

        ///<inheritdoc/>
        protected override Scope MapForAdd(ScopeDomain entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override Scope MapForUpdate(ScopeDomain entity, Scope table)
            => entity.Map(table);

        #endregion

    }

}
