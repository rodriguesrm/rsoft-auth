using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Lib.Common.Enums;
using RSoft.Lib.Web.Options;
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
        public string GenerateTokenAplication(Guid clientId, string appClientName, out DateTime? expiresIn)
        {
            UserDto userDto = new()
            {
                Id = clientId,
                Name = new FullNameRequest() { FirstName = appClientName, LastName = "Application-Client" },
                Email = "NONE",
                Type = UserType.Service,
                Roles = new List<RoleDto>() { new RoleDto() { Name = "service" } },
                ApplicationClients = new List<AppClientDto>() { new AppClientDto() { Name = appClientName } }
            };
            return GenerateToken(userDto, appClientName, out expiresIn);
        }

        ///<inheritdoc/>
        public string GenerateToken(UserDto user, string login, out DateTime? expiresIn)
        {

            IList<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, login)
            };

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

            if (user.ApplicationClients != null)
            {
                foreach (AppClientDto appClientDto in user.ApplicationClients)
                {
                    userClaims.Add(new Claim(ClaimTypes.GroupSid, appClientDto.Name));
                }
            }

            JwtSecurityToken jwt = new
            (
                 issuer: _jwtTokenOptions.Issuer,
                 audience: _jwtTokenOptions.Audience,
                 claims: userClaims,
                 notBefore: _jwtTokenOptions.NotBefore.AddMinutes(-2),
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