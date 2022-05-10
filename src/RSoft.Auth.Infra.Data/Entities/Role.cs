using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Design.Infra.Data;
using RSoft.Lib.Design.Infra.Data.Tables;
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

        /// <summary>
        /// Application-Client id value
        /// </summary>
        public Guid ApplicationClientId { get; set; }

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
        /// Application-client data
        /// </summary>
        public virtual AppClient AppClient { get; set; }

        /// <summary>
        /// Users for this role
        /// </summary>
        public virtual ICollection<UserRole> Users { get; set; }

        #endregion

        #region Local Methods

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
        private void Initialize()
        {
            IsActive = true;
            Users = new HashSet<UserRole>();
        }

        #endregion

    }

}
