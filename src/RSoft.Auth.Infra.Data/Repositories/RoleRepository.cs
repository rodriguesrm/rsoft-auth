using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Role repository
    /// </summary>
    public class RoleRepository : RepositoryBase<dmn.Role, tbl.Role, Guid>, IRoleRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public RoleRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override dmn.Role Map(tbl.Role table)
            => table.Map();

        ///<inheritdoc/>
        protected override tbl.Role MapForAdd(dmn.Role entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override tbl.Role MapForUpdate(dmn.Role entity, tbl.Role table)
            => entity.Map(table);

        #endregion

    }

}
