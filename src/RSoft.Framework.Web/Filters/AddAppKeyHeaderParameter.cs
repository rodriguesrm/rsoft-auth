using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace RSoft.Framework.Web.Filters
{

    /// <summary>
    /// Add required header parameter in swagger user-interface
    /// </summary>
    public class AddAppKeyHeaderParameter : IOperationFilter
    {

        #region Local objects/variables

        private readonly string _appKey;
        private readonly string _appAccess;

        #endregion

        #region Constructors

        public AddAppKeyHeaderParameter(IConfiguration configuration)
        {
            _appKey = configuration["Scope:Key"];
            _appAccess = configuration["Scope:Access"];
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "app-key",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString(_appKey)
                },
                
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "app-access",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString(_appAccess)
                },

            });

            #endregion

        }
    }

}
