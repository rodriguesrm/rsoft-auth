using Microsoft.Extensions.Localization;
using System;
using Microsoft.Extensions.DependencyInjection;
using RSoft.Auth.Cross.Common.Language;

namespace RSoft.Auth.Domain.Language
{

    /// <summary>
    /// Domain factory of language string localizer
    /// </summary>
    public class DomainStringLocalizerFactory : ILocalizerFactory
    {

        ///<inheritdoc/>
        public void Create(IServiceProvider provider)
        {
            IStringLocalizer<DomainResource> localizer = provider.GetService<IStringLocalizer<DomainResource>>();
            EntityLanguage.RegisterLocalizer(localizer);
        }
    }

}
