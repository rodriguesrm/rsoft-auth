﻿namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Scope request model
    /// </summary>
    public class ScopeRequest
    {

        #region Properties

        /// <summary>
        /// Scope name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether the scope can log in as a service/application
        /// </summary>
        public bool? AllowLogin { get; set; } = false;

        /// <summary>
        /// Acope active status
        /// </summary>
        public bool? IsActive { get; set; } = true;

        #endregion

    }
}