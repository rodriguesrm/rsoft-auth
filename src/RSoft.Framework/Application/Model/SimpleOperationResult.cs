using System.Collections.Generic;

namespace RSoft.Framework.Application.Model
{

    /// <summary>
    /// Simple operation model result
    /// </summary>
    public class SimpleOperationResult
    {

        #region Constructors

        /// <summary>
        /// Create a new object instance
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful</param>
        /// <param name="errors">Dictionary with list of errors/validation reviews</param>
        public SimpleOperationResult(bool success, IDictionary<string, string> errors)
        {
            Success = success;
            if (!success)
                Errors = errors;
            if (Errors == null)
                Errors = new Dictionary<string, string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// List of errors/validation reviews
        /// </summary>
        public IDictionary<string, string> Errors { get; protected set; }

        /// <summary>
        /// Return a concatenated string with an error message
        /// </summary>
        public string ErrorsMessage
        {
            get
            {

                string result = "";
                if (!Success)
                {
                    result = string.Join("|", Errors.Values);
                }
                return result;

            }
        }

        #endregion

    }

}
