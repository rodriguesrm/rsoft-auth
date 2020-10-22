using System;

namespace RSoft.Auth.Cross.Common.Abstractions
{

    /// <summary>
    /// Static service factory interface
    /// </summary>
    public interface IStaticServiceFactory
    {

        /// <summary>
        /// Create static service provider
        /// </summary>
        /// <param name="provider">Service provider</param>
        void Create(IServiceProvider provider);

    }
}
