namespace RSoft.Auth.Cross.Common.Options
{

    /// <summary>
    /// External api options parameters
    /// </summary>
    public class RSApiOptions
    {

        /// <summary>
        /// Service Authentication parameters options
        /// </summary>
        public RSApiDetailOptions Auth { get; set; }

        /// <summary>
        /// Service Mail parameters options
        /// </summary>
        public RSApiDetailOptions Mail { get; set; }


    }
}
