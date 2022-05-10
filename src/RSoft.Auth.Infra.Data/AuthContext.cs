using Microsoft.EntityFrameworkCore;
using RSoft.Auth.Infra.Data.Configurations;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Lib.Design.Infra.Data;
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
        public AuthContext(DbContextOptions options) : base(options) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void SetTableConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppClientConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserAppClientConfiguration());
        }

        #endregion

        #region DbSets

        /// <summary>
        /// Roles dbset
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Application-Client dbset
        /// </summary>
        public virtual DbSet<AppClient> ApplicationClients { get; set; }

        /// <summary>
        /// Users dbset
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// User credentials dbset
        /// </summary>
        public virtual DbSet<UserCredential> Credentials { get; set; }

        /// <summary>
        /// User credential tokens dbset
        /// </summary>
        public virtual DbSet<UserCredentialToken> CredentialTokens { get; set; }

        /// <summary>
        /// User roles dbset
        /// </summary>
        public virtual DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// User Application-Client dbset
        /// </summary>
        public virtual DbSet<UserAppClient> UserApplicationClients { get; set; }

        #endregion

    }

}