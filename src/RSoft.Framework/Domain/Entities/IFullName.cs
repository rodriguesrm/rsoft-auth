namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Full name data interface
    /// </summary>
    public interface IFullName
    {

        #region Properties

        /// <summary>
        /// First name
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Last/Family name
        /// </summary>
        string LastName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get full name
        /// </summary>
        string GetFullName();

        #endregion

    }

}
