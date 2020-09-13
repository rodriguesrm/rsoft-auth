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
        /// User acess key (for applications)
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// User password login
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        #endregion

    }

}
