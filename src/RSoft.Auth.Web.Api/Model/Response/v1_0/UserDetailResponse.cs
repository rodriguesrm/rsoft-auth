﻿using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// User response model
    /// </summary>
    public class UserDetailResponse : UserListResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new UserResponse instance
        /// </summary>
        /// <param name="id">User id key value</param>
        public UserDetailResponse(Guid id) : base(id) { }

        #endregion

        #region Properties

        /// <summary>
        /// User credential information
        /// </summary>
        public SimpleUserCredentialResponse Credential { get; set; }

        /// <summary>
        /// User rights and permissions list
        /// </summary>
        public IEnumerable<UserPermissonsResponse> Permissions { get; set; }

        #endregion
    }
}
