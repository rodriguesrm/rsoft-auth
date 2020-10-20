using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using RSoft.Auth.Cross.Common.Options;
using System.Linq;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// Language route constraint control object
    /// </summary>
    public class LanguageRouteConstraint : IRouteConstraint
    {
        //TODO: Move to framework.web

        #region Local object/variables

        private readonly CultureOptions _cultureOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of LanguageRouteConstraint
        /// </summary>
        /// <param name="options">Culture options parameters object</param>
        public LanguageRouteConstraint(IOptions<CultureOptions> options)
        {
            _cultureOptions = options?.Value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Check that the language entered in the route corresponds to a supported language.
        /// </summary>
        /// <param name="httpContext">Context http object</param>
        /// <param name="route">Router object</param>
        /// <param name="routeKey">String route key</param>
        /// <param name="values">Route values dictionary object</param>
        /// <param name="routeDirection">Route direction specification</param>
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (!values.ContainsKey("culture"))
                return false;

            string culture = values["culture"].ToString();
            return _cultureOptions.SupportedLanguage.Contains(culture);

        }

        #endregion

    }

}
