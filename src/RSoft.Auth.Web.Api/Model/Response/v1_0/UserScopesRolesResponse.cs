using RSoft.Framework.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// User rights and permissions data model
    /// </summary>
    public class UserScopesRolesResponse
    {

        /// <summary>
        /// Scope id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Scope name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of user roles for this scope
        /// </summary>
        public IEnumerable<SimpleIdentification<Guid>> Roles { get; set; }

    }

}
