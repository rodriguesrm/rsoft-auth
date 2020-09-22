using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Framework.Web.Filters;
using RSoft.Framework.Web.Token.Extensions;
using RSoft.Logs.Extensions;
using RSoft.Logs.Middleware;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
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
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddResponseCaching();

            services.AddJwtToken(Configuration);

            #region Swagger

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "RSoft Authentication API",
                        Version = "v1",
                        Description = "API for managing authentication and authorization roles.",
                        Contact = new OpenApiContact
                        {
                            Name = "Rodrigo Rodrigues",
                            Url = new Uri("https://github.com/rodriguesrm")
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Authentication v1.0");
                c.SupportedSubmitMethods(new SubmitMethod[] { });
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
