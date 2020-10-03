using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace RSoft.Auth.Web.Api.Policies
{

    /// <summary>
    /// Must application requirement handler
    /// </summary>
    public class MustApplicationHandler : AuthorizationHandler<MustApplicationRequirement>
    {

        #region Local objects/variables

        private readonly IHttpContextAccessor _accessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new handler instance
        /// </summary>
        /// <param name="accessor">Http context acessor object</param>
        public MustApplicationHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustApplicationRequirement requirement)
        {

            string appKey = _accessor.HttpContext.Request.Headers["app-key"];
            string appAccess = _accessor.HttpContext.Request.Headers["app-access"];

            Guid.TryParse(appKey, out Guid guidKey);
            Guid.TryParse(appAccess, out Guid guidAccess);

            if (guidKey != Guid.Empty && guidAccess != Guid.Empty)
            {

                if (requirement.AppKey == guidKey && requirement.AppAccess == guidAccess)
                    context.Succeed(requirement);

            }
            return Task.CompletedTask;
        }

        #endregion

    }
}
