using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
using RSoft.Framework.Web.Options;
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

        protected IHttpLoggedUser<Guid> _user;
        protected ICredentialAppService _appService;
        protected readonly JwtOptions _jwtOptions;

        #endregion

        #region Constructors

        ///<inheritdoc/>
        public AuthController
        (
            IHttpLoggedUser<Guid> user,
            ICredentialAppService appService,
            IOptions<JwtOptions> jwtOptions
        ) : base()
        {
            _user = user;
            _appService = appService;
            _jwtOptions = jwtOptions?.Value;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Autenticar o usuário no sistema e gera a cha de acesso (token)
        /// </summary>
        /// <param name="request">Informações da requisição</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        protected async Task<IActionResult> AuthenticateAsync(AuthenticateRequest request, CancellationToken cancellationToken = default)
        {

            AuthenticateResult<UserDto> authResult = await _appService.AuthenticateAsync(request.Login, request.Password, cancellationToken);
            if (authResult.Success)
            {
                //TODO: GeneratedToken
                string token = "TODO:GeneratedToken"; // GeraToken(authResult.Pessoa, out IList<string> perfis, out DateTime? validade);
                UserResponse userDetail = new UserResponse(authResult.User.Id)
                {
                    Name = authResult.User.Name,
                    Email = authResult.User.Email
                };
                AuthenticateResponse result = new AuthenticateResponse(token, DateTime.Now.AddMinutes(10), new List<string> { "master" }, userDetail);
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
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request, CancellationToken cancellationToken = default)
            => await RunActionAsync(AuthenticateAsync(request, cancellationToken), cancellationToken);

        #endregion

    }
}
