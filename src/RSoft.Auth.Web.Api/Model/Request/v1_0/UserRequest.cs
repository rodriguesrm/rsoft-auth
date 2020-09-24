using RSoft.Framework.Cross.Entities;

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
        public IFullName Name { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        /// <example>ironman@marvel.com</example>
        public string Email { get; set; }

        /// <summary>
        /// User active status
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

    }

}
