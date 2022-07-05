namespace RSoft.Auth.Web.Api.Policies
{

    /// <summary>
    /// Policy names constants
    /// </summary>
    public static class PolicyNames
    {

        /// <summary>
        /// Only me constant
        /// </summary>
        public const string OnlyThisApplication = "OnlyRSoftAuth";

        /// <summary>
        /// Only admin user or authorized service application
        /// </summary>
        public const string UserAdminOrAuthorizedService = "UserAdminOrAuthorizedService";

    }
}
