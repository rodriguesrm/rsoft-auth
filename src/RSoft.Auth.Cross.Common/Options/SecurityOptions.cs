namespace RSoft.Auth.Cross.Common.Options
{

    /// <summary>
    /// Security options model configuration
    /// </summary>
    public class SecurityOptions
    {

        /// <summary>
        /// Secret key-password to user and application password security
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Lockout options
        /// </summary>
        public SecurityLockoutOptions Lockout { get; set; }

    }
}
