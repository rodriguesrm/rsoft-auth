using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Model;
using RSoft.Framework.Web.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RSoft.Auth.Web.Api.Helpers
{

    /// <summary>
    /// Provides helpers to jwt token generate
    /// </summary>
    public class TokenHelper : ITokenHelper
    {

        #region Local objects/variables

        private readonly JwtTokenConfig _jwtTokenOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of
        /// </summary>
        /// <param name="jwtTokenOptions"></param>
        public TokenHelper(IOptions<JwtTokenConfig> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions?.Value;
        }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public string GenerateTokenAplication(string name, out DateTime? expiresIn)
            => GenerateToken(null, name, out expiresIn);

        ///<inheritdoc/>
        public string GenerateToken(UserDto user, string login, out DateTime? expiresIn)
        {

            IList<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, login)
            };

            if (user != null)
            {
                userClaims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, user.Name.FirstName));
                userClaims.Add(new Claim(ClaimTypes.Surname, user.Name.LastName));
                userClaims.Add(new Claim(ClaimTypes.Email, user.Email));
                userClaims.Add(new Claim(ClaimTypes.UserData, user.Type?.ToString()));

                if (user.Roles != null)
                {
                    foreach (RoleDto role in user.Roles)
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                if (user.Scopes != null)
                {
                    foreach (ScopeDto scope in user.Scopes)
                    {
                        userClaims.Add(new Claim(ClaimTypes.GroupSid, scope.Name));
                    }
                }

            }

            JwtSecurityToken jwt = new JwtSecurityToken
            (
                 issuer: _jwtTokenOptions.Issuer,
                 audience: _jwtTokenOptions.Audience,
                 claims: userClaims,
                 notBefore: _jwtTokenOptions.NotBefore,
                 expires: _jwtTokenOptions.Expiration,
                 signingCredentials: _jwtTokenOptions.Credentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            expiresIn = jwt.ValidTo;

            return token;

        }

        #endregion

    }
}