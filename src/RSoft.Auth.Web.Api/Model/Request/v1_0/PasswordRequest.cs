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
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(254, ErrorMessage = "Login must contain a maximum of 254 characters")]
        public string Login { get; set; }

    }

}