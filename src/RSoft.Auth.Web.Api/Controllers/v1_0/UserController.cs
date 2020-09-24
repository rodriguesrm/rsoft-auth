using System;
using System.Collections.Generic;
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

        #region Constructors

        /// <summary>
        /// Create a new UserController instance
        /// </summary>
        public UserController()
        {
        }

        #endregion

        #region Local methods

        protected override async Task<UserDto> AddAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override async Task<UserDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override UserDto Map(UserRequest request)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override UserResponse Map(UserDto dto)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override object PrepareInsertResponse(UserDto dto)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override async Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        protected override async Task<UserDto> UpdateAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion

    }
}
