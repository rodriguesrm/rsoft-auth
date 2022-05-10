using RSoft.Lib.Design.Infra.Data.Tables;
using System;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// User Application-Client
    /// </summary>
    public class UserAppClient : TableBase<UserAppClient>, ITable
    {

        #region Properties

        /// <summary>
        /// User id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Application-Client id
        /// </summary>
        public Guid? AppClientId { get; set; }

        #endregion

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Application-Client data
        /// </summary>
        public virtual AppClient ApplicationClient { get; set; }

        #endregion

    }

}
