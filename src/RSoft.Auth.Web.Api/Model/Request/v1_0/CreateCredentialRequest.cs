using System;
using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Credential creation model (first access)
    /// </summary>
    public class CreateCredentialRequest : ResetPasswordRequest
    {

        /// <summary>
        /// Login / user name
        /// </summary>
        [Required(ErrorMessage = "LOGIN_REQUIRED")]
        [MaxLength(ErrorMessage = "LOGIN_MAX_SIZE")]
        public string Login { get; set; }

    }

}
