using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RSoft.Auth.Application.Services;
using RSoft.Framework.Cross;
using RSoft.Framework.Web.Api;
using RSoft.Framework.Web.Options;

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

        #endregion

        #region Actions/Endpoints

        #endregion

    }
}
