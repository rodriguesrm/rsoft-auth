using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Framework.Cross;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Scope domain service operations
    /// </summary>
    public class ScopeDomainService : DomainServiceBase<Scope, Guid, IScopeRepository>, IScopeDomainService
    {

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        /// <param name="repository">Scope repository</param>
        /// <param name="authenticatedUser">Authenticated user</param>
        public ScopeDomainService(IScopeRepository repository, IAuthenticatedUser authenticatedUser) : base(repository, authenticatedUser)
        {
        }

        #endregion

    }

}
