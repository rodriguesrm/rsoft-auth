using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Framework.Cross.Entities;

namespace RSoft.Auth.Web.Api.Model.Response.v1_0
{

    /// <summary>
    /// Full name response model
    /// </summary>
    public class FullNameResponse : FullNameRequest, IFullName
    {

        #region Constructors

        /// <summary>
        /// Create a new response model instance
        /// </summary>
        public FullNameResponse() { }

        /// <summary>
        /// Create a new response model instance
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        public FullNameResponse(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        #endregion

    }
}
