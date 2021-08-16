using RSoft.Lib.Common.Contracts.Entities;

namespace RSoft.Auth.Web.Api.Model.Request.v1_0
{

    /// <summary>
    /// Full name request model
    /// </summary>
    public class FullNameRequest : IFullName
    {

        #region Properties

        ///<inheritdoc/>
        public string FirstName { get; set; }
        
        ///<inheritdoc/>
        public string LastName { get; set; }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public string GetFullName()
            => $"{FirstName} {LastName} ".Trim();

        #endregion

    }
}
