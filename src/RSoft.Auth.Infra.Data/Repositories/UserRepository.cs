using RSoft.Framework.Infra.Data;
using RSofth.Auth.Domain.Entities;
using RSofth.Auth.Domain.Repositories;
using System;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {

        ///<inheritdoc/>
        public UserRepository(AuthContext ctx) : base(ctx) { }

    }

}
