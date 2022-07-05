using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Web.Api.Helpers;
using RSoft.Auth.Web.Api.Language;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Common.Web.Api;
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

        private readonly ICredentialAppService _appService;
        private readonly IAppClientAppService _appClientAppService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IStringLocalizer<Resource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of AUthController API
        /// </summary>
        /// <param name="appService">Credential application service</param>
        /// <param name="appClientAppService">Application-Client application service</param>
        /// <param name="tokenHelper">Token helper</param>
        /// <param name="localizer">String language localizer</param>
        public AuthController
        (
            ICredentialAppService appService,
            IAppClientAppService appClientAppService,
            ITokenHelper tokenHelper,
            IStringLocalizer<Resource> localizer
        ) : base()
        {
            _appService = appService;
            _appClientAppService = appClientAppService;
            _tokenHelper = tokenHelper;
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Authenticate application in the system and generate the access-key (token)
        /// </summary>
        /// <param name="appKey">Client id</param>
        /// <param name="appAccess">Client secret</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> AuthenticateApplicationAsync(Guid appKey, Guid appAccess, CancellationToken cancellationToken)
        {

            if (appKey == Guid.Empty || appAccess == Guid.Empty)
            {
                return BadRequest(new List<GenericNotification>()
                {
                    new GenericNotification("Credentials", _localizer["APPCLIENT_CREDENTIALS_INVALID"])
                });
            }

            (AppClientDto applicationClient, IEnumerable<string> appClients) = await _appClientAppService.GetByCredentialsAsync(appKey, appAccess, cancellationToken);
            if (applicationClient == null)
                return Unauthorized(_localizer["INVALID_APP_KEY_ACCESS"].Value);

            if (!applicationClient.AllowLogin || !applicationClient.IsActive)
                return Unauthorized(_localizer["APP_LOGIN_DISALLOW"].Value);

            (string token, DateTime ? expiresIn) = _tokenHelper.GenerateTokenAplication(applicationClient.Id, applicationClient.Name, appClients);
            AuthenticateResponse result = new(token, expiresIn, null, null);
            return Ok(result);

        }

        /// <summary>
        /// Authenticate the user in the system and generate the access-key (token)
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="details">Indicates whether to return user details</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> AuthenticateAsync(AuthenticateRequest request, bool details, CancellationToken cancellationToken = default)
        {

            if (!AppKey.HasValue)
                return Unauthorized(_localizer["APPCLIENT_NOT_DEFINED"].Value);

            AuthenticateResult<UserDto> authResult = await _appService.AuthenticateAsync(AppKey.Value, request.Login, request.Password, cancellationToken);
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

                (string token, DateTime ? expiresIn) = _tokenHelper.GenerateToken(authResult.User, request.Login);

                AuthenticateResponse result = new(token, expiresIn, roles, userDetail);
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
        /// <response code="401">Invalid credentials, access unauthorized, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request, [FromQuery] bool details, CancellationToken cancellationToken = default)
            => await RunActionAsync(AuthenticateAsync(request, details, cancellationToken), cancellationToken);


        /// <summary>
        /// Authenticate application in the system-application / generate access token
        /// </summary>
        /// <param name="appKey">Client id</param>
        /// <param name="appAccess">Client secret</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Invalid credentials, access unauthorized, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("app")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateApplication
        (
            [FromForm(Name = "app-key")] Guid appKey, 
            [FromForm(Name = "app-access")] Guid appAccess,
            CancellationToken cancellationToken = default
        )
            => await RunActionAsync(AuthenticateApplicationAsync(appKey, appAccess, cancellationToken), cancellationToken);

        #endregion

    }
}
