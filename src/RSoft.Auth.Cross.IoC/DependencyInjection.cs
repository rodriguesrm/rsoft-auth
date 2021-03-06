﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Framework.Cross.IoC;
using RSoft.Auth.Infra.Data;
using RSoft.Auth.Infra.Data.Repositories;
using RSoft.Auth.Domain.Repositories;
using RSoft.Auth.Domain.Services;
using RSoft.Auth.Application.Services;
using RSoft.Framework.Infra.Data;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Framework.Options;

namespace RSoft.Auth.Cross.IoC
{

    /// <summary>
    /// Dependency injection register
    /// </summary>
    public static class DependencyInjection
    {

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthRegister(this IServiceCollection services, IConfiguration configuration)
        {

            #region General

            // Add Framework services
            services.AddRSoftRegister<AuthContext>(configuration, true);

            #endregion

            #region Options

            services.Configure<SecurityOptions>(options => configuration.GetSection("Application:Security").Bind(options));
            services.Configure<RSApiOptions>(options => configuration.GetSection("Application:RSoftApis").Bind(options));
            services.Configure<CredentialOptions>(options => configuration.GetSection("Application:Credential").Bind(options));
            services.Configure<CultureOptions>(options => configuration.GetSection("Application:Culture").Bind(options));
            services.Configure<PagesOptions>(options => configuration.GetSection("Application:Pages").Bind(options));

            #endregion

            #region Infra

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IUserCredentialTokenRepository, UserCredentialTokenRepository>();
            services.AddScoped<IScopeRepository, ScopeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            #endregion

            #region Domain

            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IScopeDomainService, ScopeDomainService>();
            services.AddScoped<IRoleDomainService, RoleDomainService>();

            #endregion

            #region Application

            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<ICredentialAppService, CredentialAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IScopeAppService, ScopeAppService>();

            #endregion

            return services;

        }

    }

}
