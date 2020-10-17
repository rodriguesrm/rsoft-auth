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
        [Required(ErrorMessage = "Token is required")]
        public Guid? Token { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "The password must contain 6 to 16 characters")]
        public string Password { get; set; }

    }

}
