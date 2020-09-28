using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// User Credential repository
    /// </summary>
    public class UserCredentialRepository : RepositoryBase<dmn.UserCredential, tbl.UserCredential, Guid>, IUserCredentialRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public UserCredentialRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override dmn.UserCredential Map(tbl.UserCredential table)
            => table.Map();

        ///<inheritdoc/>
        protected override tbl.UserCredential MapForAdd(dmn.UserCredential entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override tbl.UserCredential MapForUpdate(dmn.UserCredential entity, tbl.UserCredential table)
            => entity.Map(table);

        #endregion

    }

}
