using System;
using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request.v1
{

    /// <summary>
    /// Credential creation model (first access)
    /// </summary>
    public class CreateCredentialRequest
    {

        /// <summary>
        /// Token id
        /// </summary>
        [Required(ErrorMessage = "Token id is required")]
        public Guid? TokenId { get; set; }

        /// <summary>
        /// Senha da pessoa
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "The password must contain 6 to 16 characters")]
        public string Password { get; set; }

    }

}
