namespace RSoft.Auth.Cross.Common.Options
{

    /// <summary>
    /// Application credential token options parameters
    /// </summary>
    public class CredentialTokenOptions
    {

        /// <summary>
        /// Token validity time (in minutes)
        /// </summary>
        public int TimeLife { get; set; } = 15;

    }
}
