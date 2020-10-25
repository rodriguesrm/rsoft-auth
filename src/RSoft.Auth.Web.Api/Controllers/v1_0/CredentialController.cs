using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Web.Api.Helpers;
using RSoft.Auth.Web.Api.Language;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Model.Response;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers.v1_0
{

    /// <summary>
    /// API Authetication
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CredentialController : ApiBaseController
    {

        #region Local objects/variables

        private readonly ICredentialAppService _appService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IStringLocalizer<Resource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of CredentialController
        /// </summary>
        /// <param name="appService">Credential application service</param>
        /// <param name="tokenHelper">Token helper object</param>
        /// <param name="localizer">String language localizer</param>
        public CredentialController
        (
            ICredentialAppService appService, 
            ITokenHelper tokenHelper,
            IStringLocalizer<Resource> localizer
        ) : base()
        {
            _appService = appService;
            _tokenHelper = tokenHelper;
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Request credentials for first access
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> GetFirstAccessAsync(string email, CancellationToken cancellationToken = default)
        {
            PasswordProcessResult result = await _appService.GetFirstAccessAsync(email, _tokenHelper.GenerateTokenAplication(AppKey.Value, "RSoft.Auth", out _), cancellationToken);

            if (result.Success)
                return Ok(_localizer["TOKEN_FIRST_ACCESS_MAIL"].Value);

            if (result.IsException)
                return HandleException(500, result.Exception);

            return BadRequest(PrepareNotifications(result.Errors));
        }

        /// <summary>
        /// Create the user credential (first access)
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> CreateFirstAccessAsync(CreateCredentialRequest request, CancellationToken cancellationToken = default)
        {
            SimpleOperationResult result = await _appService.CreateFirstAccessAsync(request.Token.Value, request.Login, request.Password, cancellationToken);
            if (result.Success)
                return NoContent();
            else
                return BadRequest(PrepareNotifications(result.Errors));
        }

        /// <summary>
        /// Request new access credentials
        /// </summary>
        /// <param name="login">User login or e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> GetRecoveryAccessAsync(string login, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result = await _appService.GetResetAccessAsync(login, _tokenHelper.GenerateTokenAplication(AppKey.Value, "RSoft.Auth", out _), cancellationToken);

            if (result.Success)
                return Ok(_localizer["TOKEN_RECOVER_ACCESS_MAIL"].Value);

            if (result.IsException)
                return HandleException(500, result.Exception);

            return BadRequest(PrepareNotifications(result.Errors));

        }

        /// <summary>
        /// Reset user credentials (I forgot my password)
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> SetRecoveryAccessAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            SimpleOperationResult result = await _appService.SetRecoveryAccessAsync(request.Token.Value, request.Password, cancellationToken);
            if (result.Success)
                return NoContent();
            else
                return BadRequest(PrepareNotifications(result.Errors));
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            SimpleOperationResult result = await _appService.ChangePasswordAsync(request.Login, request.CurrentPassword, request.NewPasword, cancellationToken);
            if (result.Success)
                return NoContent();
            else
                return BadRequest(PrepareNotifications(result.Errors));
        }

        /// <summary>
        /// Checks whether the informed login is available
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> LoginIsAvailableAsync(string login, Guid? userId, CancellationToken cancellationToken)
        {
            SimpleOperationResult result = await _appService.LoginIsAvailableAsync(login, userId, cancellationToken);
            if (result.Errors.Count == 0)
                return Ok(result.Success);
            else
                return BadRequest(PrepareNotifications(result.Errors));
        }

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Request credentials for first access
        /// </summary>
        /// <param name="email">User e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Success in processing the request</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("first")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> FirstAccess([FromQuery] string email, CancellationToken cancellationToken)
            => await RunActionAsync(GetFirstAccessAsync(email, cancellationToken), cancellationToken);

        /// <summary>
        /// Create the user credential (first access)
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="201">Credentials created successfully</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("first")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> FirstAccess(CreateCredentialRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(CreateFirstAccessAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Request new access credentials
        /// </summary>
        /// <param name="login">User login or e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Success in processing the request</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("recovery")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoveryAccess([FromQuery] string login, CancellationToken cancellationToken)
            => await RunActionAsync(GetRecoveryAccessAsync(login, cancellationToken), cancellationToken);

        /// <summary>
        /// Reset user credentials (I forgot my password...
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Credentials successfully re-created</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("recovery")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoveryAccess([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(SetRecoveryAccessAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="204">Password changed successfully</response>
        /// <response code="401">Invalid or unreported credentials</response>
        /// <response code="403">The informed credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("password")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(ChangePasswordAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Checks whether the informed login is available
        /// </summary>
        /// <param name="login">User login (required)</param>
        /// <param name="userId">User id (optional)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Process successfully executed, see details of the response body</response>
        /// <response code="401">Invalid or unreported credentials</response>
        /// <response code="403">The informed credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("login-available")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginIsAvailable([FromQuery] string login, [FromQuery] Guid? userId, CancellationToken cancellationToken)
            => await RunActionAsync(LoginIsAvailableAsync(login, userId, cancellationToken), cancellationToken);

        #endregion

    }
}
