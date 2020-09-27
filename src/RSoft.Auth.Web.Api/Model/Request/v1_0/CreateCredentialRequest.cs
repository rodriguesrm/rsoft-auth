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
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(ErrorMessage = "Login must contain a maximum of 254 characters")]
        public string Login { get; set; }

    }

}
