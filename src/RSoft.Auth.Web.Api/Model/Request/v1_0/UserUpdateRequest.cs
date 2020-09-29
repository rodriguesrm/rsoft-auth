using RSoft.Framework.Cross.Enums;
using System;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// User update request model
    /// </summary>
    public class UserUpdateRequest
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
        /// User type (1-Person | 2-Application/Service)
        /// </summary>
        public UserType? Type { get; set; }

        /// <summary>
        /// User active status
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

    }

}
