using RSoft.Lib.Common.Models;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// User rights and permissions data model
    /// </summary>
    public class UserPermissonsResponse
    {

        /// <summary>
        /// Application-Client id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Application-Client name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of user roles for this application-client
        /// </summary>
        public IEnumerable<SimpleIdentification<Guid>> Roles { get; set; }

    }

}
