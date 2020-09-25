using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Framework.Cross;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Role domain service operations
    /// </summary>
    public class RoleDomainService : DomainServiceBase<Role, Guid, IRoleRepository>, IRoleDomainService
    {

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        /// <param name="repository">Role repository</param>
        /// <param name="authenticatedUser">Authenticaed user</param>
        public RoleDomainService(IRoleRepository repository, IAuthenticatedUser authenticatedUser) : base(repository, authenticatedUser)
        {
        }

        #endregion

    }

}
