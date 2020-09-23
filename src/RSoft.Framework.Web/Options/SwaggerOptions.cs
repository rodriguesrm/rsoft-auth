using System;
using System.Collections.Generic;
using System.Text;

namespace RSoft.Framework.Web.Options
{

    /// <summary>
    /// Swagger options configuration model
    /// </summary>
    public class SwaggerOptions
    {

        #region Properties

        /// <summary>
        /// API title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// API description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// API contact support
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// API contact uri
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Indicates whether Swagger's 'TryOut' should be active
        /// </summary>
        public bool EnableTryOut { get; set; }

        /// <summary>
        /// Indicates whether Jwt Token Authentication should be active
        /// </summary>
        public bool EnableJwtTokenAuthentication { get; set; }

        #endregion

    }

}
