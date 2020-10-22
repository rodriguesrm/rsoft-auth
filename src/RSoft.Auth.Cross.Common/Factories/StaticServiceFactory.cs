using RSoft.Auth.Cross.Common.Abstractions;
using RSoft.Auth.Cross.Common.Helpers;
using System;

namespace RSoft.Auth.Cross.Common.Factories
{

    /// <summary>
    /// Static service factory object
    /// </summary>
    public class StaticServiceFactory : IStaticServiceFactory
    {

        ///<inheritdoc/>
        public void Create(IServiceProvider provider)
        {
            ServiceProviderAccessor.RegisterProvider(provider);

        }
    }
}
