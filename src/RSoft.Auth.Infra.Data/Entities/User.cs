using RSoft.Framework.Cross.Entities;
using RSoft.Framework.Infra.Data;
using RSoft.Framework.Infra.Data.Tables;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// User of the eco-system applications
    /// </summary>
    public class User : TableIdAuditBase<Guid, User>, ITable, IAuditNavigation<Guid, User>, ISoftDeletion, IActive, IFullName
    {

        #region Constructors

        /// <summary>
        /// Create a new user instance
        /// </summary>
        public User() : base(Guid.NewGuid())
        {
            Initialize();
        }

        /// <summary>
        /// Create a new user instance
        /// </summary>
        /// <param name="id">User id value</param>
        public User(Guid id) : base(id)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new user instance
        /// </summary>
        /// <param name="id">User id text</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public User(string id) : base()
        {
            Id = new Guid(id);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Soft deletion
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        public string Email { get; set; }

        #endregion

        #region Navigation Lazy

        /// <summary>
        /// User credential data
        /// </summary>
        public virtual UserCredential Credential { get; set; }

        /// <summary>
        /// Created author data
        /// </summary>
        public virtual User CreatedAuthor { get; set; }

        /// <summary>
        /// Changed author data
        /// </summary>
        public virtual User ChangedAuthor { get; set; }

        /// <summary>
        /// User roles list
        /// </summary>
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// User scopes list
        /// </summary>
        public virtual ICollection<UserScope> Scopes { get; set; }

        /// <summary>
        /// List of Tokens for creating or retrieving credentials
        /// </summary>
        public virtual ICollection<UserCredentialToken> Tokens { get; set; }

        #region Navigation Audit

        /// <summary>
        /// Created users
        /// </summary>
        public virtual ICollection<User> CreatedUsers { get; set; }
        
        /// <summary>
        /// Changed users
        /// </summary>
        public virtual ICollection<User> ChangedUsers { get; set; }
        
        /// <summary>
        /// Created roles
        /// </summary>
        public virtual ICollection<Role> CreatedRoles { get; set; }
        
        /// <summary>
        /// Changed roles
        /// </summary>
        public virtual ICollection<Role> ChangedRoles { get; set; }
        
        /// <summary>
        /// Created scopes
        /// </summary>
        public virtual ICollection<Scope> CreatedScopes { get; set; }
        
        /// <summary>
        /// Changed scopes
        /// </summary>
        public virtual ICollection<Scope> ChangedScopes { get; set; }

        #endregion

        #endregion

        #region Local Methods

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
        private void Initialize()
        {
            IsActive = true;
            Roles = new HashSet<UserRole>();
            Scopes = new HashSet<UserScope>();
            Tokens = new HashSet<UserCredentialToken>();
            CreatedUsers = new HashSet<User>();
            ChangedUsers = new HashSet<User>();
            CreatedRoles = new HashSet<Role>();
            ChangedRoles = new HashSet<Role>();
            CreatedScopes = new HashSet<Scope>();
            ChangedScopes = new HashSet<Scope>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get full name
        /// </summary>
        public string GetFullName()
        {
            return $"{FirstName ?? string.Empty} {LastName ?? string.Empty}";
        }

        #endregion

    }

}
