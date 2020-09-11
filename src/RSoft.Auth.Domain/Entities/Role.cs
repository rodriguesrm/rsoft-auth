using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// Roles of registered users
    /// </summary>
    public class Role : EntityIdNameAuditBase<Guid, Role>, IEntity, IAuditNavigation<Guid, User>, ISoftDeletion, IActive
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
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
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

        #region Public Methods

        /// <summary>
        /// Validate entity
        /// </summary>
        public override void Validate()
        {
            //TODO: Globalization
            AddNotifications(new SingleStringValidationContract(Name, nameof(Name), true, 3, 50).Contract.Notifications);
            AddNotifications(new SingleStringValidationContract(Description, nameof(Description), true, 3, 150).Contract.Notifications);
        }

        #endregion

    }

}
