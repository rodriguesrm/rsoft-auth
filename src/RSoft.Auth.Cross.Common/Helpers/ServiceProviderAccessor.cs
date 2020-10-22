using Castle.Core;
using System;

namespace RSoft.Auth.Cross.Common.Helpers
{

    /// <summary>
    /// Service provider accessor static class
    /// </summary>
    public static class ServiceProviderAccessor
    {

        #region Properties

        public static IServiceProvider Provider { get; private set; }

        public static IServiceProviderEx ProviderEx {get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Register provider in this accessor
        /// </summary>
        /// <param name="provider"></param>
        internal static void RegisterProvider(IServiceProvider provider)
        {
            Provider = provider;
        }

        internal static void RegisterExProvider(IServiceProviderExAccessor serviceProviderExAccessor)
        {
            ProviderEx = serviceProviderExAccessor.ServiceProvider;
        }

        #endregion

    }
}
