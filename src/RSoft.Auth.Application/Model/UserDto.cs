﻿using System;
using System.Collections.Generic;
using RSoft.Framework.Application.Model;

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
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        public string Email { get; set; }

        #endregion

        #region Children data

        /// <summary>
        /// User roles
        /// </summary>
        public IEnumerable<RoleDto> Roles { get; set; }

        /// <summary>
        /// User scopes
        /// </summary>
        public IEnumerable<ScopeDto> Scopes { get; set; }

        #endregion

    }

}
