using RSoft.Framework.Cross.Entities;
using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using RSoft.Framework.Infra.Data;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// Scope of action
    /// </summary>
    public class Scope : EntityIdNameAuditBase<Guid, Scope>, IEntity, IAuditNavigation<Guid, User>, IActive
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
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

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
        public virtual ICollection<User> Users { get; set; }

        #endregion

        #region Local Methods

        private void Initialize()
        {
            IsActive = true;
            Roles = new HashSet<Role>();
            Users = new HashSet<User>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate entity
        /// </summary>
        public override void Validate()
        {
            //TODO: Globalization
            AddNotifications(new SimpleStringValidationContract(this.Name, nameof(Name), true, 3, 80).Contract.Notifications);
        }

        #endregion

    }

}
