using RSoft.Lib.Common.Models;
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
        /// <param name="errors">List of errors occurred in the operation</param>
        public PasswordProcessResult(IDictionary<string, string> errors) : this(false, null, null, null, null, errors, null) { }

        /// <summary>
        /// Creates a new object instance
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful</param>
        /// <param name="token">Token generated for the operation</param>
        /// <param name="expirationDate">Token expiration date/time</param>
        /// <param name="fullName">User full name</param>
        /// <param name="email">User e-mail address</param>
        /// <param name="errors">List of errors occurred in the operation</param>
        /// <param name="exception">Exception thrown</param>
        public PasswordProcessResult(bool success, Guid? token, DateTime? expirationDate, string fullName, string email, IDictionary<string, string> errors, Exception exception) : base(success, errors)
        {
            Token = token;
            ExpirationDate = expirationDate;
            Exception = exception;
            FullName = fullName;
            Email = email;
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

        /// <summary>
        /// Full user name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// User e-mail address to send recovery message
        /// </summary>
        public string Email { get; set; }

        #endregion

    }

}
