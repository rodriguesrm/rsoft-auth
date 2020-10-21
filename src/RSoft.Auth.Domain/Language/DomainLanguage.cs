using Microsoft.Extensions.Localization;

namespace RSoft.Auth.Domain.Language
{

    /// <summary>
    /// Domain language localizer
    /// </summary>
    public static class DomainLanguage
    {

        public static IStringLocalizer Localizer { get; private set; }

        internal static void RegisterLocalizer(IStringLocalizer localizer)
        {
            Localizer = localizer;
        }

    }

}
