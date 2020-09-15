﻿using RSoft.Framework.Cross.Entities;
using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// Roles of registered users
    /// </summary>
    public class Role : EntityIdNameAuditBase<Guid, Role>, IEntity, IAuditAuthor<Guid>, IActive
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
        /// Role description
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region Navigation Lazy

        /// <summary>
        /// Scope data
        /// </summary>
        public virtual Scope Scope { get; set; }

        /// <summary>
        /// Users for this role
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        #endregion

        #region Local Methods

        private void Initialize()
        {
            IsActive = true;
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
            AddNotifications(CreatedAuthor?.Notifications);
            AddNotifications(ChangedAuthor?.Notifications);
            AddNotifications(new SimpleStringValidationContract(Name, nameof(Name), true, 3, 50).Contract.Notifications);
            AddNotifications(new SimpleStringValidationContract(Description, nameof(Description), true, 3, 150).Contract.Notifications);
        }

        #endregion

    }

}
