using RSoft.Framework.Cross.Entities;
using RSoft.Framework.Cross.Enums;
using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using RSoft.Framework.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// User scopes list
        /// </summary>
        public virtual ICollection<Scope> Scopes { get; set; }

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
            Scopes = new HashSet<Scope>();
            Tokens = new HashSet<UserCredentialToken>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate entity
        /// </summary>
        public override void Validate()
        {

            //TODO: Globalization
            if (CreatedAuthor != null) AddNotifications(CreatedAuthor.Notifications);
            if (ChangedAuthor != null) AddNotifications(ChangedAuthor.Notifications);
            AddNotifications(Name.Notifications);
            AddNotifications(Email.Notifications);
            AddNotifications(new RequiredValidationContract<string>(Email?.Address, $"Email.{nameof(Email.Address)}", "Email is required").Contract.Notifications);
            AddNotifications(new RequiredValidationContract<UserType?>(Type, nameof(Type), "User type is required").Contract.Notifications);
            AddNotifications(new PastDateValidationContract(BornDate, "Born date", "Burn date is required").Contract.Notifications);

            if (Credential != null)
            {
                Credential.Validate();
                AddNotifications(Credential.Notifications);
            }

            if (!Scopes.Any())
            {
                AddNotification(nameof(Scopes), "The user must be assigned to at least one scope");
            }
            else
            {
                Scopes
                    .ToList()
                    .ForEach(scope =>
                    {
                        scope.Validate();
                        AddNotifications(scope.Notifications);
                    });
            }

            if (!Roles.Any())
            {
                AddNotification(nameof(Roles), "The user must have at least one role");
            }
            else
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

        #endregion

    }

}
