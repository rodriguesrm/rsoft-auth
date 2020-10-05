using RSoft.Framework.Infra.Data;
using RoleDomain = RSoft.Auth.Domain.Entities.Role;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using System.Collections.Generic;
using System.Linq;
using RSoft.Auth.Infra.Data.Entities;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Role repository
    /// </summary>
    public class RoleRepository : RepositoryBase<RoleDomain, Role, Guid>, IRoleRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public RoleRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override RoleDomain Map(Role table)
            => table.Map();

        ///<inheritdoc/>
        protected override Role MapForAdd(RoleDomain entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override Role MapForUpdate(RoleDomain entity, Role table)
            => entity.Map(table);

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public ICollection<RoleDomain> GetRolesByUser(Guid scopeId, Guid userId)
        {

            ICollection<RoleDomain> result = _ctx
                .Set<UserRole>()
                .Where(x => x.Role.ScopeId == scopeId && x.UserId == userId)
                .ToList()
                .Select(s => s.Role.Map())
                .ToList();

            return result;

        }

        ///<inheritdoc/>
        public async Task<RoleDomain> GetByNameAsync(Guid scopeId, string roleName, CancellationToken cancellationToken = default)
        {
            Role table = await _dbSet.FirstOrDefaultAsync(x => x.ScopeId == scopeId && x.Name == roleName, cancellationToken);
            RoleDomain entity = table.Map();
            return entity;
        }

        #endregion

    }

}
