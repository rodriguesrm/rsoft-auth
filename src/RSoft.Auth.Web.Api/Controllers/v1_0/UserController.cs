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
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Auth.Web.Api.Model.Request.v1_0;
using RSoft.Auth.Web.Api.Model.Response.v1_0;
using RSoft.Framework.Application.Model;
using RSoft.Framework.Cross;
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
    public class UserController : ApiCrudBaseController<Guid, UserDto, UserRequest, UserResponse>
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

        ///<inheritdoc/>
        protected override async Task<UserDto> AddAsync(UserDto dto, CancellationToken cancellationToken = default)
            => await _userAppService.AddAsync(dto, cancellationToken);

        ///<inheritdoc/>
        protected override async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override async Task<UserDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override UserDto Map(UserRequest request)
        {
            UserDto dto = null;
            if (request != null)
            {
                dto = new UserDto()
                {
                    Name = request.Name,
                    BornDate = request.BornDate,
                    Email = request.Email,
                    IsActive = request.IsActive,
                    Scopes = request.Scopes.Select(s => new ScopeDto() { Id = s }).ToList(),
                    Roles = request.Roles.Select(r => new RoleDto() { Id = r }).ToList()
                };
            }
            return dto;
        }

        ///<inheritdoc/>
        protected override UserResponse Map(UserDto dto)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override object PrepareInsertResponse(UserDto dto)
            => new GenericInsertResponse(dto.Id);

        ///<inheritdoc/>
        protected override async Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override async Task<UserDto> UpdateAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion

        #region Actions/Endpoints

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="request">Request data information</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        /// <response code="201">User added successfully</response>
        /// <response code="400">Invalid request, see details in response</response>
        /// <response code="401">Credentials is invalid or empty</response>
        /// <response code="403">The informed credential does not have access privileges to this resource</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(GenericInsertResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<GenericNotificationResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GerericExceptionResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public new async Task<IActionResult> InsertAsync([FromBody] UserRequest request, CancellationToken cancellationToken)
            => await base.InsertAsync(request, cancellationToken);

        #endregion

    }
}
