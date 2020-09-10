using System.Collections.Generic;

namespace RSoft.Framework.Application.Model
{

    /// <summary>
    /// Authenticated result model object
    /// </summary>
    public class AuthenticateResult<TUserDto> : SimpleOperationResult
        where TUserDto : class
    {

        #region Constructors

        /// <summary>
        /// Create a new object instance
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful</param>
        /// <param name="user">Dto da pessoa autenticada ou null se falhou a autenticação</param>
        /// <param name="errors">Dictionary with list of errors/validation reviews</param>
        public AuthenticateResult(bool success, TUserDto user, IDictionary<string, string> errors) : base(success, errors)
        {
            User = user;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Authenticated user data, null if access is denied
        /// </summary>
        public TUserDto User { get; protected set; }

        #endregion
    }

}