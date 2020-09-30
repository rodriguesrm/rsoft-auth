﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Web.Api.Extensions;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Model.Response;
using RSoft.Logs.Model;

namespace RSoft.Auth.Web.Api.Controllers.v1_0
{

    /// <summary>
    /// API User administration
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = "master, service")]
    public class UserController : ApiCrudBaseController<Guid, UserDto, UserRequest, UserDetailResponse>
    {

        #region Local objects/variables

        private readonly IUserAppService _userAppService;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new UserController instance
        /// </summary>
        /// <param name="userAppService">User application service</param>
        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Map request to dto object
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="request">Request data</param>
        private UserDto MapUpdateToDto(Guid id, UserUpdateRequest request)
        {
            return new UserDto()
            {
                Id = id,
                Name = new FullNameResponse(request.Name.FirstName, request.Name.LastName),
                Email = request.Email,
                BornDate = request.BornDate,
                Type = request.Type,
                IsActive = request.IsActive
            };
        }

        /// <summary>
        /// List all entity
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunUserListAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<UserDto> result = await _userAppService.GetAllAsync(AppKey.Value, cancellationToken);
            IEnumerable<UserListResponse> resp = result.Select(dto => dto.Map<UserListResponse>());
            return Ok(resp);
        }

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="key">User id key value</param>
        /// <param name="request">Update user request data</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        private async Task<IActionResult> RunUpdateUserAsync(Guid key, UserUpdateRequest request, CancellationToken cancellationToken)
        {

            UserDto dto = await GetByIdAsync(key, cancellationToken);
            if (dto == null)
                return NotFound("Data not found");

            dto = MapUpdateToDto(key, request);
            dto = await UpdateAsync(dto, cancellationToken);

            if (dto.Invalid)
            {
                IEnumerable<GenericNotificationResponse> msg = GetNotificationsErrors(dto.Notifications);
                return BadRequest(msg);
            }

            return NoContent();

        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override async Task<UserDto> AddAsync(UserDto dto, CancellationToken cancellationToken = default)
            => await _userAppService.AddAsync(dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _userAppService.GetAllAsync(cancellationToken);

        ///<inheritdoc/>
        protected override async Task<UserDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
            => await _userAppService.GetByKeyAsync(key, cancellationToken);
        //BUG: Add filter to AppKey

        ///<inheritdoc/>
        protected override UserDto Map(UserRequest request)
            => request.Map();

        ///<inheritdoc/>
        protected override UserDetailResponse Map(UserDto dto)
            => dto.Map<UserDetailResponse>();

        ///<inheritdoc/>
        protected override object PrepareInsertResponse(UserDto dto)
            => new GenericInsertResponse(dto.Id);

        ///<inheritdoc/>
        protected override async Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
            => await _userAppService.DeleteAsync(key, cancellationToken);

        ///<inheritdoc/>
        protected override async Task<UserDto> UpdateAsync(UserDto dto, CancellationToken cancellationToken = default)
            => await _userAppService.UpdateAsync(dto.Id, dto, cancellationToken);

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Add a new user
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
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request, CancellationToken cancellationToken)
            => await base.InsertAsync(request, cancellationToken);
        //BUG: RR - Fazer tratamento de scopo de inclusão de registros, somente quem tem acesso ao Authentication pode adicionar scopos, os demais somente do escopo que pertence

        /// <summary>
        /// List all users
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns list of users</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(UserListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllUser(CancellationToken cancellationToken)
            => await RunActionAsync(RunUserListAsync(cancellationToken), cancellationToken);

        /// <summary>
        /// Get user by key id
        /// </summary>
        /// <param name="key">User id key value</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="200">Successful request processing, returns user detail</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The use credential does not have access to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{key:guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserByKey([FromRoute] Guid key, CancellationToken cancellationToken)
            => await base.GetAsync(key, cancellationToken);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="key">User id key value</param>
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
        public async Task<IActionResult> UpdateUser([FromRoute] Guid key, UserUpdateRequest request, CancellationToken cancellationToken)
            => await RunActionAsync(RunUpdateUserAsync(key, request, cancellationToken), cancellationToken);
        //BUG: Update scope restrict

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="key">User id key value</param>
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
        public async Task<IActionResult> DeleteUser([FromRoute] Guid key, CancellationToken cancellationToken)
            => await base.DeleteAsync(key, cancellationToken);
        //BUG: RR - Delete scope restrict

        #endregion

    }
}
