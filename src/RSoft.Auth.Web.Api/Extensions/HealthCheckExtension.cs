using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RSoft.Auth.Cross.Common.Options;
using System;

namespace RSoft.Auth.Web.Api.Extensions
{

    /// <summary>
    /// Provides extension methods for Health Checks
    /// </summary>
    public static class HealthCheckExtension
    {

        /// <summary>
        /// Adds points to application health checks
        /// </summary>
        /// <param name="services">Services collection object</param>
        /// <param name="configuration">Configuration object</param>
        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {

            var apiOptions = new RSApiOptions();
            configuration.GetSection("Application:RSoftApis").Bind(apiOptions);

            services.AddHealthChecks()
                    .AddUrlGroup(
                                new Uri(apiOptions.Mail.Uri),
                                name: "Access the Email Sending API",
                                failureStatus: HealthStatus.Degraded,
                                tags: new string[] { "rsoft.mail.service" })
                    .AddMySql(
                        connectionString: configuration.GetValue<string>("ConnectionStrings:DbServer"),
                                name: "MySqlDb",
                                failureStatus: HealthStatus.Unhealthy,
                                timeout: TimeSpan.FromSeconds(15),
                                tags: new string[] { "mysqldb" })

            ;

            return services;

        }

    }
}
