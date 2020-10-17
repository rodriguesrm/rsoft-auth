using RSoft.Framework.Application.Model;
using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Role response model
    /// </summary>
    public class RoleResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new reponse instance
        /// </summary>
        /// <param name="id">Role id</param>
        public RoleResponse(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Role id key value
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Scope data information
        /// </summary>
        public SimpleIdentification<Guid> Scope { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        /// <example>finances</example>
        public string Name { get; set; }

        /// <summary>
        /// Role description
        /// </summary>
        /// <example>Has access to financial operations</example>
        public string Description { get; set; }

        /// <summary>
        /// Role active status
        /// </summary>
        public bool IsActive { get; set; } = true;

        #endregion

    }
}
