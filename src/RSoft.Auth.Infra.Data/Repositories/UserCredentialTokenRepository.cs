using UserCredentialTokenDomain = RSoft.Auth.Domain.Entities.UserCredentialToken;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Lib.Design.Infra.Data;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// UserCredentialToken repository
    /// </summary>
    public class UserCredentialTokenRepository : RepositoryBase<UserCredentialTokenDomain, UserCredentialToken, Guid>, IUserCredentialTokenRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public UserCredentialTokenRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override UserCredentialTokenDomain Map(UserCredentialToken table)
            => table.Map();

        ///<inheritdoc/>
        protected override UserCredentialToken MapForAdd(UserCredentialTokenDomain entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override UserCredentialToken MapForUpdate(UserCredentialTokenDomain entity, UserCredentialToken table)
            => entity.Map(table);

        #endregion

    }

}
