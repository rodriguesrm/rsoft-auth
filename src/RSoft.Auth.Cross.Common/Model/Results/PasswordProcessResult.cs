using RSoft.Framework.Application.Model;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Cross.Common.Model.Results
{

    /// <summary>
    /// Credential information request action result model
    /// </summary>
    public class PasswordProcessResult : SimpleOperationResult
    {

        #region Constructors

        /// <summary>
        /// Creates a new object instance
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful</param>
        /// <param name="token">Token generated for the operation</param>
        /// <param name="expirationDate">Token expiration date/time</param>
        /// <param name="errors">List of errors occurred in the operation</param>
        /// <param name="exception">Exception thrown</param>
        public PasswordProcessResult(bool success, Guid? token, DateTime? expirationDate, IDictionary<string, string> errors, Exception exception) : base(success, errors)
        {
            Token = token;
            ExpirationDate = expirationDate;
            Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Generated token
        /// </summary>
        public Guid? Token { get; protected set; }

        /// <summary>
        /// Token expiration date
        /// </summary>
        public DateTime? ExpirationDate { get; protected set; }

        /// <summary>
        /// Indicates whether an exception has occurred
        /// </summary>
        public bool IsException { get { return Exception != null; } }

        /// <summary>
        /// Exception thrown
        /// </summary>
        public Exception Exception { get; set; }

        #endregion

    }

}
