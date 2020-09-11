using System;
using RSoft.Framework.Application.Model;

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

        #endregion

    }

}
