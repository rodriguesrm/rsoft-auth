using RSoft.Lib.Common.Contracts;
using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Design.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// Application Client
    /// </summary>
    public class AppClient : EntityIdNameAuditBase<Guid, AppClient>, IEntity, IAuditAuthor<Guid>, IActive
    {

        #region Constructors

        /// <summary>
        /// Create a new application client instance
        /// </summary>
        public AppClient() : base(Guid.NewGuid(), null)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new application client instance
        /// </summary>
        /// <param name="id">application client id value</param>
        public AppClient(Guid id) : base(id, null)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new application client instance
        /// </summary>
        /// <param name="id">application client id text</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public AppClient(string id) : base()
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
        /// Indicates whether the application client can log in as a service/application
        /// </summary>
        public bool AllowLogin { get; set; }

        /// <summary>
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region Navigation Lazy

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

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
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
            if (CreatedAuthor != null) AddNotifications(CreatedAuthor.Notifications);
            if (ChangedAuthor != null) AddNotifications(ChangedAuthor.Notifications);
            AddNotifications(new SimpleStringValidationContract(Name, nameof(Name), true, 3, 80).Contract.Notifications);
        }

        #endregion

    }

}
