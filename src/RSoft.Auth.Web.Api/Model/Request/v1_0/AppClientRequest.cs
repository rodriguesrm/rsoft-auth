namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Application-Client request model
    /// </summary>
    public class AppClientRequest
    {

        #region Properties

        /// <summary>
        /// Application-Client name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether the application-client can log in as a service/application
        /// </summary>
        public bool? AllowLogin { get; set; } = false;

        /// <summary>
        /// Acope active status
        /// </summary>
        public bool? IsActive { get; set; } = true;

        #endregion

    }
}