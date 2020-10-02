using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Model;
using RSoft.Auth.Application.Model.Extensions;
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

        #endregion

        #region Constructors

        #endregion

        #region Local methods

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override Task<ScopeDto> AddAsync(ScopeDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override Task<IEnumerable<ScopeDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override Task<ScopeDto> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override ScopeDto Map(ScopeRequest request)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override ScopeResponse Map(ScopeDto dto)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override object PrepareInsertResponse(ScopeDto dto)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override Task RemoveAsync(Guid key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        protected override Task<ScopeDto> UpdateAsync(ScopeDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Actions/Endpoints

        #endregion

    }
}
