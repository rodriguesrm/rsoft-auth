using Microsoft.EntityFrameworkCore;
using RSoft.Auth.Infra.Data.Configuration;
using RSoft.Framework.Cross;
using RSoft.Framework.Infra.Data;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Infra.Data
{

    /// <summary>
    /// Auth database context
    /// </summary>
    public class AuthContext : DbContextBase<Guid>
    {

        #region Constructors

        /// <summary>
        /// Create a new dbcontext instance
        /// </summary>
        /// <param name="options">Context options settings</param>
        public AuthContext(DbContextOptions<AuthContext> options) : this(options, null) { }

        /// <summary>
        /// Create a new dbcontext instance
        /// </summary>
        /// <param name="options">Context options settings</param>
        /// <param name="user">Authenticated user information</param>
        public AuthContext(DbContextOptions<AuthContext> options, IHttpLoggedUser<Guid> user) : base(options, user) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void SetTableConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new ScopeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserScopeConfiguration());
        }

        #endregion

        #region DbSets

        /// <summary>
        /// Users dbset
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Roles dbset
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Scopes dbset
        /// </summary>
        public virtual DbSet<Scope> Scopes { get; set; }

        /// <summary>
        /// User roles dbset
        /// </summary>
        public virtual DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// User scopes dbset
        /// </summary>
        public virtual DbSet<UserScope> UserScopes { get; set; }

        #endregion

    }

}