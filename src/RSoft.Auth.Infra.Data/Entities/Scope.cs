using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Design.Infra.Data;
using RSoft.Lib.Design.Infra.Data.Tables;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// Scope of action
    /// </summary>
    public class Scope : TableIdNameAuditBase<Guid, Scope>, ITable, IAuditNavigation<Guid, User>, ISoftDeletion, IActive
    {

        #region Constructors

        /// <summary>
        /// Create a new application scope instance
        /// </summary>
        public Scope() : base(Guid.NewGuid(), null)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new application scope instance
        /// </summary>
        /// <param name="id">application scope id value</param>
        public Scope(Guid id) : base(id, null)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new application scope instance
        /// </summary>
        /// <param name="id">application scope id text</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public Scope(string id) : base()
        {
            Id = new Guid(id);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Application access key word
        /// </summary>
        public Guid AccessKey { get; set; }

        /// <summary>
        /// Indicates whether the scope can log in as a service/application
        /// </summary>
        public bool AllowLogin { get; set; }

        /// <summary>
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Soft deletion
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Navigation Lazy

        /// <summary>
        /// Created author data
        /// </summary>
        public virtual User CreatedAuthor { get; set; }

        /// <summary>
        /// Changed author data
        /// </summary>
        public virtual User ChangedAuthor { get; set; }

        /// <summary>
        /// Roles list
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// Users list
        /// </summary>
        public virtual ICollection<UserScope> Users { get; set; }

        #endregion

        #region Local Methods

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
        private void Initialize()
        {
            IsActive = true;
            Roles = new HashSet<Role>();
            Users = new HashSet<UserScope>();
        }

        #endregion

    }

}
