﻿using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Design.Domain.Entities;
using RSoft.Lib.Common.Contracts;
using RSoft.Lib.Common.Abstractions;

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
            Initialize();
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
        /// Application-Client data
        /// </summary>
        public virtual AppClient AppClient { get; set; }

        /// <summary>
        /// Users for this role
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        #endregion

        #region Local Methods

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
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
            IStringLocalizer<Role> localizer = ServiceActivator.GetScope().ServiceProvider.GetService<IStringLocalizer<Role>>();
            if (CreatedAuthor != null) AddNotifications(CreatedAuthor.Notifications);
            if (ChangedAuthor != null) AddNotifications(ChangedAuthor.Notifications);
            AddNotifications(new RequiredValidationContract<Guid?>(AppClient?.Id, nameof(AppClient), localizer["APPCLIENT_REQUIRED"]).Contract.Notifications);
            AddNotifications(new SimpleStringValidationContract(Name, nameof(Name), true, 3, 50).Contract.Notifications);
            AddNotifications(new SimpleStringValidationContract(Description, nameof(Description), true, 3, 150).Contract.Notifications);
        }

        #endregion

    }

}
