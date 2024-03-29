﻿using UserDomain = RSoft.Auth.Domain.Entities.User;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using RSoft.Lib.Design.Infra.Data;

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
        public async Task<IEnumerable<UserDomain>> GetAllAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            IEnumerable<User> tableUsers = await _dbSet.Where(x => x.ApplicationClients.Any(s => s.AppClientId == clientId)).ToListAsync(cancellationToken);
            IEnumerable<UserDomain> users = tableUsers.Select(u => u.Map(true));
            return users;
        }

        ///<inheritdoc/>
        public async Task AddUserAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken = default)
        {
            DbSet<UserAppClient> dbSet = _ctx.Set<UserAppClient>();
            UserAppClient userAppClient = new UserAppClient()
            {
                UserId = userId,
                AppClientId = clientId
            };
            await dbSet.AddAsync(userAppClient, cancellationToken);
        }

        ///<inheritdoc/>
        public Task RemoveUserAppClientAsync(Guid userId, Guid clientId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                DbSet<UserAppClient> dbSet = _ctx.Set<UserAppClient>();
                UserAppClient userAppClient = dbSet.Where(x => x.UserId == userId && x.AppClientId == clientId).FirstOrDefault();
                if (userAppClient != null)
                    dbSet.Remove(userAppClient);
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

        ///<inheritdoc/>
        public async Task<UserDomain> GetByDocumentAsync(string document, CancellationToken cancellationToken = default)
        {
            User user = await _dbSet.FirstOrDefaultAsync(x => x.Document == document, cancellationToken);
            return user.Map();
        }

        #endregion

    }

}
