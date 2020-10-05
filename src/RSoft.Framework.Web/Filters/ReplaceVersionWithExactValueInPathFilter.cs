using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RSoft.Framework.Web.Filters
{

    /// <summary>
    /// Replace version parameter to route-version object filter
    /// </summary>
    public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {

        /// <summary>
        /// Apply filter 
        /// </summary>
        /// <param name="swaggerDoc">OpenApiDocument object instance</param>
        /// <param name="context">DocumentFilterContext context object instance</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = paths;
        }
    }

}
