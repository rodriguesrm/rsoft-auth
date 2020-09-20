using RSoft.Framework.Application.Model;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Application.Model
{

    /// <summary>
    /// First access information request action result model
    /// </summary>
    public class PasswordProcessResult : SimpleOperationResult
    {

        #region Constructors

        /// <summary>
        /// Create a new PasswordProcessResult model
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful</param>
        /// <param name="token">Token generated for the operation (create, reset ou forgot password)</param>
        /// <param name="expiresIn">Token expiration date/time</param>
        /// <param name="errors">Errors list occurred in the operation</param>
        /// <param name="exception">Exception thrown</param>
        public PasswordProcessResult(bool success, Guid? token, DateTime? expiresIn, IDictionary<string, string> errors, Exception exception) : base(success, errors)
        {
            Token = token;
            ExpirerIn = expiresIn;
            Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Generated token
        /// </summary>
        public Guid? Token { get; protected set; }

        /// <summary>
        /// Token expiration date/time
        /// </summary>
        public DateTime? ExpirerIn { get; protected set; }

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