using RSoft.Lib.Common.Web.Models.Response;
using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Simple user response model
    /// </summary>
    public class SimpleUserResponse : EntityIdBaseResponse<Guid>
    {

        #region Constructors

        /// <summary>
        /// Create a new SimpleUserResponse instance
        /// </summary>
        /// <param name="id">User id key value</param>
        public SimpleUserResponse(Guid id) : base(id) { }

        #endregion

        #region Properties

        /// <summary>
        /// Full name
        /// </summary>
        public FullNameResponse Name { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        /// <example>ironman@marvel.com</example>
        public string Email { get; set; }

        #endregion
    }
}
