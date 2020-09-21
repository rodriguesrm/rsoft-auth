using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Model.Response
{

    /// <summary>
    /// Authentication response model
    /// </summary>
    public class AuthenticateResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new AuthenticateResponse instance
        /// </summary>
        /// <param name="token">Generated token</param>
        /// <param name="expirationDate">Token expiration date/time</param>
        /// <param name="roles">Roles list</param>
        /// <param name="user">User details</param>
        public AuthenticateResponse(string token, DateTime? expirationDate, IEnumerable<string> roles, UserResponse user)
        {
            Token = token;
            ExpirationDate = expirationDate;
            Roles = roles;
            User = user;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Generated token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Token expiration date/time
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Roles list
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// User details
        /// </summary>
        public UserResponse User { get; set; }

        #endregion

    }

}
