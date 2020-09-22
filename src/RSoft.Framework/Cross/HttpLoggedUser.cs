using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RSoft.Framework.Cross
{

    /// <summary>
    /// Logged user object class
    /// </summary>
    public class HttpLoggedUser : IHttpLoggedUser<Guid>
    {

        #region Local objects/variables

        private readonly IHttpContextAccessor _accessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new LoggedUser instance
        /// </summary>
        /// <param name="accessor"></param>
        public HttpLoggedUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        #endregion

        #region Properties

        ///<inheritdoc/>
        public Guid? Id
        {
            get
            {
                string guid =
                    _accessor
                        .HttpContext
                        .User
                        .Claims
                        .Where(x => x.Type == ClaimTypes.Sid)
                        .Select(x => x.Value)
                        .FirstOrDefault();

                if (!Guid.TryParse(guid, out Guid result))
                    return null;
                return result;

            }
        }

        ///<inheritdoc/>
        public string FirstName =>
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type == ClaimTypes.Name)
                .Select(x => x.Value)
                .FirstOrDefault();

        ///<inheritdoc/>
        public string LastName =>
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type == ClaimTypes.Surname)
                .Select(x => x.Value)
                .FirstOrDefault();

        ///<inheritdoc/>
        public string Login => 
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type == ClaimTypes.NameIdentifier)
                .Select(x => x.Value)
                .FirstOrDefault();

        ///<inheritdoc/>
        public string Email =>
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type == ClaimTypes.Email)
                .Select(x => x.Value)
                .FirstOrDefault();

        ///<inheritdoc/>
        public IEnumerable<string> Roles => 
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value);

        #endregion

    }

}
