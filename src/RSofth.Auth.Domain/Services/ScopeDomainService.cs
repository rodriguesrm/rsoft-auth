using RSoft.Framework.Domain.Services;
using RSofth.Auth.Domain.Entities;
using RSofth.Auth.Domain.Repositories;
using System;

namespace RSofth.Auth.Domain.Services
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
        public ScopeDomainService(IScopeRepository repository) : base(repository)
        {
        }

        #endregion

    }

}
