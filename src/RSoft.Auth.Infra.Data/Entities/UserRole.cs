using RSoft.Lib.Design.Infra.Data.Tables;
using System;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// User roles
    /// </summary>
    public class UserRole : TableBase<UserRole>, ITable
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

    }

}
