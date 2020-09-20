using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request
{

    /// <summary>
    /// Authentication request model
    /// </summary>
    public class AuthenticateRequest
    {

        /// <summary>
        /// User login
        /// </summary>
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(255, ErrorMessage = "The login must contain a maximum of 254 characters")]
        public string Login { get; set; }

        /// <summary>
        /// User password (base text)
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }

}
