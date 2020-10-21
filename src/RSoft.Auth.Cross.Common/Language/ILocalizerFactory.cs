using System;

namespace RSoft.Auth.Cross.Common.Language
{

    /// <summary>
    /// String localizer factory interface
    /// </summary>
    public interface ILocalizerFactory
    {

        /// <summary>
        /// Create string localizer
        /// </summary>
        /// <param name="provider">Service provider</param>
        void Create(IServiceProvider provider);

    }
}
