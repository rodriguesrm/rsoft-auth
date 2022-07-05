using RSoft.Auth.Application.Model;
using System;
using System.Collections.Generic;

namespace RSoft.Auth.Web.Api.Helpers
{

    /// <summary>
    /// Helpers to jwt token generate interface
    /// </summary>
    public interface ITokenHelper
    {

        /// <summary>
        /// Generate access token for authenticated user
        /// </summary>
        /// <param name="clientId">Application-Client id key</param>
        /// <param name="applicationClientName">Application-Client name</param>
        /// <param name="appClients">Application clients granted list</param>
        (string, DateTime?) GenerateTokenAplication(Guid clientId, string applicationClientName, IEnumerable<string> appClients);

        /// <summary>
        /// Generate access token for authenticated user
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="login">User login</param>
        (string, DateTime?) GenerateToken(UserDto user, string login);

        
    }
}