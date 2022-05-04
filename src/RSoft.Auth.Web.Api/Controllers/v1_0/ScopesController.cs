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
    /// API Scope administration
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Policy = PolicyNames.OnlyThisApplication)]
    public class ScopesController : ApiCrudBaseController<Guid, ScopeDto, ScopeRequest, ScopeResponse>
    {

        #region Local objects/variables

        private readonly IScopeAppService _scopeAppService;
        private readonly IStringLocalizer<Resource> _localizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Scope API instance
        /// </summary>
        /// <param name="scopeAppService">Scope application service</param>
        /// <param name="localizer">String language localizer</param>
        public ScopesController(IScopeAppService scopeAppService, IStringLocalizer<Resource> localizer)
        {
            _scopeAppService = scopeAppService;
            _localizer = localizer;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Perform export scope data
        /// </summary>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunExportScopeAsync(Guid scopeId, CancellationToken cancellationToken = default)
        {
            OperationResult<byte[]> result = await _scopeAppService.ExportScope(scopeId, cancellationToken);
            if (result.Sucess)
                return File(result.Result, "text/csv", $"scope-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.csv");
            else
                return NotFound(result.Message);
        }

        /// <summary>
        /// Perform import scope data
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunImportScopeAsync(CancellationToken cancellationToken)
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

            OperationResult<IEnumerable<RowImportResult>> result = await _scopeAppService.ImportScope(buffer, cancellationToken);

            fileStream.Close();

            return Ok(result.Result);
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override ScopeDto Map(ScopeRequest request)
            => request.Map();

        ///<inheritdoc/>
        protected override ScopeResponse Map(ScopeDto dto)
            => dto.Map();

        ///<inheritdoc/>
        protected override object PrepareInsertResponse(ScopeDto dto)
            => new { dto.Id, dto.AccessKey };

        ///<inheritdoc/>
        protected override async Task<ScopeDto> AddAsync(ScopeDto dto, CancellationToken cancellationToken = default)
            => await _scopeAppService.AddAsync(dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task<IEnumerable<ScopeDto>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _scopeAppService.GetAllAsync(cancellationToken);

        ///<inheritdoc/>
        protected override async Task<ScopeDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
            => await _scopeAppService.GetByKeyAsync(key, cancellationToken);


        ///<inheritdoc/>
        protected override async Task<ScopeDto> SaveUpdateAsync(ScopeDto dto, CancellationToken cancellationToken = default)
            => await _scopeAppService.UpdateAsync(dto.Id, dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
            => await _scopeAppService.DeleteAsync(key, cancellationToken);

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Add a new scope
        /// </summary>
        /// <param name="request">*Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="201">Scope added successfully</response>
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
        public async Task<IActionResult> CreateScope([FromBody] ScopeRequest request, CancellationToken cancellationToken = default)
            => await base.InsertAsync(request, cancellationToken);

        /// <summary>
        /// List all scopes
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns list of scopes</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(IEnumerable<ScopeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllScope(CancellationToken cancellationToken = default)
            => await base.ListAsync(cancellationToken);

        /// <summary>
        /// Get scope by key id
        /// </summary>
        /// <param name="key">Scope id key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns scope detail</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(ScopeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetScopeByKey([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.GetAsync(key, cancellationToken);

        /// <summary>
        /// Update scope
        /// </summary>
        /// <param name="key">Scope id key value</param>
        /// <param name="request">Scope data details</param>
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
        public async Task<IActionResult> UpdateScope([FromRoute] Guid key, ScopeRequest request, CancellationToken cancellationToken = default)
            => await base.UpdateAsync(key, request, cancellationToken);

        /// <summary>
        /// Delete scope
        /// </summary>
        /// <param name="key">Scope id key value</param>
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
        public async Task<IActionResult> DeleteScope([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.DeleteAsync(key, cancellationToken);

        /// <summary>
        /// Export scope data
        /// </summary>
        /// <param name="key">Scope id key</param>
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
            => await RunActionAsync(RunExportScopeAsync(key, cancellationToken), cancellationToken);

        /// <summary>
        /// Import scope data (By form-file data - only 1 file)
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
            => await RunActionAsync(RunImportScopeAsync(cancellationToken), cancellationToken);

        #endregion

    }
}
