using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Scope repository
    /// </summary>
    public class ScopeRepository : RepositoryBase<dmn.Scope, tbl.Scope, Guid>, IScopeRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public ScopeRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override dmn.Scope Map(tbl.Scope table)
            => table.Map();

        ///<inheritdoc/>
        protected override tbl.Scope MapForAdd(dmn.Scope entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override tbl.Scope MapForUpdate(dmn.Scope entity, tbl.Scope table)
            => entity.Map(table);

        #endregion

    }

}
