﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Auth.Infra.Data;
using RSoft.Auth.Infra.Data.Repositories;
using RSoft.Auth.Domain.Repositories;
using RSoft.Auth.Domain.Services;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Lib.Design.IoC;
using RSoft.Lib.Common.Options;
using RSoft.Lib.Design.Infra.Data;
using RSoft.Lib.Messaging.Options;
using RSoft.Lib.Messaging.Abstractions;
using MassTransit.RabbitMqTransport;
using System;

namespace RSoft.Auth.Cross.IoC
{

    /// <summary>
    /// Dependency injection register
    /// </summary>
    public static class DependencyInjection
    {

        #region Public methods

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="aditionalConfig">Action to adicional configuration for messaging</param>
        public static IServiceCollection AddAuthRegister
        (
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IRabbitMqBusFactoryConfigurator> aditionalConfig = null
        )
        {

            // Add Framework services
            services.AddRSoftRegister<AuthContext>(configuration, true);

            // Messaging
            services.AddMassTransitUsingRabbitMq(configuration, cfg => aditionalConfig?.Invoke(cfg));

            // Options
            services.Configure<SecurityOptions>(options => configuration.GetSection("Application:Security").Bind(options));
            services.Configure<RSApiOptions>(options => configuration.GetSection("Application:RSoftApis").Bind(options));
            services.Configure<CredentialOptions>(options => configuration.GetSection("Application:Credential").Bind(options));
            services.Configure<CultureOptions>(options => configuration.GetSection("Application:Culture").Bind(options));
            services.Configure<PagesOptions>(options => configuration.GetSection("Application:Pages").Bind(options));
            services.Configure<MessagingOption>(options => configuration.GetSection("Messaging:Server").Bind(options));
            services.Configure<AppClientOptions>(options => configuration.GetSection("AppClient").Bind(options));

            // Infra Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IUserCredentialTokenRepository, UserCredentialTokenRepository>();
            services.AddScoped<IAppClientRepository, AppClientRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // Domain
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IAppClientDomainService, AppClientDomainService>();
            services.AddScoped<IRoleDomainService, RoleDomainService>();

            // Application
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<ICredentialAppService, CredentialAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IAppClientAppService, AppClientAppService>();
            services.AddScoped<IUserRequestAccessAppService, UserRequestAccessAppService>();

            return services;

        }

        #endregion

    }

}
