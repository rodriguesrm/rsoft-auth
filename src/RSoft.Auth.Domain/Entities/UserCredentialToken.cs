using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using RSoft.Lib.DDD.Domain.Entities;
using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Common.Abstractions;
using RSoft.Lib.Common.Contracts;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// Token entity for creating or retrieving credentials
    /// </summary>
    public class UserCredentialToken : EntityIdBase<Guid, UserCredentialToken>, IEntity
    {

        #region Constructors

        /// <summary>
        /// Create a new UserCredentialToken object instance
        /// </summary>
        public UserCredentialToken() : base() 
        {
            Initialize();
        }

        /// <summary>
        /// Create a new UserCredentialToken object instance
        /// </summary>
        /// <param name="id">Token id</param>
        public UserCredentialToken(Guid id) : base(id) 
        {
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// User key value
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Token creation date/time
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Token expiration date/time
        /// </summary>
        public DateTime ExpiresOn { get; set; }

        /// <summary>
        /// Indicates whether it is a first access credential token
        /// </summary>
        public bool FirstAccess { get; set; }

        #endregion

        #region Navigation Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        #endregion

        #region Local Methods

        /// <summary>
        /// Iniatialize objects/properties/fields with default values
        /// </summary>
        private void Initialize()
        {
            CreatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override void Validate()
        {
            IStringLocalizer<UserCredentialToken> localizer = ServiceActivator.GetScope().ServiceProvider.GetService<IStringLocalizer<UserCredentialToken>>();
            AddNotifications(new RequiredValidationContract<Guid?>(UserId, nameof(UserId), localizer["USER_REQUIRED"]).Contract.Notifications);
            if (ExpiresOn <= CreatedAt)
                AddNotification("Dates", localizer["EXPIRED_DATE_IN_PAST"]);
        }

        /// <summary>
        /// Indicates whether the token is valid
        /// </summary>
        public bool Expired()
            => ExpiresOn < DateTime.UtcNow;

        #endregion

    }
}
