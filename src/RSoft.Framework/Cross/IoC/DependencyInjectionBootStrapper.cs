using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Framework.Infra.Data;

namespace RSoft.Framework.Cross.IoC
{

    /// <summary>
    /// Dependency injection register
    /// </summary>
    public static class DependencyInjectionBootStrapper
    {

        private const string DEFAULT_CONNECTION_STRING_NAME = "DbServer";

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration object</param>
        public static IServiceCollection AddRSoftRegister<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
            => AddRSoftRegister<TDbContext>(services, configuration, DEFAULT_CONNECTION_STRING_NAME, true);

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration object</param>
        /// <param name="connectionStringName">Connectino string name</param>
        public static IServiceCollection AddRSoftRegister<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
            where TDbContext : DbContext
            => AddRSoftRegister<TDbContext>(services, configuration, connectionStringName, true);

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration object</param>
        /// <param name="userLazyLoading">Flag indicate use lazy loading proxy</param>
        public static IServiceCollection AddRSoftRegister<TDbContext>(this IServiceCollection services, IConfiguration configuration, bool userLazyLoading)
            where TDbContext : DbContext
            => AddRSoftRegister<TDbContext>(services, configuration, DEFAULT_CONNECTION_STRING_NAME, userLazyLoading);

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration object</param>
        /// <param name="connectionStringName">Connectino string name</param>
        /// <param name="userLazyLoading">Flag indicate use lazy loading proxy</param>
        public static IServiceCollection AddRSoftRegister<TDbContext>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, bool userLazyLoading)
            where TDbContext : DbContext
        {

            #region DbContext

            services.AddDbContext<TDbContext>(opt =>
            {
                opt.UseMySql(configuration.GetConnectionString(connectionStringName));
                if (userLazyLoading)
                    opt.UseLazyLoadingProxies();
            });

            #endregion

            #region Infra.Data

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            return services;

        }

    }

}
