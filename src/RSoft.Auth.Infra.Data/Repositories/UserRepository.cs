using RSoft.Framework.Infra.Data;
using UserDomain = RSoft.Auth.Domain.Entities.User;
using RSoft.Auth.Infra.Data.Entities;
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
    public class UserRepository : RepositoryBase<UserDomain, User, Guid>, IUserRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public UserRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override UserDomain Map(User table)
            => table.Map();

        ///<inheritdoc/>
        protected override User MapForAdd(UserDomain entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override User MapForUpdate(UserDomain entity, User table)
            => entity.Map(table);


        #endregion

        #region Public methods

        ///<inheritdoc/>
        public async Task<UserDomain> GetByLoginAsync(string login, CancellationToken cancellationToken)
        {
            User table = await _dbSet.FirstOrDefaultAsync(u => u.Credential.Login == login || u.Email == login);
            return table.Map();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserDomain>> ListByLoginAsync(string login, CancellationToken cancellationToken)
        {
            IEnumerable<User> tableUsers = await _dbSet.Where(u => u.Credential.Login == login || u.Email == login).ToListAsync(cancellationToken);
            IEnumerable<UserDomain> users = tableUsers.Select(u => u.Map(true));
            return users;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<UserDomain>> GetAllAsync(Guid scopeId, CancellationToken cancellationToken = default)
        {
            IEnumerable<User> tableUsers = await _dbSet.Where(x => x.Scopes.Any(s => s.ScopeId == scopeId)).ToListAsync(cancellationToken);
            IEnumerable<UserDomain> users = tableUsers.Select(u => u.Map(true));
            return users;
        }

        ///<inheritdoc/>
        public async Task AddUserScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
        {
            DbSet<UserScope> dbSet = _ctx.Set<UserScope>();
            UserScope userScope = new UserScope()
            {
                UserId = userId,
                ScopeId = scopeId
            };
            await dbSet.AddAsync(userScope, cancellationToken);
        }

        ///<inheritdoc/>
        public Task RemoveUserScopeAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                DbSet<UserScope> dbSet = _ctx.Set<UserScope>();
                UserScope userScope = dbSet.Where(x => x.UserId == userId && x.ScopeId == scopeId).FirstOrDefault();
                if (userScope != null)
                    dbSet.Remove(userScope);
            });
        }

        ///<inheritdoc/>
        public async Task AddUserRoleAsync(Guid userId, IEnumerable<Guid> roles, CancellationToken cancellationToken = default)
        {

            if (roles?.Count() > 0)
            {

                DbSet<UserRole> dbSet = _ctx.Set<UserRole>();

                IList<UserRole> userRoles = new List<UserRole>();
                foreach (Guid item in roles)
                {
                    userRoles.Add(new UserRole()
                    {
                        UserId = userId,
                        RoleId = item
                    });
                }
                await dbSet.AddRangeAsync(userRoles, cancellationToken);

            }
        }

        ///<inheritdoc/>
        public Task RemoveUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                DbSet<UserRole> dbSet = _ctx.Set<UserRole>();
                UserRole userRole = dbSet.Where(x => x.UserId == userId && x.RoleId == roleId).FirstOrDefault();
                if (userRole != null)
                    dbSet.Remove(userRole);
            });
        }

        #endregion

    }

}
