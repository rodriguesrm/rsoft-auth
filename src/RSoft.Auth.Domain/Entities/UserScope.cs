using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// User Scopes
    /// </summary>
    public class UserScope : EntityBase<UserScope>, IEntity
    {

        #region Properties

        /// <summary>
        /// User id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Scope id
        /// </summary>
        public Guid? ScopeId { get; set; }

        #endregion

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Scope data
        /// </summary>
        public virtual Scope Scope { get; set; }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override void Validate()
        {
            AddNotifications(new RequiredValidationContract<Guid?>(UserId, nameof(UserId), "User id is required").Contract.Notifications);
            AddNotifications(new RequiredValidationContract<Guid?>(ScopeId, nameof(ScopeId), "Scope id is required").Contract.Notifications);
        }

        #endregion

    }

}
