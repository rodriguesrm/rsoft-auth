namespace RSoft.Auth.Cross.Common.Options
{

    /// <summary>
    /// Security lockout options parameters model
    /// </summary>
    public class SecurityLockoutOptions
    {

        #region Properties

        /// <summary>
        /// Number of failed login attempts
        /// </summary>
        public int Times { get; set; }

        /// <summary>
        /// Blocking minutes
        /// </summary>
        public int Minutes { get; set; }

        #endregion

    }
}
