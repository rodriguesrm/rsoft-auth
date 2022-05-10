using System;
using RSoft.Lib.Common.Contracts.Dtos;
using RSoft.Lib.Common.Dtos;

namespace RSoft.Auth.Application.Model
{

    /// <summary>
    /// Application-Client data transport object
    /// </summary>
    public class AppClientDto : AppDtoIdAuditBase<Guid>, IAuditDto<Guid>
    {

        #region Properties

        /// <summary>
        /// Application access key
        /// </summary>
        public Guid AccessKey { get; set; }

        /// <summary>
        /// Indicates whether the application-client can log in as a service/application
        /// </summary>
        public bool AllowLogin { get; set; }

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
