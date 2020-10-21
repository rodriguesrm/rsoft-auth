using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Change password model
    /// </summary>
    public class ChangePasswordRequest
    {

        /// <summary>
        /// Login do usuário (cpf ou e-mail)
        /// </summary>
        [Required(ErrorMessage = "LOGIN_REQUIRED")]
        public string Login { get; set; }

        /// <summary>
        /// Current password
        /// </summary>
        [Required(ErrorMessage = "CURRENT_PASSWORD_REQUIRED")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required(ErrorMessage = "NEW_PASSWORD_REQUIRED")]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "PASSWORD_LENGTH")]
        public string NewPasword { get; set; }

    }

}
