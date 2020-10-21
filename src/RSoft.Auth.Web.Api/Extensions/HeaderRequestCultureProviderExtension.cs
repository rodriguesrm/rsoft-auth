using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// Provider for determining the culture information of an Microsoft.AspNetCore.Http.HttpRequest
    /// </summary>
    public class HeaderRequestCultureProvider : RequestCultureProvider
    {

        #region Constructors

        /// <summary>
        /// Initialize a new instance of HeaderRequestCultureProvider
        /// </summary>
        /// <param name="defaultRequestCulture">System default language</param>
        /// <param name="supportedCultures">List of supported culture</param>
        public HeaderRequestCultureProvider(string defaultRequestCulture, IEnumerable<string> supportedCultures)
        {
            DefaultRequestCulture = defaultRequestCulture;
            SupportedCultures = supportedCultures;
        }

        #endregion

        #region Properties

        /// <summary>
        /// System default language
        /// </summary>
        public string DefaultRequestCulture { get; private set; }

        /// <summary>
        /// List of supported culture
        /// </summary>
        public IEnumerable<string> SupportedCultures { get; private set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Determine provider culture result in http context
        /// </summary>
        /// <param name="httpContext">Http context object instance</param>
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {

            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));
            
            string uiCulture;
            string culture = uiCulture = !string.IsNullOrWhiteSpace(httpContext.Request.Headers["Accept-Language"]) ? httpContext.Request.Headers["Accept-Language"].ToString() : DefaultRequestCulture;

            if (!SupportedCultures.Contains(culture))
                culture = uiCulture = DefaultRequestCulture;

            ProviderCultureResult providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);

        }

        #endregion

    }

}
