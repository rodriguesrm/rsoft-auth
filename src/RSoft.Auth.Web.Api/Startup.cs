using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Auth.Web.Api.Extensions;
using RSoft.Auth.Web.Api.Helpers;
using RSoft.Auth.Web.Api.Policies;
using RSoft.Framework.Web.Extensions;
using RSoft.Framework.Web.Filters;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
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

            #region Language/Culture

            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    CultureOptions cultureOptions = new CultureOptions();
                    Configuration.GetSection("Application:Culture").Bind(cultureOptions);

                    IList<CultureInfo> supportedCultures = cultureOptions.SupportedLanguage.Select(c => new CultureInfo(c)).ToList();

                    options.DefaultRequestCulture = new RequestCulture(culture: cultureOptions.DefaultLanguage, uiCulture: cultureOptions.DefaultLanguage);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider { IndexOfCulture = 3, IndexofUICulture = 3 } };
                });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
            });

            #endregion

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

            #region Language/Culture

            IOptions<RequestLocalizationOptions> localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            #endregion

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{culture:culture}/{controller=Home}/{action=Index}/{id?}"); //Language/Culture
            });

            ILogger logger = factory.CreateLogger("Microsoft.Hosting.Lifetime");
            app.MigrateDatabase(logger);

        }
    }
}
