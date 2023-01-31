using Microsoft.Extensions.Localization;

namespace RSoft.Auth.Application.Language
{

    /// <summary>
    /// Application language localizer interface
    /// </summary>
    public interface IAppLanguageLocalizer
    {

        /// <summary>
        /// Gets the string resource with the given name.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <returns>The string resource as a <see cref="LocalizedString"/>.</returns>
        LocalizedString this[string name] { get; }

    }

}