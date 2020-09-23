namespace RSoft.Framework.Web.Options
{

    /// <summary>
    /// Jwt options configuration model
    /// </summary>
    public class JwtOptions
    {

        #region Properties

        /// <summary>
        /// JWT token issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Jwt token hash key
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Token recipient, represents the application that will use it.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Life time token minutes
        /// </summary>
        public int TimeLife { get; set; }

        #endregion

    }

}
