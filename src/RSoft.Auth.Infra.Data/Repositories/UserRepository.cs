using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : RepositoryBase<dmn.User, tbl.User, Guid>, IUserRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public UserRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override dmn.User Map(tbl.User table)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override tbl.User Map(dmn.User entity)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion

    }

}
