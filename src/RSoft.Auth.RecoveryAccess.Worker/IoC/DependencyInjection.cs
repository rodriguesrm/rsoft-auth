using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using RSoft.Auth.Application.Language;
using RSoft.Auth.Cross.IoC;
using RSoft.Auth.RecoveryAccess.Worker.Consumers;
using RSoft.Lib.Common.Abstractions;
using RSoft.Lib.Common.Web.Extensions;
using RSoft.Lib.Contracts.Events;
using RSoft.Lib.Messaging.Extensions;

namespace RSoft.Auth.RecoveryAccess.Worker.IoC
{

    /// <summary>
    /// Dependency injection register
    /// </summary>
    public static class DependencyInjection
    {

        #region Local methods


        /// <summary>
        /// Add consumers for message bus
        /// </summary>
        /// <param name="config">Bus factory configurator instance</param>
        private static void AddConsumers(this IRabbitMqBusFactoryConfigurator config)
        {

            // Retry policy
            config.UseMessageRetry(retryConfig => retryConfig.Incremental(4, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)));

            // Consumers;
            config.AddEventConsumerEndpoint<UserRequestAccessEvent, UserRequestAccessEventConsumer>($"{nameof(UserRequestAccessEvent)}.AuthService");

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Register dependency injection services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration object</param>
        public static IServiceCollection AddAuthWorkerRegister(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddCultureLanguage(configuration);
            services.AddAuthRegister(configuration, cfg => cfg.AddConsumers());

            services.AddSingleton<IAppLanguageLocalizer, AppLanguageLocalizer>();

            ServiceActivator.Configure(services.BuildServiceProvider());

            return services;

        }

        #endregion

    }

}
