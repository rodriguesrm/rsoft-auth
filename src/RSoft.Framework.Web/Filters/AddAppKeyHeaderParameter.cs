using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
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
        private readonly bool _isProd;

        #endregion

        #region Constructors

        public AddAppKeyHeaderParameter(IConfiguration configuration)
        {
            _isProd = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production);
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
                Description = "Application key id",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = !_isProd ? new OpenApiString(_appKey) : null
                },
                
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "app-access",
                Description = "Application key access",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = !_isProd ? new OpenApiString(_appAccess) : null
                },

            });

            #endregion

        }
    }

}
