using RSoft.Lib.Common.Web.Models.Response;
using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Scope response model
    /// </summary>
    public class ScopeResponse : EntityIdBaseResponse<Guid>
    {

        #region Constructors

        /// <summary>
        /// Create a new ScopeResponse instance
        /// </summary>
        /// <param name="id">Scope id key value</param>
        public ScopeResponse(Guid id) : base(id)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Scope name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Scope access-key application
        /// </summary>
        public Guid AccessKey { get; set; }

        /// <summary>
        /// Indicates whether the scope can log in as a service/application
        /// </summary>
        public bool AllowLogin { get; set; }

        /// <summary>
        /// Acope active status
        /// </summary>
        public bool? IsActive { get; set; } = true;

        #endregion

    }
}
