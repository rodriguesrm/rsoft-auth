using RSoft.Framework.Cross.Entities;
using RSoft.Framework.Web.Model.Response;
using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Simple user response model
    /// </summary>
    public class UserResponse : EntityIdBaseResponse<Guid>
    {

        #region Constructors

        /// <summary>
        /// Create a new UserResponse instance
        /// </summary>
        /// <param name="id">User id key value</param>
        public UserResponse(Guid id) : base(id) { }

        #endregion

        #region Properties

        /// <summary>
        /// Full name
        /// </summary>
        public IFullName Name { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        /// <example>ironman@marvel.com</example>
        public string Email { get; set; }

        #endregion
    }
}
