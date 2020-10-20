using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace RSoft.Auth.Web.Api.Languages
{

    /// <summary>
    /// Provider for determining the culture information of an Microsoft.AspNetCore.Http.HttpRequest
    /// </summary>
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {

        #region Properties

        /// <summary>
        /// Index of culture in route
        /// </summary>
        public int IndexOfCulture { get; set; }
        
        /// <summary>
        /// INdex of user interface culture in route
        /// </summary>
        public int IndexofUICulture { get; set; }

        #endregion

        /// <summary>
        /// Determine provider culture result in http context
        /// </summary>
        /// <param name="httpContext">Http context object instance</param>
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {

            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            string culture = null;
            string uiCulture = null;

            culture = uiCulture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();

            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);

        }
    }

}
