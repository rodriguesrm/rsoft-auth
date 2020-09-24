using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// User response model
    /// </summary>
    public class UserResponse : SimpleUserResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new UserResponse instance
        /// </summary>
        /// <param name="id">User id key value</param>
        public UserResponse(Guid id) : base(id) { }

        #endregion

        #region Properties

        /// <summary>
        /// User active status
        /// </summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
