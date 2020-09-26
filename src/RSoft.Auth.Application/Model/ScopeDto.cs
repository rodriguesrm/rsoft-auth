using System;
using RSoft.Framework.Application.Dto;

namespace RSoft.Auth.Application.Model
{

    /// <summary>
    /// Scope data transport object
    /// </summary>
    public class ScopeDto : AppDtoIdAuditBase<Guid>, IAuditDto<Guid>
    {

        #region Properties

        /// <summary>
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Entity name value
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Scope prefix to concate in roles
        /// </summary>
        public string Prefix { get; set; }

        #endregion

    }

}
