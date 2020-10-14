using RSoft.Auth.Application.Model;
using System;

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
        /// <param name="name">Application name</param>
        /// <param name="expiresIn">Date/date expiration token</param>
        string GenerateTokenAplication(string name, out DateTime? expiresIn);

        /// <summary>
        /// Generate access token for authenticated user
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="login">User login</param>
        /// <param name="expiresIn">Date/date expiration token</param>
        string GenerateToken(UserDto user, string login, out DateTime? expiresIn);

        
    }
}