using RSoft.Lib.Common.Enums;
using System;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// User response model
    /// </summary>
    public class UserListResponse : SimpleUserResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new UserResponse instance
        /// </summary>
        /// <param name="id">User id key value</param>
        public UserListResponse(Guid id) : base(id) { }

        #endregion

        #region Properties

        /// <summary>
        /// User active status
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User document number
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User type
        /// </summary>
        public UserType? Type { get; set; }

        #endregion
    }
}
