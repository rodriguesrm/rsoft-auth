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
using RSoft.Auth.Web.Api.Model.Request;
using RSoft.Auth.Web.Api.Model.Response;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Cross;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Model.Response;
using RSoft.Framework.Web.Token.Options;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers
{

    /// <summary>
    /// API Authetication
    /// </summary>
    [Route("api/[controller]")]
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

            IList<Claim> claimnsUsuario = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name.FirstName),
                new Claim(ClaimTypes.Surname, user.Name.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, login)
            };

            foreach (RoleDto p in user.Roles)
            {
                claimnsUsuario.Add(new Claim(ClaimTypes.Role, p.Name));
            }

            JwtSecurityToken jwt = new JwtSecurityToken
            (
                 issuer: _jwtTokenOptions.Issuer,
                 audience: _jwtTokenOptions.Audience,
                 claims: claimnsUsuario,
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

            AuthenticateResult<UserDto> authResult = await _appService.AuthenticateAsync(request.Login, request.Password, cancellationToken);
            if (authResult.Success)
            {
                UserResponse userDetail = null;
                IEnumerable<string> roles = null;
                if (details)
                {
                    userDetail = new UserResponse(authResult.User.Id)
                    {
                        Name = authResult.User.Name,
                        Email = authResult.User.Email
                    };
                    roles = authResult.User.Roles.Select(r => r.Name);
                }

                string token = GenerateToken(authResult.User, request.Login, out DateTime? expiresIn);

                AuthenticateResponse result = new AuthenticateResponse(token, expiresIn, roles, userDetail);
                return Ok(result);
            }

            return Unauthorized(authResult.ErrorsMessage);
        }

        /// <summary>
        /// Request credentials for first access
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> FirstAccessAsync(PasswordRequest request, CancellationToken cancellationToken = default)
        {
            PasswordProcessResult result = await _appService.FirstAccessAsync(request.Login, cancellationToken);

            if (result.Success)
                return Ok("First access information sent to the user's email");

            if (result.IsException)
                return HandleException(500, result.Exception);

            return BadRequest(new GenericNotificationResponse("FirstAccess", result.ErrorsMessage));
        }

        /// <summary>
        /// Create the user credential (first access)
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> CreateCredentialAsync(CreateCredentialRequest request, CancellationToken cancellationToken = default)
        {
            SimpleOperationResult result = await _appService.CreateCredentialAsync(request.TokenId.Value, request.Password, true, cancellationToken);
            if (result.Success)
                return NoContent();
            else
                return BadRequest(new GenericNotificationResponse("CreateCredential", result.ErrorsMessage));
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
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request, [FromQuery] bool details, CancellationToken cancellationToken = default)
            => await RunActionAsync(AuthenticateAsync(request, details, cancellationToken), cancellationToken);

        /// <summary>
        /// Request credentials for first access
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("first-access")]
        [AllowAnonymous]
        public async Task<IActionResult> FirstAccess([FromBody] PasswordRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(FirstAccessAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Create the user credential (first access)
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="201">Credentials created successfully</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericNotificationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-credential")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCredential(CreateCredentialRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(CreateCredentialAsync(request, cancellationToken), cancellationToken);

        #endregion

    }
}
