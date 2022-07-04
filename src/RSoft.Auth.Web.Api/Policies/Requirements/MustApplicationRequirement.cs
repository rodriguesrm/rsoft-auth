using Microsoft.AspNetCore.Authorization;
using System;

namespace RSoft.Auth.Web.Api.Policies.Requirements
{

    /// <summary>
    /// Access request policy for a given application
    /// </summary>
    public class MustApplicationRequirement : IAuthorizationRequirement
    {

        #region Constructors

        /// <summary>
        /// Creaqte a new requirement instance
        /// </summary>
        /// <param name="appKey">Application id key</param>
        public MustApplicationRequirement(Guid appKey)
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
