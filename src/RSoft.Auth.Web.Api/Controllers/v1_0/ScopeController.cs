using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Cross.Common.Options;
using RSoft.Auth.Web.Api.Extensions;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Model.Response;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers.v1_0
{

    /// <summary>
    /// API Scope administration
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, service")]
    public class ScopeController : ApiCrudBaseController<Guid, ScopeDto, ScopeRequest, ScopeResponse>
    {

        #region Local objects/variables

        private readonly IScopeAppService _scopeAppService;
        private readonly ScopeOptions _scopeOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Scope API instance
        /// </summary>
        public ScopeController(IScopeAppService scopeAppService, IOptions<ScopeOptions> options)
        {
            _scopeAppService = scopeAppService;
            _scopeOptions = options?.Value;
        }

        #endregion

        #region Local methods

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
        /// <response code="201">User added successfully</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(GenericInsertResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateUser([FromBody] ScopeRequest request, CancellationToken cancellationToken = default)
            => await base.InsertAsync(request, cancellationToken);

        /// <summary>
        /// List all scopes
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns list of scopes</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(UserListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllUser(CancellationToken cancellationToken = default)
            => await base.ListAsync(cancellationToken);

        /// <summary>
        /// Get scope by key id
        /// </summary>
        /// <param name="key">User id key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns scope detail</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserByKey([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.GetAsync(key, cancellationToken);

        /// <summary>
        /// Update scope
        /// </summary>
        /// <param name="key">Scope id key value</param>
        /// <param name="request">User data details</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="204">Successful request processing</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid key, ScopeRequest request, CancellationToken cancellationToken = default)
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
        public async Task<IActionResult> DeleteUser([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.DeleteAsync(key, cancellationToken);

        #endregion

    }
}
