namespace RSoft.Framework.Web.Model
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
        /// Property/FIeld name
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Message detail
        /// </summary>
        public string Message { get; set; }

        #endregion

    }

}
