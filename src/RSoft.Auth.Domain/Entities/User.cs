﻿using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using RSoft.Lib.Design.Domain.Entities;
using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Common.Abstractions;
using RSoft.Lib.Common.Enums;
using RSoft.Lib.Common.Contracts;
using RSoft.Lib.Common.ValueObjects;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// User of the eco-system applications
    /// </summary>
    public class User : EntityIdAuditBase<Guid, User>, IEntity, IAuditAuthor<Guid>, IActive
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
        /// Document number (withou mask)
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        public Email Email { get; set; }

        /// <summary>
        /// User type
        /// </summary>
        public UserType? Type { get; set; }

        #endregion

        #region Navigation Lazy

        /// <summary>
        /// User credential data
        /// </summary>
        public virtual UserCredential Credential { get; set; }

        /// <summary>
        /// User roles list
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// User application-clients list
        /// </summary>
        public virtual ICollection<AppClient> ApplicationClients { get; set; }

        /// <summary>
        /// List of credential creation or retrieval tokens
        /// </summary>
        public virtual ICollection<UserCredentialToken> Tokens { get; set; }

        #endregion

        #region Local Methods

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
        private void Initialize()
        {
            IsActive = true;
            Roles = new HashSet<Role>();
            ApplicationClients = new HashSet<AppClient>();
            Tokens = new HashSet<UserCredentialToken>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate entity
        /// </summary>
        public override void Validate()
        {

            IStringLocalizer<User> localizer = ServiceActivator.GetScope().ServiceProvider.GetService<IStringLocalizer<User>>();
            if (CreatedAuthor != null) AddNotifications(CreatedAuthor.Notifications);
            if (ChangedAuthor != null) AddNotifications(ChangedAuthor.Notifications);
            AddNotifications(Name.Notifications);
            AddNotifications(Email.Notifications);
            AddNotifications(new RequiredValidationContract<string>(Email?.Address, $"Email.{nameof(Email.Address)}", localizer["EMAIL_REQUIRED"]).Contract.Notifications);
            AddNotifications(new RequiredValidationContract<UserType?>(Type, nameof(Type), localizer["USER_TYPE_REQUIRED"]).Contract.Notifications);
            AddNotifications(new PastDateValidationContract(BornDate, "Born date", localizer["BORN_DATE_REQUIRED"]).Contract.Notifications);

            if (Credential != null)
            {
                Credential.Validate();
                AddNotifications(Credential.Notifications);
            }

            if ((Type ?? UserType.User) == UserType.User)
            {
                AddNotifications(new BrasilianCpfValidationContract(Document, nameof(Document), true).Contract.Notifications);
                if (!ApplicationClients.Any())
                {
                    AddNotification(nameof(ApplicationClients), localizer["USER_IN_ONE_APPCLIENT"]);
                }
                else
                {
                    ApplicationClients
                        .ToList()
                        .ForEach(appClient =>
                        {
                            appClient.Validate();
                            AddNotifications(appClient.Notifications);
                        });
                }

                if (Roles != null && !Roles.Any())
                {
                    Roles
                        .ToList()
                        .ForEach(role =>
                        {
                            role.Validate();
                            AddNotifications(role.Notifications);
                        });
                }
            }

        }

        #endregion

    }

}
