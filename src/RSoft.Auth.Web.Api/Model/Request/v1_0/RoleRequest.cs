using System;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Role request model
    /// </summary>
    public class RoleRequest
    {

        #region Properties

        /// <summary>
        /// Application-Client id key value
        /// </summary>
        public Guid? ClientId { get; set; }

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
