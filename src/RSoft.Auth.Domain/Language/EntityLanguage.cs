using Microsoft.Extensions.Localization;

namespace RSoft.Auth.Domain.Language
{

    /// <summary>
    /// Domain-Entities language localizer manager
    /// </summary>
    public static class EntityLanguage
    {

        public static IStringLocalizer Localizer { get; private set; }

        internal static void RegisterLocalizer(IStringLocalizer localizer)
        {
            Localizer = localizer;
        }

    }

}
