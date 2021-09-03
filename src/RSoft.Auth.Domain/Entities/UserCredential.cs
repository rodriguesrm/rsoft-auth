using RSoft.Lib.Common.Contracts;
using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Design.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Entities
{

    /// <summary>
    /// User credential data entity
    /// </summary>
    public class UserCredential : EntityBase<UserCredential>, IEntity
    {

        #region Properties

        /// <summary>
        /// User id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// User Login (for persons)
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User password login
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Indicates that the user must change the password/access-key at the next login. Will be unable to use the system until
        /// </summary>
        public bool ChangeCredentials { get; set; }

        /// <summary>
        /// Quantity authentication failures
        /// </summary>
        public int AuthFailsQty { get; set; }

        /// <summary>
        /// Lockout deadline
        /// </summary>
        public DateTime? LockoutUntil { get; set; }

        #endregion

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override void Validate()
        {
            AddNotifications(new RequiredValidationContract<Guid?>(UserId, nameof(UserId), "User id is required").Contract.Notifications);
            AddNotifications(new SimpleStringValidationContract(Login, nameof(Login), false, 2, 254).Contract.Notifications);
            AddNotifications(new SimpleStringValidationContract(Password, nameof(Password), true, 32, 32).Contract.Notifications);
        }

        #endregion

    }

}
