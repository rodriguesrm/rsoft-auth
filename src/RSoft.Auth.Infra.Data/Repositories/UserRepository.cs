using RSoft.Framework.Infra.Data;
using dmn = RSoft.Auth.Domain.Entities;
using tbl = RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            => table.Map();

        ///<inheritdoc/>
        protected override tbl.User MapForAdd(dmn.User entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override tbl.User MapForUpdate(dmn.User entity, tbl.User table)
            => entity.Map(table);


        #endregion

        #region Public methods

        /// <summary>
        /// Get user by login
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        public async Task<dmn.User> GetByLoginAsync(string login, CancellationToken cancellationToken)
        {
            tbl.User table = await _dbSet.FirstOrDefaultAsync(u => u.Credential.Username == login || u.Email == login);
            return table.Map();
        }

        #endregion

    }

}
