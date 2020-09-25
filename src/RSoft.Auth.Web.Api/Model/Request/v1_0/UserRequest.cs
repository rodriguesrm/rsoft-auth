using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// User request model
    /// </summary>
    public class UserRequest
    {

        #region Properties

        /// <summary>
        /// Full name
        /// </summary>
        public FullNameRequest Name { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        /// <example>ironman@marvel.com</example>
        public string Email { get; set; }

        /// <summary>
        /// User active status
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User roles ids
        /// </summary>
        public IEnumerable<Guid> Roles { get; set; }

        /// <summary>
        /// User scopes ids
        /// </summary>
        public IEnumerable<Guid> Scopes { get; set; }

        #endregion

    }

}
