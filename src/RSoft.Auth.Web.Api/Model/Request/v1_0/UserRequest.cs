using RSoft.Framework.Cross.Enums;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// User request model
    /// </summary>
    public class UserRequest : UserUpdateRequest
    {

        #region Properties

        /// <summary>
        /// User roles ids
        /// </summary>
        public IEnumerable<Guid> Roles { get; set; }

        /// <summary>
        /// User scopes ids
        /// </summary>
        public IEnumerable<Guid> Scopes { get; set; }

        #endregion

    }

}
