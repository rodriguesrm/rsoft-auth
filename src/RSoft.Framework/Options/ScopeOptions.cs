using System;

namespace RSoft.Auth.Cross.Common.Options
{

    /// <summary>
    /// Scope options model configuration
    /// </summary>
    public class ScopeOptions
    {

        /// <summary>
        /// Application scope id-key value
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Application scope access-key value
        /// </summary>
        public Guid Access { get; set; }

    }
}
