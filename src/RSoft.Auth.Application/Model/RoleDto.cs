using System;
using RSoft.Lib.Common.Contracts.Dtos;
using RSoft.Lib.Common.Dtos;
using RSoft.Lib.Common.Models;

namespace RSoft.Auth.Application.Model
{

    /// <summary>
    /// Role data transport object
    /// </summary>
    public class RoleDto : AppDtoIdAuditBase<Guid>, IAuditDto<Guid>
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
        /// Role description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Application-Client data information
        /// </summary>
        public SimpleIdentification<Guid> AppClient { get; set; }

        #endregion

    }

}
