using System;
using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Credential recovery model (forgot password)
    /// </summary>
    public class ResetPasswordRequest
    {

        /// <summary>
        /// Generated token to create credentials
        /// </summary>
        [Required(ErrorMessage = "TOKEN_PASSWORD_REQUIRED")]
        public Guid? Token { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "PASSWORD_LENGTH")]
        public string Password { get; set; }

    }

}
