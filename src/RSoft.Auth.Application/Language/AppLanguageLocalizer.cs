using MassTransit.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using RSoft.Lib.Common.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Language
{

    /// <summary>
    /// Application language localizer object
    /// </summary>
    public class AppLanguageLocalizer : IAppLanguageLocalizer
    {

        #region Local objects/variables

        private readonly CultureOptions _cultureOptions;
        private readonly IDictionary<string, string> _languageResource = new Dictionary<string, string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new object instance
        /// </summary>
        /// <param name="optionsCulture">IOPtions parameters to culture configuration</param>
        public AppLanguageLocalizer(IOptions<CultureOptions> optionsCulture)
        {
            _cultureOptions = optionsCulture?.Value;
            LoadLanguageResource(_cultureOptions.DefaultLanguage);
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Load language resource from selected language file
        /// </summary>
        /// <param name="language">Language to load</param>
        private void LoadLanguageResource(string language)
        {

            string file = Path.Combine(AppContext.BaseDirectory, "assets", "language", language);
            if (!File.Exists(file))
                throw new InvalidProgramException($"Resource file to {language} not found");
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines)
            {
                string[] keyValue = line.Split('=');
                _languageResource.TryAdd(keyValue[0], keyValue[1]);
            }

        }

        /// <summary>
        /// Gets the string resource with the given name.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <returns>The string resource as a <see cref="LocalizedString"/>.</returns>
        private LocalizedString GetLocalizedString(string name)
        {
            bool resourceNotFound = !_languageResource.TryGetValue(name, out string? value);
            LocalizedString result = new(name, value ?? name, resourceNotFound);
            return result;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public LocalizedString this[string name] => GetLocalizedString(name);

        #endregion

    }
}
