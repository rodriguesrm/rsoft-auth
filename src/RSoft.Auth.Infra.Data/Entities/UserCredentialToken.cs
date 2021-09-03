using RSoft.Lib.Design.Infra.Data.Tables;
using System;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// Credential creation or retrieval token table
    /// </summary>
    public class UserCredentialToken : TableIdBase<Guid, UserCredentialToken>
    {

        #region Constructors

        /// <summary>
        /// Create a new UserCredentialToken instance
        /// </summary>
        public UserCredentialToken() : base(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Create a new UserCredentialToken instance
        /// </summary>
        /// <param name="id">Token id value</param>
        public UserCredentialToken(Guid id) : base(id)
        {
        }

        /// <summary>
        /// Create a new UserCredentialToken instance
        /// </summary>
        /// <param name="id">Token id text</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public UserCredentialToken(string id) : base()
        {
            Id = new Guid(id);
        }

        #endregion


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
