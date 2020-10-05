using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RSoft.Framework.Web.Filters;
using RSoft.Framework.Web.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RSoft.Framework.Web.Extensions
{

    /// <summary>
    /// Provides extension methods for Swagger Documentation
    /// </summary>
    public static class SwaggerExtension
    {

        /// <summary>
        /// Adds the Swagger Documentation generator
        /// </summary>
        /// <param name="services">Service collection object instance</param>
        /// <param name="configuration">Configuration object instance</param>
        /// <param name="assemblyName">API application assembly name</param>
        public static IServiceCollection AddSwaggerGenerator(this IServiceCollection services, IConfiguration configuration, string assemblyName)
        {
            string[] versions = new string[] { "1.0" };
            return AddSwaggerGenerator(services, configuration, assemblyName, versions);
        }

        /// <summary>
        /// Adds the Swagger Documentation generator
        /// </summary>
        /// <param name="services">Service collection object instance</param>
        /// <param name="configuration">Configuration object instance</param>
        /// <param name="assemblyName">API application assembly name</param>
        /// <param name="versions">Array list API versions</param>
        public static IServiceCollection AddSwaggerGenerator(this IServiceCollection services, IConfiguration configuration, string assemblyName, params string[] versions)
        {

            bool isProd = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production);

            services.Configure<SwaggerOptions>(options => configuration.GetSection("Swagger").Bind(options));

            SwaggerOptions swaggerOptions = new SwaggerOptions();
            configuration.GetSection("Swagger").Bind(swaggerOptions);

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = @"'v'VVVV";
                p.SubstituteApiVersionInUrl = false;
            });
            services.AddSwaggerGen(c =>
            {

                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                c.OperationFilter<RemoveVersionParameterFilter>();
                c.OperationFilter<AddAppKeyHeaderParameter>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.EnableAnnotations();

                foreach (var version in versions)
                {
                    c.SwaggerDoc($"v{version}",
                    new OpenApiInfo
                    {
                        Title = swaggerOptions.Title,
                        Version = $"v{version}",
                        Description = swaggerOptions.Description,
                        Contact = new OpenApiContact
                        {
                            Name = swaggerOptions.Contact,
                            Url = new Uri(swaggerOptions.Uri)
                        }
                    });
                }


                if (!isProd)
                {

                    if (swaggerOptions.EnableTryOut && swaggerOptions.EnableJwtTokenAuthentication)
                    {
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = @"JWT Authorization header using the Bearer scheme. 
                                    <br>Enter 'Bearer' [space] and then your token in the text input below.
                                    <br>Example: 'Bearer 12345abcdef'",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"
                        });

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                        new OpenApiSecurityScheme
                          {
                            Reference = new OpenApiReference
                              {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                              },
                              Scheme = "oauth2",
                              Name = "Bearer",
                              In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                    }

                }

                var xmlFile = $"{assemblyName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            return services;

        }

        /// <summary>
        /// Enables the use of Swagger Documentation in a web interface
        /// </summary>
        /// <param name="app">IApplicationBuilder object instance</param>
        /// <param name="provider">IApiVersionDescriptionProvider object instance</param>
        public static IApplicationBuilder UseSwaggerDocUI(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {

            bool isProd = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production);

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint
                    (
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToLowerInvariant()
                    );
                    if (isProd)
                        options.SupportedSubmitMethods(new SubmitMethod[] { });
                }

                options.DocExpansion(DocExpansion.List);

            });

            return app;
        }

    }

}
