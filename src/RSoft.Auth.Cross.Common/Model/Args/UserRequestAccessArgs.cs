using System;

namespace RSoft.Auth.Cross.Common.Model.Args
{
    public class UserRequestAccessArgs
    {

        #region Constructors

        /// <summary>
        /// Create a new command instance
        /// </summary>
        /// <param name="firstAccess">Indicates whether this is first access</param>
        /// <param name="name">Recipient's full name</param>
        /// <param name="email">Recipient's Email</param>
        /// <param name="token">Password creation/reset token</param>
        /// <param name="expireOn">Date and time the token will expire</param>
        /// <param name="urlCredential">URL of the credential creation/reset page</param>
        /// <param name="messageId">Message identificator</param>
        public UserRequestAccessArgs(bool firstAccess, string name, string email, Guid token, DateTime expireOn, string urlCredential, Guid? messageId)
        {
            FirstAccess = firstAccess;
            Name = name;
            Email = email;
            Token = token;
            ExpireOn = expireOn;
            UrlCredential = urlCredential;
            MessageId = messageId;
        }

        #endregion

        #region Request Data

        /// <summary>
        /// Message identificator
        /// </summary>
        public Guid? MessageId { get; private set; }

        /// <summary>
        /// Indicates whether this is first access
        /// </summary>
        public bool FirstAccess { get; private set; }

        /// <summary>
        /// Recipient's full name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Recipient's Email
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Password creation/reset token
        /// </summary>
        public Guid Token { get; private set; }

        /// <summary>
        /// Date and time the token will expire
        /// </summary>
        public DateTime ExpireOn { get; private set; }

        /// <summary>
        /// URL of the credential creation/reset page
        /// </summary>
        public string UrlCredential { get; private set; }

        #endregion

    }
}
