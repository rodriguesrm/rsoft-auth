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
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        /// <summary>
        /// Current password
        /// </summary>
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = "The password must contain 6 to 16 characters.")]
        public string NewPasword { get; set; }

    }

}
