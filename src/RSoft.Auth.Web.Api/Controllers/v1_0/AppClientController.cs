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
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Web.Api.Extensions;
using RSoft.Auth.Web.Api.Language;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Auth.Web.Api.Policies;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Common.Web.Api;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers.v1_0
{

    /// <summary>
    /// API Application Client administration
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = PolicyNames.OnlyThisApplication)]
    public class AppClientController : ApiCrudBaseController<Guid, AppClientDto, AppClientRequest, AppClientResponse>
    {

        #region Local objects/variables

        private readonly IAppClientAppService _appClientAppService;
        private readonly IStringLocalizer<Resource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Application Client API instance
        /// </summary>
        /// <param name="appClientAppService">Application-Client application service</param>
        /// <param name="localizer">String language localizer</param>
        public AppClientController(IAppClientAppService appClientAppService, IStringLocalizer<Resource> localizer)
        {
            _appClientAppService = appClientAppService;
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Perform export application-client data
        /// </summary>
        /// <param name="clientId">Client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunExportAppClientAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            OperationResult<byte[]> result = await _appClientAppService.ExportAppClient(clientId, cancellationToken);
            if (result.Sucess)
                return File(result.Result, "text/csv", $"appclient-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.csv");
            else
                return NotFound(result.Message);
        }

        /// <summary>
        /// Perform import application-client data
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunImportAppClientAsync(CancellationToken cancellationToken)
        {

            IEnumerable<IFormFile> files = null;
            bool isFileSent = false;

            try
            {
                files = Request.Form.Files;
                if (files != null && files.Any())
                    isFileSent = true;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                    isFileSent = false;
                else
                    throw;
            }

            if (!isFileSent)
                return BadRequest(new GenericNotification("File", _localizer["FILE_NOT_SENT"]));

            if (files.Count() > 1)
                return BadRequest(new GenericNotification("File", _localizer["ONLY_ONE_FILE_MUST_BE_SENT"]));

            IFormFile file = files.First();

            if (file.ContentType != "text/csv")
                return BadRequest(new GenericNotification("File", _localizer["INVALID_FORMAT_FILE"]));

            System.IO.Stream fileStream = file.OpenReadStream();
            byte[] buffer = new byte[fileStream.Length];
            await fileStream.ReadAsync(buffer, cancellationToken);

            OperationResult<IEnumerable<RowImportResult>> result = await _appClientAppService.ImportAppClient(buffer, cancellationToken);

            fileStream.Close();

            return Ok(result.Result);
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override AppClientDto Map(AppClientRequest request)
            => request.Map();

        ///<inheritdoc/>
        protected override AppClientResponse Map(AppClientDto dto)
            => dto.Map();

        ///<inheritdoc/>
        protected override object PrepareInsertResponse(AppClientDto dto)
            => new { dto.Id, dto.AccessKey };

        ///<inheritdoc/>
        protected override async Task<AppClientDto> AddAsync(AppClientDto dto, CancellationToken cancellationToken = default)
            => await _appClientAppService.AddAsync(dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task<IEnumerable<AppClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _appClientAppService.GetAllAsync(cancellationToken);

        ///<inheritdoc/>
        protected override async Task<AppClientDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
            => await _appClientAppService.GetByKeyAsync(key, cancellationToken);


        ///<inheritdoc/>
        protected override async Task<AppClientDto> SaveUpdateAsync(AppClientDto dto, CancellationToken cancellationToken = default)
            => await _appClientAppService.UpdateAsync(dto.Id, dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
            => await _appClientAppService.DeleteAsync(key, cancellationToken);

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Add a new application-client
        /// </summary>
        /// <param name="request">*Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="201">Application-Client added successfully</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(GenericInsertResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateAppClient([FromBody] AppClientRequest request, CancellationToken cancellationToken = default)
            => await base.InsertAsync(request, cancellationToken);

        /// <summary>
        /// List all application-client
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns list of application-clients</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(IEnumerable<AppClientResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllAppClients(CancellationToken cancellationToken = default)
            => await base.ListAsync(cancellationToken);

        /// <summary>
        /// Get applicatin-client by key id
        /// </summary>
        /// <param name="key">Client id key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns application-client detail</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(AppClientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAppClientByKey([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.GetAsync(key, cancellationToken);

        /// <summary>
        /// Update Application-Client
        /// </summary>
        /// <param name="key">Client id key value</param>
        /// <param name="request">Application-Client data details</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="204">Successful request processing</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateAppClient([FromRoute] Guid key, AppClientRequest request, CancellationToken cancellationToken = default)
            => await base.UpdateAsync(key, request, cancellationToken);

        /// <summary>
        /// Delete Application-Client
        /// </summary>
        /// <param name="key">Client id key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="204">Successful request processing</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteAppClient([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.DeleteAsync(key, cancellationToken);

        /// <summary>
        /// Export application-client data
        /// </summary>
        /// <param name="key">Client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{key:guid}/export")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ExportUser([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await RunActionAsync(RunExportAppClientAsync(key, cancellationToken), cancellationToken);

        /// <summary>
        /// Import Application-Client data (By form-file data - only 1 file)
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("import")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ImportUser(CancellationToken cancellationToken = default)
            => await RunActionAsync(RunImportAppClientAsync(cancellationToken), cancellationToken);

        #endregion

    }
}
