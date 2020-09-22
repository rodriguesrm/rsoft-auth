namespace RSoft.Framework.Web.Model.Response
{

    /// <summary>
    /// Generic notification response model
    /// </summary>
    public class GenericNotificationResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new GenericNotificationResponse instance
        /// </summary>
        /// <param name="property">Property/Field name</param>
        /// <param name="message">Message detail</param>
        public GenericNotificationResponse(string property, string message)
        {
            Property = property;
            Message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property/Field name
        /// </summary>
        /// <example>Name</example>
        public string Property { get; set; }

        /// <summary>
        /// Message detail
        /// </summary>
        /// <example>The name field is required</example>
        public string Message { get; set; }

        #endregion

    }

}
