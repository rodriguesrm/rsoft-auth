using UserCredentialDomain = RSoft.Auth.Domain.Entities.UserCredential;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Lib.Design.Infra.Data;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// User Credential repository
    /// </summary>
    public class UserCredentialRepository : RepositoryBase<UserCredentialDomain, UserCredential, Guid>, IUserCredentialRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public UserCredentialRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override UserCredentialDomain Map(UserCredential table)
            => table.Map();

        ///<inheritdoc/>
        protected override UserCredential MapForAdd(UserCredentialDomain entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override UserCredential MapForUpdate(UserCredentialDomain entity, UserCredential table)
            => entity.Map(table);

        #endregion

    }

}
