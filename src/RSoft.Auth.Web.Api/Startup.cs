using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Framework.Web.Extensions;
using RSoft.Framework.Web.Filters;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
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

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            services.AddJwtToken(Configuration);
            services.AddSwaggerGenerator(Configuration, assemblyName);
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
