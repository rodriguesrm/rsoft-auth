using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RSoft.Auth.Web.Api.Policies.Requirements;
using RSoft.Lib.Common.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RSoft.Auth.Web.Api.Policies.Handlers
{

    /// <summary>
    /// User admin or authorized service requirement handler
    /// </summary>
    public class UserAdminOrAuthorizedServiceHandler : AuthorizationHandler<UserAdminOrAuthorizedServiceRequirement>
    {

        #region Local objects/variables

        private readonly IHttpContextAccessor _accessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new handler instance
        /// </summary>
        /// <param name="accessor">Http context acessor object</param>
        public UserAdminOrAuthorizedServiceHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Check if user is authorized
        /// </summary>
        /// <param name="appKey">Application id key</param>
        /// <param name="requirement">Requirement policy</param>
        private bool IsAuthorized(Guid appKey, UserAdminOrAuthorizedServiceRequirement requirement)
        {

            Claim userType = _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);

            if (_accessor.HttpContext.User.IsInRole(appKey.ToString()) && userType?.Value == nameof(UserType.Service))
                return true;

            if (appKey == requirement.AppKey && _accessor.HttpContext.User.IsInRole("admin") && userType?.Value == nameof(UserType.User))
                return true;

            return false;

        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAdminOrAuthorizedServiceRequirement requirement)
        {

            string appKey = _accessor.HttpContext.Request.Headers["app-key"];

            _ = Guid.TryParse(appKey, out Guid guidKey);

            if (guidKey != Guid.Empty)
            {

                if (IsAuthorized(guidKey, requirement))
                    context.Succeed(requirement);

            }
            return Task.CompletedTask;

        }

        #endregion

    }
}
