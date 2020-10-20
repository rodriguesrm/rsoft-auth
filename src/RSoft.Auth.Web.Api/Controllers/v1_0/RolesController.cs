using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Web.Api.Extensions;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Auth.Web.Api.Policies;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Model.Response;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers.v1_0
{

    /// <summary>
    /// API Role administration
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/v{version:apiVersion}/{culture:culture}/[controller]")]
    [ApiController]
    [Authorize(Policy = PolicyNames.OnlyThisApplication)]
    public class RolesController : ApiCrudBaseController<Guid, RoleDto, RoleRequest, RoleResponse>
    {

        #region Local objects/variables

        private readonly IRoleAppService _roleAppService;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Role API instance
        /// </summary>
        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// List all roles
        /// </summary>
        /// <param name="scopeId">Scope id to filter</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunGetAllAsync(Guid? scopeId, CancellationToken cancellationToken)
        {

            IEnumerable<RoleResponse> result = 
                (await _roleAppService.GetAllAsync(cancellationToken))
                .Select(r => r.Map());

            if ((scopeId ?? Guid.Empty) != Guid.Empty)
                result = result.Where(r => r.Scope.Id == scopeId);

            result = result
                .OrderBy(o => o.Scope.Name)
                .ThenBy(o => o.Name)
                .ToList();

            return Ok(result);

        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override RoleDto Map(RoleRequest request)
            => request.Map();

        ///<inheritdoc/>
        protected override RoleResponse Map(RoleDto dto)
            => dto.Map();

        ///<inheritdoc/>
        protected override object PrepareInsertResponse(RoleDto dto)
            => dto.Id;

        ///<inheritdoc/>
        protected override async Task<RoleDto> AddAsync(RoleDto dto, CancellationToken cancellationToken = default)
            => await _roleAppService.AddAsync(dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _roleAppService.GetAllAsync(cancellationToken);

        ///<inheritdoc/>
        protected override async Task<RoleDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
            => await _roleAppService.GetByKeyAsync(key, cancellationToken);


        ///<inheritdoc/>
        protected override async Task<RoleDto> SaveUpdateAsync(RoleDto dto, CancellationToken cancellationToken = default)
            => await _roleAppService.UpdateAsync(dto.Id, dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
            => await _roleAppService.DeleteAsync(key, cancellationToken);

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Add a new role
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="201">Role added successfully</response>
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
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest request, CancellationToken cancellationToken = default)
            => await base.InsertAsync(request, cancellationToken);
            //=> await RunActionAsync(RunInsertAsync(request, cancellationToken), true, true, cancellationToken);

        /// <summary>
        /// List all roles
        /// </summary>
        /// <param name="scope">Scope id to filter</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns list of roles</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(IEnumerable<RoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllRole([FromQuery] Guid? scope, CancellationToken cancellationToken = default)
            => await RunActionAsync(RunGetAllAsync(scope, cancellationToken), cancellationToken);

        /// <summary>
        /// Get role by key id
        /// </summary>
        /// <param name="key">Role id key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns role detail</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRoleByKey([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.GetAsync(key, cancellationToken);

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="key">Role id key value</param>
        /// <param name="request">Role data details</param>
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
        public async Task<IActionResult> UpdateRole([FromRoute] Guid key, RoleRequest request, CancellationToken cancellationToken = default)
            => await base.UpdateAsync(key, request, cancellationToken);

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="key">Role id key value</param>
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
        public async Task<IActionResult> DeleteRole([FromRoute] Guid key, CancellationToken cancellationToken = default)
            => await base.DeleteAsync(key, cancellationToken);

        #endregion

    }
}
