using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Auth.Web.Api.Policies.Handlers;
using RSoft.Auth.Web.Api.Policies.Requirements;
using RSoft.Lib.Common.Options;

namespace RSoft.Auth.Web.Api.Policies
{

    /// <summary>
    /// Provide methods for register policies services
    /// </summary>
    public static class RegisterPolicies
    {

        /// <summary>
        /// Adds authorization policies to the service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration parameters object</param>
        public static IServiceCollection AddAppPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            AppClientOptions appClientOptions = new();
            configuration.GetSection("AppClient").Bind(appClientOptions);

            // Handlers
            services.AddSingleton<IAuthorizationHandler, MustApplicationHandler>();

            // Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.OnlyThisApplication, policy => 
                    policy
                        .Requirements
                        .Add(new MustApplicationRequirement(appClientOptions.ClientId, appClientOptions.ClientSecret))
                );
            });

            return services;
        }

    }
}
