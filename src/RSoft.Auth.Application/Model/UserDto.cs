using System;
using System.Collections.Generic;
using RSoft.Lib.Common.Contracts.Dtos;
using RSoft.Lib.Common.Contracts.Entities;
using RSoft.Lib.Common.Dtos;
using RSoft.Lib.Common.Enums;

namespace RSoft.Auth.Application.Model
{

    /// <summary>
    /// User data transport object
    /// </summary>
    public class UserDto : AppDtoIdAuditBase<Guid>, IAuditDto<Guid>
    {

        #region Properties

        /// <summary>
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Document number (withou mask)
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public IFullName Name { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User type
        /// </summary>
        public UserType? Type { get; set; }

        #endregion

        #region Related data

        /// <summary>
        /// User credential data
        /// </summary>
        public UserCredentialDto Credential { get; set; }

        /// <summary>
        /// User roles
        /// </summary>
        public ICollection<RoleDto> Roles { get; set; }

        /// <summary>
        /// User Applicatio-Clients
        /// </summary>
        public ICollection<AppClientDto> ApplicationClients { get; set; }

        #endregion

    }

}
