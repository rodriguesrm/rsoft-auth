using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Cross;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Model.Response;
using RSoft.Framework.Web.Options;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers.v1_0
{

    /// <summary>
    /// API Authetication
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ApiBaseController
    {

        #region Local objects/variables

        private IAuthenticatedUser _user;
        private ICredentialAppService _appService;
        private readonly JwtTokenConfig _jwtTokenOptions;

        #endregion

        #region Constructors

        ///<inheritdoc/>
        public AuthController
        (
            IAuthenticatedUser user,
            ICredentialAppService appService,
            IOptions<JwtTokenConfig> jwtTokenOptions
        ) : base()
        {
            _user = user;
            _appService = appService;
            _jwtTokenOptions = jwtTokenOptions?.Value;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Generate access token for authenticated user
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="login">User login</param>
        /// <param name="expiresIn">Date/date expiration token</param>
        protected string GenerateToken(UserDto user, string login, out DateTime? expiresIn)
        {

            IList<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name.FirstName),
                new Claim(ClaimTypes.Surname, user.Name.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, login)
            };

            foreach (RoleDto role in user.Roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            foreach (ScopeDto scope in user.Scopes)
            {
                userClaims.Add(new Claim(ClaimTypes.GroupSid, scope.Name));
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

        /// <summary>
        /// Authenticate the user in the system and generate the access-key (token)
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="details">Indicates whether to return user details</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> AuthenticateAsync(AuthenticateRequest request, bool details, CancellationToken cancellationToken = default)
        {

            if (!AppKey.HasValue || !AppAccess.HasValue)
                return Unauthorized("Scope not defined or invalid");

            AuthenticateResult<UserDto> authResult = await _appService.AuthenticateAsync(AppKey.Value, AppAccess.Value, request.Login, request.Password, cancellationToken);
            if (authResult.Success)
            {
                SimpleUserResponse userDetail = null;
                IEnumerable<string> roles = null;
                if (details)
                {
                    userDetail = new SimpleUserResponse(authResult.User.Id)
                    {
                        Name = new FullNameResponse(authResult.User.Name.FirstName, authResult.User.Name.LastName),
                        Email = authResult.User.Email
                    };
                    roles = authResult.User.Roles?.Select(r => r.Name);
                }

                string token = GenerateToken(authResult.User, request.Login, out DateTime? expiresIn);

                AuthenticateResponse result = new AuthenticateResponse(token, expiresIn, roles, userDetail);
                return Ok(result);
            }

            return Unauthorized(authResult.ErrorsMessage);
        }

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Authenticate user in the system-application / generate access token
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="details">Indicates whether to return user details (default=false)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Authentication Failed / Access Denied</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request, [FromQuery] bool details, CancellationToken cancellationToken = default)
            => await RunActionAsync(AuthenticateAsync(request, details, cancellationToken), cancellationToken);

        //TODO: Add application authentication (ScopeId + ScopeKey)

        #endregion

    }
}
