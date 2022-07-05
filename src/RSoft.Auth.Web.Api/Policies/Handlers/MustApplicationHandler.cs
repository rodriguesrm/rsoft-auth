using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RSoft.Auth.Web.Api.Policies.Requirements;
using System;
using System.Threading.Tasks;

namespace RSoft.Auth.Web.Api.Policies.Handlers
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

            _ = Guid.TryParse(appKey, out Guid guidKey);

            if (guidKey != Guid.Empty)
            {

                if (requirement.AppKey == guidKey)
                    context.Succeed(requirement);

            }
            return Task.CompletedTask;
        }

        #endregion

    }
}
