using RSoft.Framework.Infra.Data.Tables;
using System;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// Credential creation or retrieval token table
    /// </summary>
    public class UserCredentialToken : TableIdBase<Guid, UserCredentialToken>
    {

        #region Properties

        /// <summary>
        /// User key value
        /// </summary>
        public Guid UserId { get; set; }

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

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        #endregion

    }
}
