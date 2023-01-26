using Newtonsoft.Json;
using System;

namespace RSoft.Auth.Cross.Common.Model
{

    /// <summary>
    /// Token for application model
    /// </summary>
    public class TokenApplication
    {

        #region Properties

        /// <summary>
        /// Authentication token (application)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiration date/time token
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        #endregion

    }
}
