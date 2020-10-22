using Microsoft.Extensions.DependencyInjection;
using System;

namespace RSoft.Auth.Cross.Common.Abstractions
{

    /// <summary>
    /// Add static service resolver to use when dependencies injection is not available
    /// </summary>
    public class ServiceActivator
    {
        //TODO: Move to rsoft.framework

        #region Local objects/variables

        private static IServiceProvider _serviceProvider = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Configure ServiceActivator with full serviceProvider
        /// </summary>
        /// <param name="serviceProvider">Service DI provider</param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create a scope where use this ServiceActivator
        /// </summary>
        /// <param name="serviceProvider">Service DI provider</param>
        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        #endregion

    }
}
