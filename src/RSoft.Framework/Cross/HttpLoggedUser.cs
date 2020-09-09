using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// User id
        /// </summary>
        public Guid? Id
        {
            get
            {
                string guid =
                    _accessor
                        .HttpContext
                        .User
                        .Claims
                        .Where(x => x.Type.ToLower().Contains("hash"))
                        .Select(x => x.Value)
                        .FirstOrDefault();

                if (!Guid.TryParse(guid, out Guid result))
                    return null;
                return result;

            }
        }

        /// <summary>
        /// User name
        /// </summary>
        public string Username => 
            _accessor
                .HttpContext
                .User
                .Identity
                .Name;

        /// <summary>
        /// User login
        /// </summary>
        public string Login => 
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type.ToLower()
                .Contains("surname"))
                .Select(x => x.Value)
                .FirstOrDefault();

        /// <summary>
        /// User roles
        /// </summary>
        public IEnumerable<string> Roles => 
            _accessor
                .HttpContext
                .User
                .Claims
                .Where(x => x.Type.ToLower()
                .Contains("role"))
                .Select(x => x.Value);

        #endregion

    }

}
