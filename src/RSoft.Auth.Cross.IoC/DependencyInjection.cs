using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Framework.Cross.IoC;
using RSoft.Auth.Infra.Data;
using RSoft.Auth.Infra.Data.Repositories;
using RSoft.Auth.Domain.Repositories;
using RSoft.Auth.Domain.Services;
using RSoft.Auth.Application.Services;
using Microsoft.EntityFrameworkCore;
using RSoft.Framework.Infra.Data;

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

            #region DbContext

            services.AddDbContext<AuthContext>(opt =>
            {
                opt
                    .UseMySql(configuration.GetConnectionString("DbServer"))
                    .UseLazyLoadingProxies()
                ;
            }, ServiceLifetime.Scoped);

            #endregion

            // Add Framework services
            services.AddRSoftRegister<AuthContext>(configuration, true);

            #region Infra

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IScopeRepository, ScopeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            #endregion

            #region Domain

            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IScopeDomainService, ScopeDomainService>();
            services.AddScoped<IRoleDomainService, RoleDomainService>();

            #endregion

            #region Application

            services.AddScoped<IRoleAppService, RoleAppService>();

            #endregion

            return services;

        }

    }

}
