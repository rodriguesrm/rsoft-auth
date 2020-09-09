using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Auth.Infra.Data;
using RSoft.Auth.Infra.Data.Repositories;
using RSoft.Framework.Cross.IoC;
using RSofth.Auth.Domain.Repositories;

namespace RSoft.Auth.Cross.IoC
{

    /// <summary>
    /// Dependency injection register
    /// </summary>
    public static class DependencyInjectionBootStrapper
    {

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthRegister(this IServiceCollection services, IConfiguration configuration)
        {

            // Add Framework services
            services.AddRSoftRegister<AuthContext>(configuration, true);

            #region Infra

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IScopeRepository, ScopeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            #endregion

            #region Domain

            #endregion

            #region Application

            #endregion

            return services;

        }

    }

}
