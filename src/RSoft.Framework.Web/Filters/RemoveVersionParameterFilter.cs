using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace RSoft.Framework.Web.Filters
{

    /// <summary>
    /// Remove version swagger version-parameter filter
    /// </summary>
    public class RemoveVersionParameterFilter : IOperationFilter
    {

        /// <summary>
        /// Apply filter 
        /// </summary>
        /// <param name="operation">OpenApiOperation object instance</param>
        /// <param name="context">OpenFilterContext context object instance</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }

}
