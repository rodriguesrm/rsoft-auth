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
        /// <example>Antony</example>
        string FirstName { get; }

        /// <summary>
        /// Last/Family name
        /// </summary>
        /// <example>Edward Stark</example>
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
