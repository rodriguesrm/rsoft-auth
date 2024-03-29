using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Auth.Web.Api.Extensions;
using RSoft.Auth.Web.Api.Helpers;
using RSoft.Auth.Web.Api.Language;
using RSoft.Auth.Web.Api.Policies;
using RSoft.Lib.Common.Web.Extensions;
using RSoft.Lib.Common.Web.Filters;
using RSoft.Lib.Web.Extensions;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
using System.Text.Json.Serialization;
using System;
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
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        return factory.Create(typeof(Resource));
                    };
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    opt.JsonSerializerOptions.WriteIndented = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
                })
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
            services.AddApiVersioning();
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddResponseCaching();

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            services.AddJwtToken(Configuration);
            services.AddSwaggerGenerator(Configuration, assemblyName);
            services.AddAuthRegister(Configuration);
            services.AddAppPolicies(Configuration);
            services.AddMiddlewareLoggingOption(Configuration);
            services.AddApplicationHealthChecks(Configuration);
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddCultureLanguage(Configuration);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder object instance</param>
        /// <param name="env">IWebHostEnvironment object instance</param>
        /// <param name="provider">IApiVersionDescriptionProvider object instance</param>
        /// <param name="factory">Logger factory</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory factory)
        {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseResponseCaching();

            app.UseMiddleware<RequestResponseLogging<Startup>>();
            app.UseSwaggerDocUI(provider);
            app.UseApplicationHealthChecks();

            //ServiceActivator.Configure(app.ApplicationServices);
            app.ConfigureLangague();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ILogger logger = factory.CreateLogger("Microsoft.Hosting.Lifetime");
            app.MigrateDatabase(logger);

        }
    }
}
