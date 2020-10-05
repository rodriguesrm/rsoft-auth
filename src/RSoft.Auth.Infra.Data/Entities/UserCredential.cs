using RSoft.Framework.Infra.Data.Tables;
using System;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// User credential data entity
    /// </summary>
    public class UserCredential : TableBase<UserCredential>, ITable
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

    }

}
