using System.ComponentModel.DataAnnotations;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Password information request model
    /// </summary>
    public class PasswordRequest
    {

        /// <summary>
        /// User login (email
        /// </summary>
        [Required(ErrorMessage = "LOGIN_REQUIRED")]
        [MaxLength(254, ErrorMessage = "LOGIN_MAX_SIZE")]
        public string Login { get; set; }

    }

}