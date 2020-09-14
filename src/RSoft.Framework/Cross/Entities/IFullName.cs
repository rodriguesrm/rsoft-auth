namespace RSoft.Framework.Cross.Entities
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
        string FirstName { get; }

        /// <summary>
        /// Last/Family name
        /// </summary>
        string LastName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get full name
        /// </summary>
        string GetFullName();

        #endregion

    }

}
