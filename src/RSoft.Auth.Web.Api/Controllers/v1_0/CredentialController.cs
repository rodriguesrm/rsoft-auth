using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Cross;
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

        private IAuthenticatedUser _user;
        private ICredentialAppService _appService;

        #endregion

        #region Constructors

        ///<inheritdoc/>
        public CredentialController
        (
            IAuthenticatedUser user,
            ICredentialAppService appService
        ) : base()
        {
            _user = user;
            _appService = appService;
        }

        #endregion

        #region Local methods

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

        /// <summary>
        /// Request new access credentials
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> ResetPasswordAsync(PasswordRequest request, CancellationToken cancellationToken = default)
        {

            PasswordProcessResult result = await _appService.ResetPasswordAsync(request.Login, cancellationToken);

            if (result.Success)
                return Ok("Recover access information sent to the user's email");

            if (result.IsException)
                return HandleException(500, result.Exception);

            return BadRequest(new GenericNotificationResponse("ResetPassword", result.ErrorsMessage));

        }

        /// <summary>
        /// Reset user credentials (I forgot my password)
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> ResetCredentialAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            SimpleOperationResult result = await _appService.CreateCredentialAsync(request.TokenId.Value, request.Password, false, cancellationToken);
            if (result.Success)
                return NoContent();
            else
                return BadRequest(new GenericNotificationResponse("ResetCredential", result.ErrorsMessage));
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            SimpleOperationResult result = await _appService.ChangePasswordAsync(_user.Login, request.Login, request.CurrentPassword, request.NewPasword, cancellationToken);
            if (result.Success)
                return NoContent();
            else
                return BadRequest(new GenericNotificationResponse("ChangePassword", result.ErrorsMessage));
        }

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Request credentials for first access
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Success in processing the request</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("first")]
        [MapToApiVersion("1.0")]
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
        [HttpPut("first")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCredential(CreateCredentialRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(CreateCredentialAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Request new access credentials
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Success in processing the request</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("recovery")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(ResetPasswordAsync(request, cancellationToken), cancellationToken);

        /// <summary>
        /// Reset user credentials (I forgot my password...
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Credentials successfully re-created</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericNotificationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("recovery")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetCredential([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(ResetCredentialAsync(request, cancellationToken), cancellationToken);

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
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("password")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(ChangePasswordAsync(request, cancellationToken), cancellationToken);

        #endregion

    }
}
