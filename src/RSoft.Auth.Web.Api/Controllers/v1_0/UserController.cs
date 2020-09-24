using System;
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

    }
}
