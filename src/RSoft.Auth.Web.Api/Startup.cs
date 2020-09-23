using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Auth.Web.Api.Filters;
using RSoft.Framework.Web.Filters;
using RSoft.Framework.Web.Token.Extensions;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RSoft.Auth.Web.Api
{

    /// <summary>
    /// Provides methods to setup-up start-up application
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Create a new Startup instance
        /// </summary>
        /// <param name="configuration">Configuration object instance</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration object
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection object instance</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddControllers(opt => GlobalFilters.Configure(opt))
                .AddJsonOptions(opt => opt.JsonSerializerOptions.IgnoreNullValues = true)
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
            services.AddApiVersioning();
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddResponseCaching();

            services.AddJwtToken(Configuration);

            #region Swagger

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = @"'v'VVVV";
                p.SubstituteApiVersionInUrl = false;
            });
            services.AddSwaggerGen(c =>
            {

                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.EnableAnnotations();

                c.SwaggerDoc("v1.0",
                    new OpenApiInfo
                    {
                        Title = "RSoft Authentication API",
                        Version = "v1.0",
                        Description = "API for managing authentication and authorization roles.",
                        Contact = new OpenApiContact
                        {
                            Name = "Rodrigo Rodrigues",
                            Url = new Uri("https://github.com/rodriguesrm")
                        }
                    });

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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            #endregion

            services.AddAuthRegister(Configuration);
            services.AddMiddlewareLoggingOption(Configuration);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder object instance</param>
        /// <param name="env">IWebHostEnvironment object instance</param>
        /// <param name="provider">IApiVersionDescriptionProvider object instance</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToLowerInvariant());
                }

                options.DocExpansion(DocExpansion.List);

            });

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseResponseCaching();

            app.UseMiddleware<RequestResponseLogging<Startup>>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateDatabase();

        }
    }
}
