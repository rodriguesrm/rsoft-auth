using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;

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
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override tbl.Role Map(dmn.Role entity)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion

    }

}
