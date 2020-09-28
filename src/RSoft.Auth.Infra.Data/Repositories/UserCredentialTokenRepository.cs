using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// UserCredentialToken repository
    /// </summary>
    public class UserCredentialTokenRepository : RepositoryBase<dmn.UserCredentialToken, tbl.UserCredentialToken, Guid>, IUserCredentialTokenRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public UserCredentialTokenRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override dmn.UserCredentialToken Map(tbl.UserCredentialToken table, bool loadChildren)
            => table.Map(loadChildren);

        ///<inheritdoc/>
        protected override tbl.UserCredentialToken MapForAdd(dmn.UserCredentialToken entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override tbl.UserCredentialToken MapForUpdate(dmn.UserCredentialToken entity, tbl.UserCredentialToken table)
            => entity.Map(table);

        #endregion

    }

}
