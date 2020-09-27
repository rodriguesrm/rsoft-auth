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
using System.Collections.Generic;

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

        ///<inheritdoc/>
        public async Task<dmn.User> GetByLoginAsync(string login, CancellationToken cancellationToken)
        {
            tbl.User table = await _dbSet.FirstOrDefaultAsync(u => u.Credential.Login == login || u.Email == login);
            return table.Map();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<dmn.User>> ListByLoginAsync(string login, CancellationToken cancellationToken)
        {
            IEnumerable<tbl.User> tableUsers = await _dbSet.Where(u => u.Credential.Login == login || u.Email == login).ToListAsync(cancellationToken);
            IEnumerable<dmn.User> users = tableUsers
                .Select(u => u.Map(true));
            return users;
        }

        #endregion

    }

}
