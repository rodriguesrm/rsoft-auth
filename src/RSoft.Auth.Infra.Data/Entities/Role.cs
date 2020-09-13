using RSoft.Framework.Cross.Entities;
using RSoft.Framework.Infra.Data;
using RSoft.Framework.Infra.Data.Tables;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// Roles of registered users
    /// </summary>
    public class Role : TableIdNameAuditBase<Guid, Role>, ITable, IAuditNavigation<Guid, User>, ISoftDeletion, IActive
    {

        #region Constructors

        /// <summary>
        /// Create a new role instance
        /// </summary>
        public Role() : base(Guid.NewGuid(), null)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new role instance
        /// </summary>
        /// <param name="id">role id value</param>
        public Role(Guid id) : base(id, null)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new role instance
        /// </summary>
        /// <param name="id">role id text</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        public Role(string id) : base()
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
        /// Role description
        /// </summary>
        public string Description { get; set; }

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
        /// Scope data
        /// </summary>
        public virtual Scope Scope { get; set; }

        /// <summary>
        /// Users for this role
        /// </summary>
        public virtual ICollection<UserRole> Users { get; set; }

        #endregion

        #region Local Methods

        private void Initialize()
        {
            IsActive = true;
            Users = new HashSet<UserRole>();
        }

        #endregion

    }

}
