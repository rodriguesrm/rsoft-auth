using Microsoft.AspNetCore.Authorization;
using System;

namespace RSoft.Auth.Web.Api.Policies.Requirements
{

    /// <summary>
    /// Access request policy for Admin users or authorized applications
    /// </summary>
    public class UserAdminOrAuthorizedServiceRequirement : IAuthorizationRequirement
    {

        #region Constructors

        /// <summary>
        /// Create a new requirement instance
        /// </summary>
        /// <param name="appKey">Application id key</param>
        public UserAdminOrAuthorizedServiceRequirement(Guid appKey)
        {
            AppKey = appKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Application id key
        /// </summary>
        public Guid AppKey { get; private set; }

        #endregion

    }
}
