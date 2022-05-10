using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Common.Enums;
using RSoft.Lib.Design.Infra.Data;
using RSoft.Lib.Design.Infra.Data.Tables;
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
        /// Document number (withou mask)
        /// </summary>
        public string Document { get; set; }

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

        /// <summary>
        /// User type
        /// </summary>
        public UserType Type { get; set; }

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
        /// User application-clients list
        /// </summary>
        public virtual ICollection<UserAppClient> ApplicationClients { get; set; }

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
        /// Created application-clients
        /// </summary>
        public virtual ICollection<AppClient> CreatedAppClients { get; set; }
        
        /// <summary>
        /// Changed application-clients
        /// </summary>
        public virtual ICollection<AppClient> ChangedAppClients { get; set; }

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
            ApplicationClients = new HashSet<UserAppClient>();
            Tokens = new HashSet<UserCredentialToken>();
            CreatedUsers = new HashSet<User>();
            ChangedUsers = new HashSet<User>();
            CreatedRoles = new HashSet<Role>();
            ChangedRoles = new HashSet<Role>();
            CreatedAppClients = new HashSet<AppClient>();
            ChangedAppClients = new HashSet<AppClient>();
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
