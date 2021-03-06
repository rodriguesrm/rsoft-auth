﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Auth.Web.Api.Policies.Handlers;
using RSoft.Auth.Web.Api.Policies.Requirements;

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

            ScopeOptions scopeOptions = new ScopeOptions();
            configuration.GetSection("Scope").Bind(scopeOptions);

            // Handlers
            services.AddSingleton<IAuthorizationHandler, MustApplicationHandler>();

            // Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.OnlyThisApplication, policy => 
                    policy
                        .Requirements
                        .Add(new MustApplicationRequirement(scopeOptions.Key, scopeOptions.Access))
                );
            });

            return services;
        }

    }
}
