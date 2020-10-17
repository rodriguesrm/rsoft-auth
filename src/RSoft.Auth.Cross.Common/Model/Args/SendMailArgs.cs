using System;

namespace RSoft.Auth.Cross.Common.Model.Args
{

    /// <summary>
    /// Email sending arguments object
    /// </summary>
    public class SendMailArgs
    {

        /// <summary>
        /// Indicates whether this is first access
        /// </summary>
        public bool FirstAccess { get; set; }

        /// <summary>
        /// Recipient's full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recipient's Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password creation/reset token
        /// </summary>
        public Guid Token { get; set; }

    }

}
