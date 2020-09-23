using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Model.Response.v1
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
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</example>
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
