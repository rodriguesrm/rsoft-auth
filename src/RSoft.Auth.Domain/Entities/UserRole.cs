using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// User roles
    /// </summary>
    public class UserRole : EntityBase<UserRole>, IEntity
    {

        #region Properties

        /// <summary>
        /// User id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Role id
        /// </summary>
        public Guid? RoleId { get; set; }

        #endregion

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Role data
        /// </summary>
        public virtual Role Role { get; set; }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override void Validate()
        {
            AddNotifications(new RequiredValidationContract<Guid?>(UserId, nameof(UserId), "User id is required").Contract.Notifications);
            AddNotifications(new RequiredValidationContract<Guid?>(RoleId, nameof(RoleId), "Role id is required").Contract.Notifications);
        }

        #endregion

    }

}
