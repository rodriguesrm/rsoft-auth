using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Authentication request model
    /// </summary>
    public class AuthenticateRequest
    {

        /// <summary>
        /// User login
        /// </summary>
        /// <example>admin</example>
        [Required(ErrorMessage = "LOGIN_REQUIRED")]
        [MaxLength(254, ErrorMessage = "LOGIN_MAX_SIZE")]
        public string Login { get; set; }

        /// <summary>
        /// User password (base text)
        /// </summary>
        /// <example>admin123</example>
        [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        public string Password { get; set; }

    }

}
